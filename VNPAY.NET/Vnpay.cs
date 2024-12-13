using Microsoft.AspNetCore.Http;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace VNPAY.NET
{
    public class Vnpay : IVnpay
    {
        private string _tmnCode;
        private string _hashSecret;
        private string _callbackUrl;
        private string _baseUrl;
        private string _version;
        private string _orderType;

        public void Initialize(string tmnCode,
            string hashSecret,
            string baseUrl,
            string callbackUrl,
            string version = "2.1.0",
            string orderType = "other")
        {
            _tmnCode = tmnCode;
            _hashSecret = hashSecret;
            _callbackUrl = callbackUrl;
            _baseUrl = baseUrl;
            _version = version;
            _orderType = orderType;

            EnsureParametersBeforePayment();
        }

        /// <summary>
        /// Tạo URL thanh toán 
        /// </summary>
        /// <param name="request">Thông tin cần có để tạo yêu cầu</param>
        /// <returns></returns>
        public string GetPaymentUrl(PaymentRequest request)
        {
            EnsureParametersBeforePayment();

            if (request.Money < 5000 || request.Money > 1000000000)
            {
                throw new ArgumentException("Số tiền thanh toán phải nằm trong khoảng 5.000 (VND) đến 1.000.000.000 (VND).");
            }

            if (string.IsNullOrEmpty(request.Description))
            {
                throw new ArgumentException("Không được để trống mô tả giao dịch.");
            }

            if (string.IsNullOrEmpty(request.IpAddress))
            {
                throw new ArgumentException("Không được để trống địa chỉ IP.");
            }

            var helper = new PaymentHelper();
            helper.AddRequestData("vnp_Version", _version);
            helper.AddRequestData("vnp_Command", "pay");
            helper.AddRequestData("vnp_TmnCode", _tmnCode);
            helper.AddRequestData("vnp_Amount", (request.Money * 100).ToString());
            helper.AddRequestData("vnp_CreateDate", request.CreatedDate.ToString("yyyyMMddHHmmss"));
            helper.AddRequestData("vnp_CurrCode", request.Currency.ToString().ToUpper());
            helper.AddRequestData("vnp_IpAddr", request.IpAddress);
            helper.AddRequestData("vnp_Locale", EnumHelper.GetDescription(request.Language));
            helper.AddRequestData("vnp_BankCode", request.BankCode == BankCode.ANY ? string.Empty : request.BankCode.ToString());
            helper.AddRequestData("vnp_OrderInfo", request.Description.Trim());
            helper.AddRequestData("vnp_OrderType", _orderType);
            helper.AddRequestData("vnp_ReturnUrl", _callbackUrl);
            helper.AddRequestData("vnp_TxnRef", request.PaymentId.ToString());

            return helper.GetPaymentUrl(_baseUrl, _hashSecret);
        }

        /// <summary>
        /// Lấy kết quả thanh toán sau khi thực hiện giao dịch và chuyển hướng đến <c>CallbackUrl</c>.
        /// </summary>
        /// <param name="parameters">Các tham số trong chuỗi truy vấn của <c>CallbackUrl</c></param>
        /// <returns></returns>
        public PaymentResult GetPaymentResult(IQueryCollection parameters)
        {
            var responseData = parameters
                .Where(kv => !string.IsNullOrEmpty(kv.Key) && kv.Key.StartsWith("vnp_"))
                .ToDictionary(kv => kv.Key, kv => kv.Value.ToString());

            var paymentId = responseData.GetValueOrDefault("vnp_TxnRef");
            var transactionId = responseData.GetValueOrDefault("vnp_TransactionNo");
            var transactionStatus = responseData.GetValueOrDefault("vnp_TransactionStatus");
            var secureHash = responseData.GetValueOrDefault("vnp_SecureHash");

            if (string.IsNullOrEmpty(paymentId))
            {
                throw new ArgumentException("Không tìm thấy PaymentId trong thông tin thanh toán");
            }

            if (string.IsNullOrEmpty(transactionId))
            {
                throw new ArgumentException("Không tìm thấy TransactionId trong thông tin thanh toán"); 
            }

            if (string.IsNullOrEmpty(transactionStatus))
            {
                throw new ArgumentException("Không tìm thấy TransactionStatus trong thông tin thanh toán");
            }

            if (string.IsNullOrEmpty(secureHash))
            {
                throw new ArgumentException(EnumHelper.GetDescription(TransactionStatusCode.Code_97));
            }

            var helper = new PaymentHelper();
            foreach (var (key, value) in responseData)
            {
                if (!key.Equals("vnp_SecureHash"))
                {
                    helper.AddResponseData(key, value);
                }
            }

            var statusCode = (TransactionStatusCode)sbyte.Parse(transactionStatus);  
            return new PaymentResult
            {
                PaymentId = long.Parse(paymentId),
                TransactionId = long.Parse(transactionId),
                IsSuccess = statusCode == TransactionStatusCode.Code_00 && helper.IsSignatureCorrect(secureHash, _hashSecret),
                TransactionStatusCode = statusCode,
                Description = EnumHelper.GetDescription(statusCode),
            };
        }

        private void EnsureParametersBeforePayment()
        {
            if (string.IsNullOrEmpty(_baseUrl) || string.IsNullOrEmpty(_tmnCode) || string.IsNullOrEmpty(_hashSecret) || string.IsNullOrEmpty(_callbackUrl))
            {
                throw new ArgumentException("Không tìm thấy BaseUrl, TmnCode, HashSecret, hoặc CallbackUrl");
            }
        }
    }
}

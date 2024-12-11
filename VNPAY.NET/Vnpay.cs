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
            string callbackUrl,
            string paymentUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
            string version = "2.1.0",
            string orderType = "other")
        {
            _tmnCode = tmnCode;
            _hashSecret = hashSecret;
            _callbackUrl = callbackUrl;
            _baseUrl = paymentUrl;
            _version = version;
            _orderType = orderType;

            EnsureParametersBeforePayment();
        }

        public string GetPaymentUrl(PaymentRequest request)
        {
            EnsureParametersBeforePayment();

            var helper = new PaymentHelper();
            helper.AddRequestData("vnp_Version", _version);
            helper.AddRequestData("vnp_Command", "pay");
            helper.AddRequestData("vnp_TmnCode", _tmnCode);
            helper.AddRequestData("vnp_Amount", (request.Money * 100).ToString());
            helper.AddRequestData("vnp_CreateDate", request.CreatedDate.ToString("yyyyMMddHHmmss"));
            helper.AddRequestData("vnp_CurrCode", request.Currency.ToString().ToUpper());
            helper.AddRequestData("vnp_IpAddr", request.IpAddress);
            helper.AddRequestData("vnp_Locale", EnumHelper.GetDescription(request.Language).ToLower());
            helper.AddRequestData("vnp_BankCode", request.BankCode == BankCode.ANY ? string.Empty : request.BankCode.ToString());
            helper.AddRequestData("vnp_OrderInfo", request.Description.Trim());
            helper.AddRequestData("vnp_OrderType", _orderType);
            helper.AddRequestData("vnp_ReturnUrl", _callbackUrl);
            helper.AddRequestData("vnp_TxnRef", request.TxnRef.ToString());

            return helper.GetPaymentUrl(_baseUrl, _hashSecret);
        }

        public PaymentResult GetPaymentResult(IQueryCollection collections)
        {
            var helper = new PaymentHelper();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    helper.AddResponseData(key, value.ToString());
                }
            }

            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;

            return new PaymentResult
            {
                PaymentId = Convert.ToInt64(helper.GetResponseValue("vnp_TransactionNo")),
                IsSuccess = helper.IsSignatureValid(vnp_SecureHash, _hashSecret),
                TransactionId = Convert.ToInt64(helper.GetResponseValue("vnp_TransactionNo")),
                Token = vnp_SecureHash,
                VnpayResponseCode = helper.GetResponseValue("vnp_ResponseCode"),
                Description = helper.GetResponseValue("vnp_OrderInfo"),
            };
        }

        private void EnsureParametersBeforePayment()
        {
            if (string.IsNullOrEmpty(_tmnCode) || string.IsNullOrEmpty(_hashSecret) || string.IsNullOrEmpty(_callbackUrl))
            {
                throw new ArgumentNullException("Missing TmnCode, HashSecret, or CallbackUrl");
            }
        }
    }
}

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
        private string _version;
        private string _orderType;

        public void Initialize(string tmnCode, string hashSecret, string callbackUrl, string version = "2.1.0", string orderType = "other")
        {
            _tmnCode = tmnCode;
            _hashSecret = hashSecret;
            _callbackUrl = callbackUrl;
            _version = version;
            _orderType = orderType;

            EnsureParametersBeforePayment();
        }

        public string GetPaymentUrl(PaymentRequest request, bool isTest = true)
        {
            EnsureParametersBeforePayment();

            var vnpay = new PaymentUtils();
            vnpay.AddRequestData("vnp_Version", _version);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _tmnCode);
            vnpay.AddRequestData("vnp_Amount", (request.Money * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", request.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", request.Currency.ToString().ToUpper());
            vnpay.AddRequestData("vnp_IpAddr", request.IpAddress);
            vnpay.AddRequestData("vnp_Locale", EnumHelper.GetDescription(request.Locale).ToLower());
            vnpay.AddRequestData("vnp_BankCode", request.BankCode == BankCode.ANY ? string.Empty : request.BankCode.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", request.Description.Trim());
            vnpay.AddRequestData("vnp_OrderType", _orderType);
            vnpay.AddRequestData("vnp_ReturnUrl", _callbackUrl);
            vnpay.AddRequestData("vnp_TxnRef", request.TxnRef.ToString());

            return vnpay.GetPaymentUrl(isTest ? EnumHelper.GetDescription(PaymentUrl.Sandbox) : EnumHelper.GetDescription(PaymentUrl.Production), _callbackUrl);
        }

        public PaymentResponse PaymentExecute(IQueryCollection collections)
        {
            EnsureParametersBeforePayment();

            var utility = new PaymentUtils();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    utility.AddResponseData(key, value.ToString());
                }
            }

            var vnp_TxnRef = Convert.ToInt64(utility.GetResponseValue("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(utility.GetResponseValue("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = utility.GetResponseValue("vnp_ResponseCode");
            var vnp_OrderInfo = utility.GetResponseValue("vnp_OrderInfo");

            bool checkSignature = utility.IsSignatureValid(vnp_SecureHash, _hashSecret);
            if (!checkSignature)
            {
                return new PaymentResponse
                {
                    IsSuccess = false
                };
            }

            return new PaymentResponse
            {
                IsSuccess = true,
                PaymentMethod = "VNPAY",
                OrderDescription = vnp_OrderInfo,
                TxnRef = vnp_TxnRef.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                ResponseCode = vnp_ResponseCode
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

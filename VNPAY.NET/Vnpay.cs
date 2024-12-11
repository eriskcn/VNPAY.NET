using Microsoft.AspNetCore.Http;
using System;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace VNPAY.NET
{
    public class Vnpay() : IVnpay
    {
        public string Version { get; set; } = "2.1.0";
        public string OrderType { get; set; } = "other";
        public required string TmnCode { get; set; }
        public required string HashSecret { get; set; }
        public required string ReturnUrl { get; set; }
        public string CreatePaymentUrl(PaymentRequest request, bool isTest = true)
        {
            var vnpay = new PaymentUtils();
            vnpay.AddRequestParam("vnp_Version", Version);
            vnpay.AddRequestParam("vnp_Command", EnumHelper.GetDescription(request.Command));
            vnpay.AddRequestParam("vnp_TmnCode", TmnCode);
            vnpay.AddRequestParam("vnp_Amount", (request.Money * 100).ToString());
            vnpay.AddRequestParam("vnp_CreateDate", request.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestParam("vnp_CurrCode", EnumHelper.GetDescription(request.Currency));
            vnpay.AddRequestParam("vnp_IpAddr", request.IpAddress);
            vnpay.AddRequestParam("vnp_Locale", EnumHelper.GetDescription(request.Locale));
            vnpay.AddRequestParam("vnp_OrderInfo", request.Description);
            vnpay.AddRequestParam("vnp_OrderType", OrderType);
            vnpay.AddRequestParam("vnp_ReturnUrl", ReturnUrl);
            vnpay.AddRequestParam("vnp_TxnRef", request.TxnRef.ToString());

            return vnpay.GeneratePaymentUrl(isTest ? EnumHelper.GetDescription(PaymentUrl.Sandbox) : EnumHelper.GetDescription(PaymentUrl.Production), ReturnUrl);
        }

        public PaymentResponse PaymentExecute(IQueryCollection collections)
        {
            var utility = new PaymentUtils();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    utility.AddResponseParam(key, value.ToString());
                }
            }

            var vnp_uid = Convert.ToInt64(utility.GetResponseValue("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(utility.GetResponseValue("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = utility.GetResponseValue("vnp_ResponseCode");
            var vnp_OrderInfo = utility.GetResponseValue("vnp_OrderInfo");

            bool checkSignature = utility.ValidateSignature(vnp_SecureHash, HashSecret);
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
                TxnRef = vnp_uid.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                ResponseCode = vnp_ResponseCode
            };
        }
    }
}

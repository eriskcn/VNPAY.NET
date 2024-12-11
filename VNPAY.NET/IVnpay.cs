using Microsoft.AspNetCore.Http;
using VNPAY.NET.Models;

namespace VNPAY.NET
{
    public interface IVnpay
    {
        public string CreatePaymentUrl(PaymentRequest request, bool isTest = true);
        public PaymentResponse PaymentExecute(IQueryCollection collections);
    }
}

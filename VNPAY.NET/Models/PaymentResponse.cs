namespace VNPAY.NET.Models
{
    public class PaymentResponse
    {
        public bool IsSuccess { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string TxnRef { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string ResponseCode { get; set; }
    }
}

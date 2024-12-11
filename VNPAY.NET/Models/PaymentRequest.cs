using VNPAY.NET.Enums;

namespace VNPAY.NET.Models
{
    public class PaymentRequest
    {
        public required double TxnRef { get; set; }
        public required string Description { get; set; }
        public required double Money { get; set; }
        public string IpAddress { get; set; } = "13.160.92.202";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Currency Currency { get; set; } = Currency.VND;
        public Locale Locale { get; set; } = Locale.Vietnamese;
        public Command Command { get; set; } = Command.Pay;
    }
}

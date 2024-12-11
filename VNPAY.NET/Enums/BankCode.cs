using System.ComponentModel;

namespace VNPAY.NET.Enums
{
    public enum BankCode
    {
        [Description("Mọi phương thức thanh toán")]
        All,

        [Description("Thanh toán quét mã QR")]
        QrCode,

        [Description("Thẻ ATM hoặc tài khoản ngân hàng tại Việt Nam")]
        AtmOrBanking,

        [Description("Thẻ thanh toán quốc tế")]
        InternationalCard
    }
}

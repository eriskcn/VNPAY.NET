using System.ComponentModel;

namespace VNPAY.NET.Enums
{
    /// <summary>
    /// Các URL môi trường thanh toán của VNPAY.  
    /// </summary>
    public enum PaymentUrl
    {
        /// <summary>
        /// URL của môi trường Sandbox (kiểm thử), được sử dụng để kiểm tra và phát triển trước khi đưa ứng dụng vào môi trường thực tế.
        /// </summary>
        [Description("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html")]
        Sandbox,

        /// <summary>
        /// URL của môi trường thực tế, được sử dụng để thực hiện các giao dịch thanh toán thực tế với người dùng.  
        /// </summary>
        [Description("https://vnpayment.vn/paymentv2/vpcpay.html")]
        Production
    }

}

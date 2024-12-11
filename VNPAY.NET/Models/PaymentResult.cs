using VNPAY.NET.Enums;

namespace VNPAY.NET.Models
{
    /// <summary>
    /// Lớp đại diện cho phản hồi từ VNPAY sau khi thực hiện giao dịch thanh toán.
    /// </summary>
    public class PaymentResult
    {
        /// <summary>
        /// Mã tham chiếu giao dịch (Transaction Reference). Đây là mã số duy nhất dùng để xác định giao dịch.
        /// </summary>
        public long PaymentId { get; set; }

        /// <summary>
        /// Trạng thái thành công của giao dịch. Nếu là <c>true</c>, giao dịch thành công; nếu là <c>false</c>, giao dịch thất bại.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Thông tin mô tả nội dung thanh toán viết bằng tiếng Việt, không dấu.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Mã giao dịch ghi nhận trên hệ thống VNPAY.
        /// </summary>
        public long TransactionId { get; set; }

        /// <summary>
        /// Mã kiểm tra (checksum) để đảm bảo dữ liệu của giao dịch không bị thay đổi trong quá trình chuyển từ VNPAY về <c>CallbackUrl</c>. Cần kiểm tra đúng checksum khi bắt đầu xử lý yêu cầu (trước khi thực hiện các yêu cầu khác).
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Mã phản hồi kết quả thanh toán, tham khảo chi tiết tại <see href="https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/">bảng mã lỗi của VNPAY</see>.
        /// </summary>
        public TransactionStatusCode TransactionStatusCode { get; set; }
    }
}

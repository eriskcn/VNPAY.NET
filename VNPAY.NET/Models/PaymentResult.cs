namespace VNPAY.NET.Models
{
    /// <summary>
    /// Lớp đại diện cho phản hồi từ VNPAY sau khi thực hiện giao dịch thanh toán.
    /// </summary>
    public class PaymentResult
    {
        /// <summary>
        /// Mã tham chiếu giao dịch (PaymentId).  
        /// Đây là mã duy nhất xác định giao dịch từ phía hệ thống của bạn.  
        /// Mã này cần được lưu trữ để đối chiếu khi có sự cố hoặc yêu cầu hỗ trợ.
        /// </summary>
        public long PaymentId { get; set; }
        /// <summary>
        /// Trạng thái giao dịch thanh toán (thành công hay thất bại).  
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Mô tả ngắn gọn về giao dịch.  
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Mã giao dịch của hệ thống VNPAY.  
        /// Mã này giúp tra cứu chi tiết giao dịch trong trường hợp cần xác minh hoặc truy vết thông tin giao dịch.
        /// </summary>
        public long TransactionId { get; set; }

        /// <summary>
        /// Token xác thực giao dịch, dùng để xác thực tính hợp lệ của giao dịch và đảm bảo tính bảo mật.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Mã phản hồi từ VNPAY (VnpayResponseCode).  
        /// Mã này giúp xác định kết quả của giao dịch, ví dụ: mã 00 cho giao dịch thành công, mã lỗi cho giao dịch thất bại.  
        /// Để tra cứu các mã lỗi, tham khảo <see href="https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/">bảng mã lỗi của VNPAY</see>.
        /// </summary>
        public string VnpayResponseCode { get; set; }
    }

}

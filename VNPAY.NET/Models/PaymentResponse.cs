namespace VNPAY.NET.Models
{
    /// <summary>
    /// Lớp đại diện cho phản hồi từ VNPAY sau khi thực hiện giao dịch thanh toán.
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// Trạng thái giao dịch thanh toán (thành công hay thất bại).  
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Phương thức thanh toán được sử dụng trong giao dịch (ví dụ: thẻ ATM, thẻ quốc tế, quét mã QR).  
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Mô tả ngắn gọn về giao dịch.  
        /// </summary>
        public string OrderDescription { get; set; }

        /// <summary>
        /// Mã tham chiếu giao dịch (TxnRef).  
        /// Đây là mã duy nhất xác định giao dịch từ phía hệ thống của bạn.  
        /// Mã này cần được lưu trữ để đối chiếu khi có sự cố hoặc yêu cầu hỗ trợ.
        /// </summary>
        public string TxnRef { get; set; }

        /// <summary>
        /// Mã thanh toán (PaymentId) của giao dịch, được cung cấp bởi VNPAY để xác định giao dịch này trong hệ thống của họ.
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Mã giao dịch của hệ thống VNPAY.  
        /// Mã này giúp tra cứu chi tiết giao dịch trong trường hợp cần xác minh hoặc truy vết thông tin giao dịch.
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Token xác thực giao dịch, dùng để xác thực tính hợp lệ của giao dịch và đảm bảo tính bảo mật.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Mã phản hồi từ VNPAY (ResponseCode).  
        /// Mã này giúp xác định kết quả của giao dịch, ví dụ: mã 00 cho giao dịch thành công, mã lỗi cho giao dịch thất bại.  
        /// Để tra cứu các mã lỗi, tham khảo <see href="https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/">bảng mã lỗi của VNPAY</see>.
        /// </summary>
        public string ResponseCode { get; set; }
    }

}

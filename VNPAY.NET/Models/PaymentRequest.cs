using VNPAY.NET.Enums;

namespace VNPAY.NET.Models
{
    /// <summary>
    /// Yêu cầu thanh toán gửi đến cổng thanh toán VNPAY.
    /// </summary>
    public class PaymentRequest
    {
        /// <summary>
        /// Mã tham chiếu giao dịch (Transaction Reference). Đây là mã số duy nhất dùng để xác định giao dịch.  
        /// Lưu ý: Giá trị này bắt buộc và cần đảm bảo không bị trùng lặp giữa các giao dịch.
        /// </summary>
        public required double TxnRef { get; set; }

        /// <summary>
        /// Mô tả giao dịch dùng để giải thích hoặc ghi chú về mục đích của giao dịch. Ví dụ: "Thanh toán đơn hàng số 12345" hoặc "Nạp tiền tài khoản".
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Số tiền giao dịch, được tính theo đơn vị tiền tệ nhỏ nhất.  
        /// Ví dụ: nếu giao dịch sử dụng VND, số tiền sẽ được nhập tính bằng "đồng".  
        /// (100.000 VND = 100000).
        /// </summary>
        public required double Money { get; set; }

        /// <summary>
        /// Phương thức thanh toán. Mặc định là tất cả phương thức thanh toán.
        /// Ví dụ: "VNPAYQR" (Thanh toán quết má QR) hoặc "VNBANK" (Thanh toán tài khoản ngân hàng tại Việt Nam).
        /// </summary>
        public BankCode BankCode { get; set; } = BankCode.ANY;

        /// <summary>
        /// Địa chỉ IP của người dùng thực hiện giao dịch.  
        /// Giá trị mặc định là "127.0.0.1", nhưng nên thay đổi để phản ánh địa chỉ IP thật của người dùng nếu có thể.  
        /// Mục đích: Đảm bảo tính xác thực và bảo mật cho giao dịch.
        /// </summary>
        public string IpAddress { get; set; } = "127.0.0.1";

        /// <summary>
        /// Thời điểm khởi tạo giao dịch. Giá trị mặc định là ngày và giờ hiện tại tại thời điểm yêu cầu được khởi tạo.  
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Loại tiền tệ sử dụng trong giao dịch. Giá trị mặc định là "VND" (Việt Nam đồng).  
        /// Lưu ý: Chỉ hỗ trợ các loại tiền tệ được cổng thanh toán VNPAY chấp nhận.
        /// </summary>
        public Currency Currency { get; set; } = Currency.VND;

        /// <summary>
        /// Ngôn ngữ hiển thị trên giao diện thanh toán của VNPAY, mặc định là tiếng Việt.  
        /// Các giá trị khác có thể bao gồm tiếng Anh (English), tùy thuộc vào yêu cầu người dùng.
        /// </summary>
        public Locale Locale { get; set; } = Locale.Vietnamese;
    }
}

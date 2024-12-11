namespace VNPAY.NET.Enums
{
    using System.ComponentModel;

    public enum TransactionStatusCode
    {
        [Description("Giao dịch thành công")]
        Code_00 = 0,

        [Description("Tổng số tiền hoản trả lớn hơn số tiền gốc")]
        Code_02 = 2,

        [Description("Dữ liệu gửi sang không đúng định dạng")]
        Code_03 = 3,

        [Description("Không cho phép hoàn trả toàn phần sau khi hoàn trả một phần")]
        Code_04 = 4,

        [Description("Người dùng nhập sai mã OTP.")]
        Code_13 = 13,

        [Description("Thẻ hoặc tài khoản của người dùng chưa đăng ký dịch vụ InternetBanking tại ngân hàng")]
        Code_09 = 9,

        [Description("Người dùng xác thực thông tin thẻ hoặc tài khoản không đúng quá 3 lần")]
        Code_10 = 10,

        [Description("Hết hạn chờ thanh toán")]
        Code_11 = 11,

        [Description("Thẻ hoặc tài khoản của người dùng đã bị khóa")]
        Code_12 = 12,

        [Description("Khách hàng hủy giao dịch")]
        Code_24 = 24,

        [Description("Tài khoản của người dùng không đủ số dư để thực hiện giao dịch")]
        Code_51 = 51,

        [Description("Tài khoản của người dùng đã vượt quá hạn mức giao dịch trong ngày")]
        Code_65 = 65,

        [Description("Ngân hàng thanh toán đang bảo trì")]
        Code_75 = 75,

        [Description("Người dùng nhập sai mật khẩu thanh toán quá số lần quy định")]
        Code_79 = 79,

        [Description("Không tìm thấy giao dịch yêu cầu hoàn trả")]
        Code_91 = 91,

        [Description("Số tiền hoàn trả không hợp lệ. Số tiền hoàn trả phải nhỏ hơn hoặc bằng số tiền thanh toán.")]
        Code_93 = 93,

        [Description("Yêu cầu bị trùng lặp trong thời gian giới hạn của API (5 phút)")]
        Code_94 = 94,

        [Description("Giao dịch này không thành công bên VNPAY. VNPAY từ chối xử lý yêu cầu.")]
        Code_95 = 95,

        [Description("Chữ ký không hợp lệ")]
        Code_97 = 97,

        [Description("Hết thời gian chờ của hệ thống")]
        Code_98 = 98,

        [Description("Lỗi không xác định")]
        Code_99 = 99
    }
}

# VNPAY.NET

![NuGet Version](https://img.shields.io/nuget/v/VNPAY.NET) ![NuGet Downloads](https://img.shields.io/nuget/dt/VNPAY.NET)

# Quy trình thực hiện
1. Cài đặt thư viện `VNPAY.NET` vào dự án .NET.
2. Thiết lập dự án để thư viện hoạt động.
3. Tạo URL thanh toán.
4. Cập nhật kết quả thanh toán.

## Cài đặt `VNPAY.NET`
- Cách 1: Tìm và cài đặt thông qua **NuGet Package Manager** nếu bạn sử dụng Visual Studio.
- Cách 2: Cài đặt thông qua môi trường dòng lệnh. Chi tiết tại [**ĐÂY**](https://www.nuget.org/packages/VNPAY.NET).

## Cấu hình tích hợp
> [!NOTE]
> Đăng ký để lấy thông tin tích hợp tại [**ĐÂY**](https://sandbox.vnpayment.vn/devreg/). Hệ thống sẽ gửi thông tin kết nối về email được đăng ký.

| Thông tin    | Mô tả                                                                                                                                                                           |
|--------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| TmnCode      | Mã định danh kết nối được khai báo tại hệ thống của VNPAY. Mã định danh tương ứng với tên miền website, ứng dụng, dịch vụ của merchant kết nối vào VNPAY. Mỗi đơn vị có thể có một hoặc nhiều mã TmnCode kết nối. |
| HashSecret   | Chuỗi bí mật sử dụng để kiểm tra toàn vẹn dữ liệu khi hai hệ thống trao đổi thông tin (checksum).                                                                               |
| BaseUrl      | URL thanh toán. Đối với môi trường Sandbox, URL là `https://sandbox.vnpayment.vn/paymentv2/vpcpay.html`.                                                                      |
| CallbackUrl  | URL truy vấn kết quả giao dịch. URL này được tự động chuyển đến sau khi giao dịch được thực hiện.                                                                              |

## Hướng dẫn sử dụng
### Khởi tạo

- Cách 1 - Thông qua Dependency Injection:
```csharp
using VNPAY.NET;

public class VnpayPayment
{
    private string _tmnCode;
    private string _hashSecret;
    private string _baseUrl;
    private string _callbackUrl;

    private readonly IVnpay _vnpay;

    public VnpayPayment(IVnpay vnpay)
    {
        _vnpay = vnpay;
        _vnpay.Initialize(_tmnCode, _hashSecret, _baseUrl, _callbackUrl);
    }
}
```

- Cách 2 - Khởi tạo đối tượng:
```csharp
using VNPAY.NET;

public class VnpayPayment
{
    private string _tmnCode;
    private string _hashSecret;
    private string _baseUrl;
    private string _callbackUrl;

    private readonly IVnpay _vnpay;

    public VnpayPayment()
    {
        _vnpay = new Vnpay();
        _vnpay.Initialize(_tmnCode, _hashSecret, _baseUrl, _callbackUrl);
    }
}
```
## Hướng dẫn sử dụng
### Tạo URL thanh toán
```csharp
[HttpGet("CreatePaymentUrl")]
public ActionResult<string> CreatePaymentUrl(double moneyToPay, string description)
{
    if (moneyToPay <= 0)
    {
        return BadRequest("Số tiền phải lớn hơn 0.");
    }

    var ipAddress = NetworkHelper.GetIpAddress(HttpContext);

    var request = new PaymentRequest
    {
        PaymentId = DateTime.Now.Ticks,
        Money = moneyToPay,
        Description = description,
        IpAddress = ipAddress
    };

    var paymentUrl = _vnpay.GetPaymentUrl(request);

    return Created(paymentUrl, paymentUrl);
}
```

### Xử lý sau thanh toán
> [!NOTE]
> Đây chính là URL được chuyển hướng sau khi kết thúc thanh toán. Ví dụ: `https://localhost:1234/api/Vnpay/Callback`.
```csharp
[HttpGet("Callback")]
public ActionResult<PaymentResult> CallbackAction()
{
    if (Request.QueryString.HasValue)
    {
        var paymentResult = _vnpay.GetPaymentResult(Request.Query);
        if (paymentResult.IsSuccess)
        {
            // Thực hiện hành động nếu thanh toán thành công. Ví dụ: Cập nhật trạng thái đơn hàng trong cơ sở dữ liệu.
            return Ok(paymentResult);
        }

        // Thực hiện hành động nếu thanh toán thất bại. Ví dụ: Thông báo thanh toán thất bại cho người dùng.
        return BadRequest(paymentResult);
    }

    return NotFound();
}
```

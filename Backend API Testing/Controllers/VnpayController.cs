using Microsoft.AspNetCore.Mvc;
using VNPAY.NET;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace Backend_API_Testing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VnpayController : ControllerBase
    {
        private readonly IVnpay _vnpay;
        private readonly IConfiguration _configuration;

        public VnpayController(IVnpay vnPayservice, IConfiguration configuration)
        {
            _vnpay = vnPayservice;
            _configuration = configuration;

            _vnpay.Initialize(_configuration["Vnpay:TmnCode"], _configuration["Vnpay:HashSecret"], _configuration["Vnpay:CallbackUrl"]);
        }

        [HttpGet("CreatePaymentUrl")]
        public ActionResult<string> CreatePaymentUrl(double moneyToPay, string description)
        {
            if (moneyToPay <= 0)
            {
                return BadRequest("Số tiền phải lớn hơn 0..");
            }

            var ipAddress = NetworkHelper.GetClientIpAddress(HttpContext);

            var request = new PaymentRequest
            {
                TxnRef = DateTime.Now.Ticks,
                Money = moneyToPay,
                Description = description,
                IpAddress = ipAddress
            };

            var paymentUrl = _vnpay.GetPaymentUrl(request);

            return Created(paymentUrl, paymentUrl);
        }

        [HttpGet("Callback")]
        public ActionResult<PaymentResult> Callback()
        {
            if (Request.QueryString.HasValue)
            {
                var paymentResult = _vnpay.GetPaymentResult(Request.Query);
                if (paymentResult.IsSuccess)
                {
                    return Ok(paymentResult);
                }

                return BadRequest(paymentResult);
            }

            return NotFound(null);
        }
    }
}

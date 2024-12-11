using Backend_API_Testing.Helpers;
using Microsoft.AspNetCore.Mvc;
using VNPAY.NET;
using VNPAY.NET.Models;

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
            var ipAddress = HttpHelper.GetIpAddress(HttpContext);

            var request = new PaymentRequest
            {
                TxnRef = DateTime.Now.Microsecond,
                Money = moneyToPay,
                Description = description,
                IpAddress = ipAddress
            };

            var paymentUrl = _vnpay.GetPaymentUrl(request);

            return Created(paymentUrl, paymentUrl);
        }

        [HttpGet("Callback")]
        public IActionResult Callback(double moneyToPay, string description)
        {
            var uniqueNumber = DateTime.Now.Ticks;
            var request = new PaymentRequest
            {
                TxnRef = uniqueNumber,
                Money = moneyToPay,
                Description = description,
            };

            var paymentUrl = _vnpay.GetPaymentUrl(request);

            return Created(paymentUrl, paymentUrl);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNPAY.NET.Enums
{
    public enum PaymentUrl
    {
        [Description("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html")]
        Sandbox,

        [Description("https://vnpayment.vn/paymentv2/vpcpay.html")]
        Production
    }
}

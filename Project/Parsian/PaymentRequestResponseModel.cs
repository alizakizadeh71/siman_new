using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Parsian
{
    public class PaymentRequestResponseModel
    {
        public short? Status { get; set; }
        public string Message { get; set; }
        public long? Token { get; set; }
    }
}
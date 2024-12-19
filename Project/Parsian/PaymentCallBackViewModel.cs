using System.ComponentModel.DataAnnotations;

namespace OPS.Parsian
{
    public class PaymentCallBackViewModel : PaymentCallbackModel
    {
        [Display(Name = "کد پاسخ سرویس تایید تراکنش")]
        [DisplayFormat(NullDisplayText = "NULL")]
        public short? ConfirmResponseStatus { get; set; }
    }
}
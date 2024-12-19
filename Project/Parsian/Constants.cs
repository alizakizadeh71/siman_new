namespace OPS.Parsian
{
    internal sealed class Constants
    {
        public sealed class ParsianPaymentGateway
        {
            public const short Successful = 0;

            public const short OrderIdDuplicated = -112;

            public const short InvalidLoginAccount = -126;

            public const short InvalidCallerIP = -127;

            public const short BatchBillPaymentRequestWasValidForSomeOfItems = -1554;
        }
    }
}
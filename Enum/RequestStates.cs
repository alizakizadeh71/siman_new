namespace Enums
{
    public enum RequestStates : int
    {
        /// <summary>
        /// خطا
        /// </summary>
        Error = -3,

        /// <summary>
        /// تکراری
        /// </summary>
        Duplicate = -2,

        /// <summary>
        /// درخواست ناقص
        /// </summary>
        Incomplete = -1,

        /// <summary>
        /// درخواست اولیه
        /// </summary>
        InitialRequet = 0,

        /// <summary>
        /// دارای دستور پرداخت
        /// </summary>
        PaymentOrder = 1,

        /// <summary>
        /// پرداخت شده
        /// </summary>
        Payment = 2,

        /// <summary>
        /// تایید پرداخت
        /// </summary>
        PaymentConfirmation = 3
    }
}

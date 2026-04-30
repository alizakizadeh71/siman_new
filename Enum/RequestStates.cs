namespace Enums
{
    public enum RequestState
    {
        // ... اگر وضعیت‌های قبلی دارید اینجا باشند ...

        /// <summary>
        /// تایید مالی نشده
        /// </summary>
        PendingFinancialApproval = 1,

        /// <summary>
        /// تایید مالی
        /// </summary>
        FinanciallyApproved = 2,

        /// <summary>
        /// بارگیری شده
        /// </summary>
        Loaded = 3,

        /// <summary>
        /// به مقصد رسیده
        /// </summary>
        ArrivedAtDestination = 4,

        /// <summary>
        /// تحویل داده شده
        /// </summary>
        Delivered = 5
    }
}

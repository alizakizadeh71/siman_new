namespace Enums
{
    public enum RequestState
    {
        /// <summary>
        /// تایید مالی نشده
        /// </summary>
        PendingFinancialApproval = 1,

        /// <summary>
        /// تایید مالی شده
        /// </summary>
        FinanciallyApproved = 2,

        /// <summary>
        /// در انتظار بارگیری
        /// </summary>
        WaitingForLoading = 3,

        /// <summary>
        /// تحویل داده شده
        /// </summary>
        Delivered = 4
    }
}

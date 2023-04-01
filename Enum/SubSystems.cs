namespace Enums
{
    public enum SubSystems : int
    {
        /// <summary>
        /// سامانه دارو درمان - مجوز بهداشتی
        /// </summary>
        Drug_Import = 1001,

        /// <summary>
        /// سامانه دارو درمان - پرداخت فوب
        /// </summary>
        Drug_Fob = 1000,

        /// <summary>
        /// دارو و درمان - ترخیص
        /// </summary>
        Drug_Clearance = 1002,

        /// <summary>
        /// دارو و درمان - ترخیص - تبصره 23
        /// </summary>
        Drug_Clearance23 = 1003,

        /// <summary>
        /// قرنطینه - واردات
        /// </summary>
        Quarantine_Import = 2001,

        /// <summary>
        /// قرنطینه - ترخیص
        /// </summary>
        Quarantine_Clearance = 2002,

        /// <summary>
        /// قرنطینه حمل - داخلی
        /// </summary>
        Quarantine_Internal = 2003,

        /// <summary>
        /// قرنطینه - صادرات
        /// </summary>
        Quarantine_Export = 2004,

        /// <summary>
        /// قرنطینه - ترانزیت
        /// </summary>
        Quarantine_Transit = 2005,

        /// <summary>
        /// پروانه ها
        /// </summary>
        Certificate = 3001,

        /// <summary>
        /// گواهی حق ثبت
        /// </summary>
        Registration = 3002,

        /// <summary>
        ///خدمات دامپزشکی - ماده14
        /// </summary>
        InvoiceKhadamatDampezeshki = 1007,

        /// <summary>
        ///ماده پنج نظارت شرعی
        /// </summary>
        InvoiceSharei = 1008,

        /// <summary>
        /// آزمایشگاه
        /// </summary>
        Lims = 3003,

    }
}

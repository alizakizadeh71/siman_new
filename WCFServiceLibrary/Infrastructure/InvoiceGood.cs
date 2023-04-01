namespace WCFServiceLibrary
{
    /// <summary>
    /// اقلام پیش فاکتور / فاکتور ارسالی جهت پرداخت هزینه
    /// </summary>
    public class InvoiceGood
    {
        /// <summary>
        /// عنوان فارسی کالا
        /// </summary>
        public string FaName { get; set; }

        /// <summary>
        /// عنوان لاتین کالا
        /// </summary>
        public string EnName { get; set; }

        /// <summary>
        /// نوع کالا
        /// </summary>
        public string CommodityType { get; set; }

        /// <summary>
        /// مقدار کل کالا
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// واحد اندازه گیری
        /// </summary>
        public string CommodityUnit { get; set; }

        /// <summary>
        /// مبلغ اصلی : تنها برای فوب و تبصره 23 کاربرد دارد
        /// </summary>
        public decimal BaseAmount { get; set; }

        /// <summary>
        /// مبلغ قابل پرداخت که می تواند هم ارزی و ریالی باشد و براساس نوع ارز در مرحله ثبت به ریال تبدیل می شود 
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// کد HS
        /// </summary>
        public string HSCode { get; set; }

        /// <summary>
        ///  کد IVC
        /// </summary>
        public string IVC { get; set; }

        /// <summary>
        /// تولید مشابه؟
        /// </summary>
        public string SimilarProduction { get; set; }

        /// <summary>
        /// اطلاعات پایه فاکتور
        /// </summary>
        public virtual InvoiceInfo InvoiceInfo { get; set; }
    }
}

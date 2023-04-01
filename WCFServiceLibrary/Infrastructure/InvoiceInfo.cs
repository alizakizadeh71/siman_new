using System.Collections.Generic;

namespace WCFServiceLibrary
{
    /// <summary>
    /// اطلاعات مربوط به فاکتور / پیش فاکتور ارسالی جهت پرداخت هزینه
    /// </summary>
    public class InvoiceInfo
    {
        /// <summary>
        /// نام شرکت
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// شناسه ملی شرکت
        /// </summary>
        public string CompanyNationalCode { get; set; }

        /// <summary>
        /// شماره درخواست ثبت سفارش 
        /// برای ارتباط ترخیص با ثبت سفارش کاربرد دارد
        /// </summary>
        public string ImportRecordNumber { get; set; }

        /// <summary>
        /// شماره درخواست سیستم مبدا- باید یکتا باشد در ازای هر پیش فاکتور
        /// </summary>
        public string RecordNumber { get; set; }

        /// <summary>
        /// تاریخ در خواست در سیستم مبدا
        /// </summary>
        public string RecordDate { get; set; }

        /// <summary>
        /// شماره مجوز نهایی در سیستم مبدا
        /// </summary>
        public string SecNumber { get; set; }

        /// <summary>
        /// تاریخ صدور مجوز نهایی در سیستم مبدا
        /// </summary>
        public string SecDate { get; set; }

        /// <summary>
        /// شماره تماس متقاضی
        /// </summary>
        public string CellPhoneNumber { get; set; }

        /// <summary>
        /// استان
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// کد ارز
        /// </summary>
        public int Currency { get; set; }

        /// <summary>
        /// شماره پیش فاکتور
        /// </summary>
        public string PerformNumber { get; set; }

        /// <summary>
        /// تاریخ پیش فاکتور
        /// </summary>
        public string PerformDate { get; set; }

        /// <summary>
        /// شماره فاکتور در ترخیص
        /// </summary>
        public string FactorNumber { get; set; }

        /// <summary>
        /// تاریخ فاکتور در ترخیص
        /// </summary>
        public string FactorDateDate { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// لیست اقلام فاکتور / پیش فاکتور
        /// </summary>
        public virtual IList<InvoiceGood> InvoiceGoods { get; set; }
    }

}

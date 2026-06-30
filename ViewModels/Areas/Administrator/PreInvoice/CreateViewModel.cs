using System;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Areas.Administrator.PreInvoice
{
    public class CreateViewModel
    {
        [Display(Name = "نام محصول")]
        public Guid ProductName { get; set; }

        [Display(Name = "نوع محصول")]
        public Guid ProductType { get; set; }

        [Display(Name = "نوع بسته بندی")]
        public Guid PackageType { get; set; }

        [Display(Name = "کارخانه")]
        public Guid FactoryName { get; set; }

        [Display(Name = "استان")]
        public Guid? Province { get; set; }

        [Display(Name = "شهر")]
        public Guid? City { get; set; }

        [Display(Name = "تناژ")]
        public decimal Tonnage { get; set; }

        [Display(Name = "موبایل خریدار")]
        public string BuyerMobile { get; set; }

        [Display(Name = "نام خریدار")]
        public string BuyerName { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "مبلغ")]
        public double AmountPaid { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "محل تحویل")]
        public string MahalTahvil { get; set; }

        // ← اضافه شد
        [Display(Name = "باربری")]
        public Guid? CarrierId { get; set; }

        [Display(Name = "نام راننده")]
        public string DriverName { get; set; }

        [Display(Name = "نام خانوادگی راننده")]
        public string DriverLastName { get; set; }

        [Display(Name = "موبایل راننده")]
        [StringLength(11, ErrorMessage = "شماره موبایل باید 11 رقم باشد")]
        public string DriverMobile { get; set; }

        [Display(Name = "پلاک ماشین")]
        public string DriverLicensePlate { get; set; }
    }
}
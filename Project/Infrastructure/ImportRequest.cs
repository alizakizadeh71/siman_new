namespace Infrastructure
{
    public class ImportRequest
    {
        public ImportRequest()
        { }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNationalCode { get; set; }
        public string CommodityType { get; set; }
        public decimal TotalValue { get; set; }
        public string CommodityUnit { get; set; }
        public string RecordNumber { get; set; }
        public string RecordDate { get; set; }
        public string SecNumber { get; set; }
        public string SecDate { get; set; }
        public string CellPhoneNumber { get; set; }
        public long AmountPaid { get; set; }
        public string PerformNumber { get; set; }
        public string PerformDate { get; set; }
    }
}
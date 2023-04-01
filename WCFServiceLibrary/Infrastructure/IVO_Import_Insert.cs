using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WCFServiceLibrary.Infrastructure
{
    [KnownType(typeof(string[]))]
    [DataContract]
    public class IVO_Import_Insert
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public int SubSystemCode { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string CompanyNationalCode { get; set; }

        [DataMember]
        public string CommodityType { get; set; }

        [DataMember]
        public decimal TotalValue { get; set; }

        [DataMember]
        public string CommodityUnit { get; set; }

        [DataMember]
        public string RecordNumber { get; set; }

        [DataMember]
        public string RecordDate { get; set; }

        [DataMember]
        public string ImportRecordNumber { get; set; }

        [DataMember]
        public string SecNumber { get; set; }

        [DataMember]
        public string SecDate { get; set; }

        [DataMember]
        public string CellPhoneNumber { get; set; }

        [DataMember]
        public int  CurrencyCode { get; set; }

        [DataMember]
        public decimal CurrencyValue { get; set; }

        [DataMember]
        public decimal BaseCurrencyValue { get; set; }

        [DataMember]
        public long AmountPaid { get; set; }

        [DataMember]
        public string PerformNumber { get; set; }

        [DataMember]
        public string PerformDate { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public List<string> FileList { get; set; }
        
    }
}
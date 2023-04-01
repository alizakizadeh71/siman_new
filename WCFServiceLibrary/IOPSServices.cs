using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceLibrary
{
    [ServiceContract]
    public interface IOPSServices
    {
        #region IVO_Import
        [OperationContract]
        int IVO_Import_Insert_Base
            (string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode
            , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
            , string SecNumber, string SecDate, string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue, string PerformNumber
            , string PerformDate, decimal BaseCurrencyValue, List<string> FileList, out string message, bool? IsEdit = false);



        [OperationContract]
        int Cert_Payment3
    (string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode,
    string CityCode
    , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
    //, string SecNumber
    //, string SecDate
    , string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue, int SubSystem, int ServiceTariff, string Description
    //, string PerformNumber
    //, string PerformDate
    //, decimal BaseCurrencyValue
    , List<string> FileList
    , out string message);




        [OperationContract]
        int Lims_Payment
        (string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode,
        string CityCode, string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
        , string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue, string Description, List<string> FileList
        , out string message);












        [OperationContract]
        int NewCert_Payment
    (string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode,
    string CityCode
    , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
    //, string SecNumber
    //, string SecDate
    , string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue, int SubSystem
    //, string PerformNumber
    //, string PerformDate
    //, decimal BaseCurrencyValue
    , List<string> FileList
    , out string message);

        [OperationContract]
        int Cert_Payment
    (string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode,
    string CityCode
    , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
    //, string SecNumber
    //, string SecDate
    , string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue
    //, string PerformNumber
    //, string PerformDate
    //, decimal BaseCurrencyValue
    , List<string> FileList
    , out string message);
        [OperationContract]
        int Cert_Payment_State(string UserName, string Password, int InvoiceNumber, out string message);

        [OperationContract]
        int IVO_Import_State_Base(string UserName, string Password, int InvoiceNumber, out string message);

        #endregion

        #region IVO_Clearance
        [OperationContract]
        int IVO_Clearance_New
           (string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode
           , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate, string ImportRecordNumber
           , string SecNumber, string SecDate, string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue,long CustomsValue, string PerformNumber
           , string PerformDate, decimal BaseCurrencyValue, string Description, List<string> FileList, out string message);
        

        [OperationContract]
        int IVO_Clearance_Insert_Base
           (string UserName, string Password, string CompanyName, string CompanyNationalCode, string ProvinceCode
           , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate, string ImportRecordNumber
           , string SecNumber, string SecDate, string CellPhoneNumber, int CurrencyCode, decimal CurrencyValue, string PerformNumber
           , string PerformDate, decimal BaseCurrencyValue, string Description, List<string> FileList, out string message);

        [OperationContract]
        int ClearanceRequestState(string UserName, string Password, int InvoiceNumber, out string message);
        [OperationContract]
        int GetDepositNumber(string UserName, string Password, string RecordNumber, out string message);

        #endregion

        #region IVO_Quarantine
        //[OperationContract]
        //int IVO_Quarantine_Insert
        //    (string UserName, string Password, int SubSystemCode, string CompanyName, string CompanyNationalCode
        //    , string CommodityType, decimal TotalValue, string CommodityUnit, string RecordNumber, string RecordDate
        //    , string CellPhoneNumber, long AmountPaid, int Province, int City, string URLAddress
        //    , List<string> FileList, out string message);

        //[OperationContract]
        //int QuarantineRequestState(string UserName, string Password, int SubSystemCode, int InvoiceNumber, out string message);
        #endregion

        [OperationContract]
        int TestConnection(out string message);

        [OperationContract]
        bool Delete_Request(string UserName, string Password, int SubSystemCode, int InvoiceNumber, out string message);

        [OperationContract]
        bool Delete_Request_ByRecordNumber(
            string UserName, string Password, int SubSystemCode, string RecordNumber, out string message);
    }
}

using System.Collections.Generic;
using System.ServiceModel;

namespace WCFServiceLibrary
{
    [ServiceContract]
    public interface IPaymentService
    {
        /// <summary>
        /// وب سرویس پرداخت هزینه کارشناسی
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="CompanyName"></param>
        /// <param name="CompanyNationalCode"></param>
        /// <param name="ProvinceCode"></param>
        /// <param name="CommodityType"></param>
        /// <param name="TotalValue"></param>
        /// <param name="CommodityUnit"></param>
        /// <param name="RecordNumber"></param>
        /// <param name="RecordDate"></param>
        /// <param name="SecNumber"></param>
        /// <param name="SecDate"></param>
        /// <param name="CellPhoneNumber"></param>
        /// <param name="AmountPaid"></param>
        /// <param name="PerformNumber"></param>
        /// <param name="PerformDate"></param>
        /// <param name="FileList"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [OperationContract]
        int BachelorsFeePayment
            (string UserName, string Password, InvoiceInfo info, List<string> FileList, out int messageCode, out string message);


        /// <summary>
        /// وب سرویس پرداخت هزینه فوب
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="CompanyName"></param>
        /// <param name="CompanyNationalCode"></param>
        /// <param name="ProvinceCode"></param>
        /// <param name="CommodityType"></param>
        /// <param name="TotalValue"></param>
        /// <param name="CommodityUnit"></param>
        /// <param name="RecordNumber"></param>
        /// <param name="RecordDate"></param>
        /// <param name="SecNumber"></param>
        /// <param name="SecDate"></param>
        /// <param name="CellPhoneNumber"></param>
        /// <param name="CurrencyCode"></param>
        /// <param name="CurrencyValue"></param>
        /// <param name="PerformNumber"></param>
        /// <param name="PerformDate"></param>
        /// <param name="BaseCurrencyValue"></param>
        /// <param name="FileList"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [OperationContract]
        int FobPayment
            (string UserName, string Password, InvoiceInfo info, List<string> FileList
            , out int messageCode, out string message);


        /// <summary>
        /// وب سرویس پرداخت هزینه تبصره 23
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="CompanyName"></param>
        /// <param name="CompanyNationalCode"></param>
        /// <param name="ProvinceCode"></param>
        /// <param name="CommodityType"></param>
        /// <param name="TotalValue"></param>
        /// <param name="CommodityUnit"></param>
        /// <param name="RecordNumber"></param>
        /// <param name="RecordDate"></param>
        /// <param name="ImportRecordNumber"></param>
        /// <param name="SecNumber"></param>
        /// <param name="SecDate"></param>
        /// <param name="CellPhoneNumber"></param>
        /// <param name="CurrencyCode"></param>
        /// <param name="CurrencyValue"></param>
        /// <param name="PerformNumber"></param>
        /// <param name="PerformDate"></param>
        /// <param name="BaseCurrencyValue"></param>
        /// <param name="Description"></param>
        /// <param name="FileList"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [OperationContract]
        int NotePayment
           (string UserName, string Password, InvoiceInfo info, List<string> FileList, out int messageCode, out string message);


        /// <summary>
        /// وب سرویس استعلام وضعیت پرداخت
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="InvoiceNumber"></param>
        /// <param name="messageCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [OperationContract]
        int InquiryRequest(string UserName, string Password, int InvoiceNumber, out int messageCode, out string message);


    }
}

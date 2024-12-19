using Models;
using System;
using System.Linq;

namespace Infrastructure
{
    public class cbinasimService
    {
        DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

        private string RecordNumber { get; set; }
        private string Message { get; set; }
        //private string ProviderIdentifier = "100000014";
        //private string ProviderIdentifier = "100000178";
        private string ProviderIdentifier = "100000017";


        public cbinasimService()
        {
        }

        internal bool CreatedFormBody(Request oRequest, long Amount, out string FormBody)
        {
            try
            {
                FormBody = string.Empty;
                FormBody += "<form method=\"post\" action=\"https://ipg.cbinasim.ir/ipgapp/start\">";
                //FormBody += "<form method=\"post\" action=\"https://85.133.186.11:7676/ipgapp/start\">";
                FormBody += "<table style=\"font-family:tahoma,serif; font-size:12px;\"  hidden=\"hidden\">";

                FormBody += "<tr><td>ProviderIdentifier</td><td><input type =\"text\" "
                    + "id=\"ProviderIdentifier\" name=\"ProviderIdentifier\"  value=\""
                    + ProviderIdentifier + "\"/></td></tr>";

                FormBody += "<tr><td>Amount</td><td><input type=\"text\" "
                    + "id=\"Amount\" name=\"Amount\" value=\"" + Amount + "\"/></td></tr>";

                FormBody += "<tr><td>RequestIdentifier</td><td><input type=\"text\" "
                    + "id=\"RequestIdentifier\" name=\"RequestIdentifier\" value=\""
                    + oRequest.InvoiceNumber + "\"/></td></tr>";

                FormBody += "<tr><td>RedirectUrl</td><td><input type=\"text\" "
                    + "id=\"RedirectUrl\" name=\"RedirectUrl\" value=\""
                    + Infrastructure.WebServiceSetting_Sadad.ReturnURL_CBI + "\"/></td></tr>";

                FormBody += "<tr><td>DepositIdentifier</td><td><input type=\"text\" "
                    + "id=\"DepositIdentifier\" name=\"DepositIdentifier\" value=\""
                    + oRequest.DepositNumber + "\"/></td></tr>";

                FormBody += "</table>"
                    + "<button type=\"submit\" class=\"col-md-offset-2 btn btn-success\""
                    + " value='@Resources.OPS.Button.Payment'>" + Resources.OPS.Button.Payment + "</button> "
                    + "</form>"
                    ;

                return true;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void UpdatePaymentResult(int invoiceNumber, string referenceCode, string resultCode, string message)
        {
            try
            {
                var oRequest =
                    oUnitOfWork.RequestRepository.Get()
                    .Where(x => x.IsActived)
                    .Where(x => !x.IsDeleted)
                    .Where(current => current.InvoiceNumber == invoiceNumber)
                    .FirstOrDefault()
                    ;



                #region Insert PaymentMessage
                Models.Message oMessageP = new Models.Message();
                oMessageP.UserId = oRequest.UserId;
                oMessageP.LastState = oRequest.RequestState;
                oMessageP.MessageText = Resources.Message.Request.Message_Paymented;
                oMessageP.NewState = (int)Enums.RequestStates.Payment;
                oMessageP.RequestId = oRequest.Id;
                oUnitOfWork.MessageRepository.Insert(oMessageP);
                #endregion

                #region Update RequestState
                if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Import && oRequest.CurrencyCode == (int)Enums.CurrencyUnits.Rails)
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Import && oRequest.CurrencyCode != (int)Enums.CurrencyUnits.Rails)
                    oRequest.RequestState = (int)Enums.RequestStates.Payment;

                else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance)
                    oRequest.RequestState = (int)Enums.RequestStates.Payment;

                else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Certificate)
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Lims)
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Quarantine_Import)
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Quarantine_Clearance)
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Quarantine_Export)
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Quarantine_Internal)
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Quarantine_Transit)
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                if (oRequest.RequestState > (int)Enums.RequestStates.Payment)
                {
                    #region Insert Payment Confirm Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oRequest.UserId;
                    oMessage.LastState = (int)Enums.RequestStates.Payment;
                    oMessage.MessageText = Resources.Message.Request.Message_PaymentConfirmation;
                    oMessage.NewState = (int)Enums.RequestStates.PaymentConfirmation;
                    oMessage.RequestId = oRequest.Id;
                    oUnitOfWork.MessageRepository.Insert(oMessage);
                    #endregion
                }
                #endregion

                oRequest.Bank_BankReciptNumber = referenceCode;
                oRequest.Bank_AppStatusDescription = "NO_ERROR";
                oRequest.Bank_ShamsiDate = Infrastructure.Utility.Persion(DateTime.Now);


                oUnitOfWork.RequestRepository.Update(oRequest);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
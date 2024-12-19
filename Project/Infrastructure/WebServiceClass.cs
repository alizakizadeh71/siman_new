using System;
using System.Linq;

namespace Infrastructure
{
    public class WebServiceClass
    {
        DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();
        OPS.IVOServices.IVOServicesClient oIVOServices = new OPS.IVOServices.IVOServicesClient();

        private string UserName { get; set; }
        private string RecordNumber { get; set; }
        private string Message { get; set; }
        private string Password { get; set; }

        public WebServiceClass()
        {
            UserName = "webservice";
            Password = "1234512345";
        }

        public string Insert_Incomplete(string recordNumber, string message)
        {
            try
            {
                RecordNumber = recordNumber;
                Message = message;

                var oRequest =
                 oUnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                 .Where(current => current.RecordNumber == RecordNumber)
                 .FirstOrDefault()
                 ;

                string RetValue
                    = oIVOServices
                    .Insert(UserName, Password, oRequest.SubSystem.Code
                    , RecordNumber, (int)Enums.RequestStates.Incomplete, Message);

                return RetValue;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
using System.Linq;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using Models;

namespace DAL
{
    public class RequestRepository : Repository<Models.Request>, IRequestRepository
    {
        public RequestRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.Request> Get(Models.User user)
        {
            try
            {
                IQueryable<Models.Request> retValue;
                // اگر کاربر صدورفاکتور بود بر اساس ای دی در درخواست ها نمایش داده شود
                if (user.Role.Code <= (int)Enums.Roles.ExporterOFInvoice)
                {
                    retValue = Get()
                        .Where(current => current.IsDeleted == false)
                        .Where(current => current.IsActived == true)
                        .Where(current => ((current.UserId == user.Id)||((current.CityId == user.CityId) &&(user.CityId != null))));
                }
                else if (user.Role.Code <= (int)Enums.Roles.ProvinceExpert00)
                {
                    retValue = Get()
                        .Where(current => current.IsDeleted == false)
                        .Where(current => current.IsActived == true)
                        .Where(current => current.ProvinceId == user.ProvinceId);
                }
                else
                    retValue = Get()
                         .Where(current => current.IsDeleted == false)
                         .Where(current => current.IsActived == true);

                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public override void Insert(Models.Request request)
        {
            try
            {
                string ControlCode = string.Empty;
                bool IsFind = true;
                do
                {
                    request = SetControlCode(request);

                    var Find_Conflict_Row =
                        Get()
                        .Where(x => x.VirtualCode == request.VirtualCode)
                        //.Where(x => x.RecordNumber != request.RecordNumber)
                        .FirstOrDefault()
                        ;

                    if (Find_Conflict_Row == null)
                    {
                        DatabaseContext.Requests.Add(request);
                        DatabaseContext.SaveChanges();
                        IsFind = false;
                    }

                } while (IsFind);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Update(Models.Request request)
        {
            try
            {
                string ControlCode = string.Empty;
                bool IsFind = true;
                do
                {
                    request = SetControlCode(request);

                    var Find_Conflict_Row =
                        Get()
                        .Where(x => x.VirtualCode == request.VirtualCode)
                        .FirstOrDefault()
                        ;

                    if (Find_Conflict_Row == null)
                    {
                        DatabaseContext.Entry(request).State = System.Data.Entity.EntityState.Modified;
                        DatabaseContext.SaveChanges();
                        IsFind = false;
                    }

                } while (IsFind);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public string GetVirtualCode()
        {
            try
            {
                //9610000001
                System.Globalization.PersianCalendar persionDate = new System.Globalization.PersianCalendar();
                string NewYear = persionDate.GetYear(System.DateTime.Now).ToString().Substring(2);

                var LastRow = Get()
                    //.Where(current => current.IsDeleted == false)
                    //.Where(current => current.IsActived == true)
                    .Where(current => current.VirtualCode.CompareTo("9610000000") < 0 ) // برای فاکتور های سال 1400
                    .OrderByDescending(x => x.VirtualCode)
                    .FirstOrDefault()
                    ;

                string LastYear = (LastRow == null || string.IsNullOrEmpty(LastRow.VirtualCode))
                    ? "96"
                    : LastRow.VirtualCode.Substring(0, 2)
                    ;

                int LastCode = (LastRow == null || string.IsNullOrEmpty(LastRow.VirtualCode))
                    ? 10000000
                    : System.Convert.ToInt32(LastRow.VirtualCode.Substring(2))
                    ;

                int NewCode = LastYear == NewYear
                    ? LastCode + 1
                    : 10000000
                    ;

                string VirtualCode = NewYear + NewCode;

                return VirtualCode;
            }

            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        public Models.Request SetControlCode(Models.Request request)
        {
            string amount = GetValidAmount(request.AmountPaid);
            string VirtualCode = GetVirtualCode();

            if (request.ServiceTariff == null)
            {
                request.ServiceTariff = DatabaseContext.ServiceTariffInSubSystems
                    .Where(current => !current.IsDeleted)
                    .Where(current => current.IsActived)
                    .Where(current => current.SubSystemId == request.SubSystemId)
                    .Select(current=>current.ServiceTariff)
                    .FirstOrDefault();

                request.ServiceTariffId = request.ServiceTariff.Id;
            }
            // d شامل 10 عدد میشود
            var d = string.Format(
                "{0}{1}{2}",
                request.Province.BankCode, // 2 رقم بانک کد استان انتخابی

                // کد ردیف درامدی
                request.ServiceTariff.BankAccount.IncomeRow.Code, // 6 رقم بعدی 

		     	// کد معین
				request.ServiceTariff.BankAccount.Certain.Code); // 2 رقم بعدی


            // شامل 13 عدد میشود
            var e = string.Format(
                "{0}{1}",
                request.ServiceTariff.RCode, // 3 رقم - ;  کد حقیقی
                VirtualCode);

            request.VirtualCode = VirtualCode;
            request.OrganDigitCode = e;

			var b1String = string.Format(
                "{0}{1}{2}{3}{4}",
                request.ServiceTariff.BankAccount.Bank.Code,
                request.ServiceTariff.BankAccount.ExecutableCode.Code,
                d, e, amount);

            var b2String = string.Format(
                "{0}{1}{2}{3}",
                request.ServiceTariff.BankAccount.Bank.Code,
                request.ServiceTariff.BankAccount.ExecutableCode.Code,
                d, e);

            b2String = Reverse(b2String);
            amount = Reverse(amount);

			// For Test
			//b1String = "2038354140201007009710208753000000482133969";
			//b2String = Reverse("2038354140201007009710208753");
			//amount = "482133969";



			int b1 = GetVerhoefCode(b1String);
            int b2 = GetVerhoefCode(string.Format("{0}{1}", b2String, amount));

            string controlcode = string.Format("{0}{1}", b1, b2);

            string banckdigit = string.Format(
                "{0}{1}{2}{3}",
                request.ServiceTariff.BankAccount.Bank.Code,  // رقم اول که معمولا با 2 شروع میشود
                controlcode, // دو رقم بعدی
                request.ServiceTariff.BankAccount.ExecutableCode.Code, // چهار رقم بعدی که معمولا 0383 هست
                d);

            string DepositNumber = string.Format(
                "{0}{1}{2}{3}{4}",
                request.ServiceTariff.BankAccount.Bank.Code, // عدد 2
                controlcode, // دو رقم
                request.ServiceTariff.BankAccount.ExecutableCode.Code, // 0383
				d,e);

			//if (DepositNumber.Trim().Length != 30)
			//{
			//    throw new Exception("طول شناسه واریز کمتر از 30 کاراکتر است.");
			//}

			request.BankDigitCode = banckdigit.Trim();

            var OldAmount = new DatabaseContext().Requests
                .Where(current => !current.IsDeleted)
                .Where(current => current.IsActived)
                .Where(current => current.InvoiceNumber == request.InvoiceNumber)?
                .FirstOrDefault()?.AmountPaid;

            if (OldAmount != null)
            {
                if (request.AmountPaid == OldAmount)
                {
                    // در صورت وجود شناسه و مبلغ ثابت از تولید شناسه جدید جلوگیری میشود
                    request.DepositNumber = request.DepositNumber.Trim();
                }
                else
                {
                    // برای تولید شناسه جدید
                    request.DepositNumber = DepositNumber.Trim();
                }
            }
            else
            {
                request.DepositNumber = DepositNumber.Trim();
            }



            return request;
        }

        private string GetValidAmount(decimal amountpaid)
        {
			if (amountpaid <= 0 || amountpaid > 999999999999999)
			{
				throw new Exception();
			}

			var amount = amountpaid.ToString(new string('0', 15));

            return amount;
        }

        public string Reverse(string text)
        {
            if (text == null) return null;
            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }

        private int GetVerhoefCode(string code)
        {
            return int.Parse(Verhoeff.generateVerhoeff(code));

            var digits = new List<int>();

            char[] array = code.ToCharArray();

            int counter = 0;

            foreach (int ch in array)
            {
                if (counter % 2 == 0)
                {
                    int digit = ch * 2;
                    int sum = 0;
                    foreach (var s in digit.ToString().ToCharArray())
                    {
                        sum += s;
                    }
                    digits.Add(sum);
                }
                else
                {
                    digits.Add(ch);
                }

                counter++;
            }

            //return digits.Sum();
            int sumDigit = 0;
            foreach (var item in digits)
            {
                sumDigit += item;
            }

            var x1 = 10 - (sumDigit % 10);

            if (x1 == 10)
            {
                return 0;
            }

            return x1;
        }


        public Models.Request CustomGet(Guid id)
        {
            try
            {
                Models.Request oRequest = DatabaseContext.Requests
                    .Where(x => x.Id == id)
                    .Include(x => x.Province)
                    .Include(x => x.ServiceTariff)
                    .FirstOrDefault();

                return oRequest;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Models.Request CustomGetByInvoiceNumber(int invoiceNumber)
        {
            try
            {
                Models.Request oRequest = DatabaseContext.Requests
                    .Where(x => x.InvoiceNumber == invoiceNumber)
                    .Include(x => x.Province)
                    .Include(x => x.ServiceTariff)
                    .FirstOrDefault();

                return oRequest;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

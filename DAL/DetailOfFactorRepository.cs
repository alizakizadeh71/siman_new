using System.Linq;
using System.Data.Entity;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class DetailOfFactorRepository : Repository<Models.DetailOfFactor>, IDetailOfFactorRepository
    {
        public DetailOfFactorRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.DetailOfFactor> Get(System.Guid headoffactorid)
        {
            try
            {
                var HFactor = DatabaseContext.HeadOfFactors
                    .Where(x => x.IsActived)
                    .Where(x => !x.IsDeleted)
                    .Where(x => x.Id == headoffactorid)
                    .FirstOrDefault();

                IQueryable<Models.DetailOfFactor> retValue;

                if (HFactor==null)
                {
                    return null;
                }

                retValue = Get()
                    .Include(x => x.HeadOfFactor)
                    .Include(x => x.HeadOfFactor.Request)
                    .Include(x => x.HeadOfFactor.HeadLine)
                    .Include(x => x.HeadOfFactor.SubHeadLine)
                    .Include(x => x.ServiceTariff)
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.HeadOfFactorId == HFactor.Id);

                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Models.ServiceTariff> GetServiceTariff(System.Guid headoffactorid)
        {
            try
            {
                var HFactor = DatabaseContext.HeadOfFactors
                    .Where(x => x.IsActived)
                    .Where(x => !x.IsDeleted)
                    .Where(x => x.Id == headoffactorid)
                    .FirstOrDefault();

                IQueryable<Models.ServiceTariff> retValue;

                if (HFactor == null)
                {
                    return null;
                }

                retValue = DatabaseContext.ServiceTariffs
                    .Where(current => current.IsDeleted == false)
                    .Where(current => current.IsActived == true)
                    .Where(current => current.SubHeadLineId == HFactor.SubHeadLineId)
                //    .ToList()
                //    .Select(x=>new Models.ServiceTariff(
                //    {
                //        Amount=x.Amount,
                //        BankAccount=x.BankAccount,
                //        BankAccountId=x.BankAccountId,
                //        CommodityInSubSystems=x.CommodityInSubSystems,
                //        DetailOfFactors=x.DetailOfFactors,
                //        Id=x.Id,
                //        InsertDateTime=x.InsertDateTime,
                //        IsActived=x.IsActived,
                //        IsDeleted=x.IsDeleted,
                //        IsSystem=x.IsSystem,
                //        IsVerified=x.IsVerified,
                ////        Name=x.Name + " - " +x.Amount,
                //        //Name=x.NameString,
                //        RCode =x.RCode,
                //        Requests=x.Requests,
                //        ServiceTariffInSubSystems=x.ServiceTariffInSubSystems,
                //        SubHeadLine=x.SubHeadLine,
                //        SubHeadLineId=x.SubHeadLineId,
                //        Unit=x.Unit,
                //        UnitId=x.UnitId,
                //        UpdateDateTime=x.UpdateDateTime,
                //        VCode=x.VCode
                //    })
                    .AsQueryable();

                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}

﻿using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class ProductTypeRepository : Repository<Models.ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}

        public IQueryable<Models.ProductType> GetProductTypes()
        {
            IQueryable<Models.ProductType> list = null;
            list = Get();
            return list;
        }

        public Models.ProductType GetByCode(string code)
        {
            Models.ProductType oProductType =
                Get()
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oProductType;
        }

        public IQueryable<Models.ProductType> GetByProductNameId(System.Guid ProductNameId)
        {
            try
            {
                IQueryable<Models.ProductType> retValue;

                retValue
                    = Get()
                    .Where(current => current.ProductNameId == ProductNameId);


                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
namespace DAL
{
    public class CommodityRepository : Repository<Models.Commodity>, ICommodityRepository
    {
        public CommodityRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

    }
}

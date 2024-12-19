namespace DAL
{
    public class CommodityInSubSystemRepository : Repository<Models.CommodityInSubSystem>, ICommodityInSubSystemRepository
    {
        public CommodityInSubSystemRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

    }
}

using Models;
using System.Linq;

namespace DAL
{
    public class SiteSettingRepository : Repository<SiteSetting>, ISiteSettingRepository
    {
        public SiteSettingRepository(Models.DatabaseContext databaseContext)
    : base(databaseContext)
        {
        }
        public SiteSetting GetSetting()
        {
            return Get().FirstOrDefault();
        }
    }
}
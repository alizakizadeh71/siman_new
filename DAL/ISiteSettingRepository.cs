using Models;

namespace DAL
{
    public interface ISiteSettingRepository : IRepository<SiteSetting>
    {
        SiteSetting GetSetting();
    }
}
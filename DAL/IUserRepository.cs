namespace DAL
{
    public interface IUserRepository : IRepository<Models.User>
    {
        Models.User GetByUserName(string username);

        Models.User GetByPhoneNumebr(string Phonenumber);

        System.Linq.IQueryable<Models.User> Get(Models.User user);

        string GetAccountStatus(string userName);
    }
}

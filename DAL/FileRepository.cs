namespace DAL
{
    public class FileRepository : Repository<Models.File>, IFileRepository
    {
        public FileRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }
    }
}

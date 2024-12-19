namespace DAL
{
    public interface IUnitOfWork : System.IDisposable
    {
        IDetailOfFactorRepository DetailOfFactorRepository { get; }
        IFactorMessageRepository FactorMessageRepository { get; }
        IHeadOfFactorRepository HeadOfFactorRepository { get; }
        IServiceTariffInSubSystemRepository ServiceTariffInSubSystemRepository { get; }
        ICommodityInSubSystemRepository CommodityInSubSystemRepository { get; }
        ICommodityRepository CommodityRepository { get; }
        IServiceTariffRepository ServiceTariffRepository { get; }
        IPaymentwaitinglistRepository PaymentwaitinglistRepository { get; }
        IUnitRepository UnitRepository { get; }
        IBankAccountRepository BankAccountRepository { get; }
        IIncomeRowRepository IncomeRowRepository { get; }
        IExecutableCodeRepository ExecutableCodeRepository { get; }
        ICertainRepository CertainRepository { get; }
        IBankRepository BankRepository { get; }
        IHeadLineRepository HeadLineRepository { get; }
        ISubHeadLineRepository SubHeadLineRepository { get; }
        ICurrencyUnitRepository CurrencyUnitRepository { get; }
        IAccountNumberManageRepository AccountNumberManageRepository { get; }
        IAccountNumberManageLogRepository AccountNumberManageLogRepository { get; }
        IAccountNumberRepository AccountNumberRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        ICityRepository CityRepository { get; }
        ISubSystemRepository SubSystemRepository { get; }
        IInventoryamountRepository InventoryamountRepository { get; }
        IFileRepository FileRepository { get; }
        IErrorLogRepository ErrorLogRepository { get; }
        IMessageRepository MessageRepository { get; }
        IRequestRepository RequestRepository { get; }
        IRoleRepository RoleRepository { get; }
        IProjectActionRepository ProjectActionRepository { get; }
        IAccessTypeRepository AccessTypeRepository { get; }
        IUserLoginLogRepository UserLoginLogRepository { get; }

        void Save();
    }
}

namespace DAL
{
    public class UnitOfWork : System.Object, IUnitOfWork
    {
        public UnitOfWork()
        {
            IsDisposed = false;
        }

        protected bool IsDisposed { get; set; }

        private Models.DatabaseContext _databaseContext;
        protected virtual Models.DatabaseContext DatabaseContext
        {
            get
            {
                if (_databaseContext == null)
                {
                    _databaseContext = new Models.DatabaseContext();
                }
                return (_databaseContext);
            }
        }


        private IDetailOfFactorRepository _DetailOfFactorRepository;
        public IDetailOfFactorRepository DetailOfFactorRepository
        {
            get
            {
                if (_DetailOfFactorRepository == null)
                {
                    _DetailOfFactorRepository = new DetailOfFactorRepository(DatabaseContext);
                }
                return (_DetailOfFactorRepository);
            }
        }


        private IFactorMessageRepository _FactorMessageRepository;
        public IFactorMessageRepository FactorMessageRepository
        {
            get
            {
                if (_FactorMessageRepository == null)
                {
                    _FactorMessageRepository = new FactorMessageRepository(DatabaseContext);
                }
                return (_FactorMessageRepository);
            }
        }


        private IHeadOfFactorRepository _HeadOfFactorRepository;
        public IHeadOfFactorRepository HeadOfFactorRepository
        {
            get
            {
                if (_HeadOfFactorRepository == null)
                {
                    _HeadOfFactorRepository = new HeadOfFactorRepository(DatabaseContext);
                }
                return (_HeadOfFactorRepository);
            }
        }


        private IServiceTariffInSubSystemRepository _ServiceTariffInSubSystemRepository;
        public IServiceTariffInSubSystemRepository ServiceTariffInSubSystemRepository
        {
            get
            {
                if (_ServiceTariffInSubSystemRepository == null)
                {
                    _ServiceTariffInSubSystemRepository = new ServiceTariffInSubSystemRepository(DatabaseContext);
                }
                return (_ServiceTariffInSubSystemRepository);
            }
        }


        private ICommodityRepository _CommodityRepository;
        public ICommodityRepository CommodityRepository
        {
            get
            {
                if (_CommodityRepository == null)
                {
                    _CommodityRepository = new CommodityRepository(DatabaseContext);
                }
                return (_CommodityRepository);
            }
        }


        private ICommodityInSubSystemRepository _CommodityInSubSystemRepository;
        public ICommodityInSubSystemRepository CommodityInSubSystemRepository
        {
            get
            {
                if (_CommodityInSubSystemRepository == null)
                {
                    _CommodityInSubSystemRepository = new CommodityInSubSystemRepository(DatabaseContext);
                }
                return (_CommodityInSubSystemRepository);
            }
        }


        private IUnitRepository _UnitRepository;
        public IUnitRepository UnitRepository
        {
            get
            {
                if (_UnitRepository == null)
                {
                    _UnitRepository = new UnitRepository(DatabaseContext);
                }
                return (_UnitRepository);
            }
        }


        private IServiceTariffRepository _ServiceTariffRepository;
        public IServiceTariffRepository ServiceTariffRepository
        {
            get
            {
                if (_ServiceTariffRepository == null)
                {
                    _ServiceTariffRepository = new ServiceTariffRepository(DatabaseContext);
                }
                return (_ServiceTariffRepository);
            }
        }


        private IIncomeRowRepository _IncomeRowRepository;
        public IIncomeRowRepository IncomeRowRepository
        {
            get
            {
                if (_IncomeRowRepository == null)
                {
                    _IncomeRowRepository = new IncomeRowRepository(DatabaseContext);
                }
                return (_IncomeRowRepository);
            }
        }


        private IExecutableCodeRepository _ExecutableCodeRepository;
        public IExecutableCodeRepository ExecutableCodeRepository
        {
            get
            {
                if (_ExecutableCodeRepository == null)
                {
                    _ExecutableCodeRepository = new ExecutableCodeRepository(DatabaseContext);
                }
                return (_ExecutableCodeRepository);
            }
        }


        private ICertainRepository _CertainRepository;
        public ICertainRepository CertainRepository
        {
            get
            {
                if (_CertainRepository == null)
                {
                    _CertainRepository = new CertainRepository(DatabaseContext);
                }
                return (_CertainRepository);
            }
        }


        private IBankRepository _BankRepository;
        public IBankRepository BankRepository
        {
            get
            {
                if (_BankRepository == null)
                {
                    _BankRepository = new BankRepository(DatabaseContext);
                }
                return (_BankRepository);
            }
        }

        private IPaymentHeaderRepository _PaymentHeaderRepository;
        public IPaymentHeaderRepository PaymentHeaderRepository
        {
            get
            {
                if (_PaymentHeaderRepository == null)
                {
                    _PaymentHeaderRepository = new PaymentHeaderRepository(DatabaseContext);
                }
                return (_PaymentHeaderRepository);
            }
        }

        private IPaymentDetailRepository _PaymentDetailRepository;
        public IPaymentDetailRepository PaymentDetailRepository
        {
            get
            {
                if (_PaymentDetailRepository == null)
                {
                    _PaymentDetailRepository = new PaymentDetailRepository(DatabaseContext);
                }
                return (_PaymentDetailRepository);
            }
        }


        private IBankAccountRepository _BankAccountRepository;
        public IBankAccountRepository BankAccountRepository
        {
            get
            {
                if (_BankAccountRepository == null)
                {
                    _BankAccountRepository = new BankAccountRepository(DatabaseContext);
                }
                return (_BankAccountRepository);
            }
        }


        private IHeadLineRepository _HeadLineRepository;
        public IHeadLineRepository HeadLineRepository
        {
            get
            {
                if (_HeadLineRepository == null)
                {
                    _HeadLineRepository = new HeadLineRepository(DatabaseContext);
                }
                return (_HeadLineRepository);
            }
        }


        private ISubHeadLineRepository _SubHeadLineRepository;
        public ISubHeadLineRepository SubHeadLineRepository
        {
            get
            {
                if (_SubHeadLineRepository == null)
                {
                    _SubHeadLineRepository = new SubHeadLineRepository(DatabaseContext);
                }
                return (_SubHeadLineRepository);
            }
        }

        private IProductNameRepository _ProductNameRepository;
        public IProductNameRepository ProductNameRepository
        {
            get
            {
                if (_ProductNameRepository == null)
                {
                    _ProductNameRepository = new ProductNameRepository(DatabaseContext);
                }
                return (_ProductNameRepository);
            }
        }

        private IProductTypeRepository _ProductTypeRepository;
        public IProductTypeRepository ProductTypeRepository
        {
            get
            {
                if (_ProductTypeRepository == null)
                {
                    _ProductTypeRepository = new ProductTypeRepository(DatabaseContext);
                }
                return (_ProductTypeRepository);
            }
        }

        private INewsReopsitory _newsReopsitory;
        public INewsReopsitory NewsReopsitory
        {
            get
            {
                if (_newsReopsitory == null)
                {
                    _newsReopsitory = new NewsReopsitory(DatabaseContext);
                }
                return (_newsReopsitory);
            }
        }
        private IPackageTypeRepository _PackageTypeRepository;
        public IPackageTypeRepository PackageTypeRepository
        {
            get
            {
                if (_PackageTypeRepository == null)
                {
                    _PackageTypeRepository = new PackageTypeRepository(DatabaseContext);
                }
                return (_PackageTypeRepository);
            }
        }

        private IFactoryNameRepository _FactoryNameRepository;
        public IFactoryNameRepository FactoryNameRepository
        {
            get
            {
                if (_FactoryNameRepository == null)
                {
                    _FactoryNameRepository = new FactoryNameRepository(DatabaseContext);
                }
                return (_FactoryNameRepository);
            }
        }

        private IInventoryamountRepository _InventoryamountRepository;
        public IInventoryamountRepository InventoryamountRepository
        {
            get
            {
                if (_InventoryamountRepository == null)
                {
                    _InventoryamountRepository = new InventoryamountRepository(DatabaseContext);
                }
                return (_InventoryamountRepository);
            }
        }

        private IPaymentwaitinglistRepository _PaymentwaitinglistRepository;
        public IPaymentwaitinglistRepository PaymentwaitinglistRepository
        {
            get
            {
                if (_PaymentwaitinglistRepository == null)
                {
                    _PaymentwaitinglistRepository = new PaymentwaitinglistRepository(DatabaseContext);
                }
                return (_PaymentwaitinglistRepository);
            }
        }

        private ITonnageRepository _tonnageRepository;
        public ITonnageRepository tonnageRepository
        {
            get
            {
                if (_tonnageRepository == null)
                {
                    _tonnageRepository = new TonnageRepository(DatabaseContext);
                }
                return (_tonnageRepository);
            }
        }


        private ICurrencyUnitLogRepository _CurrencyUnitLogRepository;
        public ICurrencyUnitLogRepository CurrencyUnitLogRepository
        {
            get
            {
                if (_CurrencyUnitLogRepository == null)
                {
                    _CurrencyUnitLogRepository = new CurrencyUnitLogRepository(DatabaseContext);
                }
                return (_CurrencyUnitLogRepository);
            }
        }


        private ICurrencyUnitRepository _CurrencyUnitRepository;
        public ICurrencyUnitRepository CurrencyUnitRepository
        {
            get
            {
                if (_CurrencyUnitRepository == null)
                {
                    _CurrencyUnitRepository = new CurrencyUnitRepository(DatabaseContext);
                }
                return (_CurrencyUnitRepository);
            }
        }


        private IAccountNumberManageRepository _AccountNumberManageRepository;
        public IAccountNumberManageRepository AccountNumberManageRepository
        {
            get
            {
                if (_AccountNumberManageRepository == null)
                {
                    _AccountNumberManageRepository = new AccountNumberManageRepository(DatabaseContext);
                }
                return (_AccountNumberManageRepository);
            }
        }


        private IAccountNumberManageLogRepository _AccountNumberManageLogRepository;
        public IAccountNumberManageLogRepository AccountNumberManageLogRepository
        {
            get
            {
                if (_AccountNumberManageLogRepository == null)
                {
                    _AccountNumberManageLogRepository = new AccountNumberManageLogRepository(DatabaseContext);
                }
                return (_AccountNumberManageLogRepository);
            }
        }


        private IAccountNumberRepository _AccountNumberRepository;
        public IAccountNumberRepository AccountNumberRepository
        {
            get
            {
                if (_AccountNumberRepository == null)
                {
                    _AccountNumberRepository = new AccountNumberRepository(DatabaseContext);
                }
                return (_AccountNumberRepository);
            }
        }


        private IProvinceRepository _ProvinceRepository;
        public IProvinceRepository ProvinceRepository
        {
            get
            {
                if (_ProvinceRepository == null)
                {
                    _ProvinceRepository = new ProvinceRepository(DatabaseContext);
                }
                return (_ProvinceRepository);
            }
        }




        private ICityRepository _CityRepository;
        public ICityRepository CityRepository
        {
            get
            {
                if (_CityRepository == null)
                {
                    _CityRepository = new CityRepository(DatabaseContext);
                }
                return (_CityRepository);
            }
        }

        private IVillageRepository _VillageRepository;
        public IVillageRepository VillageRepository
        {
            get
            {
                if (_VillageRepository == null)
                {
                    _VillageRepository = new VillageRepository(DatabaseContext);
                }
                return (_VillageRepository);
            }
        }

        private ISubSystemRepository _SubSystemRepository;
        public ISubSystemRepository SubSystemRepository
        {
            get
            {
                if (_SubSystemRepository == null)
                {
                    _SubSystemRepository = new SubSystemRepository(DatabaseContext);
                }
                return (_SubSystemRepository);
            }
        }

        private IFileRepository _FileRepository;
        public IFileRepository FileRepository
        {
            get
            {
                if (_FileRepository == null)
                {
                    _FileRepository = new FileRepository(DatabaseContext);
                }
                return (_FileRepository);
            }
        }


        private IErrorLogRepository _ErrorLogRepository;
        public IErrorLogRepository ErrorLogRepository
        {
            get
            {
                if (_ErrorLogRepository == null)
                {
                    _ErrorLogRepository = new ErrorLogRepository(DatabaseContext);
                }
                return (_ErrorLogRepository);
            }
        }


        private IMessageRepository _MessageRepository;
        public IMessageRepository MessageRepository
        {
            get
            {
                if (_MessageRepository == null)
                {
                    _MessageRepository = new MessageRepository(DatabaseContext);
                }
                return (_MessageRepository);
            }
        }

        private IMarketerTransactionsRepository _MarketerTransactionsRepository;
        public IMarketerTransactionsRepository MarketerTransactionsRepository
        {
            get
            {
                if (_MarketerTransactionsRepository == null)
                {
                    _MarketerTransactionsRepository = new MarketerTransactionsRepository(DatabaseContext);
                }
                return (_MarketerTransactionsRepository);
            }
        }


        private IRequestRepository _RequestRepository;
        public IRequestRepository RequestRepository
        {
            get
            {
                if (_RequestRepository == null)
                {
                    _RequestRepository = new RequestRepository(DatabaseContext);
                }
                return (_RequestRepository);
            }
        }

        private IFactorCementRepository _FactorCementRepository;
        public IFactorCementRepository FactorCementRepository
        {
            get
            {
                if (_FactorCementRepository == null)
                {
                    _FactorCementRepository = new FactorCementRepository(DatabaseContext);
                }
                return (_FactorCementRepository);
            }
        }

        private IwalletFactorRepository _walletFactorRepository;
        public IwalletFactorRepository walletFactorRepository
        {
            get
            {
                if (_walletFactorRepository == null)
                {
                    _walletFactorRepository = new walletFactorRepository(DatabaseContext);
                }
                return (_walletFactorRepository);
            }
        }

        private IFinancialManagementRepository _FinancialManagementRepository;
        public IFinancialManagementRepository FinancialManagementRepository
        {
            get
            {
                if (_FinancialManagementRepository == null)
                {
                    _FinancialManagementRepository = new FinancialManagementRepository(DatabaseContext);
                }
                return (_FinancialManagementRepository);
            }
        }

        private IDestinationManagementRepository _DestinationManagementRepository;
        public IDestinationManagementRepository DestinationManagementRepository
        {
            get
            {
                if (_DestinationManagementRepository == null)
                {
                    _DestinationManagementRepository = new DestinationManagementRepository(DatabaseContext);
                }
                return (_DestinationManagementRepository);
            }
        }


        private IRoleRepository _RoleRepository;
        public IRoleRepository RoleRepository
        {
            get
            {
                if (_RoleRepository == null)
                {
                    _RoleRepository = new RoleRepository(DatabaseContext);
                }
                return (_RoleRepository);
            }
        }


        private IUserRepository _UserRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (_UserRepository == null)
                {
                    _UserRepository = new UserRepository(DatabaseContext);
                }
                return (_UserRepository);
            }
        }


        private IProjectActionRepository _ProjectActionRepository;
        public IProjectActionRepository ProjectActionRepository
        {
            get
            {
                if (_ProjectActionRepository == null)
                {
                    _ProjectActionRepository = new ProjectActionRepository(DatabaseContext);
                }
                return (_ProjectActionRepository);
            }
        }


        private IAccessTypeRepository _AccessTypeRepository;
        public IAccessTypeRepository AccessTypeRepository
        {
            get
            {
                if (_AccessTypeRepository == null)
                {
                    _AccessTypeRepository = new AccessTypeRepository(DatabaseContext);
                }
                return (_AccessTypeRepository);
            }
        }


        private IUserLoginLogRepository _UserLoginLogRepository;
        public IUserLoginLogRepository UserLoginLogRepository
        {
            get
            {
                if (_UserLoginLogRepository == null)
                {
                    _UserLoginLogRepository = new UserLoginLogRepository(DatabaseContext);
                }
                return (_UserLoginLogRepository);
            }
        }


        public void Save()
        {
            try
            {
                DatabaseContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);

                throw;
            }
        }

        public virtual int ExecuteSqlCommand(string commandText)
        {
            int intResult = -1;

            using (System.Transactions.TransactionScope oTransactionScope =
                new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                intResult = DatabaseContext.Database.ExecuteSqlCommand
                    (System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, commandText);
            }

            return (intResult);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed == false)
            {
                if (disposing)
                {
                    _databaseContext.Dispose();
                }
            }
            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }
    }
}

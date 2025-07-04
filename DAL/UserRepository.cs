﻿using Models;
using System;
using System.Linq;

namespace DAL
{
    public class UserRepository : Repository<Models.User>, IUserRepository
    {
        public UserRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public Models.User GetByUserName(string username)
        {
            try
            {
                Models.User oUser =
                    Get()
                    .Where(current => string.Compare(current.UserName, username,
                        System.StringComparison.InvariantCultureIgnoreCase) == 0)
                    .FirstOrDefault();

                return (oUser);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Models.User> Get(Models.User user)
        {
            try
            {
                IQueryable<Models.User> retValue;

                if ((int)user.Role.Code == (int)Enums.Roles.ProvinceExpert00)
                {
                    retValue
                        = Get()
                        .Where(current => current.Role.Code <= (int)user.Role.CodeEnum)
                        .Where(current => current.ProvinceId == user.ProvinceId);
                }

                else if ((int)user.Role.Code == (int)Enums.Roles.SoperAdmin)
                {
                    retValue
                        = Get()
                        .Where(current => current.Role.Code <= (int)user.Role.CodeEnum);
                }

                else
                {
                    retValue
                        = Get()
                        .Where(current => current.Role.Code <= (int)user.Role.CodeEnum);
                }
                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public User GetByPhoneNumebr(string phoneNumeber)
        {
            try
            {
                User user =
                    Get()
                    .Where(u => u.BuyerMobile == phoneNumeber)
                    .FirstOrDefault();

                return user;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public string GetAccountStatus(string userName)
        {
            try
            {
                User user =
                    Get()
                        .Where(u => u.UserName == userName)
                        .FirstOrDefault();

                long balanceAmount = user.InitialCredit - user.creditAmount;
                string accountStatus = "متعادل";
                string balanceDetails = "0";
                if (balanceAmount > 0)
                {
                    accountStatus = "بدهکار";
                    balanceDetails = $"{Math.Abs(balanceAmount).ToString("N0")}";
                }
                else if (balanceAmount < 0)
                {
                    accountStatus = "طلب کار";
                    balanceDetails = $"{Math.Abs(balanceAmount).ToString("N0")}";
                }
                else
                {
                    accountStatus = "";
                }

                return balanceDetails + accountStatus;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool IsMarketingCodeAvailable(string marketingCode)
        {
            try
            {
                bool result = Get().Any(u => u.MarketingCode == marketingCode);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public User GetUserByMarketingCode(string marketingCode)
        {
            try
            {
                User user = Get().First(u => u.MarketingCode == marketingCode);

                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetAddressByPhoneNumebr(string Phonenumber)
        {
            try
            {
                string address = Get().FirstOrDefault(u => u.BuyerMobile == Phonenumber).Address;
                return address;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

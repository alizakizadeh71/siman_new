namespace Infrastructure
{
	public class AuthenticatedUser :System.Object,Models.IAuthenticatedUser
	{

        public static bool IsAuthenticated
        {
            get
            {
                if ((System.Web.HttpContext.Current != null) &&
                    (System.Web.HttpContext.Current.Request != null) &&
                    (System.Web.HttpContext.Current.Request.IsAuthenticated))
                {
                    if (Sessions.AuthenticatedUser == null)
                    {
                        // باشد false بايد ،endResponse دقت کنيد که مقدار
                        System.Web.HttpContext.Current.Response.Redirect
                            ("~/Account/Logout", endResponse: false);

                        return (false);
                    }
                    else
                    {
                        return (true);
                    }
                }
                else
                {
                    if (Sessions.AuthenticatedUser != null)
                    {
                        Sessions.AuthenticatedUser = null;

                        // باشد false بايد ،endResponse دقت کنيد که مقدار
                        System.Web.HttpContext.Current.Response.Redirect
                            ("~/Account/Logout", endResponse: false);
                    }

                    return (false);
                }
            }
        }

        public static void SignOut()
        {
            // **************************************************
            DAL.UnitOfWork oUnitOfWork = null;
            try
            {
                if (Infrastructure.Sessions.AuthenticatedUser != null)
                {
                    System.Guid sUserId =Infrastructure.Sessions.AuthenticatedUser.Id;

                    string strSessionId =System.Web.HttpContext.Current.Session.SessionID;

                    oUnitOfWork =new DAL.UnitOfWork();

                    Models.UserLoginLog oUserLoginLog =
                        oUnitOfWork.UserLoginLogRepository
                        .GetBySessionIdAndUserId(strSessionId, sUserId);

                    if (oUserLoginLog != null)
                    {
                        oUserLoginLog.LogoutDateTime = System.DateTime.Now;
                        oUnitOfWork.Save();
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (oUnitOfWork != null)
                {
                    oUnitOfWork.Dispose();
                    oUnitOfWork = null;
                }
            }
            // **************************************************

            System.Web.Security.FormsAuthentication.SignOut();

            //Session.Clear();
            Sessions.AuthenticatedUser = null;
            //Session.Remove(Infrastructure.Sessions.AuthenticatedUserKeyName);
        }
        public AuthenticatedUser(Models.User user) 
        {
            User = user;
        }

        public Models.User User { get; set; }

        public System.Guid Id
        {
            get
            {
                return (User.Id);
            }
        }
        public string Password
        {
            get
            {
                return (User.Password.ToLower());
            }
        }

        public string UserName
        {
            get
            {
                return (User.UserName.ToLower());
            }
        }

        private Enums.Roles? _role;
        public Enums.Roles Role
        {
            get
            {
                if (_role.HasValue == false)
                {
                    DAL.UnitOfWork oUnitOfWork =
                        new DAL.UnitOfWork();

                    Models.User oUser =
                        oUnitOfWork.UserRepository.GetById(Id);

                    //if (oUser == null)
                    //{
                    //    _role = Enums.Roles.ProvinceMaliExpert;
                    //}
                    //else
                    //{
                    //    if (oUser.Role == null)
                    //    {
                    //        _role = Enums.Roles.ProvinceMaliExpert;
                    //    }
                    //    else
                    //    {
                            _role = oUser.Role.CodeEnum;
                        //}
                    //}
                }
                return (_role.Value);
            }
        }

        public int RoleCode
        {
            get
            {
                return ((int)Role);
            }
        }

        public string FullName
        {
            get
            {
                return (User.FullName);
            }
        }

        public int creditAmount
        {
            get
            {
                return (User.creditAmount);
            }
        }
    }
}

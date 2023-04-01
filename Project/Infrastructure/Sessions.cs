using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastructure
{
    public static class Sessions
    {
        private static string CultureKeyName
        {
            get
            {
                return ("Culture");
            }
        }

        public static string Culture
        {
            get
            {
                if ((System.Web.HttpContext.Current.Session[CultureKeyName] == null) ||
                    (System.Web.HttpContext.Current.Session[CultureKeyName].ToString() == string.Empty))
                {
                    DAL.UnitOfWork oUnitOfWork = null;

                    try
                    {
                        if (1 == 1)
                        {
                            System.Web.HttpContext.Current.Session[CultureKeyName] = "fa-IR";
                        }
                        else
                        {
                            System.Web.HttpContext.Current.Session[CultureKeyName] = "en-US";
                        }
                    }
                    catch (System.Exception ex)
                    {
                        System.Web.HttpContext.Current.Session[CultureKeyName] = "fa-IR";
                    }
                    finally
                    {
                        if (oUnitOfWork != null)
                        {
                            oUnitOfWork.Dispose();
                            oUnitOfWork = null;
                        }
                    }
                }

                return (System.Web.HttpContext.Current.Session[CultureKeyName].ToString());
            }
            set
            {
                System.Web.HttpContext.Current.Session[CultureKeyName] = value;
            }
        }

        private static string AuthenticatedUserKeyName
        {
            get
            {
                return ("AuthenticatedUser");
            }
        }

        public static AuthenticatedUser AuthenticatedUser
        {
            get
            {
                AuthenticatedUser oAuthenticatedUser 
                    = System.Web.HttpContext.Current.Session[AuthenticatedUserKeyName] as AuthenticatedUser;

                return (oAuthenticatedUser);
            }
            set
            {
                System.Web.HttpContext.Current.Session[AuthenticatedUserKeyName] = value;
            }
        }

        private static string CaptachKeyName
        {
            get
            {
                return ("Captach");
            }
        }

        public static string Captach
        {
            get
            {
                string oCaptach= System.Web.HttpContext.Current.Session[CaptachKeyName] as string;
                return (oCaptach);
            }
            set
            {
                System.Web.HttpContext.Current.Session[CaptachKeyName] = value;
            }
        }

        private static string SearchDataSourceKeyName
        {
            get
            {
                return ("SearchDataSource");
            }
        }

        public static object SearchDataSource
        {
            get
            {
                object oDataSource = System.Web.HttpContext.Current.Session[SearchDataSourceKeyName] as object;
                return (oDataSource);
            }
            set
            {
                System.Web.HttpContext.Current.Session[SearchDataSourceKeyName] = value;
            }
        }

        private static string CBISessionIdKeyName
        {
            get
            {
                return ("CBISessionId");
            }
        }

        public static string CBISessionId
        {
            get
            {
                string oCBISessionId = System.Web.HttpContext.Current.Session[CBISessionIdKeyName] as string;
                return (oCBISessionId);
            }
            set
            {
                System.Web.HttpContext.Current.Session[CBISessionIdKeyName] = value;
            }
        }

    }
}
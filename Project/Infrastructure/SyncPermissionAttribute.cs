using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public class SyncPermissionAttribute : System.Attribute
    {
        public SyncPermissionAttribute(bool isPublic = false, Enums.Roles role = Enums.Roles.None)
        {
            IsPublic = isPublic;
            var arrayRoles = Enum.GetValues(typeof(Enums.Roles));

            //if (role == Enums.Roles.MaliAdminGholami)
            //{ }

            Roles = new List<Enums.Roles>();

            ///
            /// اگر کاربر وب سرویس بود فقط خودش دسترسی داشته باشد
            ///
            if (role == Enums.Roles.WebServiceUser)
            {
                foreach (Enums.Roles item in arrayRoles)
                {
                    if (item == Enums.Roles.WebServiceUser)
                        Roles.Add(item);
                }
            }

            ///
            ///اگر نقشی انتخاب نشده همه بجر وب سرویس
            /// 
            else if (role == Enums.Roles.None)
            {
                foreach (Enums.Roles item in arrayRoles)
                {
                    if (item != Enums.Roles.None && item != Enums.Roles.WebServiceUser)
                        Roles.Add(item);
                }
            }

            ///
            ///اگر نقش راهبر استان بود، کارشناس سازمان نبیند
            /// 
            else if (role == Enums.Roles.ProvinceExpert00)
            {
                foreach (Enums.Roles item in arrayRoles)
                {
                    if (item != Enums.Roles.None && item != Enums.Roles.WebServiceUser && item != Enums.Roles.MaliExpert01 && ((int)item >= (int)role))
                        Roles.Add(item);
                }
            }


            ///
            /// اگر نقش وارد شده تمامی بالا دستی ها بجز وب سرویس
            ///
            else
            {
                foreach (Enums.Roles item in arrayRoles)
                {
                    if (item != Enums.Roles.None && item != Enums.Roles.WebServiceUser && ((int)item >= (int)role))
                    {
                        Roles.Add(item);
                    }
                }
            }
        }

        public bool IsPublic { get; set; }
        public IList<Enums.Roles> Roles { get; set; }
    }
}
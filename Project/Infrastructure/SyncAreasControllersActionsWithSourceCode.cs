using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPS
{
    public class SyncAreasControllersActionsWithSourceCode
    {
        private DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

        public SyncAreasControllersActionsWithSourceCode()
        {
            try
            {
                var varProjectActions = oUnitOfWork.ProjectActionRepository.Get();

                foreach (Models.ProjectAction oProjectAction in varProjectActions)
                {
                    oUnitOfWork.ProjectActionRepository.DeleteById(oProjectAction.Id);
                }
                oUnitOfWork.Save();

                SyncAreasAndControllers();

                oUnitOfWork.Save();

            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                throw ex;
            }
        }

        private void SyncAreasAndControllers()
        {
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            foreach (System.Type oType in myAssembly.ManifestModule.FindTypes(null, null))
            {
                if (IsAValidController(oType))
                {
                    if (Utilities.String.Contains(oType.Namespace, "Controllers", ignoreCase: true))
                    {
                        //System.Attribute[] oCustomAttributes =System.Attribute.GetCustomAttributes(oType);

                        //Infrastructure.SyncPermissionAttribute oPermissionAttribute = null;

                        //foreach (System.Attribute oCustomAttribute in oCustomAttributes)
                        //{
                        //    if (oCustomAttribute is Infrastructure.SyncPermissionAttribute)
                        //    {
                        //        oPermissionAttribute =oCustomAttribute as Infrastructure.SyncPermissionAttribute;

                        //        break;
                        //    }
                        //}

                        //if (oPermissionAttribute == null)
                        //    continue;

                        string areaName = string.Empty;

                        if (Utilities.String.Contains(oType.Namespace, ".Areas.", ignoreCase: true))
                        {
                            string[] strNamespaceParts = oType.Namespace.Split('.');
                            for (int intIndex = 0; intIndex <= strNamespaceParts.Length - 1; intIndex++)
                            {
                                string strNamespacePart =
                                    strNamespaceParts[intIndex];

                                if (string.Compare(strNamespacePart, "Areas", ignoreCase: true) == 0)
                                {
                                    areaName =
                                        strNamespaceParts[intIndex + 1];
                                    break;
                                }
                            }
                        }

                        string controllerName = oType.Name.Substring(0, oType.Name.Length - 10);

                        if (controllerName == "payment")
                        { }
                        SyncActions(areaName, controllerName, oType);
                    }
                }
            }
        }

        private bool IsAValidController(System.Type type)
        {
            if ((type.IsClass) &&
                (type.Name.EndsWith("Controller", System.StringComparison.InvariantCultureIgnoreCase)) &&
                (type.Namespace.EndsWith("Controllers", System.StringComparison.InvariantCultureIgnoreCase)) &&
                (type.Name.StartsWith("T4MVC_", System.StringComparison.InvariantCultureIgnoreCase) == false))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        private void SyncActions(string areaName, string controllerName, System.Type oType)
        {
            var varBindingFlags =
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.DeclaredOnly;

            var varMethodInfos =
                oType.GetMethods(bindingAttr: varBindingFlags)
                .Where(current => current.IsSpecialName == false)
                .OrderBy(current => current.Name)
                ;

            foreach (System.Reflection.MethodInfo oMethodInfo in varMethodInfos)
            {
                bool blnIsAction = true;
                string strActionName = oMethodInfo.Name;

                System.Attribute[] oCustomAttributes =
                    System.Attribute.GetCustomAttributes(oMethodInfo);

                Infrastructure.SyncPermissionAttribute oSyncPermissionAttribute = null;

                foreach (System.Attribute oCustomAttribute in oCustomAttributes)
                {
                    System.Web.Mvc.NonActionAttribute oNonActionAttribute =
                        oCustomAttribute as System.Web.Mvc.NonActionAttribute;

                    if (oNonActionAttribute != null)
                    {
                        blnIsAction = false;
                        break;
                    }

                    System.Web.Mvc.ActionNameAttribute oActionNameAttribute = oCustomAttribute as System.Web.Mvc.ActionNameAttribute;

                    if (oActionNameAttribute != null)
                    {
                        strActionName = oActionNameAttribute.Name;
                    }

                    if (oCustomAttribute is Infrastructure.SyncPermissionAttribute)
                    {
                        oSyncPermissionAttribute = oCustomAttribute as Infrastructure.SyncPermissionAttribute;
                    }
                }

                if (blnIsAction)
                {
                    Models.ProjectAction OlrRow
                        = oUnitOfWork.ProjectActionRepository
                        .GetAction(areaName, controllerName, strActionName);

                    if (OlrRow == null)
                    {
                        List<Models.Role> NewRoles = new List<Models.Role>();
                        foreach (Enums.Roles role in oSyncPermissionAttribute.Roles)
                        {
                            int RoleCode = (int)role;
                            var RoleRow
                                = oUnitOfWork.RoleRepository
                                .Get()
                                .Where(current => current.Code == RoleCode)
                                .FirstOrDefault();

                            if (RoleRow != null)
                                NewRoles.Add(RoleRow);
                        }

                        Models.ProjectAction oProjectAction = new Models.ProjectAction()
                        {
                            Action = strActionName,
                            Area = areaName,
                            Controller = controllerName,
                            Id = Guid.NewGuid(),
                            InsertDateTime = DateTime.Now,
                            IsActived = true,
                            IsDeleted = false,
                            IsPublic = oSyncPermissionAttribute.IsPublic,
                            IsSystem = true,
                            IsVerified = true,
                            UpdateDateTime = DateTime.Now,
                            Roles = NewRoles.Count > 0 ? NewRoles : null,
                        };

                        oUnitOfWork.ProjectActionRepository.Insert(oProjectAction);
                        oUnitOfWork.Save();
                    }
                }
            }
        }

    }
}

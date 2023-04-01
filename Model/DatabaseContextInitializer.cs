using System.Linq;
using System.Data.Entity;

namespace Models
{
   internal static class DatabaseContextInitializer
    {
       static DatabaseContextInitializer()
       { }

       internal static void Seed(DatabaseContext databaseContext)
       {
           Role oPersianProgrammerRole = new Role();
           oPersianProgrammerRole.IsActived = true;
           oPersianProgrammerRole.IsDeleted = false;
           oPersianProgrammerRole.IsSystem = true;
           oPersianProgrammerRole.IsVerified = true;
           oPersianProgrammerRole.Name = "برنامه نويس";
           oPersianProgrammerRole.Code = (int)Enums.Roles.Programmer;
           oPersianProgrammerRole.Id = System.Guid.NewGuid();
           databaseContext.Roles.Add(oPersianProgrammerRole);

           Role oPersianAdministratorRole = new Role();
           oPersianAdministratorRole.IsActived = true;
           oPersianAdministratorRole.IsDeleted = false;
           oPersianAdministratorRole.IsSystem = true;
           oPersianAdministratorRole.IsVerified = true;
           oPersianAdministratorRole.Name = "راهبر سیستم";
           oPersianAdministratorRole.Code = (int)Enums.Roles.SoperAdmin;
           oPersianAdministratorRole.Id = System.Guid.NewGuid();
           databaseContext.Roles.Add(oPersianAdministratorRole);
           
           databaseContext.SaveChanges();
       }
    }
}

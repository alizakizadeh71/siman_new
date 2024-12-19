namespace Enums
{
    enum LoginResponce : int
    {
        UserName = -1,
        Password = -2,
        Deleted = -3,
        NotActived = -4,
        Login = 0
        // -1 نام کاربری اشتباه است
        //-2 کلمه عبور اشتباه است
        //-3 حذف شده است
        //-4 غیر فعال
        // 0 ورود موفق
    }
}

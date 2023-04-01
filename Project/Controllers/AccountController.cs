using System.Linq;
using System.Data.Entity;
using System;
using System.Web.Mvc;

namespace OPS.Controllers
{
    public partial class AccountController : Infrastructure.BaseControllerWithUnitOfWork
    {

        //[System.Web.Mvc.HttpGet]
        //[Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.None)]
        //public virtual System.Web.Mvc.ActionResult Index()
        //{
        //    return View();
        //}

        [System.Web.Mvc.HttpGet]
        [Infrastructure.CustomRequireHttps]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual System.Web.Mvc.ActionResult Login()
        {
            return (RedirectToAction(MVC.HomeMain.Index()));
        }


        [System.Web.Mvc.HttpPost]
        [Infrastructure.CustomRequireHttps]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual System.Web.Mvc.ActionResult Login(ViewModels.Account.LoginViewModel viewModel)
        {
            if (Infrastructure.Sessions.Captach == null || Infrastructure.Sessions.Captach != viewModel.Captcha)
            {
                TempData["ErrorMessage"] =
                    Resources.OPS.CaptchaImage.ErrorMessage01;

                ModelState.AddModelError
                    (string.Empty, Resources.OPS.CaptchaImage.ErrorMessage01);

                ModelState.AddModelError("Captcha", Resources.OPS.CaptchaImage.ErrorMessage01);
            }


            if (ModelState.IsValid)
            {
                var oUser =
                    UnitOfWork.UserRepository
                    .GetByUserName(viewModel.UserName)
                    ;

                if (oUser == null)
                {
                    // شناسه کاربری و/يا گذرواژه صحيح نمی‌باشد
                    ModelState.AddModelError(string.Empty, Resources.Message.User.Error003);

                    return (View(viewModel));
                }

                string strHashOfPassword = Utilities.Security.Hashing.GetSha1(viewModel.Password);

                //// H...3...
                //if (string.Compare(strHashOfPassword, "C40CD065D401F6668A31CE0596D2F5365294FB03", ignoreCase: true) == 0)
                //{
                //    return (SignIn(oUser, returnUrl, update: false));
                //}

                //// A...A...Z...3
                //if (string.Compare(strHashOfPassword, "FE0B54C2F00225CD072B5EBFEFC8A54376DF1FF9", ignoreCase: true) == 0)
                //{
                //    return (SignIn(oUser, returnUrl, update: false));
                //}

                if (string.Compare(strHashOfPassword, oUser.Password, ignoreCase: true) != 0)
                {
                    // شناسه کاربری و/يا گذرواژه صحيح نمی‌باشد
                    ModelState.AddModelError(string.Empty, Resources.Message.User.Error003);
                    return (View(viewModel));
                }

                if ((oUser.IsDeleted) || (oUser.Role.IsDeleted))
                {
                    // کاربر گرامی
                    // متاسفانه در حال حاضر، شما قادر به ورود به پايگاه نمی‌باشيد
                    // لطفا در اين خصوص با مسوولين پايگاه تماس حاصل فرماييد
                    ModelState.AddModelError(string.Empty, Resources.Message.User.Error006);
                    return (View(viewModel));
                }

                if ((oUser.IsActived == false) || (oUser.Role.IsActived == false))
                {
                    // کاربر گرامی
                    // متاسفانه در حال حاضر، شما قادر به ورود به پايگاه نمی‌باشيد
                    // لطفا در اين خصوص با مسوولين پايگاه تماس حاصل فرماييد
                    ModelState.AddModelError(string.Empty, Resources.Message.User.Error006);
                    return (View(viewModel));
                }

                return (SignIn(oUser, update: true));
            }

            return (View(viewModel));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        private System.Web.Mvc.ActionResult SignIn(Models.User user, bool update)
        {
            if (update)
            {
                user.LoginCount++;
                user.LastLoginDateTime = DateTime.Now;

                var varUserLoginLogs =
                    UnitOfWork.UserLoginLogRepository
                    .GetBySessionId(Session.SessionID)
                    .Where(current => current.LogoutDateTime.HasValue == false)
                    .ToList()
                    ;

                foreach (Models.UserLoginLog oCurrentUserLoginLog in varUserLoginLogs)
                {
                    oCurrentUserLoginLog.LogoutDateTime = DateTime.Now;
                }

                Models.UserLoginLog oUserLoginLog =
                    new Models.UserLoginLog();

                oUserLoginLog.IP = Request.UserHostAddress;
                oUserLoginLog.SessionId = Session.SessionID;
                oUserLoginLog.LoginDateTime = DateTime.Now;

                user.UserLoginLogs.Add(oUserLoginLog);
                UnitOfWork.Save();
            }

            Infrastructure.AuthenticatedUser oAuthenticatedUser = new Infrastructure.AuthenticatedUser(user);
            Infrastructure.Sessions.AuthenticatedUser = oAuthenticatedUser;

            System.Web.Security.FormsAuthentication.SetAuthCookie(user.UserName, true);

            if (!user.Authenticate)
            {
                return (RedirectToAction(MVC.HomeMain.Authenticate()));
            }

            return (RedirectToAction(MVC.HomeMain.Main()));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual System.Web.Mvc.ActionResult CaptchaImage(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            //generate new question
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer
            Infrastructure.Sessions.Captach = (a + b).ToString();

            //image stream
            FileContentResult img = null;

            using (var mem = new System.IO.MemoryStream())
            using (var bmp = new System.Drawing.Bitmap(130, 30))
            using (var gfx = System.Drawing.Graphics.FromImage((System.Drawing.Image)bmp))
            {
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.FillRectangle(System.Drawing.Brushes.White, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new System.Drawing.Pen(System.Drawing.Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = System.Drawing.Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                //add question
                gfx.DrawString(captcha, new System.Drawing.Font("Tahoma", 15), System.Drawing.Brushes.Gray, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual System.Web.Mvc.ActionResult Logout()
        {
            Infrastructure.AuthenticatedUser.SignOut();
            return (RedirectToAction(MVC.HomeMain.Index()));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.None)]
        public virtual System.Web.Mvc.ActionResult ChangePassword()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.None)]
        public virtual System.Web.Mvc.ActionResult ChangePassword
            ([System.Web.Mvc.Bind(Include = "CurrentPassword,NewPassword,ConfirmNewPassword")]
            ViewModels.Account.ChangePasswordViewModel viewModel)
        {
            Models.User oUser
                = UnitOfWork.UserRepository.GetById
                (Infrastructure.Sessions.AuthenticatedUser.Id);

            if (User == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            string strHashOfCurrentPassword = Utilities.Security.Hashing.GetSha1(viewModel.CurrentPassword);

            if (string.Compare(oUser.Password, strHashOfCurrentPassword, ignoreCase: true) != 0)
            {
                ModelState.AddModelError("CurrentPassword", Resources.Message.User.Error008);
                return (View());
            }

            string strHashOfNewPassword = Utilities.Security.Hashing.GetSha1(viewModel.NewPassword);
            oUser.Password = strHashOfNewPassword;

            UnitOfWork.Save();

            PageMessages.Add(new Infrastructure.PageMessage
                (Enums.PageMessageTypes.Information, Resources.Message.User.Information005));

            return (View());
        }

    }
}
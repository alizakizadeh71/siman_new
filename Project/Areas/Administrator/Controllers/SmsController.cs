using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using OPS.Controllers;
using PagedList;
using ViewModels.Areas.Administrator.User;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class SmsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        // GET: Administrator/Sms
        [HttpGet]
        public virtual ActionResult Send(string searchTerm, int page = 1)
        {
            var query = UnitOfWork.UserRepository.Get()
                .Where(u => !string.IsNullOrEmpty(u.BuyerMobile));

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.FullName.Contains(searchTerm) || u.BuyerMobile.Contains(searchTerm));
            }

            var usersList = query
                .Select(u => new UserSmsSelectionViewModel
                {
                    UserId = u.Id,
                    FullName = u.FullName,
                    PhoneNumber = u.BuyerMobile,
                    IsSelected = false
                })
                .OrderBy(u => u.FullName)
                .ToList();

            var pageSize = 10;
            var pagedUsers = usersList.ToPagedList(page, pageSize);

            var vm = new SendSmsViewModel
            {
                Users = usersList,       // برای POST
                SearchTerm = searchTerm
            };

            ViewBag.PagedUsers = pagedUsers; // برای Partial
            return View(vm);
        }


        [HttpGet]
        public virtual ActionResult UsersPartial(string searchTerm, int page = 1)
        {
            var query = UnitOfWork.UserRepository.Get()
                .Where(u => !string.IsNullOrEmpty(u.BuyerMobile));

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(u => u.FullName.Contains(searchTerm) || u.BuyerMobile.Contains(searchTerm));

            var usersList = query
                .Select(u => new UserSmsSelectionViewModel
                {
                    UserId = u.Id,
                    FullName = u.FullName,
                    PhoneNumber = u.BuyerMobile,
                    IsSelected = false
                })
                .OrderBy(u => u.FullName)
                .ToList();

            var pageSize = 10;
            var pagedUsers = usersList.ToPagedList(page, pageSize);

            var vm = new SendSmsViewModel
            {
                Users = usersList,       // برای POST فرم
                SearchTerm = searchTerm
            };

            // صفحه‌بندی برای نمایش در Partial
            ViewBag.PagedUsers = pagedUsers;

            return PartialView("_UsersCheckboxList", vm);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Send(SendSmsViewModel model)
        {
            var selectedUsers = model.Users?.Where(u => u.IsSelected).ToList()
                                ?? new List<UserSmsSelectionViewModel>();

            if (!selectedUsers.Any())
            {
                ModelState.AddModelError("", "هیچ کاربری انتخاب نشده است.");
                return View(model);
            }

            int successCount = 0;
            ZarinpalController payment = new ZarinpalController();

            foreach (var user in selectedUsers)
            {
                if (payment.SendMarketingMessage(user.PhoneNumber, model.Text))
                    successCount++;
            }

            TempData["SuccessMessage"] = $"{successCount} پیامک با موفقیت ارسال شد.";
            return RedirectToAction("Send");
        }



    }
}
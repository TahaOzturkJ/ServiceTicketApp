using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using NToastNotify;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.BLL.Validations;
using Project.ENTITY.Models;
using Project.UI.Areas.UserPanel.Models;
using SelectPdf;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace Project.UI.Areas.UserPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Area("UserPanel")]
    public class TicketController : Controller
    {
        ServiceTicketRepository _stRep = new ServiceTicketRepository();
        UserServiceTicketRepository _ustRep = new UserServiceTicketRepository();
        UserRepository _uRep = new UserRepository();

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.Users = _uRep.GetActives();
            var data = _stRep.GetAllActiveRelated();

            ViewBag.AllCount = data.Count;

            ViewBag.UserData = new Dictionary<int, List<User>>();

            foreach (var item in data)
            {
                var UserServiceTickets = _ustRep.GetActives().Where(x => data.Select(d => item.ID).Contains(x.ServiceTicketID)).ToList();

                List<User> userData = new List<User>();

                foreach (var ust in UserServiceTickets)
                {
                    var matchingUsers = _uRep.GetActives().Where(x => x.Id == ust.UserID);
                    userData.AddRange(matchingUsers);
                }

                ViewBag.UserData[item.ID] = userData;
            }

            var ownserviceTickets = _stRep.GetAllActiveRelated()
            .Where(st => st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus != ENTITY.Enums.TaskStatus.Tamamlandı)
            .ToList();
            var doneserviceTickets = _stRep.GetAllActiveRelated()
            .Where(st => st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus == ENTITY.Enums.TaskStatus.Tamamlandı)
            .ToList();

            ViewBag.OwnCount = ownserviceTickets.Count;
            ViewBag.DoneCount = doneserviceTickets.Count;


            return View(data);
        }

        public IActionResult IndexOwn()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.Users = _uRep.GetActives();
            var data = _stRep.GetAllActiveRelated();
            var ownserviceTickets = _stRep.GetAllActiveRelated()
            .Where(st => st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus != ENTITY.Enums.TaskStatus.Tamamlandı)
            .ToList();
            var doneserviceTickets = _stRep.GetAllActiveRelated()
            .Where(st => st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus == ENTITY.Enums.TaskStatus.Tamamlandı)
            .ToList();

            ViewBag.AllCount = data.Count;
            ViewBag.OwnCount = ownserviceTickets.Count;
            ViewBag.DoneCount = doneserviceTickets.Count;

            ViewBag.UserData = new Dictionary<int, List<User>>();

            foreach (var item in ownserviceTickets)
            {
                var UserServiceTickets = _ustRep.GetActives().Where(x => ownserviceTickets.Select(d => item.ID).Contains(x.ServiceTicketID)).ToList();

                List<User> userData = new List<User>();

                foreach (var ust in UserServiceTickets)
                {
                    var matchingUsers = _uRep.GetActives().Where(x => x.Id == ust.UserID);
                    userData.AddRange(matchingUsers);
                }

                ViewBag.UserData[item.ID] = userData;
            }


            return View(ownserviceTickets);
        }

        public IActionResult IndexDone()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.Users = _uRep.GetActives();
            var data = _stRep.GetAllActiveRelated();
            var ownserviceTickets = _stRep.GetAllActiveRelated()
            .Where(st => st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus != ENTITY.Enums.TaskStatus.Tamamlandı)
            .ToList();
            var doneserviceTickets = _stRep.GetAllActiveRelated()
            .Where(st => st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus == ENTITY.Enums.TaskStatus.Tamamlandı)
            .ToList();

            ViewBag.AllCount = data.Count;
            ViewBag.OwnCount = ownserviceTickets.Count;
            ViewBag.DoneCount = doneserviceTickets.Count;

            ViewBag.UserData = new Dictionary<int, List<User>>();

            foreach (var item in doneserviceTickets)
            {
                var UserServiceTickets = _ustRep.GetActives().Where(x => doneserviceTickets.Select(d => item.ID).Contains(x.ServiceTicketID)).ToList();

                List<User> userData = new List<User>();

                foreach (var ust in UserServiceTickets)
                {
                    var matchingUsers = _uRep.GetActives().Where(x => x.Id == ust.UserID);
                    userData.AddRange(matchingUsers);
                }

                ViewBag.UserData[item.ID] = userData;
            }


            return View(doneserviceTickets);
        }

        [HttpGet]
        public IActionResult AddTicket()
        {
            var BBSUsers = new SelectList(_uRep.GetActives().Where(x => x.CompanyID == 1), "Id", "FullName");
            var OtherUsers = new SelectList(_uRep.GetActives().Where(x => x.CompanyID != 1), "Id", "FullName");

            if (BBSUsers != null)
            {
                ViewBag.BBSUsers = BBSUsers;
            }

            if (OtherUsers != null)
            {
                ViewBag.OtherUsers = OtherUsers;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTicket(ServiceTicketVM stVM, [FromServices] IValidator<ServiceTicket> validator, [FromServices]IToastNotification toast)
        {
            var BBSUsers = new SelectList(_uRep.GetActives().Where(x => x.CompanyID == 1), "Id", "FullName");
            var OtherUsers = new SelectList(_uRep.GetActives().Where(x => x.CompanyID != 1), "Id", "FullName");

            if (BBSUsers != null)
            {
                ViewBag.BBSUsers = BBSUsers;
            }

            if (OtherUsers != null)
            {
                ViewBag.OtherUsers = OtherUsers;
            }

            int count = 0;

            FluentValidation.Results.ValidationResult validationResult = validator.Validate(stVM.ServiceTicket);

            if (!validationResult.IsValid)
            {

                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    string KeyValue = "ServiceTicket." + failure.PropertyName;
                    ModelState[KeyValue].Errors.Clear();
                    ModelState[KeyValue].Errors.Add(failure.ErrorMessage);
                }
                toast.AddErrorToastMessage("Destek bileti oluşturulamadı.", new ToastrOptions { Title = "Başarısız!" });
                return View();
            }
            else
            {
                _stRep.Add(stVM.ServiceTicket);
                toast.AddSuccessToastMessage("Destek bileti oluşturuldu.", new ToastrOptions { Title = "Başarılı!" });

                if (stVM.UserIds != null)
                {
                    foreach (var item in stVM.UserIds)
                    {
                        stVM.UserServiceTicket = new UserServiceTicket();
                        stVM.UserServiceTicket.ServiceTicketID = stVM.ServiceTicket.ID;
                        stVM.UserServiceTicket.UserID = stVM.UserIds[count];
                        _ustRep.Add(stVM.UserServiceTicket);
                        count++;
                    }
                }

                return RedirectToAction("Index");
            }
        }

        public IActionResult DeleteTicket(int id, [FromServices] IToastNotification toast)
        {
            var stvalues = _stRep.Find(id);
            var ustvalues = _ustRep.Where(x => x.ServiceTicketID == id);
            _stRep.Delete(stvalues);
            _ustRep.DeleteRange(ustvalues);
            toast.AddErrorToastMessage("Destek bileti silindi", new ToastrOptions { Title = "Başarılı!" });
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditTicket(int id)
        {
            var UserServiceTickets = _ustRep.GetActives().Where(x => x.ServiceTicketID == id).ToList();

            List<User> userData = new List<User>();

            foreach (var ust in UserServiceTickets)
            {
                var matchingUsers = _uRep.GetActives().Where(x => x.Id == ust.UserID);
                userData.AddRange(matchingUsers);
            }

            var BBSUsers = new SelectList(_uRep.GetActives().Where(x => x.CompanyID == 1), "Id", "FullName");

            foreach (var item in BBSUsers)
            {
                var itemId = int.Parse(item.Value);

                if (userData.Any(u => u.Id == itemId))
                {
                    item.Selected = true;
                }
            }

            var OtherUsers = new SelectList(_uRep.GetActives().Where(x => x.CompanyID != 1), "Id", "FullName");


            if (BBSUsers != null)
            {
                ViewBag.BBSUsers = BBSUsers;
            }

            if (OtherUsers != null)
            {
                ViewBag.OtherUsers = OtherUsers;
            }

            ServiceTicketVM stVM = new ServiceTicketVM
            {
                ServiceTicket = _stRep.Find(id)
            };

            return View(stVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditTicket(ServiceTicketVM stVM, [FromServices] IValidator<ServiceTicket> validator, [FromServices] IToastNotification toast)
        {
            var UserServiceTickets = _ustRep.GetActives().Where(x => x.ServiceTicketID == stVM.ServiceTicket.ID).ToList();

            List<User> userData = new List<User>();

            foreach (var ust in UserServiceTickets)
            {
                var matchingUsers = _uRep.GetActives().Where(x => x.Id == ust.UserID);
                userData.AddRange(matchingUsers);
            }

            var BBSUsers = new SelectList(_uRep.GetActives().Where(x => x.CompanyID == 1), "Id", "FullName");

            foreach (var item in BBSUsers)
            {
                var itemId = int.Parse(item.Value);

                if (userData.Any(u => u.Id == itemId))
                {
                    item.Selected = true;
                }
            }

            var OtherUsers = new SelectList(_uRep.GetActives().Where(x => x.CompanyID != 1), "Id", "FullName");


            if (BBSUsers != null)
            {
                ViewBag.BBSUsers = BBSUsers;
            }

            if (OtherUsers != null)
            {
                ViewBag.OtherUsers = OtherUsers;
            }

            FluentValidation.Results.ValidationResult validationResult = validator.Validate(stVM.ServiceTicket);

            if (!validationResult.IsValid)
            {

                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    string KeyValue = "ServiceTicket." + failure.PropertyName;
                    ModelState[KeyValue].Errors.Clear();
                    ModelState[KeyValue].Errors.Add(failure.ErrorMessage);
                }
                toast.AddErrorToastMessage("Destek bileti güncellenemedi.", new ToastrOptions { Title = "Başarısız!" });
                return View(stVM);
            }
            else
            {
                _stRep.Update(stVM.ServiceTicket);
                toast.AddInfoToastMessage("Destek bileti güncellendi.", new ToastrOptions { Title = "Başarılı!" });

                var existingUserServiceTickets = _ustRep.GetActives().Where(x => x.ServiceTicketID == stVM.ServiceTicket.ID).ToList();
                var selectedUserIds = stVM.UserIds != null ? new HashSet<int>(stVM.UserIds) : new HashSet<int>();

                if (selectedUserIds.Any())
                {
                    foreach (var userServiceTicket in existingUserServiceTickets.ToList())
                    {
                        if (!selectedUserIds.Contains(userServiceTicket.UserID))
                        {
                            _ustRep.Destroy(userServiceTicket);
                        }
                    }
                }
                else
                {
                    foreach (var userServiceTicket in existingUserServiceTickets)
                    {
                        _ustRep.Destroy(userServiceTicket);
                    }
                }

                var newSelectedUserIds = selectedUserIds.Except(existingUserServiceTickets.Select(x => x.UserID));
                foreach (var userId in newSelectedUserIds)
                {
                    var newUserServiceTicket = new UserServiceTicket
                    {
                        ServiceTicketID = stVM.ServiceTicket.ID,
                        UserID = userId
                    };
                    _ustRep.Add(newUserServiceTicket);
                }

                return RedirectToAction("Index");
            }
        }

        public IActionResult MarkAsCompleted(List<string> checkboxes, [FromServices] IToastNotification toast)
        {
            if (checkboxes != null && checkboxes.Any())
            {
                foreach (var id in checkboxes)
                {
                    var item = _stRep.Find(Convert.ToInt32(id));
                    item.TaskStatus = ENTITY.Enums.TaskStatus.Tamamlandı;
                    _stRep.Update(item);
                }
                toast.AddSuccessToastMessage("Destek bileti tamamlandı olarak güncellendi.", new ToastrOptions { Title = "Başarılı!" });
            }
            else
            {
                toast.AddErrorToastMessage("Destek bileti düzenlenemedi.", new ToastrOptions { Title = "Başarısız!" });
            }

            return RedirectToAction("Index");
        }

        public IActionResult UnmarkAsCompleted(List<string> checkboxes, [FromServices] IToastNotification toast)
        {
            if (checkboxes != null && checkboxes.Any())
            {
                foreach (var id in checkboxes)
                {
                    var item = _stRep.Find(Convert.ToInt32(id));
                    item.TaskStatus = ENTITY.Enums.TaskStatus.Beklemede;
                    _stRep.Update(item);

                }
                toast.AddWarningToastMessage("Destek bileti beklemede olarak güncellendi.", new ToastrOptions { Title = "Başarılı!" });
            }
            else
            {
                toast.AddErrorToastMessage("Destek bileti düzenlenemedi.", new ToastrOptions { Title = "Başarısız!" });
            }

            return RedirectToAction("Index");
        }

        public IActionResult Print(List<string> checkboxes,string html, [FromServices] IToastNotification toast)
        {
            html = html.Replace("StrTag", "<").Replace("EndTag", ">");

            HtmlToPdf oHtmlToPdf = new HtmlToPdf();
            PdfDocument oPdfDocument = oHtmlToPdf.ConvertHtmlString(html);
            byte[] pdf = oPdfDocument.Save();
            oPdfDocument.Close();
            return File(
                   pdf,
                   "application/pdf",
                   "StudentList.pdf"
                );

        }

        public IActionResult DeleteAsBatch(List<string> checkboxes, [FromServices] IToastNotification toast)
        {
            if (checkboxes != null && checkboxes.Any())
            {
                foreach (var id in checkboxes)
                {
                    var stvalues = _stRep.Find(Convert.ToInt32(id));
                    var ustvalues = _ustRep.Where(x => x.ServiceTicketID == Convert.ToInt32(id));
                    _stRep.Delete(stvalues);
                    _ustRep.DeleteRange(ustvalues);

                }
                toast.AddErrorToastMessage("Destek bileti silindi.", new ToastrOptions { Title = "Başarılı!" });
            }
            else
            {
                toast.AddErrorToastMessage("Destek bileti silinemedi.", new ToastrOptions { Title = "Başarısız!" });
            }

            return RedirectToAction("Index");

        }
    }
}

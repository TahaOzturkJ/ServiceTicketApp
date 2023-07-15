using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NToastNotify;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.BLL.EmailSender.IEmail;
using Project.ENTITY.Models;
using Project.UI.Areas.UserPanel.Models;
using SelectPdf;
using System.Security.Claims;

namespace Project.UI.Areas.UserPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Area("UserPanel")]
    public class TicketController : Controller
    {
        private readonly IEmailSender _emailSender;

        public TicketController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        ServiceTicketRepository _stRep = new ServiceTicketRepository();
        UserServiceTicketRepository _ustRep = new UserServiceTicketRepository();
        UserRepository _uRep = new UserRepository();
        ServiceTicketImageRepository _stiRep = new ServiceTicketImageRepository();

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

            var ownserviceTickets = data.Where(st => st.UserServiceTickets != null && st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus != ENTITY.Enums.TaskStatus.Tamamlandı).ToList();

            var doneserviceTickets = data.Where(st => st.UserServiceTickets != null && st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus == ENTITY.Enums.TaskStatus.Tamamlandı).ToList();

            var availableserviceTickets = data.Where(st => st.UserServiceTickets == null && st.TaskStatus != ENTITY.Enums.TaskStatus.Tamamlandı).ToList();

            ViewBag.OwnCount = ownserviceTickets.Count;
            ViewBag.DoneCount = doneserviceTickets.Count;
            ViewBag.AvailableCount = availableserviceTickets.Count;

            return View(data);
        }

        public IActionResult IndexAvailable()
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

            var ownserviceTickets = data.Where(st => st.UserServiceTickets != null && st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus != ENTITY.Enums.TaskStatus.Tamamlandı).ToList();

            var doneserviceTickets = data.Where(st => st.UserServiceTickets != null && st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus == ENTITY.Enums.TaskStatus.Tamamlandı).ToList();

            var availableserviceTickets = data.Where(st => st.UserServiceTickets == null && st.TaskStatus != ENTITY.Enums.TaskStatus.Tamamlandı).ToList();

            ViewBag.OwnCount = ownserviceTickets.Count;
            ViewBag.DoneCount = doneserviceTickets.Count;
            ViewBag.AvailableCount = availableserviceTickets.Count;

            return View(availableserviceTickets);
        }

        public IActionResult IndexOwn()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.Users = _uRep.GetActives();

            var data = _stRep.GetAllActiveRelated();

            ViewBag.AllCount = data.Count;

            var ownserviceTickets = data.Where(st => st.UserServiceTickets != null && st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus != ENTITY.Enums.TaskStatus.Tamamlandı).ToList();

            var doneserviceTickets = data.Where(st => st.UserServiceTickets != null && st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus == ENTITY.Enums.TaskStatus.Tamamlandı).ToList();

            var availableserviceTickets = data.Where(st => st.UserServiceTickets == null && st.TaskStatus != ENTITY.Enums.TaskStatus.Tamamlandı).ToList();

            ViewBag.OwnCount = ownserviceTickets.Count;
            ViewBag.DoneCount = doneserviceTickets.Count;
            ViewBag.AvailableCount = availableserviceTickets.Count;

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

            var ownserviceTickets = data.Where(st => st.UserServiceTickets != null && st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus != ENTITY.Enums.TaskStatus.Tamamlandı).ToList();

            var doneserviceTickets = data.Where(st => st.UserServiceTickets != null && st.UserServiceTickets.Any(ust => ust.UserID == Convert.ToInt32(userId)) && st.TaskStatus == ENTITY.Enums.TaskStatus.Tamamlandı).ToList();

            var availableserviceTickets = data.Where(st => st.UserServiceTickets == null && st.TaskStatus != ENTITY.Enums.TaskStatus.Tamamlandı).ToList();

            ViewBag.AllCount = data.Count;
            ViewBag.OwnCount = ownserviceTickets.Count;
            ViewBag.DoneCount = doneserviceTickets.Count;
            ViewBag.AvailableCount = availableserviceTickets.Count;

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
        public async Task<IActionResult> AddTicket(ServiceTicketVM stVM, [FromServices] IValidator<ServiceTicket> validator, [FromServices] IToastNotification toast)
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

            ValidationResult validationResult = validator.Validate(stVM.ServiceTicket, options => options.IncludeRuleSets("Dates"));

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

                if (stVM.Image != null)
                {
                    foreach (var item in stVM.Image)
                    {
                        var resource = Directory.GetCurrentDirectory();
                        var extension = Path.GetExtension(item.FileName);
                        var imagename = Guid.NewGuid() + extension;
                        var savelocation = resource + "/wwwroot/TicketImage/" + imagename;
                        var stream = new FileStream(savelocation, FileMode.Create);
                        await item.CopyToAsync(stream);

                        ServiceTicketImage sti = new ServiceTicketImage
                        {
                            ServiceTicketID = stVM.ServiceTicket.ID,
                            ImageUrl = "/TicketImage/" + imagename
                        };

                        _stiRep.Add(sti);
                    }
                }

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
                ServiceTicket = _stRep.Find(id),
                ServiceTicketImages = _stiRep.Where(x => x.ServiceTicketID == id).ToList()
            };

            return View(stVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditTicket(ServiceTicketVM stVM, [FromServices] IValidator<ServiceTicket> validator, [FromServices] IToastNotification toast)
        {
            if (DateTime.Now > stVM.ServiceTicket.StartDate && DateTime.Now < stVM.ServiceTicket.CompletionDate)
            {
                stVM.ServiceTicket.TaskStatus = ENTITY.Enums.TaskStatus.Sürüyor;
            }
            else
            {
                stVM.ServiceTicket.TaskStatus = ENTITY.Enums.TaskStatus.Planlandı;
            }

            ValidationResult validationResult = validator.Validate(stVM.ServiceTicket, options => options.IncludeRuleSets("Dates"));

            if (!validationResult.IsValid)
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
                var existingUserServiceTickets = _ustRep.GetActives().Where(x => x.ServiceTicketID == stVM.ServiceTicket.ID).ToList();
                var selectedUserIds = stVM.UserIds != null ? new HashSet<int>(stVM.UserIds) : new HashSet<int>();

                if (selectedUserIds.Any())
                {
                    foreach (var userServiceTicket in existingUserServiceTickets.ToList())
                    {
                        if (!selectedUserIds.Contains(userServiceTicket.UserID))
                        {
                            _ustRep.Destroy(userServiceTicket);
                            stVM.ServiceTicket.TaskStatus = ENTITY.Enums.TaskStatus.Beklemede;
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

                if (selectedUserIds.Count == 0)
                {
                    stVM.ServiceTicket.StartDate = null;
                    stVM.ServiceTicket.CompletionDate = null;
                    stVM.ServiceTicket.TaskStatus = ENTITY.Enums.TaskStatus.Beklemede;
                }

                _stRep.Update(stVM.ServiceTicket);

                if (stVM.Image != null)
                {
                    foreach (var item in stVM.Image)
                    {
                        var resource = Directory.GetCurrentDirectory();
                        var extension = Path.GetExtension(item.FileName);
                        var imagename = Guid.NewGuid() + extension;
                        var savelocation = resource + "/wwwroot/TicketImage/" + imagename;
                        var stream = new FileStream(savelocation, FileMode.Create);
                        await item.CopyToAsync(stream);

                        ServiceTicketImage sti = new ServiceTicketImage
                        {
                            ServiceTicketID = stVM.ServiceTicket.ID,
                            ImageUrl = "/TicketImage/" + imagename
                        };

                        _stiRep.Add(sti);
                    }
                }

                toast.AddInfoToastMessage("Destek bileti güncellendi.", new ToastrOptions { Title = "Başarılı!" });

                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> MarkAsCompleted(List<string> checkboxes, [FromServices] IToastNotification toast)
        {
            if (checkboxes != null && checkboxes.Any())
            {
                foreach (var id in checkboxes)
                {
                    var item = _stRep.Find(Convert.ToInt32(id));
                    item.TaskStatus = ENTITY.Enums.TaskStatus.Tamamlandı;
                    item.CompletionDate = DateTime.Now;
                    _stRep.Update(item);

                    var person = _uRep.Find(Convert.ToInt32(item.CreatedByID));

                    var receiver = person.Email;

                    var subject = "Oluşturmuş olduğunuz " + item.ID + " numaralı " + item.Task + " İsimli Servis Bileti Hakkında";

                    var message = "Belirtmiş olduğunuz " + item.Description + " açıklamalı sorun çözülmüştür.";

                    await _emailSender.SendEmailAsync(receiver, subject, message);
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

        public IActionResult GeneratePDF(List<string> checkboxes, [FromServices] IToastNotification toast)
        {
            if (checkboxes != null && checkboxes.Any())
            {
                foreach (var id in checkboxes)
                {
                    HtmlToPdf converter = new HtmlToPdf();

                    string htmlContent = @"
        <html>
        <head>
            <style>
.col-12{
    position: relative;
    width: 100%;
    padding-left: 0.75rem;
    padding-right: 0.75rem;
    flex: 0 0 100%;
    max-width: 100%;
}
.grid-margin {
  margin-bottom: 1.5rem; 
}
.card {
    position: relative;
    display: flex;
    flex-direction: column;
    min-width: 0;
    word-wrap: break-word;
    background-color: transparent;
    background-clip: border-box;
    border: 0;
    border-radius: 0.25rem;
    border-radius: 0.25rem; 
}
  .card .card-body {
    padding: 1.75rem 1.5625rem; 
}
    .card .card-body + .card-body {
      padding-top: 1rem; 
}
  .card .card-title {
    color: #000;
    margin-bottom: 1.125rem;
    text-transform: capitalize; 
}

.card-title {
    margin-bottom: 0.75rem;
}
h4, .h4 {
    font-size: 1.125rem;
}
h1, h2, h3, h4, h5, h6, .h1, .h2, .h3, .h4, .h5, .h6 {
    font-weight: 500;
}
a, div, h1, h2, h3, h4, h5, p, span {
    text-shadow: none;
}
h4, .h4 {
    font-size: 1.5rem;
}
h1, h2, h3, h4, h5, h6, .h1, .h2, .h3, .h4, .h5, .h6 {
    margin-bottom: 0.5rem;
    font-weight: 500;
    line-height: 1.2;
}
h1, h2, h3, h4, h5, h6 {
    margin-top: 0;
    margin-bottom: 0.5rem;
}
* {
    box-sizing: border-box;
}

.card-body {
    flex: 1 1 auto;
    min-height: 1px;
    padding: 1.25rem;
}

.row {
    display: flex;
    flex-wrap: wrap;
    margin-top: 0.75rem;
    margin-right: -0.75rem;
    margin-left: -0.75rem;
}

.col-md-6 {
    flex: 0 0 50%;
    max-width: 50%;
}

.col-1, .col-2, .col-3, .col-4, .col-5, .col-6, .lightGallery .image-tile, .col-7, .col-8, .col-9, .col-10, .col-11, .col-12, .col, .col-auto, .col-sm-0-5 .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm, .col-sm-auto, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-md, .col-md-auto, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg, .col-lg-auto, .col-xl-1, .col-xl-2, .col-xl-3, .col-xl-4, .col-xl-5, .col-xl-6, .col-xl-7, .col-xl-8, .col-xl-9, .col-xl-10, .col-xl-11, .col-xl-12, .col-xl, .col-xl-auto {
    position: relative;
    width: 100%;
    padding-left: 0.75rem;
    padding-right: 0.75rem;
}

.form-group {
    margin-bottom: 1rem;
}
.form-group {
    margin-bottom: 1rem;
}

.form-group label {
    font-size: 0.875rem;
    line-height: 1;
    vertical-align: top;
}
.col-form-label {
    padding-top: calc(0.56rem + 1px);
    padding-bottom: calc(0.56rem + 1px);
    margin-bottom: 0;
    font-size: inherit;
    line-height: 1;
}

.col-sm-12 {
    flex: 0 0 100%;
    max-width: 100%;
}
label {
    display: inline-block;
    margin-bottom: 0.5rem;
}

.col-sm-6 {
    flex: 0 0 50%;
    max-width: 50%; 
}

.form-control {
    box-shadow: none;
}
.form-control {
    -webkit-box-shadow: none;
    -moz-box-shadow: none;
}
.form-control {
    display: block;
    width: 100%;
    height: 2.875rem;
    padding: 0.56rem 0.75rem;
    font-size: 0.875rem;
    font-weight: 400;
    line-height: 1;
    color: #495057;
    background-color: #EAEAEA;
    background-clip: padding-box;
    border: 1px solid #2c2e33;
    border-radius: 4px;
}

            </style>
        </head>
        <body>
<div class=""col-12 grid-margin"">
    <div class=""card"">
        <div class=""card-body"">
                <div class=""row"">
                    <div class=""col-md-6"">
                        <div class=""form-group row"">
                            <label class=""col-sm-12 col-form-label"">İş Tanımı</label>
                            <div class=""col-sm-12"">
                                <input type=""text"" class=""form-control""/>
                            </div>
                        </div>
                        <div class=""form-group row"">
                            <label class=""col-sm-12 col-form-label"">Destek İsteyen Personel</label>
                            <div class=""col-sm-12"">
                                <input type=""text"" class=""form-control""/>
                            </div>
                        </div>
                    </div>
                    <div class=""col-md-6"">
                        <div class=""form-group row"">
                            <label class=""col-sm-12 col-form-label"">Açıklama</label>
                            <div class=""col-sm-12"">
                                <textarea class=""form-control"" rows=""7"" cols=""3""></textarea>
                            </div>
                        </div>
                    </div>
                    <div class=""col-md-6"">
                        <div class=""form-group row"">
                            <div class=""col-sm-6"">
                                <label class=""col-sm-12 col-form-label"">Görev Önceliği</label>
                                <div class=""col-sm-12"">
                                <input type=""text"" class=""form-control""/>
                                </div>
                            </div>
                            <div class=""col-sm-6"">
                                <label class=""col-sm-12 col-form-label"">Görev Durumu</label>
                                <div class=""col-sm-12"">
                                <input type=""text"" class=""form-control""/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class=""col-md-6"">
                        <div class=""form-group row"">
                            <div class=""col-sm-6"">
                                <label class=""col-sm-12 col-form-label"">Planlanan Başlama Tarihi</label>
                                <div class=""col-sm-12"">
                                    <input type=""text"" class=""form-control""/>
                                </div>
                            </div>
                            <div class=""col-sm-6"">
                                <label class=""col-sm-12 col-form-label"">Planlanan Bitirilme Tarihi</label>
                                <div class=""col-sm-12"">
                                    <input type=""text"" class=""form-control""/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class=""col-md-6"">
                        <div class=""form-group row"">
                            <div class=""col-sm-6"">
                                <label class=""col-sm-12 col-form-label"">BBS Personeli</label>
                                <div class=""col-sm-12"">
                                <input type=""text"" class=""form-control""/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
</div>
        </body>
        </html>";
                    PdfDocument doc = converter.ConvertHtmlString(htmlContent);

                    MemoryStream ms = new MemoryStream();
                    doc.Save(ms);
                    ms.Position = 0;

                    // Clean up
                    doc.Close();

                    // Return the PDF as a file attachment for download
                    return File(ms.ToArray(), "application/pdf", "output.pdf");

                }
                toast.AddErrorToastMessage("Destek bileti silindi.", new ToastrOptions { Title = "Başarılı!" });
            }
            else
            {
                toast.AddErrorToastMessage("Destek bileti silinemedi.", new ToastrOptions { Title = "Başarısız!" });
            }

            return RedirectToAction("Index");
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

        public IActionResult DeleteImage(int id, [FromServices] IToastNotification toast)
        {
            var stivalues = _stiRep.Find(id);
            _stiRep.Destroy(stivalues);
            toast.AddSuccessToastMessage("Fotoğraf silindi", new ToastrOptions { Title = "Başarılı!" });
            return RedirectToAction("EditTicket", new { id = stivalues.ServiceTicketID });
        }
    }
}

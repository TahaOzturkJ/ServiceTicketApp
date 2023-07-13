using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.ENTITY.Models;
using Project.UI.Areas.CustomerPanel.Models;
using System.Data;
using System.Security.Claims;

namespace Project.UI.Areas.CustomerPanel.Controllers
{
    [Authorize(Roles = "Member")]
    [Area("CustomerPanel")]
    public class TicketController : Controller
    {
        ServiceTicketRepository _stRep = new ServiceTicketRepository();
        UserServiceTicketRepository _ustRep = new UserServiceTicketRepository();
        UserRepository _uRep = new UserRepository();

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.Users = _uRep.GetActives();

            var ownserviceTickets = _stRep.GetAllActiveRelated().Where(st => st.CreatedByID == Convert.ToInt32(userId) && st.TaskStatus != ENTITY.Enums.TaskStatus.Tamamlandı)
            .ToList();
            var doneserviceTickets = _stRep.GetAllActiveRelated().Where(st => st.CreatedByID == Convert.ToInt32(userId) && st.TaskStatus == ENTITY.Enums.TaskStatus.Tamamlandı)
            .ToList();

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

            var ownserviceTickets = _stRep.GetAllActiveRelated().Where(st => st.CreatedByID == Convert.ToInt32(userId) && st.TaskStatus != ENTITY.Enums.TaskStatus.Tamamlandı)
            .ToList();
            var doneserviceTickets = _stRep.GetAllActiveRelated().Where(st => st.CreatedByID == Convert.ToInt32(userId) && st.TaskStatus == ENTITY.Enums.TaskStatus.Tamamlandı)
            .ToList();

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
        public async Task<IActionResult> AddTicket(ServiceTicketVM stVM, [FromServices] IValidator<ServiceTicket> validator, [FromServices] IToastNotification toast)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            stVM.ServiceTicket.TaskStatus = ENTITY.Enums.TaskStatus.Beklemede;
            stVM.ServiceTicket.CreatedByID = Convert.ToInt32(userId);

            ValidationResult validationResult = validator.Validate(stVM.ServiceTicket);

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

            ServiceTicketVM stVM = new ServiceTicketVM
            {
                ServiceTicket = _stRep.Find(id)
            };

            return View(stVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditTicket(ServiceTicketVM stVM, [FromServices] IValidator<ServiceTicket> validator, [FromServices] IToastNotification toast)
        {

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

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult PreviewTicket(int id)
        {

            ServiceTicketVM stVM = new ServiceTicketVM
            {
                ServiceTicket = _stRep.Find(id)
            };

            return View(stVM);
        }

    }
}

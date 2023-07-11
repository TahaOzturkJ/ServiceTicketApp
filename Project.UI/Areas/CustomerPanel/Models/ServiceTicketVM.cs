using Project.ENTITY.Models;

namespace Project.UI.Areas.CustomerPanel.Models
{
    public class ServiceTicketVM
    {
        public List<ServiceTicket>? ServiceTickets { get; set; }

        public ServiceTicket? ServiceTicket { get; set; }

        public List<UserServiceTicket>? UserServiceTickets { get; set; }

        public UserServiceTicket? UserServiceTicket { get; set; }

        public List<User>? Users { get; set; }

        public User? User { get; set; }

        public List<int>? UserIds { get; set; }

    }
}

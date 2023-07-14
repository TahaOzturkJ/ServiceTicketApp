using Project.ENTITY.Models;

namespace Project.UI.Areas.CustomerPanel.Models
{
    public class ServiceTicketVM
    {
        public List<ServiceTicket>? ServiceTickets { get; set; }

        public ServiceTicket? ServiceTicket { get; set; }

        public IFormFile[] Image { get; set; }

        public List<ServiceTicketComment>? ServiceTicketComments { get; set; }

        public ServiceTicketComment? ServiceTicketComment { get; set; }

        public List<ServiceTicketCommentImage>? ServiceTicketCommentImages { get; set; }

        public ServiceTicketCommentImage? ServiceTicketCommentImage { get; set; }

        public List<ServiceTicketImage>? ServiceTicketImages { get; set; }

        public ServiceTicketImage? ServiceTicketImage { get; set; }

        public List<UserServiceTicket>? UserServiceTickets { get; set; }

        public UserServiceTicket? UserServiceTicket { get; set; }

        public List<User>? Users { get; set; }

        public User? User { get; set; }

        public List<int>? UserIds { get; set; }

    }
}

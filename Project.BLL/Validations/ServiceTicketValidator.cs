using FluentValidation;
using Project.ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Validations
{
    public class ServiceTicketValidator : AbstractValidator<ServiceTicket>
    {
        public ServiceTicketValidator()
        {
            RuleFor(x => x.Task).NotEmpty().WithMessage("Başlık alanı boş bırakılamaz")
                                .MinimumLength(4).WithMessage("Başlık alanı en az 4 harf içermeli")
                                .MaximumLength(40).WithMessage("Başlık alanı en fazla 20 harf içermeli");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş bırakılamaz")
                                       .MinimumLength(4).WithMessage("Açıklama alanı en az 4 harf içermeli")
                                       .MaximumLength(100).WithMessage("Açıklama alanı en fazla 20 harf içermeli");
            RuleFor(x => x.TaskPriority).NotEmpty().WithMessage("Öncelik alanı boş bırakılamaz");
            RuleFor(x => x.TaskStatus).NotEmpty().WithMessage("Destek durumu boş bırakılamaz");
            RuleFor(x => x.PlannedDate).NotEmpty().WithMessage("Planlanan başlangıç tarihi boş bırakılamaz")
                                       .GreaterThanOrEqualTo(DateTime.Now.AddDays(-1)).WithMessage("Planlanan başlangıç tarihi güncel tarihten küçük olamaz")
                                       .LessThanOrEqualTo(x => x.PlannedDate).WithMessage("Planlanan başlama tarihi bitiş tarihinden daha büyük olamaz");
            RuleFor(x => x.CompletionDate).NotEmpty().WithMessage("Planlanan bitirilme tarihi boş bırakılamaz")
                                          .GreaterThanOrEqualTo(DateTime.Now.AddDays(-1)).WithMessage("Planlanan bitirilme tarihi güncel tarihten küçük olamaz")
                                          .GreaterThanOrEqualTo(x => x.PlannedDate).WithMessage("Planlanan bitirilme tarihi başlangıç tarihinden küçük olamaz");
            RuleFor(x => x.CreatedByID).NotEmpty().WithMessage("Bileti oluşturan kişi boş bırakılamaz");
        }
    }
}

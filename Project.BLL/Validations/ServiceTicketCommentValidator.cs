using FluentValidation;
using Project.ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Validations
{
    public class ServiceTicketCommentValidator : AbstractValidator<ServiceTicketComment>
    {
        public ServiceTicketCommentValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık alanı boş bırakılamaz")
                .MinimumLength(4).WithMessage("Başlık alanı en az 4 harf içermeli")
                .MaximumLength(100).WithMessage("Başlık alanı en fazla 30 harf içermeli");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş bırakılamaz")
                                       .MinimumLength(4).WithMessage("Açıklama alanı en az 4 harf içermeli")
                                       .MaximumLength(400).WithMessage("Açıklama alanı en fazla 100 harf içermeli");
        }
    }
}

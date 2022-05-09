using BlazorSozluk.Common.Models.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User
{
    public class LoginUserCommandValidator:AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.EmailAdress)
                .NotNull()
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("{PropertyName} not a valid email adress");
            RuleFor(x => x.Password)
                .NotNull()
                .MinimumLength(6).WithMessage("{PropertyName} should at least be {MinLength} character ");
        }
    }
}



using FluentValidation;
using FluentValidation.Attributes;

namespace HeadManagementSystem.Models
{
    [Validator(typeof(LoginModelValidator))]
    public class LoginModel
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.LoginName)
                .NotEmpty() 
                .WithMessage("Tài khoản không được để trống.");

            //.EmailAddress()
            //.WithErrorCode(nameof(UserErrorResource.Email_Type))
            //.WithMessage(UserErrorResource.Email_Type)

            //    .Length(0, 256)
            //    .WithErrorCode(nameof(UserErrorResource.Email_Length))
            //    .WithMessage(UserErrorResource.Email_Length);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Mật khẩu không được để trống");

            //    .Length(0, 256)
            //    .WithErrorCode(nameof(UserErrorResource.Password_length))
            //    .WithMessage(UserErrorResource.Password_length);
        }
    }
}
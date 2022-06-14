using FluentValidation;

namespace HomeEasy.Domain.Commands.v1.User.AddUser
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("UserNameCannotBeNullOrEmpty");

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("PasswordCannotBeNullOrEmpty")
                .MinimumLength(8)
                .WithMessage("InvalidPasswordMinimumLength")
                .MaximumLength(16)
                .WithMessage("InvalidPasswordMaximumLength");

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("EmailCannotBeNullOrEmpty")
                .EmailAddress()
                .WithMessage("MustBeAValidEmail");
        }
    }
}
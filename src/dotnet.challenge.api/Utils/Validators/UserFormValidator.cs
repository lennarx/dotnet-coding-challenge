using dotnet.challenge.api.Utils.Forms;
using FluentValidation;
using System;

namespace dotnet.challenge.api.Utils.Validators
{
    public class UserFormValidator : AbstractValidator<UserForm>
    {
        public UserFormValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(128).WithMessage("First name must be at most 128 characters.");

            RuleFor(x => x.LastName)
                .MaximumLength(128).WithMessage("Last name must be at most 128 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(128).WithMessage("Email must be at most 128 characters.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .Must(BeAValidDate).WithMessage("Date of birth must be a valid date.")
                .Must(BeAtLeast18YearsOld).WithMessage("User must be at least 18 years old.");
        }

        private bool BeAValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }

        private bool BeAtLeast18YearsOld(string date)
        {
            if (!DateTime.TryParse(date, out var dob))
                return false;

            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;

            return age >= 18;
        }
    }
}

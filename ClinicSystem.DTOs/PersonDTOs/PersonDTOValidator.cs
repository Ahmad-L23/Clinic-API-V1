using ClinicSystem.DTOs.PersonDTOs;
using FluentValidation;

public class PersonDTOValidator : AbstractValidator<PersonDTO>
{
    public PersonDTOValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name can't exceed 100 characters.");

        RuleFor(p => p.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{7,20}$").WithMessage("Invalid phone number.");

        RuleFor(p => p.Email)
            .EmailAddress().When(p => !string.IsNullOrEmpty(p.Email))
            .WithMessage("Invalid email address.");

        RuleFor(p => p.Address)
            .MaximumLength(200);

        RuleFor(p => p.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past.");
    }
}

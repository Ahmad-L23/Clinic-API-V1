using ClinicSystem.DTOs.DoctorDTOs;
using FluentValidation;

namespace ClinicSystem.API.Validators.DoctorValidators
{
    public class DoctorAddUpdateDTOValidator : AbstractValidator<DoctorAddUpdateDTO>
    {
        public DoctorAddUpdateDTOValidator()
        {
            // DoctorID (NOT NULL in DB – required only in Update)
            RuleFor(d => d.DoctorID)
                .GreaterThan(0)
                .When(d => d.DoctorID.HasValue)
                .WithMessage("DoctorID must be greater than 0.");

            // PersonID (int NOT NULL)
            RuleFor(d => d.PersonID)
                .GreaterThan(0)
                .WithMessage("PersonID must be greater than 0.");

            // Specialization (nvarchar(100) NULL in DB)
            RuleFor(d => d.Specialization)
                .MaximumLength(100)
                .WithMessage("Specialization cannot exceed 100 characters.")
                .When(d => !string.IsNullOrWhiteSpace(d.Specialization));
        }
    }
}
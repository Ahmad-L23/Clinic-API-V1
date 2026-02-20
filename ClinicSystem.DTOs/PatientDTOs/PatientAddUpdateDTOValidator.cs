using ClinicSystem.DTOs.PatientDTOs;
using FluentValidation;

public class PatientAddUpdateDTOValidator : AbstractValidator<PatientAddUpdateDTO>
{
    public PatientAddUpdateDTOValidator()
    {
        // PersonID: required and must be positive
        RuleFor(p => p.PersonID)
            .GreaterThan(0)
            .WithMessage("PersonID must be greater than 0.");
    }
}
using ClinicSystem.DTOs.PatientDTOs;
using FluentValidation;

public class PatientAddUpdateDTOValidator : AbstractValidator<PatientAddUpdateDTO>
{
    public PatientAddUpdateDTOValidator()
    {
        // PatientID: null is allowed (Add), but if set (Update), must be positive
        RuleFor(p => p.PatientID)
            .GreaterThan(0)
            .When(p => p.PatientID.HasValue)
            .WithMessage("PatientID must be greater than 0 for updates.");

        // PersonID: required and must be positive
        RuleFor(p => p.PersonID)
            .GreaterThan(0)
            .WithMessage("PersonID must be greater than 0.");
    }
}
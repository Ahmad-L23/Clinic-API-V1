using ClinicSystem.DTOs.MedicalRecordDTOs;
using FluentValidation;

namespace ClinicSystem.API.Validators.MedicalRecordValidators
{
    public class MedicalRecordDTOValidator : AbstractValidator<MedicalRecordDTO>
    {
        public MedicalRecordDTOValidator()
        {
            // MedicalRecordID (NOT NULL in DB but nullable in DTO)
            RuleFor(m => m.MedicalRecordID)
                .GreaterThan(0)
                .When(m => m.MedicalRecordID.HasValue)
                .WithMessage("MedicalRecordID must be greater than 0.");

            // VisitDescription (nvarchar(200) NULL)
            RuleFor(m => m.VisitDescription)
                .MaximumLength(200)
                .WithMessage("Visit description cannot exceed 200 characters.")
                .When(m => !string.IsNullOrEmpty(m.VisitDescription));

            // Diagnosis (nvarchar(200) NULL)
            RuleFor(m => m.Diagnosis)
                .MaximumLength(200)
                .WithMessage("Diagnosis cannot exceed 200 characters.")
                .When(m => !string.IsNullOrEmpty(m.Diagnosis));

            // AdditionalNotes (nvarchar(200) NULL)
            RuleFor(m => m.AdditionalNotes)
                .MaximumLength(200)
                .WithMessage("Additional notes cannot exceed 200 characters.")
                .When(m => !string.IsNullOrEmpty(m.AdditionalNotes));
        }
    }
}
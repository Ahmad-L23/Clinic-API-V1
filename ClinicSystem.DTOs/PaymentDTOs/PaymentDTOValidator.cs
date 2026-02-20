using ClinicSystem.DTOs.PaymentDTOs;
using FluentValidation;
using System;

namespace ClinicSystem.API.Validators.PaymentValidators
{
    public class PaymentDTOValidator : AbstractValidator<PaymentDTO>
    {
        public PaymentDTOValidator()
        {
            // PaymentID (int NOT NULL)
            RuleFor(p => p.PaymentID)
                .GreaterThan(0)
                .WithMessage("PaymentID must be greater than 0.");

            // PaymentDate (date NOT NULL)
            RuleFor(p => p.PaymentDate)
                .NotEmpty()
                .WithMessage("Payment date is required.");

            // PaymentMethod (nvarchar(50) NULL)
            RuleFor(p => p.PaymentMethod)
                .MaximumLength(50)
                .WithMessage("Payment method cannot exceed 50 characters.")
                .When(p => !string.IsNullOrEmpty(p.PaymentMethod));

            // AmountPaid (decimal(18,0) NOT NULL)
            RuleFor(p => p.AmountPaid)
                .GreaterThan(0)
                .WithMessage("Amount paid must be greater than 0.");

            // AdditionalNotes (nvarchar(200) NULL)
            RuleFor(p => p.AdditionalNotes)
                .MaximumLength(200)
                .WithMessage("Additional notes cannot exceed 200 characters.")
                .When(p => !string.IsNullOrEmpty(p.AdditionalNotes));
        }
    }
}
using System;

namespace ClinicSystem.DTOs.PaymentDTOs
{
    public record PaymentDTO(
        int PaymentID,
        DateTime PaymentDate,
        string PaymentMethod,
        decimal AmountPaid,
        string? AdditionalNotes
    );
}

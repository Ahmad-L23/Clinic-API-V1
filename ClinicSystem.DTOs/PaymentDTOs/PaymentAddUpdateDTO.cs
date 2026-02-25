using System;

namespace ClinicSystem.DTOs.PaymentDTOs
{
    public record PaymentAddUpdateDTO(
        int? PaymentID,               // Null for Add, set for Update
        DateTime PaymentDate,
        string PaymentMethod,
        double AmountPaid,
        string? AdditionalNotes
    );
}

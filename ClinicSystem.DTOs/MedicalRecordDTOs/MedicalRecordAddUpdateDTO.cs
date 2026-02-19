namespace ClinicSystem.DTOs.MedicalRecordDTOs
{
    public record MedicalRecordAddUpdateDTO(
        int? MedicalRecordID,         // Null for Add, set for Update
        string? VisitDescription,
        string? Diagnosis,
        string? AdditionalNotes
    );
}

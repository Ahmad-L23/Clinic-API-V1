namespace ClinicSystem.DTOs.MedicalRecordDTOs
{
    public record MedicalRecordDTO(
        int MedicalRecordID,
        string? VisitDescription,
        string? Diagnosis,
        string? AdditionalNotes
    );
}

namespace ClinicSystem.DTOs.PatientDTOs
{
    public record PatientAddUpdateDTO(
        int? PatientID,   // Null for Add, set for Update
        int PersonID
    );
}

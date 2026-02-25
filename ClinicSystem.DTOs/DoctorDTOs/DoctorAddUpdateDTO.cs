namespace ClinicSystem.DTOs.DoctorDTOs
{
    public record DoctorAddUpdateDTO(
        int? DoctorID,      // Null for Add, set for Update
        int PersonID,
        string? Specialization
    );
}

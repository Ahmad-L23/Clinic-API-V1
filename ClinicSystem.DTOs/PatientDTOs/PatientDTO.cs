using System;

namespace ClinicSystem.DTOs.PatientDTOs
{
    public record PatientDTO(
        int PatientID,
        int PersonID,
        string Name,
        DateTime? DateOfBirth,
        bool? Gender,
        string? PhoneNumber,
        string? Email,
        string? Address
    );
}

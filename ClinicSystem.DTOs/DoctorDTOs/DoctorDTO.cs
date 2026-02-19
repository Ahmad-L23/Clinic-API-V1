using System;

namespace ClinicSystem.DTOs.DoctorDTOs
{
    public record DoctorDTO(
        int DoctorID,
        int PersonID,
        string Specialization,
        string Name,
        DateTime? DateOfBirth,
        bool? Gender,
        string? PhoneNumber,
        string? Email,
        string? Address
    );
}

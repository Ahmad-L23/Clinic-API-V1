using System;

namespace ClinicSystem.DTOs.PersonDTOs
{
    public record PersonDTO(
        int PersonID,
        string Name,
        DateTime? DateOfBirth,
        bool? Gender,
        string PhoneNumber,
        string? Email,
        string? Address
    );
}

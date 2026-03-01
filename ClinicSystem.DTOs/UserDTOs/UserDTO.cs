namespace ClinicSystem.DTOs.UserDTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        // Person Info
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public UserDTO(int userID, int personID, string userName, string role,
                       string name, DateTime? dateOfBirth, bool? gender,
                       string phoneNumber, string email, string address)
        {
            UserID = userID;
            PersonID = personID;
            UserName = userName;
            Role = role;
            Name = name;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
        }
    }
}
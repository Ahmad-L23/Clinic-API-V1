namespace ClinicSystem.DTOs.UserDTOs
{
    public class UserAddUpdateDTO
    {
        public int? UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public UserAddUpdateDTO(int? userID, int personID, string userName,
                                string password, string role)
        {
            UserID = userID;
            PersonID = personID;
            UserName = userName;
            Password = password;
            Role = role;
        }
    }
}
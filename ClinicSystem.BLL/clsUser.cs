using ClinicSystem.DAL;
using ClinicSystem.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using BCrypt.Net;

namespace ClinicSystem.BLL
{
    public class clsUser
    {
        public enum enMode { Add = 0, Update = 1 }

        public enMode Mode = enMode.Add;

        public int? UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        // DTO Helper
        private UserAddUpdateDTO UDto =>
            new UserAddUpdateDTO(UserID, PersonID, UserName, Password, Role);

        // Constructor
        public clsUser(UserAddUpdateDTO dto, enMode mode = enMode.Add)
        {
            this.UserID = dto.UserID;
            this.PersonID = dto.PersonID;
            this.UserName = dto.UserName;
            this.Password = dto.Password;
            this.Role = dto.Role;
            this.Mode = mode;
        }

        // =========================================
        // Private Add
        // =========================================
        private bool _AddUser()
        {
            // Hash password before saving
            Password = BCrypt.Net.BCrypt.HashPassword(Password);

            UserID = clsUsersData.AddUser(UDto);

            return (UserID != -1);
        }

        // =========================================
        // Private Update
        // =========================================
        private bool _UpdateUser()
        {
            // If password changed, hash again
            if (!Password.StartsWith("$2"))
                Password = BCrypt.Net.BCrypt.HashPassword(Password);

            return clsUsersData.UpdateUser(UDto);
        }

        // =========================================
        // Save
        // =========================================
        public bool Save()
        {
            return Mode switch
            {
                enMode.Add => _AddUser(),
                enMode.Update => _UpdateUser(),
                _ => false
            };
        }

        // =========================================
        // Delete
        // =========================================
        public static bool DeleteUser(int userID)
        {
            return clsUsersData.DeleteUser(userID);
        }

        // =========================================
        // Get All
        // =========================================
        public static List<UserDTO> GetAllUsers()
        {
            return clsUsersData.GetAllUsers();
        }

        // =========================================
        // Find By ID
        // =========================================
        public static clsUser? Find(int userID)
        {
            UserAddUpdateDTO? dto = clsUsersData.GetUserByID(userID);

            if (dto != null)
                return new clsUser(dto, enMode.Update);

            return null;
        }

        // =========================================
        // Find By UserName
        // =========================================
        public static clsUser? FindByUserName(string userName)
        {
            UserAddUpdateDTO? dto = clsUsersData.GetUserByUserName(userName);

            if (dto != null)
                return new clsUser(dto, enMode.Update);

            return null;
        }

        // =========================================
        // Login Validation
        // =========================================
        public static clsUser? Authenticate(string userName, string password)
        {
            var user = FindByUserName(userName);

            if (user == null)
                return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            return isValid ? user : null;
        }

        // =========================================
        // Check if Username Exists
        // =========================================
        public static bool IsUserNameExists(string userName)
        {
            return clsUsersData.GetUserByUserName(userName) != null;
        }
    }
}
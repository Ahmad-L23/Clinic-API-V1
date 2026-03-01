using ClinicSystem.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ClinicSystem.DAL
{
    public static class clsUsersData
    {
        private static string _connectionString =
            "Your_Connection_String_Here";

        // =========================================
        // Add User
        // =========================================
        public static int AddUser(UserAddUpdateDTO dto)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_AddUser", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PersonID", dto.PersonID);
            cmd.Parameters.AddWithValue("@UserName", dto.UserName);
            cmd.Parameters.AddWithValue("@Password", dto.Password);
            cmd.Parameters.AddWithValue("@Role", dto.Role);

            conn.Open();

            object result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : -1;
        }

        // =========================================
        // Update User
        // =========================================
        public static bool UpdateUser(UserAddUpdateDTO dto)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_UpdateUser", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", dto.UserID);
            cmd.Parameters.AddWithValue("@PersonID", dto.PersonID);
            cmd.Parameters.AddWithValue("@UserName", dto.UserName);
            cmd.Parameters.AddWithValue("@Password", dto.Password);
            cmd.Parameters.AddWithValue("@Role", dto.Role);

            conn.Open();
            int rows = Convert.ToInt32(cmd.ExecuteScalar());

            return rows > 0;
        }

        // =========================================
        // Delete User
        // =========================================
        public static bool DeleteUser(int userID)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_DeleteUser", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userID);

            conn.Open();
            int rows = Convert.ToInt32(cmd.ExecuteScalar());

            return rows > 0;
        }

        // =========================================
        // Get All Users
        // =========================================
        public static List<UserDTO> GetAllUsers()
        {
            List<UserDTO> list = new List<UserDTO>();

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetAllUsers", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new UserDTO(
                    reader.GetInt32(reader.GetOrdinal("UserID")),
                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                    reader.GetString(reader.GetOrdinal("UserName")),
                    reader.GetString(reader.GetOrdinal("Role")),

                    reader.GetString(reader.GetOrdinal("Name")),
                    reader["DateOfBirth"] as DateTime?,
                    reader["Gender"] as bool?,
                    reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    reader["Email"]?.ToString(),
                    reader["Address"]?.ToString()
                ));
            }

            return list;
        }

        // =========================================
        // Get User By ID
        // =========================================
        public static UserAddUpdateDTO? GetUserByID(int userID)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetUserByID", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userID);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new UserAddUpdateDTO(
                    reader.GetInt32(reader.GetOrdinal("UserID")),
                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                    reader.GetString(reader.GetOrdinal("UserName")),
                    reader.GetString(reader.GetOrdinal("Password")),
                    reader.GetString(reader.GetOrdinal("Role"))
                );
            }

            return null;
        }

        // =========================================
        // Get User By UserName (Login)
        // =========================================
        public static UserAddUpdateDTO? GetUserByUserName(string userName)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetUserByUserName", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", userName);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new UserAddUpdateDTO(
                    reader.GetInt32(reader.GetOrdinal("UserID")),
                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                    reader.GetString(reader.GetOrdinal("UserName")),
                    reader.GetString(reader.GetOrdinal("Password")),
                    reader.GetString(reader.GetOrdinal("Role"))
                );
            }

            return null;
        }
    }
}
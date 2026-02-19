using ClinicSystem.DAL.Global;
using ClinicSystem.DTOs.DoctorDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ClinicSystem.DAL
{
    public static class clsDoctorsData
    {
        private static readonly string _connectionString = DataAccessSetting.ConnectionString;

        // Add a new doctor
        public static int AddDoctor(DoctorAddUpdateDTO dto)
        {
            if (dto.PersonID <= 0)
                throw new ArgumentException("PersonID must be set for adding a doctor.");

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_AddDoctor", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PersonID", dto.PersonID);
            cmd.Parameters.AddWithValue("@Specialization", dto.Specialization);

            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // Update an existing doctor
        public static bool UpdateDoctor(DoctorAddUpdateDTO dto)
        {
            if (!dto.DoctorID.HasValue)
                throw new ArgumentException("DoctorID must be set for updating a doctor.");

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_UpdateDoctor", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@DoctorID", dto.DoctorID.Value);
            cmd.Parameters.AddWithValue("@PersonID", dto.PersonID);
            cmd.Parameters.AddWithValue("@Specialization", dto.Specialization);

            conn.Open();

            int rowsAffected = Convert.ToInt32(cmd.ExecuteScalar()); 
            return rowsAffected > 0;

        }

        // Delete a doctor
        public static bool DeleteDoctor(int doctorId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_DeleteDoctor", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@DoctorID", doctorId);
            conn.Open();

            int rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
            return rowsAffected > 0;

        }

        // Get all doctors
        public static List<DoctorDTO> GetAllDoctors(int pageNumber = 1, int pageSize = 10)
        {
            List<DoctorDTO> doctors = new List<DoctorDTO>();

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetAllDoctors", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Add pagination parameters
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                doctors.Add(new DoctorDTO(
                    reader.GetInt32(reader.GetOrdinal("DoctorID")),
                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                    reader.GetString(reader.GetOrdinal("Specialization")),
                    reader.GetString(reader.GetOrdinal("Name")),
                    reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? null : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetBoolean(reader.GetOrdinal("Gender")),
                    reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                    reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address"))
                ));
            }

            return doctors;
        }


        // Get a doctor by ID
        public static DoctorDTO? GetDoctorByID(int doctorId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetDoctorByID", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@DoctorID", doctorId);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new DoctorDTO(
                    reader.GetInt32(reader.GetOrdinal("DoctorID")),
                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                    reader.GetString(reader.GetOrdinal("Specialization")),
                    reader.GetString(reader.GetOrdinal("Name")),
                    reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? null : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetBoolean(reader.GetOrdinal("Gender")),
                    reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                    reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address"))
                );
            }

            return null;
        }
    }
}

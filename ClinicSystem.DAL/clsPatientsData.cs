using ClinicSystem.DAL.Global;
using ClinicSystem.DTOs.PatientDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ClinicSystem.DAL
{
    public static class clsPatientsData
    {
        private static readonly string _connectionString = DataAccessSetting.ConnectionString;

        // Add a new patient
        public static int AddPatient(PatientAddUpdateDTO dto)
        {
            if (dto.PersonID <= 0)
                throw new ArgumentException("PersonID must be set for adding a patient.");

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_AddPatient", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PersonID", dto.PersonID);

            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // Update an existing patient
        public static bool UpdatePatient(PatientAddUpdateDTO dto)
        {
            if (!dto.PatientID.HasValue)
                throw new ArgumentException("PatientID must be set for updating a patient.");

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_UpdatePatient", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PatientID", dto.PatientID.Value);
            cmd.Parameters.AddWithValue("@PersonID", dto.PersonID);

            conn.Open();

            int rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
            return rowsAffected > 0;
        }

        // Delete a patient
        public static bool DeletePatient(int patientId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_DeletePatient", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PatientID", patientId);

            conn.Open();

            int rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
            return rowsAffected > 0;
        }

        // Get all patients (from view)
        public static List<PatientDTO> GetAllPatients(int pageNumber = 1, int pageSize = 10)
        {
            List<PatientDTO> patients = new List<PatientDTO>();

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetAllPatients", conn)
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
                patients.Add(new PatientDTO(
                    reader.GetInt32(reader.GetOrdinal("PatientID")),
                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                    reader.GetString(reader.GetOrdinal("Name")),
                    reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? null : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetBoolean(reader.GetOrdinal("Gender")),
                    reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                    reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address"))
                ));
            }

            return patients;
        }


        // Get a patient by ID (from view)
        public static PatientDTO? GetPatientByID(int patientId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetPatientByID", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PatientID", patientId);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new PatientDTO(
                    reader.GetInt32(reader.GetOrdinal("PatientID")),
                    reader.GetInt32(reader.GetOrdinal("PersonID")),
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

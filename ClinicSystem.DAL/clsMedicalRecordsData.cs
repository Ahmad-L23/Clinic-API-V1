using ClinicSystem.DAL.Global;
using ClinicSystem.DTOs.MedicalRecordDTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace ClinicSystem.DAL
{
    public static class clsMedicalRecordsData
    {
        private static readonly string _connectionString = DataAccessSetting.ConnectionString;

        // Add a new medical record
        public static int AddMedicalRecord(MedicalRecordAddUpdateDTO dto)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_AddMedicalRecord", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@VisitDescription", (object?)dto.VisitDescription ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Diagnosis", (object?)dto.Diagnosis ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AdditionalNotes", (object?)dto.AdditionalNotes ?? DBNull.Value);

            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // Update an existing medical record
        public static bool UpdateMedicalRecord(MedicalRecordAddUpdateDTO dto)
        {
            if (!dto.MedicalRecordID.HasValue)
                throw new ArgumentException("MedicalRecordID must be set for updating.");

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_UpdateMedicalRecord", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@MedicalRecordID", dto.MedicalRecordID.Value);
            cmd.Parameters.AddWithValue("@VisitDescription", (object?)dto.VisitDescription ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Diagnosis", (object?)dto.Diagnosis ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AdditionalNotes", (object?)dto.AdditionalNotes ?? DBNull.Value);

            conn.Open();
            int rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
            return rowsAffected > 0;
        }

        // Delete a medical record
        public static bool DeleteMedicalRecord(int medicalRecordId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_DeleteMedicalRecord", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@MedicalRecordID", medicalRecordId);

            conn.Open();
            int rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
            return rowsAffected > 0;
        }

        // Get all medical records (paginated)
        public static List<MedicalRecordDTO> GetAllMedicalRecords(int pageNumber = 1, int pageSize = 10)
        {
            List<MedicalRecordDTO> records = new List<MedicalRecordDTO>();

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetAllMedicalRecords", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                records.Add(new MedicalRecordDTO(
                    reader.GetInt32(reader.GetOrdinal("MedicalRecordID")),
                    reader.IsDBNull(reader.GetOrdinal("VisitDescription")) ? null : reader.GetString(reader.GetOrdinal("VisitDescription")),
                    reader.IsDBNull(reader.GetOrdinal("Diagnosis")) ? null : reader.GetString(reader.GetOrdinal("Diagnosis")),
                    reader.IsDBNull(reader.GetOrdinal("AdditionalNotes")) ? null : reader.GetString(reader.GetOrdinal("AdditionalNotes"))
                ));
            }

            return records;
        }

        // Get a medical record by ID
        public static MedicalRecordDTO? GetMedicalRecordByID(int medicalRecordId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetMedicalRecordByID", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@MedicalRecordID", medicalRecordId);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new MedicalRecordDTO(
                    reader.GetInt32(reader.GetOrdinal("MedicalRecordID")),
                    reader.IsDBNull(reader.GetOrdinal("VisitDescription")) ? null : reader.GetString(reader.GetOrdinal("VisitDescription")),
                    reader.IsDBNull(reader.GetOrdinal("Diagnosis")) ? null : reader.GetString(reader.GetOrdinal("Diagnosis")),
                    reader.IsDBNull(reader.GetOrdinal("AdditionalNotes")) ? null : reader.GetString(reader.GetOrdinal("AdditionalNotes"))
                );
            }

            return null;
        }
    }
}

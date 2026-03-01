using ClinicSystem.DAL.Global;
using ClinicSystem.DTOs.AppointmentDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ClinicSystem.DAL
{
    public static class clsAppointmentsData
    {
        private static readonly string _connectionString = DataAccessSetting.ConnectionString;

        /* =========================================================
           ADD APPOINTMENT
        ========================================================= */

        public static int AddAppointment(AppointmentAddUpdateDTO dto)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_CreateAppointment", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PatientID", dto.PatientID);
            cmd.Parameters.AddWithValue("@DoctorID", dto.DoctorID);
            cmd.Parameters.AddWithValue("@AppointmentDateTime", dto.AppointmentDateTime);
            cmd.Parameters.AddWithValue("@AppointmentStatus", dto.AppointmentStatus);
            cmd.Parameters.AddWithValue("@MedicalRecordID", (object?)dto.MedicalRecordID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PaymentID", (object?)dto.PaymentID ?? DBNull.Value);

            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /* =========================================================
           UPDATE APPOINTMENT
        ========================================================= */

        public static bool UpdateAppointment(AppointmentAddUpdateDTO dto)
        {
            if (!dto.AppointmentID.HasValue)
                throw new ArgumentException("AppointmentID must be set for updating.");

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_UpdateAppointment", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@AppointmentID", dto.AppointmentID.Value);
            cmd.Parameters.AddWithValue("@PatientID", dto.PatientID);
            cmd.Parameters.AddWithValue("@DoctorID", dto.DoctorID);
            cmd.Parameters.AddWithValue("@AppointmentDateTime", dto.AppointmentDateTime);
            cmd.Parameters.AddWithValue("@AppointmentStatus", dto.AppointmentStatus);
            cmd.Parameters.AddWithValue("@MedicalRecordID", (object?)dto.MedicalRecordID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PaymentID", (object?)dto.PaymentID ?? DBNull.Value);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        /* =========================================================
           DELETE APPOINTMENT
        ========================================================= */

        public static bool DeleteAppointment(int appointmentId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_DeleteAppointment", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        /* =========================================================
           GET ALL APPOINTMENTS WITH PAGINATION
        ========================================================= */

        public static (List<AppointmentDTO> Data, int TotalRecords, int TotalPages)
            GetAllAppointments(int pageNumber = 1, int pageSize = 10)
        {
            List<AppointmentDTO> list = new();

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetAllAppointments", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            // First Result Set → Data
            while (reader.Read())
            {
                list.Add(MapAppointment(reader));
            }

            int totalRecords = 0;
            int totalPages = 0;

            // Second Result Set → Pagination Info
            if (reader.NextResult() && reader.Read())
            {
                totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
                totalPages = reader.GetInt32(reader.GetOrdinal("TotalPages"));
            }

            return (list, totalRecords, totalPages);
        }

        /* =========================================================
           GET APPOINTMENT BY ID
        ========================================================= */

        public static AppointmentDTO? GetAppointmentByID(int appointmentId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetAppointmentByID", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
                return MapAppointment(reader);

            return null;
        }

        /* =========================================================
           GET BY PATIENT
        ========================================================= */

        public static List<AppointmentDTO> GetAppointmentsByPatientID(int patientId)
        {
            List<AppointmentDTO> list = new();

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetAppointmentsByPatientID", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PatientID", patientId);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(MapAppointment(reader));
            }

            return list;
        }

        /* =========================================================
           GET BY DOCTOR
        ========================================================= */

        public static List<AppointmentDTO> GetAppointmentsByDoctorID(int doctorId)
        {
            List<AppointmentDTO> list = new();

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetAppointmentsByDoctorID", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@DoctorID", doctorId);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(MapAppointment(reader));
            }

            return list;
        }

        /* =========================================================
           CHECK APPOINTMENT EXISTS
        ========================================================= */

        public static bool AppointmentExists(int doctorId, DateTime dateTime)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_CheckAppointmentExistsByDoctorAndDate", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@DoctorID", doctorId);
            cmd.Parameters.AddWithValue("@AppointmentDateTime", dateTime);

            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
        }

        /* =========================================================
           MAP METHOD (VERY IMPORTANT)
        ========================================================= */

        private static AppointmentDTO MapAppointment(SqlDataReader reader)
        {
            return new AppointmentDTO(
                reader.GetInt32(reader.GetOrdinal("AppointmentID")),
                reader.GetInt32(reader.GetOrdinal("PatientID")),
                reader.GetInt32(reader.GetOrdinal("DoctorID")),
                reader.GetDateTime(reader.GetOrdinal("AppointmentDateTime")),
                reader.GetByte(reader.GetOrdinal("AppointmentStatus")),
                reader.IsDBNull(reader.GetOrdinal("MedicalRecordID")) ? null : reader.GetInt32(reader.GetOrdinal("MedicalRecordID")),
                reader.IsDBNull(reader.GetOrdinal("PaymentID")) ? null : reader.GetInt32(reader.GetOrdinal("PaymentID")),
                reader.IsDBNull(reader.GetOrdinal("VisitDescription")) ? null : reader.GetString(reader.GetOrdinal("VisitDescription")),
                reader.IsDBNull(reader.GetOrdinal("Diagnosis")) ? null : reader.GetString(reader.GetOrdinal("Diagnosis")),
                reader.IsDBNull(reader.GetOrdinal("AdditionalNotes")) ? null : reader.GetString(reader.GetOrdinal("AdditionalNotes")),
                reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                reader.IsDBNull(reader.GetOrdinal("Specialization")) ? null : reader.GetString(reader.GetOrdinal("Specialization"))
            );
        }
    }
}
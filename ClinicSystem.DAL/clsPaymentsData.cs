using ClinicSystem.DAL.Global;
using ClinicSystem.DTOs.PaymentDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ClinicSystem.DAL
{
    public static class clsPaymentsData
    {
        private static readonly string _connectionString = DataAccessSetting.ConnectionString;

        // Add a new payment
        public static int AddPayment(PaymentAddUpdateDTO dto)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_AddPayment", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PaymentDate", dto.PaymentDate);
            cmd.Parameters.AddWithValue("@PaymentMethod", dto.PaymentMethod);
            cmd.Parameters.AddWithValue("@AmountPaid", dto.AmountPaid);
            cmd.Parameters.AddWithValue("@AdditionalNotes", (object?)dto.AdditionalNotes ?? DBNull.Value);

            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // Update an existing payment
        public static bool UpdatePayment(PaymentAddUpdateDTO dto)
        {
            if (!dto.PaymentID.HasValue)
                throw new ArgumentException("PaymentID must be set for updating.");

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_UpdatePayment", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PaymentID", dto.PaymentID.Value);
            cmd.Parameters.AddWithValue("@PaymentDate", dto.PaymentDate);
            cmd.Parameters.AddWithValue("@PaymentMethod", dto.PaymentMethod);
            cmd.Parameters.AddWithValue("@AmountPaid", dto.AmountPaid);
            cmd.Parameters.AddWithValue("@AdditionalNotes", (object?)dto.AdditionalNotes ?? DBNull.Value);

            conn.Open();

            int rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
            return rowsAffected > 0;
        }

        // Delete a payment
        public static bool DeletePayment(int paymentId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_DeletePayment", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PaymentID", paymentId);
            conn.Open();

            int rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
            return rowsAffected > 0;
        }

        // Get all payments (with pagination)
        public static List<PaymentDTO> GetAllPayments(int pageNumber = 1, int pageSize = 10)
        {
            List<PaymentDTO> payments = new List<PaymentDTO>();

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetAllPayments", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                payments.Add(new PaymentDTO(
                    reader.GetInt32(reader.GetOrdinal("PaymentID")),
                    reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                    reader.GetString(reader.GetOrdinal("PaymentMethod")),
                    reader.GetDecimal(reader.GetOrdinal("AmountPaid")),
                    reader.IsDBNull(reader.GetOrdinal("AdditionalNotes")) ? null : reader.GetString(reader.GetOrdinal("AdditionalNotes"))
                ));
            }

            return payments;
        }

        // Get a payment by ID
        public static PaymentDTO? GetPaymentByID(int paymentId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetPaymentByID", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PaymentID", paymentId);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new PaymentDTO(
                    reader.GetInt32(reader.GetOrdinal("PaymentID")),
                    reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                    reader.GetString(reader.GetOrdinal("PaymentMethod")),
                    reader.GetDecimal(reader.GetOrdinal("AmountPaid")),
                    reader.IsDBNull(reader.GetOrdinal("AdditionalNotes")) ? null : reader.GetString(reader.GetOrdinal("AdditionalNotes"))
                );
            }

            return null;
        }
    }
}

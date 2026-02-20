using ClinicSystem.DAL.Global;
using ClinicSystem.DTOs.PersonDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ClinicSystem.DAL
{
    public static class clsPersonData
    {
        private static readonly string _connectionString = DataAccessSetting.ConnectionString;

        // Add a new person
        public static int AddPerson(PersonDTO person)
        {

            int NewId = -1;
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_AddPerson", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Name", person.Name);
            cmd.Parameters.AddWithValue("@DateOfBirth", (object?)person.DateOfBirth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Gender", (object?)person.Gender ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PhoneNumber", (object?)person.PhoneNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)person.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object?)person.Address ?? DBNull.Value);

            conn.Open();

            object result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                NewId = Convert.ToInt32(result);
            }

            return NewId;
        }

        // Update an existing person
        public static bool UpdatePerson(PersonDTO person)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_UpdatePerson", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PersonID", person.PersonID);
            cmd.Parameters.AddWithValue("@Name", person.Name);
            cmd.Parameters.AddWithValue("@DateOfBirth", (object?)person.DateOfBirth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Gender", (object?)person.Gender ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PhoneNumber", (object?)person.PhoneNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)person.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object?)person.Address ?? DBNull.Value);

            conn.Open();

            int rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
            return rowsAffected > 0;
        }

        // Delete a person
        public static bool DeletePerson(int personId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_DeletePerson", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PersonID", personId);

            conn.Open();

            int rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
            return rowsAffected > 0;
        }

        // Get all persons
        public static List<PersonDTO> GetAllPersons(int pageNumber = 1, int pageSize = 10)
        {
            List<PersonDTO> persons = new List<PersonDTO>();

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetAllPersons", conn)
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
                persons.Add(new PersonDTO(
                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                    reader.GetString(reader.GetOrdinal("Name")),
                    reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? null : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetBoolean(reader.GetOrdinal("Gender")),
                    reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                    reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address"))
                ));
            }

            return persons;
        }


        // Get a person by ID
        public static PersonDTO? GetPersonByID(int personId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_GetPersonByID", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PersonID", personId);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new PersonDTO(
                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                    reader.GetString(reader.GetOrdinal("Name")),
                    reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? null : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetBoolean(reader.GetOrdinal("Gender")),
                    reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                    reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address"))
                );
            }

            return null;
        }
    }
}

using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ClinicSystem.DAL.Global
{
    internal class clsDataAccessHelper
    {
        public void DeleteRecord(string storedProcName, int id, string fieldName)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add the ID parameter
                    cmd.Parameters.AddWithValue(fieldName, id);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} row(s) deleted.");
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                        Console.WriteLine("Error: " + ex.Message);
                        throw;
                    }
                }
            }
        }
    }
}

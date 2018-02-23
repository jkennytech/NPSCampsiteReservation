using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class CampgroundSqlDAL
    {
        private string connectionString;

        public CampgroundSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Campground> GetAllAvailableCampgroundsInPark(string parkId)
        {
            List<Campground> output = new List<Campground>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM campground WHERE park_id = @parkId ORDER BY campground_id;", conn);

                    cmd.Parameters.AddWithValue("@parkId", parkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        Campground c = new Campground();
                        c.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                        c.Park_Id = Convert.ToInt32(reader["park_id"]);
                        c.Name = Convert.ToString(reader["name"]);
                        c.Open_From_Mm = Convert.ToInt32(reader["open_from_mm"]);
                        c.Open_To_Mm = Convert.ToInt32(reader["open_to_mm"]);
                        c.Daily_Fee = Convert.ToDecimal(reader["daily_fee"]);

                        output.Add(c);
                    }
                }
            }

            catch(SqlException ex)
            {
                Console.WriteLine("An error occurred while reading the database: " + ex.Message);
            }

            return output;
        }
    }
}

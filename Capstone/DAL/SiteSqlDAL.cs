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
    public class SiteSqlDAL
    {
        private string connectionString;

        public SiteSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Site> GetAllAvailableCampsites(string campgroundId, DateTime arrivalDate, DateTime departureDate)
        {
            List<Site> output = new List<Site>();

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM site WHERE campground_id = @campid ORDER BY site_id;", conn);

                    cmd.Parameters.AddWithValue("@campid", campgroundId);
                    cmd.Parameters.AddWithValue("@arrival", arrivalDate);
                    cmd.Parameters.AddWithValue("@departure", departureDate);


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site s = new Site();
                        s.Site_Id = Convert.ToInt32(reader["site_id"]);
                        s.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                        s.Site_Number = Convert.ToInt32(reader["site_number"]);
                        s.Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]);
                        s.Accessible = Convert.ToBoolean(reader["accessible"]);
                        s.Max_Rv_Length = Convert.ToInt32(reader["max_rv_length"]);
                        s.Utilities = Convert.ToBoolean(reader["utilities"]);

                        output.Add(s);
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("An error occurred while reading from the database: " + ex.Message);
                throw;
            }

            return output;

        }

    }
}

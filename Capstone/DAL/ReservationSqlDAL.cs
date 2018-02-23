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
    public class ReservationSqlDAL
    {
        private string connectionString;

        public ReservationSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public int BookReservation(int siteId, string name, DateTime arrivalDate, DateTime departureDate)
        {
            int reservationId = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (@siteid, @name, @arrivalDate, @departureDate);", conn);

                    cmd.Parameters.AddWithValue("@siteid", siteId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@arrivalDate", arrivalDate);
                    cmd.Parameters.AddWithValue("@departureDate", departureDate);

                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT MAX(reservation.reservation_id) AS 'id' FROM reservation;", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        reservationId = Convert.ToInt32(reader["id"]);
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("There was an error booking the reservation: " + ex.Message);
                throw;
            }

            return reservationId;
        }


    }
}

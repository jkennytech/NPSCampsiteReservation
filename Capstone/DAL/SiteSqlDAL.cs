﻿using System;
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

            

            string arrivalMonth = arrivalDate.ToString().Substring(0, 2);
            if(arrivalMonth.Contains('/'))
            {
                arrivalMonth = arrivalMonth[0].ToString();
            }
            int arrivalMonthInt = int.Parse(arrivalMonth);
            string departureMonth = departureDate.ToString().Substring(0, 2);
            if(departureMonth.Contains('/'))
            {
                departureMonth = departureMonth[0].ToString();
            }
            int departureMonthInt = int.Parse(departureMonth);            

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT TOP 5 site.site_id, site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, campground.daily_fee FROM site LEFT JOIN campground ON site.campground_id = campground.campground_id WHERE site.site_id NOT IN (SELECT reservation.site_id FROM reservation WHERE((reservation.from_date >= @arrivalDate  AND reservation.from_date < @departureDate) OR(reservation.to_date > @arrivalDate AND reservation.to_date <= @departureDate) OR(reservation.from_date <= @arrivalDate AND reservation.to_date >= @departureDate))) AND(@arrival >= campground.open_from_mm AND @arrival <= campground.open_to_mm) AND(@departure >= campground.open_from_mm AND @departure <= campground.open_to_mm) AND campground.campground_id = @campid;", conn);

                    cmd.Parameters.AddWithValue("@campid", campgroundId);
                    cmd.Parameters.AddWithValue("@arrival", arrivalMonthInt);
                    cmd.Parameters.AddWithValue("@departure", departureMonthInt);
                    cmd.Parameters.AddWithValue("@arrivalDate", arrivalDate);
                    cmd.Parameters.AddWithValue("@departureDate", departureDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site s = new Site();
                        s.Site_Id = Convert.ToInt32(reader["site_id"]);
                        s.Campground_Id = Convert.ToInt32(campgroundId);
                        s.Site_Number = Convert.ToInt32(reader["site_number"]);
                        s.Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]);
                        s.Accessible = Convert.ToBoolean(reader["accessible"]);
                        s.Max_Rv_Length = Convert.ToInt32(reader["max_rv_length"]);
                        s.Utilities = Convert.ToBoolean(reader["utilities"]);
                        s.Daily_Fee = Convert.ToDecimal(reader["daily_fee"]);
                        
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

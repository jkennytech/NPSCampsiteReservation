using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.Models;
using System.Data.SqlClient;
using System.Transactions;
using Capstone.Tests.Tests;

namespace Capstone.Tests.Tests
{
    [TestClass]
    public class ReservationSqlDALTests
    {
        public static string connectionString = @"Server=.\SQLEXPRESS;Database=NationalParks;Trusted_Connection=True;";

        public int InsertFakeReservation(string name, int siteId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (@siteid, @name, @arrivalDate, @departureDate);", conn);

                cmd.Parameters.AddWithValue("@siteid", siteId);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@arrivalDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@departureDate", DateTime.Now);

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT MAX(reservation.reservation_id) FROM reservation;", conn);

                int result = Convert.ToInt32(cmd.ExecuteScalar());

                return result;
            }
        }

        [TestMethod]
        public void ReservationSqlDALTestsBookReservation()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                //Arrange
                SiteSqlDALTests tests = new SiteSqlDALTests();
                int siteId = tests.InsertFakeSite();
                int id = InsertFakeReservation("Random", siteId);
                ReservationSqlDAL testClass = new ReservationSqlDAL(connectionString);

                //Act
                int finalId = testClass.BookReservation(siteId, "Random", DateTime.Now, DateTime.Now);

                //Assert
                Assert.AreEqual(id, finalId - 1);
            }
        }
    }
}

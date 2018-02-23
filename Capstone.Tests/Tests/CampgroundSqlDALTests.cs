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

namespace Capstone.Tests.Tests
{
    [TestClass]
    public class CampgroundSqlDALTests
    {
        public static string connectionString = @"Server=.\SQLEXPRESS;Database=NationalParks;Trusted_Connection=True;";

        public int InsertFakeCampground(string name)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT into campground VALUES (3, @name, 1, 12, 30.00);", conn);

                cmd.Parameters.AddWithValue("@name", name);

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT MAX(campground.campground_id) FROM campground;", conn);

                int result = Convert.ToInt32(cmd.ExecuteScalar());

                return result;
            }
        }

        [TestMethod]
        public void CampgroundSqlDALReturnAllCampgrounds()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                //Arrange
                int id = InsertFakeCampground("Random");
                CampgroundSqlDAL testClass = new CampgroundSqlDAL(connectionString);

                //Act
                List<Campground> campgrounds = testClass.GetAllAvailableCampgroundsInPark("3");

                //Assert
                Assert.AreEqual(id, campgrounds[campgrounds.Count - 1].Campground_Id);
            }
        }
    }
}

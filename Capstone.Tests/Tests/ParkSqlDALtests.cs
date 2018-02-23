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
    public class ParkSqlDALTests
    {
        public static string connectionString = @"Server=.\SQLEXPRESS;Database=NationalParks;Trusted_Connection=True;";

        public int InsertFakePark(string name)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT into park VALUES (@name, 'Illinois', @now, 1000, 1, 'A park');", conn);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@now", DateTime.Now);

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT MAX(park.park_id) FROM park;", conn);

                int result = Convert.ToInt32(cmd.ExecuteScalar());

                return result;
            }
        }



        [TestMethod]
        public void ParkSqlDALTestsGetAllAvailableParks()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                //Arrange
                int id = InsertFakePark("Random");
                ParkSqlDAL testClass = new ParkSqlDAL(connectionString);

                //Act
                List<Park> parks = testClass.GetAllAvailableParks();

                //Assert
                Assert.AreEqual(id, parks[parks.Count - 1].Park_Id);
            }
        }
    }
}

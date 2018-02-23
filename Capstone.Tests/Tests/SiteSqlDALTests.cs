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
    public class SiteSqlDALTests
    {
        public static string connectionString = @"Server=.\SQLEXPRESS;Database=NationalParks;Trusted_Connection=True;";

        public int InsertFakeSite()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT into site (site_number, campground_id) VALUES (9999, 6);", conn);

                //cmd.Parameters.AddWithValue("@name", name);

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT MAX(site.site_id) FROM site;", conn);

                int result = Convert.ToInt32(cmd.ExecuteScalar());

                return result;
            }
        }

        [TestMethod]
        public void SiteSqlDALGetAllAvailableCampsites()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                //Arrange
                int id = InsertFakeSite();
                SiteSqlDAL testClass = new SiteSqlDAL(connectionString);

                //Act
                List<Site> sites = testClass.GetAllAvailableCampsites("6", DateTime.Now, DateTime.Now);

                //Assert
                Assert.AreEqual(id, sites[sites.Count - 1].Site_Id);
            }
        }
    }
}


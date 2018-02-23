using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.Models;
using System.Data.Sql;

namespace Capstone
{
    public class ProjectCLI
    {

        private string connectionString;
        static Dictionary<int, string> numberMonth = new Dictionary<int, string>()
        {
            {1, "January" },
            {2, "February" },
            {3, "March" },
            {4, "April" },
            {5, "May" },
            {6, "June" },
            {7, "July" },
            {8, "August" },
            {9, "September" },
            {10, "October" },
            {11, "November" },
            {12, "December" }
        };

        public ProjectCLI(string dBConnection)
        {
            connectionString = dBConnection;
        }

        public void RunCLI()
        {
            while (true)
            {
                PrintHeader();
                PrintMainMenu();


                string userInput = Console.ReadLine();

                Console.Clear();

                if(userInput != "1" && userInput != "2")
                {
                    Console.Clear();
                    Console.WriteLine("Please enter a valid input");
                    Freeze();
                }
                else if (userInput == "1")
                {
                    PrintParkSelectMenu();
                }
                else if(userInput == "2")
                {
                    Environment.Exit(0);
                }
            }




        }



        public void PrintHeader()
        {
            Console.WriteLine("National Park Campsite Reservation");
            Console.WriteLine();
        }

        public void PrintMainMenu()
        {
            Console.WriteLine("(1) View all available parks");
            Console.WriteLine("(2) Exit");
            Console.WriteLine();
        }

        public void PrintParkSelectMenu()
        {

            while (true)
            {
                PrintHeader();
                ParkSqlDAL dal = new ParkSqlDAL(connectionString);
                List<Park> parks = dal.GetAllAvailableParks();



                if (parks.Count > 0)
                {

                    int counter = 1;
                    

                    Console.WriteLine("Select a Park for Further Details");
                    foreach (Park park in parks)
                    {
                        Console.WriteLine($"{counter}) {park.Name}");
                        counter++;                     
                    }

                    Console.WriteLine("Q) quit");
                }
                else
                {
                    Console.WriteLine("**** NO AVAILABLE PARKS ****");
                    break;
                }

                //Console.WriteLine();
                //Console.WriteLine("In which park would you like to book a campground?");
                //Console.WriteLine();

                string userInput = Console.ReadLine();
                Console.Clear();


                foreach (Park park in parks)
                {
                    if (userInput == park.Park_Id.ToString())
                    {
                        DisplayParkInformation(park.Park_Id.ToString());
                    }

                }

                if(userInput.ToLower() == "q")
                {
                    Environment.Exit(0);
                }
            }


        }

        public void PrintCampgroundsInParkSelectMenu(string parkId, string parkName)
        {

            while (true)
            {
                PrintHeader();
                CampgroundSqlDAL dal = new CampgroundSqlDAL(connectionString);
                List<Campground> campgrounds = dal.GetAllAvailableCampgroundsInPark(parkId);


                if (campgrounds.Count > 0)
                {
                    string campgroundId = "ID#";
                    string name = "Name:";
                    string openDate = "Open (mm):";
                    string closeDate = "Close (mm):";
                    string dailyFee = "Daily Fee:";

                    Console.WriteLine($"{parkName} National Park Campgrounds");
                    Console.WriteLine();
                    Console.WriteLine($"{campgroundId.PadRight(5)}{name.PadRight(35)}{openDate.PadRight(20)}{closeDate.PadRight(20)}{dailyFee.PadRight(10)}");
                    foreach (Campground campground in campgrounds)
                    {
                        Console.WriteLine($"#{campground.Campground_Id.ToString().PadRight(4)}{campground.Name.PadRight(35)}{numberMonth[campground.Open_From_Mm].PadRight(20)}{numberMonth[campground.Open_To_Mm].PadRight(20)}{campground.Daily_Fee.ToString("C").PadRight(10)}");

                    }

                }
                else
                {
                    Console.WriteLine("**** NO AVAILABLE CAMPGROUNDS IN THIS PARK ****");
                    break;
                }

                Console.WriteLine();
                Console.WriteLine("Select a Command");
                Console.WriteLine("1) Search for Available Reservation");
                Console.WriteLine("2) Return to Previous Screen");

                string userInput = Console.ReadLine();
                Console.Clear();

                if (userInput != "1" && userInput != "2")
                {
                    Console.Clear();
                    Console.WriteLine("Please enter a valid input");
                    Freeze();
                }
                else if(userInput == "1")
                {
                    SearchForCampgroundReservation(parkId, parkName);
                }
                else if(userInput == "2")
                {
                    break;
                }


            }

        }

        private void SearchForCampgroundReservation(string parkId, string parkName)
        {

            while (true)
            {
                PrintHeader();
                CampgroundSqlDAL dal = new CampgroundSqlDAL(connectionString);
                List<Campground> campgrounds = dal.GetAllAvailableCampgroundsInPark(parkId);
                string userCampground;
                bool isValidCampground = false;
                bool isValidArrivalDate = false;
                bool isValidDepartureDate = false;
                string userDateArrive;
                string userDateDepart;

                if (campgrounds.Count > 0)
                {
                    string campgroundId = "ID#";
                    string name = "Name:";
                    string openDate = "Open (mm):";
                    string closeDate = "Close (mm):";
                    string dailyFee = "Daily Fee:";

                    Console.WriteLine($"{campgroundId.PadRight(5)}{name.PadRight(35)}{openDate.PadRight(20)}{closeDate.PadRight(20)}{dailyFee.PadRight(10)}");
                    foreach (Campground campground in campgrounds)
                    {
                        Console.WriteLine($"#{campground.Campground_Id.ToString().PadRight(4)}{campground.Name.PadRight(35)}{numberMonth[campground.Open_From_Mm].PadRight(20)}{numberMonth[campground.Open_To_Mm].PadRight(20)}{campground.Daily_Fee.ToString("C").PadRight(10)}");
                    }

                }
                else
                {
                    Console.WriteLine("**** NO AVAILABLE CAMPGROUNDS IN THIS PARK ****");
                    break;
                }

                Console.WriteLine();
                Console.Write("Which campground (enter 0 to cancel)? ");
                userCampground = Console.ReadLine();
                if(userCampground == "0")
                {
                    Console.Clear();
                    break;
                }
                bool canParse = int.TryParse(userCampground, out int num);
                Console.Write("What is the arrival date? (mm/dd/yyyy) ");
                userDateArrive = Console.ReadLine();
                bool canParseArrivalDate = DateTime.TryParse(userDateArrive, out DateTime arrive);
                Console.Write("What is the departure date? (mm/dd/yyyy)");
                userDateDepart = Console.ReadLine();
                bool canParseDepartureDate = DateTime.TryParse(userDateDepart, out DateTime depart);

                Console.Clear();

                if (canParse)
                {
                    foreach (Campground campground in campgrounds)
                    {
                        if (campground.Campground_Id == num)
                        {
                            isValidCampground = true;
                        }
                    }
                }

                if (isValidCampground && canParseArrivalDate && canParseDepartureDate && userCampground != "0" && arrive <= depart)
                {
                    DisplaySitesMatchingSearchCriteriaSelectMenu(userCampground, arrive, depart);

                }
                else if (!isValidCampground)
                {
                    Console.Clear();
                    Console.WriteLine("Please Enter a Valid Campground ID#");
                    Freeze();
                }
                else if (!canParseArrivalDate)
                {
                    Console.Clear();
                    Console.WriteLine("Please Enter your arrival date in the correct format");
                    Freeze();
                }
                else if (!canParseDepartureDate)
                {
                    Console.Clear();
                    Console.WriteLine("Please Enter your departure date in the correct format");
                    Freeze();
                }
                else if(arrive > depart)
                {
                    Console.Clear();
                    Console.WriteLine("Please Enter an arrival date that is an earlier date than the departure date.");
                    Freeze();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("At least one of your inputs is invalid. Please enter valid inputs.");
                    Freeze();
                }
 
            
            }
        }

        private void DisplayParkInformation(string parkId)
        {
            while (true)
            {
                ParkSqlDAL dal = new ParkSqlDAL(connectionString);
                List<Park> parks = dal.GetAllAvailableParks();
                PrintHeader();
                string parkName = "";

                foreach (Park park in parks)
                {
                    if (park.Park_Id.ToString() == parkId)
                    {
                        Console.WriteLine(park.Name + " National Park");
                        parkName = park.Name;
                        Console.WriteLine(("Location:").PadRight(20) + park.Location);
                        Console.WriteLine(("Established:").PadRight(20) + park.Establish_Date.ToString());
                        Console.WriteLine(("Area:").PadRight(20) + park.Area);
                        Console.WriteLine(("Annual Visitors:").PadRight(20) + park.Visitors);
                        Console.WriteLine();
                        Console.WriteLine(park.Description);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Select a Command");
                Console.WriteLine("1) View Campgrounds");
                Console.WriteLine("2) Search for Reservation");
                Console.WriteLine("3) Return to Previous Screen");
                Console.WriteLine();

                string userInput = Console.ReadLine();
                Console.Clear();

                if (userInput != "1" && userInput != "2" && userInput != "3")
                {
                    Console.WriteLine("Please enter a valid input");
                    Freeze();
                }
                else if (userInput == "1")
                {
                    PrintCampgroundsInParkSelectMenu(parkId, parkName);
                }
                else if (userInput == "2")
                {
                    SearchForCampgroundReservation(parkId, parkName);
                }
                else if (userInput == "3")
                {
                    break;
                }
            }
        }

        private void Freeze()
        {
            Console.WriteLine();
            Console.WriteLine("Press ENTER to Continue: ");
            Console.ReadLine();
            Console.Clear();
        }

        private void DisplaySitesMatchingSearchCriteriaSelectMenu(string campId, DateTime arrivalDate, DateTime departureDate) // Add datetimes for arrival and departure?
        {
            while(true)
            {
                SiteSqlDAL dal = new SiteSqlDAL(connectionString);
                SiteSqlDAL dal2 = new SiteSqlDAL(connectionString);
                List<Site> sites = dal.GetAllAvailableCampsites(campId, arrivalDate, departureDate);
                PrintHeader();
                string accessible;
                string utility;
                int confirmationID = -1;                

                Console.Clear();

                if (sites.Count > 0)
                {
                    Console.WriteLine(("Site No.").PadRight(12) + ("Max Occup.").PadRight(12) + ("Accessible?").PadRight(20) + ("Max RV Length").PadRight(20) + ("Utility").PadRight(12) + ("Cost of Stay").PadRight(12));
                    foreach (Site site in sites)
                    {
                        TimeSpan timeSpan = departureDate.Subtract(arrivalDate);
                        int totalDays = (int)timeSpan.TotalDays;
                        decimal cost = totalDays * site.Daily_Fee;

                        if (site.Campground_Id.ToString() == campId)
                        {
                            if (site.Accessible)
                            {
                                accessible = "Yes";
                            }
                            else
                            {
                                accessible = "No";
                            }
                            if (site.Utilities)
                            {
                                utility = "Yes";
                            }
                            else
                            {
                                utility = "N/A";
                            }

                            Console.WriteLine(site.Site_Number.ToString().PadRight(12) + site.Max_Occupancy.ToString().PadRight(12) + accessible.PadRight(20) + site.Max_Rv_Length.ToString().PadRight(20) + utility.PadRight(12) + cost.ToString("C").PadRight(12));
                        }
                    }

                    Console.WriteLine();
                    Console.Write("Which site should be reserved (enter 0 to cancel)? ");
                    string userInputSiteId = Console.ReadLine();

                    if(userInputSiteId == "0")
                    {
                        Console.Clear();
                        break;
                    }

                    Console.Write("What name should the reservation be made under?" );
                    string userInputName = Console.ReadLine();

                    ReservationSqlDAL dalRes = new ReservationSqlDAL(connectionString);
                    
                    foreach(Site site in sites)
                    {
                        if(userInputSiteId == site.Site_Number.ToString())
                        {
                            confirmationID = dalRes.BookReservation(site.Site_Id, userInputName, arrivalDate, departureDate);
                            Console.WriteLine();
                            Console.WriteLine($"The Reservation has been made for {userInputName} and the Confirmation ID# is {confirmationID}");
                            Freeze();
                            break;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("You entered an invalid ID");
                            Freeze();
                        }
                    }
                       
                    Console.Write("Enter any key to add another reservation or Enter (X) to EXIT ");
                    char userInput = Console.ReadKey(false).KeyChar;
                    if(userInput.ToString().ToLower() == "x")
                    {
                        Console.WriteLine();
                        Environment.Exit(0);
                    }
                }

                else
                {
                    Console.WriteLine("****THERE ARE CURRENTLY NO AVAILABLE CAMPSITES IN THAT CAMPGROUND IN THAT TIME PERIOD****");
                    Freeze();
                    break;
                }
            }
        }
    }
}

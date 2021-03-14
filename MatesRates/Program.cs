using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace MRRC
{
    class Program
    {
        static void Main(string[] args)
        {
            
            CRM crm = new CRM("Data\\customers.csv");
            Fleet fleet = new Fleet("Data\\fleet.csv", "Data\\rentals.csv");
            Vehicle vehicle = new Vehicle();
            bool ProgramClose = false;
            Console.SetWindowSize(130, 30);

            //while(Console.ReadKey(true).Key != ConsoleKey.Escape)

            while (!(Console.KeyAvailable) && ProgramClose == false)
            {
                Console.WriteLine("### Mates-Rates Rent-a-Car Operation Menu ### \r\n");
                Console.WriteLine("You may press the ESC key two times at the main menu to exit. Press the BACKSPACE key to return to the previous menu \r\n");
                Console.WriteLine("Please enter a character from the options below: \r\n");
                Console.WriteLine("a) Customer Management");
                Console.WriteLine("b) Fleet Management");
                Console.WriteLine("c) Rental Management\r\n");

                char input = Console.ReadKey().KeyChar;
                if (input == 'a')
                {
                    Console.WriteLine("\r\nPlease enter a character from the options below: \r\n");
                    Console.WriteLine("a) Display Customers");
                    Console.WriteLine("b) New Customer");
                    Console.WriteLine("c) Modify Customer");
                    Console.WriteLine("d) Delete Customer\r\n");

                    // Display Customer
                    input = Console.ReadKey().KeyChar;
                    if (input == 'a')
                    {
                        Customer customer = new Customer();
                        Console.WriteLine(customer.ToCSVString() + "\r\n");
                    } // New Customer
                    else if (input == 'b')
                    {
                        Customer.gender inputGender;
                        Console.WriteLine("\r\nPlease fill the following fields (fields marked with * are required)\r\n");
                        Console.Write("ID*: ");
                        Int32.TryParse(Console.ReadLine(), out int inputID);
                        Console.Write("Title*: ");
                        string inputTitle = Console.ReadLine();
                        Console.Write("First Name*: ");
                        string inputFirstName = Console.ReadLine();
                        Console.Write("Last Name*: ");
                        string inputLastName = Console.ReadLine();
                        Console.Write("Gender*: ");
                        if (Console.ReadLine() == "Male")
                        {
                            inputGender = Customer.gender.Male;
                        }
                        else if (Console.ReadLine() == "Female")
                        {
                            inputGender = Customer.gender.Female;
                        }
                        else
                        {
                            inputGender = Customer.gender.Other;
                        }

                        Console.Write("DOB (DD/MM/YYY format)*: ");
                        string inputDateTime = Console.ReadLine();
                        string[] inputDob = inputDateTime.Split('/');
                        string inputDobDay = string.Format("{0:00}", Convert.ToInt32(inputDob[0]));
                        string inputDobDT = inputDob[2] + '-' + inputDob[1] + '-' + inputDobDay;
                        DateTime inputDOB = DateTime.ParseExact(inputDobDT, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                        Customer inputCustomer = new Customer(inputID, inputTitle, inputFirstName, inputLastName, inputGender, inputDOB);
                        crm.AddCustomer(inputCustomer);
                    } //Modify Customer
                    else if (input == 'c')
                    {
                        Console.WriteLine("\r\n Please enter the following details for the Customer which you wish to change");
                        Console.Write("ID: ");
                        Int32.TryParse(Console.ReadLine(), out int inputID);
                        Console.Write("Attribute: ");
                        string inputAttribute = Console.ReadLine();
                        Console.Write("Value: ");
                        string inputValue = Console.ReadLine();

                        crm.ModifyCustomer(inputID, inputAttribute, inputValue);
                    } //Remove customer
                    else if (input == 'd')
                    {
                        Console.WriteLine("\r\nPlease enter the ID of the Customer which you wish to remove, " +
                            "please note \r\nthat you cannot remove customers that are currently renting a vehicle");
                        Console.Write("ID: ");
                        Int32.TryParse(Console.ReadLine(), out int inputID);
                        crm.RemoveCustomer(inputID, fleet);
                    } else
                    {
                        Console.WriteLine("\r\nInput not recognised, please try again\r\n");
                        Console.ReadKey();
                    }
                }

                //Fleet Management
                else if (input == 'b')
                {
                    Console.WriteLine("\r\nPlease enter a character from the options below: \r\n");
                    Console.WriteLine("a) Display Fleet");
                    Console.WriteLine("b) New Vehicle");
                    Console.WriteLine("c) Modify Vehicle");
                    Console.WriteLine("d) Delete Vehicle\r\n");
                    input = Console.ReadKey().KeyChar;

                    //Display Fleet
                    if (Console.ReadKey().KeyChar == 'a')
                    {
                        Console.WriteLine(vehicle.ToCSVString());
                    }
                    else if (input == 'b') //Add Vehicle
                    {
                        Vehicle.VehicleGrade inputGrade;
                        Vehicle.TransmissionType inputTransmission;
                        Vehicle.FuelType inputFuel;
                        bool inputGPS;
                        bool inputSunroof;

                        Console.WriteLine("\r\nPlease fill the following fields (fields marked with * are required)\r\n");
                        Console.Write("Registration(ABC123 formatting)*: ");
                        string inputRegistration = Console.ReadLine();
                        Console.Write("\r\nGrade*: ");
                        if (Console.ReadLine() == "Economy")
                        {
                            inputGrade = Vehicle.VehicleGrade.Economy;
                        }
                        else if (Console.ReadLine() == "Family")
                        {
                            inputGrade = Vehicle.VehicleGrade.Family;
                        }
                        else if (Console.ReadLine() == "Luxury")
                        {
                            inputGrade = Vehicle.VehicleGrade.Luxury;
                        }
                        else
                        {
                            inputGrade = Vehicle.VehicleGrade.Commercial;
                        }
                        Console.Write("\r\nMake*: ");
                        string inputMake = Console.ReadLine();
                        Console.Write("\r\nModel*: ");
                        string inputModel = Console.ReadLine();
                        Console.Write("\r\nYear*: ");
                        Int32.TryParse(Console.ReadLine(), out int inputYear);
                        Console.Write("\r\nNumber of Seats: ");
                        Int32.TryParse(Console.ReadLine(), out int inputNumSeats);
                        Console.Write("\r\nTransmission: ");
                        if (Console.ReadLine() == "Manual")
                        {
                            inputTransmission = Vehicle.TransmissionType.Manual;
                        }
                        else
                        {
                            inputTransmission = Vehicle.TransmissionType.Automatic;
                        }
                        Console.Write("\r\nFuel: ");
                        if (Console.ReadLine() == "Petrol")
                        {
                            inputFuel = Vehicle.FuelType.Petrol;
                        }
                        else
                        {
                            inputFuel = Vehicle.FuelType.Diesel;
                        }
                        Console.Write("\r\nGPS: ");
                        if (Console.ReadLine() == "true")
                        {
                            inputGPS = true;
                        }
                        else
                        {
                            inputGPS = false;
                        }
                        Console.Write("\r\nSun Roof: ");
                        if (Console.ReadLine() == "true")
                        {
                            inputSunroof = true;
                        }
                        else
                        {
                            inputSunroof = false;
                        }
                        Console.Write("\r\nDaily Rate: ");
                        Double.TryParse(Console.ReadLine(), out double inputDailyRate);
                        Console.Write("\r\nColour: ");
                        string inputColour = Console.ReadLine();

                        Vehicle inputVehicle = new Vehicle(inputRegistration, inputGrade, inputMake, inputModel, inputYear, inputNumSeats,
                            inputTransmission, inputFuel, inputGPS, inputSunroof, inputDailyRate, inputColour);
                        fleet.AddVehicle(inputVehicle);
                    }

                    else if (input == 'c') // Modify Vehicle
                    {
                        Console.WriteLine("\r\nPlease enter the following details for the Vehicle which you wish to change");
                        Console.Write("Registration: ");
                        string inputRegistration = Console.ReadLine();
                        Console.Write("Attribute: ");
                        string inputAttribute = Console.ReadLine();
                        Console.Write("Value: ");
                        string inputValue = Console.ReadLine();

                        fleet.ModifyVehicle(inputRegistration, inputAttribute, inputValue);
                    }
                    else if (input == 'd') //Remove Vehicle
                    {
                        Console.WriteLine("\r\nPlease enter the Registration of the Vehicle which you wish to remove, " +
                            "please note \r\nthat you cannot remove vehicle that are currently being rented");
                        Console.Write("Registration: ");
                        string inputRegistration = Console.ReadLine();
                        fleet.RemoveVehicle(inputRegistration);

                    } else
                    {
                        Console.WriteLine("\r\nInput not recognised, please try again\r\n");
                        Console.ReadKey();
                    }

                }
                else if (input == 'c') //Rental Management
                {
                    Console.WriteLine("\r\nPlease enter a character from the options below: \r\n");
                    Console.WriteLine("a) Display Rentals");
                    Console.WriteLine("b) Search Vehicles");
                    Console.WriteLine("c) Rent Vehicle");
                    Console.WriteLine("d) Return Vehicle\r\n");
                    
                    Rental rental = new Rental();
                    RentalFleet rentalFleet = new RentalFleet ();

                    input = Console.ReadKey().KeyChar;
                    if (input == 'a') // Rental Report
                    {
                        Console.WriteLine("\r\n" + rental.rentalReport());
                    } else if (input == 'b') //Search Vehicles
                    {

                    } else if (input == 'c') // Rent a Vehicle
                    {
                        Console.WriteLine("\r\nPlease fill the following fields\r\n");
                        Console.Write("Registration*: ");
                        string inputRegistration = Console.ReadLine();
                        Console.Write("ID*: ");
                        Int32.TryParse(Console.ReadLine(), out int inputID);

                        Rental inputRental = new Rental(inputRegistration, inputID);

                        rentalFleet.RentVehicle(inputRental);
                    } else if (input == 'c') // Return Vehicle
                    {
                        Console.WriteLine("\r\nPlease fill the following fields\r\n");
                        Console.Write("Registration*: ");
                        string inputRegistration = Console.ReadLine();
                        Console.Write("ID*: ");
                        Int32.TryParse(Console.ReadLine(), out int inputID);

                        rentalFleet.ReturnVehicle(inputRegistration, inputID);
                    } else
                    {
                        Console.WriteLine("\r\nInput not recognised, please try again\r\n");
                        Console.ReadKey();
                    }
                } else if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    ProgramClose = true;
                    Console.WriteLine("\r\nProgam successfully exited");
                }
                else {
                    Console.WriteLine("\r\nInput not recognised, please try again\r\n");
                    Console.ReadKey();
                }
            }

            //RentalFleet rentalFleet = new RentalFleet();
            //Rental testrental = new Rental("851VOJ", 2);
            //Rental testrental = new Rental("519YUY", 5);
            //Rental testrental = new Rental("324YUY", 12);
            //rentalFleet.RentVehicle(testrental);
            //rentalFleet.AddRental(testrental);
            //rentalFleet.ReturnVehicle("602VVZ", 0);

            //Console.WriteLine(rental.rentalReport());


            //DateTime dob = DateTime.ParseExact("1999-02-10", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            //Customer testCustomer = new Customer(51, "Mr", "Fidel", "Seng", Customer.gender.Female, dob);

            //CRM CRMtest = new CRM("Data\\customers.csv");

            //Customer testCustomer = CRMtest.GetCustomer(5);
            //Console.WriteLine(testCustomer.ToCSVString());

            //Fleet testFleet = new Fleet("Data\\fleet.csv", "Data\\rentals.csv");

            //testFleet.IsRented("602VVZ");
            //CRMtest.ModifyCustomer(4, "Male", "Gender");
            //CRMtest.RemoveCustomer(6, testFleet);
            //CRMtest.GetCustomers();

            //testFleet.ModifyVehicle("682GWJ", "2020", "Year");
            //Fleet testFleet = new Fleet("Data\\fleet.csv", "Data\\rentals.csv");
            //testFleet.RemoveVehicle("519YUY", testFleet);
            //CRMtest.AddCustomer(testCustomer);
            //CRMtest.RemoveCustomer(6, testFleet);

            //Fleet testFleet = new Fleet("Data\\fleet.csv", "Data\\rentals.csv");
            //Vehicle testVehicle = new Vehicle("345DSG", Vehicle.VehicleGrade.Economy, "Audi", "D4", 1999, 4, Vehicle.TransmissionType.Automatic, Vehicle.FuelType.Diesel, true, true, 355.3, "Gold");
            //Console.WriteLine(testVehicle.ToCSVString());

            //testFleet.AddVehicle(testVehicle);
            //Fleet testFleet = new Fleet("Data\\fleet.csv", "Data\\rentals.csv");
            Console.ReadKey();
        }
    }
}

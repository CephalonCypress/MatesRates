using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MRRC
{
    class RentalFleet
    {
        private List<Rental> RentalList = new List<Rental>();
        string rentalsFile = "Data\\rentals.csv";
        string fleetFile = "Data\\fleet.csv";
        string crmFile = "Data\\customers.csv";


        public RentalFleet()
        {
            if (File.Exists(rentalsFile))
            {
                this.rentalsFile = rentalsFile;
                RentalList = LoadRentalsFromFile();
            }
            else
            {
                FileStream rentalsFileNew = new FileStream(rentalsFile, FileMode.Create, FileAccess.Write); //Create rentals.csv file
                StreamWriter Writer = new StreamWriter(rentalsFileNew);
                Writer.WriteLine("Registration, CustomerID"); //Write headers
                Writer.Close();
                rentalsFileNew.Close();
            }

        }

        // This method returns the fleet (as a list of Rentals).
        public List<Rental> GetRentedList()
        {
            return RentalList;
        }

        // This method loads the list of rentals from the file.
        private List<Rental> LoadRentalsFromFile()
        {
            if (File.Exists(rentalsFile))
            {
                using (StreamReader reader = new StreamReader(rentalsFile))
                {
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] tokens = line.Split(','); //Split the columns

                        string rentedRegistration = tokens[0];
                        Int32.TryParse(tokens[1], out int rentedID);

                        Rental rentalEntry = new Rental(rentedRegistration, rentedID);
                        RentalList.Add(rentalEntry);
                    }
                    reader.Close();
                }
            }
            return RentalList;
        }

        //// This method rents the vehicle with the provided registration to the customer with
        //// the provided ID, if the vehicle is not currently being rented. It returns true if
        //// the rental was possible, and false otherwise.
        public bool RentVehicle(Rental rental)
        {
            Fleet fleet = new Fleet(fleetFile, rentalsFile);
            CRM crm = new CRM(crmFile);

            List<Vehicle> VehicleFleet = fleet.GetFleet();
            List<Customer> CustomerList = crm.GetCustomers();

            Vehicle vehicleToRent = fleet.GetVehicle(rental.registration);
            Customer customerRenting = crm.GetCustomer(rental.id);

            if (IsRented(rental.registration) == false && IsRenting(rental.id) == false && vehicleToRent != new Vehicle() && customerRenting != new Customer())
            {
                RentalList.Add(rental);
                SaveRentalsToFile();
                Console.WriteLine("Customer " + rental.id + " has successfully rented Vehicle " + rental.registration);
            }
            return true;
        }

        public bool ReturnVehicle(string returningVehicleRegistration, int returningVehicleCustomerID)
        {
            bool rentedVehicle = false;
            bool matchingCustomer = false;

            using (StreamReader reader = new StreamReader(rentalsFile))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] tokens = line.Split(',');
                    string registration = tokens[0];
                    Int32.TryParse(tokens[1], out int rentedVehicleCustomerID);
                    if (registration == returningVehicleRegistration)
                    {
                        rentedVehicle = true;
                    }
                    if (rentedVehicleCustomerID == returningVehicleCustomerID)
                    {
                        matchingCustomer = true;
                        break;
                    }
                }
                reader.Close();

                // Finding the index of the given registration
                int RentedIndex = RentalList.FindIndex(a => a.registration.Equals(returningVehicleRegistration));

                //If the requested registration exists in the data, remove the requested Vehicle
                if (RentedIndex != -1 && rentedVehicle == true && matchingCustomer == true)
                {
                    RentalList.RemoveAt(RentedIndex);
                    Console.WriteLine("Successfully returned Vehicle Registration\"" + returningVehicleRegistration + "\" from Customer " + returningVehicleCustomerID);
                    SaveRentalsToFile();
                    return true;
                }
                else
                {
                    Console.WriteLine("Deletion failed");
                    return false;
                }
            }
        }



        //This method returns the rented Vehicle with the matching registration
        public Rental GetRentedVehicle(string requestedRegistration)
        {
            Rental returnedRentedVehicle = new Rental();
            using (StreamReader reader = new StreamReader(fleetFile))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] tokens = line.Split(',');
                    string registration = tokens[0];

                    if (registration == requestedRegistration)
                    {
                        Int32.TryParse(tokens[1], out int id);
                        returnedRentedVehicle = new Rental(registration, id);
                        break;
                    }
                }
                reader.Close();
            }
            return returnedRentedVehicle;
        }

        //// This method returns whether the vehicle with the provided registration is currently
        //// being rented.
        public bool IsRented(string registration)
        {
            RentalFleet rentalFleet = new RentalFleet();
            List<Rental> RentalList = rentalFleet.GetRentedList();
            bool Rented = false;

            RentalList.ForEach(delegate (Rental RentedVehicle)
            {
                if (RentedVehicle.registration == registration)
                {
                    Rented = true;
                }
            });

            if (Rented == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //// This method returns whether the customer with the provided customer ID is currently
        //// renting a vehicle.
        public bool IsRenting(int customerID)
        {
            RentalFleet rentalFleet = new RentalFleet();
            List<Rental> RentalList = rentalFleet.GetRentedList();
            bool Renting = false;

            RentalList.ForEach(delegate (Rental RentedVehicle)
            {
                if (RentedVehicle.id == customerID)
                {
                    Renting = true;
                }
            });

            if (Renting == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //// This method returns the customer ID of the current renter of the vehicle. If it is
        //// rented by no one, it returns -1. Technically this method can replace IsRented.
        //public int RentedBy(string registration)
        //{

        //}


        // This method saves the current list of rentals to file.
        public void SaveRentalsToFile()
        {
            using (StreamWriter Writer = new StreamWriter(rentalsFile))
            {
                Writer.WriteLine("Registration, CustomerID"); //Write headers
                RentalList.ForEach(delegate (Rental rentalEntry)
                {
                    Writer.WriteLine(rentalEntry.registration + "," + rentalEntry.id);
                });
            Writer.Close();
            }
        }
    }
}

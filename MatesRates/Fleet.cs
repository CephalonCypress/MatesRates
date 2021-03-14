using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MRRC
{
    class Fleet
    {
        private string rentalsFile = "Data\\rentals.csv";
        private string fleetFile = "Data\\fleet.csv";
        private List<Rental> RentalList = new List<Rental>();

        public string FleetFile
        {
            get { return fleetFile; }
            set { fleetFile = value; }
        }
        public string RentalsFile
        {
            get { return rentalsFile; }
            set { rentalsFile = value; }
        }
        private List<Vehicle> VehicleFleet = new List<Vehicle>();

        // Empty Constructor
        public Fleet()
        {
        }

        // If there is no fleet file at the specified location, this constructor constructors
        // an empty fleet and empty rentals. Otherwise it loads the fleet and rentals from the
        // specified files (see the assignment specification).
        public Fleet(string fleetFile, string rentalsFile)
        {
            if (File.Exists(fleetFile))
            {
                this.fleetFile = fleetFile;
                VehicleFleet = LoadVehiclesFromFile();
            }
            else
            {
                FileStream fleetFileNew = new FileStream("Data\\fleet.csv", FileMode.Create, FileAccess.Write); //Create fleet.csv file
                StreamWriter Writer = new StreamWriter(fleetFileNew);
                Writer.WriteLine("Registration, Grade, Make, Model, Year, NumSeats, Transmission, Fuel, GPS, SunRoof, DailyRate, Colour"); //Write headers
                Writer.Close();
                fleetFileNew.Close();
            }

        }

        // Adds the provided vehicle to the fleet assuming the vehicle registration does not
        // already exist. It returns true if the add was successful (the registration did not
        // already exist in the fleet), and false otherwise.
        public bool AddVehicle(Vehicle vehicle)
        {
            // Finding the index of the given registration
            int VehicleIndex = VehicleFleet.FindIndex(a => a.Registration.Equals(vehicle.Registration));

            //If the requested ID does not exist in the data, add the requested customer
            if (VehicleIndex == -1)
            {
                VehicleFleet.Add(vehicle);
                SaveVehiclesToFile();
                Console.WriteLine("Vehicle " + vehicle.Registration + " has been successfully added to the fleet");
                return true;
            }
            else
            {
                return false;
            }
        }

        // This method removes the vehicle with the provided rego from the fleet if it is not
        // currently rented. It returns true if the removal was successful and false otherwise.
        public bool RemoveVehicle(string requestedRegistration)
        {
            bool rentedVehicle = false;
            //RentalFleet rentalFleet = new RentalFleet();
            //List<Rental> RentalFleet = rentalFleet.GetRentedList();

            //If the vehicle is currently being rented, rentedVehicle is false
            using (StreamReader reader = new StreamReader(rentalsFile))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] tokens = line.Split(',');
                    string registration = tokens[0];
                    if (registration == requestedRegistration)
                    {
                        rentedVehicle = true;
                        break;
                    }
                }
            }

            // Finding the index of the given registration
            int VehicleIndex = VehicleFleet.FindIndex(a => a.Registration.Equals(requestedRegistration));

            //If the requested registration exists in the data, remove the requested Vehicle
            if (VehicleIndex != -1 && rentedVehicle == false)
            {
                VehicleFleet.RemoveAt(VehicleIndex);
                Console.WriteLine("Vehicle " + requestedRegistration + " has been successfully deleted from the database");
                SaveVehiclesToFile();
                return true;
            }
            else
            {
                //Console.WriteLine("Deletion failed, does not exist");
                return false;
            }
        }

        //Modifies the Vehicle with the matching registration, returns true if successful
        public bool ModifyVehicle(string requestedRegistration, string ChangingAttribute, string value)
        {
            Vehicle VehicleEntry = GetVehicle(requestedRegistration);

            if (ChangingAttribute == "Registration")
            {
                VehicleEntry.Registration = value;
            }
            else if (ChangingAttribute == "Grade")
            {
                if (value == "Economy")
                {
                    VehicleEntry.Grade = Vehicle.VehicleGrade.Economy;
                } else if (value == "Family")
                {
                    VehicleEntry.Grade = Vehicle.VehicleGrade.Family;
                } else if (value == "Luxury")
                {
                    VehicleEntry.Grade = Vehicle.VehicleGrade.Luxury;
                } else if (value == "Commercial")
                {
                    VehicleEntry.Grade = Vehicle.VehicleGrade.Commercial;
                }
            }
            else if (ChangingAttribute == "Make")
            {
                VehicleEntry.Make = value;
            }
            else if (ChangingAttribute == "Model")
            {
                VehicleEntry.Model = value;
            }
            else if (ChangingAttribute == "Year")
            {
                VehicleEntry.Year = Convert.ToInt32(value);
            }
            else if (ChangingAttribute == "NumSeats")
            {
                VehicleEntry.NumSeats = Convert.ToInt32(value);
            }
            else if (ChangingAttribute == "Transmission")
            {
                if (value == "Manual")
                {
                    VehicleEntry.Transmission = Vehicle.TransmissionType.Manual;
                }
                else if (value == "Automatic")
                {
                    VehicleEntry.Transmission = Vehicle.TransmissionType.Automatic;
                }
            }
            else if (ChangingAttribute == "Fuel")
            {
                if (value == "Petrol")
                {
                    VehicleEntry.Fuel = Vehicle.FuelType.Petrol;
                }
                else if (value == "Diesel")
                {
                    VehicleEntry.Fuel = Vehicle.FuelType.Diesel;
                }
            }
            else if (ChangingAttribute == "GPS")
            {
                if (value == "true")
                {
                    VehicleEntry.GPS = true;
                }
                else if (value == "false")
                {
                    VehicleEntry.GPS = false;
                }
            }
            else if (ChangingAttribute == "SunRoof")
            {
                if (value == "true")
                {
                    VehicleEntry.SunRoof = true;
                }
                else if (value == "false")
                {
                    VehicleEntry.SunRoof = false;
                }
            }
            else if (ChangingAttribute == "Daily Rate")
            {
                VehicleEntry.DailyRate = Convert.ToDouble(value);
            }
            else if (ChangingAttribute == "Colour")
            {
                VehicleEntry.Colour = value;
            }

            //Deleting the Vehicle from the Fleet
            //Fleet fleet = new Fleet();
            int index = VehicleFleet.FindIndex(a => a.Registration.Equals(requestedRegistration));
            VehicleFleet.RemoveAt(index);
            SaveVehiclesToFile();

            //Re-instantiating the modified Vehicle
            VehicleEntry = new Vehicle(VehicleEntry.Registration, VehicleEntry.Grade, VehicleEntry.Make, VehicleEntry.Model, VehicleEntry.Year, VehicleEntry.NumSeats, 
                VehicleEntry.Transmission, VehicleEntry.Fuel, VehicleEntry.GPS, VehicleEntry.SunRoof, VehicleEntry.DailyRate, VehicleEntry.Colour);

            //Adding the modified Vehicle back into the Fleet
            AddVehicle(VehicleEntry);

            if (VehicleFleet.Contains(VehicleEntry))
            {
                Console.WriteLine("Successfully changed Customer " + VehicleEntry.Registration + "'s " + ChangingAttribute + " to " + value);
                return true;
            }
            else
            {
                return false;
            }
        }

        // This method returns the fleet (as a list of Vehicles).
        public List<Vehicle> GetFleet()
        {
            return VehicleFleet;
        }

        //// This method returns the vehicle with the matching registration
        public Vehicle GetVehicle(string requestedRegistration)
        {
            Vehicle returnedVehicle = new Vehicle();

            using (StreamReader reader = new StreamReader(fleetFile))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] tokens = line.Split(',');
                    Vehicle.VehicleGrade VehicleGrade;
                    Vehicle.TransmissionType VehicleTransmission;
                    Vehicle.FuelType VehicleFuel;
                    bool sunRoof;
                    bool GPS;

                    string registration = tokens[0];

                    if (registration == requestedRegistration)
                    {
                        if (tokens[1] == "Commercial")
                        {
                            VehicleGrade = Vehicle.VehicleGrade.Commercial;
                        }
                        else if (tokens[1] == "Economy")
                        {
                            VehicleGrade = Vehicle.VehicleGrade.Economy;
                        }
                        else if (tokens[1] == "Family")
                        {
                            VehicleGrade = Vehicle.VehicleGrade.Family;
                        }
                        else
                        {
                            VehicleGrade = Vehicle.VehicleGrade.Luxury;
                        }
                        string Make = tokens[2];
                        string Model = tokens[3];
                        int Year = int.Parse(tokens[4]);
                        int NumSeats = int.Parse(tokens[5]);
                        if (tokens[6] == "Manual")
                        {
                            VehicleTransmission = Vehicle.TransmissionType.Manual;
                        }
                        else
                        {
                            VehicleTransmission = Vehicle.TransmissionType.Automatic;
                        }
                        if (tokens[7] == "Diesel")
                        {
                            VehicleFuel = Vehicle.FuelType.Diesel;
                        }
                        else
                        {
                            VehicleFuel = Vehicle.FuelType.Petrol;
                        }
                        if (tokens[8] == "TRUE")
                        {
                            sunRoof = true;
                        }
                        else
                        {
                            sunRoof = false;
                        }
                        if (tokens[9] == "TRUE")
                        {
                            GPS = true;
                        }
                        else
                        {
                            GPS = false;
                        }
                        double dailyRate = double.Parse(tokens[10]);
                        string Colour = tokens[11];

                        returnedVehicle = new Vehicle(registration, VehicleGrade, Make, Model, Year, NumSeats, VehicleTransmission, VehicleFuel, sunRoof, GPS, dailyRate, Colour);
                        break;
                    }
                }
            }
            return returnedVehicle;
        }

        //// This method returns a subset of vehicles in the fleet depending on whether they are
        //// currently rented. If rented is true, this method returns all rented vehicles. If it
        //// false, this method returns all not yet rented vehicles.
        public List<Vehicle> GetRentedFleet(bool rented)
        {
            RentalFleet rentalFleet = new RentalFleet();
            RentalList = rentalFleet.GetRentedList();
            List<Vehicle> RentedVehicles = new List<Vehicle>();
            List<Vehicle> UnrentedVehicles = new List<Vehicle>();

        //Adding Vehicles to appropriate list if the registration matches
        VehicleFleet.ForEach(delegate (Vehicle VehicleEntry)
        {
            RentalList.ForEach(delegate (Rental VehicleRentedRegistration)
            {

                if (VehicleEntry.Registration == VehicleRentedRegistration.registration)
                {
                    RentedVehicles.Add(VehicleEntry);
                }
                else
                {
                    UnrentedVehicles.Add(VehicleEntry);
                }
            });
        });
            
        //Return appropriate list depending on whether "rented" is true or false
        if (rented == true)
        {
            return RentedVehicles;
        } else
        {
            UnrentedVehicles = UnrentedVehicles.Distinct().ToList(); // Remove duplicates

            RentedVehicles.ForEach(delegate (Vehicle RentedVehicle) // Remove unintentional entries
            {
                if (RentedVehicles.Contains(RentedVehicle))
                    UnrentedVehicles.Remove(RentedVehicle);
            });
            return UnrentedVehicles;
        }
    }


        //// This method returns a vehicle. If the return is successful (it was currently being
        //// rented) it returns the customer ID of the customer who was renting it, otherwise it
        //// returns -1.
        //public int ReturnVehicle(string registration)
        //{

        //}

        // This method saves the current list of vehicles to file.
        public void SaveVehiclesToFile()
        {
            using (StreamWriter Writer = new StreamWriter(fleetFile))
            {
                    Writer.WriteLine("Registration, Grade, Make, Model, Year, NumSeats, Transmission, Fuel, GPS, SunRoof, DailyRate, Colour"); //Write headers
                    VehicleFleet.ForEach(delegate (Vehicle VehicleEntry)
                    {
                        Writer.WriteLine(VehicleEntry.Registration + "," + VehicleEntry.Grade + "," + VehicleEntry.Make + "," + VehicleEntry.Model + "," + VehicleEntry.Year + "," + VehicleEntry.NumSeats + "," +
                            VehicleEntry.Transmission + "," + VehicleEntry.Fuel + "," + VehicleEntry.GPS + "," + VehicleEntry.SunRoof + "," + VehicleEntry.DailyRate + "," + VehicleEntry.Colour);
                    
                });
                Writer.Close();
            }
        }


        // This method loads the list of vehicles from the file.
        private List<Vehicle> LoadVehiclesFromFile()
        {
            using (StreamReader reader = new StreamReader(fleetFile))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] tokens = line.Split(',');
                    Vehicle.VehicleGrade VehicleGrade;
                    Vehicle.TransmissionType VehicleTransmission;
                    Vehicle.FuelType VehicleFuel;
                    bool sunRoof;
                    bool GPS;

                    string Registration = tokens[0];
                    if (tokens[1] == "Commercial")
                    {
                        VehicleGrade = Vehicle.VehicleGrade.Commercial;
                    }
                    else if (tokens[1] == "Economy")
                    {
                        VehicleGrade = Vehicle.VehicleGrade.Economy;
                    }
                    else if (tokens[1] == "Family")
                    {
                        VehicleGrade = Vehicle.VehicleGrade.Family;
                    }
                    else
                    {
                        VehicleGrade = Vehicle.VehicleGrade.Luxury;
                    }
                    string Make = tokens[2];
                    string Model = tokens[3];
                    int Year = int.Parse(tokens[4]);
                    int NumSeats = int.Parse(tokens[5]);
                    if (tokens[6] == "Manual")
                    {
                        VehicleTransmission = Vehicle.TransmissionType.Manual;
                    }
                    else
                    {
                        VehicleTransmission = Vehicle.TransmissionType.Automatic;
                    }
                    if (tokens[7] == "Diesel")
                    {
                        VehicleFuel = Vehicle.FuelType.Diesel;
                    }
                    else
                    {
                        VehicleFuel = Vehicle.FuelType.Petrol;
                    }
                    if (tokens[8] == "TRUE")
                    {
                        sunRoof = true;
                    }
                    else
                    {
                        sunRoof = false;
                    }
                    if (tokens[9] == "TRUE")
                    {
                        GPS = true;
                    }
                    else
                    {
                        GPS = false;
                    }
                    double dailyRate = double.Parse(tokens[10]);
                    string Colour = tokens[11];

                    Vehicle VehicleEntry = new Vehicle(Registration, VehicleGrade, Make, Model, Year, NumSeats, VehicleTransmission, VehicleFuel, sunRoof, GPS, dailyRate, Colour);
                    VehicleFleet.Add(VehicleEntry);
                }
                reader.Close();
                return VehicleFleet;
            }
        }


    }
}

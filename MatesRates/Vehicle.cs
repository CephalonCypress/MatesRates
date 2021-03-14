using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRC
{
    public class Vehicle
    {
        public enum VehicleGrade
        {
            Economy = 0,
            Family = 1,
            Luxury = 2,
            Commercial = 3
        }

        public enum TransmissionType
        {
            Manual = 0,
            Automatic = 1
        }

        public enum FuelType
        {
            Petrol = 0,
            Diesel = 1
        }

        private string registration;
        public string Registration
        {
            get { return registration; }
            set { registration = value; }
        }

        private VehicleGrade grade;
        public VehicleGrade Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        private string make;
        public string Make
        {
            get { return make; }
            set { make = value; }
        }

        private string model;
        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        private int year;
        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        private int numSeats;
        public int NumSeats
        {
            get { return numSeats; }
            set { numSeats = value; }
        }

        private TransmissionType transmission;
        public TransmissionType Transmission
        {
            get { return transmission; }
            set { transmission = value; }
        }

        private FuelType fuel;
        public FuelType Fuel
        {
            get { return fuel; }
            set { fuel = value; }
        }

        private bool gps;
        public bool GPS
        {
            get { return gps; }
            set { gps = value; }
        }
        private bool sunRoof;
        public bool SunRoof
        {
            get { return sunRoof; }
            set { sunRoof = value; }
        }
        private double dailyRate;
        public double DailyRate
        {
            get { return dailyRate; }
            set { dailyRate = value; }
        }
        private string colour;
        public string Colour
        {
            get { return colour; }
            set { colour = value; }
        }

        // This constructor provides only the mandatory parameters of the vehicle. Others are
        // set based on the defaults of each class.
        // Overall defaults:
        // - 4-seater
        // - Manual transmission
        // - Petrol fuel
        // - No GPS
        // - No sun-roof
        // - $50/day
        // - Black
        // Economy vehicles:
        // - Automatic transmission
        // Family vehicles:
        // - $80/day
        // Luxury vehicles:
        // - Has GPS
        // - Has sun-roof
        // - $120/day
        // Commercial vehicle:
        // - Diesel fuel
        // - $130/day
        public Vehicle(string registration, VehicleGrade grade, string make, string model, 
            int year, int numSeats = 4, TransmissionType transmission = 0, FuelType fuel = 0, 
            bool GPS = false, bool sunRoof = false, double dailyRate = 50, string colour = "black")
        {
            if (grade == VehicleGrade.Economy)
            {
                Transmission = TransmissionType.Automatic;
            }
            else if (grade == VehicleGrade.Family)
            {
                DailyRate = 80;
            } else if (grade == VehicleGrade.Luxury)
            {
                GPS = true;
                SunRoof = true;
                DailyRate = 120;
            } else if (grade == VehicleGrade.Commercial)
            {
                Fuel = FuelType.Diesel;
                DailyRate = 130;
            }

            Registration = registration;
            Grade = grade;
            Make = make;
            Model = model;
            Year = year;
            NumSeats = numSeats; //MUST BE BETWEEN 2 AND 10
            Transmission = transmission;
            Fuel = fuel;
            gps = GPS;
            SunRoof = sunRoof;
            DailyRate = dailyRate;
            Colour = colour;
        }

        // Empty Constructor
        public Vehicle()
        {
        }


        // This method should return a CSV representation of the vehicle that is consistent
        // with the provided data files.
        public string ToCSVString()
        {
            Fleet fleet = new Fleet("Data\\fleet.csv", "Data\\rentals.csv");
            List<Vehicle> VehicleFleet = fleet.GetFleet();
            string CSVString;
            CSVString = "|------------------------------------------------------------------------------------------------------------------------------|\r\n";
            CSVString += "|Registration |Grade      |Make       | Model       |Year  |NumSeats |Transmission |Fuel   |GPS    |SunRoof |DailyRate |Colour |\r\n";
            CSVString += "|------------------------------------------------------------------------------------------------------------------------------|\r\n";

            VehicleFleet.ForEach(delegate (Vehicle VehicleEntry)
            {
                CSVString += String.Format("|{0, -13}|{1, -11}|{2, -11}|{3, -13}|{4, -6}|{5, -9}|{6, -13}|{7, -7}|{8, -7}|{9, -8}|{10, -10}|{11, -7}|\r\n",
                    VehicleEntry.Registration,
                    VehicleEntry.Grade,
                    VehicleEntry.Make,
                    VehicleEntry.Model,
                    VehicleEntry.Year,
                    VehicleEntry.NumSeats,
                    VehicleEntry.Transmission,
                    VehicleEntry.Fuel,
                    VehicleEntry.GPS,
                    VehicleEntry.SunRoof,
                    VehicleEntry.DailyRate,
                    VehicleEntry.Colour
                    );
            });
            CSVString += "|------------------------------------------------------------------------------------------------------------------------------|";

            return CSVString;
        }

        //// This method should return a string representation of the attributes of the vehicle.
        public override string ToString()
        {
            string Header;
            Header = "The Vehicle class is defined by the following attributes; \r\n";
            Header += "<Registration> <Grade> <Make> <Model> <Year> <NumSeats> <Transmission> <Fuel> <GPS> <SunRoof> <DailyRate> <Colour> \r\n";
            Header += "Registration is the unique 6 character alphanumerical identifier that can be used to identify any specific Vehicle";
            Header += "Grade signifies the type of car whether it is Economy, Family, Luxury or Commercial";
            Header += "Make, Model and Year signify the Brand, product range and year of production by the manufacturer";
            Header += "NumSeats is how many seats are available in the car";
            Header += "Transmission is either Automatic or Manual Transmission"; ;
            Header += "Fuel is the type of fuel that it takes";
            Header += "GPS signifies whether it has GPS or not";
            Header += "SunRoof signifies whether it has a Sun roof or not";
            Header += "DailyRate is how much it costs to rent the car per day";
            Header += "Colour is the colour of the car";
            return Header;
        }

        //// This method should return a list of strings which represent each attribute. Values
        //// should be made to be unique (e.g. numSeats should not be written as “4” but as “4
        //// Seater”, sunroof should not be written as “True” but as “sunroof” or with no string
        //// added if there is no sunroof). Vehicle rego, grade, make, model, year, transmission
        //// type, fuel type, and colour can all be assumed to not overlap (i.e. if the make
        //// “Mazda” exists, “Mazda” will not exist in other attributes). You do not need to
        //// maintain this restriction, only assume it is true. You do not need to add the daily
        //// rate to this list.
        //public List<string> GetAttributeList()
        //{

        //}
    }
}

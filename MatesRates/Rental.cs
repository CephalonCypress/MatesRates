using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MRRC
{
    class Rental
    {
        private List<Rental> RentalList = new List<Rental>();
        string rentalsFile = "Data\\rentals.csv";
        string fleetFile = "Data\\fleet.csv";

        private string Registration;
        public string registration
        {
            get { return Registration; }
            set { Registration = value; }
        }

        private int ID;
        public int id
        {
            get { return ID; }
            set { ID = value; }
        }

        //Empty Constructor
        public Rental()
        {
        }

        //Constructor
        public Rental(string registration, int ID)
        {
            this.id = ID;
            this.Registration = registration;
        }

        public string rentalReport()
        {
            Fleet fleet = new Fleet("Data\\fleet.csv", "Data\\rentals.csv");
            CRM crm = new CRM("Data\\customers.csv");
            RentalFleet rentalFleet = new RentalFleet();

            List<Customer> CustomerList = crm.GetCustomers();
            List<Vehicle> RentedFleet = fleet.GetRentedFleet(true);
            List<Rental> RentalList = rentalFleet.GetRentedList();
            string rentalReport;

            rentalReport = "|--------------------------------------------------------------------|\r\n";
            rentalReport += "|ID |Registration |Title |First Name     |Last Name      |Daily Rate |\r\n";
            rentalReport += "|--------------------------------------------------------------------|\r\n";

            RentalList.ForEach(delegate (Rental RentalEntry)
            {
                CustomerList.ForEach(delegate (Customer CustomerEntry)
                {
                    RentedFleet.ForEach(delegate (Vehicle RentedVehicle)
                    {
                        if (RentalEntry.id == CustomerEntry.id && RentedVehicle.Registration == RentalEntry.registration)
                        {
                            rentalReport += String.Format("|{0, -3}|{4, -13}|{1, -6}|{2, -15}|{3, -15}|${5,-10:#.00}|\r\n",
                            CustomerEntry.id, CustomerEntry.Title, CustomerEntry.FirstName, CustomerEntry.LastName, RentedVehicle.Registration, RentedVehicle.DailyRate);
                        }
                    });
                });
            });
            rentalReport += "|--------------------------------------------------------------------|\r\n";

            return rentalReport;
        }
    }
}



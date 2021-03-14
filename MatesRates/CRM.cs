using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRC
{
    class CRM
    {
        private string crmFile = "Data\\customers.csv";
        private List<Customer> CustomerList = new List<Customer>();

        // If there is no CRM file at the specified location, this constructor constructors an
        // empty CRM with no customers. Otherwise it loads the customers from the specified file // (see the assignment specification).
        public CRM(string crmFile)
        {
            if (File.Exists(crmFile))
            {
                this.crmFile = crmFile;
                CustomerList = LoadFromFile();
            }
            else
            {
                FileStream crmFileNew = new FileStream("Data\\customers.csv", FileMode.Create, FileAccess.Write); //Create customer.csv file
                StreamWriter Writer = new StreamWriter(crmFileNew);
                Writer.WriteLine("ID, Title, FirstName, LastName, Gender, DOB"); //Write headers
                Writer.Close();
                crmFileNew.Close();
            }
        }

        // This method adds the provided customer to the customer list if the customer ID doesn’t
        // already exist in the CRM. It returns true if the addition was successful (the customer
        // ID didn’t already exist in the CRM) and false otherwise.
        public bool AddCustomer(Customer customer)
        {
            // Finding the index of the given ID
            int CustomerIndex = CustomerList.FindIndex(a => a.id.Equals(customer.id));

            //If the requested ID does not exist in the data, add the requested customer
            if (CustomerIndex == -1)
            {
                CustomerList.Add(customer);
                Console.WriteLine("Successfully added " + customer.Title + " " + customer.FirstName + " " + customer.LastName + " to the Database");
                SaveToFile();
                return true;
            }
            else
            {
                return false;
            }
        }

        

        // This method removes the customer with the provided customer ID from the CRM if they
        // are not currently renting a vehicle. It returns true if the removal was successful,
        // otherwise it returns false.
        public bool RemoveCustomer(int requestedID, Fleet fleet)
        {
            bool rentingVehicle = false;
            int ID;

            // Checking if the Customer is currently renting a vehicle
            using (StreamReader reader = new StreamReader(fleet.RentalsFile))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] tokens = line.Split(',');
                    Int32.TryParse(tokens[1], out ID);
                    if (ID == requestedID)
                    {
                        rentingVehicle = true;
                        break;
                    }
                }
            }

            // Finding the index of the given ID
            int CustomerIndex = CustomerList.FindIndex(a => a.id.Equals(requestedID));

            //If the requested ID exists in the data, remove the requested customer
            if (CustomerIndex != -1 && rentingVehicle == false)
            {
                CustomerList.RemoveAt(CustomerIndex);
                SaveToFile();
                Console.WriteLine("Successfully deleted Customer " + requestedID + " from the Database");
                return true;
            }
            else
            {
                //Console.WriteLine("Deletion failed, does not exist");
                return false;
            }
        }

        //Modifies the Customer with the matching ID, returns true if successful
        public bool ModifyCustomer(int CustomerID, string ChangingAttribute, string value)
        {
            Customer CustomerEntry = GetCustomer(CustomerID);
            DateTime DOB = CustomerEntry.DOB;

            if (ChangingAttribute == "Title")
            {
                CustomerEntry.Title = value;
            }
            else if (ChangingAttribute == "FirstName")
            {
                CustomerEntry.FirstName = value;
            }
            else if (ChangingAttribute == "LastName")
            {
                CustomerEntry.LastName = value;
            }
            else if (ChangingAttribute == "Gender")
            {
                if (value == "Male")
                {
                    CustomerEntry.Sex = Customer.gender.Male;
                }
                else if (value == "Female")
                {
                    CustomerEntry.Sex = Customer.gender.Female;
                }
                else
                {
                    CustomerEntry.Sex = Customer.gender.Other;
                }               
            }
            else if (ChangingAttribute == "DOB")
            {
                string[] CustomerDob = value.Split('/');
                string CustomerDobDay = string.Format("{0:00}", Convert.ToInt32(CustomerDob[0]));
                string CustomerDobDT = CustomerDob[2] + '-' + CustomerDob[1] + '-' + CustomerDobDay;
                DOB = DateTime.ParseExact(CustomerDobDT, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }

            //Deleting the customer from the List
            Fleet fleet = new Fleet();
            int index = CustomerList.FindIndex(a => a.id.Equals(CustomerID));
            RemoveCustomer(index, fleet);

            //Re-initialising the modified Customer
            CustomerEntry = new Customer(CustomerID, CustomerEntry.Title, CustomerEntry.FirstName, CustomerEntry.LastName, CustomerEntry.Sex, DOB);

            //Adding the customer back into the list
            AddCustomer(CustomerEntry);

            if (CustomerList.Contains(CustomerEntry))
            {
                Console.WriteLine("Successfully changed Customer " + CustomerID + "'s" + ChangingAttribute + "to " + value);

                return true;
            } else
            {
                return false;
            }
        }

        //This method returns the list of current customers.
        public List<Customer> GetCustomers()
        {
            return CustomerList;
        }

        //// This method returns the customer who matches the provided ID.
        public Customer GetCustomer(int requestedID)
        {
            Customer returnedCustomer = new Customer();

            using (StreamReader reader = new StreamReader(crmFile))
            {
                reader.ReadLine();
                

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] tokens = line.Split(',');
                    int customerID = int.Parse(tokens[0]);

                    if (customerID == requestedID)
                    {
                        Customer.gender CustomerGender;

                        string title = tokens[1];
                        string firstName = tokens[2];
                        string lastName = tokens[3];
                        if (tokens[4] == "Male")
                        {
                            CustomerGender = Customer.gender.Male;
                        }
                        else if (tokens[4] == "Female")
                        {
                            CustomerGender = Customer.gender.Female;
                        }
                        else
                        {
                            CustomerGender = Customer.gender.Other;
                        }
                        Customer.gender gender = CustomerGender;
                        string[] CustomerDob = tokens[5].Split('/');
                        string CustomerDobDay = string.Format("{0:00}", Convert.ToInt32(CustomerDob[0]));
                        string CustomerDobDT = CustomerDob[2] + '-' + CustomerDob[1] + '-' + CustomerDobDay;
                        DateTime DOB = DateTime.ParseExact(CustomerDobDT, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        returnedCustomer = new Customer(customerID, title, firstName, lastName, gender, DOB);
                        break;
                    }
                }
                reader.Close();
            }
            return returnedCustomer;
        }

        // This method saves the current state of the CRM to file.
        public void SaveToFile()
        {
            FileStream crmFileNew = new FileStream("Data\\customers.csv", FileMode.Create, FileAccess.Write); //Create customer.csv file

            using (StreamWriter Writer = new StreamWriter(crmFileNew))
            {
                Writer.WriteLine("ID, Title, FirstName, LastName, Gender, DOB");
                CustomerList.ForEach(delegate (Customer CustomerEntry)
                {
                    Writer.WriteLine(CustomerEntry.id + "," + CustomerEntry.Title + "," + CustomerEntry.FirstName + "," +
                            CustomerEntry.LastName + "," + CustomerEntry.Sex + "," + CustomerEntry.DOB.ToShortDateString());
                    
                });
                Writer.Close();
            }
        }

        //This method loads the state of the CRM from file.
        public List<Customer> LoadFromFile()
        {
            if (File.Exists(crmFile))
            {
                using (StreamReader reader = new StreamReader(crmFile))
                {
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] tokens = line.Split(','); //Split the columns
                        Customer.gender CustomerGender;

                        int ID = Int32.Parse(tokens[0]);
                        string title = tokens[1];
                        string firstName = tokens[2];
                        string lastName = tokens[3];
                        if (tokens[4] == "Male") //Resassigning the Customer's gender
                        {
                            CustomerGender = Customer.gender.Male;
                        }
                        else if (tokens[4] == "Female")
                        {
                            CustomerGender = Customer.gender.Female;
                        }
                        else
                        {
                            CustomerGender = Customer.gender.Other;
                        }
                        Customer.gender gender = CustomerGender;
                        string[] CustomerDob = tokens[5].Split('/'); //Converting the DateTime into the correct format
                        string CustomerDobDay = string.Format("{0:00}", Convert.ToInt32(CustomerDob[0]));
                        string CustomerDobDT = CustomerDob[2] + '-' + CustomerDob[1] + '-' + CustomerDobDay;
                        DateTime DOB = DateTime.ParseExact(CustomerDobDT, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                        //CustomerList.ForEach(delegate (Customer CustomerEntry)
                        //{
                        //    Console.WriteLine(CustomerEntry.id);
                        //    Console.WriteLine(CustomerEntry.Title);
                        //    Console.WriteLine(CustomerEntry.FirstName);
                        //    Console.WriteLine(CustomerEntry.LastName);
                        //    Console.WriteLine(CustomerEntry.Sex);
                        //    Console.WriteLine(CustomerEntry.DOB.ToShortDateString());
                        //});

                        Customer customerEntry = new Customer(ID, title, firstName, lastName, gender, DOB);
                        CustomerList.Add(customerEntry);
                    }
                    reader.Close();

                }
            }
            return CustomerList;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRC
{
    class Customer
    {
        private string crmFile = "Data\\customers.csv";

        public enum gender
        {
            Male = 0,
            Female = 1,
            Other = 2
        }

        private int ID;
        public int id
        {
            get { return ID; }
            set { ID = value; }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private gender sex;
        public gender Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        private DateTime dob;
        public DateTime DOB
        {
            get { return dob; }
            set { dob = value; }
        }

        // Empty Constructor
        public Customer()
        {
        }

        // Empty Constructor
        public Customer(int ID)
        {
            this.id = ID;
        }


        // This constructor should construct a customer with the provided attributes.
        public Customer(int ID, string title, string firstName, string lastName, gender sex, DateTime DOB)
        {
            this.id = ID;
            this.Title = title;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Sex = sex;
            this.dob = DOB;
        }

        public string ToCSVString()
        {
            CRM crm = new CRM(crmFile);
            List<Customer> CustomerList = crm.GetCustomers();
            string CSVString;
            CSVString = "|----------------------------------------------------------------------|\r\n";
            CSVString += "|ID   |Title  |First Name     |Last Name      |Gender    |DOB          |\r\n";
            CSVString += "|----------------------------------------------------------------------|\r\n";

            CustomerList.ForEach(delegate(Customer CustomerEntry){
                CSVString += String.Format("|{0, -5}|{1, -7}|{2, -15}|{3, -15}|{4, -10}|{5, -13}|\r\n", 
                    CustomerEntry.id, CustomerEntry.title, CustomerEntry.firstName, CustomerEntry.LastName, CustomerEntry.Sex, CustomerEntry.DOB.ToShortDateString());
            });
            CSVString += "|----------------------------------------------------------------------|";

            return CSVString;
        }

    // This method should return a string representation of the attributes of the customer.
    public override string ToString()
        {
            string Header;
            Header = "The Customer class is defined by the following attributes; \r\n";
            Header += "<ID> <Title> <FirstName> <LastName> <Gender> <DOB> \r\n \r\n";
            Header += "ID is a unique identifier that is assigned to every customer so that they are distinguishable  \r\n" +
                "even if every other attribute is identical \r\n \r\n";
            Header += "Title, FirstName and LastName are the Customer's names and titles \r\n \r\n";
            Header += "Gender is the Customer's gender, if they identify as neither Male or Female, they are listed as \"Other\" \r\n \r\n";
            Header += "DOB is the Customer's Date of Birth in the format DD/MM/YYYY";
            return Header;
        }
    }
}

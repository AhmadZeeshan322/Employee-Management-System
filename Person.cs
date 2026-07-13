using System;
namespace EMS
{
    class Person
    {
        //Static variable
        public static int totalPersons = 0;
        //Const variable
        public const string companyName = "Solution Park";
        //Readonly variable
        public readonly int personId;
        //Private variables (Encapsulation)
        private string name;
        private int age;
        //Protected variable
        protected string address;
        //Default Constructor
        public Person()
        {
            personId = 0;
            name = "Unknown";
            age = 0;
            address = "Not Available";
            totalPersons++;
        }
        //Parameterized Constructor
        public Person(int id, string name, int age)
        {
            this.personId = id;
            this.name = name;
            this.age = age;
            this.address = "Pakistan";
            totalPersons++;
        }
        //Property for Name
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        //Property for Age
        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
            }
        }
        //Virtual method for overriding
        public virtual void DisplayInfo()
        {
            Console.WriteLine("Person ID: " + personId);
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("Age: " + Age);
            Console.WriteLine("Company: " + companyName);
        }
    }
}
using System;
namespace EMS
{
    class Developer : Employee
    {
        //Private variable
        private string progLang;
        //Default Constructor
        public Developer() : base()
        {
            progLang = "Not Assigned";
            Department = "Development";
        }
        //Parameterized Constructor
        public Developer(int personId, string name, int age, int empId, double salary, string progLang) : base(personId, name, age, empId, salary)
        {
            this.progLang = progLang;
            Department = "Development";
        }
        //Property for Programming Language
        public string ProgLang
        {
            get
            {
                return progLang;
            }
            set
            {
                progLang = value;
            }
        }
        //Method Overriding
        public override double CalculateSalary()
        {
            return Salary + 5000;
        }
        // Override DisplayInfo
        public override void DisplayInfo()
        {
            base.DisplayInfo();

            Console.WriteLine("Programming Language: " + ProgLang);
            Console.WriteLine("Final Salary: " + CalculateSalary());
        }
    }
}
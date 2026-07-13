using System;
namespace EMS
{
    class HR : Employee
    {
        //Private variable
        private int empHired;
        //Default Constructor
        public HR() : base()
        {
            empHired = 0;
            Department = "Human Resources";
        }
        //Parameterized Constructor
        public HR(int personId, string name, int age, int empId, double salary, int empHired) : base(personId, name, age, empId, salary)
        {
            this.empHired = empHired;
            Department = "Human Resources";
        }
        //Property
        public int EmpHired
        {
            get
            {
                return empHired;
            }

            set
            {
                empHired = value;
            }
        }
        //Override CalculateSalary
        public override double CalculateSalary()
        {
            return Salary + 3000;
        }
        //Override DisplayInfo
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine("Employees Hired: " + EmpHired);
            Console.WriteLine("Final Salary: " + CalculateSalary());
        }
    }
}
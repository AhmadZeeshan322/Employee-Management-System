using System;
namespace EMS
{
    class Employee : Person
    {
        //Static variable to count employees
        public static int totalEmp = 0;
        //Private variables
        private int empId;
        private double salary;
        //Protected variable
        protected string department;
        //Default Constructor
        public Employee() : base()
        {
            empId = 0;
            salary = 0;
            department = "Not Assigned";
            totalEmp++;
        }
        //Parameterized Constructor
        public Employee(int personId, string name, int age, int empId, double salary) : base(personId, name, age)
        {
            this.empId = empId;
            this.salary = salary;
            department = "General";                  
            totalEmp++;
        }
        //Employee ID Property
        public int EmpId
        {
            get
            {
                return empId;
            }

            set
            {
                empId = value;
            }
        }
        //Salary Property
        public double Salary
        {
            get
            {
                return salary;
            }

            set
            {
                salary = value;
            }
        }
        //Department Property
        public string Department
        {
            get
            {
                return department;
            }

            set
            {
                department = value;
            }
        }
        //Method Overloading
        public void UpdateSalary(double amount)
        {
            salary = amount;
        }
        public void UpdateSalary(double amount, double bonus)
        {
            salary = amount + bonus;
        }
        //Virtual method for overriding
        public virtual double CalculateSalary()
        {
            return Salary;
        }
        //Method for Method Hiding
        public void ShowRole()
        {
            Console.WriteLine("I am an Employee");
        }
        //Method Overriding
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine("Employee ID: " + EmpId);
            Console.WriteLine("Department: " + Department);
            Console.WriteLine("Salary: " + CalculateSalary());
        }
        //Method for comparing salary
        public static bool operator >(Employee e1, Employee e2)
        {
            return e1.Salary > e2.Salary;
        }
        public static bool operator <(Employee e1, Employee e2)
        {
            return e1.Salary < e2.Salary;
        }
    }
}
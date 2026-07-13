using System;
namespace EMS
{
    class Manager : Employee
    {
        //Private variable
        private double bonus;
        //Default Constructor
        public Manager() : base()
        {
            bonus = 0;
            Department = "Management";
        }
        //Parameterized Constructor
        public Manager(int personId, string name, int age, int empId, double salary, double bonus) : base(personId, name, age, empId, salary)
        {
            this.bonus = bonus;
            Department = "Management";
        }
        //Bonus Property (Encapsulation)
        public double Bonus
        {
            get
            {
                return bonus;
            }

            set
            {
                bonus = value;
            }
        }
        //Method Overriding
        public override double CalculateSalary()
        {
            return Salary + Bonus;
        }
        //Method Hiding using new keyword
        public new void ShowRole()
        {
            Console.WriteLine("I am a Manager");
        }
        //Method Overriding DisplayInfo
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine("Bonus: " + Bonus);
            Console.WriteLine("Final Salary: " + CalculateSalary());
        }
    }
}
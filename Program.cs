using System;
namespace EMS
{
    class Program
    {
        static Employee[] employees = new Employee[100];
        static int employeeCount = 0;
        static void Main(string[] args)
        {
            int choice;
            do
            {
                Console.WriteLine("\n---- Employee Management System ----");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. View Employees");
                Console.WriteLine("3. Update Employee");
                Console.WriteLine("4. Delete Employee");
                Console.WriteLine("5. Calculate Salary");
                Console.WriteLine("6. Compare Salaries");
                Console.WriteLine("7. Display Total Employees");
                Console.WriteLine("8. Exit");
                Console.Write("Enter choice: ");
                choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        AddEmployee();
                        break;
                    case 2:
                        ViewEmployees();
                        break;
                    case 3:
                        UpdateEmployee();
                        break;
                    case 4:
                        DeleteEmployee();
                        break;
                    case 5:
                        CalculateSalary();
                        break;
                    case 6:
                        CompareSalary();
                        break;
                    case 7:
                        Console.WriteLine("Total Employees: " + Employee.totalEmp);
                        break;
                    case 8:
                        Console.WriteLine("Exit...");
                        break;
                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                }

            } while (choice != 8);
        }
        static void AddEmployee()
        {
            Console.WriteLine("\n1. Manager");
            Console.WriteLine("2. Developer");
            Console.WriteLine("3. HR");
            Console.Write("Select Type: ");
            int type = int.Parse(Console.ReadLine());
            Console.Write("Person ID: ");
            int personId = int.Parse(Console.ReadLine());
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Age: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("Employee ID: ");
            int employeeId = int.Parse(Console.ReadLine());
            Console.Write("Salary: ");
            double salary = double.Parse(Console.ReadLine());
            Employee emp = null;
            if (type == 1)
            {
                Console.Write("Bonus: ");
                double bonus = double.Parse(Console.ReadLine());

                emp = new Manager(personId, name, age, employeeId, salary, bonus);
            }
            else if (type == 2)
            {
                Console.Write("Programming Language: ");
                string language = Console.ReadLine();
                emp = new Developer(personId, name, age, employeeId, salary, language);
            }
            else if (type == 3)
            {
                Console.Write("Employees Hired: ");
                int hired = int.Parse(Console.ReadLine());
                emp = new HR(personId, name, age, employeeId, salary, hired);
            }
            if (emp != null)
            {
                employees[employeeCount] = emp;
                employeeCount++;
                Console.WriteLine("Employee Added Successfully");
            }
        }
        static void ViewEmployees()
        {
            Console.WriteLine("\n===== Employee Details =====");
            for (int i = 0; i < employeeCount; i++)
            {
                employees[i].DisplayInfo();
                Console.WriteLine("---------------------");
            }
        }
        static void UpdateEmployee()
        {
            Console.Write("Enter Employee ID: ");
            int id = int.Parse(Console.ReadLine());
            for (int i = 0; i < employeeCount; i++)
            {
                if (employees[i].EmpId == id)
                {
                    Console.Write("Enter New Salary: ");
                    double salary = double.Parse(Console.ReadLine());
                    employees[i].Salary = salary;
                    Console.WriteLine("Salary Updated Successfully");
                    Console.WriteLine("Updated Salary: " + employees[i].Salary);
                    return;
                }
            }
            Console.WriteLine("Employee Not Found");
        }
        static void DeleteEmployee()
        {
            Console.Write("Enter Employee ID: ");
            int id = int.Parse(Console.ReadLine());
            for (int i = 0; i < employeeCount; i++)
            {
                if (employees[i].EmpId == id)
                {
                    for (int j = i; j < employeeCount - 1; j++)
                    {
                        employees[j] = employees[j + 1];
                    }
                    employees[employeeCount - 1] = null;
                    employeeCount--;
                    // decrease total employee count
                    Employee.totalEmp--;
                    Console.WriteLine("Employee Deleted Successfully");
                    return;
                }
            }

            Console.WriteLine("Employee Not Found");
        }
        static void CalculateSalary()
        {
            Console.Write("Enter Employee ID: ");
            int id = int.Parse(Console.ReadLine());
            for (int i = 0; i < employeeCount; i++)
            {
                if (employees[i].EmpId == id)
                {
                    Console.WriteLine("Salary: " + employees[i].CalculateSalary());
                    return;
                }
            }
            Console.WriteLine("Employee Not Found");
        }
        static void CompareSalary()
        {
            if (employeeCount < 2)
            {
                Console.WriteLine("Need two employees");
                return;
            }
            Employee emp1 = employees[0];
            Employee emp2 = employees[1];
            if (emp1 > emp2)
            {
                Console.WriteLine(emp1.Name + " has higher salary");
            }
            else if (emp1 < emp2)
            {
                Console.WriteLine(emp2.Name + " has higher salary");
            }
            else
            {
                Console.WriteLine("Both salaries are equal");
            }
        }
    }
}
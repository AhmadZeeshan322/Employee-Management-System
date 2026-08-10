namespace EMS
{
    class Program
    {
        static Employee[] employees = new Employee[100];
        static int employeeCount = 0;
        static void Main(string[] args)
        {
            TestDatabaseConnection();
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
        static void TestDatabaseConnection()
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                Console.WriteLine("Database connection successful!");
            }
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
                string empType = type == 1 ? "Manager" : type == 2 ? "Developer" : "HR";
                SaveEmployeeToDatabase(emp, empType);
                Console.WriteLine("Employee Saved to Database");
            }
        }
        static void SaveEmployeeToDatabase(Employee emp, string type)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO Employees 
            (EmpId, PersonId, Name, Age, Salary, Department, EmployeeType, Bonus, ProgLang, EmpHired)
            VALUES 
            (@EmpId, @PersonId, @Name, @Age, @Salary, @Department, @EmployeeType, @Bonus, @ProgLang, @EmpHired)";

                using (var cmd = new Microsoft.Data.SqlClient.SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmpId", emp.EmpId);
                    cmd.Parameters.AddWithValue("@PersonId", emp.personId);
                    cmd.Parameters.AddWithValue("@Name", emp.Name);
                    cmd.Parameters.AddWithValue("@Age", emp.Age);
                    cmd.Parameters.AddWithValue("@Salary", emp.Salary);
                    cmd.Parameters.AddWithValue("@Department", emp.Department);
                    cmd.Parameters.AddWithValue("@EmployeeType", type);
                    // Type-specific fields — sirf ek non-null hoga, subclass ke hisaab se
                    cmd.Parameters.AddWithValue("@Bonus", emp is Manager mgr ? mgr.Bonus : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ProgLang", emp is Developer dev ? dev.ProgLang : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EmpHired", emp is HR hr ? hr.EmpHired : (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        static void LoadEmployeesFromDatabase()
        {
            employeeCount = 0; // reset in-memory array before reloading

            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Employees";

                using (var cmd = new Microsoft.Data.SqlClient.SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int empId = (int)reader["EmpId"];
                        int personId = (int)reader["PersonId"];
                        string name = (string)reader["Name"];
                        int age = (int)reader["Age"];
                        double salary = (double)reader["Salary"];
                        string department = (string)reader["Department"];
                        string empType = (string)reader["EmployeeType"];

                        Employee emp = null;

                        if (empType == "Manager")
                        {
                            double bonus = reader["Bonus"] != DBNull.Value ? (double)reader["Bonus"] : 0;
                            emp = new Manager(personId, name, age, empId, salary, bonus);
                        }
                        else if (empType == "Developer")
                        {
                            string progLang = reader["ProgLang"] != DBNull.Value ? (string)reader["ProgLang"] : "";
                            emp = new Developer(personId, name, age, empId, salary, progLang);
                        }
                        else if (empType == "HR")
                        {
                            int empHired = reader["EmpHired"] != DBNull.Value ? (int)reader["EmpHired"] : 0;
                            emp = new HR(personId, name, age, empId, salary, empHired);
                        }

                        if (emp != null)
                        {
                            emp.Department = department; // in case DB value differs from constructor default
                            employees[employeeCount] = emp;
                            employeeCount++;
                        }
                    }
                }
            }
        }
        static void ViewEmployees()
        {
            LoadEmployeesFromDatabase();
            Console.WriteLine("\n===== Employee Details =====");
            for (int i = 0; i < employeeCount; i++)
            {
                employees[i].DisplayInfo();
                Console.WriteLine("---------------------");
            }
        }
        static void UpdateEmployee()
        {
            LoadEmployeesFromDatabase();
            Console.Write("Enter Employee ID: ");
            int id = int.Parse(Console.ReadLine());
            for (int i = 0; i < employeeCount; i++)
            {
                if (employees[i].EmpId == id)
                {
                    Console.Write("Enter New Salary: ");
                    double salary = double.Parse(Console.ReadLine());
                    employees[i].Salary = salary;
                    UpdateSalaryInDatabase(id, salary);
                    Console.WriteLine("Salary Updated Successfully");
                    Console.WriteLine("Updated Salary: " + employees[i].Salary);
                    return;
                }
            }
            Console.WriteLine("Employee Not Found");
        }
        static void UpdateSalaryInDatabase(int empId, double newSalary)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE Employees SET Salary = @Salary WHERE EmpId = @EmpId";
                using (var cmd = new Microsoft.Data.SqlClient.SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Salary", newSalary);
                    cmd.Parameters.AddWithValue("@EmpId", empId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        static void DeleteEmployee()
        {
            LoadEmployeesFromDatabase();
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
                    DeleteEmployeeFromDatabase(id);

                    Console.WriteLine("Employee Deleted Successfully");
                    return;
                }
            }
            Console.WriteLine("Employee Not Found");
        }
        static void DeleteEmployeeFromDatabase(int empId)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Employees WHERE EmpId = @EmpId";
                using (var cmd = new Microsoft.Data.SqlClient.SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmpId", empId);
                    cmd.ExecuteNonQuery();
                }
            }
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
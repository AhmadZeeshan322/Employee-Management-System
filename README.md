# Employee-Management-System
A C# console-based Employee Management System demonstrating core OOP concepts (inheritance, polymorphism, encapsulation, operator overloading) with Manager, Developer, and HR employee types

This document explains **where** and **how** each Object-Oriented Programming (OOP) concept is implemented across the EMS codebase (`Person.cs`, `Employee.cs`, `Manager.cs`, `Developer.cs`, `HR.cs`, `Program.cs`).

---

## 1. Encapsulation

Encapsulation is achieved by keeping fields `private`/`protected` and exposing controlled access through properties.

- **`Person.cs`**
  - `private string name;` and `private int age;` are hidden from outside classes.
  - Access is only possible through the `Name` and `Age` properties, which define `get`/`set` accessors.
- **`Employee.cs`**
  - `private int empId;` and `private double salary;` are encapsulated, exposed via `EmpId` and `Salary` properties.
  - `protected string department;` is hidden from outside code but still accessible to derived classes (see Inheritance).
- **`HR.cs`**
  - `private int empHired;` is encapsulated behind the `EmpHired` property.

**Why it matters:** External code (e.g., `Program.cs`) never touches `name`, `salary`, or `empHired` directly — it always goes through properties, so validation/logic could be added later without breaking callers.

---

## 2. Abstraction

Abstraction is shown through exposing only relevant behavior while hiding internal details.

- **`Person.cs`** defines a general `DisplayInfo()` method that abstracts "showing information about a person" without the caller needing to know how each subclass displays its data.
- **`Employee.cs`** defines an abstract-like contract via the `virtual double CalculateSalary()` method — callers just call `CalculateSalary()` without knowing the salary formula used by each role (Manager, Developer, HR).
- **`Program.cs`** interacts with employees purely through the `Employee` base type (`Employee[] employees`) and calls high-level operations (`AddEmployee`, `CalculateSalary`, `DisplayInfo`) without needing to know the concrete subclass — this is abstraction at the design level.

---

## 3. Inheritance

Inheritance models an **is-a** relationship between classes.

- **`Person` → `Employee`**
  - `class Employee : Person` in `Employee.cs`. The `Employee` class inherits `personId`, `Name`, `Age`, `address`, and `DisplayInfo()` from `Person`.
  - The constructor `public Employee(...) : base(personId, name, age)` calls the `Person` constructor to initialize inherited fields.
- **`Employee` → `Manager` / `Developer` / `HR`**
  - `class HR : Employee` in `HR.cs` inherits `EmpId`, `Salary`, `Department`, and behavior from `Employee`.
  - `public HR(...) : base(personId, name, age, empId, salary)` calls the `Employee` constructor.
  - (Based on `Program.cs` usage, `Manager` and `Developer` follow the same pattern, inheriting from `Employee` and adding their own fields like `bonus` and `language`.)

This creates a 3-level inheritance chain: **`Person` → `Employee` → `HR` / `Manager` / `Developer`**.

---

## 4. Polymorphism

Two forms of polymorphism appear in this project:

### a) Runtime (Method Overriding) Polymorphism
Achieved using `virtual` and `override` keywords.

- **`Person.DisplayInfo()`** is declared `virtual`.
- **`Employee.DisplayInfo()`** uses `override` and calls `base.DisplayInfo()` to extend the parent's behavior, then adds Employee-specific fields.
- **`HR.DisplayInfo()`** again `override`s and calls `base.DisplayInfo()`, extending it further with `EmpHired` and `Final Salary`.
- Similarly, **`CalculateSalary()`** is `virtual` in `Employee` and `override`n in `HR`:
  ```csharp
  public override double CalculateSalary()
  {
      return Salary + 3000;
  }
  ```
  When `Program.cs` calls `employees[i].CalculateSalary()`, the correct version (Manager's, Developer's, or HR's) runs automatically based on the actual object type — this is classic runtime polymorphism.

### b) Compile-time (Method Overloading) Polymorphism
Achieved by defining multiple methods with the same name but different parameters.

- **`Employee.cs`**:
  ```csharp
  public void UpdateSalary(double amount) { ... }
  public void UpdateSalary(double amount, double bonus) { ... }
  ```
  The compiler picks the correct version based on the number of arguments passed.

### c) Operator Overloading
- **`Employee.cs`** overloads the `>` and `<` operators:
  ```csharp
  public static bool operator >(Employee e1, Employee e2)
  public static bool operator <(Employee e1, Employee e2)
  ```
  This lets `Program.cs` (`CompareSalary()`) compare two `Employee` objects directly using `emp1 > emp2`, which internally compares their `Salary` values.

---

## 5. Method Hiding

Method hiding (as opposed to overriding) is demonstrated by:

- **`Employee.ShowRole()`**:
  ```csharp
  public void ShowRole()
  {
      Console.WriteLine("I am an Employee");
  }
  ```
  This method is **not** `virtual`, so if a derived class (e.g., `HR`) redefines `ShowRole()` using the `new` keyword, it would *hide* the base version rather than override it — meaning the method called depends on the **reference type**, not the actual object type (unlike overriding, which depends on the actual object type).

---

## 6. Constructors (Default & Parameterized)

Every class demonstrates constructor overloading:

- **`Person.cs`**: `Person()` (default) and `Person(int id, string name, int age)` (parameterized).
- **`Employee.cs`**: `Employee()` and `Employee(int personId, string name, int age, int empId, double salary)`, both chaining to the appropriate `Person` constructor via `: base(...)`.
- **`HR.cs`**: `HR()` and `HR(int personId, string name, int age, int empId, double salary, int empHired)`, chaining to `Employee`'s constructors.

This demonstrates **constructor chaining** across the inheritance hierarchy (`HR → Employee → Person`).

---

## 7. Static Members

Static members belong to the class itself, not to individual objects.

- **`Person.totalPersons`** — incremented in every `Person` constructor; tracks how many `Person` objects (including all employees, since they inherit from `Person`) have been created.
- **`Employee.totalEmp`** — incremented in every `Employee` constructor; tracks total employees. Also decremented in `Program.DeleteEmployee()` when an employee is removed.
- **`Program.employees`** and **`Program.employeeCount`** are also `static`, since `Main` and all helper methods are static and share this single array/counter across the whole program run.

---

## 8. Constants and Read-only Fields

- **`Person.companyName`** is declared `public const string companyName = "Solution Park";` — a compile-time constant shared by all instances, can never change.
- **`Person.personId`** is declared `public readonly int personId;` — can only be assigned once, during construction (in either constructor), and never modified afterward. This protects an employee's identity from being changed after creation.

---

## 9. Access Modifiers Summary

| Modifier | Used For | Example |
|---|---|---|
| `private` | Fields only accessible within their own class | `name`, `age`, `empId`, `salary`, `empHired` |
| `protected` | Accessible within the class and its subclasses | `address` (in `Person`), `department` (in `Employee`) |
| `public` | Accessible from anywhere | Properties, constructors, `DisplayInfo()`, static counters |
| `readonly` | Assignable only in constructor | `personId` |
| `const` | Fixed at compile time | `companyName` |

---

## 10. Composition / Aggregation (Program level)

- **`Program.cs`** holds an array `static Employee[] employees = new Employee[100];`. This is a **has-a** relationship (aggregation) — the `Program` class manages a collection of `Employee` objects without being one itself. All CRUD operations (`AddEmployee`, `ViewEmployees`, `UpdateEmployee`, `DeleteEmployee`) operate on this collection.

---

## Class Hierarchy Diagram

```
Person
 ├── totalPersons (static)
 ├── companyName (const)
 ├── personId (readonly)
 ├── name, age (private, via properties)
 ├── address (protected)
 └── DisplayInfo() [virtual]
        │
        ▼
     Employee : Person
      ├── totalEmp (static)
      ├── empId, salary (private, via properties)
      ├── department (protected)
      ├── UpdateSalary() [overloaded]
      ├── CalculateSalary() [virtual]
      ├── ShowRole() [hidden, not virtual]
      ├── DisplayInfo() [override]
      └── operator >, operator < [overloaded]
             │
     ┌───────┼────────┐
     ▼       ▼        ▼
  Manager  Developer   HR : Employee
                        ├── empHired (private, via property)
                        ├── CalculateSalary() [override → Salary + 3000]
                        └── DisplayInfo() [override]
```

---

## Quick Reference Table

| OOP Concept | Location | Key Element |
|---|---|---|
| Encapsulation | `Person.cs`, `Employee.cs`, `HR.cs` | private fields + public properties |
| Abstraction | `Person.cs`, `Employee.cs`, `Program.cs` | `DisplayInfo()`, `CalculateSalary()`, base-type usage |
| Inheritance | `Employee.cs`, `HR.cs` | `: Person`, `: Employee`, `base(...)` |
| Polymorphism (override) | `Employee.cs`, `HR.cs` | `virtual`/`override` on `DisplayInfo()`, `CalculateSalary()` |
| Polymorphism (overload) | `Employee.cs` | `UpdateSalary(double)`, `UpdateSalary(double, double)` |
| Operator Overloading | `Employee.cs` | `operator >`, `operator <` |
| Method Hiding | `Employee.cs` | `ShowRole()` (non-virtual) |
| Constructors | All classes | Default + parameterized, chained via `base(...)` |
| Static Members | `Person.cs`, `Employee.cs`, `Program.cs` | `totalPersons`, `totalEmp`, `employees[]` |
| Const / Readonly | `Person.cs` | `companyName` (const), `personId` (readonly) |
| Aggregation | `Program.cs` | `Employee[] employees` array |

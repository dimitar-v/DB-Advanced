using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                var result = RemoveTown(context);
                Console.WriteLine(result);
            }
        }

        //P03.Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.EmployeeId)
                .ToList()
                .ForEach(e => sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}"));

            return sb.ToString().Trim();
        }

        //P04.Employees with Salary Over 50 000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToList()
                .ForEach(e => sb.AppendLine($"{e.FirstName} - {e.Salary:F2}"));

            return sb.ToString().Trim();
        }

        //P05.Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList()
                .ForEach(e => sb.AppendLine($"{e.FirstName} {e.LastName} from Research and Development - ${e.Salary:F2}"));

            return sb.ToString().Trim();
        }

        //P06.	Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            employee.Address = address;

            context.SaveChanges();


            StringBuilder sb = new StringBuilder();

            context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => e.Address.AddressText)
                .Take(10)
                .ToList()
                .ForEach(a => sb.AppendLine(a));

            return sb.ToString().Trim();
        }

        //P07.	Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var emplpyees = context.Employees
                .Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001
                    && ep.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    EmployeeName = $"{e.FirstName} {e.LastName}",
                    ManagerName = $"{e.Manager.FirstName} {e.Manager.LastName}",
                    Projects = e.EmployeesProjects
                        .Select(ep => new
                        {
                            ep.Project.Name,
                            ep.Project.StartDate,
                            ep.Project.EndDate
                        })
                        .ToList()
                })
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in emplpyees)
            {
                sb.AppendLine($"{emp.EmployeeName} - Manager: {emp.ManagerName}");

                foreach (var p in emp.Projects)
                {
                    var startDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    var endDate = p.EndDate.HasValue
                        ? p.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                        : "not finished";

                    sb.AppendLine($"--{p.Name} - {startDate} - {endDate}");
                }
            }

            return sb.ToString().Trim();
        }

        //P08.	Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Addresses
                .Select(a => new
                {
                    a.AddressText,
                    Town = a.Town.Name,
                    EmployeesCount = a.Employees.Count
                })
                .OrderByDescending(a => a.EmployeesCount)
                .ThenBy(a => a.Town)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToList()
                .ForEach(a => sb.AppendLine($"{a.AddressText}, {a.Town} - {a.EmployeesCount} employees"));

            return sb.ToString().Trim();
        }

        //P09.	Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var emp147 = context.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                        .Select(ep => ep.Project.Name)
                        .OrderBy(p => p)
                        .ToList()
                })
                .SingleOrDefault(e => e.EmployeeId == 147);

            sb.AppendLine($"{emp147.FirstName} {emp147.LastName} - {emp147.JobTitle}");

            foreach (var p in emp147.Projects)
            {
                sb.AppendLine(p);
            }

            return sb.ToString().Trim();
        }

        //P10.	Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    Manager = d.Manager.FirstName + " " + d.Manager.LastName,
                    Employees = d.Employees
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .Select(e => $"{e.FirstName} {e.LastName} - {e.JobTitle}")
                        .ToList()
                })
                .ToList()
                .ForEach(d => sb.AppendLine($"{d.Name} - {d.Manager}{Environment.NewLine}{string.Join(Environment.NewLine, d.Employees)}"));

            return sb.ToString().Trim();
        }

        //11.	Find Latest 10 Projects
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Projects
                .OrderByDescending(p => p.StartDate)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate

                })
                .Take(10)
                .OrderBy(p => p.Name)
                .ToList()
                .ForEach(p =>
                {
                    sb.AppendLine(p.Name);
                    sb.AppendLine(p.Description);
                    sb.AppendLine(p.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
                });

            return sb.ToString().Trim();
        }

        //12.	Increase Salaries
        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design"
                    || e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                e.Salary *= 1.12m;
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:F2})");
            }

            context.SaveChanges();

            return sb.ToString().Trim();
        }

        //13.	Find Employees by First Name Starting With "Sa"
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Employees
                .Where(e => EF.Functions.Like(e.FirstName, "sa%")) //e.FirstName.StartsWith("Sa")) 
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList()
                .ForEach(e => sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})"));

            return sb.ToString().Trim();
        }

        //14.	Delete Project by Id
        public static string DeleteProjectById(SoftUniContext context)
        {
            var project = context.Projects
                .SingleOrDefault(p => p.ProjectId == 2);

            var empProject = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2)
                .ToList();
            

            context.EmployeesProjects.RemoveRange(empProject);
            context.Projects.Remove(project);

            context.SaveChanges();

            var sb = new StringBuilder();
            context.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToList()
                .ForEach(p => sb.AppendLine(p));

            return sb.ToString().Trim();
        }

        //15.	Remove Town
        public static string RemoveTown(SoftUniContext context)
        {
            var town = context.Towns.SingleOrDefault(t => t.Name == "Seattle");
            var addresses = context.Addresses
                .Where(a => a.Town.Name == town.Name)
                .ToList();

            var addrCount = addresses.Count;

            context.Employees
                .Where(e => addresses.Contains(e.Address))
                .ToList()
                .ForEach(e => e.Address = null);

            context.Addresses.RemoveRange(addresses);
            context.Towns.Remove(town);

            context.SaveChanges();

            return $"{addrCount} addresses in {town.Name} were deleted";
        }
    }
}

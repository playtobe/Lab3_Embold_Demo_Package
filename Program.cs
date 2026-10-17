using Project3.Controllers;
using Project3.Library;
using Project3.Models;
using Project3.Views;

namespace Project3;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Initializing data...");
        DeveloperList developerList = new DeveloperList(new List<Developer>());
        developerList.LoadFile();
        ProjectList projectList = new ProjectList(developerList);
        projectList.LoadFile();

        string[] options = new string[]
        {
            "List all Developers",
            "Add a new Developer",
            "Search for a Developer by ID",
            "Update a Developer's salary by ID",
            "List all Developers by Language",
            "Add a new Project",
            "List all Projects by Developer (Grouped)",
            "Calculate Total Experience by Dev ID",
            "Remove a Developer by ID",
            "Sort Developers by Salary",
            "Save data to files",
            "Quit program"
        };

        int choice = 0;

        do
        {
            Console.WriteLine("\n--- Software Developer Management ---");
            choice = Menu.GetChoice(options);

            switch (choice)
            {
                case 1:
                    Console.WriteLine("1. List all Developers (Coming soon)");
                    developerList.DisplayAll();
                    break;
                case 2:
                    developerList.AddDeveloper();
                    Console.WriteLine("2. Add a new Developer (Coming soon)");
                    break;
                case 3:
                    developerList.SearchDeveloper();
                    break;
                case 4:
                    developerList.UpdateSalaryByID();
                    break;
                case 5:
                    developerList.DisplayDeveloperByLanguage();
                    break;
                case 6:
                    projectList.AddProject();
                    projectList.DisplayAll();
                    break;
                case 7:
                    projectList.DisplayProjectsByDeveloper();
                    break;
                case 8:
                    projectList.CalculateTotalDuration();
                    break;
                case 9:
                    projectList.RemovedDeveloperByID();
                    break;
                case 10:
                    developerList.SortDeveloperBySalary();
                    break;
                case 11:
                    Console.WriteLine("Saving data to files...");
                    developerList.SaveToFile();
                    projectList.SaveToFile();
                    break;
                case 12:
                    string confirm = Inputter.GetString("Do you want to save the changes before exiting? (Y/N): ");
                    if (confirm.Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        developerList.SaveToFile();
                        projectList.SaveToFile();
                    }
                    Console.WriteLine("Good bye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again!");
                    break;
            }
        } while (choice != 12);
    }
}

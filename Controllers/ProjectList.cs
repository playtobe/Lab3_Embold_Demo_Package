using Project3.Library;
using Project3.Models;

namespace Project3.Controllers;

public class ProjectList
{
    private readonly string filename = "projects.txt";
    private List<Project> projectList = new();
    private DeveloperList developerList;

    public ProjectList(DeveloperList developerList)
    {
        this.developerList = developerList;
    }

    private string ResolveFilePath()
    {
        if (File.Exists(filename))
        {
            return filename;
        }

        string baseDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        if (File.Exists(baseDirPath))
        {
            return baseDirPath;
        }

        return filename;
    }

    public List<Project> GetProjectList() => projectList;

    public void LoadFile()
    {
        string filePath = ResolveFilePath();
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File {filename} not found!");
            return;
        }

        if (projectList == null)
        {
            projectList = new List<Project>();
        }
        else
        {
            projectList.Clear();
        }

        try
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var rawLine in lines)
            {
                string line = rawLine.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                string[] parts = line.Split(',');
                if (parts.Length >= 5)
                {
                    try
                    {
                        string projectID = parts[0].Trim();
                        string devID = parts[1].Trim();
                        string projectName = parts[2].Trim();

                        if (!int.TryParse(parts[3].Trim(), out int duration) || duration <= 0)
                        {
                            continue;
                        }

                        DateOnly? startDate = Inputter.ToDateOnly(parts[4].Trim());
                        if (startDate == null)
                        {
                            Console.WriteLine($"Lỗi định dạng ngày tháng dòng: [{line}]");
                            continue;
                        }

                        projectList.Add(new Project(projectID, devID, projectName, duration, startDate.Value));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Lỗi đọc dòng: [{line}] -> {e.Message}");
                    }
                }
            }

            Console.WriteLine($"Successfully Loaded file {filename} ({projectList.Count} projects)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"File {filename} error: {ex.Message}");
        }
    }

    public void SaveToFile()
    {
        try
        {
            using var bw = new StreamWriter(filename);
            foreach (var p in projectList)
            {
                string formattedDate = Inputter.DateToString(p.StartDate);
                string line = $"{p.ProjectID}, {p.DevID}, {p.ProjectName}, {p.Duration}, {formattedDate}";
                bw.WriteLine(line);
            }
            Console.WriteLine($"Saved {filename} successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving file: {e.Message}");
        }
    }

    public Project? GetbyID(string id)
    {
        return projectList.FirstOrDefault(p => p.ProjectID.Equals(id, StringComparison.OrdinalIgnoreCase));
    }

    public void DisplayAll()
    {
        if (projectList == null || projectList.Count == 0)
        {
            Console.WriteLine("The project list is empty.");
            return;
        }

        Console.WriteLine("---------------------------------------------------------------------------------------------");
        Console.WriteLine(string.Format("| {0,-12} | {1,-10} | {2,-25} | {3,-15} | {4,-12} |",
            "Project ID", "Dev ID", "Project Name", "Duration(mos)", "Start Date"));
        Console.WriteLine("---------------------------------------------------------------------------------------------");

        foreach (var p in projectList)
        {
            string formattedDate = Inputter.DateToString(p.StartDate);
            Console.WriteLine(string.Format("| {0,-12} | {1,-10} | {2,-25} | {3,-15} | {4,-12} |",
                p.ProjectID,
                p.DevID,
                p.ProjectName,
                p.Duration,
                formattedDate));
        }

        Console.WriteLine("---------------------------------------------------------------------------------------------");
    }

    public void AddProject()
    {
        string projectID;
        while (true)
        {
            projectID = Inputter.InputAndLoop("Enter ProjectID: ", Acceptable.PROJECT_ID_VALID, false);
            if (GetbyID(projectID) == null)
            {
                break;
            }
            Console.WriteLine("ID already exist!Please Try Again!!");
        }

        developerList.DisplayAll();
        string devID = Inputter.InputAndLoop("Enter DevID: ", Acceptable.DEVELOPER_ID_VALID, false);
        if (developerList.GetbyID(devID) == null)
        {
            Console.WriteLine("DevID doesnt Exist in developerList!!");
            return;
        }

        string projectName = Inputter.InputAndLoop("Enter ProjectName: ", Acceptable.PROJECT_NAME_VALID, false);
        string durationMonth = Inputter.InputAndLoop("Enter DurationMonth: ", Acceptable.POSITIVE_INT_VALID, false);
        DateOnly? startDate = Inputter.InputDateOnly("Enter Start_Date(Must be in future): ", true);

        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        if (startDate == null || startDate.Value <= today)
        {
            Console.WriteLine("Start_Date must be in future!!!");
            return;
        }

        projectList.Add(new Project(projectID, devID, projectName, int.Parse(durationMonth), startDate.Value));
        Console.WriteLine("Add Project Successfully");
    }

    public void DisplayProjectsByDeveloper()
    {
        if (developerList == null || developerList.GetDeveloperList().Count == 0)
        {
            Console.WriteLine("No developers found to display.");
            return;
        }

        var devs = developerList.GetDeveloperList();
        foreach (var dev in devs)
        {
            Console.WriteLine("\n=============================================================");
            Console.WriteLine($"Developer: {dev.DeveloperID} - {dev.Fullname}");
            Console.WriteLine("-------------------------------------------------------------");

            bool hasProject = false;
            foreach (var p in projectList)
            {
                if (p.DevID.Equals(dev.DeveloperID, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(string.Format("  | {0,-12} | {1,-15} | {2,-15} |", "Project ID", "Duration(mos)", "Start Date"));
                    Console.WriteLine("  |--------------|-----------------|-----------------|");
                    string formattedDate = Inputter.DateToString(p.StartDate);
                    Console.WriteLine(string.Format("  | {0,-12} | {1,-15} | {2,-15} |",
                        p.ProjectID, p.Duration, formattedDate));
                    hasProject = true;
                }
            }

            if (!hasProject)
            {
                Console.WriteLine("  (No projects assigned to this developer)");
            }
        }

        Console.WriteLine("=============================================================\n");
    }

    public void CalculateTotalDuration()
    {
        int count = 0;
        string developerID = Inputter.InputAndLoop("Enter ID to calculate: ", Acceptable.DEVELOPER_ID_VALID, false);
        if (developerList.GetbyID(developerID) == null)
        {
            Console.WriteLine("ID Doesnt Exists");
            return;
        }

        foreach (var p in projectList)
        {
            if (p.DevID.Equals(developerID, StringComparison.OrdinalIgnoreCase))
            {
                count += p.Duration;
            }
        }

        Console.WriteLine($"Developer has spent on all registered projects: {count}");
    }

    public bool IsDevAssignedToProject(string devID)
    {
        if (projectList == null)
        {
            return false;
        }

        return projectList.Any(p => p.DevID.Equals(devID, StringComparison.OrdinalIgnoreCase));
    }

    public void RemovedDeveloperByID()
    {
        string devID = Inputter.InputAndLoop("Enter Developer ID to remove: ", Acceptable.DEVELOPER_ID_VALID, false);
        var dev = developerList.GetbyID(devID);
        if (dev == null)
        {
            Console.WriteLine("Developer ID does not exist!");
            return;
        }

        if (IsDevAssignedToProject(devID))
        {
            Console.WriteLine("Cant Delete! Developer is assigned to projects.");
            return;
        }

        developerList.GetDeveloperList().Remove(dev);
        Console.WriteLine("Removed Successfully!!");
    }

    // --- Các hàm thuần logic phục vụ Unit Test (Không dùng Console) ---
    public int GetTotalDurationByDevID(string devID)
    {
        if (developerList.GetbyID(devID) == null)
        {
            throw new ArgumentException("Developer ID does not exist");
        }

        return projectList.Where(p => p.DevID.Equals(devID, StringComparison.OrdinalIgnoreCase)).Sum(p => p.Duration);
    }

    public void RemoveDeveloper(string devID)
    {
        var dev = developerList.GetbyID(devID);
        if (dev == null)
        {
            throw new ArgumentException("Developer ID does not exist");
        }

        if (IsDevAssignedToProject(devID))
        {
            throw new InvalidOperationException("Cannot delete! Developer is assigned to projects.");
        }

        developerList.GetDeveloperList().Remove(dev);
    }

    // Các alias tương thích Java
    public void saveToFile() => SaveToFile();
    public void displayAll() => DisplayAll();
    public void displayProjectsByDeveloper() => DisplayProjectsByDeveloper();
    public bool isDevAssignedToProject(string devID) => IsDevAssignedToProject(devID);
}

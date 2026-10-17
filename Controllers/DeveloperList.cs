using Project3.Library;
using Project3.Models;
using System.Text.RegularExpressions;

namespace Project3.Controllers;

public class DeveloperList
{
    private readonly string filename = "developers.txt";
    private List<Developer> developerList;

    public DeveloperList(List<Developer>? developerList = null)
    {
        this.developerList = developerList ?? new List<Developer>();
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

    public void LoadFile()
    {
        string filePath = ResolveFilePath();
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File {filename} not found!");
            return;
        }

        if (developerList == null)
        {
            developerList = new List<Developer>();
        }
        else
        {
            developerList.Clear();
        }

        try
        {
            var regex = new Regex(Acceptable.FILE);
            var lines = File.ReadAllLines(filePath);

            foreach (var rawLine in lines)
            {
                string line = rawLine.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var match = regex.Match(line);
                if (match.Success)
                {
                    string id = match.Groups[1].Value.Trim();
                    string name = match.Groups[2].Value.Trim();
                    string skillsRaw = match.Groups[3].Value.Trim();
                    if (!int.TryParse(match.Groups[4].Value.Trim(), out int salary))
                    {
                        continue;
                    }

                    if (!Acceptable.IsValid(id, Acceptable.DEVELOPER_ID_VALID))
                    {
                        continue;
                    }

                    List<string> proLang = new();
                    if (!string.IsNullOrEmpty(skillsRaw))
                    {
                        string[] skillsArray = skillsRaw.Split(',');
                        foreach (string s in skillsArray)
                        {
                            if (!string.IsNullOrWhiteSpace(s))
                            {
                                proLang.Add(s.Trim());
                            }
                        }
                    }

                    if (proLang.Count > 0)
                    {
                        developerList.Add(new Developer(id, name, proLang, salary));
                    }
                }
            }

            Console.WriteLine($"Successfully Loaded {developerList.Count} developers.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void SaveToFile()
    {
        try
        {
            using var bw = new StreamWriter(filename);
            foreach (var p in developerList)
            {
                string proLangStr = "[" + string.Join(", ", p.ProLang) + "]";
                string line = $"{p.DeveloperID}, {p.Fullname}, {proLangStr}, {p.Salary}";
                bw.WriteLine(line);
            }
            Console.WriteLine("Saved developers.txt successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving developers: {e.Message}");
        }
    }

    public List<Developer> GetDeveloperList() => developerList;

    public void DisplayAll()
    {
        if (developerList == null || developerList.Count == 0)
        {
            Console.WriteLine("The developer list is empty.");
            return;
        }

        Console.WriteLine("-----------------------------------------------------------------------------------");
        Console.WriteLine(string.Format("| {0,-8} | {1,-20} | {2,-30} | {3,-10} |", "ID", "Name", "Language", "Salary($)"));
        Console.WriteLine("-----------------------------------------------------------------------------------");

        foreach (var dev in developerList)
        {
            string langStr = "[" + string.Join(", ", dev.ProLang) + "]";
            Console.WriteLine(string.Format("| {0,-8} | {1,-20} | {2,-30} | {3,-10} |",
                dev.DeveloperID,
                dev.Fullname,
                langStr,
                dev.Salary));
        }

        Console.WriteLine("-----------------------------------------------------------------------------------");
    }

    public Developer? GetbyID(string id)
    {
        return developerList.FirstOrDefault(d => d.DeveloperID.Equals(id, StringComparison.OrdinalIgnoreCase));
    }

    public void AddDeveloper()
    {
        string developerID;
        while (true)
        {
            developerID = Inputter.InputAndLoop("Enter DeveloperID: ", Acceptable.DEVELOPER_ID_VALID, false);
            if (GetbyID(developerID) == null)
            {
                break;
            }
            Console.WriteLine("Developer ID already Exists. Please try another one!");
        }

        string name = Inputter.InputAndLoop("Enter FullName: ", Acceptable.NAME_VALID, false);

        string lang;
        while (true)
        {
            lang = Inputter.GetString("Enter Programming Language (must comma seperated): ");
            if (!string.IsNullOrWhiteSpace(lang))
            {
                break;
            }
            Console.WriteLine("Programming Language cant be empty! Please Try Again!");
        }

        List<string> proLang = new();
        string[] langArray = lang.Split(',');
        foreach (string item in langArray)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                proLang.Add(item.Trim());
            }
        }

        int salary;
        while (true)
        {
            string salaryStr = Inputter.InputAndLoop("Enter Salary(above 1000USD): ", Acceptable.POSITIVE_INT_VALID, false);
            if (int.TryParse(salaryStr, out salary) && salary >= 1000)
            {
                break;
            }
            Console.WriteLine("Salary must over 1000 USD. Try Again!");
        }

        developerList.Add(new Developer(developerID, name, proLang, salary));
        Console.WriteLine("Add Developer Successfully!");
    }

    public void SearchDeveloper()
    {
        string id = Inputter.InputAndLoop("Enter ID to search: ", Acceptable.DEVELOPER_ID_VALID, false);
        foreach (var d in developerList)
        {
            if (d.DeveloperID.Equals(id, StringComparison.OrdinalIgnoreCase))
            {
                string langStr = "[" + string.Join(", ", d.ProLang) + "]";
                Console.WriteLine("-----------------------------------------------------------------------------------");
                Console.WriteLine(string.Format("| {0,-8} | {1,-20} | {2,-30} | {3,-10} |", "ID", "Name", "Language", "Salary($)"));
                Console.WriteLine("-----------------------------------------------------------------------------------");
                Console.WriteLine(string.Format("| {0,-8} | {1,-20} | {2,-30} | {3,-10} |",
                    d.DeveloperID,
                    d.Fullname,
                    langStr,
                    d.Salary));
                Console.WriteLine("-----------------------------------------------------------------------------------");
                return;
            }
        }
        Console.WriteLine("Developer ID does not exist!");
    }

    public void UpdateSalaryByID()
    {
        string id = Inputter.InputAndLoop("Enter ID to update Salary : ", Acceptable.DEVELOPER_ID_VALID, false);
        Developer? target = GetbyID(id);
        if (target == null)
        {
            Console.WriteLine("Developer ID doesnt exist");
            return;
        }

        string salaryStr = Inputter.GetString("Enter Salary : ");
        if (int.TryParse(salaryStr, out int salary) && salary >= 1000)
        {
            target.Salary = salary;
            Console.WriteLine("Updated Successfully");
        }
        else
        {
            Console.WriteLine("Salary must over 1000 USD.");
        }
    }

    public void DisplayDeveloperByLanguage()
    {
        if (developerList == null || developerList.Count == 0)
        {
            Console.WriteLine("Developer List is empty!!");
            return;
        }

        string searchLang = Inputter.GetString("Enter Programming Language to search: ");
        if (string.IsNullOrWhiteSpace(searchLang))
        {
            Console.WriteLine("Cant empty");
            return;
        }

        var matchingDevelopers = developerList.Where(dev =>
            dev.ProLang.Any(l => l.Equals(searchLang, StringComparison.OrdinalIgnoreCase))
        ).ToList();

        if (matchingDevelopers.Count == 0)
        {
            Console.WriteLine($"No developer found with the langugage! {searchLang}");
            return;
        }

        Console.WriteLine("-----------------------------------------------------------------------------------");
        Console.WriteLine(string.Format("| {0,-8} | {1,-20} | {2,-30} | {3,-10} |", "ID", "Name", "Language", "Salary($)"));
        Console.WriteLine("-----------------------------------------------------------------------------------");

        foreach (var dev in matchingDevelopers)
        {
            string langStr = "[" + string.Join(", ", dev.ProLang) + "]";
            Console.WriteLine(string.Format("| {0,-8} | {1,-20} | {2,-30} | {3,-10} |",
                dev.DeveloperID,
                dev.Fullname,
                langStr,
                dev.Salary));
        }

        Console.WriteLine("-----------------------------------------------------------------------------------");
    }

    public void SortDeveloperBySalary()
    {
        if (developerList == null || developerList.Count == 0)
        {
            Console.WriteLine("DeveloperList is empty!!");
            return;
        }

        developerList.Sort((d1, d2) => d1.Salary.CompareTo(d2.Salary));
        DisplayAll();
        Console.WriteLine("Developers sorted by salary successfully (Ascending)!");
    }

    // --- Các hàm thuần logic phục vụ Unit Test (Không dùng Console) ---
    public void UpdateSalary(string id, int newSalary)
    {
        Developer? target = GetbyID(id);
        if (target == null)
        {
            throw new ArgumentException("Developer ID does not exist");
        }

        if (newSalary < 1000)
        {
            throw new ArgumentException("Salary must be over 1000 USD");
        }

        target.Salary = newSalary;
    }
    public List<Developer> GetDevelopersByLanguage(string language)
    {
        if (developerList == null) return new List<Developer>();

        return developerList.Where(dev => 
            dev.ProLang.Any(l => l.Equals(language, StringComparison.OrdinalIgnoreCase))
        ).ToList();
    }

    public List<Developer> SortDevelopersBySalaryLogic()
    {
        if (developerList == null) return new List<Developer>();

        // Trả về một list mới đã được sắp xếp để không ảnh hưởng list gốc (hoặc xếp list gốc)
        var sortedList = new List<Developer>(developerList);
        sortedList.Sort((d1, d2) => d1.Salary.CompareTo(d2.Salary));
        return sortedList;
    }

    // Các alias tương thích tên gốc từ Java
    public void saveToFile() => SaveToFile();
    public void displayAll() => DisplayAll();
    public List<Developer> getDeveloperList() => GetDeveloperList();
}

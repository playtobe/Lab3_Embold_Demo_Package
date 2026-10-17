namespace Project3.Models;

public class Developer
{
    public string DeveloperID { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public List<string> ProLang { get; set; } = new();
    public int Salary { get; set; }

    public Developer()
    {
    }

    public Developer(string developerID, string fullname, List<string> proLang, int salary)
    {
        DeveloperID = developerID;
        Fullname = fullname;
        ProLang = proLang ?? new List<string>();
        Salary = salary;
    }

    // Tương thích thêm các getter/setter nếu cần
    public string GetDeveloperID() => DeveloperID;
    public void SetDeveloperID(string developerID) => DeveloperID = developerID;

    public string GetFullname() => Fullname;
    public void SetFullname(string fullname) => Fullname = fullname;

    public List<string> GetProLang() => ProLang;
    public void SetProLang(List<string> proLang) => ProLang = proLang;

    public int GetSalary() => Salary;
    public void SetSalary(int salary) => Salary = salary;
}

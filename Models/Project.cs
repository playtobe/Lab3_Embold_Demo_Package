namespace Project3.Models;

public class Project
{
    public string ProjectID { get; set; } = string.Empty;
    public string DevID { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public int Duration { get; set; }
    public DateOnly StartDate { get; set; }

    public Project()
    {
    }

    public Project(string projectID, string devID, string projectName, int duration, DateOnly startDate)
    {
        ProjectID = projectID;
        DevID = devID;
        ProjectName = projectName;
        Duration = duration;
        StartDate = startDate;
    }

    // Tương thích thêm các getter/setter nếu cần
    public string GetProjectID() => ProjectID;
    public void SetProjectID(string projectID) => ProjectID = projectID;

    public string GetDevID() => DevID;
    public void SetDevID(string devID) => DevID = devID;

    public string GetProjectName() => ProjectName;
    public void SetProjectName(string projectName) => ProjectName = projectName;

    public int GetDuration() => Duration;
    public void SetDuration(int duration) => Duration = duration;

    public DateOnly GetStartDate() => StartDate;
    public void SetStartDate(DateOnly startDate) => StartDate = startDate;
}

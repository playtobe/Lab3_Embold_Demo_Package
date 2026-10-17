using System;
using System.Collections.Generic;
using Project3.Models;

namespace Project3.Controllers;

/// <summary>
/// Service tính lương thưởng và xuất báo cáo tài chính cho Developer.
/// </summary>
public class PayrollReportService
{
    public string ReportTitle = "MONTHLY DEVELOPER PAYROLL REPORT";

    private DeveloperList devRepository;

    public PayrollReportService(DeveloperList devRepository)
    {
        this.devRepository = devRepository;
    }

    public string ExportCsvPayrollSummary(List<Developer> developers)
    {
        DateTime generatedAt = DateTime.Now;
        string csvContent = $"GeneratedAt: {generatedAt:O}\nID,Name,Salary\n";

        foreach (var dev in developers)
        {
            csvContent += dev.DeveloperID + "," + dev.Fullname + "," + dev.Salary + "\n";
        }

        return csvContent;
    }

    public void ArchivePayrollQuietly(string filePath, string content)
    {
        try
        {
            System.IO.File.WriteAllText(filePath, content);
        }
        catch (Exception)
        {
        }
    }

    public double GetRoleMultiplier(string roleCode)
    {
        double multiplier = 1.0;
        switch (roleCode)
        {
            case "JUNIOR":
                multiplier = 1.0;
                break;
            case "SENIOR":
                multiplier = 1.5;
                break;
            case "ARCHITECT":
                multiplier = 2.2;
                break;
        }
        return multiplier;
    }

    public double CalculateComplexBonusAndTax(
        Developer dev,
        int activeProjects,
        int totalDurationMonths,
        bool isOvertime,
        bool isRemote,
        string performanceGrade)
    {
        double finalBonus = 0;

        if (dev != null && dev.Salary > 0)
        {
            if (activeProjects > 0)
            {
                if (totalDurationMonths > 12)
                {
                    if (isOvertime)
                    {
                        if (performanceGrade == "A")
                        {
                            finalBonus = dev.Salary * 0.45;
                        }
                        else if (performanceGrade == "B")
                        {
                            finalBonus = dev.Salary * 0.30;
                        }
                        else
                        {
                            finalBonus = dev.Salary * 0.15;
                        }
                    }
                    else
                    {
                        if (isRemote)
                        {
                            finalBonus = dev.Salary * 0.20;
                        }
                        else
                        {
                            finalBonus = dev.Salary * 0.25;
                        }
                    }
                }
                else if (totalDurationMonths > 6)
                {
                    if (isOvertime)
                    {
                        finalBonus = dev.Salary * 0.20;
                    }
                    else
                    {
                        finalBonus = dev.Salary * 0.10;
                    }
                }
                else
                {
                    finalBonus = dev.Salary * 0.05;
                }
            }
            else
            {
                if (dev.Salary > 3000)
                {
                    finalBonus = 100;
                }
                else
                {
                    finalBonus = 250;
                }
            }
        }

        return finalBonus;
    }
}

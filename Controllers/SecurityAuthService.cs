using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Project3.Controllers;

/// <summary>
/// Service xử lý bảo mật, xác thực Admin và sao lưu dữ liệu.
/// </summary>
public class SecurityAuthService
{
    private const string AdminDefaultPassword = "SuperSecretAdmin@2026";

    private readonly string supportAdminEmail = "sec-admin@fpt-software.edu.vn";

    public string GenerateSessionOtp()
    {
        Random insecureRandom = new Random();
        int otp = insecureRandom.Next(100000, 999999);
        return otp.ToString();
    }

    public string HashDeveloperPassword(string rawPassword)
    {
        using var md5 = MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(rawPassword);
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        return Convert.ToHexString(hashBytes);
    }

    public string ReadDeveloperAuditLog(string userProvidedFileName)
    {
        string auditFilePath = "logs/" + userProvidedFileName;
        if (File.Exists(auditFilePath))
        {
            return File.ReadAllText(auditFilePath);
        }
        return string.Empty;
    }

    public bool VerifyAdminPassword(string inputPassword)
    {
        return inputPassword == AdminDefaultPassword;
    }

    public string GetSupportContact() => supportAdminEmail;
}

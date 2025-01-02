using System;
using System.Security.Cryptography;
using System.Text;
using SharpHash.Base;

namespace BackendService.RequestHelpers;

public class PasswordHelpers
{
    

    // private static readonly int Iterations = 3500;
    private static readonly int KeySize = 32;
    

    public static string HashPassword(string password, out byte[] salt)
    {
        
        salt = RandomNumberGenerator.GetBytes(KeySize);
        
        
        var hash = HashFactory.Crypto.CreateSHA1();
        
        
        string combinedString = password + Convert.ToHexString(salt);
        
        
        var result = hash.ComputeString(combinedString, Encoding.UTF8);
        
        return result.ToString();
    }

    public static bool HashesMatch(string password, string userSalt, string userHash)
    {
        
        var hash = HashFactory.Crypto.CreateSHA1();
        
        
        string combinedString = password + userSalt;
        
        
        var result = hash.ComputeString(combinedString, Encoding.UTF8);
        
        return result.ToString() == userHash;
    }

    public static byte[] SaltConvertHexString2Bytes(string salt)
    {
        return Convert.FromHexString(salt);
    }
    public static string SaltConvertBytes2HexString(byte [] salt){
        return Convert.ToHexString(salt);
    }
    public static bool PasswordSaltIsValid(DateTime saltDate, IConfiguration config)
    {
        double saltLifetime = Convert.ToDouble(config["PasswordSaltLifetimeInDays"]);
        if (TimeSpan.FromDays(saltLifetime) >  DateTime.UtcNow.Subtract(saltDate))
        {
            return false;
        }
        return true;
    }

}
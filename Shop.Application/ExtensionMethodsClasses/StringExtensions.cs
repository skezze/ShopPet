using System.Security.Cryptography;
using System.Text;

namespace Shop.Application.ExtensionMethodsClasses;

public static class StringExtensions
{
    public static string GetSha256StringFromString(this string s) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(s)));
}
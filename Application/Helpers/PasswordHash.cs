using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers;

public static class PasswordHash
{
    public static string EncodePasswordMd5(string pass)
    {
        Byte[] originalBytes;
        Byte[] encodedBytes;
        MD5 md5 = new MD5CryptoServiceProvider();
        originalBytes = ASCIIEncoding.Default.GetBytes(pass);
        encodedBytes = md5.ComputeHash(originalBytes);
        return BitConverter.ToString(encodedBytes);
    }
}
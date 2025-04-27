namespace Application.Helpers;

public static class PasswordHash
{
    [System.Obsolete("Obsolete")]
    public static string EncodePasswordMd5(string pass)
    {
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        var originalBytes = System.Text.Encoding.Default.GetBytes(pass);
        var encodedBytes = md5.ComputeHash(originalBytes);
        return System.BitConverter.ToString(encodedBytes);
    }
}
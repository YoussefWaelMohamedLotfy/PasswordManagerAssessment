using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.API.Services;

public class EncryptionService : IEncryptionService
{
    private readonly string key = "b14ca5898a4e4142aace2ea2143a2410";

    public string EncryptString(string plainText)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);

            using (StreamWriter streamWriter = new(cryptoStream))
            {
                streamWriter.Write(plainText);
            }

            array = memoryStream.ToArray();
        }

        return Convert.ToBase64String(array);
    }

    public string DecryptString(string cipherText)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);

        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = iv;
        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using MemoryStream memoryStream = new(buffer);
        using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
        using StreamReader streamReader = new(cryptoStream);

        return streamReader.ReadToEnd();
    }
}

namespace PasswordManager.API.Services;

public interface IEncryptionService
{
    string DecryptString(string cipherText);
    string EncryptString(string plainText);
}
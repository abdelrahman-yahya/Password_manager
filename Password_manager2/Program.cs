using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Password_Manager
{
    internal class Program
    {
        private static readonly Dictionary<string, string> _PasswordEntries = new();

        static void Main(string[] args)
        {
            ReadPasswords();
            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. List all passwords");
                Console.WriteLine("2. Add / Change password");
                Console.WriteLine("3. Get password");
                Console.WriteLine("4. Delete password");
                Console.WriteLine("5. Exit");

                var selectedOption = Console.ReadLine();

                switch (selectedOption)
                {
                    case "1":
                        ListAllPasswords();
                        break;
                    case "2":
                        AddOrChangePassword();
                        break;
                    case "3":
                        GetPassword();
                        break;
                    case "4":
                        DeletePassword();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                Console.WriteLine("-------------------------------------");
            }
        }

        private static void ListAllPasswords()
        {
            if (_PasswordEntries.Count == 0)
            {
                Console.WriteLine("No passwords stored.");
                return;
            }

            foreach (var entry in _PasswordEntries)
                Console.WriteLine($"{entry.Key} = {entry.Value}");
        }

        private static void AddOrChangePassword()
        {
            Console.Write("Please enter website/app name: ");
            var appName = Console.ReadLine();
            Console.Write("Please enter your password: ");
            var password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(appName) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("App name or password cannot be empty.");
                return;
            }

            _PasswordEntries[appName] = password;
            SavePasswords();
            Console.WriteLine("Password saved successfully.");
        }

        private static void GetPassword()
        {
            Console.Write("Please enter website/app name: ");
            var appName = Console.ReadLine();

            if (_PasswordEntries.TryGetValue(appName, out var password))
                Console.WriteLine($"Your password is: {password}");
            else
                Console.WriteLine("Password not found.");
        }

        private static void DeletePassword()
        {
            Console.Write("Please enter website/app name: ");
            var appName = Console.ReadLine();

            if (_PasswordEntries.Remove(appName))
            {
                SavePasswords();
                Console.WriteLine("Password deleted successfully.");
            }
            else
            {
                Console.WriteLine("Password not found.");
            }
        }

        private static void ReadPasswords()
        {
            if (File.Exists("passwords.txt"))
            {
                var passwordLines = File.ReadAllLines("passwords.txt");
                foreach (var line in passwordLines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var equalIndex = line.IndexOf('=');
                    if (equalIndex > 0)
                    {
                        var appName = line.Substring(0, equalIndex);
                        var password = line.Substring(equalIndex + 1);
                        _PasswordEntries[appName] = EncryptionUtility.Decrypt(password);
                    }
                }
            }
        }

        private static void SavePasswords()
        {
            var sb = new StringBuilder();
            foreach (var entry in _PasswordEntries)
                sb.AppendLine($"{entry.Key}={EncryptionUtility.Encrypt(entry.Value)}");

            File.WriteAllText("passwords.txt", sb.ToString());
        }
    }

    internal static class EncryptionUtility
    {
        public static string Encrypt(string plainText)
        {
            // Replace this with actual encryption logic.
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        public static string Decrypt(string cipherText)
        {
            // Replace this with actual decryption logic.
            return Encoding.UTF8.GetString(Convert.FromBase64String(cipherText));
        }
    }
}

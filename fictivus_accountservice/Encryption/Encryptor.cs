using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace fictivus_accountservice.Encryption
{
    public abstract class Encryptor
    {
        public static string EncryptPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        public static bool ValidatePassword(string originalPassword, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(originalPassword, storedPassword);
        }

        public static string EncryptEmail(string email)
        {
            return BCrypt.Net.BCrypt.HashPassword(email, BCrypt.Net.BCrypt.GenerateSalt());
        }

        public static bool ValidateEmail(string originalEmail, string storedEmail)
        {
            return BCrypt.Net.BCrypt.Verify(originalEmail, storedEmail);
        }

    }
}

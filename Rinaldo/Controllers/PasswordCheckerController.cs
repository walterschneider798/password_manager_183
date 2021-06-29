using EncryptStringSample;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rinaldo.Controllers
{
    public class PasswordCheckerController : Controller
    {
        public ActionResult AddPassword(string email, string password, string name, string addedPassword)
        {
            var json = string.Empty;
            try
            {
                var streamString = string.Empty;
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(System.IO.Directory.GetCurrentDirectory() + @"\JSON\passwords.json"))
                {
                    json = sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                throw e;
            }

            var passwordList = JsonConvert.DeserializeObject<List<PasswordModel>>(json);

            var user = AuthenticateUser(email, password);

            var tempPassword = user.Password;

            var encryptedPassword = DataEncryptor.Encrypt(addedPassword, tempPassword.Substring(16));


            var toAddPassword = new PasswordModel(Guid.NewGuid().ToString(), user.ID, name, encryptedPassword);
            passwordList.Add(toAddPassword);

            var newJson = JsonConvert.SerializeObject(passwordList);
            using (var sw = new StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\JSON\passwords.json"))
            {
                sw.Write(newJson);
            }

            return Content("Success");
        }
        public ActionResult GetAllPasswords(string email, string password)
        {
            var json = string.Empty;
            try
            {
                var streamString = string.Empty;
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(System.IO.Directory.GetCurrentDirectory() + @"\JSON\passwords.json"))
                {
                    json = sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                throw e;
            }

            var passwordList = JsonConvert.DeserializeObject<List<PasswordModel>>(json);
            var authenticatedUser = AuthenticateUser(email, password);
            if (authenticatedUser == null)
            {
                return Content("No User");
            }
            var userPasswordList = passwordList.Where(x => x.UserId == authenticatedUser.ID);

            foreach (var item in userPasswordList)
            {
                var tempPassword = authenticatedUser.Password;
                var decrypted = DataEncryptor.Decrypt(item.Password, tempPassword.Substring(16));
                item.Password = decrypted;
            }

            return Json(userPasswordList);
        }
        public class PasswordModel
        {
            public string ID { get; set; }
            public int UserId { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }

            public PasswordModel(string id, int userId, string name, string password)
            {
                this.ID = id;
                this.UserId = userId;
                this.Name = name;
                this.Password = password;
            }
        }
        private UserModel AuthenticateUser(string email, string password)
        {
            var json = string.Empty;
            try
            {
                var streamString = string.Empty;
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(System.IO.Directory.GetCurrentDirectory() + @"\JSON\users.json"))
                {
                    json = sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                throw e;
            }

            var userList = JsonConvert.DeserializeObject<List<UserModel>>(json);
            if (email != string.Empty && password != string.Empty)
            {

                var authenticationUser = userList.Where(x => x.Email == email)?.FirstOrDefault();
                // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: authenticationUser.Salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                if (hashed == authenticationUser.Password)
                {
                    return authenticationUser;
                }
            }
            return null;
        }
    }
}

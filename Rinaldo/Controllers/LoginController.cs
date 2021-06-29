using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rinaldo.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult AddUser(string email, string password)
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
            JsonResult addedUser;
            if (email != string.Empty && password != string.Empty && !userList.Any(x => x.Email == email))
            {
                var listLength = userList.Count();

                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                var newUser = new UserModel(listLength + 1, email, hashed, salt);
                addedUser = Json(newUser);
                userList.Add(newUser);
            }
            else
            {
                return Content("User already exists");
            }
            var newJson = JsonConvert.SerializeObject(userList);
            using (var sw = new StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\JSON\users.json"))
            {
                sw.Write(newJson);
            }

            return addedUser;
        }
        public ActionResult AuthenticateUser(string email, string password)
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
                if (authenticationUser == null)
                {
                    return Content("No User");
                }
                // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: authenticationUser.Salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                if (hashed == authenticationUser.Password)
                {
                    return Json(authenticationUser);
                }
                else
                {
                    return Content("User doesn't exist");
                }
            }
            return Content("User doesn't exist");
        }
    }
    public class UserModel
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public byte[] Salt { get; set; }

        public UserModel(int id, string email, string password, byte[] salt)
        {
            this.ID = id;
            this.Email = email;
            this.Password = password;
            this.Salt = salt;
        }
    }
}


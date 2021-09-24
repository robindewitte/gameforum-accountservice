using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using fictivus_accountservice.DTO;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using fictivus_accountservice.Encryption;
using fictivus_accountservice.Datamodels;
using fictivus_accountservice.Repositories;
using Microsoft.EntityFrameworkCore;

namespace fictivus_accountservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly AccountContext _context;

        private readonly IConfiguration _config;

        public AccountController(AccountContext context, IConfiguration config)
        {
            lastone = DateTime.Now;
            amountofWrongAttempts = 0;
            _context = context;
            _config = config;
            totalTime = 0;
        }

        private string GenerateJSONWebToken(LoginDTO loginAttempt)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, loginAttempt.Username)
            };


            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static int amountofWrongAttempts;
        private static double totalTime;
        private static DateTime lastone;


        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(LoginDTO loginDTO)
        {
            Console.WriteLine("binnen");
            //hardcoded for now
            Account fetchFromDB = await _context.Accounts.Where(b => b.Username == loginDTO.Username).SingleAsync();
            if (fetchFromDB != null && Encryptor.ValidatePassword(loginDTO.Password, fetchFromDB.Password))
            {
                string token = GenerateJSONWebToken(loginDTO);
                return token;
            }
            else
            {
                amountofWrongAttempts++;
                TimeSpan difference = DateTime.Now.Subtract(lastone);
                totalTime = totalTime + difference.TotalSeconds;
                if (totalTime > 30 && amountofWrongAttempts > 10)
                {
                    //theoretische email service hier
                    return "nu zou er een alert in de mail komen van een dictionary attack of ddos";
                }
                else if (totalTime > 30)
                {
                    totalTime = 0;
                }
                return "Verkeerd";
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<RegisterResponseDTO>> Register(RegisterDTO registerDTO)
        {
            //hardcoded for now
            if (ValidateEmail(registerDTO.EmailAdress) && ValidatePassword(registerDTO.Password))
            {
                Account account = new Account();
                account.Username = registerDTO.Username;
                account.Password = Encryptor.EncryptPassword(registerDTO.Password);
                account.EmailAdress = registerDTO.EmailAdress;
                if (_context.Accounts.Any(o => o.Username == account.Username) || _context.Accounts.Any(o => o.EmailAdress == account.EmailAdress))
                {
                    return Json(new RegisterResponseDTO("FOUT! de gebruikersnaam of het emailadress is in gebruik."));
                }
                else
                {
                    _context.Add(account);
                    await _context.SaveChangesAsync();
                }

                return Json(new RegisterResponseDTO("U bent geregistreerd"));
            }
            return Json(new RegisterResponseDTO("FOUT! De ingevoerde gegevens voldoen niet aan de eisen."));
        }


        public static bool ValidateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidatePassword(string pw)
        {
            if (pw.Length > 11 && pw.Any(char.IsUpper) && pw.Any(char.IsNumber) && pw.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                return true;
            }
            return false;
        }
    }
}

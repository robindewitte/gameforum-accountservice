using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace fictivus_accountservice.Datamodels
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string EmailAdress { get; set; }
        public string Password { get; set; }

        public Account()
        {

        }
    }
}

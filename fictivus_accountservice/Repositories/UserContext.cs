using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using fictivus_accountservice.Datamodels;

namespace fictivus_accountservice.Repositories
{
    public class AccountContext: DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options)
           : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

    }
}

using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Rodrap50.Financial.Api.Data;

namespace Rodrap50.Financial.Api.Models
{
    public class AccountCreateModel
    {
        public string AccountName { get; set; }
        public Boolean SoftAccount { get; set; }
        public string GeneralAccountId { get; set; }


        public Account GenerateAccount()
        {
            Account factory = new Account();
            factory.AccountName = this.AccountName;
            factory.Balance = 0;
            factory.SoftAccount = this.SoftAccount;
            factory.GeneralAccountId = this.GeneralAccountId;

            return factory;
        }
    }
}
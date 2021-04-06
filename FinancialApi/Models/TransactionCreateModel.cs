using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Rodrap50.Financial.Api.Data;
using Rodrap50.Financial.Api.Data.Base;

namespace Rodrap50.Financial.Api.Models {
    public class TransactionCreateModel : TransactionBase {
        
        public Transaction GenerateTransaction() {
            Transaction factory = new Transaction();
            factory.copy(this);            
            return factory;
        }
    }
}
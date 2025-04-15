using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Financial.Api.Data;
using Financial.Api.Data.Base;

namespace Financial.Api.Models; 
public class TransactionCreateModel : TransactionBase {
    
    public Transaction GenerateTransaction() {
        Transaction factory = new Transaction();
        factory.copy(this);            
        return factory;
    }
}

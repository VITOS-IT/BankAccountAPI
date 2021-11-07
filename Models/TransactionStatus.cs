using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServicesAPI.Models
{
    public class TransactionStatus
    {
        public string Message { get; set; }
        public float SourceBalance { get; set; }
        public float DestinationBalance { get; set; }
    }
}

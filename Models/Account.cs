using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServicesAPI.Models
{
    public class Account
    {
        [Key]
        public int AccountID { get; set; }
        public string AccountType { get; set; }
        public string CustomerID { get; set; }
        public float Balance { get; set; }

    }
}

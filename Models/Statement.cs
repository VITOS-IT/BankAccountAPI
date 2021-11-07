using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServicesAPI.Models
{
    public class Statement
    {
        public DateTime Date { get; set; }
        public string  Narration { get; set; }
        public int RefNo { get; set; }
        public DateTime ValueDate { get; set; }
        public float Withdraw { get; set; }
        public float Deposit { get; set; }
        public float ClosingBalace { get; set; }
    }
}

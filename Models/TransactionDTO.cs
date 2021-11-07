using System;

namespace AccountServicesAPI.Models
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public string Type { get; set; }
        public int SourceAccountId { get; set; }
        public int TargerAccountId { get; set; }
        public float AccountBalance { get; set; }
        public float Amount { get; set; }
        public string TransactionStatus { get; set; }
        public DateTime Date { get; set; }
    }
}

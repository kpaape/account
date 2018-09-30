using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoginReg.Models
{
    public class Transaction
    {
        [Key]
        public int id { get; set; }
        public int amount { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        public int user_id { get; set; }
        public User user {get;set;}
    }
}
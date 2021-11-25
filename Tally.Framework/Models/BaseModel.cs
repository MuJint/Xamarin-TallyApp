using System;
using System.ComponentModel.DataAnnotations;

namespace Tally.Framework.Models
{
    public class BaseModel
    {
        [Key]
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
    }
}

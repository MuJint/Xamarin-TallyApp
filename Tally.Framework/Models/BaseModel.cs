using SQLite;
using System;

namespace Tally.Framework.Models
{
    public class BaseModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Tally.Framework.Models
{
    public class BaseModel
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// time
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}

using Aggregates.Configurations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aggregates.Models.Students
{
    [Table("Students")]
    public class Student:Entity
    {
        public string Name { get; set; }
        public int? Age { get; set; }
    }
}

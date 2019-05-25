using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aggregates.Configurations
{
    public class Entity : IEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [NotMapped]
        public IDictionary<string, string> ValidationResults { get; set; } = new Dictionary<string, string>();
    }
}

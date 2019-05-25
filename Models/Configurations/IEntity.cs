using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregates.Configurations
{
    public interface IEntity:IValidationResult
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        long Id { get; set; }
        
    }
}

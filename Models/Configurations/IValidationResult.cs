using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregates.Configurations
{
    public interface IValidationResult
    {
        [NotMapped]
        IDictionary<string, string> ValidationResults { get; set; }
    }
}

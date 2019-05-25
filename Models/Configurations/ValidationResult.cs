using System.Collections.Generic;

namespace Aggregates.Configurations
{
    public class ValidationResult : IValidationResult
    {
        public IDictionary<string, string> ValidationResults { get; set; } = new Dictionary<string, string>();
    }
}

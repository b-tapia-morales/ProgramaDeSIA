using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramaDeSIA.Factors
{
    public static class AggregationOperations
    {
        public static Dictionary<Incidence, int> ByCount(IEnumerable<Factor> factors)
        {
            return factors
                .GroupBy(e => e.Incidence)
                .Select(e => new { 
                    Incidence = e.Key,
                    Frequency = e.Count() 
                }).ToDictionary(e => e.Incidence, e => e.Frequency);
        }
    }
}

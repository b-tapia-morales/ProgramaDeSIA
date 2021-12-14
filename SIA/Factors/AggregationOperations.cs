using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramaDeSIA.Factors
{
    public static class AggregationOperations
    {
        /// <summary>
        /// Retorna un diccionario con cada nivel de incidencia y la cantidad de veces que aparece en una lista de micro-factores
        /// </summary>
        /// <param name="factors">Micro-factores</param>
        /// <returns></returns>
        public static Dictionary<Incidence, int> ByCount(IEnumerable<Factor> factors)
        {
            return factors
                .GroupBy(e => e.Incidence)
                .Select(e => new
                {
                    Incidence = e.Key,
                    Frequency = e.Count()
                }).ToDictionary(e => e.Incidence, e => e.Frequency);
        }
    }
}
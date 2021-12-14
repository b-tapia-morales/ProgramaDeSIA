using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgramaDeSIA.Factors
{
    public static class FactorOperations
    {
        private static readonly double[,] MatrixA =
        {
            { 1, 1, 1 },
            { 4, -3, 0 },
            { 5, 0, -3 },
        };

        private static readonly double[,] MatrixB =
        {
            { 1 },
            { 0 },
            { 0 }
        };

        private static readonly List<double> MacrofactorWeights =
            MatrixOperations.CalculateWeight(MatrixA, MatrixB).ToList();

        private static IEnumerable<Factor> Filter(IEnumerable<Factor> factors, Incidence incidence)
        {
            return factors.Where(e => e.Incidence == incidence);
        }

        private static bool CheckTriggers(IEnumerable<Factor> factors)
        {
            return factors.Where(e => e.IsTrigger).Any(e => e.Value >= 0.75);
        }

        private static IEnumerable<Factor> FindTriggers(IEnumerable<Factor> factors)
        {
            return factors.Where(e => e.IsTrigger && e.Value >= 0.75);
        }

        private static double FilteredCalculation(IEnumerable<Factor> factors, double multiplier)
        {
            return multiplier * factors.Max(e => e.Value);
        }

        private static double MicroFactorsCalculation(IReadOnlyCollection<Factor> factors)
        {
            var dictionary = AggregationOperations.ByCount(factors);
            var matrix = MatrixOperations.GenerateWeightMatrix(dictionary);
            var weights = MatrixOperations.GenerateWeightList(matrix).ToList();
            var sum = 0.0;
            var j = 0;
            for (var i = 0; i < 5; i++)
            {
                var filteredFactors = Filter(factors, (Incidence)(i + 1)).ToList();
                if (filteredFactors.Count == 0)
                {
                    continue;
                }

                if (CheckTriggers(filteredFactors))
                {
                    Console.WriteLine("Factores disparadores sobre el umbral permitido:");
                    var triggers = FindTriggers(filteredFactors);
                    var printable = triggers.Select(e => e.ToString()).ToList();
                    printable.ForEach(Console.WriteLine);
                    return 1.0;
                }

                sum += FilteredCalculation(filteredFactors, weights[j]);
                j++;
            }

            return sum;
        }

        public static double OverallCalculation()
        {
            List<double> deterioration = new();
            for (var i = 0; i < 5; i++)
            {
                var list = StaticFactory.GenerateFactorsOnDemand(i).ToList();
                var printable = list.Select(e => e.ToString()).ToList();
                printable.ForEach(Console.WriteLine);
                deterioration.Add(MicroFactorsCalculation(list));
            }

            return (MacrofactorWeights[2] * deterioration[0] + MacrofactorWeights[0] * deterioration[1] +
                    MacrofactorWeights[1] * deterioration[2] + MacrofactorWeights[2] * deterioration[3] +
                    MacrofactorWeights[2] * deterioration[4]) /
                   (MacrofactorWeights[0] + 3 * MacrofactorWeights[1] + MacrofactorWeights[2]);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgramaDeSIA.Factors
{
    public static class FactorOperations
    {
        /*
         * En términos simples, Niveles de Incidencia:
         * Agua, Flora, Fauna -> Muy alto.
         * Suelo -> Alto.
         * Aire -> Medio.
         * Esto, según lo que determinó el grupo de Investigación.
         */
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

        /// <summary>
        /// Filtra los Micro-factores según el nivel de incidencia indicado.
        /// </summary>
        /// <param name="factors">Los Micro-factores</param>
        /// <param name="incidence">El Nivel de Incidencia de interés</param>
        /// <returns>La lista filtrada</returns>
        private static IEnumerable<Factor> Filter(IEnumerable<Factor> factors, Incidence incidence)
        {
            return factors.Where(e => e.Incidence == incidence);
        }

        /// <summary>
        /// Verifica si alguno de los Micro-factores que sean disparadores sobrepase el valor máximo de umbral.
        /// Basta con que tan sólo uno de ellos sobrepase el valor máximo de umbral para que el método retorne verdadero.
        /// </summary>
        /// <param name="factors">Los Micro-factores</param>
        /// <returns></returns>
        private static bool CheckTriggers(IEnumerable<Factor> factors)
        {
            return factors.Where(e => e.IsTrigger).Any(e => e.Value >= 0.75);
        }

        /// <summary>
        /// Retorna como lista todos y cada uno de los Micro-factores que sean disparadores, y cuyo nivel de deterioro
        /// sobrepasa el valor máximo de umbral
        /// </summary>
        /// <param name="factors">Los Micro-factores</param>
        /// <returns>Los Micro-factores que cumplen con la condición previamente indicada</returns>
        private static IEnumerable<Factor> FindTriggers(IEnumerable<Factor> factors)
        {
            return factors.Where(e => e.IsTrigger && e.Value >= 0.75);
        }

        /// <summary>
        /// Calcula el nivel de deterioro de aquellos Micro-factores cuyo Nivel de Incidencia sea el mismo para todos los
        /// elementos presentes en lista.
        /// </summary>
        /// <param name="factors">Los Micro-factores</param>
        /// <param name="multiplier">El Nivel de deterioro asociado al Nivel de Incidencia en cuestión</param>
        /// <returns>El nivel de deterioro</returns>
        private static double FilteredCalculation(IEnumerable<Factor> factors, double multiplier)
        {
            return multiplier * factors.Max(e => e.Value);
        }

        /// <summary>
        /// Calcula el nivel de deterioro total de los Micro-factores asociados a un Macro-factor en particular.
        /// </summary>
        /// <param name="factors">Los Micro-factores asociados al Macro-Factor de interés</param>
        /// <returns></returns>
        private static double MicroFactorsCalculation(IReadOnlyCollection<Factor> factors)
        {
            // Se forma un diccionario con los Niveles de Incidencia y sus frecuencias correspondientes.
            var dictionary = AggregationOperations.ByCount(factors);
            // Se forma la matriz con los pesos en base al diccionario.
            var matrix = MatrixOperations.GenerateWeightMatrix(dictionary);
            /*
            * Se resuelven los sistemas de ecuaciones lineales en base a la matriz de pesos, y se obtienen los pesos 
            * correspondientes como lista.
            */
            var weights = MatrixOperations.GenerateWeightList(matrix).ToList();
            var sum = 0.0;
            var j = 0;
            // Se itera por los cinco Niveles de Incidencia definidos.
            for (var i = 0; i < 5; i++)
            {
                // Se obtienen todos los Micro-factores correspondientes al Nivel de Incidencia actual.
                var filteredFactors = Filter(factors, (Incidence)(i + 1)).ToList();
                // Si no existe ningún Micro-factor asociado al Nivel de Incidencia actual, se ignora.
                if (filteredFactors.Count == 0)
                {
                    continue;
                }

                /*
                * Se verifica si alguno de los niveles de deterioro asociado a los factores disparadores supera el valor
                * máximo de umbral.
                */
                if (CheckTriggers(filteredFactors))
                {
                    /*
                     * Si es ese el caso, se imprimen por consola los factores disparadores en cuestión y se retorna el
                     * nivel de deterioro total (en este caso, es igual a 1).
                     */
                    Console.WriteLine("Factores disparadores sobre el umbral permitido:");
                    var triggers = FindTriggers(filteredFactors).Select(e => e.ToString()).ToList();
                    triggers.ForEach(Console.WriteLine);
                    return 1.0;
                }

                /*
                 * Se calcula el nivel de deterioro utilizando los Micro-factores y el peso asociados al Nivel de Incidencia
                 * en cuestión
                 */
                sum += FilteredCalculation(filteredFactors, weights[j]);
                j++;
            }

            return sum;
        }

        /// <summary>
        /// Calcula el nivel de deterioro total
        /// </summary>
        /// <returns>El nivel de deterioro</returns>
        public static double OverallCalculation()
        {
            List<double> deterioration = new();
            // Itera sobre los 5 Macro-factores definidos.
            for (var i = 0; i < 5; i++)
            {
                var list = StaticFactory.GenerateFactorsOnDemand(i).ToList();
                var printable = list.Select(e => e.ToString()).ToList();
                // Los imprime por consola primero.
                printable.ForEach(Console.WriteLine);
                // Calcula su Nivel de Deterioro asociado.
                deterioration.Add(MicroFactorsCalculation(list));
            }

            // Calcula el Nivel de Deterioro total en base a los pesos definidos como constantes al inicio de la clase.
            return (MacrofactorWeights[2] * deterioration[0] + MacrofactorWeights[0] * deterioration[1] +
                    MacrofactorWeights[1] * deterioration[2] + MacrofactorWeights[2] * deterioration[3] +
                    MacrofactorWeights[2] * deterioration[4]) /
                   (MacrofactorWeights[0] + 3 * MacrofactorWeights[1] + MacrofactorWeights[2]);
        }
    }
}
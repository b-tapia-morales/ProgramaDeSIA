using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ProgramaDeSIA.Factors
{
    public static class MatrixOperations
    {
        /// <summary>
        /// Genera la matriz de pesos en base al diccionario de los Niveles de Incidencia y sus frecuencias
        /// correspondientes.
        /// </summary>
        /// <param name="dictionary">El diccionario</param>
        /// <returns>La matriz de pesos</returns>
        public static double[,] GenerateWeightMatrix(Dictionary<Incidence, int> dictionary)
        {
            var n = dictionary.Count;
            // Genera la matriz cuadrada de pesos.
            var matrix = new double[n, n];
            // Inicializa los valores de la primera fila con 1's.
            for (var i = 0; i < n; i++)
            {
                matrix[0, i] = 1;
            }

            // Obtiene como lista las incidencias ordenadas por sus valores correspondientes en la enumeración original.
            var incidences = dictionary.Keys.OrderBy(e => (int)e).ToList();
            // El primer elemento es el valor base para calcular los demás pesos.
            var lowestIncidence = (int)incidences.First();
            var j = 1;
            for (var i = 1; i < n; i++)
            {
                /*
                 * El primer elemento en la columna correspondiente es siempre el factor multiplicativo necesario
                 * para poder expresar como igualdad la resta entre el peso del Nivel de Incidencia más bajo y el
                 * peso del Nivel de Incidencia a encontrar a través del sistema de ecuaciones.
                 */
                matrix[i, 0] = (int)incidences[i];
                // El Nivel de Incidencia más bajo siempre es negativo.
                matrix[i, j] = -lowestIncidence;
                j++;
            }

            return matrix;
        }

        public static IEnumerable<double> GenerateWeightList(double[,] matrix)
        {
            var n = matrix.GetLength(0);
            var columnMatrix = new double[n, 1];
            columnMatrix[0, 0] = 1;
            // Primer valor de la matriz columna es siempre 1, los demás valores son todos 0.
            for (var i = 1; i < n; i++)
            {
                columnMatrix[i, 0] = 0;
            }

            // Se encuentran los valores deseados a través del sistema de ecuaciones expresado.
            return CalculateWeight(matrix, columnMatrix);
        }
        
        /// <summary>
        /// Calcula los pesos a través del cálculo de las dos matrices que recibe como parámetros
        /// </summary>
        /// <param name="matrixA">Matriz cuadrada</param>
        /// <param name="matrixB">Matriz columna</param>
        /// <returns>La lista con los pesos</returns>
        public static IEnumerable<double> CalculateWeight(double[,] matrixA, double[,] matrixB)
        {
            var a = Matrix.Build.DenseOfArray(matrixA);
            var b = Matrix.Build.DenseOfArray(matrixB);
            return a.Solve(b).ToColumnMajorArray();
        }
        
    }
}
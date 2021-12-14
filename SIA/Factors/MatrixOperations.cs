using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ProgramaDeSIA.Factors
{
    public static class MatrixOperations
    {
        public static double[,] GenerateWeightMatrix(Dictionary<Incidence, int> dictionary)
        {
            var n = dictionary.Count;
            var matrix = new double[n, n];
            for (var i = 0; i < n; i++)
            {
                matrix[0, i] = 1;
            }

            var incidences = dictionary.Keys.OrderBy(e => (int)e).ToList();
            var lowestIncidence = (int)incidences.First();
            var j = 1;
            for (var i = 1; i < n; i++)
            {
                matrix[i, 0] = (int)incidences[i];
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
            for (var i = 1; i < n; i++)
            {
                columnMatrix[i, 0] = 0;
            }

            return CalculateWeight(matrix, columnMatrix);
        }
        
        public static IEnumerable<double> CalculateWeight(double[,] matrixA, double[,] matrixB)
        {
            var a = Matrix.Build.DenseOfArray(matrixA);
            var b = Matrix.Build.DenseOfArray(matrixB);
            return a.Solve(b).ToColumnMajorArray();
        }
        
    }
}
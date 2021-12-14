using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgramaDeSIA.Factors
{
    public static class StaticFactory
    {
        private static readonly Random Random = new();

        /// <summary>
        /// Crea la lista con los Micro-factores de agua.
        /// </summary>
        /// <returns>La lista con los Micro-factores de agua.</returns>
        private static IEnumerable<Factor> WaterFactors()
        {
            return new List<Factor>
            {
                new(MacroFactor.Water, "Florecimiento de Algas", Incidence.VeryLow, Random.NextDouble(), false),
                new(MacroFactor.Water, "Nivel de Profundidad del Agua", Incidence.Low, Random.NextDouble(), false),
                new(MacroFactor.Water, "Temperatura del Agua", Incidence.Low, Random.NextDouble(),
                    false),
                new(MacroFactor.Water, "Salinidad del Agua", Incidence.Medium, Random.NextDouble(), false),
                new(MacroFactor.Water, "Solubilidad de Contaminantes", Incidence.Medium, Random.NextDouble(),
                    false),
                new(MacroFactor.Water, "Caudal", Incidence.Medium, Random.NextDouble(), false),
                new(MacroFactor.Water, "Dosificación de Plaguicidas", Incidence.Medium, Random.NextDouble(),
                    false),
                new(MacroFactor.Water, "PH del Agua", Incidence.High, Random.NextDouble(),
                    true),
                new(MacroFactor.Water, "Turbidez del Agua", Incidence.High, Random.NextDouble(), true),
                new(MacroFactor.Water, "Sedimentación", Incidence.High, Random.NextDouble(), true),
                new(MacroFactor.Water, "Residuos Sólidos", Incidence.VeryHigh, Random.NextDouble(), true)
            };
        }

        /// <summary>
        /// Crea la lista con los Micro-factores de aire.
        /// </summary>
        /// <returns>La lista con los Micro-factores de aire.</returns>
        private static IEnumerable<Factor> AirFactors()
        {
            return new List<Factor>
            {
                new(MacroFactor.Air, "Nivel de CO2", Incidence.VeryLow, Random.NextDouble(), false),
                new(MacroFactor.Air, "Nivel de CO", Incidence.Low, Random.NextDouble(), false),
                new(MacroFactor.Air, "Compuestos Orgánicos Volátiles", Incidence.Medium, Random.NextDouble(),
                    false),
                new(MacroFactor.Air, "Dosificación de Plaguicidas", Incidence.High, Random.NextDouble(), false)
            };
        }

        /// <summary>
        /// Crea la lista con los Micro-factores de suelo.
        /// </summary>
        /// <returns>La lista con los Micro-factores de suelo.</returns>
        private static IEnumerable<Factor> SoilFactors()
        {
            return new List<Factor>
            {
                new(MacroFactor.Soil, "Dosificación de Plaguicidas", Incidence.Low, Random.NextDouble(), false),
                new(MacroFactor.Soil, "Salinidad del Suelo", Incidence.Medium, Random.NextDouble(), false),
                new(MacroFactor.Soil, "Nivel de Erosión", Incidence.High, Random.NextDouble(), true)
            };
        }

        /// <summary>
        /// Crea la lista con los Micro-factores de flora.
        /// </summary>
        /// <returns>La lista con los Micro-factores de flora.</returns>
        private static IEnumerable<Factor> FloraFactors()
        {
            return new List<Factor>
            {
                new(MacroFactor.Flora, "Dosificación de Plaguicidas", Incidence.Low, Random.NextDouble(), false),
                new(MacroFactor.Flora, "Salinidad del Suelo", Incidence.Low, Random.NextDouble(), false),
                new(MacroFactor.Flora, "Temperaturas Extremas", Incidence.Low, Random.NextDouble(), false),
                new(MacroFactor.Flora, "PH del Agua", Incidence.Medium, Random.NextDouble(), false)
            };
        }

        /// <summary>
        /// Crea la lista con los Micro-factores de fauna.
        /// </summary>
        /// <returns>La lista con los Micro-factores de fauna.</returns>
        private static IEnumerable<Factor> FaunaFactors()
        {
            return new List<Factor>
            {
                new(MacroFactor.Fauna, "Sobre Reproducción", Incidence.Low, Random.NextDouble(), false),
                new(MacroFactor.Fauna, "Introducción de Especies", Incidence.Medium, Random.NextDouble(), false),
                new(MacroFactor.Fauna, "Diversidad Animal", Incidence.Medium, Random.NextDouble(), false),
                new(MacroFactor.Fauna, "Plagas", Incidence.High, Random.NextDouble(), false),
            };
        }

        /// <summary>
        /// Genera una lista en base al índice que recibe como parámetro.
        /// 1 -> Agua
        /// 2 -> Aire
        /// 3 -> Suelo
        /// 4 -> Flora
        /// 5 -> Fauna
        /// </summary>
        /// <returns>La lista con los Micro-factores de interés.</returns>
        public static IEnumerable<Factor> GenerateFactorsOnDemand(int value)
        {
            return value switch
            {
                0 => WaterFactors(),
                1 => AirFactors(),
                2 => SoilFactors(),
                3 => FloraFactors(),
                4 => FaunaFactors(),
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };
        }

        /// <summary>
        /// Genera una lista con todos los factores incluidos, independiente de su tipo.
        /// </summary>
        /// <returns>La lista con todos los factores.</returns>
        public static IEnumerable<Factor> AllFactors()
        {
            return WaterFactors().Concat(AirFactors()).Concat(SoilFactors()).Concat(FloraFactors()).Concat(FaunaFactors());
        }
        
    }
}
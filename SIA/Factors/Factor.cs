namespace ProgramaDeSIA.Factors
{
    public class Factor
    {
        private MacroFactor MacroFactor { get; }
        private string Name { get; }
        internal Incidence Incidence { get; }
        internal double Value { get; }
        internal bool IsTrigger { get; }

        public Factor(MacroFactor macroFactor, string name, Incidence incidence, double value, bool isTrigger)
        {
            MacroFactor = macroFactor;
            Name = name;
            Incidence = incidence;
            Value = value;
            IsTrigger = isTrigger;
        }

        public override string ToString()
        {
            return
                $"Macrofactor: {MacroFactor}\nNombre: {Name}\nIncidencia: {Incidence}\nValor: {Value}\nEs disparador?: {IsTrigger}\n";
        }
    }

    public enum Incidence
    {
        VeryLow = 1,
        Low = 2,
        Medium = 3,
        High = 4,
        VeryHigh = 5
    }

    public enum MacroFactor
    {
        Water = 0,
        Air = 1,
        Soil = 2,
        Flora = 3,
        Fauna = 4
    }
}
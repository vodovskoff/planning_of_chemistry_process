using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Планирование_химического_процесса
{
    public class ReactionSubstance
    {
        public ChemicalSubstance Substance { get; set; }
        public double? MolarMass { get; set; }
        public int Coefficient { get; set; }

        public ReactionSubstance(ChemicalSubstance substance, double? molarMass, int coefficient)
        {
            Substance = substance;
            MolarMass = molarMass;
            Coefficient = coefficient;
        }
    }
}

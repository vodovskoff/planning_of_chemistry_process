using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Планирование_химического_процесса
{
    public class ReactionSubstance
    {
        public string Name => Substance.SubstanceName;
        public ChemicalSubstance Substance { get; set; }
        public double? MolarMass => Substance.MolarMass;
        public int Coefficient { get; set; }
        public bool isExecuting { get; set; }
        public ReactionSubstance(ChemicalSubstance substance, int coefficient)
        {
            Substance = substance;
            Coefficient = coefficient;
            isExecuting = true;
        }
    }
}

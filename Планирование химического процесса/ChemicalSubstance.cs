using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Планирование_химического_процесса
{
    public class ChemicalSubstance
    {
        public string Substance { get; set; }
        public double? MolarMass { get; set; }
        public double? Mass { get; set; }

        public ChemicalSubstance(string substance, double? molarMass)
        {
            Substance = substance;
            MolarMass = molarMass;
        }
    }
}

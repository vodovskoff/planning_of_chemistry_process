using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Планирование_химического_процесса
{
    class IsDataFullChecker
    {
        public static string CheckChemicalSubstance(ChemicalSubstance substance)
        {
                if (substance.MolarMass ==null)
                {
                    return "молярная масса не введена";
                }

            return "ok";
        }

        public static string CheckReactions(ChemicalReaction reaction)
        {
                if (reaction.Reactants.Count==0)
                {
                    return "реагенты не введены";
                }

                if (reaction.Products.Count == 0)
                {
                return "реакции не введены";
                }

                foreach (var product in reaction.Products)
                {
                    if (product.Coefficient==null)
                    {
                    return $"коэффициент {product.Substance.SubstanceName} не введён";
                    }
                }
            return "ok";
        }
    }
}

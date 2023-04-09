using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Планирование_химического_процесса
{
    public class ChemicalReaction
    {
        public String reactionName {  get; set; }
        public HashSet<ReactionSubstance> Reactants { get; set; }
        public HashSet<ReactionSubstance> Products { get; set; }
    }
}

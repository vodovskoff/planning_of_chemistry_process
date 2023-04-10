using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Планирование_химического_процесса
{
    public class ChemicalReactionStorage
    {
        private const string FileName = "chemical_reactions.json";

        public List<ChemicalReaction> Reactions { get; set; }

        public ChemicalReactionStorage(List<ChemicalReaction> reactions)
        {
            Reactions = reactions;
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(Reactions);
            File.WriteAllText(FileName, json);
        }

        public List<ChemicalReaction> Load()
        {
            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);
                List<ChemicalReaction> reactions = JsonConvert.DeserializeObject<List<ChemicalReaction>>(json);
                return reactions;
            }
            return new List<ChemicalReaction>();
        }
    }
}

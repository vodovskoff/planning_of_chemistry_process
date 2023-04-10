using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Планирование_химического_процесса
{
    public class ChemicalSubstanceStorage
    {
        private const string FileName = "chemical_substances.json";

        public List<ChemicalSubstance> Substances { get; set; }

        public ChemicalSubstanceStorage(List<ChemicalSubstance> substances)
        {
            Substances = substances;
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(Substances);
            File.WriteAllText(FileName, json);
        }

        public List<ChemicalSubstance> Load()
        {
            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);
                List<ChemicalSubstance> substances = JsonConvert.DeserializeObject<List<ChemicalSubstance>>(json);
                return substances;
            }
            return new List<ChemicalSubstance>();
        }
    }
}

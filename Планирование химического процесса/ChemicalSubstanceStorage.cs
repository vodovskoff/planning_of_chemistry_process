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

        public List<ChemicalSubstance> Load(string fileName)
        {
            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                List<ChemicalSubstance> substances = JsonConvert.DeserializeObject<List<ChemicalSubstance>>(json);
                return substances;
            }
            return new List<ChemicalSubstance>();
        }
    }
}

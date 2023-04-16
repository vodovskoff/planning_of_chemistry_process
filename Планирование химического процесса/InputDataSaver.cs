using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Планирование_химического_процесса
{
    class InputDataSaver
    {
        private const string StartSubstancesFileName = "start_substances.json";
        private const string TargetSubstancesFileName = "target_substances.json";

        public List<ReactionSubstance> startSubstances { get; set; }
        public List<ReactionSubstance> targetSubstances { get; set; }

        public InputDataSaver(List<ReactionSubstance> startSubstances, List<ReactionSubstance> targetSubstances)
        {
            this.startSubstances = startSubstances;
            this.targetSubstances = targetSubstances;
        }

        public void Save()
        {
            string jsonStart = JsonConvert.SerializeObject(startSubstances);
            string jsonTarget = JsonConvert.SerializeObject(targetSubstances);
            File.WriteAllText(StartSubstancesFileName, jsonStart);
            File.WriteAllText(TargetSubstancesFileName, jsonTarget);
        }

        public List<ReactionSubstance> LoadStartSubstances()
        {
            if (File.Exists(StartSubstancesFileName))
            {
                string json = File.ReadAllText(StartSubstancesFileName);
                List<ReactionSubstance> startSubstancesNew = JsonConvert.DeserializeObject<List<ReactionSubstance>>(json);
                return startSubstancesNew;
            }
            return new List<ReactionSubstance>();
        }

        public List<ReactionSubstance> LoadTargetSubstances()
        {
            if (File.Exists(TargetSubstancesFileName))
            {
                string json = File.ReadAllText(TargetSubstancesFileName);
                List<ReactionSubstance> targetSubstancesNew = JsonConvert.DeserializeObject<List<ReactionSubstance>>(json);
                return targetSubstancesNew;
            }
            return new List<ReactionSubstance>();
        }
    }
}

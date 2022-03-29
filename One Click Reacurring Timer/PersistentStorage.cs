using One_Click_Reacurring_Timer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace One_Click_Reacurring_Timer
{
    public class PersistentStorage
    {
        private static readonly string _saveTo = Directory.GetCurrentDirectory() + @"\dateAndTimeClicked.json";
        static public List<ClickedModel> GetZeiten()
        {
            try
            {
                string jsonResult = File.ReadAllText(_saveTo);
                List<ClickedModel> returnList = JsonSerializer.Deserialize<List<ClickedModel>>(jsonResult);
                return returnList;
            }
            catch (FileNotFoundException)
            {
                return new List<ClickedModel>();
            }
        }
        static public void WriteZeitenList(List<ClickedModel> toSave)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string toWrite = JsonSerializer.Serialize(toSave, options);
            File.WriteAllText(_saveTo, toWrite);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace One_Click_Reacurring_Timer.Models
{
    public class ClickedModel
    {
        public string Date { get; set; }
        public string Time { get; set; }

        [JsonConstructor]
        public ClickedModel(string date, string time)
        {
            Date = date;
            Time = time;
        }
    }
}

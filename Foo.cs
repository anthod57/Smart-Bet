using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Bet
{
    public class Foo
    {
        [Index(0)]

        public string League { get; set; }

        public string Date { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public int? FTHG { get; set; }

        public int? FTAG { get; set; }

        public double? B365A { get; set; }

        public double? B365H { get; set; }

        public double? B365D { get; set; }
    }
}

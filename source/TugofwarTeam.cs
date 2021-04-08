using System;
using System.Collections.Generic;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class TugofwarTeam : Team
    {
        public TugofwarTeam(string name) : base(name)
        {
            MaxSize = 8;
        }
        public override string TypeOfSportAsString
        {
            get => "Przeciąganie liny";
        }
        public override Sport TypeOfSportAsEnum
        {
            get => Sport.Tugofwar;
        }
    }
}

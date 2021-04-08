using System;
using System.Collections.Generic;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class DodgeballTeam : Team
    {
        public DodgeballTeam(string name) : base(name)
        {
            MaxSize = 6;
        }
        public override Sport TypeOfSportAsEnum
        {
            get => Sport.Dodgeball;
        }
        public override string TypeOfSportAsString
        {
            get => "Dwa ognie";
        }
    }
}

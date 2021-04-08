using System;
using System.Collections.Generic;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class VolleyballTeam : Team
    {
        public VolleyballTeam(string name) : base(name)
        {
            MaxSize = 2;
        }

        public override Sport TypeOfSportAsEnum
        {
            get => Sport.Volleyball;
        }
        public override string TypeOfSportAsString
        {
            get => "Siatkówka";
        }
    }
}

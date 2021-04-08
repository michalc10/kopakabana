using System;
using System.Collections.Generic;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class TugofwarMatch : Match
    {
        public TugofwarMatch() : base()
        {
        }

        public TugofwarMatch(Team team1, Team team2, Referee referee) : base(team1, team2, referee)
        {
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

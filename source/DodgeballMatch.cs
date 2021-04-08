using System;
using System.Collections.Generic;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class DodgeballMatch : Match
    {
        public DodgeballMatch() : base()
        {
        }

        public DodgeballMatch(Team team1, Team team2, Referee referee) : base(team1, team2, referee)
        {
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

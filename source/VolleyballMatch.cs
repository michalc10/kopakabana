using System;
using System.Collections.Generic;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class VolleyballMatch : Match
    {
        public Referee AssistantReferee1 { get; set; }
        public Referee AssistantReferee2 { get; set; }
        public VolleyballMatch() : base()
        {
        }

        public VolleyballMatch(Team team1, Team team2, Referee referee) : base(team1, team2, referee)
        {
        }
        public VolleyballMatch(Team team1, Team team2, Referee referee, Referee assistant1, Referee assistant2) : base(team1, team2, referee)
        {
            AssistantReferee1 = assistant1;
            AssistantReferee2 = assistant2;
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

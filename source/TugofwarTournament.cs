using System;
using System.Collections.Generic;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class TugofwarTournament : Tournament
    {
        public TugofwarTournament(string name) : base(name)
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

        public override void GenerateFinal()
        {
            if (Semifinal1 == null || Semifinal2 == null) throw new SemifinalsNotGeneratedException("There is no semifinals.");
            if (String.IsNullOrWhiteSpace(Semifinal1.Score)) throw new EmptyScoresException($"There is no score assigned to match with id {Semifinal1.Id}", Semifinal1.Id);
            if (String.IsNullOrWhiteSpace(Semifinal2.Score)) throw new EmptyScoresException($"There is no score assigned to match with id {Semifinal2.Id}", Semifinal2.Id);
            GenerateFinalsHelper(new TugofwarMatch());
        }

        public override void GenerateSemifinals()
        {
            GenerateScoreTable();
            if (scoreTable.Count < 4) throw new NotEnoughTeamsException($"Not enough teams in the torurnament ({scoreTable.Count}/4)", scoreTable.Count);
            GenerateSemifinalsHelper(new TugofwarMatch(), new TugofwarMatch());
        }
    }
}

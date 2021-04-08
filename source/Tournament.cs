using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.InteropServices.ComTypes;

namespace Kopakabana.source
{
    [Serializable]
    public abstract class Tournament : ITournament
    {
        private static int lastId = -1;
        public static void ResetIdCounter()
        {
            lastId = -1;
        }
        public Match Semifinal1 { get; private set; }
        public Match Semifinal2 { get; private set; }
        public Match Final { get; private set; }

        protected ObservableCollection<Match> matches = new ObservableCollection<Match>();
        protected List<ScoreTableRow> scoreTable = new List<ScoreTableRow>();
        public int Id { get; private set; }
        public string Name { get; set; }
        private static int GenerateNewId()
        {
            return ++lastId;
        }

        public Tournament(string name)
        {
            Id = GenerateNewId();
            Name = name;
        }
        public void AddMatch(Match match)
        {
            matches.Add(match);
        }
        public void RemoveMatch(int id)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].Id == id)
                {
                    matches.RemoveAt(i);
                    return;
                }
            }
            throw new InvalidMatchIdException($"There is no match with id {id}", id);
        }
        public Match GetMatch(int id)
        {
            foreach (Match match in matches)
            {
                if (match.Id == id) return match;
            }
            throw new InvalidMatchIdException($"There is no match with id {id}", id);
        }
        public ObservableCollection<Match> GetMatchesList()
        {
            return matches;
        }
        private int GetTeamIndexInScoreTable(Team team)
        {
            for (int i = 0, size = scoreTable.Count; i < size; i++)
            {
                if (scoreTable[i].TeamId == team.Id) return i;
            }
            return -1;
        }
        private void CreateRowInScoreTable(Team team)
        {
            ScoreTableRow row = new ScoreTableRow();
            row.TeamId = team.Id;
            row.TeamName = team.Name;
            scoreTable.Add(row);
        }
        protected void GenerateScoreTable()
        {
            scoreTable.Clear();
            foreach (Match match in matches)
            {
                if (String.IsNullOrWhiteSpace(match.Score)) throw new EmptyScoresException($"There is no score assigned to match with id {match.Id}", match.Id);
                string[] score = match.Score.Split('-');
                int score1 = int.Parse(score[0]);
                int score2 = int.Parse(score[1]);
                int i1 = GetTeamIndexInScoreTable(match.Team1);
                int i2 = GetTeamIndexInScoreTable(match.Team2);
                if (i1 < 0)
                {
                    CreateRowInScoreTable(match.Team1);
                    i1 = scoreTable.Count - 1;
                }
                if (i2 < 0)
                {
                    CreateRowInScoreTable(match.Team2);
                    i2 = scoreTable.Count - 1;
                }
                ScoreTableRow row1 = scoreTable[i1];
                ScoreTableRow row2 = scoreTable[i2];
                if (score1 == score2)
                {
                    row1.Draws++;
                    row2.Draws++;
                }
                else if (score1 > score2)
                {
                    row1.Wins++;
                    row2.Losses++;
                }
                else
                {
                    row1.Losses++;
                    row2.Wins++;
                }
                scoreTable[i1] = row1;
                scoreTable[i2] = row2;
            }
        }
        public abstract void GenerateSemifinals();
        protected void GenerateSemifinalsHelper(Match newSemifinal1, Match newSemifinal2)
        {
            GenerateScoreTable();
            int[] max_point = new int[] { 0, 0, 0, 0 };
            foreach (ScoreTableRow scores in scoreTable)
            {
                int points = scores.Wins * 3 + scores.Draws - scores.Losses;
                for (int i = 0; i < 4; i++)
                {
                    if (points > max_point[i])
                    {
                        for (int j = 3; j > i; j--)
                            max_point[j] = max_point[j - 1];
                        max_point[i] = points;
                        break;
                    }
                }
            }
            int[] check = new int[] { 0, 0, 0, 0 };
            foreach (ScoreTableRow scores in scoreTable)
            {
                int points = scores.Wins * 3 + scores.Draws - scores.Losses;
                if (points == max_point[0] && check[0] == 0)
                {
                    newSemifinal1.Team1 = getTeamFromMatches(scores.TeamId);
                    check[0]++;
                }
                else if (points == max_point[1] && check[1] == 0)
                {
                    newSemifinal1.Team2 = getTeamFromMatches(scores.TeamId);
                    check[1]++;
                }
                else if (points == max_point[2] && check[2] == 0)
                {
                    newSemifinal2.Team1 = getTeamFromMatches(scores.TeamId);
                    check[2]++;
                }
                else if (points == max_point[3] )
                {
                    newSemifinal2.Team2 = getTeamFromMatches(scores.TeamId);
                    check[3]++;
                }
            }
            if (check[3] > 1) throw new ImpossibleToCreateSemifinals("Impossible to create semifinals with that points");
            else
            {
                newSemifinal1.Date = DateTime.Now;
                newSemifinal2.Date = DateTime.Now;
                Semifinal1 = newSemifinal1;
                Semifinal2 = newSemifinal2;
            }

        }
        private Team getTeamFromMatches(int id)
        {
            foreach (Match match in matches)
            {
                if (match.Team1.Id == id)
                    return match.Team1;
                if (match.Team2.Id == id)
                    return match.Team2;
            }
            throw new InvalidTeamIdException($"Couldn't find team with id {id}", id);
        }
        public abstract void GenerateFinal();
        protected void GenerateFinalsHelper(Match newMatch)
        {
            string[] score = Semifinal1.Score.Split('-');
            int score1 = int.Parse(score[0]);
            int score2 = int.Parse(score[1]);
            if (score1 > score2)
                newMatch.Team1 = Semifinal1.Team1;
            else
                newMatch.Team1 = Semifinal1.Team2;

            score = Semifinal2.Score.Split('-');
            score1 = int.Parse(score[0]);
            score2 = int.Parse(score[1]);
            if (score1 > score2)
                newMatch.Team2 = Semifinal2.Team1;
            else
                newMatch.Team2 = Semifinal2.Team2;
            newMatch.Date = DateTime.Now;
            Final = newMatch;
        }
        public List<ScoreTableRow> GetScoreBoard()
        {
            GenerateScoreTable();
            return scoreTable;
        }
        public Match GetSemiFinal1()
        {
            return Semifinal1;
        }
        public Match GetSemiFinal2()
        {
            return Semifinal2;
        }
        public Match GetFinal()
        {
            return Final;
        }

        public override string ToString()
        {
            return Id + " " + Name;
        }
        public abstract string TypeOfSportAsString { get; }
        public abstract Sport TypeOfSportAsEnum { get; }

        [OnDeserialized]
        private void RepairId(StreamingContext context)
        {
            if (Id > lastId) lastId = Id;
        }
    }
}
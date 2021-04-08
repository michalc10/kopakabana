using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Kopakabana.source
{
    interface IMainManager
    {
        TournamentsManager Tournaments { get; }
        TeamsManager Teams { get; }
        RefereesManager Referees { get; }
        void SaveToFile(string path);
    }
    interface ITournamentsManager
    {
        void AddTournament(Tournament newTournament);
        void RemoveTournament(int id);
        Tournament GetTournament(int id);
        ObservableCollection<Tournament> GetTournamentList();
    }
    interface ITeamsManager
    {
        void AddTeam(Team newTeam);
        void RemoveTeam(int id);
        Team GetTeam(int id);
        ObservableCollection<Team> GetTeamsList();
        ObservableCollection<Team> GetVolleyballTeamsList();
        ObservableCollection<Team> GetDodgeballTeamsList();
        ObservableCollection<Team> GetTugofwarTeamsList();
    }
    interface IRefereesManager
    {
        void AddReferee(Referee newRefree);
        void RemoveReferee(int id);
        Referee GetReferee(int id);
        ObservableCollection<Referee> GetRefereesList();
    }
    interface IPerson
    {
        string Name { get; set; }
        string Surname { get; set; }
        int Age { get; set; }
        int Id { get; }
    }
    interface ITeam
    {
        string Name { get; set; }
        int Id { get; }
        int MaxSize { get; }
        int CurrentSize { get; }
        Sport TypeOfSportAsEnum { get; }
        string TypeOfSportAsString { get; }
        void AddMember(Player player);
        void RemoveMember(int id);
        bool IsFull();
        ObservableCollection<Player> GetMembersList();
    }
    interface IMatch
    {
        int Id { get; }
        Team Team1 { get; set; }
        Team Team2 { get; set; }
        Referee Referee { get; set; }
        DateTime Date { get; set; }
        string Time { get; set; }
        string Score { get; set; }
        Sport TypeOfSportAsEnum { get; }
        string TypeOfSportAsString { get; }
    }
    interface ITournament
    {
        Match Semifinal1 { get; }
        Match Semifinal2 { get; }
        Match Final { get; }
        int Id { get; }
        string Name { get; set; }
        Sport TypeOfSportAsEnum { get; }
        string TypeOfSportAsString { get; }
        void RemoveMatch(int id);
        Match GetMatch(int id);
        ObservableCollection<Match> GetMatchesList();
        void GenerateSemifinals();
        void GenerateFinal();
        List<ScoreTableRow> GetScoreBoard();

    }
}

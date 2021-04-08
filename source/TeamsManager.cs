using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class TeamsManager : ITeamsManager
    {
        public ObservableCollection<Team> teams = new ObservableCollection<Team>();
        public void AddTeam(Team newTeam)
        {
            teams.Add(newTeam);
        }

        

        public Team GetTeam(int id)
        {
            foreach (Team team in teams)
            {
                if (team.Id == id) return team;
            }
            throw new InvalidTeamIdException($"There is no team with id {id}", id);
        }

        public ObservableCollection<Team> GetTeamsList()
        {
            return teams;
        }

        public ObservableCollection<Team> GetVolleyballTeamsList()
        {
            ObservableCollection<Team> output = new ObservableCollection<Team>();
            foreach (Team team in teams)
            {
                VolleyballTeam casted = team as VolleyballTeam;
                if (casted != null)
                    output.Add(casted);
            }
            return output;
        }
        public ObservableCollection<Team> GetDodgeballTeamsList()
        {
            ObservableCollection<Team> output = new ObservableCollection<Team>();
            foreach (Team team in teams)
            {
                DodgeballTeam casted = team as DodgeballTeam;
                if (casted != null)
                    output.Add(casted);
            }
            return output;
        }
        public ObservableCollection<Team> GetTugofwarTeamsList()
        {
            ObservableCollection<Team> output = new ObservableCollection<Team>();
            foreach (Team team in teams)
            {
                TugofwarTeam casted = team as TugofwarTeam;
                if (casted != null)
                    output.Add(casted);
            }
            return output;
        }

        public void RemoveTeam(int id)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].Id == id)
                {
                    teams.RemoveAt(i);
                    return;
                }
            }
            throw new InvalidTeamIdException($"There is no Team with id {id}", id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class TournamentsManager : ITournamentsManager
    {
        ObservableCollection<Tournament> tournaments = new ObservableCollection<Tournament>();

        public void AddTournament(Tournament newTournament)
        {
            tournaments.Add(newTournament);
        }

        public void RemoveTournament(int id)
        {
            for (int i = 0; i < tournaments.Count; i++)
            {
                if (tournaments[i].Id == id)
                {
                    tournaments.RemoveAt(i);
                    return;
                }
            }
            throw new InvalidTournamentIdException($"There is no tournament with id {id}", id);
        }

        public Tournament GetTournament(int id)
        {
            foreach (Tournament tournament in tournaments)
            {
                if (tournament.Id == id) return tournament;
            }
            throw new InvalidTournamentIdException($"There is no tournament with id {id}", id);
        }

        public ObservableCollection<Tournament> GetTournamentList()
        {
            return tournaments;
        }
    }
}

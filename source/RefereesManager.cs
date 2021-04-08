using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class RefereesManager : IRefereesManager
    {
        private ObservableCollection<Referee> referees = new ObservableCollection<Referee>();
        public void AddReferee(Referee newReferee)
        {
            referees.Add(newReferee);
        }

        public Referee GetReferee(int id)
        {
            foreach(Referee referee in referees)
            {
                if (referee.Id == id) return referee;
            }
            throw new InvalidRefereeIdException($"There is no referee with id {id}",id);
        }

        public ObservableCollection<Referee> GetRefereesList()
        {
            return referees;
        }

        public void RemoveReferee(int id)
        {
            for (int i = 0; i < referees.Count; i++)
            {
                if (referees[i].Id == id)
                {
                    referees.RemoveAt(i);
                    return;
                }
            }
            throw new InvalidRefereeIdException($"There is no referee with id {id}", id);
        }
    }
}

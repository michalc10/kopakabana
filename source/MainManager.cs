using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class MainManager : IMainManager
    {
        public TournamentsManager Tournaments { get; }

        public TeamsManager Teams { get; }

        public RefereesManager Referees { get; }
        public MainManager()
        {
            Tournaments = new TournamentsManager();
            Teams = new TeamsManager();
            Referees = new RefereesManager();
        }
        public static void ResetAllIdCounters()
        {
            Person.ResetIdCounter();
            Team.ResetIdCounter();
            Tournament.ResetIdCounter();
            Match.ResetIdCounter();
        }
        public void SaveToFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, this);
            }
            finally
            {
                fs.Close();
            }
        }
        public static MainManager LoadFromFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            MainManager output = null;
            try
            {
                output = (MainManager)formatter.Deserialize(fs);
            }
            finally
            {
                fs.Close();
            }
            
            return output;
        }
    }
}

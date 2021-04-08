using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Runtime.Serialization;
namespace Kopakabana.source
{
    [Serializable]
    abstract public class Team : ITeam
    {
        private ObservableCollection<Player> members = new ObservableCollection<Player>();
        private static int lastId = -1;
        public static void ResetIdCounter() 
        {
            lastId = -1;
        }
        public int MaxSize { get; protected set; }
        public int CurrentSize { get; private set; } //licznik graczy
        private static int GenerateNewId()
        {
            return ++lastId;
        }
        public string Name { get; set; }
        public int Id { get; private set; }
        public Team(string name)
        {
            MaxSize = 0;
            CurrentSize = 0;
            Name = name;
            Id = GenerateNewId();
        }
        public bool IsFull() { return CurrentSize == MaxSize; }
        public void AddMember(Player player)
        {
            if (IsFull())
                throw new FullTeamException($"Team {Name} is full!");
            members.Add(player);
            CurrentSize++;
        }
        public ObservableCollection<Player> GetMembersList()
        {
            return members;
        }
        public void RemoveMember(int id)
        {
            for (int i = 0; i < members.Count; i++)
            {
                if (members[i].Id == id)
                {
                    members.RemoveAt(i);
                    CurrentSize--;
                    return;
                }
            }
            throw new InvalidPlayerIdException($"There is no member with id {id}", id);
        }
        public override string ToString()
        {
            return Id+" "+Name;
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
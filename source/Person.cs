using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class Person : IPerson
    {
        private static int lastId = -1;
        public static void ResetIdCounter()
        {
            lastId = -1;
        }
        private static int GenerateNewId()
        {
            return ++lastId;
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public int Id { get; }
        public Person(string name, string surname, int age)
        {
            Name = name;
            Surname = surname;
            Age = age;
            Id = GenerateNewId();
        }
        public override string ToString()
        {
            return Id + " " + Name + " " + Surname + " " + Age;
        }
        [OnDeserialized]
        private void RepairId(StreamingContext context)
        {
            if (Id > lastId) lastId = Id;
        }
    }
}

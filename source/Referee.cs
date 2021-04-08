using System;
using System.Collections.Generic;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class Referee : Person
    {
        public Referee(string name, string surname, int age) : base(name, surname, age)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Kopakabana.source
{
    [Serializable]
    public class Player : Person
    {
        public Player(string name, string surname, int age) : base(name, surname, age)
        {
        }
    }
}

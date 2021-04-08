using System;

namespace Kopakabana.source
{
    [Serializable]
    public struct ScoreTableRow
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
    }
}
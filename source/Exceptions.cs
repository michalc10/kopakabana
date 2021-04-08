using System;
using System.Collections.Generic;
using System.Text;

namespace Kopakabana.source
{
    public class InvalidIdException : Exception
    {
        int invalidId;
        public InvalidIdException(int id)
        {
            invalidId = id;
        }

        public InvalidIdException(string message, int id) : base(message)
        {
            invalidId = id;
        }
        public int GetId() => invalidId;
    }
    public class InvalidRefereeIdException : InvalidIdException
    {
        public InvalidRefereeIdException(int id) : base(id)
        {
        }

        public InvalidRefereeIdException(string message, int id) : base(message, id)
        {
        }
    }
    public class InvalidTeamIdException : InvalidIdException
    {
        public InvalidTeamIdException(int id) : base(id)
        {
        }

        public InvalidTeamIdException(string message, int id) : base(message, id)
        {
        }
    }
    public class InvalidPlayerIdException : InvalidIdException
    {
        public InvalidPlayerIdException(int id) : base(id)
        {
        }

        public InvalidPlayerIdException(string message, int id) : base(message, id)
        {
        }
    }
    public class FullTeamException : Exception
    {
        public FullTeamException()
        {
        }

        public FullTeamException(string message) : base(message)
        {
        }
    }
    public class InvalidTournamentIdException : InvalidIdException
    {
        public InvalidTournamentIdException(int id) : base(id)
        {
        }

        public InvalidTournamentIdException(string message, int id) : base(message, id)
        {
        }
    }
    public class InvalidMatchIdException : InvalidIdException
    {
        public InvalidMatchIdException(int id) : base(id)
        {
        }

        public InvalidMatchIdException(string message, int id) : base(message, id)
        {
        }
    }
    public class NotEnoughTeamsException : Exception
    {
        private int currentCount;
        public int GetCurrentCount() => currentCount;
        public NotEnoughTeamsException(int count)
        {
            currentCount = count;
        }

        public NotEnoughTeamsException(string message, int count) : base(message)
        {
            currentCount = count;
        }
    }
    public class EmptyScoresException : Exception
    {
        private int id;
        public int GetMatchId() => id;
        public EmptyScoresException(int matchId)
        {
            id = matchId;
        }

        public EmptyScoresException(string message, int matchId) : base(message)
        {
            id = matchId;
        }
    }
    public class SemifinalsNotGeneratedException : Exception
    {
        public SemifinalsNotGeneratedException()
        {
        }
        public SemifinalsNotGeneratedException(string message) : base(message)
        {
        }
    }
    class ImpossibleToCreateSemifinals : Exception
    {
        public ImpossibleToCreateSemifinals()
        {
        }
        public ImpossibleToCreateSemifinals(string message) : base(message)
        {
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace Projekti
{
    [Serializable()]
    public class ScoreExistsAlreadyException : Exception
    {
        public ScoreExistsAlreadyException() : base(String.Format("Score exists already")) { }
        public ScoreExistsAlreadyException(string message) : base(String.Format(message)) { }
        public ScoreExistsAlreadyException(string message, Exception inner) : base(String.Format("Score exists already ", message, inner)) { }

        protected ScoreExistsAlreadyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable()]
    public class PlayerNotFoundException : Exception
    {
        public PlayerNotFoundException() : base(String.Format("Player not found.")) { }
        public PlayerNotFoundException(string message) : base(String.Format(message)) { }
        public PlayerNotFoundException(string message, Exception inner) : base(String.Format("Player not found :", message, inner)) { }

        protected PlayerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable()]
    public class GameNotFoundException : Exception
    {
        public GameNotFoundException() : base(String.Format("Game not found")) { }
        public GameNotFoundException(string message) : base(String.Format(message)) { }
        public GameNotFoundException(string message, Exception inner) : base(String.Format("Game not found: ", message, inner)) { }

        protected GameNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable()]
    public class GameNotFinishedException : Exception
    {
        public GameNotFinishedException() : base(String.Format("Game not finished")) { }
        public GameNotFinishedException(string message) : base(String.Format(message)) { }
        public GameNotFinishedException(string message, Exception inner) : base(String.Format("Game not finished", message, inner)) { }

        protected GameNotFinishedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable()]
    public class WrongValueException : Exception
    {
        public WrongValueException() : base(String.Format("Wrong value")) { }
        public WrongValueException(string message) : base(String.Format(message)) { }
        public WrongValueException(string message, Exception inner) : base(String.Format("Wrong value", message, inner)) { }

        protected WrongValueException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

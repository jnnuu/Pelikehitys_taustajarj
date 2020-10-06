using System;
using System.Runtime.Serialization;

namespace ExceptionHandler
{

    [Serializable()]
    public class ScoreOutOfBoundsException : System.Exception
    {
        public ScoreOutOfBoundsException() : base(String.Format("Score out of bounds")) { }
        public ScoreOutOfBoundsException(string message) : base(String.Format("Score out of bounds: ", message)) { }
        public ScoreOutOfBoundsException(string message, System.Exception inner) : base(String.Format("Score out of bounds: ", message, inner)) { }

        protected ScoreOutOfBoundsException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable()]
    public class GameNotFoundException : System.Exception
    {
        public GameNotFoundException() : base(String.Format("Game not found")) { }
        public GameNotFoundException(string message) : base(String.Format("Game not found: ", message)) { }
        public GameNotFoundException(string message, System.Exception inner) : base(String.Format("Game not found: ", message, inner)) { }

        protected GameNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable()]
    public class GameNotFinishedException : System.Exception
    {
        public GameNotFinishedException() : base(String.Format("Game not finished")) { }
        public GameNotFinishedException(string message) : base(String.Format("Game not finished", message)) { }
        public GameNotFinishedException(string message, System.Exception inner) : base(String.Format("Game not finished", message, inner)) { }

        protected GameNotFinishedException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable()]
    public class WrongValueException : System.Exception
    {
        public WrongValueException() : base(String.Format("Wrong value")) { }
        public WrongValueException(string message) : base(String.Format("Wrong value", message)) { }
        public WrongValueException(string message, System.Exception inner) : base(String.Format("Wrong value", message, inner)) { }

        protected WrongValueException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
using System;
using System.Runtime.Serialization;

[Serializable()]
public class ScoreOutOfBoundsException : Exception
{
    public ScoreOutOfBoundsException() : base(String.Format("Score out of bounds")) { }
    public ScoreOutOfBoundsException(string message) : base(String.Format("Score out of bounds: ", message)) { }
    public ScoreOutOfBoundsException(string message, Exception inner) : base(String.Format("Score out of bounds: ", message, inner)) { }

    protected ScoreOutOfBoundsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

[Serializable()]
public class PlayerNotFoundException : Exception
{
    public PlayerNotFoundException() : base(String.Format("Player not found.")) { }
    public PlayerNotFoundException(string message) : base(String.Format("Player not found :", message)) { }
    public PlayerNotFoundException(string message, Exception inner) : base(String.Format("Player not found :", message, inner)) { }

    protected PlayerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

[Serializable()]
public class GameNotFoundException : Exception
{
    public GameNotFoundException() : base(String.Format("Game not found")) { }
    public GameNotFoundException(string message) : base(String.Format("Game not found: ", message)) { }
    public GameNotFoundException(string message, Exception inner) : base(String.Format("Game not found: ", message, inner)) { }

    protected GameNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

[Serializable()]
public class GameNotFinishedException : Exception
{
    public GameNotFinishedException() : base(String.Format("Game not finished")) { }
    public GameNotFinishedException(string message) : base(String.Format("Game not finished", message)) { }
    public GameNotFinishedException(string message, Exception inner) : base(String.Format("Game not finished", message, inner)) { }

    protected GameNotFinishedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

[Serializable()]
public class WrongValueException : Exception
{
    public WrongValueException() : base(String.Format("Wrong value")) { }
    public WrongValueException(string message) : base(String.Format("Wrong value", message)) { }
    public WrongValueException(string message, Exception inner) : base(String.Format("Wrong value", message, inner)) { }

    protected WrongValueException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
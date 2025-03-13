public enum SampleState
{
    Default,
}

public enum LevelState
{
    Pending,
    Playing,
    Ending
}

public enum FoodSize
{
    Normal = 0,
    Big
}

public enum GameEvent
{
    SnakeMove,
    SnakeDead,
    SnakeTurnLeft,
    SnakeTurnRight,
}

public enum Difficulty
{
    Default,
    Easy,
    Medium,
    Hard
}

public enum GameStatus
{
    Ok,
    SnakeEat,
    SnakeDead,
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public enum ReplayState
{
    Playing,
    Pausing
}
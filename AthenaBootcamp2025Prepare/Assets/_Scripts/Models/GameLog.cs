using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameLog : ICloneable<GameLog>
{
    [SerializeField] List<Turn> gameTurns = new List<Turn>();

    public List<Turn> GameTurns { get => gameTurns; }

    public void Add(Turn newTurn)
    {
        gameTurns.Add(newTurn);
    }

    public void Reset()
    {
        gameTurns = new List<Turn>();
    }
    public override string ToString()
    {
        return $"{{{nameof(GameTurns)}={GameTurns}}}";
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameLog : ICloneable<GameLog>
{
    [SerializeField] List<Turn> gameTurns;
}

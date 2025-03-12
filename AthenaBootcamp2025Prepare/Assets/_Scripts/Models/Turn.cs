using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Turn : ICloneable<Turn>
{
    [SerializeField] List<GameEvent> gameEvents = new();
    [SerializeField] int[][] matrix;
    [SerializeField] List<Coordinate> snakeCoords = new(); //index at 0 is Head, at last is Tail
}

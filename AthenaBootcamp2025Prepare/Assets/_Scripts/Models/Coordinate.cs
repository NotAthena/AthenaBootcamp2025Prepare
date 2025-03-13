using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Coordinate : ICloneable<Coordinate>
{
    [SerializeField] int x;
    [SerializeField] int y;

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

    public void SetValue(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(Coordinate)) return false;
        var tmp = obj as Coordinate;
        if (X == tmp.X && Y == tmp.Y) return true;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"({x}, {y})";
    }
}

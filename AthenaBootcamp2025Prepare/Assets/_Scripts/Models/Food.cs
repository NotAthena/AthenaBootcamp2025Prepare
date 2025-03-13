﻿using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Food : ICloneable<Food>
{
    [SerializeField] FoodSize size;
    [SerializeField] int score;
    [SerializeField] Coordinate coordinate;

    public FoodSize Size { get => size; set => size = value; }
    public int Score { get => score; set => score = value; }
    public Coordinate Coordinate { get => coordinate; set => coordinate = value; }

    public override string ToString()
    {
        return $"{{{nameof(Size)}={Size.ToString()}, " +
            $"\n{nameof(Score)}={Score.ToString()}, " +
            $"\n{nameof(Coordinate)}={Coordinate}}}";
    }
}
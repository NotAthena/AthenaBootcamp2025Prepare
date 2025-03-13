using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class Level : ICloneable<Level>
{
    [SerializeField] Difficulty difficulty;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] List<Coordinate> snakeStartCoords;
    [SerializeField] Direction snakeStartDirection;
    [Description("Turn per second. \n Ex: if snakeSpeed equal 2, delta time between every turn is 0.5 second")]
    [SerializeField] float snakeSpeed;
    [SerializeField] List<Coordinate> wallCoords;
    [SerializeField] List<FoodSO> foodSOs;

    public Difficulty Difficulty { get => difficulty; set => difficulty = value; }
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public List<Coordinate> SnakeStartCoords { get => snakeStartCoords; set => snakeStartCoords = value; }
    public Direction SnakeStartDirection { get => snakeStartDirection; set => snakeStartDirection = value; }
    public float SnakeSpeed { get => snakeSpeed; set => snakeSpeed = value; }
    public List<Coordinate> WallCoords { get => wallCoords; set => wallCoords = value; }
    public List<FoodSO> FoodSOs { get => foodSOs; set => foodSOs = value; }

    //public Food GetFood(int index)
    //{
    //    if (index >= foodSOs.Count) index = foodSOs.Count - 1;
    //    return ((ICloneable<Food>)foodSOs[index].Food).CloneSelf();
    //}

    //public Food GetRandomFood()
    //{
    //    var random = Random.Range(0, foodSOs.Count);
    //    return ((ICloneable<Food>)foodSOs[random].Food).CloneSelf();

    //}

    public bool VerifyLevel()
    {
        bool result = true;
        if (snakeStartCoords == null || snakeStartCoords.Count == 0) return false;

        if (ListHelper.IsContainDuplicatedElement(snakeStartCoords)) return false;

        if (ListHelper.IsContainDuplicatedElement(wallCoords)) return false;

        return result;
    }
    public override string ToString()
    {
        return $"{{{nameof(Difficulty)}={Difficulty}, " +
            $"\n{nameof(Height)}={Height}, " +
            $"\n{nameof(Width)}={Width.ToString()}, " +
            $"\n{nameof(SnakeStartCoords)}={SnakeStartCoords}, " +
            $"\n{nameof(SnakeSpeed)}={SnakeSpeed.ToString()}, " +
            $"\n{nameof(WallCoords)}={WallCoords}}}";
    }
}

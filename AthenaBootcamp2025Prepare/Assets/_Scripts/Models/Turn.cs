using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Turn : ICloneable<Turn>
{
    [SerializeField] int turnIndex = 0;
    [SerializeField] Level levelInfo;
    [SerializeField] List<GameEvent> gameEvents = new();
    [SerializeField] int[,] matrix;
    [SerializeField] List<Coordinate> snakeCoords = new(); //index at 0 is Head, at last is Tail
    [SerializeField] Direction snakeDirection;
    [SerializeField] Food food;
    [SerializeField] int foodEatenCounter = 1;
    [SerializeField] int score;


    public int TurnIndex { get => turnIndex; set => turnIndex = value; }
    public Level LevelInfo { get => levelInfo; set => levelInfo = value; }
    public List<GameEvent> GameEvents { get => gameEvents; set => gameEvents = value; }
    public int[,] Matrix { get => GenerateMatrix(); set => matrix = value; }
    public List<Coordinate> SnakeCoords { get => snakeCoords; set => snakeCoords = value; }
    public Direction SnakeDirection { get => snakeDirection; set => snakeDirection = value; }
    public Food Food { get => food; set => food = value; }
    public string MatrixToString { get => GetMatrixString(); }
    public int Score { get => score; set => score = value; }
    public int FoodEatenCounter { get => foodEatenCounter; }

    public static readonly int snakeHeadSign = 1;
    public static readonly int snakeBodySign = 2;
    public static readonly int wallSign = 3;
    public static readonly int foodSign = 4;

    private int[,] GenerateMatrix()
    {
        ResetMatrix();

        foreach (var wallCoord in levelInfo.WallCoords)
        {
            matrix[wallCoord.X, wallCoord.Y] = wallSign;
        }
        
        if (snakeCoords.Count > 0)
        {
            for (int i = 1; i < snakeCoords.Count; i++)
            {
                matrix[snakeCoords[i].X, snakeCoords[i].Y] = snakeBodySign;
            }
            matrix[snakeCoords[0].X, snakeCoords[0].Y] = snakeHeadSign;
        }


        if (Food != null) matrix[Food.Coordinate.X, Food.Coordinate.Y] = foodSign;
        return matrix;
    }

    private void ResetMatrix()
    {
        if (matrix == null)
        {
            matrix = ArrayHelper.Create2DArray(levelInfo.Height, levelInfo.Width);
        }
        else
        {
            for (int i = 0; i < matrix.GetLength(0); i++) // Rows
            {
                for (int j = 0; j < matrix.GetLength(1); j++) // Columns
                {
                    matrix[i, j] = 0;
                }
            }
        }
    }

    private string GetMatrixString()
    {
        GenerateMatrix();
        StringBuilder sb = new StringBuilder();
        sb.Append("\n");
        for (int i = 0; i < matrix.GetLength(0); i++) // Rows
        {
            for (int j = 0; j < matrix.GetLength(1); j++) // Columns
            {
                sb.Append(matrix[i, j] + " ");
            }
            sb.AppendLine(); // New line after each row
        }
        return sb.ToString();
    }
    public GameStatus VerifyStatus()
    {
        GameStatus status = GameStatus.Ok;
        if (ListHelper.IsContainDuplicatedElement(snakeCoords))
        {
            status = GameStatus.SnakeDead;
        }
        if (ListHelper.IsHaveCommonElements(snakeCoords, levelInfo.WallCoords))
        {
            status = GameStatus.SnakeDead;
        }
        if (SnakeCoords[0].Equals(food.Coordinate))
        {
            status = GameStatus.SnakeEat;
        }
        return status;
    }

    public Food GenerateFood()
    {
        GenerateMatrix();
        if (levelInfo.FoodSOs.Count == 0) return null;

        List<Coordinate> availableCoords = new();
        for (int i = 0; i < matrix.GetLength(0); i++) // Rows
        {
            for (int j = 0; j < matrix.GetLength(1); j++) // Columns
            {
                if (matrix[i, j] == 0)
                {
                    availableCoords.Add(new(i, j));
                }
            }
        }
        int foodCoord = Random.Range(0, availableCoords.Count);

        if (FoodEatenCounter > 0 && FoodEatenCounter % 5 == 0)
        {
            Food = ((ICloneable<Food>)levelInfo.FoodSOs[1].Food).CloneSelf();
        }
        else
        {
            Food = ((ICloneable<Food>)levelInfo.FoodSOs[0].Food).CloneSelf();
        }
        Food.Coordinate = availableCoords[foodCoord];

        GenerateMatrix();
        return Food;
    }

    public void AddScore(int addedScore)
    {
        score += addedScore;
        foodEatenCounter++;
    }

    public override string ToString()
    {
        return $"{{{nameof(LevelInfo)}={LevelInfo}, " +
            $"\n{nameof(GameEvents)}={GameEvents}, " +
            $"\n{nameof(Matrix)}={Matrix}, " +
            $"\n{nameof(SnakeCoords)}={SnakeCoords}, " +
            $"\n{nameof(SnakeDirection)}={SnakeDirection.ToString()}, " +
            $"\n{nameof(Food)}={Food}, " +
            $"\n{nameof(MatrixToString)}={MatrixToString}}}";
    }
}

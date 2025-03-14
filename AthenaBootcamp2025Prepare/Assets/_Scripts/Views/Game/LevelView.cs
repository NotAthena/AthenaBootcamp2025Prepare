using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] GameObject cellPrefab;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject snakeHeadPrefab;
    [SerializeField] GameObject snakeBodyPrefab;
    [SerializeField] GameObject snakeTailPrefab;
    [SerializeField] List<GameObject> foodPrefabs;
    [SerializeField] List<GameObject> bigFoodPrefabs;

    [Header("Runtime Parameters")]
    [SerializeField] List<GameObject> snakeGameObject = new();
    [SerializeField] GameObject[,] gridHandler;
    [SerializeField] float cellEdgeLength = 1;
    [SerializeField] Turn previousTurn;
    [SerializeField] Turn currentTurn;
    [SerializeField] List<GameObject> currentSnake = new();
    [SerializeField] List<Vector3> targetSnake = new();
    [SerializeField] List<GameObject> walls = new();
    [SerializeField] GameObject currentFood;

    private Food food;
    private void Awake()
    {
        ObserverHelper.RegisterListener(ObserverConstants.START_PLAYING,
            (param) =>
            {
                //Debug.Log("TurnIndex: " + ((Turn)param[0]).TurnIndex);
                GenerateGrid(((Turn)param[0]).Matrix);
                GenerateWall(((Turn)param[0]).LevelInfo.WallCoords);
                GenerateSnake(((Turn)param[0]).SnakeCoords);
                //GenerateFood(((Turn)param[0]).Food);
                previousTurn = (Turn)param[0];
                currentTurn = (Turn)param[0];
            }
        );

        ObserverHelper.RegisterListener(ObserverConstants.SNAKE_MOVE,
            (param) =>
            {
                MoveSnake(((Turn)param[0]).SnakeCoords);
                previousTurn = currentTurn;
                currentTurn = (Turn)param[0];
                //GenerateFood(((Turn)param[0]).Food);
            });

    }

    public void GenerateGrid(int[,] matrix)
    {
        //Convert to 2D array form
        gridHandler = new GameObject[matrix.GetLength(1), matrix.GetLength(0)];
        for (int i = 0; i < matrix.GetLength(0); i++) // Rows
        {
            for (int j = 0; j < matrix.GetLength(1); j++) // Columns
            {
                gridHandler[j, i] = PoolingHelper.SpawnObject(cellPrefab, transform, new Vector3(cellEdgeLength * j, cellEdgeLength * -i), Quaternion.identity);
                gridHandler[j, i].name = $"{cellPrefab.name}_{j}_{i}";
            }
        }
    }

    public void GenerateWall(List<Coordinate> wallCoords)
    {
        for (int i = 0; i < wallCoords.Count; i++)
        {
            Vector3 spawnPosition = gridHandler[wallCoords[i].Y, wallCoords[i].X].transform.localPosition;
            walls.Add(
                    PoolingHelper.SpawnObject(wallPrefab, transform, spawnPosition, Quaternion.identity)
                );
        }
    }

    public void GenerateFood(Food food)
    {
        if (this.food.Equals(food)) return;

        PoolingHelper.ReturnObjectToPool(currentFood);
        Vector3 spawnPosition = gridHandler[food.Coordinate.Y, food.Coordinate.X].transform.localPosition;
        int random = Random.Range(0, foodPrefabs.Count);
        currentFood = PoolingHelper.SpawnObject(foodPrefabs[random], transform, spawnPosition, Quaternion.identity);
        this.food = food;

    }

    public void GenerateSnake(List<Coordinate> snakeCoords)
    {
        currentSnake = new();
        for (int i = 0; i < snakeCoords.Count; i++)
        {
            Vector3 spawnPosition = gridHandler[snakeCoords[i].Y, snakeCoords[i].X].transform.localPosition;
            Debug.Log(snakeCoords[i] + " | " + spawnPosition);
            if (i == 0)
            {
                currentSnake.Add(
                    PoolingHelper.SpawnObject(snakeHeadPrefab, transform, spawnPosition, Quaternion.identity)
                    );
            }
            else if (i > 1 && i < snakeCoords.Count - 1)
            {
                currentSnake.Add(
                    PoolingHelper.SpawnObject(snakeBodyPrefab, transform, spawnPosition, Quaternion.identity)
                    );
            }
            else if (i == snakeCoords.Count - 1)
            {
                currentSnake.Add(
                    PoolingHelper.SpawnObject(snakeTailPrefab, transform, spawnPosition, Quaternion.identity)
                    );
            }
        }
    }

    private void MoveSnake(List<Coordinate> incomingSnakeCoords)
    {
        PlaySnakeAnimationHeadBody();


        Debug.Log("1) " + currentSnake.Count + " | " + incomingSnakeCoords.Count);
        targetSnake = new();
        if (currentSnake.Count == incomingSnakeCoords.Count)
        {
            for (int i = 0; i < currentSnake.Count; i++)
            {
                targetSnake.Add(gridHandler[incomingSnakeCoords[i].Y, incomingSnakeCoords[i].X].transform.localPosition);
                currentSnake[i].transform.localPosition = targetSnake[i];
                //currentSnake[incomingSnakeCoords[i]].transform.localPosition 
                //    = gridHandler[incomingSnakeCoords[i].Y, incomingSnakeCoords[i].X].transform.localPosition;
            }
        }
        else
        {
            for (int i = 0; i < incomingSnakeCoords.Count; i++)
            {
                targetSnake.Add(gridHandler[incomingSnakeCoords[i].Y, incomingSnakeCoords[i].X].transform.localPosition);
                Vector3 spawnPosition = gridHandler[incomingSnakeCoords[i].Y, incomingSnakeCoords[i].X].transform.localPosition;
                if (i == incomingSnakeCoords.Count - 2)
                {
                    currentSnake.Insert(currentSnake.Count - 1,
                        PoolingHelper.SpawnObject(snakeBodyPrefab, transform, spawnPosition, Quaternion.identity)
                    );
                    Debug.Log("2) " + currentSnake.Count + " | " + incomingSnakeCoords.Count);
                }
                else
                {
                    currentSnake[i].transform.localPosition = targetSnake[i];
                }
            }
        }

        PlaySnakeAnimationTail();
    }

    private void PlaySnakeAnimationHeadBody()
    {
        for (int i = 0; i < currentSnake.Count; i++)
        {
            var animator = currentSnake[i].GetComponent<Animator>();
            if (i == 0) //Head
            {
                switch (currentTurn.SnakeDirection)
                {
                    case Direction.Up:
                        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != AnimationConstants.HeadUp)
                        {
                            animator.Play(AnimationConstants.HeadUp);
                        }
                        break;
                    case Direction.Down:
                        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != AnimationConstants.HeadDown)
                        {
                            animator.Play(AnimationConstants.HeadDown);
                        }
                        break;
                    case Direction.Left:
                        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != AnimationConstants.HeadLeft)
                        {
                            animator.Play(AnimationConstants.HeadLeft);
                        }
                        break;
                    case Direction.Right:
                        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != AnimationConstants.HeadRight)
                        {
                            animator.Play(AnimationConstants.HeadRight);
                        }
                        break;
                }
            }

            else if (i > 0 && i < currentSnake.Count - 1) //Body
            {
                if (i % 2 == 0)
                {
                    if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != AnimationConstants.Body_1)
                    {
                        animator.Play(AnimationConstants.Body_1);
                    }
                }
                else
                {
                    if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != AnimationConstants.Body_2)
                    {
                        animator.Play(AnimationConstants.Body_2);
                    }
                }
            }
            currentSnake[i].GetComponent<SpriteRenderer>().sortingOrder
                = (int)(currentTurn.SnakeCoords[i].X);

        }

    }

    private void PlaySnakeAnimationTail()
    {
        for (int i = 0; i < currentSnake.Count; i++)
        {
            var animator = currentSnake[i].GetComponent<Animator>();
            if (i == currentSnake.Count - 1) //Tail
            {
                if (currentSnake[currentSnake.Count - 2].transform.localPosition.x == currentSnake[currentSnake.Count - 1].transform.localPosition.x)
                {
                    if (currentSnake[currentSnake.Count - 2].transform.localPosition.y < currentSnake[currentSnake.Count - 1].transform.localPosition.y)
                    {
                        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != AnimationConstants.TailDown)
                        {
                            animator.Play(AnimationConstants.TailDown);
                        }
                    }
                    else
                    {
                        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != AnimationConstants.TailUp)
                        {
                            animator.Play(AnimationConstants.TailUp);
                        }
                    }
                }
                else if (currentSnake[currentSnake.Count - 2].transform.localPosition.y == currentSnake[currentSnake.Count - 1].transform.localPosition.y)
                {
                    if (currentSnake[currentSnake.Count - 2].transform.localPosition.x > currentSnake[currentSnake.Count - 1].transform.localPosition.x)
                    {
                        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != AnimationConstants.TailRight)
                        {
                            animator.Play(AnimationConstants.TailRight);
                        }
                    }
                    else
                    {
                        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != AnimationConstants.TailLeft)
                        {
                            animator.Play(AnimationConstants.TailLeft);
                        }
                    }
                }
            }
        }

    }

}


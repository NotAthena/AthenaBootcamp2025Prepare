using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private LevelSO levelSO;

    [Header("Game Object assign")]
    [SerializeField] private TMP_Text matrixView;
    [SerializeField] private TMP_Text scoreView;

    [Header("Runtime Parameter")]
    [SerializeField] private Level level;
    [SerializeField] Turn currentTurn;
    [SerializeField] GameLog gameLog;
    [SerializeField] LevelState currentState;
    [SerializeField] bool isAbleToInput = true;

    private List<Coroutine> waitAndDoTurnCoroutines = new();

    private void OnEnable()
    {
        level = ((ICloneable<Level>)levelSO.Level).CloneSelf();
        GenerateMap();
        matrixView.SetText(currentTurn.MatrixToString);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        UpdateOnState();
    }

    private void GenerateMap()
    {
        currentTurn = new()
        {
            LevelInfo = level,
            Matrix = ArrayHelper.Create2DArray(level.Height, level.Width),
            SnakeCoords = level.SnakeStartCoords,
            SnakeDirection = level.SnakeStartDirection,
        };
        currentTurn.GenerateFood();
        gameLog.Reset();
        gameLog.GameTurns.Add(currentTurn);
    }

    IEnumerator WaitAndDoTurn()
    {
        yield return new WaitForSeconds(1f / level.SnakeSpeed);
        MoveSnake_Playing();
        gameLog.Add(currentTurn);
        currentTurn.TurnIndex = gameLog.GameTurns.Count;
        currentTurn = ((ICloneable<Turn>)currentTurn).CloneSelf();
        waitAndDoTurnCoroutines.Add(StartCoroutine(WaitAndDoTurn()));

        // View Controller Here
        matrixView.SetText(currentTurn.MatrixToString);
        scoreView.SetText(currentTurn.Score.ToString());
        // ====================
    }



    private void ChangeSnakeDirection(Direction incomingDirection)
    {
        if (incomingDirection == currentTurn.SnakeDirection) return;
        switch (currentTurn.SnakeDirection)
        {
            case Direction.Up:
                if (incomingDirection == Direction.Down) return;
                break;
            case Direction.Down:
                if (incomingDirection == Direction.Up) return;
                break;
            case Direction.Left:
                if (incomingDirection == Direction.Right) return;
                break;
            case Direction.Right:
                if (incomingDirection == Direction.Left) return;
                break;
        }

        currentTurn.SnakeDirection = incomingDirection;
    }

    #region State Machine
    public void SwitchToState(LevelState incomingState)
    {
        if (incomingState == currentState) return;
        switch (currentState)
        {
            case LevelState.Pending:
                ExitState_Pending();
                break;
            case LevelState.Playing:
                ExitState_Playing();
                break;
            case LevelState.Ending:
                ExitState_Ending();
                break;
        }

        currentState = incomingState;
        Debug.Log(currentState);

        switch (incomingState)
        {
            case LevelState.Pending:
                EnterState_Pending();
                break;
            case LevelState.Playing:
                EnterState_Playing();
                break;
            case LevelState.Ending:
                EnterState_Ending();
                break;
        }
    }

    protected void UpdateOnState()
    {
        switch (currentState)
        {
            case LevelState.Pending:
                UpdateState_Pending();
                break;
            case LevelState.Playing:
                UpdateState_Playing();
                break;
            case LevelState.Ending:
                UpdateState_Ending();
                break;
        }
    }

    #region State Pending
    protected virtual void EnterState_Pending()
    {

    }
    protected virtual void UpdateState_Pending()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            SwitchToState(LevelState.Playing);
        }
    }
    protected virtual void ExitState_Pending()
    {

    }
    #endregion

    #region State Playing
    protected virtual void EnterState_Playing()
    {
        waitAndDoTurnCoroutines.Add(StartCoroutine(WaitAndDoTurn()));
        isAbleToInput = true;
    }
    protected virtual void UpdateState_Playing()
    {
        InputCheck_Playing();
        CheckEndGame_Playing();
    }
    protected virtual void ExitState_Playing()
    {
        foreach (var coroutine in waitAndDoTurnCoroutines)
        {
            StopCoroutine(coroutine);
        }
        waitAndDoTurnCoroutines.Clear();
    }

    protected void InputCheck_Playing()
    {
        if (!isAbleToInput) return;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            ChangeSnakeDirection(Direction.Up);
            StartCoroutine(WaitInputCooldown());
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            ChangeSnakeDirection(Direction.Down);
            StartCoroutine(WaitInputCooldown());
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            ChangeSnakeDirection(Direction.Left);
            StartCoroutine(WaitInputCooldown());
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            ChangeSnakeDirection(Direction.Right);
            StartCoroutine(WaitInputCooldown());
        }
    }

    protected IEnumerator WaitInputCooldown()
    {
        isAbleToInput = false;
        yield return new WaitForSeconds(1f / level.SnakeSpeed);
        isAbleToInput = true;
    }
    protected void CheckEndGame_Playing()
    {
        if (currentTurn.VerifyStatus() == GameStatus.SnakeDead)
        {
            SwitchToState(LevelState.Ending);
        }
    }
    private void MoveSnake_Playing()
    {
        int x = 0, y = 0;
        switch (currentTurn.VerifyStatus())
        {
            case GameStatus.Ok:
                for (int i = currentTurn.SnakeCoords.Count - 1; i >= 1; i--)
                {
                    currentTurn.SnakeCoords[i] = currentTurn.SnakeCoords[i - 1];
                }

                switch (currentTurn.SnakeDirection)
                {
                    case Direction.Up:
                        x = currentTurn.SnakeCoords[0].X - 1;
                        y = currentTurn.SnakeCoords[0].Y;
                        if (x < 0) x = level.Height - 1;
                        break;
                    case Direction.Down:
                        x = currentTurn.SnakeCoords[0].X + 1;
                        y = currentTurn.SnakeCoords[0].Y;
                        if (x >= level.Height) x = 0;
                        break;
                    case Direction.Left:
                        x = currentTurn.SnakeCoords[0].X;
                        y = currentTurn.SnakeCoords[0].Y - 1;
                        if (y < 0) y = level.Width - 1;
                        break;
                    case Direction.Right:
                        x = currentTurn.SnakeCoords[0].X;
                        y = currentTurn.SnakeCoords[0].Y + 1;
                        if (y >= level.Width) y = 0;
                        break;
                }
                currentTurn.SnakeCoords[0] = new Coordinate(x, y);
                break;
            case GameStatus.SnakeEat:
                Coordinate tail = currentTurn.SnakeCoords[currentTurn.SnakeCoords.Count - 1];
                for (int i = currentTurn.SnakeCoords.Count - 1; i >= 1; i--)
                {
                    currentTurn.SnakeCoords[i] = currentTurn.SnakeCoords[i - 1];
                }
                currentTurn.SnakeCoords.Add(tail);
                switch (currentTurn.SnakeDirection)
                {
                    case Direction.Up:
                        x = currentTurn.SnakeCoords[0].X - 1;
                        y = currentTurn.SnakeCoords[0].Y;
                        if (x < 0) x = level.Height - 1;
                        break;
                    case Direction.Down:
                        x = currentTurn.SnakeCoords[0].X + 1;
                        y = currentTurn.SnakeCoords[0].Y;
                        if (x >= level.Height) x = 0;
                        break;
                    case Direction.Left:
                        x = currentTurn.SnakeCoords[0].X;
                        y = currentTurn.SnakeCoords[0].Y - 1;
                        if (y < 0) y = level.Width - 1;
                        break;
                    case Direction.Right:
                        x = currentTurn.SnakeCoords[0].X;
                        y = currentTurn.SnakeCoords[0].Y + 1;
                        if (y >= level.Width) y = 0;
                        break;
                }
                currentTurn.SnakeCoords[0] = new Coordinate(x, y);
                currentTurn.AddScore(currentTurn.Food.Score);
                Debug.Log(currentTurn.TurnIndex + " | " + currentTurn.FoodEatenCounter + " | " + currentTurn.Food.Size + " | " + currentTurn.Food.Score);
                currentTurn.GenerateFood();
                break;
            case GameStatus.SnakeDead:
                break;
        }
        //Debug.Log(currentTurn.MatrixToString);
    }
    #endregion

    #region State Ending
    protected virtual void EnterState_Ending()
    {

    }
    protected virtual void UpdateState_Ending()
    {

    }
    protected virtual void ExitState_Ending()
    {

    }
    #endregion

    #endregion
}
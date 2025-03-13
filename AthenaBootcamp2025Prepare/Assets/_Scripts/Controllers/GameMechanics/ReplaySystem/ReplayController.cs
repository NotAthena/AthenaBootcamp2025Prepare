using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReplayController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] string filePath; //TO DO: Add Read from list file in Application.persistentDataPath


    [Header("Game Object assign")]
    [SerializeField] private TMP_Text matrixView;
    [SerializeField] private TMP_Text scoreView;

    [Header("Runtime Parameter")]
    [SerializeField] GameLog gameLog;
    [SerializeField] int currentTurnIndex;
    [SerializeField] private ReplayState currentState;

    private List<Coroutine> waitAndDoTurnCoroutines = new();

    private void OnEnable()
    {
        gameLog = JsonHelper.ReadData<GameLog>(filePath);
    }
    private void Update()
    {
        UpdateOnState();
    }
    public void Play()
    {
        StartCoroutine(WaitAndDoTurn());
    }

    public void Restart()
    {
        Pause();
        currentTurnIndex = 0;
    }

    public void Pause()
    {
        foreach (var coroutine in waitAndDoTurnCoroutines)
        {
            StopCoroutine(coroutine);
        }
        waitAndDoTurnCoroutines.Clear();
    }

    IEnumerator WaitAndDoTurn()
    {
        if (currentTurnIndex < gameLog.GameTurns.Count)
        {
            yield return new WaitForSeconds(1f / gameLog.GameTurns[currentTurnIndex].LevelInfo.SnakeSpeed);
            waitAndDoTurnCoroutines.Add(StartCoroutine(WaitAndDoTurn()));

            // View Controller Here
            matrixView.SetText(gameLog.GameTurns[currentTurnIndex].MatrixToString);
            scoreView.SetText(gameLog.GameTurns[currentTurnIndex].Score.ToString());
            // ====================

            currentTurnIndex++;
        }
    }

    #region State Machine
    public void SwitchToState(ReplayState incomingState)
    {
        if (incomingState == currentState) return;
        switch (currentState)
        {
            case ReplayState.Playing:
                ExitState_Playing();
                break;
            case ReplayState.Pausing:
                ExitState_Pausing();
                break;
        }

        currentState = incomingState;

        switch (incomingState)
        {
            case ReplayState.Playing:
                EnterState_Playing();
                break;
            case ReplayState.Pausing:
                EnterState_Pausing();
                break;
        }
    }

    protected void UpdateOnState()
    {
        switch (currentState)
        {
            case ReplayState.Playing:
                UpdateState_Playing();
                break;
            case ReplayState.Pausing:
                UpdateState_Pausing();
                break;
        }
    }

    #region State Default
    protected virtual void EnterState_Playing()
    {

    }
    protected virtual void UpdateState_Playing()
    {

    }
    protected virtual void ExitState_Playing()
    {

    }
    #endregion

    #region State Default
    protected virtual void EnterState_Pausing()
    {

    }
    protected virtual void UpdateState_Pausing()
    {

    }
    protected virtual void ExitState_Pausing()
    {

    }
    #endregion

    #endregion

}

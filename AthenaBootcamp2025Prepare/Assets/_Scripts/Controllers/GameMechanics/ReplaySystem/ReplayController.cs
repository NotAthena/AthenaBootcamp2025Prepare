using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReplayController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] string fileName = string.Empty; //TO DO: Add Read from list file in Application.persistentDataPath


    [Header("Game Object assign")]
    [SerializeField] private TMP_Text matrixView;
    [SerializeField] private TMP_Text scoreView;
    [SerializeField] private TMP_Dropdown saveListDropdown;

    [Header("Runtime Parameter")]
    [SerializeField] GameLog gameLog;
    [SerializeField] int currentTurnIndex;
    [SerializeField] private ReplayState currentState;
    List<string> fileNames = new();

    private List<Coroutine> waitAndDoTurnCoroutines = new();

    private void Awake()
    {
        //saveListDropdown.onValueChanged.AddListener((x) => OnDropDownValueChange());
    }

    private void OnEnable()
    {
        fileNames = GameSaveManager.GetSaveNameList();
        //gameLog = JsonHelper.ReadData<GameLog>(filePath);
        saveListDropdown.ClearOptions();
        foreach (var fileName in fileNames)
        {
            saveListDropdown.options.Add(new() { text = fileName });
        }
        //StartCoroutine(WaitLoadFileList());
    }

    private void Update()
    {
        UpdateOnState();
    }
    public void Play()
    {
        if (string.IsNullOrEmpty(fileName))
        {
            fileName = saveListDropdown.options[saveListDropdown.value].text;
            gameLog = GameSaveManager.Load(fileName);
            Restart();
        }
        else if (fileName != saveListDropdown.options[saveListDropdown.value].text)
        {
            fileName = saveListDropdown.options[saveListDropdown.value].text;
            gameLog = GameSaveManager.Load(fileName);
            Restart();
        }
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
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        waitAndDoTurnCoroutines.Clear();
    }

    public void OnDropDownValueChange()
    {
        fileName = saveListDropdown.options[saveListDropdown.value].text;

    }

    IEnumerator WaitAndDoTurn()
    {
        if (currentTurnIndex < gameLog.GameTurns.Count)
        {
            yield return new WaitForSeconds(1f / gameLog.GameTurns[currentTurnIndex].LevelInfo.SnakeSpeed);

            // View Controller Here
            matrixView.SetText(gameLog.GameTurns[currentTurnIndex].MatrixToString);
            scoreView.SetText(gameLog.GameTurns[currentTurnIndex].Score.ToString());
            // ====================

            currentTurnIndex++;
            waitAndDoTurnCoroutines.Add(StartCoroutine(WaitAndDoTurn()));
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

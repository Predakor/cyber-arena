using Scripts.Player;
using System;
using Systems.Shared;
using UnityEngine;
using UnityEngine.Events;

public sealed class GameManager : Singleton<GameManager>
{
    [field: SerializeField] public GameState State { get; private set; }
    [field: SerializeField] public Player Player { get; private set; }


    #region events
    public UnityEvent<GameState, GameState> OnBeforeGameStateChanged; //<new state ,old state>
    public UnityEvent<GameState, GameState> OnGameStateChanged; //<new state ,old state>
    public UnityEvent OnGameStarted;
    public UnityEvent OnGamePaused;
    public UnityEvent OnGameUnPaused;
    public UnityEvent OnGameLost;
    public UnityEvent OnGameWon;
    public UnityEvent OnGameRestart;

    #endregion

    public void ChangeGameState(GameState newGameState)
    {
        if (State == newGameState)
        {
            return;
        }

        OnBeforeGameStateChanged?.Invoke(newGameState, State);

        switch (newGameState)
        {
            case GameState.started:
                OnGameStarted?.Invoke();
                HandleStart();
                break;

            case GameState.paused:
                OnGamePaused?.Invoke();
                HandlePause();
                break;

            case GameState.lost:
                OnGameLost?.Invoke();
                HandleGameOver();
                break;

            case GameState.won:
                OnGameWon?.Invoke();
                HandleGameWon();
                break;
        }

        State = newGameState;
        OnGameStateChanged?.Invoke(newGameState, State);
    }

    #region public methods
    public void WinGame() => ChangeGameState(GameState.won);
    public void LoseGame() => ChangeGameState(GameState.lost);
    public void PauseGame() => ChangeGameState(GameState.paused);
    public void StartGame() => ChangeGameState(GameState.started);
    public void RestartGame() => ChangeGameState(GameState.started);
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (Player == null)
        {
            Player = FindObjectOfType<Player>();
        }
        StartGame();
    }

    public void Init()
    {
        if (Player == null)
        {
            Player = FindObjectOfType<Player>();
        }
    }


    private void HandleStart()
    {
        //load other managers
        //load map
        //load enemies
        //spawn player
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    private void HandlePause()
    {
        Time.timeScale = 0;
        //show pause menu
        throw new NotImplementedException();

    }

    private void HandleRestart()
    {
        //reset player
        //reset level
        //reset score trackers
        throw new NotImplementedException();

    }

    private void HandleGameWon()
    {
        //show game victory screen

        throw new NotImplementedException();
    }

    private void HandleGameOver()
    {

        //show game lsot screen
        throw new NotImplementedException();
    }
}

public enum GameState
{
    paused,
    started,
    lost,
    won
}
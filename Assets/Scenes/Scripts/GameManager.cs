 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Home,
    Gameplay,
    Pause,
    Gameover
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private HomePanel homePanel;
    [SerializeField] private GameplayPanel gameplayPanel;
    [SerializeField] private GameoverPanel gameoverPanel;
    [SerializeField] private PausePanel pausePanel;

    private SpawnManager spawnManager;
    private AudioManager audioManager;
    private GameState gameState;
    private bool m_Win;
    private int score;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        spawnManager = FindObjectOfType<SpawnManager>();
        homePanel.gameObject.SetActive(false);
        gameplayPanel.gameObject.SetActive(false);
        gameoverPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        SetState(GameState.Home);
        
    }

    private void SetState(GameState state)
    {
        gameState = state;
        homePanel.gameObject.SetActive(gameState == GameState.Home);
        gameplayPanel.gameObject.SetActive(gameState == GameState.Gameplay);
        gameoverPanel.gameObject.SetActive(gameState == GameState.Gameover);
        pausePanel.gameObject.SetActive(gameState == GameState.Pause);

        if (gameState == GameState.Pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;


        if (gameState == GameState.Home)
            audioManager.PlayHomeMusic();
        else
            audioManager.PlayBattleMusic();
    }

    public bool isActive()
    {
        return gameState == GameState.Gameplay;
    }

    public void Play()
    {
        spawnManager.StartGame();
        SetState(GameState.Gameplay);
        score = 0;
        gameplayPanel.DisplayScore(score);
    }

    public void Pause()
    {
        SetState(GameState.Pause);
    }

    public void Home()
    {
        SetState(GameState.Home);
        spawnManager.Clear();
    }

    public void Continue()
    {
        SetState(GameState.Gameplay);
    }

    public void Gameover(bool win)
    {
        m_Win = win;
        SetState(GameState.Gameover);
        gameoverPanel.DisplayResult(m_Win);
        gameoverPanel.DisplayHighScore(score);
    }

    public void AddScore(int value)
    {
        score += value;
        gameplayPanel.DisplayScore(score);
        if(spawnManager.IsClear())
        {
            Gameover(true);
        }
    }

}

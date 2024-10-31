using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameplayPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private Image imgHpBar;

    private GameManager gameManager;
    private SpawnManager spawnManager;
    //private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        spawnManager = FindAnyObjectByType<SpawnManager>();

        if (spawnManager.Player != null)
        {
            spawnManager.Player.onHPChanged += OnHpChanged;
        }
    }

    void OnEnable()
    {
        if (spawnManager.Player != null)
        {
            spawnManager.Player.onHPChanged += OnHpChanged;
        }
    }

    void OnDisable()
    {

        if (spawnManager.Player != null)
        {
            spawnManager.Player.onHPChanged -= OnHpChanged;
        }

    }

    private void OnHpChanged(int curHp, int maxHp)
    {
        imgHpBar.fillAmount = Mathf.Clamp01(curHp * 1f / maxHp);
    }

    public void BtnPause_OnPressed()
    {
        gameManager.Pause();
    }

    public void DisplayScore(int score)
    {
        txtScore.text = "SCORE: " + score;
    }
}

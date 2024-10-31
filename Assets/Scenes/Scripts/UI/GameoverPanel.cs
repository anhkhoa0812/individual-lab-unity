using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameoverPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtResult;
    [SerializeField] private TextMeshProUGUI txtHighScore;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void BtnHome_OnPressed()
    {
        gameManager.Home();
    }

    public void DisplayHighScore(int score)
    {
        txtHighScore.text = "HIGH SCORE: " + score;
    }

    public void DisplayResult(bool isWin)
    {
        if (isWin)
            txtResult.text = "YOU WIN";
        else
            txtResult.text = "YOU LOSE";
    }
}

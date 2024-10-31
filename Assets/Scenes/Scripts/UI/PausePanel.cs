using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
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

    public void BtnContinue_OnPressed()
    {
        gameManager.Continue();
    }
}

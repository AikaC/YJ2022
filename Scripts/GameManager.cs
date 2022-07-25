using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Game on/off
    public bool GameOn = false;

    [Header("Canva")]
    public GameObject PauseScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        //pause game
        GameOn = false;
        // game physics
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GameStatus()
    {
        return GameOn;
    }

    // start game on play button
    public void GameStart()
    {
        GameOn = true;
        Time.timeScale = 1;
    }

    public void GamePause()
    {
        GameOn = false;
        Time.timeScale = 0;
        PauseScreen.SetActive(true);
    }

    //restart game after pause
    public void GamePlay()
    {
        GameOn = true;
        Time.timeScale = 1;
        PauseScreen.SetActive(false);
    }
}
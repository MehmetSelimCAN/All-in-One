using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private float timer;
    private static Text timerUI;

    private static Transform lives;
    private static int currentLifeCount;

    private static Transform gameAreas;
    private float changeGameTimer;

    private static bool gameOver;
    private static Transform gameOverScreen;
    private Button restartButton;

    private void Awake() {
        timer = 0f;
        timerUI = GameObject.Find("Canvas/timer").GetComponent<Text>();
        lives = GameObject.Find("Canvas/Lives").transform;
        currentLifeCount = lives.childCount;

        gameAreas = GameObject.Find("GameAreas").transform;
        changeGameTimer = 2f;

        int randomGameArea = UnityEngine.Random.Range(0, gameAreas.childCount);
        gameAreas.GetChild(randomGameArea).gameObject.SetActive(true);

        
        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.onClick.AddListener(() => {
            Restart();
        });

        gameOverScreen = GameObject.Find("GameOverScreen").transform;
        gameOverScreen.gameObject.SetActive(false);
        gameOver = false;
    }

    private void Update() {
        if (gameOver) {
            return;
        }

        #region game timer
        timer += Time.deltaTime;
        TimeSpan t = TimeSpan.FromSeconds(timer);

        //silly solution
        if (timer < 10) {
            timerUI.text = string.Format("{0:D1}s {1:D3}", t.Seconds, t.Milliseconds);
        }
        else if (timer < 60) {
            timerUI.text = string.Format("{0:D2}s {1:D3}", t.Seconds, t.Milliseconds);
        }
        else if (timer > 60 && timer < 600){
            timerUI.text = string.Format("{0:D1}m {1:D2}s {2:D3}",t.Minutes, t.Seconds, t.Milliseconds);
        }
        else {
            timerUI.text = string.Format("{0:D2}m {1:D2}s {2:D3}",t.Minutes, t.Seconds, t.Milliseconds);
        }
        #endregion

        changeGameTimer -= Time.deltaTime;

        if (changeGameTimer < 0) {
            ChangeGames();
            changeGameTimer = 8f;
        }
    }

    public static void ChangeGames() {
        int activeAreaCount = 0;
        List<int> allGameAreasIndex = new List<int>();
        for (int i = 0; i < gameAreas.childCount; i++) {
            allGameAreasIndex.Add(i);
        }

        List<int> activeGameAreasIndex = new List<int>();
        for (int i = 0; i < gameAreas.childCount; i++) {
            if (gameAreas.GetChild(i).gameObject.activeInHierarchy) {
                activeAreaCount++;
                activeGameAreasIndex.Add(i);
                allGameAreasIndex.Remove(i);
            }
        }

        if (activeAreaCount < 3) {
            int randomGameArea = UnityEngine.Random.Range(0, allGameAreasIndex.Count);
            gameAreas.GetChild(allGameAreasIndex[randomGameArea]).gameObject.SetActive(true);
        }
        else {
            int randomGameArea1 = UnityEngine.Random.Range(0, activeGameAreasIndex.Count);
            gameAreas.GetChild(activeGameAreasIndex[randomGameArea1]).gameObject.SetActive(false);

            int randomGameArea2 = UnityEngine.Random.Range(0, allGameAreasIndex.Count);
            gameAreas.GetChild(allGameAreasIndex[randomGameArea2]).gameObject.SetActive(true);

            activeGameAreasIndex.Add(randomGameArea2);
            allGameAreasIndex.Remove(randomGameArea1);

            
        }
    }

    public static void LoseLife() {
        if (currentLifeCount > 0) {
            currentLifeCount--;
        }

        if (currentLifeCount == 0) {
            GameOver();
        }

        lives.GetChild(currentLifeCount).Find("front").gameObject.SetActive(false);
    }

    public static void GameOver() {
        gameOver = true;
        gameOverScreen.gameObject.SetActive(true);
        gameOverScreen.Find("score").GetComponent<Text>().text = timerUI.text + "ms";
    }

    public static void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    private Button skipButton;
    private Button playButton;

    private Transform gameAreas;
    private Transform UISteps;

    private int stepNumber = 0;

    private void Awake() {
        skipButton = GameObject.Find("SkipButton").GetComponent<Button>();
        skipButton.onClick.AddListener(() => {
            NextStep();
        });

        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(() => {
            Play();
        });

        gameAreas = GameObject.Find("GameAreas").transform;
        UISteps = GameObject.Find("Canvas/Steps").transform;

        gameAreas.GetChild(0).gameObject.SetActive(true);
        UISteps.GetChild(0).gameObject.SetActive(true);

        UISteps.GetChild(12).gameObject.SetActive(false); //play button
    }

    private void Play() {
        SceneManager.LoadScene("GameScene");
    }

    private void NextStep() {
        if (stepNumber < gameAreas.childCount) {
            gameAreas.GetChild(stepNumber).gameObject.SetActive(false);
        }

        if (stepNumber < 10) {
            UISteps.GetChild(stepNumber).gameObject.SetActive(false);
        }

        stepNumber++;
        
        if (stepNumber > 9) {
            skipButton.gameObject.SetActive(false);
            StartCoroutine(TextAnimation());
        }

        if (stepNumber < gameAreas.childCount) {
            gameAreas.GetChild(stepNumber).gameObject.SetActive(true);
        }

        UISteps.GetChild(stepNumber).gameObject.SetActive(true);
    }

    private IEnumerator TextAnimation() {
        UISteps.GetChild(10).gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        UISteps.GetChild(11).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        UISteps.GetChild(12).gameObject.SetActive(true);
    }
}

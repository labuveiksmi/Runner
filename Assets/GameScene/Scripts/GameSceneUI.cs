using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField] private CountDown countDown = null;
    [SerializeField] private HealthScore healthScore = null;
    [SerializeField] private GameObject gameover = null;

    public void StartCountDown()
    {
        countDown.gameObject.SetActive(true);
    }

    public void SetDisplayedScore(int score)
    {
        healthScore.SetDisplayedScore(score);
    }

    public void SetDisplayedHealth(int health)
    {
        healthScore.SetDisplayedHealth(health);
    }

    internal void DisplayGameOver()
    {
        gameover.SetActive(true);
        StartCoroutine(WaitAndLoadMenu());
    }

    private IEnumerator WaitAndLoadMenu()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(ConstantsStrings.MainMenu);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField] private CountDown countDown = null;
    [SerializeField] private HealthScore healthScore = null;

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
}
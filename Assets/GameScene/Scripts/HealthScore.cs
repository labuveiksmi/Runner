using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScore : MonoBehaviour
{
    [SerializeField] private Text scoreText = null;
    [SerializeField] private Text healthText = null;

    public void SetDisplayedScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetDisplayedHealth(int health)
    {
        healthText.text = health.ToString();
    }
}
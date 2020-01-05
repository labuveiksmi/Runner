using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int givesScore = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstantsStrings.PlayerTag))
        {
            GameManager.Instance.AddScore(givesScore);
            gameObject.SetActive(false);
        }
    }
}
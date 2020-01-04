using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField] private CountDown countDown = null;

    public void StartCountDown()
    {
        countDown.gameObject.SetActive(true);
    }

    //private void OnEnable()
    //{
    //    GameManager.Instance.GameIsReady += StartCountDown;
    //    Debug.Log("subscribing to GameIsReady");
    //}

    //private void OnDisable()
    //{
    //    GameManager.Instance.GameIsReady -= StartCountDown;
    //    Debug.Log("unsubscribing to GameIsReady");
    //}
}
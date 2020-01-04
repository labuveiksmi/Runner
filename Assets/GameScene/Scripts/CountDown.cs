using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] private Text countText = null;

    private void OnEnable()
    {
        StartCoroutine(Count());
    }

    private IEnumerator Count()
    {
        for (int i = 3; i > 0; i--)
        {
            countText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countText.text = "Go";
        yield return new WaitForSeconds(0.4f);
        GameManager.Instance.StartGame();
        gameObject.SetActive(false);
    }
}
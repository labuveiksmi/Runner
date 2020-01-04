using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.IsPlaing)
        {
            transform.Translate(GameManager.Instance.RoadMoovingSpeed * Time.deltaTime);
        }
    }
}
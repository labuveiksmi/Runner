using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Instance.IsPlaing)
        {
            transform.Translate(GameManager.Instance.RoadMoovingSpeed * Time.deltaTime);
        }
    }
}
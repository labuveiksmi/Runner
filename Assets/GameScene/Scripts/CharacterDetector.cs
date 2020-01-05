﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstantsStrings.PlayerTag))
        {
            GameManager.Instance.ExtendRoad(true);
        }
    }
}
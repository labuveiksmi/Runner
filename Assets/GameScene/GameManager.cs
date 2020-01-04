using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PoolManager poolManager;
    [SerializeField] private int roadsAtStart = 5;

    //Should change this to calculated value, if we using roads with different sizes
    [SerializeField] private Vector3 roadShift;

    private Vector3 endRoadPosition;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        endRoadPosition = gameObject.transform.position;
        for (int i = 0; i < roadsAtStart; i++)
        {
            poolManager.PoolRoad(endRoadPosition);
            endRoadPosition += roadShift;
        }
    }
}
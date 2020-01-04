using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private RoadPooler roadPooler;

    public void PoolRoad(Vector3 position)
    {
        GameObject road = roadPooler.GetPooledRoad();
        road.transform.position = position;
        road.SetActive(true);
    }
}
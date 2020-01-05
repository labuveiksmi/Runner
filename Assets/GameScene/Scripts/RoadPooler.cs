using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPooler : MonoBehaviour
{
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private int roadsInPool = 20;

    private List<GameObject> roadList = new List<GameObject>();

    private void Start()
    {
        InitializePool();
    }

    public GameObject GetPooledRoad()
    {
        GameObject road;
        for (int i = 0; i < roadList.Count; i++)
        {
            road = roadList[i];
            if (!road.activeInHierarchy)
            {
                return road;
            }
        }
        //If we have no vacant road, we creating new one
        road = CreateNewRoad();
        return road;
    }

    private GameObject CreateNewRoad()
    {
        GameObject road = Instantiate(roadPrefab, transform);
        roadList.Add(road);
        road.SetActive(false);
        return road;
    }

    private void InitializePool()
    {
        for (int i = 0; i < roadsInPool; i++)
        {
            CreateNewRoad();
        }
    }
}
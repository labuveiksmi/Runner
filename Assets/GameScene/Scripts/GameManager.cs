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
    public static GameManager Instance;

    #region Singleton

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    #endregion Singleton

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        endRoadPosition = gameObject.transform.position;
        for (int i = 0; i < roadsAtStart; i++)
        {
            ExtendRoad();
        }
    }

    public void ExtendRoad()
    {
        poolManager.PoolRoad(endRoadPosition);
        endRoadPosition += roadShift;
    }
}
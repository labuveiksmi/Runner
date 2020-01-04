using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PoolManager poolManager;
    [SerializeField] private int roadsAtStart = 5;
    [SerializeField] private float cameraFloatStep = 0.2f;

    //Should change this to calculated value, if we using roads with different sizes
    [SerializeField] private Vector3 roadShift;

    //public delegate void GameReady();

    //public event GameReady GameIsReady;

    private Vector3 endRoadPosition;
    public static GameManager Instance;
    private GameSceneUI gameSceneUI;

    public GameSceneUI GameSceneUI
    {
        get
        {
            if (!gameSceneUI)
            {
                gameSceneUI = FindObjectOfType<GameSceneUI>();
            }
            return gameSceneUI;
        }
    }

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

    public void StartGame()
    {
        throw new NotImplementedException();
    }

    #endregion Singleton

    private void Start()
    {
        Initialize();
        StartCoroutine(PrepareGame());
    }

    private IEnumerator PrepareGame()
    {
        float steps = (Camera.main.transform.position.y - 1) / cameraFloatStep;
        for (int i = 0; i < steps; i++)
        {
            yield return new WaitForEndOfFrame();
            float cameraShift = Mathf.Lerp(Camera.main.transform.position.y, 1, cameraFloatStep);
            Camera.main.transform.position = new Vector3(0, cameraShift, -10);
        }
        //GameIsReady();
        GameSceneUI.StartCountDown();
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
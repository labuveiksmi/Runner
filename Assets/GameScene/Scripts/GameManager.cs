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

    public bool IsPlaing = false;

    public Vector3 RoadMoovingSpeed = Vector3.back * 4;
    private Vector3 endRoadPosition;
    public static GameManager Instance;
    private GameSceneUI gameSceneUI;
    private int score = 0;
    private int lives = 5;

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
        IsPlaing = true;
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
        GameSceneUI.StartCountDown();
    }

    private void Initialize()
    {
        endRoadPosition = gameObject.transform.position;
        for (int i = 0; i < roadsAtStart - 1; i++)
        {
            ExtendRoad();
        }
        //spawn last section without shifting endRoadPosition
        // should be removed, if we start mooving one parent, instead of every road section
        ExtendRoad(true);
    }

    /// <summary>
    /// Spawning new Road section, at the end of current
    /// </summary>
    /// <param name="detector">true if we spawning road, cause of character movement</param>
    public void ExtendRoad(bool detector = false)
    {
        poolManager.CreateRoad(endRoadPosition);
        if (!detector)
        {
            endRoadPosition += roadShift;
        }
        CreateStuffRandomly();
    }

    private void CreateStuffRandomly()
    {
        int randomNuber = UnityEngine.Random.Range(0, 20);
        if (randomNuber > 10)
        {
            if (randomNuber > 15)
            {
                poolManager.PopDanger(endRoadPosition);
            }
            else
            {
                poolManager.PopCoin(endRoadPosition);
            }
        }
    }

    public void AddScore(int score)
    {
        this.score += score;
        GameSceneUI.SetDisplayedScore(this.score);
    }

    public void TakeLive()
    {
        lives--;
        GameSceneUI.SetDisplayedHealth(lives);
        if (lives < 1)
        {
            GameSceneUI.DisplayGameOver();
            IsPlaing = false;
        }
    }
}
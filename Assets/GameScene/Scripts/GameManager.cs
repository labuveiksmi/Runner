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
        StartCoroutine(PrepareGame());
    }

    private IEnumerator PrepareGame()
    {
        float steps = (Camera.main.transform.position.y - 1) / cameraFloatStep;
        for (int i = 0; i < steps; i++)
        {
            yield return new WaitForEndOfFrame();
            float cameraShift = Mathf.Lerp(Camera.main.transform.position.y, 1, cameraFloatStep);
            Debug.Log(cameraShift);
            Camera.main.transform.position = new Vector3(0, cameraShift, -10);
        }
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
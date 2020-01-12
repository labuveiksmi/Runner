using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region PUBLIC VARIABLES

	public static GameManager Instance;

	[Header("Road")]
	[Range(RoadProperties.MIN_ROAD_SIZE, RoadProperties.MAX_ROAD_SIZE)]
	[Space(order = 20)]
	public int amountRoadInstances = 5;

	//TODO: Add checking on game type: AR or Simple game
	[Range(RoadProperties.MIN_ROAD_SPEED_AR_GAME, RoadProperties.MAX_ROAD_SPEED_AR_GAME)]
	public float roadSpeed = 0.15f;

	[Space(order = 20)]
	public bool isShowDebugLogMessagesRoad = true;

	[Header("Game")]
	[Space(order = 20)]
	public bool IsPlaying = false;

	#endregion PUBLIC VARIABLES

	#region PUBLIC PROPERTIES

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

	#endregion PUBLIC PROPERTIES

	#region PRIVATE VARIABLES

	[Header("Refs")]
	[Space(order = 20)]
	[SerializeField] private PoolManager _refPoolManager;
	[SerializeField] private PlayerVer2 _refPlayer;
	

	private GameSceneUI gameSceneUI;

	private Vector3 endRoadPosition;

	private int score = 0;
	[Header("Player")]
	[Space(order = 20)]
	[SerializeField] private int lives = 5;
	[SerializeField] private float _sideBiasForce;

	[Space(order = 20)]
	public bool isShowDebugLogMessagesPlayer = true;

	#endregion PRIVATE VARIABLES

	#region OBSOLETE

	[Obsolete]
	[HideInInspector]
	public Vector3 RoadMoovingSpeed = Vector3.back * 4;

	[Obsolete]
	private Vector3 roadShift;

	[Obsolete]
	private int roadsAtStart = 5;

	[Obsolete]
	private float cameraFloatStep = 0.2f;
	//Should change this to calculated value, if we using roads with different sizes

	#endregion OBSOLETE

	#region PUBLIC METHODS

	public void StartGame()
	{
		_refPoolManager.CreateRoad(amountRoadInstances);
		_refPlayer.IsCanJump = true;

		IsPlaying = true;
	}

	/// <summary>
	/// Spawning new Road section, at the end of current
	/// </summary>
	/// <param name="detector">true if we spawning road, cause of character movement</param>
	[Obsolete]
	public void ExtendRoad(bool detector = false)
	{
		_refPoolManager.CreateRoad(endRoadPosition);
		if (!detector)
		{
			endRoadPosition += roadShift;
		}
		CreateStuffRandomly();
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
			IsPlaying = false;
		}
	}

	#endregion PUBLIC METHODS

	#region PRIVATE METHODS

	#region NATIVE

	private void Awake()
	{
		Singleton();
	}

	private void Start()
	{
		InitializeGame();
		//StartCoroutine(PrepareGame());
	}

	private void Update()
	{
		if (IsPlaying)
		{
			_refPoolManager.MoveRoad(roadSpeed);
		}
		else
		{
			_refPoolManager.MoveRoad(0);
		}
	}

	#endregion NATIVE

	#region IENUMERATOR

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

	#endregion IENUMERATOR

	private void Singleton()
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

	private void InitializeGame()
	{
		//endRoadPosition = gameObject.transform.position;
		//for (int i = 0; i < roadsAtStart - 1; i++)
		//{
		//    ExtendRoad();
		//}
		//spawn last section without shifting endRoadPosition
		// should be removed, if we start mooving one parent, instead of every road section
		//ExtendRoad(true);

		_refPoolManager.InitializeRoad(roadSpeed, isShowDebugLogMessagesRoad);
		_refPlayer.Initialize(_sideBiasForce, isShowDebugLogMessagesPlayer);
		_refPlayer.IsCanJump = false;
	}

	private void CreateStuffRandomly()
	{
		//TODO:[START] For testing
		int randomValue = UnityEngine.Random.Range(0, 2);
		Vector3 randomPosition = endRoadPosition;

		switch (randomValue)
		{
			case 0:
				randomPosition = new Vector3(endRoadPosition.x - 0.025f, endRoadPosition.y, endRoadPosition.z);
				break;

			case 2:
				randomPosition = new Vector3(endRoadPosition.x + 0.025f, endRoadPosition.y, endRoadPosition.z);
				break;
		}

		//TODO:[END] For testing

		int randomNuber = UnityEngine.Random.Range(0, 20);
		if (randomNuber > 10)
		{
			if (randomNuber > 15)
			{
				_refPoolManager.PopDanger(randomPosition);
			}
			else
			{
				_refPoolManager.PopCoin(randomPosition);
			}
		}
	}

	#endregion PRIVATE METHODS
}
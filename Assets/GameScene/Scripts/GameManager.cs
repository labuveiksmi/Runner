using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	#region ROAD

	[Header("Road")]
	[Range(RoadProperties.MIN_ROAD_SIZE, RoadProperties.MAX_ROAD_SIZE)]
	[Space(order = 20)]
	public int amountRoadInstances = 5;

	//TODO: Add checking on game type: AR or Simple game
	[Range(RoadProperties.MIN_ROAD_SPEED_3D_GAME, RoadProperties.MAX_ROAD_SPEED_3D_GAME)]
	public float roadSpeed = 0.15f;
	public float speedBooster = 5f;
	public float timerSpeedBooster = 10f;

	private Vector3 _worldPositionEndRoad;

	[Space(order = 20)]
	public bool isShowDebugLogMessagesRoad = true;

	#endregion ROAD

	#region GAME

	[Header("Game")]
	[Space(order = 20)]
	public bool IsPlaying = false;
	private int score = 0;

	#endregion GAME

	#region REFS

	[Header("Refs")]
	[Space(order = 20)]
	[SerializeField] private PoolManager _refPoolManager;
	[SerializeField] private PlayerVer2 _refPlayer;
	private GameSceneUI gameSceneUI;

	#endregion REFS

	#region PLAYER

	[Header("Player")]
	[Space(order = 20)]
	[SerializeField] private int lives = 5;
	[SerializeField] private float _sideBiasForce;

	[Space(order = 20)]
	public bool isShowDebugLogMessagesPlayer = true;

	#endregion PLAYER

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
	[Obsolete]
	private Vector3 endRoadPosition;

	#endregion OBSOLETE

	#region PUBLIC METHODS

	public void StartGame()
	{
		_worldPositionEndRoad = _refPoolManager.CreateRoad(amountRoadInstances);

		_refPlayer.IsCanJump = true;

		IsPlaying = true;

		InvokeRepeating(nameof(IncreaseRoadSpeed), timerSpeedBooster * 2, timerSpeedBooster);
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

	public void CreateStuffRandomly()
	{
		//TODO:[START] For testing
		int randomValue = UnityEngine.Random.Range(0, 3);

		//var updatedPosition = _localPositionEndRoad;
		//if (Storage.instance?.LoadData(Storage.instance.aliasGameType) == (int)GAME_TYPE.AR_GAME ||
		//	SceneManager.GetActiveScene().name.Equals(ConstantsStrings.GameSceneAR))
		//{
		//	updatedPosition = new Vector3(updatedPosition.x, updatedPosition.y + (1 / 100f), updatedPosition.z);

		//}
		//else
		//{
		//	updatedPosition /= 2f;
		//	updatedPosition = new Vector3(updatedPosition.x, updatedPosition.y + 1f, updatedPosition.z);
		//}


		Debug.LogFormat("{0}: {1}", nameof(_worldPositionEndRoad), _worldPositionEndRoad);

		Vector3 randomWorldPosition = _worldPositionEndRoad;

		switch (randomValue)
		{
			case 0:
				randomWorldPosition = new Vector3(_worldPositionEndRoad.x - _sideBiasForce, _worldPositionEndRoad.y, _worldPositionEndRoad.z);
				break;

			case 2:
				randomWorldPosition = new Vector3(_worldPositionEndRoad.x + _sideBiasForce, _worldPositionEndRoad.y, _worldPositionEndRoad.z);
				break;
		}

		//TODO:[END] For testing
		Debug.LogFormat("{0}: {1}", nameof(randomWorldPosition), randomWorldPosition);

		int randomNumber = UnityEngine.Random.Range(0, 20);
		if (randomNumber > 10)
		{
			if (randomNumber > 15)
			{
				//_refPoolManager.PopDanger(randomPosition);
				_refPoolManager.PopCoin(randomWorldPosition);
			}
			else
			{
				//_refPoolManager.PopCoin(randomPosition);
				_refPoolManager.PopDanger(randomWorldPosition);
			}
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
		//DontDestroyOnLoad(gameObject);
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

		if (Storage.instance?.LoadData(Storage.instance.aliasGameType) == (int)GAME_TYPE.AR_GAME ||
			SceneManager.GetActiveScene().name.Equals(ConstantsStrings.GameSceneAR))
		{
			roadSpeed /= 100f;
			_sideBiasForce /= 100f;
			speedBooster /= 100f;
		}

		_refPoolManager.InitializeRoad(roadSpeed, isShowDebugLogMessagesRoad);
		_refPlayer.Initialize(_sideBiasForce, isShowDebugLogMessagesPlayer);
		_refPlayer.IsCanJump = false;
	}

	//TODO: Need to add a check for the end of the game, max road speed, maybe something else
	private void IncreaseRoadSpeed()
	{
		if (isShowDebugLogMessagesRoad)
		{
			Debug.LogFormat("To increase road speed!");
		}
		roadSpeed += speedBooster;
	}

	//TODO: Need to add a check for the end of the game, max road speed, maybe something else
	private void IncreaseRoadSpeed(float valueSpeedBooster)
	{
		roadSpeed += valueSpeedBooster;
	}

	#endregion PRIVATE METHODS
}
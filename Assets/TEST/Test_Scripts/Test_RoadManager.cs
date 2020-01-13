using System.Collections.Generic;
using UnityEngine;

public class Test_RoadManager : MonoBehaviour
{
	#region PUBLIC VARIABLES

	[Header("Prefabs", order = 10)]
	[Space(order = 20)]
	public Test_Road prefabRoad;
	public GameObject prefabTriggerOfRoad;

	[Space(order = 20)]
	[Header("Game objects", order = 10)]
	public GameObject containerRoad;

	//TODO: Under testing
	[Space(order = 20)]
	[Header("Properties", order = 10)]
	public float delayBeforeRebuildRoad;

	#region EVENTS

	public delegate void ChangedSpeed();
	public static event ChangedSpeed OnSpeedChanged;

	#endregion EVENTS

	#endregion PUBLIC VARIABLES

	#region PUBLIC PROPERTIES

	public Vector3 WorldPositionEndRoad { get { return _worldPositionEndRoad; } }

	#endregion PUBLIC PROPERTIES

	#region PRIVATE VARIABLES

	private static int _indexItemPooledRoad = 0;

	[Space(order = 20)]
	[Header("For testing", order = 10)]
	[SerializeField] private List<Test_Road> _pooledRoad = new List<Test_Road>();

	private GameObject _refRoadTrigger;

	private Vector3 _worldPositionEndRoad;

	private float _startSpeed;
	private float _previousSpeed;
	private float _speedRoadMovement;
	private float _amountItemsInPool;

	private bool _isShowDebugMessages;

	#endregion PRIVATE VARIABLES

	#region PUBLIC METHODS

	public void MoveRoad(float moveSpeed)
	{
		containerRoad.transform.position += Vector3.back * moveSpeed * Time.deltaTime;
	}

	public void RebuildRoad()
	{
		int lastItemIndex = _indexItemPooledRoad == 0 ? (_pooledRoad.Count - 1) : (_indexItemPooledRoad - 1);

		Vector3 endPointLastItem = _pooledRoad[lastItemIndex].endPoint.position;
		Vector3 beginPointSelectedItem = _pooledRoad[_indexItemPooledRoad].beginPoint.position;

		if (_isShowDebugMessages)
		{
			Debug.DrawLine(new Vector3(beginPointSelectedItem.x, (beginPointSelectedItem.y + 1) * 5, beginPointSelectedItem.z),
						   new Vector3(endPointLastItem.x, (endPointLastItem.y + 1) * 5, endPointLastItem.z), Color.yellow, 5);
		}

		_pooledRoad[_indexItemPooledRoad].transform.position += endPointLastItem - beginPointSelectedItem;

		_indexItemPooledRoad = (_indexItemPooledRoad >= _pooledRoad.Count - 1) ? 0 : _indexItemPooledRoad + 1;
	}

	#endregion PUBLIC METHODS

	#region PRIVATE METHODS

	#region NATIVE

	private void OnEnable()
	{
		Test_Trigger.OnEnterTrigger += HandlerOnEnterTrigger;
		OnSpeedChanged += HandlerOnChangedSpeed;
	}

	private void Update()
	{
		if (_previousSpeed != _speedRoadMovement)
		{
			OnSpeedChanged?.Invoke();

			_previousSpeed = _speedRoadMovement;
		}

		//MoveRoad(_speedRoadMovement);
	}

	private void OnDisable()
	{
		Test_Trigger.OnEnterTrigger -= HandlerOnEnterTrigger;
		OnSpeedChanged -= HandlerOnChangedSpeed;
	}

	#endregion NATIVE

	public void Initialize(float speedRoadMovement, bool isShowDebugLog = true)
	{
		this._speedRoadMovement = speedRoadMovement;
		_startSpeed = _speedRoadMovement;
		_previousSpeed = _speedRoadMovement;
		_isShowDebugMessages = isShowDebugLog;
	}

	private void CreateRoadTrigger(float numberItemsPool)
	{
		//TODO: [START] Create function for this
		float lengthRoadInstance = Vector3.Distance(prefabRoad.beginPoint.position, prefabRoad.endPoint.position);

		if (Storage.instance?.LoadData(Storage.instance.aliasGameType) == (int)GAME_TYPE.AR_GAME ||
			UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(ConstantsStrings.GameSceneAR))
		{
			lengthRoadInstance /= 100f;
		}

		if (_isShowDebugMessages)
		{
			Debug.LogFormat("{0}: {1}", nameof(lengthRoadInstance), lengthRoadInstance);
		}

		//TODO: Give normal name the variable and move variable to the inspector
		int partRoad = (int)numberItemsPool / 3;
		if (_isShowDebugMessages)
		{
			Debug.LogFormat("{0}: {1}", nameof(partRoad), partRoad);
		}

		float distanceTriggerFromRoad = lengthRoadInstance * partRoad;
		if (_isShowDebugMessages)
		{
			Debug.LogFormat("{0}: {1}", nameof(distanceTriggerFromRoad), distanceTriggerFromRoad);
		}
		//TODO: [END] Create function for this

		GameObject roadTriggerInstance = Instantiate(original: prefabTriggerOfRoad, parent: transform);
		roadTriggerInstance.transform.position = -Vector3.forward * distanceTriggerFromRoad;

		_refRoadTrigger = roadTriggerInstance;

		if (_isShowDebugMessages)
		{
			Debug.DrawLine(start: roadTriggerInstance.transform.position, end: roadTriggerInstance.transform.up * 50f, color: Color.blue, 5);
			Debug.LogFormat("{0}: {1}", nameof(roadTriggerInstance), roadTriggerInstance.transform.position);
		}
	}

	/// <summary>
	/// The method creates road instances according to the number of items in the pool
	/// </summary>
	/// <param name="numberItemsPool"></param>
	/// <returns>Position of end road</returns>
	public Vector3 CreateRoad(float numberItemsPool)
	{
		if (numberItemsPool < RoadProperties.MIN_ROAD_SIZE)
		{
			Debug.LogWarningFormat("({0}:{1}) is too small", nameof(numberItemsPool), numberItemsPool);

			throw new System.Exception();
		}
		else if (numberItemsPool > RoadProperties.MAX_ROAD_SIZE)
		{
			Debug.LogWarningFormat("({0}:{1}) is too big", nameof(numberItemsPool), numberItemsPool);

			throw new System.Exception();
		}

		CreateRoadTrigger(numberItemsPool);

		containerRoad.transform.position = _refRoadTrigger.transform.position;

		for (int i = 0; i < numberItemsPool; i++)
		{
			CreateRoadInstance(this.prefabRoad, containerRoad.transform);
		}

		BuildRoadAlong();

		return _worldPositionEndRoad;
	}

	/// <summary>
	/// The method creates a road instance, sets parent for the instance, adds to the list "Pooled Road"
	/// </summary>
	/// <param name="prefabRoad"></param>
	/// <param name="parentInstance"></param>
	private void CreateRoadInstance(Test_Road prefabRoad, Transform parentInstance)
	{
		if (parentInstance != null && prefabRoad != null)
		{
			Test_Road roadInstance = Instantiate(original: prefabRoad, parent: parentInstance.transform);
			_pooledRoad.Add(roadInstance);
		}
		else if (parentInstance == null)
		{
			Debug.LogWarningFormat("{0} is null", nameof(parentInstance));
		}
		else if (prefabRoad == null)
		{
			Debug.LogWarningFormat("{0} is null", nameof(prefabRoad));
		}
		else
		{
			throw new System.Exception();
		}
	}

	private void BuildRoadAlong()
	{
		//TODO: Add checking for the list on null
		for (int i = 1; i < _pooledRoad.Count; i++)
		{
			_pooledRoad[i].transform.position += _pooledRoad[i - 1].endPoint.position - _pooledRoad[i].beginPoint.position;
			if (_isShowDebugMessages)
			{
				Debug.DrawLine(start: _pooledRoad[i].transform.position, end: _pooledRoad[i].transform.up * 100, color: Color.green, 5);
				Debug.DrawLine(start: _pooledRoad[i - 1].endPoint.position, end: _pooledRoad[i].beginPoint.position, color: Color.white, 5);
			}
		}

		// Setting the world position end of road
		_worldPositionEndRoad = _pooledRoad[_pooledRoad.Count - 1].transform.position;

		if (_isShowDebugMessages)
		{
			Vector3 end = new Vector3(_worldPositionEndRoad.x, (_worldPositionEndRoad.y + 1) * 60, _worldPositionEndRoad.z);
			Debug.DrawLine(start: _worldPositionEndRoad, end: end, color: Color.red, 5);
		}

	}

	/// <summary>
	/// The method returns a new delay value which depends on the percent of the changed speed
	/// </summary>
	/// <param name="currentSpeed"></param>
	/// <param name="startSpeed"></param>
	/// <param name="delayRebuild"></param>
	/// <returns>A new delay value</returns>
	private float UpdateDelayRebuildRoad(float currentSpeed, float startSpeed, float delayRebuild)
	{
		const float ONE_HUNDRED_PERCENT = 100f;

		// Get percent current speed. Then get percent difference
		float percentChangedSpeed = (ONE_HUNDRED_PERCENT * currentSpeed / startSpeed) - ONE_HUNDRED_PERCENT;
		if (_isShowDebugMessages)
		{
			Debug.LogFormat("Percent of changed speed ({0}:{1})", nameof(percentChangedSpeed), percentChangedSpeed);
		}

		float percentUpdatedDelay = delayRebuild * (percentChangedSpeed / ONE_HUNDRED_PERCENT);
		if (_isShowDebugMessages)
		{
			Debug.LogFormat("{0}:{1}", nameof(percentUpdatedDelay), percentUpdatedDelay);
		}

		return delayRebuild - percentUpdatedDelay;
	}

	#region HANDLERS

	private void HandlerOnEnterTrigger(GameObject target)
	{
		if (_isShowDebugMessages)
		{
			Debug.LogFormat("{0} entered to the road trigger", target.name);
		}

		if (target.name.Equals(_pooledRoad[0].name))
		{
			Invoke(nameof(RebuildRoad), delayBeforeRebuildRoad);
		}
	}

	private void HandlerOnChangedSpeed()
	{
		delayBeforeRebuildRoad = (delayBeforeRebuildRoad > 0) ? UpdateDelayRebuildRoad(_speedRoadMovement, _startSpeed, delayBeforeRebuildRoad) : 0;
	}

	#endregion HANDLERS

	#endregion PRIVATE METHODS
}
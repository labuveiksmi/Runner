using System.Collections.Generic;
using UnityEngine;

public class Test_RoadManager : MonoBehaviour
{
    #region PUBLIC VARIABLES

    [Header("For testing", order = 10)]
    public bool isShowDebugMessages = true;

    public float speedRoadMovement;

    [Space(order = 20)]
    [Header("Prefabs", order = 10)]
    public Test_Road prefabRoad;

    public GameObject prefabTriggerOfRoad;

    [Space(order = 20)]
    [Header("Game objects", order = 10)]
    public GameObject containerRoad;

    [Space(order = 20)]
    [Header("Properties", order = 10)]
    public float amountItemsInPool;

    public float delayBeforeRebuildRoad;

    [Space(order = 20)]

    #endregion PUBLIC VARIABLES

    #region PRIVATE VARIABLES

    private List<Test_Road> _pooledRoad = new List<Test_Road>();

    private int _indexItemPooledRoad = 0;

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

        if (isShowDebugMessages)
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
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        //TODO: For testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RebuildRoad();
        }
        if (Input.GetKey(KeyCode.W))
        {
            MoveRoad(speedRoadMovement);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            MoveRoad(0);
        }

        MoveRoad(speedRoadMovement);
    }

    private void OnDisable()
    {
        Test_Trigger.OnEnterTrigger -= HandlerOnEnterTrigger;
    }

    #endregion NATIVE

    private void Initialize()
    {
        CreateRoadTrigger();

        CreateRoads();
        SetRoadAlong();
    }

    private void CreateRoadTrigger()
    {
        //TODO: [START] Create function for this
        float lengthRoadInstance = Vector3.Distance(prefabRoad.beginPoint.position, prefabRoad.endPoint.position);
        if (isShowDebugMessages)
        {
            Debug.LogFormat("{0}: {1}", nameof(lengthRoadInstance), lengthRoadInstance);
        }
        //TODO: Give normal name the variable and move variable to the inspector
        int halfRoads = (int)amountItemsInPool / 2;
        if (isShowDebugMessages)
        {
            Debug.LogFormat("{0}: {1}", nameof(halfRoads), halfRoads);
        }
        float distanceForTrigger = lengthRoadInstance * halfRoads;
        if (isShowDebugMessages)
        {
            Debug.LogFormat("{0}: {1}", nameof(distanceForTrigger), distanceForTrigger);
        }
        //TODO: [END] Create function for this

        GameObject roadTriggerInstance = Instantiate(original: prefabTriggerOfRoad, parent: transform);
        roadTriggerInstance.transform.localPosition = -roadTriggerInstance.transform.forward * distanceForTrigger;
        if (isShowDebugMessages)
        {
            Debug.DrawLine(start: roadTriggerInstance.transform.position, end: roadTriggerInstance.transform.up * 5, color: Color.blue, 5);
            Debug.LogFormat("{0}: {1}", nameof(roadTriggerInstance.transform.position), roadTriggerInstance.transform.position);
        }
    }

    private void CreateRoads()
    {
        //TODO: Add checking for amountItemsInPool
        for (int i = 0; i < amountItemsInPool; i++)
        {
            CreateRoadInstance();
        }
    }

    private void CreateRoadInstance()
    {
        if(containerRoad != null)
        {
            Test_Road roadInstance = Instantiate(original: prefabRoad, parent: containerRoad.transform);
            _pooledRoad.Add(roadInstance);
        }
        else
        {
            Debug.LogWarningFormat("{0} is null", nameof(containerRoad));
        }
    }

    //TODO: Give normal name the function
    private void SetRoadAlong()
    {
        //TODO: Add checking for the list on null
        for (int i = 1; i < _pooledRoad.Count; i++)
        {
            _pooledRoad[i].transform.position = _pooledRoad[i - 1].endPoint.position - _pooledRoad[i].beginPoint.position;
        }
    }

    #region HANDLERS

    private void HandlerOnEnterTrigger(GameObject target)
    {
        if (isShowDebugMessages)
        {
            Debug.LogFormat("{0} entered to the road trigger", target.name);
        }

        if (target.name.Equals(_pooledRoad[0].name))
        {
            Invoke(nameof(RebuildRoad), delayBeforeRebuildRoad);
        }
    }

    #endregion HANDLERS

    #endregion PRIVATE METHODS
}
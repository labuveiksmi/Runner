using System.Collections;
using UnityEngine;

public class PlayerVer2 : MonoBehaviour
{
	#region PUBLIC VARIABLES


	#endregion PUBLIC VARIABLES

	#region PUBLIC PROPERTIES

	public bool IsCanJump { get { return _isCanJump; } set { _isCanJump = value; } }

	#endregion PUBLIC PROPERTIES

	#region PRIVATE VARIABLES

	private enum MOVE { LEFT, RIGHT }

	[SerializeField] private float jumpForce;
	/*[SerializeField] */private float _sideBiasForce;
	[SerializeField] private float jumpReloadingTime;

	/*[SerializeField] */private bool _isCanJump;
	private bool _isShowDebugMessages;

	//TODO: For testing. After testing need to delete
	[Header("Test", order = 20)]
	[SerializeField] private GameObject prefabPlayerAxisForward;
	[SerializeField] private GameObject prefabPlayerAxisRight;

	//TODO: For testing. After testing need to delete
	private GameObject playerForward;
	private GameObject playerRight;

	private Vector3 startPlayerPosition;

	#endregion PRIVATE VARIABLES

	#region PUBLIC METHODS

	public void Initialize(float sideBiasForce, bool isShowDebugLog = true)
	{
		_sideBiasForce = sideBiasForce;
		_isShowDebugMessages = isShowDebugLog;
	}

	#endregion PUBLIC METHODS

	#region PRIVATE METHODS

	#region NATIVE

	private void OnEnable()
	{
		SubscribeEvents();
	}

	private void OnDisable()
	{
		UnSubscribeEvents();
	}

	private void Start()
	{
		Initialize();
	}

	#endregion NATIVE

	#region HANDLERS

	private void HandlerOnSwipeUp()
	{
		if (_isShowDebugMessages)
		{
			Debug.Log("Event on swipe up");
		}

		Jump();
	}

	private void HandlerOnSwipeRight()
	{
		if (_isShowDebugMessages)
		{
			Debug.Log("Event on swipe right");
		}

		Movement(MOVE.RIGHT);
	}

	private void HandlerOnSwipeLeft()
	{
		if (_isShowDebugMessages)
		{
			Debug.Log("Event on swipe left");
		}

		Movement(MOVE.LEFT);
	}

	#endregion HANDLERS

	#region IENUMERATOR

	private IEnumerator IReloadingJump()
	{
		_isCanJump = false;
		yield return new WaitForSecondsRealtime(jumpReloadingTime);
		_isCanJump = true;
	}

	private IEnumerator IJump()
	{
		transform.Translate(transform.up * jumpForce, Space.World);
		yield return StartCoroutine(IReloadingJump());
		transform.Translate(-transform.up * jumpForce, Space.World);
	}

	#endregion IENUMERATOR

	private void Initialize()
	{
		InitializePlayerDirection();

		startPlayerPosition = transform.position;
	}

	private void InitializePlayerDirection()
	{
		// Instantiate axes
		playerForward = Instantiate(prefabPlayerAxisForward, transform);
		playerRight = Instantiate(prefabPlayerAxisRight, transform);

		// Set parent for axes
		playerForward.transform.parent = playerRight.transform.parent = transform;
	}

	private void Jump()
	{
		if (_isCanJump)
		{
			StartCoroutine(IJump());

			if (_isShowDebugMessages)
			{
				Debug.Log("Player jumped");
			}
		}
	}

	private void Movement(MOVE moveTO)
	{
		switch (moveTO)
		{
			case MOVE.LEFT:
				float extremeLeftPoint = (float)System.Math.Round(startPlayerPosition.x - _sideBiasForce, 3);
				bool isPlayerPosMoreExtreme = transform.position.x > extremeLeftPoint;

				if (isPlayerPosMoreExtreme)
				{
					//TODO: Add smoothness in the future
					transform.position = new Vector3(transform.position.x - _sideBiasForce, transform.position.y, transform.position.z);

					if (_isShowDebugMessages)
					{
						Debug.Log("The player has moved to the left");
					}
				}
				else
				{
					Debug.Log("The player has reached extreme left point");
				}

				break;

			case MOVE.RIGHT:
				float extremeRightPoint = (float)System.Math.Round(startPlayerPosition.x + _sideBiasForce, 3);
				bool isPlayerPosLessExtreme = transform.position.x < extremeRightPoint;

				if (isPlayerPosLessExtreme)
				{
					//TODO: Add smoothness in the future
					transform.position = new Vector3(transform.position.x + _sideBiasForce, transform.position.y, transform.position.z);

					if (_isShowDebugMessages)
					{
						Debug.Log("The player has moved to the right");
					}
				}
				else
				{
					Debug.Log("The player has reached extreme right point");
				}

				break;
		}
	}

	private void SubscribeEvents()
	{
		InputController.OnSwipeLeft += HandlerOnSwipeLeft;
		InputController.OnSwipeRight += HandlerOnSwipeRight;
		InputController.OnSwipeUp += HandlerOnSwipeUp;
	}

	private void UnSubscribeEvents()
	{
		InputController.OnSwipeLeft -= HandlerOnSwipeLeft;
		InputController.OnSwipeRight -= HandlerOnSwipeRight;
		InputController.OnSwipeUp -= HandlerOnSwipeUp;
	}

	#endregion PRIVATE METHODS
}
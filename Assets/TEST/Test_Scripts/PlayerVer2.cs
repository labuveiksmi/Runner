using System.Collections;
using System.Threading;
using UnityEngine;

public class PlayerVer2 : MonoBehaviour
{
    #region PUBLIC VARIABLES

    public bool isShowDebugMessages = true;

    #endregion PUBLIC VARIABLES

    #region PRIVATE VARIABLES

    private enum MOVE { LEFT, RIGHT }

    [SerializeField] private float jumpForce;
    [SerializeField] private float sideBiasForce;
    [SerializeField] private float jumpReloadingTime;

    [SerializeField] private bool isCanJump = true;

    //TODO: For testing. After testing need to delete
    [Header("Test", order = 20)]
    [SerializeField] private GameObject prefabPlayerAxisForward;
    [SerializeField] private GameObject prefabPlayerAxisRight;

    //TODO: For testing. After testing need to delete
    private GameObject playerForward;
    private GameObject playerRight;

    private Vector3 startPlayerPosition;

    #endregion PRIVATE VARIABLES

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
        if (isShowDebugMessages)
            Debug.Log("Event on swipe up");
        Jump();
    }

    private void HandlerOnSwipeRight()
    {
        if (isShowDebugMessages)
            Debug.Log("Event on swipe right");
        Movement(MOVE.RIGHT);
    }

    private void HandlerOnSwipeLeft()
    {
        if (isShowDebugMessages)
            Debug.Log("Event on swipe left");
        Movement(MOVE.LEFT);
    }

    #endregion HANDLERS

    #region IENUMERATOR

    private IEnumerator IReloadingJump()
    {
        isCanJump = false;
        yield return new WaitForSecondsRealtime(jumpReloadingTime);
        isCanJump = true;
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
        /* Instantiate axes */
        playerForward = Instantiate(prefabPlayerAxisForward, transform);
        playerRight = Instantiate(prefabPlayerAxisRight, transform);

        /* Set parent for axes */
        playerForward.transform.parent = playerRight.transform.parent = transform;

    }

    private void Jump()
    {
        if (isCanJump)
        {
            StartCoroutine(IJump());

            if (isShowDebugMessages)
                Debug.Log("Player jumped");
        }
    }

    private void Movement(MOVE moveTO)
    {
        switch (moveTO)
        {
            case MOVE.LEFT:
                float extremeLeftPoint = (float)System.Math.Round(startPlayerPosition.x - sideBiasForce, 3);
                bool isPlayerPosMoreExtreme = transform.position.x > extremeLeftPoint;

                if (isPlayerPosMoreExtreme)
                {
                    //TODO: Add smoothness in the future
                    transform.position = new Vector3(transform.position.x - sideBiasForce, transform.position.y, transform.position.z);

                    if (isShowDebugMessages)
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
                float extremeRightPoint = (float)System.Math.Round(startPlayerPosition.x + sideBiasForce, 3);
                bool isPlayerPosLessExtreme = transform.position.x < extremeRightPoint;

                if (isPlayerPosLessExtreme)
                {
                    //TODO: Add smoothness in the future
                    transform.position = new Vector3(transform.position.x + sideBiasForce, transform.position.y, transform.position.z);

                    if (isShowDebugMessages)
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
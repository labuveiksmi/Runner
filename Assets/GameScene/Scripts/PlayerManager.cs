using System.Collections;
using UnityEngine;

[System.Obsolete]
public class PlayerManager : MonoBehaviour
{
    #region PUBLIC VARIABLES

    public bool isShowDebugLog = true;

    #endregion PUBLIC VARIABLES

    #region PUBLIC PROPERTIES

    public bool IsCanJump { get { return isCanJump; } }

    #endregion PUBLIC PROPERTIES

    #region PRIVATE VARIABLES

    private enum MOVE { LEFT, RIGHT}

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float sideBiasForce = 5f;
    [SerializeField] private float jumpReloadingTime = 1f;

    [SerializeField] private bool isCanJump = true;

    private Rigidbody playerRigidBody;

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
        if (isShowDebugLog)
            Debug.Log("Event on swipe up");
        Jump();
    }
    private void HandlerOnSwipeRight()
    {
        if (isShowDebugLog)
            Debug.Log("Event on swipe right");
        Movement(MOVE.RIGHT);
    }
    private void HandlerOnSwipeLeft()
    {
        if (isShowDebugLog)
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

    #endregion IENUMERATOR

    private void Jump()
    {
        if (isCanJump)
        {
            playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            StartCoroutine(IReloadingJump());

            if (isShowDebugLog)
                Debug.Log("Player jumped");
        }
    }
    private void Initialize()
    {
        playerRigidBody = GetComponent<Rigidbody>();
    }
    private void Movement(MOVE moveTO)
    {
        switch (moveTO)
        {
            case MOVE.LEFT:
                playerRigidBody.AddForce(Vector3.left * sideBiasForce, ForceMode.Impulse);

                if (isShowDebugLog)
                    Debug.Log("The player has moved to the left");
                break;
            case MOVE.RIGHT:
                playerRigidBody.AddForce(Vector3.right * sideBiasForce, ForceMode.Impulse);

                if (isShowDebugLog)
                    Debug.Log("The player has moved to the right");
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
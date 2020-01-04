using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region PUBLIC PROPERTIES

    public bool IsCanJump { get { return _isCanJump; } }

    #region DELEGATE

    public delegate void PlayerDie();

    #endregion DELEGATE
    #region EVENTS

    public static event PlayerDie OnPlayerDie;

    #endregion EVENTS

    #endregion PUBLIC PROPERTIES

    #region PRIVATE VARIABLES

    private enum MOVE { LEFT, RIGHT}


    [SerializeField] private float _healthPoint = 3f;
    [SerializeField] private float _forceJump = 10f; // 10
    [SerializeField] private float _forceBiasSide = 2.5f; // 2.5
    [SerializeField] private float _timeReloadingJump = 1f;

    [SerializeField] private bool _isCanJump = true;

    private Rigidbody _playerRigidBody;

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
        Debug.Log("Event on swipe up");
        Jump();
    }
    private void HandlerOnSwipeRight()
    {
        Debug.Log("Event on swipe right");
        Movement(MOVE.RIGHT);
    }
    private void HandlerOnSwipeLeft()
    {
        Debug.Log("Event on swipe left");
        Movement(MOVE.LEFT);
    }
    private void HandlerOnIncreaseHealthPoint()
    {
        IncreaseHealthPoint(1);
    }
    private void HandlerOnDecreaseHealthPoint()
    {
        DecreaseHealthPoint(1);
    }

    #endregion HANDLERS

    #region IENUMERATOR

    private IEnumerator IReloadingJump()
    {
        _isCanJump = false;
        yield return new WaitForSecondsRealtime(_timeReloadingJump);
        _isCanJump = true;
    }

    #endregion IENUMERATOR

    private void IncreaseHealthPoint(int amountHealthPoint)
    {
        _healthPoint += amountHealthPoint;
        Debug.Log("Was added " + amountHealthPoint + " health points");
        Debug.Log("HP: " + _healthPoint);
    }
    private void DecreaseHealthPoint(int amountHealthPoint)
    {
        if (_healthPoint <= 0)
        {
            OnPlayerDie?.Invoke();
        }
        else
        {
            _healthPoint -= amountHealthPoint;
            Debug.Log("Was decrease " + amountHealthPoint + " health points");
            Debug.Log("HP: " + _healthPoint);
        }
    }
    private void Jump()
    {
        if (_isCanJump)
        {
            _playerRigidBody.AddForce(Vector3.up * _forceJump, ForceMode.Impulse);
            StartCoroutine(IReloadingJump());

            Debug.Log("Player jumped");
        }
    }
    private void Initialize()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
    }
    private void Movement(MOVE moveTO)
    {
        switch (moveTO)
        {
            case MOVE.LEFT:
                _playerRigidBody.AddForce(Vector3.left * _forceBiasSide, ForceMode.Impulse);

                Debug.Log("The player has moved to the left");
                break;
            case MOVE.RIGHT:
                _playerRigidBody.AddForce(Vector3.right * _forceBiasSide, ForceMode.Impulse);

                Debug.Log("The player has moved to the right");
                break;
        }
    }
    private void SubscribeEvents()
    {
        InputController.OnSwipeLeft += HandlerOnSwipeLeft;
        InputController.OnSwipeRight += HandlerOnSwipeRight;
        InputController.OnSwipeUp += HandlerOnSwipeUp;
        InputController.OnIncreaseHealthPoint += HandlerOnIncreaseHealthPoint;
        InputController.OnDecreaseHealthPoint += HandlerOnDecreaseHealthPoint;
    }
    
    private void UnSubscribeEvents()
    {
        InputController.OnSwipeLeft -= HandlerOnSwipeLeft;
        InputController.OnSwipeRight -= HandlerOnSwipeRight;
        InputController.OnSwipeUp -= HandlerOnSwipeUp;
        InputController.OnIncreaseHealthPoint -= HandlerOnIncreaseHealthPoint;
        InputController.OnDecreaseHealthPoint -= HandlerOnDecreaseHealthPoint;
    }

    #endregion PRIVATE METHODS
}
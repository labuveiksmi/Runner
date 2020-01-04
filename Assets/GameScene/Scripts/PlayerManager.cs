using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region PUBLIC PROPERTIES

    public bool IsCanJump { get { return _isCanJump; } }
    public bool IsPlayerDie { get { return _healthPoint <= 0 ? true : false; } }

    #endregion PUBLIC PROPERTIES

    #region PRIVATE VARIABLES

    private const float MaxHealthPoint = 3;

    [SerializeField] private float _healthPoint = 3f;
    [SerializeField] private float _forceJump = 10f; // 10
    [SerializeField] private float _forceBiasSide = 2.5f; // 2.5
    [SerializeField] private float _timeReloadingJump = 1f;

    [SerializeField] private bool _isCanJump = true;

    private Rigidbody _playerRigidBody;

    #endregion PRIVATE VARIABLES

    #region PRIVATE METHODS

    #region NATIVE

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (!IsPlayerDie)
        {
            if (InputController.IsSwipeUp)
            {
                Jump();
            }
            if (Input.GetKeyUp(KeyCode.O))
            {
                IncreaseHealthPoint(1);
            }
            if (Input.GetKeyUp(KeyCode.I))
            {
                DecreaseHealthPoint(1);
            }

            Movement();
        }
    }

    #endregion NATIVE

    #region IENUMERATOR

    private IEnumerator IJump()
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
        _healthPoint -= amountHealthPoint;
        Debug.Log("Was decrease " + amountHealthPoint + " health points");
        Debug.Log("HP: " + _healthPoint);
    }

    private void Jump()
    {
        if (_isCanJump)
        {
            _playerRigidBody.AddForce(Vector3.up * _forceJump, ForceMode.Impulse);
            StartCoroutine(IJump());

            Debug.Log("Player jumped");
        }
    }

    private void Initialize()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
    }

    private void Movement()
    {
        if (InputController.IsSwipeLeft)
        {
            _playerRigidBody.AddForce(Vector3.left * _forceBiasSide, ForceMode.Impulse);

            Debug.Log("The player has moved to the left");
        }
        if (InputController.IsSwipeRight)
        {
            _playerRigidBody.AddForce(Vector3.right * _forceBiasSide, ForceMode.Impulse);

            Debug.Log("The player has moved to the right");
        }
    }

    #endregion PRIVATE METHODS
}
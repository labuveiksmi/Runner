using UnityEngine;

public class InputController : MonoBehaviour
{
    public delegate void Swipe();
    public static event Swipe OnSwipeLeft;
    public static event Swipe OnSwipeRight;
    public static event Swipe OnSwipeUp;

    public delegate void PressedButton();
    public static event PressedButton OnIncreaseHealthPoint;
    public static event PressedButton OnDecreaseHealthPoint;

    private bool _tap;
    private bool _isDraging = false;
    private Vector2 _startTouch, _swipeDelta;

    [SerializeField] private float _sensitivitySwipe = 150f;

    private void Update()
    {
        _tap = false;

        #region Standalone Inputs

                //#region Mouse Inputs

        ////if (Input.GetMouseButtonDown(0))
        ////{
        ////    _tap = _isDraging = true;
        ////    _startTouch = Input.mousePosition;
        ////}
        ////else if (Input.GetMouseButtonUp(0))
        ////{
        ////    _isDraging = false;
        ////    Reset();
        ////}

        //#endregion Mouse Inputs

        #region Keyboard Inputs

        if (Input.GetKeyDown(KeyCode.A))
        {
            OnSwipeLeft?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnSwipeRight?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSwipeUp?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            OnIncreaseHealthPoint?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnDecreaseHealthPoint?.Invoke();
        }
        #endregion Keyboard Inputs

        #endregion Standalone Inputs

        #region Mobile Inputs

        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                _tap = _isDraging = true;
                _startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                _isDraging = false;
                Reset();
            }
        }

        #endregion Mobile Inputs

        // Calculate the distance
        _swipeDelta = Vector2.zero;
        if (_isDraging)
        {
            if (Input.touches.Length > 0)
            {
                _swipeDelta = Input.touches[0].position - _startTouch;
            }
            else if (Input.GetMouseButton(0))
            {
                _swipeDelta = (Vector2)Input.mousePosition - _startTouch;
            }
        }

        // Did we cross the deadzone?
        if (_swipeDelta.magnitude > _sensitivitySwipe)
        {
            // Which direction
            float x = _swipeDelta.x;
            float y = _swipeDelta.y;

            if (Mathf.Abs(x) < Mathf.Abs(y))
            {
                if (y > 0)
                {
                    OnSwipeUp?.Invoke();
                }
            }
            else
            {
                if (x > 0)
                {
                    OnSwipeRight?.Invoke();
                }
                else
                {
                    OnSwipeLeft?.Invoke();
                }
            }

            Reset();
        }

    }
    private void OnMouseDown()
    {
        _tap = _isDraging = true;
        _startTouch = Input.mousePosition;
    }
    private void OnMouseUp()
    {
        _isDraging = false;
        Reset();
    }

    private void Reset()
    {
        _startTouch = _swipeDelta = Vector2.zero;
        _isDraging = false;
    }
}
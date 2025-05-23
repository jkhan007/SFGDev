using UnityEngine;


public class PlayerControler : MonoBehaviour
{
    public float forwardSpeed = 5f;       // Forward movement speed
    public float horizontalSpeed = 10f;   // Horizontal movement speed
    public float laneLimit = 3f;          // How far left/right player can move
    public float sensitivity = 0.01f;     // Swipe sensitivity
    public float smoothTime = 0.1f;       // Smoothing time for horizontal movement

    private Vector2 lastTouchPosition;
    private bool isDragging = false;

    private float targetX;
    private float velocityX = 0f;

    void Start()
    {
        targetX = transform.position.x;
    }

    void Update()
    {
        HandleInput();

        // Move forward constantly
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Smooth horizontal movement
        float newX = Mathf.SmoothDamp(transform.position.x, targetX, ref velocityX, smoothTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    void HandleInput()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPosition;
            UpdateTargetX(delta.x);
            lastTouchPosition = Input.mousePosition;
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.position - lastTouchPosition;
                UpdateTargetX(delta.x);
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
#endif
    }

    void UpdateTargetX(float deltaX)
    {
        targetX += deltaX * sensitivity;
        targetX = Mathf.Clamp(targetX, -laneLimit, laneLimit);
    }

}

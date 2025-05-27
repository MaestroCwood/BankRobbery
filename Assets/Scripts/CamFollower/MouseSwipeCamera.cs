using UnityEngine;
using UnityEngine.EventSystems;

public class MouseSwipeCamera : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Camera")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -4);
    [SerializeField] private float minY = -30f, maxY = 60f;

    [Header("Swipe Settings")]
    public float sensitivity = 0.2f;
    public bool invert = false;

    private float yaw = 0f;
    private float pitch = 0f;
    private bool isDragging = false;
    private Vector2 lastInputPos;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("sensitivity"))
            sensitivity = PlayerPrefs.GetFloat("sensitivity");
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_WEBGL
        if (Input.GetMouseButtonDown(0))
        {
            if (IsOnRightSide(Input.mousePosition))
            {
                isDragging = true;
                lastInputPos = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastInputPos;
            lastInputPos = Input.mousePosition;

            ApplyRotation(delta);
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // только первый палец

            if (touch.phase == TouchPhase.Began)
            {
                if (IsOnRightSide(touch.position))
                {
                    isDragging = true;
                    lastInputPos = touch.position;
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }

            if (isDragging && touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.position - lastInputPos;
                lastInputPos = touch.position;

                ApplyRotation(delta);
            }
        }
#endif
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsOnRightSide(eventData.position))
        {
            isDragging = true;
            lastInputPos = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void ApplyRotation(Vector2 delta)
    {
        float dx = delta.x * sensitivity;
        float dy = delta.y * sensitivity;

        if (invert)
        {
            dx = -dx;
            dy = -dy;
        }

        yaw += dx;
        pitch -= dy;
        pitch = Mathf.Clamp(pitch, minY, maxY);
    }

    private bool IsOnRightSide(Vector2 position)
    {
        return position.x > Screen.width / 2f;
    }

    void LateUpdate()
    {
        if (!target) return;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = target.position + rotation * offset;
        transform.rotation = rotation;
    }
}

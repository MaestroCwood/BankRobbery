using UnityEngine;
using YG;
[RequireComponent(typeof(CharacterController))]
public class SimpleFPSController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = 20f;
    public DynamicJoystick joystick;

    [Header("Camera")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float lookXLimit = 80f;

    public GameObject canvasIsMObile;
    public RectTransform lookArea;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    Vector3 direction;
    private float rotationX = 0;
    private Vector2 lastTouchPos;
    private bool isDragging = false;
  

    bool isMobile;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        isMobile = YG2.envir.isMobile;

        canvasIsMObile.SetActive(isMobile);
        // Блокируем курсор

        if(!isMobile)
        {
             Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    void Update()
    {
       
        if (isMobile)
        {
            HandleTouchLook(); // свайп камеры
        }

        Move();
    }


    void HandleTouchLook()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Проверка: находится ли палец над Image
            if (RectTransformUtility.RectangleContainsScreenPoint(lookArea, touch.position))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    lastTouchPos = touch.position;
                    isDragging = true;
                }
                else if (touch.phase == TouchPhase.Moved && isDragging)
                {
                    Vector2 delta = touch.position - lastTouchPos;
                    lastTouchPos = touch.position;

                    float mouseX = delta.x * lookSensitivity * 0.05f;
                    float mouseY = delta.y * lookSensitivity * 0.05f;

                    rotationX -= mouseY;
                    rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                    cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);

                    transform.rotation *= Quaternion.Euler(0, mouseX, 0);
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isDragging = false;
                }
            }
        }
    }

    void Move()
    {
        // ==== Вращение мышью ====
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

       // rotationX -= mouseY;
       // rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        //cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        //transform.rotation *= Quaternion.Euler(0, mouseX, 0);

        if (controller.isGrounded)
        {
            // Получаем ввод
            float inputX = isMobile ? joystick.Horizontal : Input.GetAxis("Horizontal");
            float inputY = isMobile ? joystick.Vertical : Input.GetAxis("Vertical");

            // Направления движения относительно игрока
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            // Формируем направление движения
            Vector3 move = (forward * inputY + right * inputX).normalized;

            moveDirection = move * moveSpeed;

            // Прыжок
            if (!isMobile && Input.GetButton("Jump"))
                moveDirection.y = jumpForce;
        }

        // Применение гравитации
        moveDirection.y -= gravity * Time.deltaTime;

        // Движение с учетом гравитации
        controller.Move(moveDirection * Time.deltaTime);
        // Прыжок
        if (Input.GetButton("Jump"))
                moveDirection.y = jumpForce;
        
    }
}

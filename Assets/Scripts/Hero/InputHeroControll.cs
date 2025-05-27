using UnityEngine;
using YG;

public class InputHeroControll : MonoBehaviour
{
    public InputSystem_Actions inputPlayer;
    Icontrollable iConrollable;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] Joystick joystick;

    Vector3 move;
    bool isMobile;
    private void Awake()
    {   
        isMobile = YG2.envir.isMobile;
        inputPlayer = new InputSystem_Actions();
        iConrollable = GetComponent<Icontrollable>();
        inputPlayer.Enable();
    }

    private void OnEnable()
    {
        inputPlayer.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        inputPlayer.Player.Jump.performed -= OnJumpPerformed;
    }

    private void OnJumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        iConrollable.Jump();
    }

    private void Update()
    {
        ReadMovement();
    }
    void ReadMovement()
    {   
        Vector2 joysticInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        joysticInput = joysticInput.normalized;
        Vector2 input = isMobile ? joysticInput : inputPlayer.Player.Move.ReadValue<Vector2>();

        // Получаем forward и right от камеры, но только в XZ плоскости
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Преобразуем input от игрока в мировые координаты относительно камеры
         move = forward * input.y + right * input.x;

        iConrollable.Move(move);
    }

    public void ResetInput()
    {
        iConrollable.Move(Vector3.zero);
        move = Vector3.zero;
    }
}

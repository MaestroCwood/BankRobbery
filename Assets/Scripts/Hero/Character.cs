using UnityEngine;

public class Character : MonoBehaviour, Icontrollable
{
    CharacterController characterController;
    Vector3 velocityY;
    Vector3 moveDirection;
    public Vector3 currentHorizontalMove;
    bool isGrounded;

    [SerializeField] float speed = 5f;
    [SerializeField] float rotateSpeedHero = 10f;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] Transform groundedCheck;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundMaskLayer;
    [SerializeField] Animator animator;

    public void Jump()
    {
        if (isGrounded)
        {
            velocityY.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
            
        }
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGrounded = CheckGroundedHero();
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalSpeed", velocityY.y);
        if (isGrounded && velocityY.y < 0f)
            velocityY.y = 0;
        else
            velocityY.y += gravity * Time.deltaTime;

        Vector3 totalMove = currentHorizontalMove;
        totalMove.y = velocityY.y;

        characterController.Move(totalMove * Time.deltaTime);
      

    }
    bool CheckGroundedHero()
    {
        bool grounded = Physics.CheckSphere(groundedCheck.position, groundCheckRadius, groundMaskLayer);
        return grounded;
    }

    public void Move (Vector3 direction)
    {
        currentHorizontalMove = direction * speed;
        float currentSpeed = currentHorizontalMove.magnitude;
        animator.SetFloat("Speed", currentSpeed);
        // Поворот
        if (direction != Vector3.zero)
        {
            Vector3 targetDirection = new Vector3(direction.x, 0f, direction.z).normalized;
            transform.forward = Vector3.Lerp(transform.forward, targetDirection, Time.deltaTime * rotateSpeedHero);
        }
    }

    void DoGravity()
    {
        if (isGrounded && velocityY.y < 0f)
        {
            velocityY.y = -2f; // мягкое "прилипание" к земле
        }
        else
        {
            velocityY.y += gravity * Time.fixedDeltaTime;
        }

        Vector3 gravityMove = new Vector3(0, velocityY.y, 0);
        characterController.Move(gravityMove * Time.fixedDeltaTime);

    }
}

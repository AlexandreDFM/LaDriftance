using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private float movementForce = 1f;

    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private float maxSpeed = 5f;

    [SerializeField] private Camera playerCamera;

    private Animator animator;

    private Vector3 forceDirection = Vector3.zero;

    //input fields
    //private ThirdPersonActionsAsset playerActionsAsset;
    private InputActionAsset inputAsset;
    private InputAction move;
    private InputActionMap playerActionMap;

    //movement fields
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        //playerActionsAsset = new ThirdPersonActionsAsset();
        inputAsset = GetComponent<PlayerInput>().actions;
        playerActionMap = inputAsset.FindActionMap("Player");
    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if (rb.linearVelocity.y < 0f)
            rb.linearVelocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        var horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.linearVelocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.linearVelocity.y;

        LookAt();
    }

    private void OnEnable()
    {
        //playerActionsAsset.Player.Jump.started += DoJump;
        //playerActionsAsset.Player.Attack.started += DoAttack;
        //move = playerActionsAsset.Player.Move;
        //playerActionsAsset.Player.Enable();
        playerActionMap.FindAction("Jump").started += DoJump;
        playerActionMap.FindAction("Attack").started += DoAttack;
        move = playerActionMap.FindAction("Move");
        playerActionMap.Enable();
    }

    private void OnDisable()
    {
        //playerActionsAsset.Player.Jump.started -= DoJump;
        //playerActionsAsset.Player.Attack.started -= DoAttack;
        //playerActionsAsset.Player.Disable();
        playerActionMap.FindAction("Jump").started -= DoJump;
        playerActionMap.FindAction("Attack").started -= DoAttack;
        playerActionMap.Disable();
    }

    private void LookAt()
    {
        var direction = rb.linearVelocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        var forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        var right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded()) forceDirection += Vector3.up * jumpForce;
    }

    private bool IsGrounded()
    {
        var ray = new Ray(transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out var hit, 0.5f))
            return true;
        return false;
    }

    private void DoAttack(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Attack");
    }
}
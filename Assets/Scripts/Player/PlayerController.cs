using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 12f;
    [SerializeField] private float gravity = -20f;

    private CharacterController characterController;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private Vector3 verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions.FindAction("Player/Move");

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (moveAction == null)
        {
            Debug.LogError("Could not find the Player/Move action.", this);
        }
    }

    private void Update()
    {
        if (moveAction == null)
        {
            return;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        MovePlayer(input);
    }

    private void MovePlayer(Vector2 input)
    {
        Vector3 forward = cameraTransform != null
            ? cameraTransform.forward
            : transform.forward;

        Vector3 right = cameraTransform != null
            ? cameraTransform.right
            : transform.right;

        forward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
        right = Vector3.ProjectOnPlane(right, Vector3.up).normalized;

        Vector3 moveDirection = forward * input.y + right * input.x;

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }

        if (characterController.isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -2f;
        }

        verticalVelocity.y += gravity * Time.deltaTime;

        Vector3 velocity = moveDirection * moveSpeed + verticalVelocity;
        characterController.Move(velocity * Time.deltaTime);
    }
}

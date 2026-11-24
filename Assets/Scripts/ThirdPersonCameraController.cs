using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float followHeight = 1.5f;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 60f;

    private InputAction lookAction;
    private float yaw;
    private float pitch;

    private void Awake()
    {
        if (target == null)
        {
            Debug.LogError("Assign the Player as the camera target.", this);
            enabled = false;
            return;
        }

        if (playerInput == null)
        {
            playerInput = target.GetComponent<PlayerInput>();
        }

        if (playerInput == null)
        {
            Debug.LogError("The Player needs a Player Input component.", this);
            enabled = false;
            return;
        }

        lookAction = playerInput.actions.FindAction("Player/Look");

        if (lookAction == null)
        {
            Debug.LogError("Could not find the Player/Look action.", this);
            enabled = false;
            return;
        }

        Vector3 startingAngles = transform.eulerAngles;
        yaw = startingAngles.y;
        pitch = startingAngles.x;

        if (pitch > 180f)
        {
            pitch -= 360f;
        }
    }

    private void Update()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void LateUpdate()
    {
        transform.position = target.position + Vector3.up * followHeight;
    }
}
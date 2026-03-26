using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMagicEffects : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider playerCollider;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private bool isGrounded;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 movementInput = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var handlers = GetComponents<ITriggerHandler>();
        foreach (var handler in handlers)
        {
            if (handler.CanHandle(other))
            {
                handler.Handle(other);
                break;
            }
        }
    }
}
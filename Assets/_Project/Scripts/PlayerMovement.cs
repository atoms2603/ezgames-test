using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float rotateSpeed = 10f;

    private PlayerController controller;
    private bool isMoving = false;

    public bool IsMoving => isMoving;
    private void Start()
    {
        joystick = FindAnyObjectByType<DynamicJoystick>(FindObjectsInactive.Include);
        controller = GetComponentInParent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameStarted) return;

        if (controller.IsKnocked) return;

        if (joystick.Direction.sqrMagnitude > 0.01f)
        {
            ManualMovement();
        }
        else
        {
            controller.Rigidbody.linearVelocity = Vector3.zero;
            isMoving = false;
            controller.Animator.SetBool("isWalking", isMoving);
        }
    }

    private void ManualMovement()
    {
        Vector2 inputDirection = joystick.Direction.normalized;
        Vector3 moveDirection = new Vector3(inputDirection.x, 0f, inputDirection.y);

        controller.Rigidbody.linearVelocity = moveDirection * speed;

        Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, toRotation, rotateSpeed * Time.fixedDeltaTime);

        isMoving = true;
        controller.Animator.SetBool("isWalking", isMoving);
    }

}

using Unity.AppUI.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private float sailSpeed = 5f;
    [SerializeField] private float turnSpeed = 5f;

    public GameObject Sail;
    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 momentum;
    private float waterDrag = 0.03f;
    private float sailExpansion = 100f;
    private quaternion sailRotation = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1000f;
        rb.linearDamping = 0f;
        rb.angularDamping = 1f;

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        //temp. Replace with sails and proper drag calculations
        momentum -= new Vector3 (drag, 0, drag);
        momentum += new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(momentum * Time.deltaTime);
    }
}

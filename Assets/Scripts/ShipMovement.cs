using Unity.AppUI.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private float sailSpeed = 5f;
    [SerializeField] private float turnSpeed = 5f;

    private CharacterController controller;
    private Vector3 moveInput;
    private Vector3 momentum = Vector3.zero;
    private float drag;
    void Start()
    {
        controller = GetComponent<CharacterController>();
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

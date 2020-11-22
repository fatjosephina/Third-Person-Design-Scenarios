using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RigidbodyCharacterController : MonoBehaviour
{
    [SerializeField]
    private float accelerationForce = 10f;

    [SerializeField]
    private float maxSpeed = 2f;

    [SerializeField]
    private PhysicMaterial stoppingPhysicMaterial, movingPhysicMaterial;

    [SerializeField]
    [Tooltip("0 = no turning, 1 = instant turning")]
    [Range (0, 1)]
    private float turnSpeed = 0.1f;

    private new Rigidbody rigidbody;
    private Vector2 input;
    private new Collider collider;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        Vector3 cameraRelativeInputDirection = GetCameraRelativeInputDirection();
        UpdatePhysicsMaterial();
        Move(cameraRelativeInputDirection);
        RotateToFaceMoveInputDirection(cameraRelativeInputDirection);
    }

    /// <summary>
    /// Turns the character to face the direction it wants to move in
    /// </summary>
    /// <param name="movementDirection">The direction the character is trying to move in</param>
    private void RotateToFaceMoveInputDirection(Vector3 movementDirection)
    {
        if (movementDirection.magnitude > 0)
        {
            var targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed);
        }
    }

    /// <summary>
    /// Moves the player in a direction based on its speed and acceleration
    /// </summary>
    /// <param name="moveDirection">The direction to move in</param>
    private void Move(Vector3 moveDirection)
    {
        if (rigidbody.velocity.magnitude < maxSpeed)
        {
            rigidbody.AddForce(moveDirection * accelerationForce, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Updates the physics material to a low friction option if the player is moving or a high friction option if they are
    /// trying to stop
    /// </summary>
    private void UpdatePhysicsMaterial()
    {
        collider.material = input.magnitude > 0 ? movingPhysicMaterial : stoppingPhysicMaterial;
    }

    /// <summary>
    /// Uses the input vector to create a camera relative version so the player can move based on the camera's forward
    /// </summary>
    /// <returns>Returns the camera relative input direction</returns>
    private Vector3 GetCameraRelativeInputDirection()
    {
        var inputDirection = new Vector3(input.x, 0, input.y);

        Vector3 cameraFlattenedForward = Camera.main.transform.forward;
        cameraFlattenedForward.y = 0;
        var cameraRotation = Quaternion.LookRotation(cameraFlattenedForward);

        Vector3 cameraRelativeInputDirectionToReturn = cameraRotation * inputDirection;
        return cameraRelativeInputDirectionToReturn;
    }

    /// <summary>
    /// This event handler is called from the PlayerInput component using the new input system
    /// </summary>
    /// <param name="context">Vector 2 representing the move input</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyCharacterController : MonoBehaviour
{
    [SerializeField]
    private float accelerationForce = 10f;

    [SerializeField]
    private float maxSpeed = 2f;

    [SerializeField]
    private PhysicMaterial stoppingPhysicMaterial, movingPhysicMaterial;

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
        var inputDirection = new Vector3(input.x, 0, input.y);

        Vector3 cameraFlattenedForward = Camera.main.transform.forward;
        cameraFlattenedForward.y = 0;
        var cameraRotation = Quaternion.LookRotation(cameraFlattenedForward);

        Vector3 cameraRelativeInputDirection = cameraRotation * inputDirection;
        collider.material = inputDirection.magnitude > 0 ? movingPhysicMaterial : stoppingPhysicMaterial;

        /*if (inputDirection.magnitude > 0)
        {
            collider.material = movingPhysicMaterial;
        }
        else
        {
            collider.material = stoppingPhysicMaterial;
        }*/

        if (rigidbody.velocity.magnitude < maxSpeed)
        {
            rigidbody.AddForce(cameraRelativeInputDirection * accelerationForce, ForceMode.Acceleration);
        }
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }
}

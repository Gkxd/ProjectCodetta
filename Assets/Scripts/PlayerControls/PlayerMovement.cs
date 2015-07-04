using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed;

    float horizontalAxis;
    float verticalAxis;

    public Rigidbody rigidBody;

    Transform headTransform;

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        headTransform = transform.Find("Head");
    }

    void FixedUpdate() {
        Vector3 movementDirection = (verticalAxis * headTransform.forward + horizontalAxis * headTransform.right);
        movementDirection.Scale(new Vector3(1, 0, 1)); // Project to horizontal plane
        movementDirection.Normalize();

        rigidBody.velocity = movementDirection * moveSpeed;
    }

    void Update() {
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        verticalAxis = Input.GetAxisRaw("Vertical");
    }
}

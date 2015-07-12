using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed;

    public Transform headTransform;

    float horizontalAxis;
    float verticalAxis;

    private bool explorationMode;

    void Start() {
        explorationMode = true;
    }

    void Update() {
        if (explorationMode) {
            horizontalAxis = Input.GetAxisRaw("Horizontal");
            verticalAxis = Input.GetAxisRaw("Vertical");

            Vector3 movementDirection = (verticalAxis * headTransform.forward + horizontalAxis * headTransform.right);
            movementDirection.Scale(new Vector3(1, 0, 1)); // Project to horizontal plane
            movementDirection.Normalize();

            transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    public void enterCombat() {
        explorationMode = false;
    }

    public void exitCombat() {
        explorationMode = true;
    }
}

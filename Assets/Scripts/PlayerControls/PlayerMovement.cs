using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed;

    public Transform headTransform;

    float horizontalAxis;
    float verticalAxis;

    private bool explorationMode;
    private Animator animator;

    void Start() {
        explorationMode = true;
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (!ModeSwitch.isCutscene()) {
            if (explorationMode) {
                horizontalAxis = Input.GetAxisRaw("Horizontal");
                verticalAxis = Input.GetAxisRaw("Vertical");

                Vector3 movementDirection = (verticalAxis * headTransform.forward + horizontalAxis * headTransform.right);
                movementDirection.Scale(new Vector3(1, 0, 1)); // Project to horizontal plane
                movementDirection.Normalize();

				if(movementDirection.sqrMagnitude > 0) {
					animator.SetBool("moving", true);
					gameObject.GetComponent<AudioSource>().volume = .5f;
				} else {
					animator.SetBool("moving", false);
					gameObject.GetComponent<AudioSource>().volume = 0f;
				}
                transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    public void enterCombat() {
        explorationMode = false;

        animator.SetBool("moving", false);
		gameObject.GetComponent<AudioSource>().volume = 0f;
        animator.SetBool("inCombat", true);
    }

    public void exitCombat() {
        animator.SetBool("inCombat", false);
        explorationMode = true;
    }
}

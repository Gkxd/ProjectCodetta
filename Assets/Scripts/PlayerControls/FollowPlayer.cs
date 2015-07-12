using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    public GameObject player;
    public GameObject followPosition;
    public GameObject combatPosition;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private bool needsToFaceForward;
    private bool explorationMode;

	void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        needsToFaceForward = false;
        explorationMode = true;

        animator = gameObject.GetComponent<Animator>();
	}

    void Update() {
        if (explorationMode) {
            Vector3 vectorToPlayer = transform.position - player.transform.position;
            vectorToPlayer.Scale(new Vector3(1, 0, 1));

            if (vectorToPlayer.sqrMagnitude > 4) {
                navMeshAgent.SetDestination(followPosition.transform.position);
            }


            animator.SetBool("moving", navMeshAgent.velocity.magnitude > 0);
        }
        else {
            if (needsToFaceForward) {
                if ((combatPosition.transform.position - transform.position).sqrMagnitude < 0.01f) {
                    StartCoroutine(turnForward());
                }
            }
        }
    }

    private IEnumerator turnForward() {
        while ((transform.forward - player.transform.forward).sqrMagnitude > 0.0001f) {
            Vector3 lookVector = Vector3.Slerp(transform.forward, player.transform.forward, 0.1f);
            transform.LookAt(transform.position + lookVector);

            yield return null;
        }

        needsToFaceForward = false;
    }

    public void enterCombat() {
        animator.SetBool("moving", false);
        animator.SetBool("inCombat", true);

        explorationMode = false;
        enterCombatPosition();
    }

    public void exitCombat() {
        animator.SetBool("inCombat", false);
        explorationMode = true;
    }

    public void enterCombatPosition() {
        navMeshAgent.SetDestination(combatPosition.transform.position);
        needsToFaceForward = true;
    }
}

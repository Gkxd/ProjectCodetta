using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    public GameObject player;
    public GameObject followObject;

    private NavMeshAgent navMeshAgent;

	void Start () {
        //player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
	}

    void FixedUpdate() {
        Vector3 vectorToPlayer = transform.position - player.transform.position;
        vectorToPlayer.Scale(new Vector3(1, 0, 1));

        if (vectorToPlayer.sqrMagnitude > 1) {
            navMeshAgent.SetDestination(followObject.transform.position);
        }
    }
}

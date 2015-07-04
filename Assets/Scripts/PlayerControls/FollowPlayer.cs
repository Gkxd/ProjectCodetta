using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    private GameObject player;
    private Transform playerTransform;
    private Rigidbody playerRigidbody;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform.Find("Head");
	}

    void FixedUpdate() {

    }

	// Update is called once per frame
	void Update () {
	
	}
}

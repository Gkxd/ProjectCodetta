using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    public GameObject player;
    public Rigidbody playerRigidbody;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}

    void FixedUpdate() {

    }

	// Update is called once per frame
	void Update () {
	
	}
}

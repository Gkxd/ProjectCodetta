using UnityEngine;
using System.Collections;

public class whaleAnimOffset : MonoBehaviour {
	private float waitWhaleSound = 0f;
	void Start () {
		gameObject.GetComponent<Animator>().Play("swim", -1, Random.Range(0.0f,0.3f));
	}
	void Update() {
		if(gameObject.GetComponent<AudioSource>() != null) {
			if(Time.time > waitWhaleSound) {
				gameObject.GetComponent<AudioSource>().pitch = Random.Range(0.5f,1f);
				gameObject.GetComponent<AudioSource>().Play();
				waitWhaleSound = Time.time + Random.Range(5f,15f);
			}
		}
	}
}

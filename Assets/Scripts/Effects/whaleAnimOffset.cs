using UnityEngine;
using System.Collections;

public class whaleAnimOffset : MonoBehaviour {
	void Start () {
		gameObject.GetComponent<Animator>().Play("swim", -1, Random.Range(0.0f,0.3f));
	}
}

using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {

	public string level;

	IEnumerator Start() {
		AsyncOperation async = Application.LoadLevelAsync(level);
		yield return async;
		Debug.Log("Loading complete");
	}
}


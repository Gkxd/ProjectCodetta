using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour {

	private Vector2[] texCoord;

	private float lightningLife = 0.0f;
	public float lightningLifeTime;

	private float spawnTimer = 0.0f;
	public float spawnTimeMin;
	public float spawnTimeMax;

	public Transform anchor;
	public Transform cam;

	public ParticleSystem sparks;
	public ParticleSystem cloud;
	private bool sparked;

	void Start () {
		texCoord = new Vector2[4];
		texCoord[0] = new Vector2(0.0f,0.0f);
		texCoord[1] = new Vector2(0.0f,0.5f);
		texCoord[2] = new Vector2(0.5f,0.0f);
		texCoord[3] = new Vector2(0.5f,0.5f);
	}

	void Update () {

		//every time lightning is sparked:
		if (spawnTimer <= 0.0f) {
			//look at cam
			anchor.LookAt(new Vector3(cam.position.x,transform.position.y,cam.position.z));
			//choose texture
			int r = Random.Range(0,4);
			gameObject.GetComponent<Renderer>().material.mainTextureOffset = texCoord[r];
		    //start life
			lightningLife = lightningLifeTime;
			//play clouds
			cloud.Play();
			//restart the spawn timer
			spawnTimer = Random.Range(spawnTimeMin,spawnTimeMax);
			sparked = false;
		} else {	
			spawnTimer -= Time.deltaTime;
		}

		//when lightning is still alive
		if (lightningLife >= 0.0f) {
			if(lightningLife <= lightningLifeTime*0.5f && !sparked) {
				//show Lightning
				if(Random.Range(0,2) == 0) {
					gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(0.5f,0.5f);
				} else {
					gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(-0.5f,0.5f);
				}
				gameObject.GetComponent<AudioSource>().pitch = Random.Range(0.5f,1f);
				gameObject.GetComponent<AudioSource>().Play();
				sparked = true;
				sparks.Play();
			}
			lightningLife -= Time.deltaTime;
		} else {
			gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(0.0f,0.0f);
		}
	}
}

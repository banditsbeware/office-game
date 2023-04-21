using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//will set an RTPC value to between 0 and 2, centered around 1.
public class dopplerEffect : MonoBehaviour
{
	public float SpeedOfSound = 343.3f;
	public float DopplerFactor = 3.0f;
	Vector3 emitterLastPosition = Vector3.zero;
	Vector3 listenerLastPosition = Vector3.zero;
  GameObject player;
	
	private void Start() 
	{
		player = GameObject.Find("Player");
	}

	private void FixedUpdate () 
	{
		//get velocity of source/emitter manually
		Vector3 emitterSpeed = (emitterLastPosition - transform.position) / Time.fixedDeltaTime;
		emitterLastPosition = transform.position;
	
		//get velocity of listener/player manually
		Vector3 listenerSpeed = (listenerLastPosition - player.transform.position) / Time.fixedDeltaTime;
		listenerLastPosition = player.transform.position;
		
		//do doppler calc
		var distance = (player.transform.position - transform.position); // source to listener vector
		var listenerRelativeSpeed = Vector3.Dot(distance, listenerSpeed) / distance.magnitude;
		var emitterRelativeSpeed = Vector3.Dot(distance, emitterSpeed) / distance.magnitude;
		listenerRelativeSpeed = Mathf.Min (listenerRelativeSpeed, (SpeedOfSound / DopplerFactor));
		emitterRelativeSpeed = Mathf.Min (emitterRelativeSpeed, (SpeedOfSound / DopplerFactor));
		var dopplerPitch = (SpeedOfSound + (listenerRelativeSpeed * DopplerFactor)) / (SpeedOfSound + (emitterRelativeSpeed * DopplerFactor));

		// pass dopplerPitch to an RTPC in Wwise
		AkSoundEngine.SetRTPCValue("dopplerNum", dopplerPitch, gameObject); 
  }
}

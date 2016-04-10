using UnityEngine;
using System.Collections;

public class CarCollisions : MonoBehaviour {

	public Rigidbody rigid;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collide)
	{
		if (collide.collider.name == "Car Body") {
			collide.rigidbody.AddForce (rigid.velocity * 100000);
			Debug.Log ("TWAT IT");
		} else {
			Debug.Log ("TWAT THE OTHER ONE");
		}
	}

	void OnTriggerEnter(Collider collide)
	{
		if (collide.name == "Car Body") {
			
			collide.attachedRigidbody.AddForce (rigid.transform.GetComponent<SimpleCarController>().CachedVelocity * 30000);
			collide.attachedRigidbody.GetComponent<NetworkCharacterController> ().JustHit ();
			Debug.Log ("TWAT IT TRUGGE");
		} else {
			Debug.Log ("TWAT THE OTHER ONE TRUGGE");
		}
	}
}

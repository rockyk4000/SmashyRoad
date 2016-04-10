using UnityEngine;
using System.Collections;

public class NetworkCharacterController : Photon.MonoBehaviour {

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private Vector3 correctVeloctity;
	private int steering;
	private bool breakDown;
	public Rigidbody body;
	int CountBeforeForcePos = 0;
	public SimpleCarController carController;

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			//we controll this one, send to others
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
			stream.SendNext (body.velocity);
			stream.SendNext (carController.Steering);
			stream.SendNext (carController.BreakDown);

			//Debug.Log ("SEND STEER " + carController.Steering + " BREAK " + carController.BreakDown);
		} else {
			//dont controll so receive pos


			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext ();
			correctVeloctity = (Vector3)stream.ReceiveNext ();
			steering = (int)stream.ReceiveNext ();
			breakDown = (bool)stream.ReceiveNext ();
			if (CountBeforeForcePos == 0) {

				//if ((transform.position - correctPlayerPos).magnitude > 10) {
					transform.position = correctPlayerPos;
				//}

				//if ((transform.rotation.eulerAngles - correctPlayerRot.eulerAngles).magnitude > 10) {
					transform.rotation = correctPlayerRot;
				//}
				CountBeforeForcePos += 1;
			
				
				
			} else {
				CountBeforeForcePos += 1;
				if (CountBeforeForcePos > 100) {
					//CountBeforeForcePos = 0;
				}
			}

			if (Time.time - hitTime >0.5f) {
				if ((transform.position - correctPlayerPos).magnitude > 50) {
					transform.position = correctPlayerPos;
				}
			}

			transform.GetComponent<SimpleCarController> ().CachedVelocity = correctVeloctity;
			body.velocity = correctVeloctity;

			updateTime = Time.time;
		}
	}

	float hitTime = -10;
	public void JustHit()
	{
		Debug.Log ("Got the hit through");
		hitTime = Time.time;
	}
	float updateTime;
	void FixedUpdate()
	{
		if (!photonView.isMine) {
			//body.velocity = correctVeloctity;
		//	body.MovePosition (correctPlayerPos);
			//transform.Translate ((  correctPlayerPos-transform.position)* Time.deltaTime, Space.World);
			//transform.position = Vector3.Lerp (transform.position, correctPlayerPos, Time.deltaTime * 5);
			//transform.rotation = Quaternion.Lerp (transform.rotation, correctPlayerRot, Time.deltaTime * 5);

			if (Time.time - hitTime > 0.5f) {
				Vector3 projectedPosition = this.correctPlayerPos + body.velocity * (Time.time - updateTime);
				transform.position = Vector3.Lerp (transform.position, projectedPosition, Time.deltaTime * 4);

			
			}
			transform.rotation = Quaternion.Lerp (transform.rotation, this.correctPlayerRot, Time.deltaTime * 4);
			transform.GetComponent<SimpleCarController> ().Steering = steering;
			transform.GetComponent<SimpleCarController> ().BreakDown = breakDown;
			//Debug.Log ("STEER " + steering + " BREAK " + breakDown);
		}

	}
}

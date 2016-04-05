using UnityEngine;
using System.Collections;

public class NetworkCharacterController : Photon.MonoBehaviour {

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			//we controll this one, send to others
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
		} else {
			//dont controll so receive pos
			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext ();
		}
	}

	void Update()
	{
		if (!photonView.isMine) {

			transform.position = Vector3.Lerp (transform.position, correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp (transform.rotation, correctPlayerRot, Time.deltaTime * 5);
		}

	}
}

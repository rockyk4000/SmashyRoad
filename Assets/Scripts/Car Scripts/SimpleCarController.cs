using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;
	public bool steering;
}

public class SimpleCarController : MonoBehaviour {
	public List<AxleInfo> axleInfos; 
	public float maxMotorTorque;
	public float maxSteeringAngle;
	public bool IsControllable = false;

	// finds the corresponding visual wheel
	// correctly applies the transform
	public void ApplyLocalPositionToVisuals(WheelCollider collider)
	{
		if (collider.transform.childCount == 0) {
			return;
		}

		Transform visualWheel = collider.transform.GetChild(0);

		Vector3 position;
		Quaternion rotation;
		collider.GetWorldPose(out position, out rotation);

		visualWheel.transform.position = position;
		visualWheel.transform.rotation = rotation;

		Rigidbody body;
	}

	public void Start()
	{
		rigid.centerOfMass += new Vector3 (0, 0, 0.1f);

	}

	public void FixedUpdate()
	{


		if (IsControllable) {


			float motor = maxMotorTorque * Input.GetAxis ("Vertical");
			float steering = maxSteeringAngle * Input.GetAxis ("Horizontal");

			foreach (AxleInfo axleInfo in axleInfos) {
				if (axleInfo.steering) {
					axleInfo.leftWheel.steerAngle = steering;
					axleInfo.rightWheel.steerAngle = steering;
				}
				if (axleInfo.motor) {
					axleInfo.leftWheel.motorTorque = motor;
					axleInfo.rightWheel.motorTorque = motor;
				}
				ApplyLocalPositionToVisuals (axleInfo.leftWheel);
				ApplyLocalPositionToVisuals (axleInfo.rightWheel);

				RollUpdate (axleInfo.leftWheel, axleInfo.rightWheel);
			}

			if (rigid.velocity.magnitude < 15 && Input.GetAxis ("Vertical") > 0) {
				Debug.Log ("SPEEEED " + rigid.velocity.magnitude);
				rigid.AddRelativeForce (Vector3.forward * 20000);
			}
		}
	}
	public void Update()
	{
		Camera.main.transform.position = new Vector3 (transform.position.x, 35, transform.position.z - 20);
		Camera.main.transform.LookAt (transform.position);
	}

	public void RollUpdate(WheelCollider WheelL, WheelCollider WheelR)
	{

		float travelL = 1.0f;
		float travelR = 1.0f;
		WheelHit hit;
		bool groundedL = WheelL.GetGroundHit (out hit);
		if (groundedL)
			travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

		bool groundedR = WheelL.GetGroundHit (out hit);		
		if (groundedR)
			travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

		float antiRollForce =  AntiRoll;
	
		if (groundedL)
			rigid.AddForceAtPosition(WheelL.transform.up * -antiRollForce,
				WheelL.transform.position); 
		if (groundedR)
			rigid.AddForceAtPosition(WheelR.transform.up * -antiRollForce,
				WheelR.transform.position); 
	}


	public float AntiRoll = 15000.0f;
	public Rigidbody rigid;

		



}
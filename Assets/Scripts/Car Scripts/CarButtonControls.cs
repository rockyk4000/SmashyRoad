using UnityEngine;
using System.Collections;

public class CarButtonControls : MonoBehaviour {

	public int SteerDirection = 0;
	public  bool BreakDown = false;
	public bool ResetPos = false;

	public bool LeftBreakDown = false;
	public bool RightBreakDown = false;

	public  void LeftButtonDown()
	{
		SteerDirection = -1;
		Debug.Log ("Left");
	}

	public  void LeftButtonUp()
	{
		SteerDirection = 0;
		Debug.Log ("Left UP");
	}

	public  void RightButtonDown()
	{
		SteerDirection = 1;
		Debug.Log ("Right");
	}

	public  void RightButtonUp()
	{
		SteerDirection = 0;
		Debug.Log ("Right UP");
	}

	public void LeftBreakButtonDown()
	{
		BreakDown = true;
		LeftBreakDown = true;
	}

	public void RightBreakButtonDown()
	{
		BreakDown = true;
		RightBreakDown = true;
	}

	public void LeftBreakButtonUp()
	{
		if (!RightBreakDown) {
			BreakDown = false;
		}
		LeftBreakDown = false;

	}
	public void RightBreakButtonUp()
	{
		if (!LeftBreakDown) {
			BreakDown = false;
		}
		RightBreakDown = false;
	}



	public void ResetPosClick()
	{
		ResetPos = true;
	}


}

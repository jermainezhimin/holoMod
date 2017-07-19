using UnityEngine;
using System.Collections;

public class ButterflyController : MonoBehaviour {

	public GestureController information;
	public GameObject leftWing;
	public GameObject rightWing;
	private float leftScore;
	private float rightScore;

	void Start () {
		leftWing.transform.eulerAngles = new Vector3 (-40f, 20f, 0f);
		rightWing.transform.eulerAngles = new Vector3(40f, -20f ,0f);
	}

	// Update is called once per frame
	void Update () {
		leftScore = information.rotationL - 100;
		rightScore = information.rotationR - 100;
	
		if (leftScore > 0) {
			float leftCal = (leftScore*-1) + 20;
			leftWing.transform.eulerAngles = new Vector3(-40f, leftCal ,0f);
		} else if (leftScore < 0) {
			float leftCal =  (leftScore*-1) + 20;
			leftWing.transform.eulerAngles  = new Vector3(-40f, leftCal ,0f);
		}

		if (rightScore > 0) {
			float rightCal = (rightScore) - 20;
			rightWing.transform.eulerAngles = new Vector3(-15f, rightCal ,0f);
		} else if (rightScore < 0) {
			float rightCal =  (rightScore) - 20;
			rightWing.transform.eulerAngles  = new Vector3(-15f, rightCal ,0f);
		}

	}
}

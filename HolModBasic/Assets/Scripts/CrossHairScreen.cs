using DG.Tweening;
using UnityEngine;
using System.Collections;

public class CrossHairScreen : MonoBehaviour {

	public GameObject planeParent;
	public GameObject[] crossHair;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Transform[] childrenPlane = planeParent.GetComponentsInChildren<Transform>();
		if (childrenPlane.Length == 2) {
			foreach (GameObject obj in crossHair) {
				obj.GetComponent<Transform> ().DOScale (new Vector3 (0f, 0f, 0f), 0.5f);
			}
		} else {
			foreach (GameObject obj in crossHair) {
				obj.GetComponent<Transform> ().DOScale (new Vector3 (1f, 2f, 1f), 0.5f);
			}
		}
	}
}

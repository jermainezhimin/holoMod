using DG.Tweening;

using Leap;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;

public class ModelScreen : MonoBehaviour {


	public MeshFilter MF;
	public GameObject item;
	public GameObject selector;
	public GameObject handController;
	public GameObject zoomButton;
	public GameObject subButton;
	public GameObject subText;
	public GameObject brushButton;
	public GameObject sphere;
	public int zoomToggle = 0;
	public int brushToggle = 0;
	private float zPos = 0;
	private float zSpeed = 0;
	private float distOrigin = 0;

	//For the actual modeling
	private Vector3 dirOrigin;
	private Vector3 dirDirection;
	private Vector3 hitOrigin;
	private float pullMag;
	int layerMask = (1<<8);

	public void subdivide(){
		Mesh m = MF.mesh;
		Debug.Log (2 * Mathf.Pow (m.vertexCount, 0.5f));
		if ( 2* Mathf.Pow (m.vertexCount, 0.5f) < 75) {
			MeshHelper.Subdivide (m);
			MF.mesh = m;
			item.GetComponent<MeshCollider> ().sharedMesh = m;
		}else {
			subButton.GetComponent<Button> ().interactable = false;
		}
	}

	public void zoom(){
		if (zoomToggle == 0) {
			zoomToggle = 1;
			zoomButton.GetComponent<UnityEngine.UI.Image> ().DOColor(new Color(0.312f,0.807f,0.219f,1.0f),0.5f).SetEase(Ease.OutExpo);
			subButton.GetComponent<Button>().interactable = false;
			brushButton.GetComponent<Button>().interactable = false;

		} else {
			zoomToggle = 0;
			zoomButton.GetComponent<UnityEngine.UI.Image> ().DOColor(new Color(1.0f,1.0f,1.0f,1.0f),0.5f).SetEase(Ease.OutExpo);
			subButton.GetComponent<Button>().interactable = true;
			brushButton.GetComponent<Button>().interactable = true;
		}
	}

	public void brush(){
		if (brushToggle == 0) {
			brushToggle = 1;
			brushButton.GetComponent<UnityEngine.UI.Image> ().DOColor(new Color(0.312f,0.807f,0.219f,1.0f),0.5f).SetEase(Ease.OutExpo);
			subButton.GetComponent<Button>().interactable = false;
			zoomButton.GetComponent<Button>().interactable = false;

		} else if (brushToggle == 1) {
			brushToggle = 2;
			brushButton.GetComponent<UnityEngine.UI.Image> ().DOColor(new Color(0.854f,0.423f,0.333f,1.0f),0.5f).SetEase(Ease.OutExpo);
			subButton.GetComponent<Button> ().interactable = false;
			zoomButton.GetComponent<Button> ().interactable = false;
		} else {
			brushToggle = 0;
			brushButton.GetComponent<UnityEngine.UI.Image> ().DOColor(new Color(1.0f,1.0f,1.0f,1.0f),0.5f).SetEase(Ease.OutExpo);
			subButton.GetComponent<Button>().interactable = true;
			zoomButton.GetComponent<Button>().interactable = true;
		}
	}

	// Update is called once per frame
	void Update () {

		subText.GetComponent<Text> ().text = MF.mesh.vertexCount.ToString();

		if ((zoomToggle == 1) && (handController.GetComponent<GestureController>().pinchStrengthL > 0.9)) {
			zSpeed = handController.GetComponent<GestureController> ().leftHandPosition.z - zPos;
			item.transform.localScale = new Vector3(zSpeed*0.0001f + item.transform.localScale.z,
				zSpeed*0.0001f + item.transform.localScale.y,
				zSpeed*0.0001f + item.transform.localScale.z);
			zPos = zSpeed;
			zSpeed = 0;
		}

		if ((brushToggle == 1)||(brushToggle == 2)) {
			sphere.SetActive (true);
			foreach (var h in handController.GetComponent<HandController>().GetFrame().Hands) {
				if (h.IsLeft) {
					dirOrigin = h.PalmPosition.ToUnity(); 
					dirDirection = -h.PalmPosition.ToUnityScaled ();
					pullMag = dirOrigin.magnitude;
				}
			}

			RaycastHit hit;
			//Debug.DrawRay (dirOrigin, dirDirection,Color.red,0.1f);
			if (Physics.Raycast (dirOrigin, dirDirection, out hit, Mathf.Infinity, layerMask)) {
				hitOrigin = hit.point;
				sphere.transform.position = hitOrigin;
			}	

			if (handController.GetComponent<GestureController> ().pinchStrengthL == 1) {
				Vector3[] vertices = item.GetComponent<MeshFilter> ().mesh.vertices;
				for (int a = 0; a < item.GetComponent<MeshFilter> ().mesh.vertexCount; a++) {

					float selectionScale = sphere.GetComponent<Transform> ().localScale.x;
					Vector3 scaledVertices = new Vector3 ((vertices [a].x * item.transform.localScale.x), 
						(vertices [a].y * item.transform.localScale.y), 
						(vertices [a].z * item.transform.localScale.z));
					float circleValue = (sphere.transform.position - item.transform.rotation*scaledVertices ).magnitude;

					if (circleValue < selectionScale) {
						if (brushToggle == 1) {
							vertices [a] = vertices [a] + Vector3.Scale(vertices[a],new Vector3(0.01f,0.01f,0.01f));
						} else if (brushToggle == 2){
							vertices [a] = vertices [a] - Vector3.Scale(vertices[a],new Vector3(0.01f,0.01f,0.01f));
						}

					}
				}
				MF.mesh.vertices = vertices;
				MF.mesh.RecalculateNormals();
				item.GetComponent<MeshCollider> ().sharedMesh = MF.mesh;
			}

		} else if (brushToggle == 0) {
			sphere.SetActive (false);
		}
	}
}
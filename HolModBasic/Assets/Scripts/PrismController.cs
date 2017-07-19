using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PrismController : MonoBehaviour {

	public CanvasController canvas;
	public GameObject inputText;
	public GameObject warningText;
	public GameObject planesParent;
	public GameObject cameraParent;

	public void checkInput() {
		
		try{
			int sides = Int32.Parse (inputText.GetComponent<Text>().text);

			//Making sure allowing input as mentioned in warning text is enforced
			if ((sides >= 1) && (sides <=9)) {

				canvas.outroPrismScreen();

				//Rubbish collection for scene items
				for (int a = 0; a < planesParent.transform.childCount; a++) {
				if (a == planesParent.transform.childCount - 1) {
						planesParent.transform.GetChild (a).transform.DOScale (new Vector3 (0f, 0f, 0f), 0.25f).OnComplete (clearScene);
					} else {
						planesParent.transform.GetChild (a).transform.DOScale (new Vector3 (0f, 0f, 0f), 0.25f);
					}
				}

				//Coroutines allow us to use delays
				StartCoroutine (generatorSides(sides));

			} else {
				warningAnimation ();
			}
		}
		catch{
			warningAnimation ();
		}
	}

	//Animation For Wrong Input
	//Used in checkInput()
	void warningAnimation(){
		float alpha = warningText.GetComponent<Text> ().color.a;
		if (alpha < 1) {
			warningText.GetComponent<Text> ().DOFade (1, 0.5f).SetEase (Ease.OutExpo);
		} else {
			Vector3 warningTextPos = warningText.transform.position;
			warningText.GetComponent<Text> ().DOFade (0.25f, 0.1f).SetEase (Ease.Linear);
			warningText.GetComponent<Text> ().DOFade (1.0f, 0.1f).SetEase (Ease.Linear).SetDelay(0.1f);
			warningText.GetComponent<Text> ().DOFade (0.25f, 0.1f).SetEase (Ease.Linear).SetDelay(0.2f);
			warningText.GetComponent<Text> ().DOFade (1.0f, 0.1f).SetEase (Ease.Linear).SetDelay(0.3f);
		}
	}
		
	//Clear scene by rubbish collecting the objects for destruction
	void clearScene(){
		foreach (Transform child in planesParent.transform) {
			Destroy(child.gameObject);
		}
		foreach (Transform child in cameraParent.transform) {
			Destroy(child.gameObject);
		}
	}

	//Uses cirle properties to configure the different positions
	//x = Rcos (Teta); 
	//y = Rcos (Teta);
	//Includes a delay sequence to ensure ruubbish collection is complete

	IEnumerator generatorSides(int sides) {
		yield return new WaitForSeconds (0.5f);

		//Required objects
		GameObject[] cCams;
		GameObject[] cPlanes;
		Material[] cMats;
		RenderTexture[] cRender;

		//Ensuring arrays empty
		cCams = new GameObject[sides];
		cPlanes = new GameObject[sides];
		cMats = new Material[sides];
		cRender = new RenderTexture[sides];

		float pRadius = 5f;
		float cRadius = 2f;
		float degSep = (2f*Mathf.PI) / sides;
		float degStart = 0f;

		if (sides > 1) {
			for (int a=0; a<sides; a++){

				float xCam = cRadius * Mathf.Sin(degStart);
				float zCam = cRadius * Mathf.Cos(degStart);
				float xPlane = pRadius * Mathf.Sin(degStart);
				float zPlane = pRadius * Mathf.Cos(degStart);
				degStart += degSep;

				cRender[a] = new RenderTexture(512, 512, 24, RenderTextureFormat.ARGB32);

				cMats [a] = new Material (Shader.Find("Mobile/Diffuse"));
				cMats [a].mainTexture = cRender [a];

				cCams[a] = new GameObject("Camera " + a);
				cCams[a].transform.parent = cameraParent.transform;
				cCams [a].tag = "SubCamera";
				cCams [a].layer = 8;
				cCams [a].AddComponent<Camera> ();
				cCams [a].GetComponent<Camera> ().backgroundColor = new Color(0f,0f,0f,0f);
				cCams [a].GetComponent<Camera> ().clearFlags = CameraClearFlags.SolidColor;
				cCams [a].GetComponent<Camera> ().targetTexture = cRender[a];
				cCams [a].transform.localPosition = new Vector3 (xCam, 0f, zCam);
				cCams [a].transform.LookAt(new Vector3 (0f, 0f, 0f));
				cCams [a].GetComponent<Camera>().cullingMask = ~(1<<9);
			  

				cPlanes[a] = GameObject.CreatePrimitive(PrimitiveType.Plane);
				cPlanes[a].name = "Plane " + a;
				cPlanes [a].GetComponent<MeshRenderer> ().material = cMats [a];
				cPlanes [a].transform.parent = planesParent.transform;
				cPlanes [a].transform.localPosition = new Vector3 (xPlane, 0f,zPlane);
				cPlanes [a].transform.localScale = new Vector3 (0f, 0f, 0f);
				cPlanes [a].transform.DOScale (new Vector3 (-1*0.5f*(4f/sides), 0.5f*(4f/sides), 0.5f*(4f/sides)), 0.25f); 
				cPlanes [a].transform.LookAt(new Vector3 (0f, 0f, 0f));
				cPlanes [a].layer = 9;
			}

		} else if (sides == 1){
			degStart = -Mathf.PI;
			float xCam = cRadius * Mathf.Sin(degStart);
			float zCam = cRadius * Mathf.Cos(degStart);
			float xPlane = pRadius * Mathf.Sin(degStart);
			float zPlane = pRadius * Mathf.Cos(degStart);


			cRender[0] = new RenderTexture(1024, 1024, 24, RenderTextureFormat.ARGB32);

			cMats [0] = new Material (Shader.Find("Mobile/Diffuse"));
			cMats [0].mainTexture = cRender [0];

			cCams[0] = new GameObject("Camera 0");
			cCams[0].transform.parent = cameraParent.transform;
			cCams [0].tag = "SubCamera";
			cCams [0].layer = 8;
			cCams [0].AddComponent<Camera> ();
			cCams [0].GetComponent<Camera> ().backgroundColor = new Color(0f,0f,0f,0f);
			cCams [0].GetComponent<Camera> ().clearFlags = CameraClearFlags.SolidColor;
			cCams [0].GetComponent<Camera> ().targetTexture = cRender[0];
			cCams [0].transform.localPosition = new Vector3 (xCam, 0f, zCam);
			cCams [0].transform.LookAt(new Vector3 (0f, 0f, 1f));
			cCams [0].GetComponent<Camera>().cullingMask = ~(1<<9);

			cPlanes [0] = GameObject.CreatePrimitive(PrimitiveType.Plane);
			cPlanes [0].name = "Plane 0";
			cPlanes [0].GetComponent<MeshRenderer> ().material = cMats [0];
			cPlanes [0].transform.parent = planesParent.transform;
			cPlanes [0].transform.localPosition = new Vector3 (0f, 0f, 0f);
			cPlanes [0].transform.localScale = new Vector3 (0f, 0f, 0f);
			cPlanes [0].transform.DOScale (new Vector3 (-1*0.5f*(4f/sides), 0.5f*(4f/sides), 0.5f*(4f/sides)), 0.25f);; 
			cPlanes [0].transform.LookAt(new Vector3 (0f, 0f, 1f));
			cPlanes [0].layer = 9;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}

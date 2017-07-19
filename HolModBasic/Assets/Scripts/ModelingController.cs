using UnityExtension;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ModelingController : MonoBehaviour {

	public GameObject canvas;
	public GameObject item;
	public Text importText;
	public GameObject importWarning;
	Dictionary <int,PrimitiveType> primDict = new Dictionary<int, PrimitiveType>() { {0,PrimitiveType.Capsule}, {1, PrimitiveType.Cube}, {2, PrimitiveType.Cylinder},{3,PrimitiveType.Sphere} };
	int curPrim = 1;

	public void importObject() {
		try{
			var lStream = new FileStream (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Desktop) + "/" + importText.text, FileMode.Open);
			var lOBJData = OBJLoader.LoadOBJ (lStream);
			item.GetComponent<MeshFilter> ().mesh.LoadOBJ (lOBJData);
			item.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();	
			item.GetComponent<MeshCollider> ().sharedMesh = item.GetComponent<MeshFilter> ().mesh;
			lStream.Close ();
			canvas.GetComponent<CanvasController>().outroDownloadScreen();
		} catch (FileNotFoundException){
			warningAnimation (importWarning);
		} catch(UnauthorizedAccessException){
			warningAnimation (importWarning);
		}
	}

	void warningAnimation(GameObject warningText){
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


	public void primitiveSwapper(){
		curPrim += 1;
		curPrim = curPrim % 4;
		item.GetComponent<MeshFilter> ().mesh = PrimitiveHelper.GetPrimitiveMesh (primDict[curPrim]);
		item.GetComponent<MeshCollider> ().sharedMesh = item.GetComponent<MeshFilter> ().mesh;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using UnityExtension;
using UnityEngine.UI;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class SettingController : MonoBehaviour {

	public CanvasController canvas;
	public GameObject item;
	public Text exportText;
	public GameObject exportWarning; 


	public void exportObject() {
		if (exportText.text.Length == 0) {
			warningAnimation (exportWarning);
		} else {
			ObjExporter.MeshToFile (item.GetComponent<MeshFilter> (), exportText.text);
			canvas.outroUploadScreen();
		}
	}

	public void exit(){
		Application.Quit ();
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


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

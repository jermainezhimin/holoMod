using UnityEngine;
using UnityExtension;
using UnityEngine.UI;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CanvasController : MonoBehaviour {

	//Initializing required game objects
	//Starting Screen
	public GameObject ball;
	public GameObject loadingBar;
	public GameObject startScreen;

	//Screen
	public GameObject prismScreen;
	public GameObject uploadScreen;
	public GameObject downloadScreen;
	public GameObject[] simulationScreen;
	public GameObject[] settingScreen;
	public GameObject[] importScreen;
	public GameObject[] modelScreen;
	public GameObject[] progressScreen;


	//Objects
	public GameObject[] reprojectionPlanes;

	//Routine to create the Starting Screen Animations And Functions
	void bounceUp (){ball.transform.DOLocalMoveY (0, 0.5f).SetEase(Ease.OutQuint).OnComplete(bounceDown);}
	void bounceDown (){ball.transform.DOLocalMoveY (-20, 0.5f).SetEase(Ease.InQuint).OnComplete(bounceUp);}
	void deleteStartScreen(){startScreen.SetActive (false);}
	void outroStartScreen(){
		startScreen.transform.DOScale (new Vector3 (0, 0, 0), 1f).SetEase (Ease.OutExpo).OnComplete(deleteStartScreen);
		introPrismScreen ();
		introReprojectionScreen();
		introProgressScreen();
		introImportScreen();
		introSettingScreen ();
	}

	//Routine to create the Reprojection Screen Animations And Functions
	void introReprojectionScreen(){
		for (int a = 0; a < reprojectionPlanes.Length; a++) {
			reprojectionPlanes [a].transform.DOScale (new Vector3 (0.5f, 0.5f, 0.5f), 0.5f).SetEase (Ease.OutExpo);
		}
	}

	//Routine to create the simulation Screen Animations And Functions
	public void introSimulationScreen(){
		for (int a = 0; a<simulationScreen.Length; a++){
			//Debug.Log (importScreenLeft [a].transform.localPosition);
			simulationScreen [a].transform.DOLocalMoveX (0, 1).SetEase(Ease.OutExpo);
		}
	}

	public void outroSimulationScreen(){
		for (int a = 0; a<simulationScreen.Length; a++){
			//Debug.Log (importScreenLeft [a].transform.localPosition);
			simulationScreen [a].transform.DOLocalMoveX (-200, 1).SetEase(Ease.OutExpo);
		}
	}

	//Routine to create the import Screen Animations And Functions
	public void introImportScreen(){
		for (int a = 0; a<importScreen.Length; a++){
			//Debug.Log (importScreenLeft [a].transform.localPosition);
			importScreen [a].transform.DOLocalMoveX (0, 1).SetEase(Ease.OutExpo);
		}
	}

	public void outroImportScreen(){
		for (int a = 0; a<importScreen.Length; a++){
			//Debug.Log (importScreenLeft [a].transform.localPosition);
			importScreen [a].transform.DOLocalMoveX (-200, 1).SetEase(Ease.OutExpo);
		}
	}

	//Routine to create the model Screen Animations And Functions
	public void introModelScreen(){
		for (int a = 0; a<modelScreen.Length; a++){
			//Debug.Log (importScreenLeft [a].transform.localPosition);
			modelScreen [a].transform.DOLocalMoveX (0, 1).SetEase(Ease.OutExpo);
		}
	}

	public void outroModelScreen(){
		for (int a = 0; a<modelScreen.Length; a++){
			//Debug.Log (importScreenLeft [a].transform.localPosition);
			modelScreen [a].transform.DOLocalMoveX (-200, 1).SetEase(Ease.OutExpo);
		}
	}

	//Routine to kill all bottom tools
	public void killAllScreens(){
		outroModelScreen ();
		outroImportScreen ();
		outroSimulationScreen ();
	}

	//Routine to create the Prism Screen Animations And Functions
	public void introPrismScreen(){
		killAllPop ();
		prismScreen.transform.DOScale(new Vector3(0.3f,0.3f,0.3f),0.5f).SetEase(Ease.OutExpo);
	}
	public void outroPrismScreen(){
		prismScreen.transform.DOScale(new Vector3(0f,0f,0f),0.5f).SetEase(Ease.OutExpo);
		prismScreen.transform.Find("InputField/Warning").GetComponent<Text>().color = new Color(1.0f,0.266f,0.266f,0.0f);
	}

	//Function to assist with progress screen
	void introProgressScreen(){
		for (int a = 0; a<progressScreen.Length; a++){
			//Debug.Log (progressScreenLeft [a].transform.localPosition);
			progressScreen [a].transform.DOLocalMoveX (0, 1).SetEase(Ease.OutExpo);
		}
	}

	//Function to assist with progress screen
	void introSettingScreen(){
		for (int a = 0; a<settingScreen.Length; a++){
			//Debug.Log (settingScreenLeft [a].transform.localPosition);
			settingScreen [a].transform.DOLocalMoveX (0, 1).SetEase(Ease.OutExpo);
		}
	}

	public void introUploadScreen(){
		killAllPop ();
		uploadScreen.transform.DOScale(new Vector3(0.3f,0.3f,0.3f),0.5f).SetEase(Ease.OutExpo);
	}
	public void outroUploadScreen(){
		uploadScreen.transform.DOScale(new Vector3(0f,0f,0f),0.5f).SetEase(Ease.OutExpo);
		uploadScreen.transform.Find("InputField/Warning").GetComponent<Text>().color = new Color(1.0f,0.266f,0.266f,0.0f);
	}

	public void introDownloadScreen(){
		killAllPop ();
		downloadScreen.transform.DOScale(new Vector3(0.3f,0.3f,0.3f),0.5f).SetEase(Ease.OutExpo);
	}
	public void outroDownloadScreen(){
		downloadScreen.transform.DOScale(new Vector3(0f,0f,0f),0.5f).SetEase(Ease.OutExpo);
		downloadScreen.transform.Find("InputField/Warning").GetComponent<Text>().color = new Color(1.0f,0.266f,0.266f,0.0f);
	}


	public void killAllPop(){
		outroPrismScreen ();
		outroUploadScreen ();
		outroDownloadScreen ();
	}

	// Use this for initialization
	void Start (){
		bounceUp ();
		loadingBar.transform.DOScaleX (1.5f, 3f).SetEase (Ease.Linear).OnComplete (outroStartScreen);
	}

	// Update is called once per frame
	void Update () {

	}
}
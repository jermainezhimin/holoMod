using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

public class ProgressController : MonoBehaviour {


	public GameObject item;
	public GameObject butterfly;
	public GameObject[] settingButtons;
	public GameObject testButton;
	private string prevProgress;
	private int testToggle = 0;

	public GameObject canvas;
	public GameObject ModelScreen;
	public GameObject progressText;
	public GameObject progressBar;
	public GameObject forButton;
	public GameObject backButton;
	private Dictionary<string,string> dictProgress = new Dictionary<string, string>() { {"1. IMPORTING","2. MODELING"}, {"2. MODELING","3. SIMULATING"} };


	public void testLeap(){
		if (testToggle == 0) {
			testToggle = 1;
			canvas.GetComponent<CanvasController> ().killAllPop ();
			prevProgress = progressText.GetComponent<Text> ().text;
			progressBar.transform.DOScaleX (0, 0.5f);
			progressText.GetComponent<Text> ().DOText ("  LEAP TEST", 0.5f).OnComplete (interactCheck);
			testButton.GetComponent<UnityEngine.UI.Image> ().DOColor (new Color (0.312f, 0.807f, 0.219f, 1.0f), 0.5f).SetEase (Ease.OutExpo);

			foreach (GameObject button in settingButtons) {
				button.GetComponent<Button> ().interactable = false;
			}

			butterfly.SetActive (true);
			item.SetActive (false);

		} else if (testToggle == 1) {
			testToggle = 0;
			canvas.GetComponent<CanvasController> ().killAllPop ();
			progressText.GetComponent<Text> ().DOText (prevProgress, 0.5f).OnComplete (interactCheck);
			progressBar.transform.DOScaleX (float.Parse ( prevProgress[0].ToString ()), 0.5f);
			testButton.GetComponent<UnityEngine.UI.Image> ().DOColor (new Color (1f, 1f, 1f, 1.0f), 0.5f).SetEase (Ease.OutExpo);

			foreach (GameObject button in settingButtons) {
				button.GetComponent<Button> ().interactable = true;
			}

			butterfly.SetActive (false);
			item.SetActive (true);

		}

	}

	public void outrotestLeap(){
		canvas.GetComponent<CanvasController> ().killAllPop ();
		prevProgress = progressText.GetComponent<Text> ().text;
		progressBar.transform.DOScaleX (0, 0.5f);
		progressText.GetComponent<Text> ().DOText ("  LEAP TEST", 0.5f).OnComplete (interactCheck);
		testButton.GetComponent<UnityEngine.UI.Image> ().DOColor (new Color (0.312f, 0.807f, 0.219f, 1.0f), 0.5f).SetEase (Ease.OutExpo);

		foreach (GameObject button in settingButtons) {
			button.GetComponent<Button> ().interactable = false;
		}

		butterfly.SetActive (true);
		item.SetActive (false);

	}

	public void increaseProgress(){

		canvas.GetComponent<CanvasController> ().killAllPop ();

		//Makes sure the animation only works for transitable values
		if (dictProgress.ContainsKey (progressText.GetComponent<Text> ().text)) {
			progressBar.transform.DOScaleX (float.Parse (dictProgress [progressText.GetComponent<Text> ().text] [0].ToString ()), 0.5f);
			progressText.GetComponent<Text> ().DOText (dictProgress [progressText.GetComponent<Text> ().text], 0.5f).OnComplete (interactCheck);
		}
	}

	public void decreaseProgress(){

		canvas.GetComponent<CanvasController>().killAllPop();

		//Makes sure the animation only works for transitable values
		foreach (KeyValuePair<string,string> keyValue in dictProgress) {
			string key = keyValue.Key;
			string value = keyValue.Value;
			if (value == progressText.GetComponent<Text> ().text){
				progressBar.transform.DOScaleX (float.Parse (key [0].ToString ()), 0.5f);
				progressText.GetComponent<Text> ().DOText (key, 0.5f).OnComplete(interactCheck);
			}
		}
	}

	//Ensures stopping by initializing last state
	void interactCheck(){
		canvas.GetComponent<CanvasController> ().killAllScreens ();
		ModelScreen.GetComponent<ModelScreen> ().brushToggle = 0;
		ModelScreen.GetComponent<ModelScreen> ().zoomToggle = 0;

		ModelScreen.GetComponent<ModelScreen>().brushButton.GetComponent<Image> ().DOColor(new Color(1.0f,1.0f,1.0f,1.0f),0.5f).SetEase(Ease.OutExpo);
		ModelScreen.GetComponent<ModelScreen>().subButton.GetComponent<Button>().interactable = true;
		ModelScreen.GetComponent<ModelScreen>().zoomButton.GetComponent<Button>().interactable = true;

		ModelScreen.GetComponent<ModelScreen>().zoomButton.GetComponent<Image> ().DOColor(new Color(1.0f,1.0f,1.0f,1.0f),0.5f).SetEase(Ease.OutExpo);
		ModelScreen.GetComponent<ModelScreen>().subButton.GetComponent<Button>().interactable = true;
		ModelScreen.GetComponent<ModelScreen>().brushButton.GetComponent<Button>().interactable = true;

		if (progressText.GetComponent<Text> ().text == "3. SIMULATING") {
			
			forButton.GetComponent<Button> ().interactable = false;
			backButton.GetComponent<Button> ().interactable = true;
			canvas.GetComponent<CanvasController> ().introSimulationScreen ();

		} else if (progressText.GetComponent<Text> ().text == "1. IMPORTING") {

			forButton.GetComponent<Button> ().interactable = true;
			backButton.GetComponent<Button> ().interactable = false;
			canvas.GetComponent<CanvasController> ().introImportScreen ();

		} else if (progressText.GetComponent<Text> ().text == "2. MODELING"){
			
			forButton.GetComponent<Button> ().interactable = true;
			backButton.GetComponent<Button> ().interactable = true;	
			canvas.GetComponent<CanvasController> ().introModelScreen ();
			
		} else if (progressText.GetComponent<Text> ().text == "  LEAP TEST"){
			forButton.GetComponent<Button> ().interactable = false;
			backButton.GetComponent<Button> ().interactable = false;	
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}

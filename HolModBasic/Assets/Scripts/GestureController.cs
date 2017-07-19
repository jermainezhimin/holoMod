using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Leap;

public class GestureController : MonoBehaviour {

	//Retrieves the deformation items which controls all behaviours
	public HandController hc;
	public GameObject item;
	public GameObject pinchingHand;
	public GameObject pepperHand;
	public GameObject deformButton;
	public GameObject[] accessables;
	private Mesh nonDestroyed;
	private int deformToggle = 0;



	//Fracturing
	public GameObject fractureFloor;
	private int fractureToggle = 0;
	public GameObject fractureButton;

	//Important information
	public Vector3 leftHandPosition;
	public Vector3 leftHandNorm;
	public Vector3 rightHandPosition;
	public Vector3 rightHandNorm;
	public float swipeUp;
	public float swipeRight;
	public float rotationL;
	public float rotationR;
	public float pinchStrengthL;
	public float pinchStrengthR;

	// Update is called once per frame
	Frame currentFrame;

	// Use this for initialization
	void Start () {
		hc.GetLeapController().EnableGesture(Gesture.GestureType.TYPECIRCLE);
		hc.GetLeapController().EnableGesture(Gesture.GestureType.TYPESWIPE);
		hc.GetLeapController().EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
	}
		
	// Update is called once per frame
	void Update () {
		currentFrame = hc.GetFrame();
		GestureList gestures = this.currentFrame.Gestures();
		foreach (var g in gestures) {
			//Debug.Log(g.Type);
		}

		foreach (var h in hc.GetFrame().Hands) {
			if (h.IsRight) {
				pinchStrengthR = h.PinchStrength;
				swipeUp = h.PalmPosition.ToUnity ().y - rightHandPosition.y;
				swipeRight = h.PalmPosition.ToUnity ().x - rightHandPosition.x;
				rightHandPosition = h.PalmPosition.ToUnity ();
				rightHandNorm = h.PalmNormal.ToUnity ();
				rotationR = Vector3.Angle (new Vector3 (0, 0, 1), rightHandNorm);
			} else if (h.IsLeft) {
				pinchStrengthL = h.PinchStrength;
				leftHandPosition = h.PalmPosition.ToUnity();
				leftHandNorm = h.PalmNormal.ToUnity();
				rotationL = Vector3.Angle (new Vector3 (0, 0, 1), leftHandNorm);
			}
		}

		if (pinchStrengthR > 0.5f) {
			item.transform.Rotate(new Vector3(swipeUp,-swipeRight,0));
			//item.transform.rotation = Quaternion.Euler (item.transform.rotation.x + swipeUp,
			//	item.transform.rotation.y - swipeRight,
			//	item.transform.rotation.z);
			swipeUp = 0;
			swipeRight = 0;
		}
			
	}

	public void deformHand(){
		if (deformToggle == 0) {
			deformToggle = 1;
			deformButton.GetComponent<UnityEngine.UI.Image> ().DOColor(new Color(0.312f,0.807f,0.219f,1.0f),0.5f).SetEase(Ease.OutExpo);

			for (int a = 0; a < accessables.Length; a++) {
				accessables [a].GetComponent<Button> ().interactable = false;
			}
			fractureButton.GetComponent<Button> ().interactable = false;

			nonDestroyed = item.GetComponent<MeshFilter> ().mesh;
			hc.leftPhysicsModel = pinchingHand.GetComponent<HandModel> ();
		} else if (deformToggle == 1) {
			deformToggle = 0;
			deformButton.GetComponent<UnityEngine.UI.Image> ().DOColor(new Color(1f,1f,1f,1.0f),0.5f).SetEase(Ease.OutExpo);

			for (int a = 0; a < accessables.Length; a++) {
				accessables [a].GetComponent<Button> ().interactable = true;
			}
			fractureButton.GetComponent<Button> ().interactable = true;

			item.GetComponent<MeshFilter> ().mesh = nonDestroyed;
			hc.leftPhysicsModel = pepperHand.GetComponent<HandModel> ();
		}
	}

	public void fractureHand(){
		if (fractureToggle == 0) {
			fractureToggle = 1;
			fractureButton.GetComponent<UnityEngine.UI.Image> ().DOColor (new Color (0.312f, 0.807f, 0.219f, 1.0f), 0.5f).SetEase (Ease.OutExpo);

			for (int a = 0; a < accessables.Length; a++) {
				accessables [a].GetComponent<Button> ().interactable = false;
			}
			deformButton.GetComponent<Button> ().interactable = false;

			nonDestroyed = item.GetComponent<MeshFilter> ().mesh;
			fractureFloor.SetActive (true);
			item.GetComponent<Meshinator> ().m_ImpactType = Meshinator.ImpactTypes.Fracture;
			item.GetComponent<Meshinator> ().m_ForceResistance = 6.8f;
			item.GetComponent<Meshinator> ().m_MaxForcePerImpact = 15.5f;
			item.GetComponent<Meshinator> ().m_ForceMultiplier = 2f;
			item.GetComponent<Rigidbody> ().isKinematic = false;
			item.transform.position = new Vector3 (0f, 5f, 0f);

		} else if (fractureToggle == 1) {
			fractureToggle = 0;
			fractureButton.GetComponent<UnityEngine.UI.Image> ().DOColor (new Color (1f, 1f, 1f, 1.0f), 0.5f).SetEase (Ease.OutExpo);

			for (int a = 0; a < accessables.Length; a++) {
				accessables [a].GetComponent<Button> ().interactable = true;
			}
			deformButton.GetComponent<Button> ().interactable = true;

			item.GetComponent<MeshFilter> ().mesh = nonDestroyed;
			fractureFloor.SetActive (false);
			item.GetComponent<Meshinator> ().m_ImpactType = Meshinator.ImpactTypes.Fracture;
			item.GetComponent<Meshinator> ().m_ForceResistance = 7f;
			item.GetComponent<Meshinator> ().m_MaxForcePerImpact = 15.5f;
			item.GetComponent<Meshinator> ().m_ForceMultiplier = 5.5f;
			item.GetComponent<Rigidbody> ().isKinematic = true;
			item.transform.position = new Vector3 (0f, 0f, 0f);
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SmartStreetLights.Lights;
using SmartStreetLights.Message;
using SmartStreetLights.State;
using SmartStreetLights.Exception;

public class Controller : MonoBehaviour {
	
	private float _maxIntensity = 5.1f;
	private float _dimStepUp = 0.1f;
	private float _dimStepDown = 0.3f;
	
	public List<StreetLight> lights = new List<StreetLight>();
	// An array of all LightBox'es
	private LightBox[] _lightBoxes = null;
	
	/// <summary>
	/// Offset of the "SmartStreetLights" group.
	/// The group contains all the other SmartStreetLights.
	/// It will be used to calculate the correct position of one specific light.
	/// </summary>
	private Vector3 _lgOffset = new Vector3(0, 0, 0);
	
	/// <summary>
	/// The tolerance has the size of a 'Box' (which is a member of "SmartStreetLight".
	/// It will be used to provide a tolerance when selecting a "SmartStreetLight".
	/// </summary>
	private Vector3 _lightBoxScale = new Vector3(0, 0, 0);
	
	private List<StreetLightAction> _running = new List<StreetLightAction>();
	
	private float _sphereColliderScale;
	
	private class StreetLightAction {
		public StreetLight light;
		public Action action;
		
		public StreetLightAction(StreetLight light, Action action) {
			this.light = light;
			this.action = action;
		}
	}

	// Use this for initialization
	void Start () {	
		InitializeLights();
		_lgOffset = GetLightsGroupOffset();
		_lightBoxScale = GetBoxSize();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit) && hit.transform.name == "Box") {
				Vector3 hitPoint = hit.point;
				Vector3 realHP = hitPoint - _lgOffset;
				Debug.Log("Mouse: hit: " + hitPoint + ", realHP: " + realHP);
				
				try {
					int id = GetIdOfPosition(realHP);
					Debug.Log("id found: " + id);
					StreetLight light = GetLightById(id);
					//light.ToggleOnOff();
					AddLightToRunning(light, Action.DimUp);
				} catch (LightNotFoundException e) {
					Debug.Log("Controller.Update(): " + e.Message);	
				}
			}
		}
		PerformRunnings();
		CleanRunnings();
	}
	
	public void ReceiveMessage(Message message) {
		try {
			StreetLight light = GetLightById(message.id);
			Debug.Log("id found: " + message.id);
			Debug.Log("Message received: " + typeof(Message) + ", id: " + message.id + ", detectedObjects: " + message.detectedObjects);
			if (message.detectedObjects > 0) {
				AddLightToRunning(light, Action.DimUp);
			} else {
				AddLightToRunning(light, Action.DimDown);
			}
		} catch (LightNotFoundException e) {
			Debug.Log("Controller.ReceiveMessage(): " + e.Message);	
		}
	}
	
	private LightBox[] GetLightBoxes() {
		if (_lightBoxes == null) {
			_lightBoxes = GameObject.FindObjectsOfType(typeof(LightBox)) as LightBox[];
			foreach (LightBox lb in _lightBoxes) {
				Debug.Log("Found LightBox at: " + lb.transform.position);
			}
		}
		Debug.Log("lightBoxes.Length: " + _lightBoxes.Length);
		return _lightBoxes;
	}
	
	private void InitializeLights() {
		_lightBoxes = GetLightBoxes();
		
		for (int i = 0; i < _lightBoxes.Length; i++) {
			StreetLight light = (StreetLight) _lightBoxes[i].GetComponentInChildren(typeof(StreetLight));
			_lightBoxes[i].SetStreetLight(i + 1, light);
			_lightBoxes[i].GetStreetLight().SetMaxIntensity(_maxIntensity);
			_lightBoxes[i].SetController(this);
			Debug.Log("Initialized LightBox and StreetLight with ID " + (i + 1));
		}
	}
	
	private Vector3 GetLightsGroupOffset() {
		GameObject smartStreetLigths = GameObject.Find("SmartStreetLights");
		Vector3 offset = smartStreetLigths.transform.position;
		string name = smartStreetLigths.transform.name;
		Debug.Log("GetLightsGroupOffset(): " + name + ", " + offset);
		
		return offset;
	}
	
	private Vector3 GetBoxSize() {
		GameObject box = GameObject.Find("Box");
		Vector3 scale = box.transform.localScale;
		string name = box.transform.name;
		Debug.Log("GetBoxSize(): " + name + ", " + scale);
		
		return scale;
	}
	
	private int GetIdOfPosition(Vector3 position) {
		foreach (LightBox lb in GetLightBoxes()) {
			if (InRangeIncludeTolerance(lb.transform.position, position)) {
				return lb.GetId();
			}
		}
		throw new LightNotFoundException("LightNotFoundException: GetIdOfPosition()");
	}
	
	private StreetLight GetLightById(int id) {
		if (id < 1 || id > GetLightBoxes().Length) {
			throw new LightNotFoundException("GetLightById(): Light not found");
		}
		return GetLightBoxes()[id-1].GetStreetLight();
	}
	
	private bool InRangeIncludeTolerance(Vector3 v1, Vector3 v2) {		
		Vector3 v1_p = v1 + _lightBoxScale;
		Vector3 v1_m = v1 - _lightBoxScale;
		Vector3 v2_offset = v2 + _lgOffset;
		
		if (v1_m.x <= v2_offset.x && v2_offset.x < v1_p.x
			&& v1_m.y <= v2_offset.y && v2_offset.y < v1_p.y
			&& v1_m.z <= v2_offset.z && v2_offset.z < v1_p.z) {
			Debug.Log("v1_p: " + v1_p);
			Debug.Log("v2: " + v2_offset);
			Debug.Log("v1_m: " + v1_m);
			Debug.Log ("--------------");
			return true;
		}
		
		return false;
	}
	
	private void AddLightToRunning(StreetLight light, Action action) {
		bool alreadyInList = false;
		foreach (StreetLightAction la in _running) {
			if (la.light.GetId() == light.GetId()) {
				alreadyInList = true;
				// set the new action
				la.action = action;
				break;
			}
		}
		if (!alreadyInList) {
			_running.Add(new StreetLightAction(light, action));
		}
	}
	
	private void PerformRunnings() {
		foreach (StreetLightAction la in _running) {
			PerformLightAction(la);
		}
	}
	
	private void CleanRunnings() {
		List<StreetLightAction> zombies = new List<StreetLightAction>();
		foreach (StreetLightAction la in _running) {
			if (!la.light.IsRunning()) {
				zombies.Add(la);
			}
		}
		if (zombies.Count > 0) {
			foreach (StreetLightAction la in zombies) {
				Debug.Log("Cleaning zombie: " + la.light.GetId());
				_running.Remove(la);
			}
			Debug.Log("Cleaned '" + zombies.Count + "' zombie(s)");
		}
	}
	
	private void PerformLightAction(StreetLightAction la) {
		Action action = la.action;
		StreetLight light = la.light;
		switch (action) {
		case Action.DimUp:
			light.DimUp(_dimStepUp);
			break;
		case Action.DimDown:
			light.DimDown(_dimStepDown);
			break;
		case Action.LightOn:
			light.On();
			break;
		case Action.LightOff:
			light.Off();
			break;
		default:
			Debug.Log("Controller.PerformLightAction(): no action provided");
			throw new UnityException("No Action provided!");
		}
	}
	
	float _sliderValue = 1.8f;
	float _minSliderValue = 1.8f;
	float _maxSliderValue = 4.0f;
	
	void OnGUI(){
		GUI.Box (new Rect (0,0,100,50), "Top-left");
		//GUI.Box (new Rect (Screen.width - 100,0,100,50), "Top-right");
		
		_sliderValue = GUI.HorizontalSlider(new Rect(Screen.width - 100, 0, 100, 30), _sliderValue, _minSliderValue, _maxSliderValue);
		SetSphereColliderRadius(_sliderValue);
		
		GUI.Box (new Rect (0,Screen.height - 50,100,50), "Bottom-left");
		GUI.Box (new Rect (Screen.width - 100,Screen.height - 50,100,50), "Bottom-right");
	}
	
	private void SetSphereColliderRadius(float radius) {
		LightBox[] boxes = GetLightBoxes();
		foreach (LightBox box in boxes) {
			SphereCollider collider = (SphereCollider) box.collider;
			collider.radius = radius;
		}
	}
	
	
}

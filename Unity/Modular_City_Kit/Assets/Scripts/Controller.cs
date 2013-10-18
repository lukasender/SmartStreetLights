using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SmartStreetLights.Exception;
using SmartStreetLights.State;

public class Controller : MonoBehaviour {
	
	private float _maxIntensity = 5.1f;
	private float _dimStep = 0.1f;
	
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
	
	private List<StreetLight> _running = new List<StreetLight>();

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
					AddLightToRunning(light);
				} catch (LightNotFoundException e) {
					Debug.Log("Controller.Update(): " + e.Message);	
				}
			}
		}
		PerformRunnings();
		CleanRunnings();
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
		throw new LightNotFoundException();
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
	
	private void AddLightToRunning(StreetLight light) {
		bool alreadyInList = false;
		foreach (StreetLight l in _running) {
			if (l.GetId() == light.GetId()) {
				alreadyInList = true;
				break;
			}
		}
		if (!alreadyInList) {
			_running.Add(light);
		}
	}
	
	private void PerformRunnings() {
		foreach (StreetLight l in _running) {
			l.DimUp(_dimStep);
		}
	}
	
	private void CleanRunnings() {
		List<StreetLight> zombies = new List<StreetLight>();
		foreach (StreetLight l in _running) {
			if (!l.IsRunning()) {
				zombies.Add(l);
			}
		}
		if (zombies.Count > 0) {
			foreach (StreetLight l in zombies) {
				Debug.Log("Cleaning zombie: " + l.GetId());
				_running.Remove(l);
			}
			Debug.Log("Cleaned '" + zombies.Count + "' zombie(s)");
		}
	}
}

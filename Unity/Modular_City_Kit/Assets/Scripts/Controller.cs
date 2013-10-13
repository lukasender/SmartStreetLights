using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SmartStreetLights.Exception;
using SmartStreetLights.State;

public class Controller : MonoBehaviour {
	
	private float _maxIntensity = 5.1f;
	private float _dimStep = 0.1f;
	
	public List<StreetLight> lights = new List<StreetLight>();
	// An array of all SmartStreetLights
	private StreetLight[] _streetLights = null;
	
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
	private Vector3 _tolerance = new Vector3(0, 0, 0);
	
	private List<StreetLight> _running = new List<StreetLight>();

	// Use this for initialization
	void Start () {	
		InitializeLights();
		_lgOffset = GetLightsGroupOffset();
		_tolerance = GetBoxSize();
	}
	
	// Update is called once per frame
	void Update () {		
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit) && hit.transform.name == "Box") {
				Vector3 hitPoint = hit.point;
				Vector3 realHP = hitPoint - _lgOffset;
				Debug.Log("----------> Mouse: hit: " + hitPoint + ", realHP: " + realHP);
				
				try {
					int id = GetIdOfPosition(realHP);
					Debug.Log("id found: " + id);
					StreetLight light = GetLightById(id);
					//light.ToggleOnOff();
					AddLightToRunning(light);
				} catch (LightNotFoundException e) {
					Debug.Log(e.Message);	
				}
			}
		}
		PerformRunnings();
		CleanRunnings();
	}
	
	private StreetLight[] GetStreetLights() {
		if (_streetLights == null) {
			_streetLights = GameObject.FindObjectsOfType(typeof(StreetLight)) as StreetLight[];
			foreach (StreetLight l in _streetLights) {
				Debug.Log("Found SSL at: " + l.transform.position);
			}
		}
		Debug.Log ("streetLights.length: " + _streetLights.Length);
		return _streetLights;
	}
	
	private void InitializeLights() {
		int id = 1;
		_streetLights = GetStreetLights();
		for (int i = 0; i < _streetLights.Length; i++) {
			_streetLights[i].SetId(id++);
			_streetLights[i].SetMaxIntensity(_maxIntensity);
		}
		Debug.Log("SmartStreetLights count: " + id);
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
		foreach (StreetLight l in GetStreetLights()) {
			if (InRangeIncludeTolerance(l.transform.position, position)) {
				return l.GetId();
			}
		}
		
		throw new LightNotFoundException();
	}
	
	private StreetLight GetLightById(int id) {
		if (id < 1 || id > GetStreetLights().Length) {
			throw new LightNotFoundException();
		}
		return GetStreetLights()[id-1];
	}
	
	private bool InRangeIncludeTolerance(Vector3 v1, Vector3 v2) {
		Vector3 v1_p = v1 + _tolerance;
		Vector3 v1_m = v1 - _tolerance;
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {
	
	private float _maxIntensity = 5.1f;
	private float _dimStep = 0.1f;
	
	public List<StreetLight> lights = new List<StreetLight>();
	private StreetLight[] streetLights = null;

	// Use this for initialization
	void Start () {
		int id = 1;
		streetLights = getStreetLights();
		for (int i = 0; i < streetLights.Length; i++) {
			streetLights[i].setId(id++);
			streetLights[i].setMaxIntensity(_maxIntensity);
		}
		
		Debug.Log("lights count: " + id);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown(0))
            Debug.Log("Pressed left click.");
        if(Input.GetMouseButtonDown(1))
            Debug.Log("Pressed right click.");
        if(Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");
		
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit) && hit.transform.name == "Box") {
				Debug.Log("----------> Mouse: " + hit.point.x + ", " + hit.point.z);
			}
		}
		
		int size = getStreetLights().Length / 2;
		StreetLight[] test = new StreetLight[size];
		for (int i = 0; i < size; i++) {
			test[i] = getStreetLights()[i];
		}
		DimOnOff(test);
	}
	
	private StreetLight[] getStreetLights() {
		if (streetLights == null) {
			streetLights = GameObject.FindObjectsOfType(typeof(StreetLight)) as StreetLight[];
		}
		Debug.Log ("streetLights.length: " + streetLights.Length);
		return streetLights;
	}
	
	bool dimUp = true;
	
	void DimOnOff(StreetLight[] lights) {
		if (!DimUpGroup(lights) && dimUp) {
			DimUpGroup(lights);
		} else {
			dimUp = false;
		}
		
		if (!DimDownGroup(lights) && !dimUp) {
			DimDownGroup(lights);
		} else {
			dimUp = true;
		}
	}
	
	private bool DimUpGroup(StreetLight[] lights) {
		bool maxLevelReached = false;
		foreach (StreetLight l in lights) {
			maxLevelReached = l.DimUp(_dimStep);
		}
		return maxLevelReached;
	}
	
	private bool DimDownGroup(StreetLight[] lights) {
		bool minLevelReached = false;
		foreach (StreetLight l in lights) {
			minLevelReached = l.DimDown(_dimStep);
		}
		return minLevelReached;
	}
}

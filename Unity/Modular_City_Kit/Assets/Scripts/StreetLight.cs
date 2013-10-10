using UnityEngine;
using System.Collections;

public class StreetLight : MonoBehaviour {
	
	private int _id;
	float _maxIntensity;
	
	public int getId() {
		return _id;
	}
	
	public void setId(int id) {
		_id = id;
	}
	
	public void setMaxIntensity(float max) {
		_maxIntensity = max;
	}

	// Use this for initialization
	void Start () {
		light.color = Color.white;
		light.intensity = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool DimUp(float step) {
		Debug.Log("called DimUp()");
		if (light.intensity < _maxIntensity) {
			if ((light.intensity + step) >= _maxIntensity) {
				light.intensity = _maxIntensity;
				Debug.Log("StreetLight.DimUp(): max level reached");
				return true; // max level reached
			} else {
				light.intensity += step;
				Debug.Log ("StreetLight.DimUp(): set light.intensity to '" + light.intensity + "'");
				return false; // max level not reached
			}
		}
		return false;
	}
	
	public bool DimDown(float step) {
		if (light.intensity > 0) {
			if ((light.intensity - step) <= 0) {
				light.intensity = 0;
				Debug.Log("StreetLight.DimUp(): min level reached");
				return true; // min level reached
			} else {
				light.intensity -= step;
				return false; // min level not reached
			}
		}
		
		return false;
	}
	
}
using System;
using UnityEngine;

public class LightBox : MonoBehaviour {
	
	private int _id;
	
	private StreetLight _light;
	
	public LightBox () {
		
	}
	
	public void SetId(int id) {
		_id = id;
	}
	
	public int GetId() {
		return _id;
	}
	
	public void SetStreetLight(int id, StreetLight light) {
		SetId(id);
		light.SetId(id);
		_light = light;
	}
	
	public StreetLight GetStreetLight() {
		return _light;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

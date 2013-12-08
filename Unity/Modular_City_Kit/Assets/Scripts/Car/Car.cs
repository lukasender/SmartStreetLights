using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {
	
	private Camera _camera;

	// Use this for initialization
	void Start () {
		_camera = (Camera) GetComponentInChildren(typeof(Camera));
		Debug.Log("found camera in Car at " + _camera.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public Camera GetCamera() {
		return _camera;
	}
	
}

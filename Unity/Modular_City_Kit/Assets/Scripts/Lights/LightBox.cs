using System;
using UnityEngine;

namespace SmartStreetLights.Lights {
	
	using SmartStreetLights.Message;
	
	public class LightBox : MonoBehaviour {
		
		private int _id;
		
		private int _objectsDetected;
		
		private StreetLight _light;
		
		private Controller _controller;
		
		//private Boolean _OnTriggerStayCalling;
		//private float MAX_TIME = 5; // sec
		
		public LightBox () {
		}
		
		public void SetId(int id) {
			_id = id;
		}
		
		public int GetId() {
			return _id;
		}
		
		public void SetController(Controller controller) {
			_controller = controller;
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
			_objectsDetected = 0;
			//_OnTriggerStayCalling = false;
		}
		
		// Update is called once per frame
		void Update () {
			//if (!_OnTriggerStayCalling && (Time.deltaTime % MAX_TIME == 0)) {
			//	_objectsDetected = 0;
			//}
		}
		
		public int GetCurrentDetectedObjects() {
			return _objectsDetected;
		}
		
		void SendMessageToServer(Message message) {
			_controller.ReceiveMessage(message);
		}
		
		void OnTriggerEnter(Collider other) {
			Debug.Log("LightBox.OnTriggerEnter() - Object detected at light #" + _id);
			_objectsDetected++;
			SendMessageToServer(new ObjectEnterMessage(GetId(), GetCurrentDetectedObjects()));
		}
		
		void OnTriggerStay(Collider other) {
			//_OnTriggerStayCalling = true;
			//Debug.Log("LightBox.OnTriggerStay() - Object detected at light #" + _id);
			//SendMessageToServer(new ObjectDetectedMessage(GetId()));
		}
		
		void OnTriggerExit(Collider other) {
			if (_objectsDetected > 0) {
				_objectsDetected--;
			}
			Debug.Log("LightBox.OnTriggerExit() - Object detected at light #" + _id);
			SendMessageToServer(new ObjectLeaveMessage(GetId(), GetCurrentDetectedObjects()));
			//_OnTriggerStayCalling = false;
		}
		
	}
}
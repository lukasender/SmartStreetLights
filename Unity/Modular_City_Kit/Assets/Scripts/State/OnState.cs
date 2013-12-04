using System;
using UnityEngine;

using SmartStreetLights.Lights;

namespace SmartStreetLights.State
{
	public class OnState : State {
		public void On(StreetLight light) {
			
		}
		public void Off(StreetLight light) {
			light.GetLight().intensity = 0;
			Debug.Log("SSL: " + light.GetId() + ", light.intensity: " + light.GetLight().intensity);
			light.SetState(new OffState());
		}
		public void DimUp(StreetLight light, float step) {}
		public void DimDown(StreetLight light, float step) {}
	}
}


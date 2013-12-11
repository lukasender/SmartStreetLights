using System;
using UnityEngine;

using SmartStreetLights.Lights;

namespace SmartStreetLights.State
{
	public class OffState : State 
	{
		public void On(StreetLight light) {
			light.GetLight().intensity = light.GetMaxIntensity();
			light.GetPointLight().intensity = light.GetMaxIntensity();
			Debug.Log ("SSL: " + light.GetId() + ", light.intensity: " + light.GetLight().intensity);
			light.SetState(new OnState());
		}
	
		public void Off(StreetLight light) {}
		
		public void DimUp(StreetLight light, float step) {
			light.SetState(new DimmingUpState());
			light.DimUp(step);
		}
		
		public void DimDown(StreetLight light, float step) {}
	}
}


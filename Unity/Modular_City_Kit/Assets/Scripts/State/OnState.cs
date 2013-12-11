using System;
using UnityEngine;

using SmartStreetLights.Lights;

namespace SmartStreetLights.State
{
	public class OnState : State 
	{
		public void On(StreetLight light) {
			
		}
		
		public void Off(StreetLight light) {
			light.GetLight().intensity = 0;
			light.GetPointLight().intensity = 0;
			light.SetState(new OffState());
			Debug.Log("SSL: " + light.GetId() + ", light.intensity: " + light.GetLight().intensity);
		}
		
		public void DimUp(StreetLight light, float step) {}
		
		public void DimDown(StreetLight light, float step) {
			light.SetState(new DimmingDownState());
			light.DimDown(step);
		}
	}
}


using System;
using UnityEngine;

using SmartStreetLights.Lights;

namespace SmartStreetLights.State
{
	public class OffState : State 
	{
		
		public OffState(StreetLight light) {
			light.stopCounter();
		}
		
		public void On(StreetLight light) {
			light.SetState(new OnState(light));
			light.On();
		}
	
		public void Off(StreetLight light) {
			light.GetLight().intensity = 0;
			light.GetPointLight().intensity = 0;
			light.SetState(new OffState(light));
			Debug.Log("OffState().Off()" + light.GetId() + ", light.intensity: " + light.GetLight().intensity);
		}
		
		public void DimUp(StreetLight light, float step) {
			light.SetState(new DimmingUpState());
			light.DimUp(step);
		}
		
		public void DimDown(StreetLight light, float step) {}
	}
}


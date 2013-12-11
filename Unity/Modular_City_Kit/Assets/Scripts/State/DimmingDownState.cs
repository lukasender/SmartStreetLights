using System;
using UnityEngine;

using SmartStreetLights.Lights;

namespace SmartStreetLights.State
{
	public class DimmingDownState : State
	{
		public void On(StreetLight light) {
			light.GetLight().intensity = light.GetMaxIntensity();
			light.GetPointLight().intensity = light.GetMaxIntensity();
			light.SetState(new OnState());
			Debug.Log ("SSL: " + light.GetId() + ", light.intensity: " + light.GetLight().intensity);
		}
		
		public void Off(StreetLight light) {
			light.GetLight().intensity = 0;
			light.GetPointLight().intensity = 0;
			light.SetState(new OffState());
			Debug.Log("SSL: " + light.GetId() + ", light.intensity: " + light.GetLight().intensity);
		}
		
		public void DimUp(StreetLight light, float step) {
			light.SetState(new DimmingUpState());
			light.DimUp(step);
		}
		
		public void DimDown(StreetLight light, float step) {
			Light l = light.GetLight();
			if (l.intensity > 0) {
				if ((l.intensity - step) <= 0) {
					l.intensity = 0;
					light.GetPointLight().intensity = 0;
					Debug.Log("StreetLight.DimUp(): min level reached");
					light.SetState(new OffState());
				} else {
					l.intensity -= step;
					light.GetPointLight().intensity -= step;
					// min level not reached, remain in this state
				}
			}	
		}
	}
}


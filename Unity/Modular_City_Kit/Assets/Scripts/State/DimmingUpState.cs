using System;
using UnityEngine;

using SmartStreetLights.Lights;

namespace SmartStreetLights.State
{
	public class DimmingUpState : State 
	{
		public void On(StreetLight light) {
			light.GetLight().intensity = light.GetMaxIntensity();
			light.SetState(new OnState());
			Debug.Log ("SSL: " + light.GetId() + ", light.intensity: " + light.GetLight().intensity);
		}
		
		public void Off(StreetLight light) {
			light.GetLight().intensity = 0;
			light.SetState(new OffState());
			Debug.Log("SSL: " + light.GetId() + ", light.intensity: " + light.GetLight().intensity);
		}
		
		public void DimUp(StreetLight light, float step) {
			Debug.Log("called DimUp()");
			Light l = light.GetLight();
			if (l.intensity < light.GetMaxIntensity()) {
				if ((l.intensity + step) >= light.GetMaxIntensity()) {
					l.intensity = light.GetMaxIntensity();
					Debug.Log("StreetLight.DimUp(): max level reached");
					light.SetState(new OnState());
				} else {
					l.intensity += step;
					Debug.Log ("DimUp(): set light.intensity to '" + l.intensity + "'");
					// max level not reached, remain in this state
				}
			}
			// TODO: should we set state to OnState?
		}
		
		public void DimDown(StreetLight light, float step) {
			light.SetState(new DimmingDownState());
			light.DimDown(step);
		}
	}
}


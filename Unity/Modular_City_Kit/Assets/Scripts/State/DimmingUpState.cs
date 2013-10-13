using System;
using UnityEngine;

namespace SmartStreetLights.State
{
	public class DimmingUpState : State {
		public void On(StreetLight light) {
			
		}
		public void Off(StreetLight light) {
			
		}
		public void DimUp(StreetLight light, float step) {
			Debug.Log("called DimUp()");
			Light l = light.GetLight();
			if (l.intensity < light.GetMaxIntensity()) {
				if ((l.intensity + step) >= light.GetMaxIntensity()) {
					l.intensity = light.GetMaxIntensity();
					Debug.Log("StreetLight.DimUp(): max level reached");
					light.SetState(new DimmingDownState());
				} else {
					l.intensity += step;
					Debug.Log ("DimUp(): set light.intensity to '" + l.intensity + "'");
					// max level not reached, remain in this state
				}
			}
			// TODO: should we set state to OnState?
		}
		public void DimDown(StreetLight light, float step) {}
	}
}


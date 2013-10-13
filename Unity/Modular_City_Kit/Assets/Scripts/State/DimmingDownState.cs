using System;
using UnityEngine;

namespace SmartStreetLights.State
{
	public class DimmingDownState : State
	{
		
		public void On(StreetLight light) {}
		public void Off(StreetLight light) {}
		public void DimUp(StreetLight light, float step) {
			// FIXME: hack! The current implemented controller never calls DimDown... so we redirect it here, which is awful...
			DimDown(light, step);
		}
		public void DimDown(StreetLight light, float step) {
			Light l = light.GetLight();
			if (l.intensity > 0) {
				if ((l.intensity - step) <= 0) {
					l.intensity = 0;
					Debug.Log("StreetLight.DimUp(): min level reached");
					light.SetState(new OffState());
				} else {
					l.intensity -= step;
					// min level not reached, remain in this state
				}
			}	
		}
	}
}


using System;
using UnityEngine;

using SmartStreetLights.Lights;

namespace SmartStreetLights.State
{
	public class DimmingUpState : State 
	{
		public void On(StreetLight light) {
			light.SetState(new OnState(light));
			light.On();
		}
		
		public void Off(StreetLight light) {
			light.SetState(new OffState(light));
			light.Off();
		}
		
		public void DimUp(StreetLight light, float step) {
			Debug.Log("called DimUp()");
			Light l = light.GetLight();
			if ((l.intensity + step) >= light.GetMaxIntensity()) {
				light.SetState(new OnState(light));
				light.On();
			} else {
				l.intensity += step;
				light.GetPointLight().intensity += step;
				Debug.Log ("DimmingUpState().DimUp(): set light.intensity to '" + l.intensity + "'");
				// max level not reached, remain in this state
				long time = Convert.ToInt64(Time.deltaTime * 1000.0f);
				light.increaseOnTime(time);
			}
		}
		
		public void DimDown(StreetLight light, float step) {
			light.SetState(new DimmingDownState());
			light.DimDown(step);
		}
	}
}


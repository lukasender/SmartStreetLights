using System;
using UnityEngine;

using SmartStreetLights.Lights;

namespace SmartStreetLights.State
{
	public class DimmingDownState : State
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
			light.SetState(new DimmingUpState());
			light.DimUp(step);
		}
		
		public void DimDown(StreetLight light, float step) {
			Light l = light.GetLight();
			if ((l.intensity - step) <= 0) {
				light.SetState(new OffState(light));
				light.Off();
			} else {
				l.intensity -= step;
				light.GetPointLight().intensity -= step;
				Debug.Log ("DimmingDownState().DimDown(): set light.intensity to '" + l.intensity + "'");
				// min level not reached, remain in this state
				long time = Convert.ToInt64(Time.deltaTime * 1000.0f);
				light.increaseOnTime(time);
			}	
		}
	}
}


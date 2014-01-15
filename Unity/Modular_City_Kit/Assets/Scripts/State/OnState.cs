using System;
using UnityEngine;

using SmartStreetLights.Lights;

namespace SmartStreetLights.State
{
	public class OnState : State 
	{
		
		public OnState(StreetLight light) {
			light.startCounter();
		}
		
		public void On(StreetLight light) {
			light.GetLight().intensity = light.GetMaxIntensity();
			light.GetPointLight().intensity = light.GetMaxIntensity();
			Debug.LogWarning("OnState().On(): " + light.GetId());
			long time = Convert.ToInt64(Time.deltaTime * 1000.0f);
			light.increaseOnTime(time);
		}
		
		public void Off(StreetLight light) {
			light.SetState(new OffState(light));
			light.Off();
		}
		
		public void DimUp(StreetLight light, float step) {
			On(light); // to measure time
		}
		
		public void DimDown(StreetLight light, float step) {
			light.SetState(new DimmingDownState());
			light.DimDown(step);
		}
		
	}
}


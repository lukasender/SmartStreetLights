using UnityEngine;

namespace SmartStreetLights.Lights
{
	public class StreetPointLight : MonoBehaviour
	{
		public StreetPointLight ()
		{
		}
		
		public Light GetLight() {
			return light;
		}
	}
}


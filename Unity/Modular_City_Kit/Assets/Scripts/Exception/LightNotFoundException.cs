using System;

namespace SmartStreetLights.Exception
{
	public class LightNotFoundException : System.Exception
	{
		public LightNotFoundException () : base() {}
		
		public LightNotFoundException(string message) : base(message) {}
	}
}


using System;
using UnityEngine;

namespace SmartStreetLights.Message
{
	public class ObjectEnterMessage : Message
	{
		
		public ObjectEnterMessage (int id, int detectedObjects) : base(id, detectedObjects)
		{
			this.id = id;
			this.detectedObjects = detectedObjects;
			this.message = "Object entered";
		}
		
	}
}


using System;
using UnityEngine;

namespace SmartStreetLights.Message
{
	public class ObjectLeaveMessage : Message
	{
		
		public ObjectLeaveMessage (int id, int detectedObjects) : base(id, detectedObjects)
		{
			this.id = id;
			this.detectedObjects = detectedObjects;
			this.message = "Object left";
		}
		
	}
}


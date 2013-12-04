using System;
using UnityEngine;

namespace SmartStreetLights.Message
{
	public class ObjectDetectedMessage : Message
	{
		
		public ObjectDetectedMessage (int id, int detectedObjects) : base(id, detectedObjects)
		{
			this.id = id;
			this.detectedObjects = detectedObjects;
			this.message = "Object detected";
		}
		
	}
}


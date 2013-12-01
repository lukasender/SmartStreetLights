using System;
using UnityEngine;

namespace SmartStreetLights.Message
{
	public class ObjectDetectedMessage : Message
	{
		
		public ObjectDetectedMessage (int id) : base(id)
		{
			this.id = id;
			this.message = "Object detected";
		}
		
	}
}


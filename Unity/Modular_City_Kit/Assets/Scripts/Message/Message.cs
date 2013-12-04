using System;
using UnityEngine;

namespace SmartStreetLights.Message
{
	public class Message
	{
		public String message;
		
		/// <summary>
		/// The light id.
		/// </summary>
		public int id;
		
		public int detectedObjects;
		
		public Message (int id, int detectedObjects)
		{
			this.id = id;
			this.detectedObjects = detectedObjects;
		}
		
		public Message(int id, int detectedObjects, String message) {
			this.id = id;
			this.detectedObjects = detectedObjects;
			this.message = message;
		}
		
		public override string ToString () {
			return string.Format (message + ", id: " + id + ", detectedObjects: " + detectedObjects);
		}
	}
}


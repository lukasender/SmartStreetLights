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
		
		public Message (int id)
		{
			this.id = id;
		}
		
		public Message(int id, String message) {
			this.id = id;
			this.message = message;
		}
		
		public override string ToString () {
			return string.Format (message + ", id: " + id);
		}
	}
}


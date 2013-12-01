using System;
using UnityEngine;

namespace SmartStreetLights.Message
{
	public class ObjectLeaveMessage : Message
	{
		
		public ObjectLeaveMessage (int id) : base(id)
		{
			this.id = id;
			this.message = "Object left";
		}
		
	}
}


using System;
using UnityEngine;

namespace SmartStreetLights.Message
{
	public class ObjectEnterMessage : Message
	{
		
		public ObjectEnterMessage (int id) : base(id)
		{
			this.id = id;
			this.message = "Object entered";
		}
		
	}
}


using System;

namespace AssemblyCSharp
{
	public class EncounterState
	{
		public string eid = "";
		public int i = 0;
		public int j = 0;
		public bool isFinish;

		public EncounterState(string eid = "empty"){
			this.eid = eid;
		}
	}
}


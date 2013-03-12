using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Castle.MonoRail.DebugToolbar
{
	public class DebugToolbarTraceListener : TraceListener
	{
		public IList<string> Messages = new List<string> ();

		public DebugToolbarTraceListener ()
		{
		}

		#region implemented abstract members of System.Diagnostics.TraceListener
		public override void Write (string message)
		{
			Messages.Add (message);
		}

		public override void WriteLine (string message)
		{
			Messages.Add (message);
		}
		#endregion
	}
}


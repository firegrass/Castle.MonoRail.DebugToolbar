using System;
using Castle.MonoRail.Framework;

namespace Castle.MonoRail.DebugToolbar.Panels
{
	public class ServerVariablesPanel : AbstractPanel
	{
		public ServerVariablesPanel (IEngineContext context) : base (context)
		{
		}

		#region IPanel implementation

		public override string Title {
			get { return "Server variables"; }
		}

		public override string Subtitle {
			get { return "ASP.NET server variables"; }
		}

		public override string DomId {
			get { return "servervars"; }
		}

		public override bool HasContent {
			get { return true; }
		}

		#endregion

	}
}


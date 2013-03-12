using System;
using Castle.MonoRail.Framework;

namespace Castle.MonoRail.DebugToolbar.Panels
{
	public class SessionPanel : AbstractPanel
	{
		public SessionPanel (IEngineContext context) : base (context)
		{
		}

		public override string Title {
			get {
				return "Session / Flash";
			}
		}

		public override string Subtitle {
			get {
				return "Persistent variables";
			}
		}

		public override string DomId {
			get {
				return "dt_session";
			}
		}

		public override bool HasContent {
			get {
				return true;
			}
		}
	}
}


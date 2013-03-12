using System;
using Castle.MonoRail.Framework;
using Castle.Core.Logging;

namespace Castle.MonoRail.DebugToolbar.Panels
{
	public class HeadersPanel : AbstractPanel
	{
		public HeadersPanel (IEngineContext context) : base (context)
		{
		}

		public override string Title {
			get {
				return "Headers";
			}
		}

		public override string Subtitle {
			get {
				return "Request and response";
			}
		}

		public override string DomId {
			get {
				return "dt_headers";
			}
		}

		public override bool HasContent {
			get {
				return true;
			}
		}
	}
}


using System;
using Castle.MonoRail.Framework;

namespace Castle.MonoRail.DebugToolbar.Panels
{
	public class ExceptionPanel : AbstractPanel
	{
		public ExceptionPanel (IEngineContext context) : base (context)
		{
		}

		public override string Title {
			get {
				return "Exception";
			}
		}

		public override string Subtitle {
			get {
				return "";
			}
		}

		public override string DomId {
			get {
				return "dt_exception";
			}
		}

		public override bool HasContent {
			get {
				return true;
			}
		}
	}
}


using System;
using Castle.MonoRail.Framework;

namespace Castle.MonoRail.DebugToolbar.Panels
{
	public class PropertyBagPanel : AbstractPanel
	{
		public PropertyBagPanel (IEngineContext context) : base (context)
		{
		}

		public override string Title {
			get {
				return "Property Bag";
			}
		}

		public override string Subtitle {
			get {
				return "Transient variables";
			}
		}

		public override string DomId {
			get {
				return "dt_propertybag";
			}
		}

		public override bool HasContent {
			get {
				return true;
			}
		}
	}
}
using System;
using log4net.Layout;
using log4net.Core;
using System.IO;

namespace Castle.MonoRail.DebugToolbar
{
	public class DebugToolbarLog4netTraceLayout : log4net.Layout.LayoutSkeleton
	{
		private PatternLayout _PatternLayout1;
		PatternLayout PatternLayout1
		{
			get { return _PatternLayout1 ?? (_PatternLayout1 = new PatternLayout(Normal)); }
		}

		private PatternLayout _PatternLayout2;
		PatternLayout PatternLayout2
		{
			get { return _PatternLayout2 ?? (_PatternLayout2 = new PatternLayout(Sql)); }
		}

		public string Normal { get; set; }
		public string Sql { get; set; }

		public override void ActivateOptions()
		{
		}

		public override void Format(TextWriter writer, LoggingEvent loggingEvent)
		{
			if (loggingEvent.GetLoggingEventData ().LoggerName != "NHibernate.SQL")
				PatternLayout1.Format(writer, loggingEvent);
			else
				PatternLayout2.Format(writer, loggingEvent);
		}
	}
}


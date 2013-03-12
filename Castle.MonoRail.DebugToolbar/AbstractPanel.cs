using System;
using System.IO;
using Castle.MonoRail.Framework;

namespace Castle.MonoRail.DebugToolbar
{
	public abstract class AbstractPanel : IPanel
	{
		IEngineContext context;

		public AbstractPanel (IEngineContext context)
		{
			this.context = context;
		}

		#region IPanel implementation
		public virtual void Before ()
		{
		}

		public virtual void After ()
		{
			if (HasContent)
				RenderPanel ();
		}

		public virtual string Title {
			get {
				throw new System.NotImplementedException ();
			}
		}

		public virtual string Subtitle {
			get { return ""; }
		}

		public virtual string DomId {
			get {
				throw new System.NotImplementedException ();
			}
		}

		public virtual bool HasContent {
			get; set;
		}

		public virtual string Content {
			get; set;
		}
		#endregion

		private void RenderPanel ()
		{
			var templateName = this.GetType().Name;
			try {
				var writer = new StringWriter ();
				context.Services.ViewEngineManager.ProcessPartial ("Castle.MonoRail.DebugToolbar.Templates." + templateName + ".vm", writer, context, context.CurrentController, context.CurrentControllerContext);
				Content = writer.ToString ();
			} catch (Exception ex) {
				throw new Exception ("Could not process " + templateName + " template.", ex);
			}

		}

		public string GetDurationInWords (TimeSpan aTimeSpan)
		{
			string timeTaken = string.Empty;

			if (aTimeSpan.Days > 0)
				timeTaken += aTimeSpan.Days + " day" + (aTimeSpan.Days > 1 ? "s" : "");

			if (aTimeSpan.Hours > 0)
			{
				if (!string.IsNullOrEmpty (timeTaken))
					timeTaken += " ";
				timeTaken += aTimeSpan.Hours + " hour" + (aTimeSpan.Hours > 1 ? "s" : "");
			}

			if (aTimeSpan.Minutes > 0)
			{
				if (!string.IsNullOrEmpty (timeTaken))
					timeTaken += " ";
				timeTaken += aTimeSpan.Minutes + " minute" + (aTimeSpan.Minutes > 1 ? "s" : "");
			}

			if (aTimeSpan.Seconds > 0)
			{
				if (!string.IsNullOrEmpty (timeTaken))
					timeTaken += " ";
				timeTaken += aTimeSpan.Seconds + " second" + (aTimeSpan.Seconds > 1 ? "s" : "");
			}

			if (aTimeSpan.Milliseconds > 0)
			{
				if (!string.IsNullOrEmpty (timeTaken))
					timeTaken += " ";
				timeTaken += aTimeSpan.Milliseconds + " milisecond" + (aTimeSpan.Milliseconds > 1 ? "s" : "");
			}

			/*if (aTimeSpan.Ticks > 0)
			{
				if (!string.IsNullOrEmpty (timeTaken))
					timeTaken += " ";
				timeTaken += aTimeSpan.Ticks + " tick" + (aTimeSpan.Milliseconds > 1 ? "s" : "");
			}*/

			if (string.IsNullOrEmpty (timeTaken))
				timeTaken = "error";

			return timeTaken;
		}
	}
}


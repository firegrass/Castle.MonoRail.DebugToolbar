using System;
using Castle.MonoRail.Framework;
using System.IO;
using Castle.Core.Logging;
using System.Text;

namespace Castle.MonoRail.DebugToolbar
{
	public class DebugToolbarFilter : IFilter
	{
		public DebugToolbarFilter ()
		{
		}

		#region IFilter implementation
		public bool Perform (ExecuteWhen exec, IEngineContext context, IController controller, IControllerContext controllerContext)
		{
			ILoggerFactory factory = (ILoggerFactory) context.GetService (typeof (ILoggerFactory));
			ILogger logger = factory.Create (this.GetType ());

			logger.Debug ("Firing....");

			// Process panels
			if (exec == ExecuteWhen.BeforeAction) {

				logger.Debug ("Doing BeforeAction...");

			} else if (exec == ExecuteWhen.AfterAction) {

				logger.Debug ("Doing AfterAction...");

			} else if (exec == ExecuteWhen.AfterRendering) {

				logger.Debug ("Doing AfterRendering...");
			}

			return true;
		}
		#endregion

	}
}
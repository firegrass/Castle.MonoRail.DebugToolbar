using System;
using Castle.MonoRail.Framework;
using Castle.MonoRail.Framework.Services;
using Castle.Core.Resource;
using Castle.MonoRail.Framework.Configuration;
using Castle.Core.Logging;
using System.IO;
using Castle.MonoRail.DebugToolbar.Panels;
using System.Linq;
using System.Diagnostics;

namespace Castle.MonoRail.DebugToolbar
{
	public class DebugToolbarExtension : IMonoRailExtension
	{
		[ThreadStatic]
		static DebugToolbarTraceListener Listener;

		public DebugToolbarExtension ()
		{
		}

		#region IMRServiceEnabled implementation
		public void Service (IMonoRailServices serviceProvider)
		{
			ILoggerFactory factory = (ILoggerFactory) serviceProvider.GetService (typeof (ILoggerFactory));
			ILogger logger = factory.Create (this.GetType ());

			// Register resources
			string assemblyName = "Castle.MonoRail.DebugToolbar";
			string resourceName = "Castle.MonoRail.DebugToolbar.Toolbar";

			var staticResourceRegistry = (IStaticResourceRegistry) serviceProvider.GetService(typeof(IStaticResourceRegistry));

			IResource resourceJsFunctions = new AssemblyBundleResource(new CustomUri("assembly://" + assemblyName + "/" + resourceName + "/jsfunctions"));
			staticResourceRegistry.RegisterCustomResource ("DebugToolbarJsFunctions", null, null, resourceJsFunctions, "text/javascript", null);

			IResource resourceCss = new AssemblyBundleResource(new CustomUri("assembly://" + assemblyName + "/" + resourceName + "/css"));
			staticResourceRegistry.RegisterCustomResource ("DebugToolbarCss", null, null, resourceCss, "text/css", null);

			IResource resourceJsRequire = new AssemblyBundleResource(new CustomUri("assembly://" + assemblyName + "/" + resourceName + "/jsrequire"));
			staticResourceRegistry.RegisterCustomResource ("DebugToolbarJsRequire", null, null, resourceJsRequire, "text/javascript", null);

			IResource resourcejQuery = new AssemblyBundleResource(new CustomUri("assembly://" + assemblyName + "/" + resourceName + "/jquery"));
			staticResourceRegistry.RegisterCustomResource ("DebugToolbarjQuery", null, null, resourcejQuery, "text/javascript", null);

			/*IResource close = new AssemblyResource (new CustomUri("assembly://" + assemblyName + "/Clubhouse.Web.DebugToolbar.Resources." + "close.png"));
			staticResourceRegistry.RegisterCustomResource ("DebugToolbarClose", null, null, close, "image/png", null);
			IResource closeHover = new AssemblyResource (new CustomUri("assembly://" + assemblyName + "/Clubhouse.Web.DebugToolbar.Resources." + "closehover.png"));
			staticResourceRegistry.RegisterCustomResource ("DebugToolbarCloseHover", null, null, closeHover, "image/png", null);
			IResource vert = new AssemblyResource (new CustomUri("assembly://" + assemblyName + "/Clubhouse.Web.DebugToolbar.Resources." + "djdt_vertical.png"));
			staticResourceRegistry.RegisterCustomResource ("DebugToolbarVert", null, null, vert, "image/png", null);*/

			// Access the services
			var manager = (ExtensionManager) serviceProvider.GetService(typeof(ExtensionManager));
			//var config = (IMonoRailConfiguration) serviceProvider.GetService(typeof(IMonoRailConfiguration));
			//var filterFactory = (IFilterFactory) serviceProvider.GetService(typeof(IFilterFactory));

			manager.PreControllerProcess += delegate(IEngineContext context) {
				logger.Debug ("PreControllerProcess for {0}", context.Request.Url);

				// Add Trace Listener
				Listener = new DebugToolbarTraceListener ();
				Trace.Listeners.Add (Listener);

				// Load all ddls and output version numbers

				// MR routes needs code change to list routes

				// Use connection string / ajax to return EXPLAIN for sql queries

				// Create panels
				var panels = new IPanel[] {
					new HeadersPanel (context),
					new ServerVariablesPanel (context),
					new PropertyBagPanel (context),
					new SessionPanel (context),
					new NHibernatePanel (context),
					new ExceptionPanel (context),
				};

				foreach (var panel in panels)
					panel.Before ();

				context.CurrentControllerContext.PropertyBag ["debugtoolbar_panels"] = panels;
			};

			
			manager.PostControllerProcess += delegate(IEngineContext engineContext) {
				logger.Debug ("PostControllerProcess for {0}", engineContext.Request.Url);

				var panels = (IPanel[]) engineContext.CurrentControllerContext.PropertyBag ["debugtoolbar_panels"];
				foreach (var panel in panels)
					panel.After ();

				var writer = new StringWriter ();
				engineContext.Services.ViewEngineManager.ProcessPartial ("Castle.MonoRail.DebugToolbar.toolbar.vm", writer,
				                                                         engineContext, engineContext.CurrentController, engineContext.CurrentControllerContext);
				var filter = new DebugToolbarResponseFilter (engineContext.UnderlyingContext.Response.Filter, writer.ToString ());
				engineContext.UnderlyingContext.Response.Filter = filter;

				// Remove trace
				Trace.Listeners.Remove (Listener);
			};

			var loader = (IViewSourceLoader) serviceProvider.GetService(typeof(IViewSourceLoader));
			loader.AddAssemblySource (new AssemblySourceInfo ("Castle.MonoRail.DebugToolbar", "Castle.MonoRail.DebugToolbar.Resources"));
		}
		#endregion

		#region IMonoRailExtension implementation
		public void SetExtensionConfigNode (Castle.Core.Configuration.IConfiguration node)
		{
			/*throw new System.NotImplementedException ();*/
		}
		#endregion

	}
}


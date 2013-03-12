using System;
using Castle.ActiveRecord;
using Castle.MonoRail.Framework;
using Castle.Core.Logging;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Castle.MonoRail.DebugToolbar.Panels
{
	public class NHibernatePanel : AbstractPanel
	{
		IEngineContext context;
		ILogger logger;

		public NHibernatePanel (IEngineContext context) : base (context)
		{
			this.context = context;
			ILoggerFactory factory = (ILoggerFactory) context.GetService (typeof (ILoggerFactory));
			logger = factory.Create (this.GetType ());
		}

		#region IPanel implementation
		public override void Before ()
		{
			var holder = ActiveRecordMediator.GetSessionFactoryHolder();
			var sessionFactories = holder.GetSessionFactories ();
			if (sessionFactories.Length > 0) {
				var sessionFactory = sessionFactories.First ();
				if (sessionFactory != null) {
					sessionFactory.Statistics.Clear ();
					sessionFactory.Statistics.IsStatisticsEnabled = true;
				} else {
					logger.Warn ("SessionFactory was null");
				}
			} else {
				logger.Warn ("No SessionFactories found");
			}
		}
		
		public override void After ()
		{
			var sessionFactories = ActiveRecordMediator.GetSessionFactoryHolder ().GetSessionFactories();
			if (sessionFactories.Length > 0) {
				var sessionFactory = sessionFactories.First ();
				if (sessionFactory != null) {
					var stats = sessionFactory.Statistics;
					context.CurrentControllerContext.PropertyBag ["dt_nhibernate_stats"] = stats;
					context.CurrentControllerContext.PropertyBag ["dt_nhibernate"] = this;
					
					var dict = new Dictionary<string,object> ();
					foreach (var propInfo in stats.GetType().GetProperties ()) {
						if (propInfo.CanRead)
							dict.Add (propInfo.Name, propInfo.GetValue (stats, null));
					}
					context.CurrentControllerContext.PropertyBag ["dt_nhibernate_stats_dict"] = dict;
					
					sessionFactory.Statistics.IsStatisticsEnabled = false;
					
					var listener = GetTraceListener ();
					if (listener != null) {
						var lookFor = "NHibernate.SQL";
						var messages = listener.Messages.Where (m => m.Contains (lookFor))
							.Select (s => s.Substring (IndexOfSql (s))).ToArray ();
						
						var fromRegex = new Regex ("FROM", RegexOptions.IgnoreCase);
						var selectRegex = new Regex ("SELECT", RegexOptions.IgnoreCase);
						var insertRegex = new Regex ("INSERT", RegexOptions.IgnoreCase);
						var deleteRegex = new Regex ("DELETE", RegexOptions.IgnoreCase);
						var updateRegex = new Regex ("UPDATE", RegexOptions.IgnoreCase);
						var whereRegex = new Regex ("WHERE", RegexOptions.IgnoreCase);
						var havingRegex = new Regex ("HAVING", RegexOptions.IgnoreCase);
						var groupbyRegex = new Regex ("GROUP BY", RegexOptions.IgnoreCase);
					
						for (int i = 0; i < messages.Length; ++i) {
							messages [i] = fromRegex.Replace (messages [i], "<span style=\"color: Brown;\">FROM</span>");
							messages [i] = selectRegex.Replace (messages [i], "<span style=\"color: Brown;\">SELECT</span>");
							messages [i] = insertRegex.Replace (messages [i], "<span style=\"color: Brown;\">SELECT</span>");
							messages [i] = deleteRegex.Replace (messages [i], "<span style=\"color: Brown;\">SELECT</span>");
							messages [i] = updateRegex.Replace (messages [i], "<span style=\"color: Brown;\">UPDATE</span>");
							messages [i] = whereRegex.Replace (messages [i], "<span style=\"color: Brown;\">WHERE</span>");
							messages [i] = havingRegex.Replace (messages [i], "<span style=\"color: Brown;\">HAVING</span>");
							messages [i] = groupbyRegex.Replace (messages [i], "<span style=\"color: Brown;\">GROUP BY</span>");
						}
					
						context.CurrentControllerContext.PropertyBag ["dt_nhibernate_sql"] = messages;
					}
				
				} else {
					logger.Warn ("SessionFactory was null");
				}
			} else {
				logger.Warn ("No SessionFactories found");
			}
			
			base.After ();
		}
		
		private DebugToolbarTraceListener GetTraceListener ()
		{
			foreach (var listener in Trace.Listeners) {
				if (listener is DebugToolbarTraceListener)
					return (DebugToolbarTraceListener) listener;
			}
			return null;
		}
		
		private int IndexOfSql (string message)
		{
			var indexes = new int[] {
				message.IndexOf ("SELECT"),
				message.IndexOf ("select"),
				message.IndexOf ("INSERT"),
				message.IndexOf ("insert"),
				message.IndexOf ("UPDATE"),
				message.IndexOf ("update"),
				message.IndexOf ("DELETE"),
				message.IndexOf ("delete")
			}.ToList ();
			
			var matches = indexes.Where (i => i != -1).ToList ();
			if (matches.Count > 0)
				return matches.Min ();
			
			return 0;
		}
		
		public override string Title {
			get {
				return "NHibernate";
			}
		}
		
		public override string Subtitle {
			get {
				return "Statistics and performance";
			}
		}
		
		public override string DomId {
			get {
				return "dt_nhibernate";
			}
		}
		
		public override bool HasContent {
			get {
				return true;
			}
		}
		#endregion
	}
}


using System;

namespace Castle.MonoRail.DebugToolbar
{
	public interface IPanel
	{
		void Before();
		void After();

		string Title { get; }
		string Subtitle { get; }
		string DomId { get; }
		bool HasContent { get; }
		string Content { get; }
	}
}
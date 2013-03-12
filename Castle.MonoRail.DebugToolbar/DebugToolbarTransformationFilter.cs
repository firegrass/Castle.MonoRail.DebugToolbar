using System;
using Castle.MonoRail.Framework;
using System.IO;
using System.Text;

namespace Castle.MonoRail.DebugToolbar
{
	public class DebugToolbarResponseFilter : Stream
	{
		Stream baseStream;
		string toolbarMarkup;
		bool closed;

		public DebugToolbarResponseFilter (Stream baseStream, string toolbarMarkup)
		{
			this.toolbarMarkup = toolbarMarkup;
			this.baseStream = baseStream;
			closed = false;
		}

		/// <summary>
		/// The stream to the filter can use to write write to
		/// </summary>
		protected Stream BaseStream
		{
			get { return baseStream; }
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			var content = Encoding.Default.GetString (buffer, offset, count);

			if (content.Contains ("</body>"))
				content = content.Replace ("</body>", toolbarMarkup + "\n</body>");

			var output = Encoding.Default.GetBytes (content);
			baseStream.Write (output, 0, output.Length);
		}

		/// <summary>
		/// This method is not supported for an HttpFilter
		/// </summary>
		/// <exception cref="NotSupportedException">Always thrown</exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// This method is not supported for an HttpFilter
		/// </summary>
		public override bool CanRead
		{
			get { return false; }
		}

		/// <summary>
		/// This method is not supported for an HttpFilter
		/// </summary>
		public override bool CanSeek
		{
			get { return false; }
		}

		/// <summary>
		/// Indicates if the Stream is closed or open
		/// </summary>
		public override bool CanWrite
		{
			get { return !closed; }
		}

		/// <summary>
		/// Close implementation.
		/// </summary>
		/// <remarks>
		/// Don't forget to call base.Close is you override this function.
		/// </remarks>
		public override void Close()
		{
			closed = true;
			baseStream.Close();
		}

		/// <summary>
		/// This method is not supported for an HttpFilter
		/// </summary>
		/// <exception cref="NotSupportedException">Always thrown</exception>
		public override long Length
		{
			get { throw new NotSupportedException(); }
		}

		/// <summary>
		/// This method is not supported for an HttpFilter
		/// </summary>
		/// <exception cref="NotSupportedException">Always thrown</exception>
		public override long Position
		{
			get { throw new NotSupportedException(); }
			set { throw new NotSupportedException(); }
		}

		/// <summary>
		/// Flushes the base stream
		/// </summary>
		public override void Flush()
		{
			baseStream.Flush();
		}

		/// <summary>
		/// This method is not supported for an HttpFilter
		/// </summary>
		/// <exception cref="NotSupportedException">Always thrown</exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// This method is not supported for an HttpFilter
		/// </summary>
		/// <exception cref="NotSupportedException">Always thrown</exception>
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}
	}

}


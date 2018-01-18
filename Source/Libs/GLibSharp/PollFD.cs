// This file was generated by the Gtk# code generator.
// Any changes made will be lost if regenerated.

namespace GLib {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;

#region Autogenerated code
	[StructLayout(LayoutKind.Sequential)]
	public partial struct PollFD : IEquatable<PollFD> {

		public int Fd;
		public ushort Events;
		public ushort Revents;

		public static GLib.PollFD Zero = new GLib.PollFD ();

		public static GLib.PollFD New(IntPtr raw) {
			if (raw == IntPtr.Zero)
				return GLib.PollFD.Zero;
			return (GLib.PollFD) Marshal.PtrToStructure (raw, typeof (GLib.PollFD));
		}

		delegate IntPtr d_g_pollfd_get_type();
		static d_g_pollfd_get_type g_pollfd_get_type = Marshal.GetDelegateForFunctionPointer<d_g_pollfd_get_type>(FuncLoader.GetProcAddress(GLibrary.Load(Library.GLib), "g_pollfd_get_type"));

		public static GLib.GType GType { 
			get {
				IntPtr raw_ret = g_pollfd_get_type();
				GLib.GType ret = new GLib.GType(raw_ret);
				return ret;
			}
		}

		public bool Equals (PollFD other)
		{
			return true && Fd.Equals (other.Fd) && Events.Equals (other.Events) && Revents.Equals (other.Revents);
		}

		public override bool Equals (object other)
		{
			return other is PollFD && Equals ((PollFD) other);
		}

		public override int GetHashCode ()
		{
			return this.GetType().FullName.GetHashCode() ^ Fd.GetHashCode () ^ Events.GetHashCode () ^ Revents.GetHashCode ();
		}

		public static explicit operator GLib.Value (GLib.PollFD boxed)
		{
			GLib.Value val = GLib.Value.Empty;
			val.Init (GLib.PollFD.GType);
			val.Val = boxed;
			return val;
		}

		public static explicit operator GLib.PollFD (GLib.Value val)
		{
			return (GLib.PollFD) val.Val;
		}
#endregion
	}
}
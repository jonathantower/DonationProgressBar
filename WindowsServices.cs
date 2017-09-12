using System;
using System.Runtime.InteropServices;

namespace ProgressBar
{
	public static class WindowsServices
	{
		private const int WS_EX_TRANSPARENT = 32;

		private const int GWL_EXSTYLE = -20;

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern int GetWindowLong(IntPtr hwnd, int index);

		public static void SetWindowExTransparent(IntPtr hwnd)
		{
			int windowLong = WindowsServices.GetWindowLong(hwnd, -20);
			WindowsServices.SetWindowLong(hwnd, -20, windowLong | 32);
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
	}
}
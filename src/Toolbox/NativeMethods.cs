using System.Runtime.InteropServices;

namespace Toolbox;

public static partial class NativeMethods
{
	private const uint LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008;

	[LibraryImport("kernel32.dll", EntryPoint = "LoadLibraryExW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
	private static partial nint LoadLibraryEx(string lpFileName, nint hFile, uint dwFlags);

	/// <summary>
	/// Loads a dll by full path, resolving its own imports from the dll's folder first.
	/// The extender updater (DWrite.dll) imports the game's SDL2.dll, which only exists next to it.
	/// </summary>
	public static nint LoadLibrary(string lpFileName) => LoadLibraryEx(lpFileName, 0, LOAD_WITH_ALTERED_SEARCH_PATH);

	[LibraryImport("kernel32.dll", SetLastError = true)]
	public static partial nint GetProcAddress(nint hModule, [MarshalAs(UnmanagedType.LPStr)] string procedureName);

	[LibraryImport("kernel32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool FreeLibrary(nint libraryReference);

	public static void ThrowExceptionForLastWin32Error()
	{
		var errorCode = Marshal.GetHRForLastWin32Error();
		Marshal.ThrowExceptionForHR(errorCode);
	}
}

using System;
using System.Runtime.InteropServices;
using Mono.Unix.Native;

namespace PrjFSLib.Linux.Interop
{
    internal static class PrjFSLib
    {
        // TODO(Linux): set value from that defined in Linux library header
        public const int PlaceholderIdLength = 128;

        public const ulong PROJFS_CLOSE_WRITE = 0x00000008;
        public const ulong PROJFS_OPEN = 0x00000020;
        public const ulong PROJFS_DELETE_SELF = 0x00000400;
        public const ulong PROJFS_MOVE_SELF = 0x00000800;
        public const ulong PROJFS_CREATE_SELF = 0x000100000000;

        public const ulong PROJFS_ONDIR = 0x40000000;

        public const int PROJFS_ALLOW = 0x01;
        public const int PROJFS_DENY = 0x02;

        private const string PrjFSLibPath = "libprojfs.so";

        public delegate int EventHandler(ref Event ev);

        public static Fs New(string lowerdir, string mountdir, Handlers handlers)
        {
            IntPtr fs = _New(
                lowerdir,
                mountdir,
                ref handlers,
                (uint)Marshal.SizeOf<Handlers>(),
                IntPtr.Zero);

            return fs == IntPtr.Zero ? null : new Fs(fs);
        }

        public static int Start(Fs fs)
        {
            return _Start(fs.Handle);
        }

        public static void Stop(Fs fs)
        {
            _Stop(fs.Handle);
        }

        public static Errno CreateProjDir(Fs fs, string relativePath)
        {
            return _CreateProjDir(fs.Handle, relativePath);
        }

        public static Errno CreateProjFile(Fs fs, string relativePath, ulong fileSize, ushort fileMode)
        {
            return _CreateProjFile(fs.Handle, relativePath, fileSize, fileMode);
        }

        public static Errno CreateProjSymlink(Fs fs, string relativePath, string symlinkTarget)
        {
            return _CreateProjSymlink(fs.Handle, relativePath, symlinkTarget);
        }

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_write_file_contents")]
        public static extern Errno WriteFileContents(int fd, IntPtr bytes, ulong byteCount);

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_new")]
        private static extern IntPtr _New(
            string lowerdir,
            string mountdir,
            ref Handlers handlers,
            uint handlers_size,
            IntPtr user_data);

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_start")]
        private static extern int _Start(IntPtr fs);

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_stop")]
        private static extern IntPtr _Stop(IntPtr fs);

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_create_proj_dir")]
        private static extern Errno _CreateProjDir(IntPtr fs, string relativePath);

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_create_proj_file")]
        private static extern Errno _CreateProjFile(IntPtr fs, string relativePath, ulong fileSize, ushort fileMode);

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_create_proj_symlink")]
        private static extern Errno _CreateProjSymlink(IntPtr fs, string relativePath, string symlinkTarget);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Event
        {
            public IntPtr Fs;
            public ulong Mask;
            public int Pid;
            public IntPtr Path;
            public IntPtr TargetPath;
            public int Fd;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Handlers
        {
            public EventHandler HandleProjEvent;
            public EventHandler HandleNotifyEvent;
            public EventHandler HandlePermEvent;
        }

        public class Fs
        {
            internal readonly IntPtr Handle;

            public Fs(IntPtr handle)
            {
                this.Handle = handle;
            }
        }
    }
}

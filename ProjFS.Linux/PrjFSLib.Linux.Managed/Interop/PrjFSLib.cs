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

        internal delegate int EventHandler(ref Event ev);

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_new")]
        public static extern IntPtr New(
            string lowerdir,
            string mountdir,
            ref Handlers handlers,
            uint handlers_size,
            IntPtr user_data);

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_start")]
        public static extern int Start(IntPtr fs);

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_stop")]
        public static extern IntPtr Stop(IntPtr fs);

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_create_proj_dir")]
        public static extern Errno CreateProjDir(IntPtr fs, string relativePath);

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_create_proj_file")]
        public static extern Errno CreateProjFile(IntPtr fs, string relativePath, ulong fileSize, ushort fileMode);

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_create_proj_symlink")]
        public static extern Errno CreateProjSymlink(IntPtr fs, string relativePath, string symlinkTarget);

        [DllImport(PrjFSLibPath, EntryPoint = "projfs_write_file_contents")]
        public static extern Errno WriteFileContents(int fd, IntPtr bytes, ulong byteCount);

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
    }
}

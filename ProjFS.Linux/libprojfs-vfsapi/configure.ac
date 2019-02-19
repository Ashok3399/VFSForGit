# Linux Projected Filesystem
# Copyright (C) 2019 GitHub, Inc.
#
# See the NOTICE file distributed with this library for additional
# information regarding copyright ownership.
#
# This library is free software; you can redistribute it and/or
# modify it under the terms of the GNU Lesser General Public
# License as published by the Free Software Foundation; either
# version 2.1 of the License, or (at your option) any later version.
#
# This library is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
# Lesser General Public License for more details.
#
# You should have received a copy of the GNU Lesser General Public
# License along with this library, in the file COPYING; if not,
# see <http://www.gnu.org/licenses/>.

AC_INIT([libprojfs-vfsapi], [0.1])

# highest version required by local m4 macros
AC_PREREQ([2.64])
AM_INIT_AUTOMAKE([foreign no-dist-gzip dist-xz subdir-objects])
LT_INIT

AC_CONFIG_HEADERS([include/config.h])
AC_CONFIG_MACRO_DIR([m4])

AC_PROG_AWK
# AC_PROG_CC adds -g -O2 to CFLAGS by default for gcc
AC_PROG_CC
AC_PROG_INSTALL
AC_PROG_MKDIR_P
AC_PROG_SED

AC_SYS_LARGEFILE

AX_COMPILER_FLAGS()

libprojfs_vfsapi_export_regex='PrjFS'

AC_SUBST([libprojfs_vfsapi_export_regex])

AC_CHECK_PROGS([DIFF], [diff])
AC_ARG_VAR([DIFF], [File comparison tool])

AC_TYPE_PID_T
AC_TYPE_SIZE_T
AC_TYPE_UINT64_T

AC_HEADER_STDBOOL
AC_TYPE_UINT16_T

AC_CHECK_HEADER([projfs/projfs.h], [],
  [AC_MSG_ERROR([libprojfs header file not found])]dnl
)dnl
AC_SEARCH_LIBS([projfs_start], [projfs], [],
  [AC_MSG_ERROR([libprojfs library not found])]dnl
)dnl

AC_CONFIG_FILES([Makefile include/Makefile lib/Makefile
                 config.sh])
AC_OUTPUT


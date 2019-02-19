#!/bin/bash

set -e

CONFIGURATION=$1
if [ -z $CONFIGURATION ]; then
  CONFIGURATION=Debug
fi

SCRIPTDIR=$(dirname ${BASH_SOURCE[0]})
SRCDIR=$SCRIPTDIR/../..
ROOTDIR=$SRCDIR/..
PACKAGES=$ROOTDIR/packages

PROJFS=$SRCDIR/ProjFS.Linux

# XXX: currently assumes system installed libprojfs

(cd $PROJFS/libprojfs-vfsapi && ./autogen.sh && ./configure && make)
mkdir -p $ROOTDIR/BuildOutput/ProjFS.Linux/Native/$CONFIGURATION
cp $PROJFS/libprojfs-vfsapi/lib/.libs/libprojfs-vfsapi.so* $ROOTDIR/BuildOutput/ProjFS.Linux/Native/$CONFIGURATION

dotnet restore $PROJFS/PrjFSLib.Linux.Managed/PrjFSLib.Linux.Managed.csproj /p:Configuration=$CONFIGURATION /p:Platform=x64 --packages $PACKAGES || exit 1
dotnet build $PROJFS/PrjFSLib.Linux.Managed/PrjFSLib.Linux.Managed.csproj /p:Configuration=$CONFIGURATION /p:Platform=x64 || exit 1

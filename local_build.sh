#!/usr/bin/env sh

set -x

#C:\Program Files\Unity\Hub\Editor\2020.1.17f1\Editor
export UNITY_EXECUTABLE=${UNITY_EXECUTABLE:-"C:\Program Files\Unity\Hub\Editor\2020.1.17f1\Editor\Unity.exe"}
export BUILD_NAME=${BUILD_NAME:-"Remote Idyll"}

BUILD_TARGET=StandaloneLinux64 ./ci/build.sh
BUILD_TARGET=StandaloneOSX ./ci/build.sh
BUILD_TARGET=StandaloneWindows64 ./ci/build.sh
BUILD_TARGET=WebGL ./ci/build.sh

$ROOT_DIR = '.\runtimes\win-x64'
$BUILD_DIR = '.\runtimes\win-x64\msvc\build'
$LIBS_DIR = '.\runtimes\win-x64\native'

Remove-Item -Force -Recurse $ROOT_DIR -ErrorAction SilentlyContinue | Out-Null
mkdir $BUILD_DIR | Out-Null
mkdir $LIBS_DIR  | Out-Null

cmake -G "Visual Studio 17 2022" -DKOMPUTE_OPT_DISABLE_VULKAN_VERSION_CHECK=ON -DKOMPUTE_OPT_USE_BUILT_IN_VULKAN_HEADER=OFF -A X64 -S ..\..\gpt4all-backend -B $BUILD_DIR
cmake --build $BUILD_DIR --parallel --config Release

cp "$BUILD_DIR\bin\Release\*.dll" $LIBS_DIR
mv "$LIBS_DIR\llmodel.dll" "$LIBS_DIR\libllmodel.dll"

set fdir="C:\Program Files (x86)\MSBuild\14.0\Bin"

::if not exist %fdir% (
::	set fdir=%WINDIR%\Microsoft.NET\Framework
::)

set msbuild=%fdir%\msbuild.exe
%msbuild% ../src/Raven.Rpc.Tracing/Raven.Rpc.Tracing.csproj /t:Clean;Rebuild /p:Configuration=Release;VisualStudioVersion=12.0;OutputPath="..\..\output\net45\Raven.Rpc.Tracing"

set msbuild=%fdir%\msbuild.exe
%msbuild% ../src/Raven.Rpc.Tracing.Owin/Raven.Rpc.Tracing.Owin.csproj /t:Clean;Rebuild /p:Configuration=Release;VisualStudioVersion=12.0;OutputPath="..\..\output\net45\Raven.Rpc.Tracing.Owin"

set msbuild=%fdir%\msbuild.exe
%msbuild% ../src/Raven.Rpc.Tracing.NoContext/Raven.Rpc.Tracing.NoContext.csproj /t:Clean;Rebuild /p:Configuration=Release;VisualStudioVersion=12.0;OutputPath="..\..\output\net45\Raven.Rpc.Tracing.NoContext"

set msbuild=%fdir%\msbuild.exe
%msbuild% ../src/Raven.Rpc.Tracing.WebHost/Raven.Rpc.Tracing.WebHost.csproj /t:Clean;Rebuild /p:Configuration=Release;VisualStudioVersion=12.0;OutputPath="..\..\output\net45\Raven.Rpc.Tracing.WebHost"

set msbuild=%fdir%\msbuild.exe
%msbuild% ../src/Raven.Rpc.HttpProtocol.Tracing/Raven.Rpc.HttpProtocol.Tracing.csproj /t:Clean;Rebuild /p:Configuration=Release;VisualStudioVersion=12.0;OutputPath="..\..\output\net45\Raven.Rpc.HttpProtocol.Tracing"

set msbuild=%fdir%\msbuild.exe
%msbuild% ../src/Raven.AspNet.WebApiExtensions.Tracing/Raven.AspNet.WebApiExtensions.Tracing.csproj /t:Clean;Rebuild /p:Configuration=Release;VisualStudioVersion=12.0;OutputPath="..\..\output\net45\Raven.AspNet.WebApiExtensions.Tracing"

set msbuild=%fdir%\msbuild.exe
%msbuild% ../src/Raven.AspNet.MvcExtensions.Tracing/Raven.AspNet.MvcExtensions.Tracing.csproj /t:Clean;Rebuild /p:Configuration=Release;VisualStudioVersion=12.0;OutputPath="..\..\output\net45\Raven.AspNet.MvcExtensions.Tracing"

pause
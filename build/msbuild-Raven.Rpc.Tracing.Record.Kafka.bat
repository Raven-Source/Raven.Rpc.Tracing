set fdir="C:\Program Files (x86)\MSBuild\14.0\Bin"

::if not exist %fdir% (
::	set fdir=%WINDIR%\Microsoft.NET\Framework
::)

set msbuild=%fdir%\msbuild.exe
%msbuild% ../src/Raven.Rpc.Tracing.Record.Kafka/Raven.Rpc.Tracing.Record.Kafka.csproj /t:Clean;Rebuild /p:Configuration=Release;VisualStudioVersion=12.0;OutputPath="..\..\output\net45\Raven.Rpc.Tracing.Record.Kafka"

pause
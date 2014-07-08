@echo off

pushd %~dp0\..\

echo Current directory: 
cd
echo.

echo Setting up build environment...
echo -------------------------------
echo.

set config=%1
if "%config%" == "" (
   set config=Release
)

if "%PackageVersion%" == "" (

  rem Simulate myget.org environment.
  set PackageVersion=999.99
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

echo Restoring packages...
echo -------------------------------
rem Packages must be explicity restored otherwise NullGuard.Fody does not run.
rem
rem Packages folder must be packages otherwise MyGet will push dependant
rem packages to my feed.
echo.
.nuget\nuget restore -PackagesDirectory .\packages
if not "%errorlevel%" == "0" goto Error
echo.
echo.

echo Building solution...
echo -------------------------------
echo.

"C:\Program Files (x86)\MSBuild\12.0\bin\msbuild" /p:Configuration=%config%
if not "%errorlevel%" == "0" goto Error
echo.
echo.

echo Running unit tests...
echo -------------------------------
echo.

if "%GallioEcho%" == "" (

  if exist "C:\Program Files\Gallio\bin\Gallio.Echo.exe" (

    echo Setting GallioEcho environment variable...
    set GallioEcho=C:\Program Files\Gallio\bin\Gallio.Echo.exe
    
  ) else (

    echo Gallio is required to run unit tests. Try cinst Gallio.
    goto Error
	
  )
  
) else (

  echo.
  echo. todo: Add unit tests for MyGet build server.
  goto PostTests
)

"%GallioEcho%" Projects\CQRSMagic.Specifications\bin\Release\CQRSMagic.Specifications.dll
if not "%errorlevel%" == "0" goto Error
echo.
echo.  

:PostTests
  
echo Building NuGet package...
echo -------------------------
echo.

if exist .\Build\nul (

  echo .\Build folder exists.
  
) else (

  echo Creating .\Build folder...
  md .\Build
)

echo.
echo Creating NuGet package...

.\.nuget\nuget pack .\.nuget\CQRSMagic.nuspec -o .\Build %Version%

if not "%errorlevel%" == "0" goto Error
echo.
echo.

.\.nuget\nuget pack .\.nuget\CQRSMagic.Azure.nuspec -o .\Build %Version%

if not "%errorlevel%" == "0" goto Error
echo.
echo.

:Success
echo.
echo.
echo Build was successful.
echo =====================
echo.
echo.
popd
exit 0

:Error
echo.
echo.
echo **************************
echo *** An error occurred. ***
echo **************************
echo.
echo.
popd
exit -1
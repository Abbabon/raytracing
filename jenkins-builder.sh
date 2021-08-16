! rm image.ppm
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
/var/jenkins_home/.dotnet/dotnet run -c release -p cs/RaytracingInOneWeekend > image.ppm

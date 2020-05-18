#cp ../3rdParty/*.dll ../bin/
xbuild /p:Configuration="Release" /p:BuildWithMono="true" MOSA.sln /p:TargetFrameworkVersion="v4.5"

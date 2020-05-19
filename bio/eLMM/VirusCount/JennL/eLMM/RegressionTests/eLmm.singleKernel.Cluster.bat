set exe=eLMM.exe

REM-----------Regression test that runs on cluster and learns the kernel
set workingDir=jennl\LMM
set clusterVersion=2
set priority=abovenormal

set E=01
set impType=eigImp

set expData=expression.test2.txt
set snpKernel=snp.cov.txt

set initKernel=expression.cov.txt

set inputTabFile=null
set kernelEmTaskCount=2
set lmmTaskCount=2

set numEMit=3
set numIt=2


set inputFilesDir=Input\%initKernel%,Input\%expData%,Input\%snpKernel%

REM set cluster=RR1-N13-14-H41
REM set cluster=RR1-N13-17-H41
REM set cluster=RR1-N13-03-H41
REM set cluster=RR1-N13-20-H41
set cluster=RR1-N13-11-H41
REM set cluster=RR1-N13-09-H44

set MinimumNumberOfCores=%lmmTaskCount%
set MaximumNumberOfCores=%lmmTaskCount%

%REM new code
set outputName=test.learnedK.EH.txt
%exe% %E%^
 -dateStamp singleKernCluster^
 -phenDataFile Input\%expData%^
 -inputTabFile Input\%inputTabFile%^
 -impType %impType%^
 -model LMM^
 -numEMit %numEMit% -numIt %numIt%^
 -outputFile Results\%outputName%^
 -indOfKernelInit 0^
 -k1 Input\%initKernel%^
 -doFullKernelLearning^
 -lmmTaskCount %lmmTaskCount% -taskCount %kernelEmTaskCount%^
 -cluster %cluster% -dir %workingDir% -version %clusterVersion% -relativeDir -copyInputFiles %inputFilesDir% -priority %priority%^
 -MinimumNumberOfNodes %MinimumNumberOfCores% -MaximumNumberOfNodes %MaximumNumberOfCores%
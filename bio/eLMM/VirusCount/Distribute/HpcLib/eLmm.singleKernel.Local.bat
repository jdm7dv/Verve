set exe=eLMM.exe

REM-----------Regression test that runs on cluster and learns the kernel

REM encoding of the SNPs, should be either 01, or 012
set E=01

REM imputation type, recommend using eigImp
set impType=eigImp

REM phenotype data (here, gene expression)
set expData=expression.test2.txt

REM the kernel to start of kernel learning, as in the PNAS paper
set initKernel=expression.cov.txt

REM number of EM steps for each full iteration
set numEMit=3

REM number of total iterations
set numIt=2

set inputFilesDir=Input\%initKernel%,Input\%expData%,Input\%snpKernel%

%REM new code
set outputName=test.learnedK.EH.txt
%exe% %E%^
 -dateStamp singleKernLocal^
 -phenDataFile Input\%expData%^
 -inputTabFile Input\%inputTabFile%^
 -impType %impType%^
 -model LMM^
 -numEMit %numEMit% -numIt %numIt%^
 -outputFile Results\%outputName%^
 -k1 Input\%initKernel%^
 -doFullKernelLearning

set exe=FastLmm.exe

REM-----------Regression test that runs on cluster and runs FastLmm to get p-values

REM encoding of the SNPs, should be either 01, or 012
set E=01

REM imputation type, recommend using eigImp
set impType=eigImp

REM phenotype data (here, gene expression)
set expData=expression.test2.txt

REM the SNP kernel
set snpKernel=snp.cov.txt

REM SNPs to test
set snpData=Input\snps.test100.txt

REM the gene expression kernel
set expKernel=expression.cov.txt

set covLearnType=Once

%REM new code
set outputName=test.fastLmm.EH.txt
%exe% %E%^
 -dateStamp singleKernLocal^
 -covLearnType Once^
 -snpDataFile %snpData%^
 -PredictorTargetPairFilter AllPossiblePairs^
 -phenDataFile Input\%expData%^
 -impType %impType%^
 -model FAST_LMM^
 -outputFile Results\%outputName%^
 -k1 Input\%expKernel%



%REM new code
set outputName=test.fastLmm.EHandPS.txt
%exe% %E%^
 -dateStamp twoKernLocal^
 -covLearnType Once^
 -snpDataFile %snpData%^
 -PredictorTargetPairFilter AllPossiblePairs^
 -phenDataFile Input\%expData%^
 -impType %impType%^
 -model FAST_LMM^
 -outputFile Results\%outputName%^
 -k1 Input\%snpKernel%^
 -k2 Input\%expKernel%
  
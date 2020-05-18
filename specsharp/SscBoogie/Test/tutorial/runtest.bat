@echo off
setlocal

set BOOGIEDIR=..\..\Binaries
set SSCEXE=%BOOGIEDIR%\ssc.exe
set SSC_ARGS=/verifyopt:/nologo,%1 /verifyopt:/nologo,%2 /verifyopt:/nologo,%3 /verifyopt:/nologo,%4 /verifyopt:/nologo,%5 /verifyopt:/nologo,%6 /verifyopt:/nologo,%7 /verifyopt:/nologo,%8 /verifyopt:/nologo,%9

for %%f in (
    Fig0-NonNull.ssc
    Fig1-ISqrt.ssc
    Fig10-Controller.ssc
    Fig11-Dictionary.ssc
    Fig12-Collection.ssc
    Fig13-NonNullElements.ssc
    Fig14-DrawingEngine-Array.ssc
    Fig15-DrawingEngine-Generic.ssc
    Fig16-Band.ssc
    Fig17-Iterator.ssc
    Fig18-Counter.ssc
    Fig19-Counter.ssc
    Fig2-Counter.ssc
    Fig20-Counter.ssc
    Fig3-LinearSearch.ssc
    Fig4-BinarySearch.ssc
    Fig5-Rectangle.ssc
    Fig6-Swap.ssc
    Fig7-Override.ssc
    Fig8-Band.ssc
    Sec1.4-ContrivedModification.ssc
    Sec2.1-Visibility.ssc
    Sec2.2-Additive.ssc
    Sec2.3-BaseCall.ssc
    Sec2.3-Delayed.ssc
    Sec3.1-Inside.ssc
    Sec3.1-PeerConsistency.ssc
    Sec4.0-CovariantArrays.ssc
    Sec7.0-AdditiveCounter.ssc
    Sec7.0-SealedCounter.ssc
    Sec7.2-Dependencies.ssc
    Sec7.2-Determinism.ssc
    Sec7.2-Feasibility.ssc
    Sec7.2-Recursion.ssc) do (
  echo.
  echo -------------------- %%f --------------------
  "%SSCEXE%" /target:library /debug /nn /verify %SSC_ARGS% %%f
)

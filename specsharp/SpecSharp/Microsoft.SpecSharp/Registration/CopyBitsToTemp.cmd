@echo off
set FROM=%1
set TO=%2
set PDBFROM=%FROM:~0,-4%.pdb
set PDBTO=%TO:~0,-4%.pdb


REM This script checks that we can lock the appropriate files for reading and writing, to be done in the next script - either CopyBitsFromTemp or InstallBitsFromTemp


REM Make sure we have a Binaries dir to work in

if not exist Binaries mkdir Binaries
if ERRORLEVEL 1 (
  echo ERROR creating Binaries directory
  exit 1
)


REM Clean temp file already existing in Binaries dir

if exist Binaries\%TO%.temp del Binaries\%TO%.temp
if ERRORLEVEL 1 (
  echo ERROR removing Binaries\%TO%.temp
  exit 1
)

if exist Binaries\%PDBTO%.temp del Binaries\%PDBTO%.temp
if ERRORLEVEL 1 (
  echo ERROR removing %PDBTO%.temp
  exit 1
)




REM Check that any existing target file can be written to (by copying to,from temp file)

if exist %TO% (
  copy /y /v %TO% Binaries\%TO%.temp > nul
  copy /y /v Binaries\%TO%.temp %TO% > nul
  del Binaries\%TO%.temp
)

IF %ERRORLEVEL% NEQ 0 (
  echo ERROR locking for write %TO%
  exit 1
)

if exist %PDBTO% (
  copy /y /v %PDBTO% Binaries\%PDBTO%.temp > nul
  copy /y /v Binaries\%PDBTO%.temp %PDBTO% > nul
  del Binaries\%PDBTO%.temp
)
if ERRORLEVEL 1 (
  echo ERROR locking for write %PDBTO%
  exit 1
)




REM Copy newly compiled files into Binaries folder

copy /y /v %FROM% Binaries\%TO%.temp > nul
if ERRORLEVEL 1 (
  echo ERROR copying %FROM% to Binaries\%TO%.temp
  exit 1
)

if exist %PDBFROM% copy /y /v %PDBFROM% Binaries\%PDBTO%.temp > nul
if ERRORLEVEL 1 (
  echo ERROR copying %PDBFROM% to Binaries\%PDBTO%.temp
  exit 1
)



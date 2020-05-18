



axiom (forall $Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int::MemSepInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi)==((((((forall i:int::{($Mem)[i]}sAddr(i)==>($Mem)[i]==($sMem)[i])&&(forall i:int::{($Mem)[i]}dAddr(i)==>($Mem)[i]==($dMem)[i]))&&(forall i:int::{($Mem)[i]}pciAddr(i)==>($Mem)[i]==($pciMem)[i]))&&(forall i:int::{($Mem)[i]}gcAddr(i)==>($Mem)[i]==($gcMem)[i]))&&(forall s:int,i:int::{(($fMems)[s])[i]}fAddr(s,i)==>($Mem)[i]==(($fMems)[s])[i]))&&(forall s:int,i:int::{(($tMems)[s])[i]}tAddr(s,i)==>($Mem)[i]==(($tMems)[s])[i])));
#line 16
implementation initSeparation()
#line 16
{
  
  $sMem:=($Mem);
  $dMem:=($Mem);
  $pciMem:=($Mem);
  $gcMem:=($Mem);
  $tMems:=MapStacksToMem(($Mem));
  $fMems:=MapStacksToMem(($Mem));
  call SLo:=Mov(ecx);
  assert TV(ecx)&&TO(256);
  call ecx:=AddChecked(ecx,1024);
  call DLo:=Mov(ecx);
  assert TV(ecx)&&TO(256);
  call ecx:=AddChecked(ecx,1024);
  call PciLo:=Mov(ecx);
  assert TV(ecx)&&TO(2*65536);
  call ecx:=AddChecked(ecx,524288);
  call TLo:=Mov(ecx);
  assert TV(ecx)&&TO(4096);
  call ecx:=AddChecked(ecx,16384);
  call FLo:=Mov(ecx);
  assert TV(ecx)&&TO(273920);
  call ecx:=AddChecked(ecx,1095680);
  call GcLo:=Mov(ecx);
  call GcHi:=Mov(edx);
  havoc $Eip;
#line 41
  if(ecx<=edx)
  {
#line 41
    goto __L1;
  }
  call eax:=Mov(1431633936);
  call debugBreak();
#line 41
  __L1:
  
  
  
  
  
  assert MemSepInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert ?gcLo<=?gcHi;
  assert Aligned(?sLo);
  assert Aligned(?dLo);
  assert Aligned(?pciLo);
  assert Aligned(?tLo);
  assert Aligned(?fLo);
  assert Aligned(?gcLo);
  assert Aligned(?gcHi);
  assert SLo==?sLo;
  assert DLo==?dLo;
  assert PciLo==?pciLo;
  assert TLo==?tLo;
  assert FLo==?fLo;
  assert GcLo==?gcLo;
  assert GcHi==?gcHi;
}


implementation sLoad($ptr:int)
#line 66
{
}


implementation sStore($ptr:int,$val:int)
#line 70
{
  
  $sMem:=($sMem)[($ptr):=($val)];
}

implementation dLoad($ptr:int)
#line 75
{
}


implementation dStore($ptr:int,$val:int)
#line 79
{
  
  $dMem:=($dMem)[($ptr):=($val)];
}

implementation pciLoad($ptr:int)
#line 84
{
}


implementation pciStore($ptr:int,$val:int)
#line 88
{
  
  $pciMem:=($pciMem)[($ptr):=($val)];
}

implementation gcLoad($ptr:int)
#line 93
{
}


implementation gcStore($ptr:int,$val:int)
#line 97
{
  
  $gcMem:=($gcMem)[($ptr):=($val)];
}

implementation fLoad($s:int,$ptr:int)
#line 102
{
}


implementation fStore($s:int,$ptr:int,$val:int)
#line 106
{
  
  $fMems:=($fMems)[($s):=((($fMems)[($s)])[($ptr):=($val)])];
}

implementation tLoad($s:int,$ptr:int)
#line 111
{
}


implementation tStore($s:int,$ptr:int,$val:int)
#line 115
{
  
  $tMems:=($tMems)[($s):=((($tMems)[($s)])[($ptr):=($val)])];
}

implementation revealFMem()
#line 120
{
}


implementation _call()
#line 124
{
  
  $sMem:=($sMem)[(esp-4):=NextEip(($Eip))];
}

implementation _ret($oldMem:[int]int,$oldSMem:[int]int,$oldDMem:[int]int,$oldPciMem:[int]int,$oldTMem:[int][int]int,$oldFMem:[int][int]int,$oldGcMem:[int]int)
#line 129
{
}


implementation _fret($s:int,$oldMem:[int]int,$oldSMem:[int]int,$oldDMem:[int]int,$oldPciMem:[int]int,$oldTMem:[int][int]int,$oldFMem:[int][int]int,$oldGcMem:[int]int)
#line 133
{
}

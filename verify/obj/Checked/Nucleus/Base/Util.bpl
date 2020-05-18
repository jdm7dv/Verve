
implementation writeHexDigit()
#line 2
{
  
  call ecx:=Mov(eax);
  call ecx:=And(ecx,15);
  havoc $Eip;
#line 6
  if(ecx>=10)
  {
#line 6
    goto __L1;
  }
#line 6
  call ecx:=Add(ecx,3120);
#line 6
  havoc $Eip;
#line 6
  goto __L2;
#line 6
  __L1:
  call ecx:=AddChecked(ecx,3127);
#line 6
  __L2:
  
  call VgaDebugStore16(edx+753664,ecx);
}


implementation writeHex()
#line 12
{
  
  call edx:=Add(edx,14);
  call writeHexDigit();
  call edx:=Sub(edx,2);
  call eax:=Shr(eax,4);
  call writeHexDigit();
  call edx:=Sub(edx,2);
  call eax:=Shr(eax,4);
  call writeHexDigit();
  call edx:=Sub(edx,2);
  call eax:=Shr(eax,4);
  call writeHexDigit();
  call edx:=Sub(edx,2);
  call eax:=Shr(eax,4);
  call writeHexDigit();
  call edx:=Sub(edx,2);
  call eax:=Shr(eax,4);
  call writeHexDigit();
  call edx:=Sub(edx,2);
  call eax:=Shr(eax,4);
  call writeHexDigit();
  call edx:=Sub(edx,2);
  call eax:=Shr(eax,4);
  call writeHexDigit();
}

implementation debugBreak()
#line 39
{
  
  call edx:=Mov(2);
  call writeHex();
  
  call edx:=Mov(753664);
  call eax:=Mov(0);
  loop:
  call eax:=LeaUnchecked(eax+1);
  call VgaDebugStore16(edx,eax);
  havoc $Eip;
#line 49
  goto loop;
}

implementation DebugBreak()
#line 52
{
  
  call debugBreak();
  call Ret(old($RET));
#line 55
  return;
}

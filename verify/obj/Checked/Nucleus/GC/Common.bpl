#line 11
axiom (forall f:int,j:int,$FrameAddr:[int]int,$FrameSlice:[int]int::_InFrameSlice(f,j,($FrameAddr),($FrameSlice))==(($FrameSlice)[($FrameAddr)[f]+4*j]==f));

implementation _inFrameSlice()
#line 13
{
}
axiom (forall $IoMmuEnabled:bool,$PciConfigState:[int]int,DmaAddr:int,$pciMem:[int]int::IoInv(($IoMmuEnabled),($PciConfigState),DmaAddr,($pciMem))==_IoInv(($IoMmuEnabled),($PciConfigState),DmaAddr,($pciMem)));
implementation _ioInv()
#line 16
{
}
procedure TableSearch($base:int,$count:int,$x:int)
#line 18

requires (ecx==($base)&&edx==($count))&&ebp==($x);
requires (word(ecx)&&word(edx))&&word(ebp);
requires (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<=($count)==>inRo(($base)+4*j,4)));
requires (forall j1:int,j2:int::{TO(j1),TO(j2)}TO(j1)&&TO(j2)==>((0<=j1&&j1<j2)&&j2<=($count)==>ro32(($base)+4*j1)<ro32(($base)+4*j2)));

requires SMemRequireRA(100,($sMem),esp,($RET));
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,esp;
modifies $Mem;


ensures edx!=0<==>(exists j:int::{TO(j)}((TO(j)&&0<=j)&&j<($count))&&between(ro32(($base)+4*j),ro32(($base)+4*(j+1)),($x)));
ensures edx!=0==>(0<=eax&&eax<($count))&&between(ro32(($base)+4*eax),ro32(($base)+4*(eax+1)),($x));
ensures SMemEnsure(($sMem),old($sMem),esp,old(esp));
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
{
  havoc $Eip;
#line 35
  if(edx>=0)
  {
#line 35
    goto skip1;
  }
#line 36
  call edx:=Mov(0);
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 37
  call Ret(old($RET));
#line 37
  return;
  skip1:
  
  assert TO(0);
  call eax:=RoLoad32(ecx);
  havoc $Eip;
#line 42
  if(ebp>=eax)
  {
#line 42
    goto skip2;
  }
#line 43
  assert TO(0);
  call edx:=Mov(0);
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 45
  call Ret(old($RET));
#line 45
  return;
  skip2:
  
  assert TO(($count));
  call eax:=RoLoad32(ecx+4*edx);
  havoc $Eip;
#line 50
  if(ebp<eax)
  {
#line 50
    goto skip3;
  }
#line 51
  assert TO(($count));
  
  assert (forall j:int::{TSlot(j)}(((TSlot(j)&&TO(j))&&TO(j+1))&&0<=j)&&j<($count)==>($x)>=ro32(($base)+4*(j+1)));
  assert (forall j:int::{TO(j)}((TO(j)&&TSlot(j))&&0<=j)&&j<($count)==>($x)>=ro32(($base)+4*(j+1)));
  call edx:=Mov(0);
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 56
  call Ret(old($RET));
#line 56
  return;
  skip3:
  
  call esi:=Mov(0);
  call edi:=Mov(edx);
  
  
  loop:
  assert (((TO(esi)&&TO(edi))&&0<=esi)&&esi<edi)&&edi<=($count);
  assert (exists j:int::{TO(j)}((TO(j)&&0<=j)&&j<($count))&&between(ro32(($base)+4*j),ro32(($base)+4*(j+1)),($x)))==>ro32(($base)+4*esi)<=($x)&&($x)<ro32(($base)+4*edi);
  
  call eax:=Lea(esi+1);
  havoc $Eip;
#line 68
  if(eax>=edi)
  {
#line 68
    goto loopEnd;
  }
  call ebx:=LeaUnchecked(esi+1*edi);
  call ebx:=Shr(ebx,1);
  havoc $Eip;
#line 72
  if(ebx<=esi)
  {
#line 72
    goto do4;
  }
#line 73
  havoc $Eip;
#line 73
  if(ebx>=edi)
  {
#line 73
    goto do4;
  }
#line 74
  havoc $Eip;
#line 74
  goto skip4;
  do4:
  call ebx:=Lea(esi+1);
  skip4:
  
  assert TO(ebx);
  call eax:=RoLoad32(ecx+4*ebx);
  havoc $Eip;
#line 81
  if(eax<=ebp)
  {
#line 81
    goto do5;
  }
#line 82
  call edi:=Mov(ebx);
  havoc $Eip;
#line 83
  goto skip5;
  do5:
  call esi:=Mov(ebx);
  skip5:
  havoc $Eip;
#line 87
  goto loop;
  loopEnd:
  
  call eax:=Mov(esi);
  call ebx:=RoLoad32(ecx+4*eax);
  havoc $Eip;
#line 92
  if(ebp<ebx)
  {
#line 92
    goto skip6;
  }
#line 93
  call ebx:=RoLoad32(ecx+4*eax+4);
  havoc $Eip;
#line 94
  if(ebp>=ebx)
  {
#line 94
    goto skip6;
  }
#line 95
  call edx:=Mov(1);
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 96
  call Ret(old($RET));
#line 96
  return;
  skip6:
  call edx:=Mov(0);
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 99
  call Ret(old($RET));
#line 99
  return;
}

implementation TablesSearch($s:int,$f:int,$ra:int,$nextFp:int)
#line 102
{
#line 102
  var tmp2:int;
#line 102
  var tmp1:int;
#line 102
  var tableBase:int;
#line 102
  var index:int;
#line 102
  var table:int;
#line 102
  var nextFp:int;
#line 102
  var ra:int;
  
  
  
  
  
  
  
  
  call ra:=Mov(ecx);
  call nextFp:=Mov(edx);
  call table:=Mov(0);
  call edx:=Mov(0);
  
  
  loop:
  assert TVFT(($f));
  assert TT(table)&&0<=table;
  assert edx==0;
  assert !(exists t:int,j:int::{TT(t),TO(j)}((TT(t)&&TO(j))&&t<table)&&RetAddrAt(t,j,ra));
  assert word(table)&&word(ra);
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  assert MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  call eax:=Mov(table);
  havoc $Eip;
#line 126
  if(?callSiteTableCount<=eax)
  {
#line 126
    goto loopEnd;
  }
  call ebx:=Mov(?returnAddressToCallSiteSetNumbers);
  call ecx:=RoLoad32(ebx+4*eax);
  call ebx:=Mov(?callSiteSetCount);
  call edx:=RoLoad32(ebx+4*eax);
  call edx:=RoLoad32(edx);
  call ebx:=Mov(?codeBaseStartTable);
  call esi:=RoLoad32(ebx+4*eax);
  
  call ebp:=Mov(ra);
  
  assert TO(0);
  call eax:=RoLoad32(ecx);
  
  havoc $Eip;
#line 141
  if(esi>ebp)
  {
#line 141
    goto skip1;
  }
#line 142
  call ebp:=Sub(ebp,esi);
  call _call();
#line 143
  call Call();
#line 143
  call TableSearch(ecx,edx,ra-esi);
  call index:=Mov(eax);
  
  assert TO(index);
  havoc $Eip;
#line 147
  if(edx==0)
  {
#line 147
    goto skip1;
  }
#line 148
  call esi:=Mov(table);
  call edi:=Mov(?activationDescriptorTable);
  call ecx:=RoLoad32(edi+4*esi);
  call edi:=Mov(?callSiteSetNumberToIndex);
  call ebp:=RoLoad32(edi+4*esi);
  call ebp:=RoLoadU16(ebp+2*eax);
  call eax:=RoLoad32(ecx+4*ebp);
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 155
  call Ret(old($RET));
#line 155
  return;
  skip1:
  
  call table:=AddChecked(table,1);
  call edx:=Mov(0);
  havoc $Eip;
#line 160
  goto loop;
  loopEnd:
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 162
  call Ret(old($RET));
#line 162
  return;
}

procedure getSize($abs:int,$ptr:int,$vt:int,$_nElems1:int,$_nElems2:int)
#line 165

requires ecx==($ptr)&&edx==($vt);
requires ObjSize(($abs),($vt),($_nElems1),($_nElems2));
requires VTable(($abs),($vt));
requires numFields(($abs))>=3==>AlignedHeapAddr(($ptr)+8);
requires numFields(($abs))>=3&&!VFieldPtr(($abs),2)==>($gcMem)[($ptr)+8]==($_nElems1);
requires numFields(($abs))>=4==>AlignedHeapAddr(($ptr)+12);
requires numFields(($abs))>=4&&!VFieldPtr(($abs),3)==>($gcMem)[($ptr)+12]==($_nElems2);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
modifies $Eip,eax,ebx,edx,esi,edi,ebp;
ensures eax==4*numFields(($abs));
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
{
  assert TVL(($abs));
  assert TVT(($abs),($vt));
  call ebp:=RoLoad32(edx+?VT_MASK);
  call ebp:=And(ebp,15);
  
  havoc $Eip;
#line 183
  if(ebp!=?SPARSE_TAG)
  {
#line 183
    goto skip1;
  }
#line 184
  call eax:=RoLoad32(edx+?VT_BASE_LENGTH);
  havoc $Eip;
#line 185
  goto end;
  skip1:
  
  havoc $Eip;
#line 188
  if(ebp!=?DENSE_TAG)
  {
#line 188
    goto skip2;
  }
#line 189
  call eax:=RoLoad32(edx+?VT_BASE_LENGTH);
  havoc $Eip;
#line 190
  goto end;
  skip2:
  
  havoc $Eip;
#line 193
  if(ebp!=?STRING_TAG)
  {
#line 193
    goto skip3;
  }
#line 194
  assert TO(2);
  call gcLoad(ecx+8);
#line 195
  call esi:=Load(ecx+8);
  
  call eax:=Mov(esi);
  call eax:=AddChecked(eax,eax);
  call eax:=AddChecked(eax,19);
  call ebx:=Mov(3);
  call ebx:=Not(ebx);
  call eax:=And(eax,ebx);
  havoc $Eip;
#line 203
  goto end;
  skip3:
  
  havoc $Eip;
#line 206
  if(ebp!=?PTR_VECTOR_TAG)
  {
#line 206
    goto skip4;
  }
#line 207
  assert TO(2);
  call gcLoad(ecx+8);
#line 208
  call esi:=Load(ecx+8);
  call eax:=RoLoad32(edx+?VT_BASE_LENGTH);
  
  call esi:=AddChecked(esi,esi);
  call esi:=AddChecked(esi,esi);
  call eax:=AddChecked(eax,esi);
  call eax:=AddChecked(eax,3);
  call ebx:=Mov(3);
  call ebx:=Not(ebx);
  call eax:=And(eax,ebx);
  havoc $Eip;
#line 218
  goto end;
  skip4:
  
  havoc $Eip;
#line 221
  if(ebp!=?OTHER_VECTOR_TAG)
  {
#line 221
    goto skip5;
  }
#line 222
  assert TO(2);
  call gcLoad(ecx+8);
#line 223
  call esi:=Load(ecx+8);
  call eax:=RoLoad32(edx+?VT_BASE_LENGTH);
  call ebx:=RoLoad32(edx+?VT_ARRAY_ELEMENT_SIZE);
  
  call ebp:=Mov(eax);
  call eax:=Mov(ebx);
  call eax,edx:=MulChecked(eax,esi);
  call eax:=AddChecked(eax,ebp);
  call eax:=AddChecked(eax,3);
  call ebx:=Mov(3);
  call ebx:=Not(ebx);
  call eax:=And(eax,ebx);
  havoc $Eip;
#line 235
  goto end;
  skip5:
  
  havoc $Eip;
#line 238
  if(ebp!=?PTR_ARRAY_TAG)
  {
#line 238
    goto skip6;
  }
#line 239
  assert TO(3);
  call gcLoad(ecx+12);
#line 240
  call esi:=Load(ecx+12);
  call eax:=RoLoad32(edx+?VT_BASE_LENGTH);
  
  call esi:=AddChecked(esi,esi);
  call esi:=AddChecked(esi,esi);
  call eax:=AddChecked(eax,esi);
  call eax:=AddChecked(eax,3);
  call ebx:=Mov(3);
  call ebx:=Not(ebx);
  call eax:=And(eax,ebx);
  havoc $Eip;
#line 250
  goto end;
  skip6:
  
  havoc $Eip;
#line 253
  if(ebp!=?OTHER_ARRAY_TAG)
  {
#line 253
    goto skip7;
  }
#line 254
  assert TO(3);
  call gcLoad(ecx+12);
#line 255
  call esi:=Load(ecx+12);
  call eax:=RoLoad32(edx+?VT_BASE_LENGTH);
  call ebx:=RoLoad32(edx+?VT_ARRAY_ELEMENT_SIZE);
  
  call ebp:=Mov(eax);
  call eax:=Mov(ebx);
  call eax,edx:=MulChecked(eax,esi);
  call eax:=AddChecked(eax,ebp);
  call eax:=AddChecked(eax,3);
  call ebx:=Mov(3);
  call ebx:=Not(ebx);
  call eax:=And(eax,ebx);
  havoc $Eip;
#line 267
  goto end;
  skip7:
  
  
  call eax:=RoLoad32(edx+?VT_BASE_LENGTH);
  end:
}

implementation GetSize($ptr:int,$vt:int,$rs:[int]int,$rt:[int]int)
#line 275
{
  
  assert TV(($ptr));
  assert TO(1);
  assert TO(2);
  assert TO(3);
  call getSize(($rs)[($ptr)],($ptr),($vt),(($AbsMem)[($rs)[($ptr)]])[2],(($AbsMem)[($rs)[($ptr)]])[3]);
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 282
  call Ret(old($RET));
#line 282
  return;
}

implementation readTag($abs:int,$vt:int)
#line 285
{
  
  assert TVT(($abs),($vt));
  call eax:=RoLoad32(ecx+?VT_MASK);
  call eax:=And(eax,15);
}

implementation readArrayOf($abs:int,$vt:int)
#line 292
{
  
  assert TVT(($abs),($vt));
  call ebp:=RoLoad32(ecx+?VT_ARRAY_OF);
}

implementation readElementInfo($abs:int,$vt:int)
#line 298
{
  
  assert TVT(($abs),($vt));
  call esi:=RoLoad32(ecx+?VT_ARRAY_ELEMENT_SIZE);
  call edi:=RoLoad32(ecx+?VT_ARRAY_ELEMENT_CLASS);
  call edi:=RoLoad32(edi+?VT_MASK);
}

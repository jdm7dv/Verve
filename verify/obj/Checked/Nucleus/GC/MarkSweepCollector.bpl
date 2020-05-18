#line 32
function Unallocated(i:int)
  returns(bool)
{
  i==0
}
#line 33
function White(i:int)
  returns(bool)
{
  i==1
}
#line 34
function Gray(i:int)
  returns(bool)
{
  i==2
}
#line 35
function Black(i:int)
  returns(bool)
{
  i==3
}
#line 37
function ObjectSeq(i1:int,i2:int,r:[int]int,$fs:[int]int,$fn:[int]int)
  returns(bool)
{
  (forall i:int::{TV(i)}TV(i)==>(i1<=i&&i<i2==>(r[i]!=NO_ABS==>(($fs)[i]==0&&i+4*numFields(r[i])<=i2)&&(forall ii:int::{TV(ii)}TV(ii)==>(i<ii&&ii<i+4*numFields(r[i])==>r[ii]==NO_ABS&&($fs)[ii]==0)))&&(($fs)[i]!=0==>((((((r[i]==NO_ABS&&i+8<=i+($fs)[i])&&i+($fs)[i]<=i2)&&Aligned(i))&&Aligned(i+($fs)[i]))&&(($fn)[i]!=0==>between(i+($fs)[i],i2,($fn)[i])&&($fs)[($fn)[i]]!=0))&&(forall ii:int::{TV(ii)}TV(ii)==>(i<ii&&ii<i+($fs)[i]==>r[ii]==NO_ABS&&($fs)[ii]==0)))&&(forall ii:int::{TV(ii)}TV(ii)==>((i<ii&&ii<($fn)[i])&&($fs)[ii]!=0==>($fn)[ii]==0)))))
}
#line 56
function Objects(i1:int,i2:int,r:[int]int,$fs:[int]int,$fn:[int]int)
  returns(bool)
{
  (forall i:int::{TV(i)}TV(i)==>(i1<=i&&i<i2==>(r[i]!=NO_ABS==>((($fs)[i]==0&&Aligned(i))&&i+4*numFields(r[i])<=i2)&&(forall ii:int::{TV(ii)}TV(ii)==>(i<ii&&ii<i+4*numFields(r[i])==>r[ii]==NO_ABS&&($fs)[ii]==0)))&&(($fs)[i]!=0==>Aligned(i))))
}








function Disconnected(i1:int,ptr:int,$fs:[int]int,$fn:[int]int)
  returns(bool)
{
  (forall i:int::{TV(i)}TV(i)==>((i1<=i&&i<ptr)&&($fs)[i]!=0==>($fn)[i]!=ptr))
}
function FreeInvBase(i1:int,i2:int,$fs:[int]int,$fn:[int]int,data:[int]int,CachePtr:int,CacheSize:int)
  returns(bool)
{
  (i2>=i1+8&&($fs)[i1]==8)&&(CacheSize!=0==>(between(i1,i2,CachePtr)&&($fs)[CachePtr]==CacheSize)&&Disconnected(i1,CachePtr,($fs),($fn)))
}


function FreeInvI(i:int,i1:int,i2:int,$fs:[int]int,$fn:[int]int,data:[int]int,CachePtr:int,CacheSize:int)
  returns(bool)
{
  data[i]==($fn)[i]&&data[i+4]==($fs)[i]
}

function FreeInv(i1:int,i2:int,$fs:[int]int,$fn:[int]int,data:[int]int,CachePtr:int,CacheSize:int)
  returns(bool)
{
  FreeInvBase(i1,i2,($fs),($fn),data,CachePtr,CacheSize)&&(forall i:int::{TV(i)}TV(i)==>((i1<=i&&i<i2)&&($fs)[i]!=0==>FreeInvI(i,i1,i2,($fs),($fn),data,CachePtr,CacheSize)))
}



function AllocInv2($toAbs:[int]int,min:int,max:int,$r1:[int]int,$r2:[int]int,r1Live:bool)
  returns(bool)
{
  (forall i:int::{TV(i)}TV(i)==>((min<=i&&i<max)&&r1Live==>(($toAbs)[i]==NO_ABS==>($r1)[i]==NO_ABS)))&&(forall i:int::{TV(i)}TV(i)==>(min<=i&&i<max==>(($toAbs)[i]==NO_ABS==>($r2)[i]==NO_ABS)))
}

function MsInvColorI(i:int,t:Time,$r1:[int]int,$r2:[int]int,$color:[int]int,r1Live:bool,$gcMem:[int]int,$toAbs:[int]int,$AbsMem:[int][int]int,$gcSlice:[int]int)
  returns(bool)
{
  ((White(($color)[i])==>(($r1)[i]!=NO_ABS&&($r2)[i]==NO_ABS)&&(r1Live==>ObjInv(i,($r1),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice))))&&(Gray(($color)[i])==>((($r1)[i]!=NO_ABS&&($r2)[i]!=NO_ABS)&&ObjInv(i,($r1),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice)))&&reached(($toAbs)[i],t)))&&(Black(($color)[i])==>((($r1)[i]!=NO_ABS&&($r2)[i]!=NO_ABS)&&ObjInv(i,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice)))&&reached(($toAbs)[i],t))
}
#line 117
function MsInvBase(min:int,max:int,$r1:[int]int,$r2:[int]int,$color:[int]int,r1Live:bool,$gcMem:[int]int,$toAbs:[int]int,$gcSlice:[int]int)
  returns(bool)
{
  ((forall i:int::{TV(i)}TV(i)==>((((min<=i&&i<max)&&r1Live)&&($r1)[i]!=NO_ABS)&&($r2)[i]!=NO_ABS==>($r1)[i]==($r2)[i]))&&(forall i:int::{TV(i)}TV(i)==>((min<=i&&i<max)&&($toAbs)[i]!=NO_ABS==>($toAbs)[i]!=NO_ABS&&($toAbs)[i]==($r1)[i])))&&WellFormed(($toAbs))
}





function MsGcInv(min:int,max:int,t:Time,$r1:[int]int,$r2:[int]int,$color:[int]int,r1Live:bool,$gcMem:[int]int,$toAbs:[int]int,$AbsMem:[int][int]int,$gcSlice:[int]int)
  returns(bool)
{
  MsInvBase(min,max,($r1),($r2),($color),r1Live,($gcMem),($toAbs),($gcSlice))&&(forall i:int::{TV(i)}TV(i)==>((min<=i&&i<max)&&($toAbs)[i]!=NO_ABS==>MsInvColorI(i,t,($r1),($r2),($color),r1Live,($gcMem),($toAbs),($AbsMem),($gcSlice))))
}




function MsInv(min:int,max:int,$color:[int]int,$gcMem:[int]int,$toAbs:[int]int,$AbsMem:[int][int]int,$gcSlice:[int]int)
  returns(bool)
{
  ((forall i:int::{TV(i)}TV(i)==>(min<=i&&i<max==>(($toAbs)[i]!=NO_ABS==>($toAbs)[i]!=NO_ABS&&ObjInv(i,($toAbs),($toAbs),($toAbs),($AbsMem),($gcMem),($gcSlice)))))&&(forall i:int::{TV(i)}TV(i)==>(min<=i&&i<max==>(White(($color)[i])<==>($toAbs)[i]!=NO_ABS))))&&WellFormed(($toAbs))
}






function GcInv(ColorBase:int,HeapLo:int,HeapHi:int,$color:[int]int,$toAbs:[int]int,$r1:[int]int,$r2:[int]int,$Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int)
  returns(bool)
{
  ((((((((((?gcLo<=ColorBase&&ColorBase<=HeapLo)&&HeapLo<=HeapHi)&&HeapHi<=?gcHi)&&Aligned(ColorBase))&&Aligned(HeapLo))&&Aligned(HeapHi))&&bb2vec4(($color),HeapLo,($gcMem),HeapLo,HeapLo,HeapHi,ColorBase,HeapLo))&&(forall i:int::{TV(i)}TV(i)==>(i<HeapLo||i>=HeapHi==>(($toAbs)[i]==NO_ABS&&($r1)[i]==NO_ABS)&&($r2)[i]==NO_ABS)))&&(forall i:int::{TV(i)}TV(i)==>(HeapLo<=i&&i<HeapHi==>between(0,4,($color)[i]))))&&(forall i:int::{TV(i)}TV(i)==>(HeapLo<=i&&i<HeapHi==>(Unallocated(($color)[i])<==>($toAbs)[i]==NO_ABS))))&&MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi)
}
#line 161
function BigGcInv($freshAbs:int,ColorBase:int,HeapLo:int,HeapHi:int,$color:[int]int,$fs:[int]int,$fn:[int]int,$toAbs:[int]int,$r1:[int]int,$r2:[int]int,$AbsMem:[int][int]int,$gcSlice:[int]int,CachePtr:int,CacheSize:int,$Time:Time,$Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int)
  returns(bool)
{
  (((AllocInv2(($toAbs),HeapLo,HeapHi,($r1),($r2),true)&&MsGcInv(HeapLo,HeapHi,($Time),($r1),($r2),($color),true,($gcMem),($toAbs),($AbsMem),($gcSlice)))&&GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($r1),($r2),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi))&&ObjectSeq(HeapLo,HeapHi,($toAbs),($fs),($fn)))&&FreeInv(HeapLo,HeapHi,($fs),($fn),($gcMem),CachePtr,CacheSize)
}







function __NucleusInv($toAbs:[int]int,$AbsMem:[int][int]int,CurrentStack:int,$gcSlice:[int]int,$color:[int]int,StackTop:int,$fs:[int]int,$fn:[int]int,CachePtr:int,CacheSize:int,ColorBase:int,HeapLo:int,HeapHi:int,ReserveMin:int,$Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int,$FrameCounts:[int]int,$FrameAddrs:[int][int]int,$FrameLayouts:[int][int]$FrameLayout,$FrameSlices:[int][int]int,$FrameAbss:[int][int][int]int,$FrameOffsets:[int][int]int)
  returns(bool)
{
  ((MsInv(HeapLo,HeapHi,($color),($gcMem),($toAbs),($AbsMem),($gcSlice))&&GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($toAbs),($toAbs),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi))&&ObjectSeq(HeapLo,HeapHi,($toAbs),($fs),($fn)))&&FreeInv(HeapLo,HeapHi,($fs),($fn),($gcMem),CachePtr,CacheSize)
}



function _StateInv($s:int,$state:StackState,$toAbs:[int]int,$Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int,$FrameCounts:[int]int,$FrameAddrs:[int][int]int,$FrameLayouts:[int][int]$FrameLayout,$FrameSlices:[int][int]int,$FrameAbss:[int][int][int]int,$FrameOffsets:[int][int]int)
  returns(bool)
{
  ((IsStackStateTagFor(StackTag(($s),($tMems)),($state))&&(!IsEmpty(($state))==>MutatorStackInvS(($s),($toAbs),($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($fMems))))&&(IsYielded(($state))==>((fAddr(($s),StackEsp(($s),($tMems)))&&Aligned(StackEsp(($s),($tMems))))&&($state)==StackYielded(StackEbp(($s),($tMems)),StackEsp(($s),($tMems))+4,StackRA(($s),($tMems),($fMems))))&&ScanStackInv(($s),($fMems)[($s)],($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),StackRA(($s),($tMems),($fMems)),StackEsp(($s),($tMems)),StackEbp(($s),($tMems)))))&&(IsInterrupted(($state))==>((fAddr(($s),StackEsp(($s),($tMems)))&&fAddr(($s),StackEsp(($s),($tMems))+8))&&Aligned(StackEsp(($s),($tMems))))&&($state)==StackInterrupted(StackEax(($s),($tMems)),StackEbx(($s),($tMems)),StackEcx(($s),($tMems)),StackEdx(($s),($tMems)),StackEsi(($s),($tMems)),StackEdi(($s),($tMems)),StackEbp(($s),($tMems)),StackEsp(($s),($tMems))+12,StackRA(($s),($tMems),($fMems)),StackCS(($s),($tMems),($fMems)),StackEfl(($s),($tMems),($fMems))))
}
#line 200
function _NucleusInv($S:int,$stackState:[int]StackState,$toAbs:[int]int,$AbsMem:[int][int]int,CurrentStack:int,$gcSlice:[int]int,$color:[int]int,StackTop:int,$fs:[int]int,$fn:[int]int,CachePtr:int,CacheSize:int,ColorBase:int,HeapLo:int,HeapHi:int,ReserveMin:int,$Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int,$FrameCounts:[int]int,$FrameAddrs:[int][int]int,$FrameLayouts:[int][int]$FrameLayout,$FrameSlices:[int][int]int,$FrameAbss:[int][int][int]int,$FrameOffsets:[int][int]int,$IoMmuEnabled:bool,$PciConfigState:[int]int,DmaAddr:int)
  returns(bool)
{
  ((__NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets))&&CurrentStack==($S))&&(forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))==>_StateInv(($s),($stackState)[($s)],($toAbs),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets)))))&&IoInv(($IoMmuEnabled),($PciConfigState),DmaAddr,($pciMem))
}



axiom (forall $S:int,$stackState:[int]StackState,$toAbs:[int]int,$AbsMem:[int][int]int,CurrentStack:int,$gcSlice:[int]int,$color:[int]int,StackTop:int,$fs:[int]int,$fn:[int]int,CachePtr:int,CacheSize:int,ColorBase:int,HeapLo:int,HeapHi:int,ReserveMin:int,$Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int,$FrameCounts:[int]int,$FrameAddrs:[int][int]int,$FrameLayouts:[int][int]$FrameLayout,$FrameSlices:[int][int]int,$FrameAbss:[int][int][int]int,$FrameOffsets:[int][int]int,$IoMmuEnabled:bool,$PciConfigState:[int]int,DmaAddr:int::NucleusInv(($S),($stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr)==_NucleusInv(($S),($stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr));



function MarkStack(s1:int,s2:int,$toAbs:[int]int,$color:[int]int,stack:[int]int,extra:int)
  returns(bool)
{
  (((s1<=s2&&Aligned(s2))&&(forall i:int::{TV(i)}TV(i)==>(gcAddr(i)==>((($toAbs)[i]!=NO_ABS&&Gray(($color)[i]))&&i!=extra==>(exists s:int::{TV(s)}(((TV(s)&&s1<=s)&&s<s2)&&Aligned(s))&&stack[s]==i)))))&&(forall s:int::{TV(s)}TV(s)==>((s1<=s&&s<s2)&&Aligned(s)==>((gcAddr(stack[s])&&($toAbs)[stack[s]]!=NO_ABS)&&Gray(($color)[stack[s]]))&&stack[s]!=extra)))&&(forall s:int,t:int::{TV(s),TV(t)}TV(s)&&TV(t)==>(s1<=s&&s<s2==>(((s1<=t&&t<s2)&&Aligned(s))&&Aligned(t)==>(s!=t==>stack[s]!=stack[t]))))
}








procedure BB4Zero2($a:[int]int,$aBase:int,$i0:int,$i1:int,$i2:int,$k:int,$g1:int,$g2:int,$_i0:int,$_g1:int)
#line 225

requires eax==($g1);
requires ebx==($g2);
requires (forall i:int::{TV(i)}(TV(i)&&($i1)<=i)&&i<($i2)==>($a)[($aBase)+(i-($i0))]==0);
requires Aligned(($g1))&&Aligned(($g2));
requires ($i2)-($i1)==16*(($g2)-($g1));
requires word(($i1)-($i0))&&word(($i2)-($i0));
requires (?gcLo<=($g1)&&($g1)<=($g2))&&($g2)<=?gcHi;
requires ($i1)==($i0);
requires SMemRequireRA(100,($sMem),esp,($RET));
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
modifies $gcMem;
modifies $Eip,eax,ebx,esi,edi,ebp,esp,$Mem,$sMem;
ensures bb2vec4(($a),($aBase),($gcMem),($i0),($i1),($i2),($g1),($g2));
ensures (forall i:int::{TV(i)}TV(i)&&!between(($g1),($g2),i)==>($gcMem)[i]==old($gcMem)[i]);
ensures SMemEnsure(($sMem),old($sMem),esp,old(esp));
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
{
#line 225
  var $iter:int;
#line 244
  call esi:=Mov(eax);
  $iter:=($i1);
  
  loop:
  assert Aligned(esi)&&TV(esi);
  assert ($g1)<=esi&&esi<=($g2);
  assert ($iter)-($i1)==16*(esi-($g1));
  assert bb2vec4(($a),($aBase),($gcMem),($i0),($i1),($iter),($g1),($g2));
  assert (forall i:int::{TV(i)}TV(i)&&!between(($g1),($g2),i)==>($gcMem)[i]==old($gcMem)[i]);
  assert MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  havoc $Eip;
#line 254
  if(esi>=ebx)
  {
#line 254
    goto loopEnd;
  }
  call __notAligned(esi);
  call __bb4Zero2(($a),($aBase),($gcMem),($i0),($i1),($iter),($g1),($g2),esi);
  call gcStore(esi,0);
#line 258
  call Store(esi,0);
  $iter:=($iter)+64;
  call esi:=Add(esi,4);
  assert TO(1);
  havoc $Eip;
#line 262
  goto loop;
  loopEnd:
  
  assert esi==($g2);
  assert ($iter)==($i2);
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 267
  call Ret(old($RET));
#line 267
  return;
}

procedure bb4GetColor($a:[int]int,$aBase:int,$i0:int,$i1:int,$i2:int,$k:int,$g1:int,$g2:int)
#line 270

requires bb2vec4(($a),($aBase),($gcMem),($i0),($i1),($i2),($g1),($g2));
requires (word(($k)-($i0))&&($i1)<=($k))&&($k)<($i2);
requires ((word(($k))&&word(($i0)))&&Aligned(($k)))&&Aligned(($i0));
requires word(($i2)-($i0));
requires eax==($g1);
requires ebx==($k)-($i0);
requires (Aligned(($g1))&&?gcLo<=($g1))&&($g2)<=?gcHi;
requires word(($i1)-($i0))&&word(($i2)-($i0));
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
modifies $Eip,ebx,ecx,edx;
ensures ebx==($a)[($aBase)+(($k)-($i0))];
{
#line 270
  var $bbb:int;
#line 270
  var $idx:int;
#line 285
  $idx:=($g1)+4*shr(($k)-($i0),6);
  $bbb:=and(shr(($gcMem)[($idx)],and(shr(($k)-($i0),1),31)),3);
  call __subAligned(($k),($i0));
  call __bb4Get2Bit(($a),($aBase),($gcMem),($i0),($i1),($i2),($k),($idx),($bbb),($g1),($g2));
  
  assert TV(($g1));
  assert TO(shr(ebx,6));
  
  call ecx:=Mov(ebx);
  call ecx:=Shr(ecx,6);
  call edx:=Lea(eax+4*ecx);
  call ecx:=Mov(ebx);
  call ecx:=Shr(ecx,1);
  call ecx:=And(ecx,31);
  call gcLoad(edx);
#line 299
  call edx:=Load(edx);
  call ebx:=Mov(edx);
  call ebx:=Shr(ebx,ecx);
  call ebx:=And(ebx,3);
}

procedure bb4SetColor($a:[int]int,$val:int,$aBase:int,$i0:int,$i1:int,$i2:int,$k:int,$g1:int,$g2:int)
#line 305

requires bb2vec4(($a),($aBase),($gcMem),($i0),($i1),($i2),($g1),($g2));
requires (word(($k)-($i0))&&($i1)<=($k))&&($k)<($i2);
requires ((word(($k))&&word(($i0)))&&Aligned(($k)))&&Aligned(($i0));
requires word(($i2)-($i0));
requires 0<=($val)&&($val)<=3;
requires esi==($k)-($i0);
requires edi==($g1);
requires edx==($val);
requires (Aligned(($g1))&&?gcLo<=($g1))&&($g2)<=?gcHi;
requires word(($i1)-($i0))&&word(($i2)-($i0));
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
modifies $Eip,eax,esi,edi,ecx,edx,$gcMem,$Mem;
ensures bb2vec4(($a)[(($aBase)+(($k)-($i0))):=($val)],($aBase),($gcMem),($i0),($i1),($i2),($g1),($g2));
ensures ($gcMem)==old($gcMem)[(($g1)+ColorIndex(($i0),($k))):=esi];
ensures Aligned(($k)-($i0));
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
{
#line 305
  var $_bbb:int;
#line 305
  var $bbb:int;
#line 305
  var $idx:int;
#line 326
  $idx:=($g1)+4*shr(($k)-($i0),6);
  $bbb:=and(($gcMem)[($idx)],neg(shl(3,and(shr(($k)-($i0),1),31))));
  $_bbb:=or(($bbb),shl(($val),and(shr(($k)-($i0),1),31)));
  call __subAligned(($k),($i0));
  call __bb4Set2Bit(($a),($val),($aBase),($gcMem),($i0),($i1),($i2),($k),($idx),($bbb),($_bbb),($gcMem)[($idx):=($_bbb)],($g1),($g2));
  assert TV(($g1));
  assert TO(shr(esi,6));
  
  call ecx:=Mov(esi);
  call esi:=Shr(esi,6);
  call edi:=Lea(edi+4*esi);
  
  
  call ecx:=Shr(ecx,1);
  call ecx:=And(ecx,31);
  call eax:=Mov(3);
  call eax:=Shl(eax,ecx);
  call eax:=Not(eax);
  call gcLoad(edi);
#line 344
  call esi:=Load(edi);
  call esi:=And(esi,eax);
  
  
  call edx:=Shl(edx,ecx);
  call esi:=Or(esi,edx);
  assert ($_bbb)==esi;
  call gcStore(edi,esi);
#line 351
  call Store(edi,esi);
}

procedure visit($_ptr:int,$abs:int,$extra:int)
#line 354

requires ecx==($_ptr);
requires AllocInv2(($toAbs),HeapLo,HeapHi,($r1),($r2),true);
requires MsGcInv(HeapLo,HeapHi,($Time),($r1),($r2),($color),true,($gcMem),($toAbs),($AbsMem),($gcSlice));
requires GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($r1),($r2),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires Value(true,($r1),($_ptr),($abs));
requires !gcAddrEx(($_ptr))||reached(($toAbs)[($_ptr)-4],($Time));
requires TV(($_ptr)-4);
requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($extra));
requires ($extra)!=0==>Gray(($color)[($extra)]);
requires StackTop<=ColorBase;
requires gcAddrEx(($_ptr));
requires SMemRequire(100,($sMem),esp);

modifies $r2,$color;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
modifies StackTop,$gcMem;

ensures old(StackTop)<=StackTop&&StackTop<=ColorBase;
ensures (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($extra));
ensures GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($r1),($r2),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures AllocInv2(($toAbs),HeapLo,HeapHi,($r1),($r2),true);
ensures MsGcInv(HeapLo,HeapHi,($Time),($r1),($r2),($color),true,($gcMem),($toAbs),($AbsMem),($gcSlice));
ensures RExtend(old($r2),($r2));
ensures Value(true,($r2),($_ptr),($abs));
ensures !gcAddrEx(($_ptr))||!White(($color)[($_ptr)-4]);
ensures (forall i:int::{TV(i)}TV(i)==>(gcAddr(i)==>(Gray(old($color)[i])==>Gray(($color)[i]))));
ensures ((old(StackTop<ColorBase)&&($gcMem)==(old($gcMem)[(ColorBase+ColorIndex(HeapLo,($_ptr)-4)):=(($gcMem)[ColorBase+ColorIndex(HeapLo,($_ptr)-4)])])[old(StackTop):=(($gcMem)[old(StackTop)])])&&between(ColorBase,HeapLo,ColorBase+ColorIndex(HeapLo,($_ptr)-4)))||($gcMem)==old($gcMem);




ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
  
  call ebp:=Mov(ecx);
  call ebp:=Sub(ebp,4);
  
  call ebx:=Mov(ebp);
  call ebx:=Sub(ebx,HeapLo);
  call eax:=Mov(ColorBase);
  call bb4GetColor(($color),HeapLo,HeapLo,HeapLo,HeapHi,ebp,ColorBase,HeapLo);
  assert ebx==($color)[ebp];
  
  havoc $Eip;
#line 399
  if(ebx!=1)
  {
#line 399
    goto end;
  }
#line 400
  call eax:=Mov(StackTop);
  havoc $Eip;
#line 401
  if(eax!=ColorBase)
  {
#line 401
    goto skip1;
  }
  call eax:=Mov(1431633985);
  call _call();
#line 404
  call Call();
#line 404
  call DebugBreak();
  skip1:
  call esi:=Mov(ebp);
  call esi:=Sub(esi,HeapLo);
  call edi:=Mov(ColorBase);
  call edx:=Mov(2);
  call bb4SetColor(($color),2,HeapLo,HeapLo,HeapLo,HeapHi,ebp,ColorBase,HeapLo);
  $color:=($color)[ebp:=2];
  
  $r2:=($r2)[ebp:=(($r1)[ebp])];
  call eax:=Mov(StackTop);
  call gcStore(eax,ebp);
#line 415
  call Store(eax,ebp);
  assert TV(StackTop)&&TO(1);
  call __notAligned(StackTop);
  call StackTop:=Add(StackTop,4);
  end:
}

procedure scanObjectDense($vt:int,$ptr:int,$abs:int)
#line 422

requires ebx==($ptr);
requires ecx==($vt);
requires ($vt)==(($AbsMem)[($r1)[($ptr)]])[1];
requires tag(($vt))==?DENSE_TAG;
requires ($abs)==($r1)[($ptr)];
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires Pointer(($r1),($ptr),($abs));
requires TV(($ptr));
requires Gray(($color)[($ptr)]);
requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
requires StackTop<=ColorBase;
requires SMemRequire(100,($sMem),esp);

modifies $r2,$color;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
modifies StackTop,$gcMem;

ensures old(StackTop)<=StackTop&&StackTop<=ColorBase;
ensures (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures RExtend(old($r2),($r2));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($abs))==>Value(VFieldPtr(($abs),j),($r2),($gcMem)[($ptr)+4*j],(($AbsMem)[($toAbs)[($ptr)]])[j])));

ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 422
  var save3:int;
#line 422
  var save2:int;
#line 422
  var save1:int;
#line 422
  var ptr:int;
#line 456
  assert TO(numFields(($r1)[($ptr)]));
  call ptr:=Mov(ebx);
  
  call esi:=Mov(2);
  assert TVT(($r1)[($ptr)],($vt));
  assert TVL(($r1)[($ptr)]);
  call ebp:=RoLoad32(ecx+?VT_BASE_LENGTH);
  call edx:=RoLoad32(ecx+?VT_MASK);
  call edi:=Mov(ebx);
  call edi:=Add(edi,8);
  call ebp:=Add(ebp,ebx);
  
  
  loop:
  assert TO(esi);
  assert (edi==($ptr)+4*esi&&ebp==($ptr)+4*numFields(($r1)[($ptr)]))&&edx==mask(($vt));
  assert 2<=esi&&esi<=numFields(($r1)[($ptr)]);
  assert Pointer(($r2),($ptr),($r1)[($ptr)]);
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert old(StackTop)<=StackTop&&StackTop<=ColorBase;
  assert (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
  assert Gray(($color)[($ptr)]);
  assert RExtend(old($r2),($r2));
  assert ObjInvPartial(($ptr),0,esi,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ObjInvPartial(($ptr),esi,numFields(($r1)[($ptr)]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ($toAbs)[($ptr)]!=NO_ABS&&($toAbs)[($ptr)]==($r1)[($ptr)];
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 485
  if(edi>=ebp)
  {
#line 485
    goto loopEnd;
  }
#line 486
  havoc $Eip;
#line 486
  if(esi>=30)
  {
#line 486
    goto loopEnd;
  }
  assert TO(0)&&TO(1);
  assert TVT(($r1)[($ptr)],($vt));
  assert TVL(($r1)[($ptr)]);
  call ecx:=Lea(esi+2);
  call ebx:=Mov(edx);
  call ebx:=Shr(ebx,ecx);
  call ebx:=And(ebx,1);
  havoc $Eip;
#line 495
  if(ebx!=1)
  {
#line 495
    goto skip1;
  }
#line 496
  call gcLoad(edi);
#line 496
  call ecx:=Load(edi);
  
  
  havoc $Eip;
#line 499
  if(ecx<GcLo)
  {
#line 499
    goto skip2;
  }
#line 500
  havoc $Eip;
#line 500
  if(ecx>GcHi)
  {
#line 500
    goto skip2;
  }
#line 501
  assert TV(ecx-4);
  call reach(($toAbs)[($ptr)],esi,($Time));
  
  call save1:=Mov(esi);
  call save2:=Mov(ebp);
  call save3:=Mov(edx);
  call visit(ecx,(($AbsMem)[($toAbs)[($ptr)]])[esi],($ptr));
  call esi:=Mov(save1);
  call ebp:=Mov(save2);
  call edx:=Mov(save3);
  call edi:=Mov(ptr);
  call edi:=Lea(edi+4*esi);
  skip2:
  skip1:
  
  call esi:=Add(esi,1);
  call edi:=Add(edi,4);
  havoc $Eip;
#line 518
  goto loop;
  loopEnd:
  
  assert TVT(($r1)[($ptr)],($vt));
  assert TVL(($r1)[($ptr)]);
  assert TO(1);
}

procedure scanObjectSparse($vt:int,$ptr:int,$abs:int)
#line 526

requires ebx==($ptr);
requires ecx==($vt);
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
requires StackTop<=ColorBase;
requires Pointer(($r1),($ptr),($abs));
requires TV(($ptr));
requires Gray(($color)[($ptr)]);
requires ($vt)==(($AbsMem)[($r1)[($ptr)]])[1];
requires tag(($vt))==?SPARSE_TAG;
requires SMemRequire(100,($sMem),esp);
modifies $r2,$color,StackTop,$gcMem;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
ensures old(StackTop)<=StackTop&&StackTop<=ColorBase;
ensures (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
ensures RExtend(old($r2),($r2));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($abs))==>Value(VFieldPtr(($abs),j),($r2),($gcMem)[($ptr)+4*j],(($AbsMem)[($toAbs)[($ptr)]])[j])));

ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 526
  var ptr:int;
#line 526
  var save4:int;
#line 526
  var save3:int;
#line 526
  var save2:int;
#line 526
  var save1:int;
#line 556
  call ptr:=Mov(ebx);
  assert TO(numFields(($r1)[($ptr)]));
  
  call esi:=Mov(1);
  
  assert TO(numFields(($r1)[($ptr)]));
  assert TVT(($r1)[($ptr)],($vt));
  call ebp:=RoLoad32(ecx+?VT_BASE_LENGTH);
  call edx:=RoLoad32(ecx+?VT_MASK);
  assert TVL(($r1)[($ptr)]);
  
  call esi:=Mov(1);
  
  loop:
  assert edx==mask(($vt))&&ebp==4*numFields(($r1)[($ptr)]);
  assert (TSlot(esi)&&0<esi)&&esi<=8;
  assert Pointer(($r2),($ptr),($r1)[($ptr)]);
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert old(StackTop)<=StackTop&&StackTop<=ColorBase;
  assert (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
  assert Gray(($color)[($ptr)]);
  assert RExtend(old($r2),($r2));
  assert ObjInvBase(($ptr),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($r1)[($ptr)])==>(between(1,esi,fieldToSlot(($vt),j-2))==>ObjInvField(($ptr),j,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice)))));
  
  
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($r1)[($ptr)])==>(!between(1,esi,fieldToSlot(($vt),j-2))==>ObjInvField(($ptr),j,($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice)))));
  
  
  assert ($toAbs)[($ptr)]!=NO_ABS&&($toAbs)[($ptr)]==($r1)[($ptr)];
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 589
  if(esi>=8)
  {
#line 589
    goto loopEnd;
  }
  assert TO(0)&&TO(1);
  assert TVT(($r1)[($ptr)],($vt));
  assert TO(getNib(edx,4*esi)+1);
  
  
  call ecx:=Mov(0);
  call ecx:=Lea(ecx+4*esi);
  call ebx:=Mov(edx);
  call ebx:=Shr(ebx,ecx);
  call ebx:=And(ebx,15);
  havoc $Eip;
#line 601
  if(ebx==0)
  {
#line 601
    goto skip1;
  }
#line 602
  call eax:=Mov(ptr);
  call gcLoad(eax+4*ebx+4);
#line 603
  call ecx:=Load(eax+4*ebx+4);
  
  
  havoc $Eip;
#line 606
  if(ecx<GcLo)
  {
#line 606
    goto skip2;
  }
#line 607
  havoc $Eip;
#line 607
  if(ecx>GcHi)
  {
#line 607
    goto skip2;
  }
#line 608
  assert TV(ecx-4);
  call reach(($toAbs)[($ptr)],1+ebx,($Time));
  
  call save1:=Mov(esi);
  call save2:=Mov(ebp);
  call save3:=Mov(edx);
  call save4:=Mov(ebx);
  call visit(ecx,(($AbsMem)[($toAbs)[($ptr)]])[1+getNib(edx,4*esi)],($ptr));
  call esi:=Mov(save1);
  call ebp:=Mov(save2);
  call edx:=Mov(save3);
  call ebx:=Mov(save4);
  skip2:
  skip1:
  
  call esi:=Add(esi,1);
  havoc $Eip;
#line 624
  goto loop;
  loopEnd:
  
  assert TVT(($r1)[($ptr)],($vt));
  assert TVL(($r1)[($ptr)]);
  assert TO(1);
}

procedure scanObjectOtherVectorNoPointers($vt:int,$ptr:int,$abs:int)
#line 632

requires ebx==($ptr);
requires ecx==($vt);
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
requires StackTop<=ColorBase;
requires Pointer(($r1),($ptr),($abs));
requires TV(($ptr));
requires Gray(($color)[($ptr)]);
requires ($vt)==(($AbsMem)[($r1)[($ptr)]])[1];
requires tag(($vt))==?OTHER_VECTOR_TAG;
requires arrayOf(($vt))!=?TYPE_STRUCT||(arrayOf(($vt))==?TYPE_STRUCT&&mask(arrayElementClass(($vt)))==?SPARSE_TAG);
requires SMemRequire(100,($sMem),esp);
modifies $r2,$color,StackTop,$gcMem;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
ensures old(StackTop)<=StackTop&&StackTop<=ColorBase;
ensures (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
ensures RExtend(old($r2),($r2));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($abs))==>Value(VFieldPtr(($abs),j),($r2),($gcMem)[($ptr)+4*j],(($AbsMem)[($toAbs)[($ptr)]])[j])));

ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
  assert TO(numFields(($r1)[($ptr)]));
  assert TVT(($r1)[($ptr)],($vt));
}

procedure scanObjectOtherVectorPointersSparseFields($vt:int,$ptr:int,$abs:int,$jj:int,$index:int)
#line 662

requires edx==mask(arrayElementClass(($vt)));
requires edi==($jj);
requires ebp==($ptr);
requires tag(($vt))==?OTHER_VECTOR_TAG;
requires arrayOf(($vt))==?TYPE_STRUCT&&tag(arrayElementClass(($vt)))==?SPARSE_TAG;
requires TO(($jj))&&($jj)==baseWords(($vt))+Mult(arrayElementWords(($vt)),($index));
requires ($jj)<numFields(($abs));
requires TO(($index))&&0<=($index);
requires Mult(arrayElementWords(($vt)),($index))>=0;
requires reached(($abs),($Time));
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
requires StackTop<=ColorBase;
requires Pointer(($r1),($ptr),($abs));
requires TV(($ptr));
requires Gray(($color)[($ptr)]);
requires ($vt)==(($AbsMem)[($r1)[($ptr)]])[1];
requires ObjInvPartial(($ptr),0,($jj),($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
requires ObjInvPartial(($ptr),($jj),numFields(($r1)[($ptr)]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
requires ($toAbs)[($ptr)]!=NO_ABS&&($toAbs)[($ptr)]==($r1)[($ptr)];
requires (forall j:int::{TO(j)}TO(j)==>(between(0,arrayElementWords(($vt)),j-($jj))==>ObjInvField(($ptr),j,($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice))));


requires SMemRequire(100,($sMem),esp);

modifies $r2,$color,StackTop,$gcMem;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;

ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
ensures old(StackTop)<=StackTop&&StackTop<=ColorBase;
ensures (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
ensures Gray(($color)[($ptr)]);
ensures RExtend(old($r2),($r2));
ensures ObjInvPartial(($ptr),0,($jj),($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
ensures ObjInvPartial(($ptr),($jj)+arrayElementWords(($vt)),numFields(($r1)[($ptr)]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
ensures ($toAbs)[($ptr)]!=NO_ABS&&($toAbs)[($ptr)]==($r2)[($ptr)];
ensures (forall j:int::{TO(j)}TO(j)==>(between(0,arrayElementWords(($vt)),j-($jj))==>(between(1,8,fieldToSlot(arrayElementClass(($vt)),j-($jj)))==>ObjInvField(($ptr),j,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice)))));



ensures (forall j:int::{TO(j)}TO(j)==>(between(0,arrayElementWords(($vt)),j-($jj))==>(!between(1,8,fieldToSlot(arrayElementClass(($vt)),j-($jj)))==>ObjInvField(($ptr),j,($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice)))));



ensures edi==old(edi);
ensures edx==old(edx);
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 662
  var save5:int;
#line 662
  var save4:int;
#line 662
  var save3:int;
#line 662
  var save2:int;
#line 662
  var save1:int;
#line 720
  assert TO(numFields(($r2)[($ptr)]));
  assert TO(2);
  assert TVT(($r2)[($ptr)],($vt));
  assert TVL(($r2)[($ptr)]);
  
  call esi:=Mov(1);
  
  loop:
  assert (TSlot(esi)&&0<esi)&&esi<=8;
  assert edx==mask(arrayElementClass(($vt)));
  assert edi==($jj);
  assert ebp==($ptr);
  
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
  assert old(StackTop)<=StackTop&&StackTop<=ColorBase;
  assert (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
  
  assert Gray(($color)[($ptr)]);
  assert RExtend(old($r2),($r2));
  assert ObjInvPartial(($ptr),0,($jj),($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ObjInvPartial(($ptr),($jj)+arrayElementWords(($vt)),numFields(($r2)[($ptr)]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ($toAbs)[($ptr)]!=NO_ABS&&($toAbs)[($ptr)]==($r2)[($ptr)];
  assert (forall j:int::{TO(j)}TO(j)==>(between(0,arrayElementWords(($vt)),j-($jj))==>(between(1,esi,fieldToSlot(arrayElementClass(($vt)),j-($jj)))==>ObjInvField(($ptr),j,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice)))));
  
  
  
  assert (forall j:int::{TO(j)}TO(j)==>(between(0,arrayElementWords(($vt)),j-($jj))==>(!between(1,esi,fieldToSlot(arrayElementClass(($vt)),j-($jj)))==>ObjInvField(($ptr),j,($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice)))));
  
  
  
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 753
  if(esi>=8)
  {
#line 753
    goto loopEnd;
  }
  
  call ecx:=Mov(0);
  call ecx:=Lea(ecx+4*esi);
  call ebx:=Mov(edx);
  call ebx:=Shr(ebx,ecx);
  call ebx:=And(ebx,15);
  assert ebx==getNib(mask(arrayElementClass(($vt))),4*esi);
  
  
  havoc $Eip;
#line 764
  if(ebx==0)
  {
#line 764
    goto skip1;
  }
#line 765
  call ebx:=Sub(ebx,1);
  call ebx:=Add(ebx,edi);
  assert TO(ebx);
  
  call eax:=Mov(ebp);
  call gcLoad(eax+4*ebx);
#line 770
  call ecx:=Load(eax+4*ebx);
  
  havoc $Eip;
#line 772
  if(ecx<GcLo)
  {
#line 772
    goto skip2;
  }
#line 773
  havoc $Eip;
#line 773
  if(ecx>GcHi)
  {
#line 773
    goto skip2;
  }
  assert TV(ecx-4);
  call reach(($toAbs)[($ptr)],ebx,($Time));
  
  assert TO(0);
  assert TO(1);
  
  call save1:=Mov(esi);
  call save2:=Mov(edi);
  call save3:=Mov(edx);
  call save4:=Mov(ebx);
  call save5:=Mov(ebp);
  call visit(ecx,(($AbsMem)[($toAbs)[($ptr)]])[ebx],($ptr));
  call esi:=Mov(save1);
  call edi:=Mov(save2);
  call edx:=Mov(save3);
  call ebx:=Mov(save4);
  call ebp:=Mov(save5);
  
  skip2:
  skip1:
  call esi:=Add(esi,1);
  havoc $Eip;
#line 796
  goto loop;
  loopEnd:
}

procedure scanObjectOtherVectorPointers($vt:int,$ptr:int,$abs:int)
#line 800

requires ebx==($ptr);
requires ecx==($vt);
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
requires StackTop<=ColorBase;
requires Pointer(($r1),($ptr),($abs));
requires TV(($ptr));
requires Gray(($color)[($ptr)]);
requires ($vt)==(($AbsMem)[($r1)[($ptr)]])[1];
requires tag(($vt))==?OTHER_VECTOR_TAG;
requires arrayOf(($vt))==?TYPE_STRUCT&&tag(arrayElementClass(($vt)))==?SPARSE_TAG;
requires SMemRequire(100,($sMem),esp);
modifies $r2,$color,StackTop,$gcMem;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
ensures old(StackTop)<=StackTop&&StackTop<=ColorBase;
ensures (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
ensures RExtend(old($r2),($r2));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($abs))==>Value(VFieldPtr(($abs),j),($r2),($gcMem)[($ptr)+4*j],(($AbsMem)[($toAbs)[($ptr)]])[j])));

ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 800
  var $index:int;
#line 800
  var ptr:int;
#line 800
  var save2:int;
#line 800
  var save1:int;
#line 832
  assert TO(numFields(($r2)[($ptr)]));
  assert TO(2);
  assert TVT(($r2)[($ptr)],($vt));
  assert TVL(($r2)[($ptr)]);
  
  $index:=0;
  
  call ptr:=Mov(ebx);
  call edx:=Mov(ebx);
  
  call edi:=RoLoad32(ecx+?VT_BASE_LENGTH);
  call ebx:=RoLoad32(ecx+?VT_ARRAY_ELEMENT_SIZE);
  call gcLoad(edx+8);
#line 844
  call edx:=Load(edx+8);
  call eax:=Mov(ebx);
  call eax,edx:=MulChecked(eax,edx);
  call eax:=AddChecked(eax,edi);
  call eax:=AddChecked(eax,3);
  call edx:=Mov(3);
  call edx:=Not(edx);
  call eax:=And(eax,edx);
  call ebp:=Mov(eax);
  call edi:=Shr(edi,2);
  
  call ebx:=Shr(ebx,2);
  
  assert TVM(ebx,0);
  call edx:=RoLoad32(ecx+?VT_ARRAY_ELEMENT_CLASS);
  call edx:=RoLoad32(edx+?VT_MASK);
  
  
  loop:
  assert TO(($index))&&0<=($index);
  assert Mult(ebx,($index))>=0;
  assert TO(edi)&&edi==baseWords(($vt))+Mult(ebx,($index));
  
  assert ebp==4*numFields(($r2)[($ptr)]);
  assert edx==mask(arrayElementClass(($vt)));
  assert ebx==arrayElementWords(($vt));
  assert 4*edi<=ebp;
  
  assert Pointer(($r2),($ptr),($r1)[($ptr)]);
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert old(StackTop)<=StackTop&&StackTop<=ColorBase;
  assert (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
  assert Gray(($color)[($ptr)]);
  assert RExtend(old($r2),($r2));
  assert ObjInvPartial(($ptr),0,edi,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ObjInvPartial(($ptr),edi,numFields(($r2)[($ptr)]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ($toAbs)[($ptr)]!=NO_ABS&&($toAbs)[($ptr)]==($r2)[($ptr)];
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  call eax:=Mov(0);
  call eax:=Lea(eax+4*edi);
  havoc $Eip;
#line 886
  if(eax>=ebp)
  {
#line 886
    goto loopEnd;
  }
  call save1:=Mov(ebx);
  call save2:=Mov(ebp);
  call ebp:=Mov(ptr);
  call scanObjectOtherVectorPointersSparseFields(($vt),($ptr),($abs),edi,($index));
  call ebx:=Mov(save1);
  call ebp:=Mov(save2);
  
  assert TVM3(ebx,($index),1);
  assert TVM(ebx,($index));
  $index:=($index)+1;
  call edi:=Add(edi,ebx);
  havoc $Eip;
#line 899
  goto loop;
  loopEnd:
  
  assert TO(1);
}

procedure scanObjectPtrArray($vt:int,$ptr:int,$abs:int)
#line 905

requires ebx==($ptr);
requires ecx==($vt);
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
requires StackTop<=ColorBase;
requires Pointer(($r1),($ptr),($abs));
requires TV(($ptr));
requires Gray(($color)[($ptr)]);
requires ($vt)==(($AbsMem)[($r1)[($ptr)]])[1];
requires tag(($vt))==?PTR_ARRAY_TAG;
requires SMemRequire(100,($sMem),esp);
modifies $r2,$color,StackTop,$gcMem;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
ensures old(StackTop)<=StackTop&&StackTop<=ColorBase;
ensures (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
ensures RExtend(old($r2),($r2));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($abs))==>Value(VFieldPtr(($abs),j),($r2),($gcMem)[($ptr)+4*j],(($AbsMem)[($toAbs)[($ptr)]])[j])));

ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 905
  var save2:int;
#line 905
  var save1:int;
#line 905
  var $ind:int;
#line 934
  assert TO(numFields(($r1)[($ptr)]));
  assert TO(3);
  assert TVT(($r1)[($ptr)],($vt));
  assert TVL(($r1)[($ptr)]);
  
  call gcLoad(ebx+12);
#line 939
  call ebp:=Load(ebx+12);
  call esi:=RoLoad32(ecx+?VT_BASE_LENGTH);
  
  call ebp:=AddChecked(ebp,ebp);
  call ebp:=AddChecked(ebp,ebp);
  call ebp:=AddChecked(ebp,esi);
  call ebp:=AddChecked(ebp,3);
  call eax:=Mov(3);
  call eax:=Not(eax);
  call ebp:=And(ebp,eax);
  call esi:=Shr(esi,2);
  $ind:=esi;
  
  call edi:=Lea(ebx+4*esi);
  call ebp:=Add(ebp,ebx);
  
  
  loop:
  assert edi==($ptr)+4*($ind);
  assert ebp==($ptr)+4*numFields(($r1)[($ptr)]);
  assert TO(($ind))&&baseWords(($vt))<=($ind);
  assert Pointer(($r2),($ptr),($r1)[($ptr)]);
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert old(StackTop)<=StackTop&&StackTop<=ColorBase;
  assert (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
  assert Gray(($color)[($ptr)]);
  assert RExtend(old($r2),($r2));
  assert ObjInvPartial(($ptr),0,($ind),($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ObjInvPartial(($ptr),($ind),numFields(($r1)[($ptr)]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ($toAbs)[($ptr)]!=NO_ABS&&($toAbs)[($ptr)]==($r1)[($ptr)];
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 972
  if(edi>=ebp)
  {
#line 972
    goto loopEnd;
  }
  assert (TO(0)&&TO(1))&&TO(3);
  assert TVT(($r1)[($ptr)],($vt));
  assert TVL(($r1)[($ptr)]);
  call gcLoad(edi);
#line 977
  call ecx:=Load(edi);
  
  havoc $Eip;
#line 979
  if(ecx<GcLo)
  {
#line 979
    goto skip1;
  }
#line 980
  havoc $Eip;
#line 980
  if(ecx>GcHi)
  {
#line 980
    goto skip1;
  }
#line 981
  assert TV(ecx-4);
  call reach(($toAbs)[($ptr)],($ind),($Time));
  
  call save1:=Mov(edi);
  call save2:=Mov(ebp);
  call visit(ecx,(($AbsMem)[($toAbs)[($ptr)]])[($ind)],($ptr));
  call edi:=Mov(save1);
  call ebp:=Mov(save2);
  skip1:
  
  $ind:=($ind)+1;
  call edi:=Add(edi,4);
  havoc $Eip;
#line 993
  goto loop;
  loopEnd:
}

procedure scanObjectPtrVector($vt:int,$ptr:int,$abs:int)
#line 997

requires ebx==($ptr);
requires ecx==($vt);
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
requires StackTop<=ColorBase;
requires Pointer(($r1),($ptr),($abs));
requires TV(($ptr));
requires Gray(($color)[($ptr)]);
requires ($vt)==(($AbsMem)[($r1)[($ptr)]])[1];
requires tag(($vt))==?PTR_VECTOR_TAG;
requires SMemRequire(100,($sMem),esp);
modifies $r2,$color,StackTop,$gcMem;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
ensures old(StackTop)<=StackTop&&StackTop<=ColorBase;
ensures (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
ensures RExtend(old($r2),($r2));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($abs))==>Value(VFieldPtr(($abs),j),($r2),($gcMem)[($ptr)+4*j],(($AbsMem)[($toAbs)[($ptr)]])[j])));

ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 997
  var save2:int;
#line 997
  var save1:int;
#line 997
  var $ind:int;
#line 1025
  assert TO(numFields(($r1)[($ptr)]));
  assert TO(2);
  assert TVT(($r1)[($ptr)],($vt));
  assert TVL(($r1)[($ptr)]);
  
  call gcLoad(ebx+8);
#line 1030
  call ebp:=Load(ebx+8);
  call esi:=RoLoad32(ecx+?VT_BASE_LENGTH);
  
  call ebp:=AddChecked(ebp,ebp);
  call ebp:=AddChecked(ebp,ebp);
  call ebp:=AddChecked(ebp,esi);
  call ebp:=AddChecked(ebp,3);
  call eax:=Mov(3);
  call eax:=Not(eax);
  call ebp:=And(ebp,eax);
  call esi:=Shr(esi,2);
  $ind:=esi;
  
  call edi:=Lea(ebx+4*esi);
  call ebp:=Add(ebp,ebx);
  
  
  loop:
  assert edi==($ptr)+4*($ind);
  assert ebp==($ptr)+4*numFields(($r1)[($ptr)]);
  assert TO(($ind))&&baseWords(($vt))<=($ind);
  assert Pointer(($r2),($ptr),($r1)[($ptr)]);
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert old(StackTop)<=StackTop&&StackTop<=ColorBase;
  assert (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
  assert Gray(($color)[($ptr)]);
  assert RExtend(old($r2),($r2));
  assert ObjInvPartial(($ptr),0,($ind),($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ObjInvPartial(($ptr),($ind),numFields(($r1)[($ptr)]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ($toAbs)[($ptr)]!=NO_ABS&&($toAbs)[($ptr)]==($r1)[($ptr)];
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 1063
  if(edi>=ebp)
  {
#line 1063
    goto loopEnd;
  }
  assert (TO(0)&&TO(1))&&TO(2);
  assert TVT(($r1)[($ptr)],($vt));
  call gcLoad(edi);
#line 1067
  call ecx:=Load(edi);
  
  havoc $Eip;
#line 1069
  if(ecx<GcLo)
  {
#line 1069
    goto skip1;
  }
#line 1070
  havoc $Eip;
#line 1070
  if(ecx>GcHi)
  {
#line 1070
    goto skip1;
  }
#line 1071
  assert TV(ecx-4);
  call reach(($toAbs)[($ptr)],($ind),($Time));
  
  call save1:=Mov(edi);
  call save2:=Mov(ebp);
  call visit(ecx,(($AbsMem)[($toAbs)[($ptr)]])[($ind)],($ptr));
  call edi:=Mov(save1);
  call ebp:=Mov(save2);
  skip1:
  
  $ind:=($ind)+1;
  call edi:=Add(edi,4);
  havoc $Eip;
#line 1083
  goto loop;
  loopEnd:
}

procedure scanObjectOtherArrayNoPointers($vt:int,$ptr:int,$abs:int)
#line 1087

requires ebx==($ptr);
requires ecx==($vt);
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
requires StackTop<=ColorBase;
requires Pointer(($r1),($ptr),($abs));
requires TV(($ptr));
requires Gray(($color)[($ptr)]);
requires ($vt)==(($AbsMem)[($r1)[($ptr)]])[1];
requires tag(($vt))==?OTHER_ARRAY_TAG;
requires arrayOf(($vt))!=?TYPE_STRUCT;
requires SMemRequire(100,($sMem),esp);
modifies $r2,$color,StackTop,$gcMem;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
ensures old(StackTop)<=StackTop&&StackTop<=ColorBase;
ensures (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
ensures RExtend(old($r2),($r2));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($abs))==>Value(VFieldPtr(($abs),j),($r2),($gcMem)[($ptr)+4*j],(($AbsMem)[($toAbs)[($ptr)]])[j])));

ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
  assert TO(numFields(($r1)[($ptr)]));
  assert TVT(($r1)[($ptr)],($vt));
}

procedure scanObjectString($vt:int,$ptr:int,$abs:int)
#line 1117

requires ebx==($ptr);
requires ecx==($vt);
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
requires StackTop<=ColorBase;
requires Pointer(($r1),($ptr),($abs));
requires TV(($ptr));
requires Gray(($color)[($ptr)]);
requires ($vt)==(($AbsMem)[($r1)[($ptr)]])[1];
requires tag(($vt))==?STRING_TAG;
requires SMemRequire(100,($sMem),esp);
modifies $r2,$color,StackTop,$gcMem;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
ensures old(StackTop)<=StackTop&&StackTop<=ColorBase;
ensures (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
ensures RExtend(old($r2),($r2));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($abs))==>Value(VFieldPtr(($abs),j),($r2),($gcMem)[($ptr)+4*j],(($AbsMem)[($toAbs)[($ptr)]])[j])));

ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
  assert TO(numFields(($r1)[($ptr)]));
  assert TVT(($r1)[($ptr)],($vt));
}

procedure scanObjectOther($vt:int,$ptr:int,$abs:int)
#line 1146

requires ebx==($ptr);
requires ecx==($vt);
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
requires StackTop<=ColorBase;
requires Pointer(($r1),($ptr),($abs));
requires TV(($ptr));
requires Gray(($color)[($ptr)]);
requires ($vt)==(($AbsMem)[($r1)[($ptr)]])[1];
requires isOtherTag(tag(($vt)));
requires SMemRequire(100,($sMem),esp);
modifies $r2,$color,StackTop,$gcMem;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
ensures old(StackTop)<=StackTop&&StackTop<=ColorBase;
ensures (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
ensures RExtend(old($r2),($r2));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($abs))==>Value(VFieldPtr(($abs),j),($r2),($gcMem)[($ptr)+4*j],(($AbsMem)[($toAbs)[($ptr)]])[j])));

ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1146
  var ptr:int;
#line 1146
  var save5:int;
#line 1146
  var save4:int;
#line 1146
  var save3:int;
#line 1146
  var save2:int;
#line 1146
  var save1:int;
#line 1177
  call ptr:=Mov(ebx);
  
  assert TO(numFields(($r1)[($ptr)]));
  assert TVT(($r1)[($ptr)],($vt));
  assert TVL(($r1)[($ptr)]);
  
  call edx:=RoLoad32(ecx+?VT_MASK);
  call edi:=RoLoad32(edx);
  call ebp:=RoLoad32(ecx+?VT_BASE_LENGTH);
  
  call esi:=Mov(1);
  
  
  loop:
  assert ebp==4*numFields(($r1)[($ptr)]);
  assert edx==mask(($vt));
  assert edi==ro32(mask(($vt)));
  assert (TSlot(esi)&&0<esi)&&esi<=ro32(mask(($vt)))+1;
  assert Pointer(($r2),($ptr),($r1)[($ptr)]);
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert old(StackTop)<=StackTop&&StackTop<=ColorBase;
  assert (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
  assert Gray(($color)[($ptr)]);
  assert RExtend(old($r2),($r2));
  assert ObjInvBase(($ptr),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($r1)[($ptr)])==>(between(1,esi,fieldToSlot(($vt),j))==>ObjInvField(($ptr),j,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice)))));
  
  
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($r1)[($ptr)])==>(!between(1,esi,fieldToSlot(($vt),j))==>ObjInvField(($ptr),j,($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice)))));
  
  
  assert ($toAbs)[($ptr)]!=NO_ABS&&($toAbs)[($ptr)]==($r1)[($ptr)];
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 1212
  if(esi>edi)
  {
#line 1212
    goto loopEnd;
  }
  assert TO(0)&&TO(1);
  assert TVT(($r1)[($ptr)],($vt));
  assert TVL(($r1)[($ptr)]);
  assert TO(ro32(mask(($vt))+4*esi)+1);
  call ebx:=RoLoad32(edx+4*esi);
  
  havoc $Eip;
#line 1220
  if(ebx==0)
  {
#line 1220
    goto skip1;
  }
#line 1221
  call eax:=Mov(ptr);
  call gcLoad(eax+4*ebx+4);
#line 1222
  call ecx:=Load(eax+4*ebx+4);
  
  
  havoc $Eip;
#line 1225
  if(ecx<GcLo)
  {
#line 1225
    goto skip2;
  }
#line 1226
  havoc $Eip;
#line 1226
  if(ecx>GcHi)
  {
#line 1226
    goto skip2;
  }
#line 1227
  assert TV(ecx-4);
  call reach(($toAbs)[($ptr)],1+ro32(edx+4*esi),($Time));
  
  call save1:=Mov(ebx);
  call save2:=Mov(esi);
  call save3:=Mov(edi);
  call save4:=Mov(ebp);
  call save5:=Mov(edx);
  call visit(ecx,(($AbsMem)[($toAbs)[($ptr)]])[1+ro32(edx+4*esi)],($ptr));
  call ebx:=Mov(save1);
  call esi:=Mov(save2);
  call edi:=Mov(save3);
  call ebp:=Mov(save4);
  call edx:=Mov(save5);
  skip2:
  skip1:
  call esi:=AddChecked(esi,1);
  havoc $Eip;
#line 1244
  goto loop;
  loopEnd:
  
  assert TVT(($r1)[($ptr)],($vt));
  assert TVL(($r1)[($ptr)]);
}

procedure scanObject($ptr:int,$abs:int)
#line 1251

requires ebx==($ptr);
requires ($abs)==($r1)[($ptr)];
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires Pointer(($r1),($ptr),($abs));
requires TV(($ptr));
requires Gray(($color)[($ptr)]);
requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
requires StackTop<=ColorBase;
requires SMemRequire(100,($sMem),esp);

modifies $r2,$color;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
modifies StackTop,$gcMem;

ensures old(StackTop)<=StackTop&&StackTop<=ColorBase;
ensures (forall ss:int::{TV(ss)}TV(ss)==>(HeapHi<=ss&&ss<old(StackTop)==>($gcMem)[ss]==old($gcMem)[ss]));
ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),($ptr));
ensures RExtend(old($r2),($r2));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($abs))==>Value(VFieldPtr(($abs),j),($r2),($gcMem)[($ptr)+4*j],(($AbsMem)[($toAbs)[($ptr)]])[j])));

ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1251
  var $vt:int;
#line 1279
  assert TO(1);
  call gcLoad(ebx+4);
#line 1280
  call ecx:=Load(ebx+4);
  $vt:=ecx;
  
  assert TO(numFields(($r1)[($ptr)]));
  
  call readTag(($toAbs)[($ptr)],($vt));
  
  havoc $Eip;
#line 1287
  if(eax!=?SPARSE_TAG)
  {
#line 1287
    goto skip1;
  }
#line 1288
  call scanObjectSparse(($vt),($ptr),($abs));
  havoc $Eip;
#line 1289
  goto end;
  skip1:
  
  havoc $Eip;
#line 1292
  if(eax!=?DENSE_TAG)
  {
#line 1292
    goto skip2;
  }
#line 1293
  call scanObjectDense(($vt),($ptr),($abs));
  havoc $Eip;
#line 1294
  goto end;
  skip2:
  
  havoc $Eip;
#line 1297
  if(eax!=?STRING_TAG)
  {
#line 1297
    goto skip3;
  }
#line 1298
  call scanObjectString(($vt),($ptr),($abs));
  havoc $Eip;
#line 1299
  goto end;
  skip3:
  
  havoc $Eip;
#line 1302
  if(eax!=?PTR_VECTOR_TAG)
  {
#line 1302
    goto skip4;
  }
#line 1303
  call scanObjectPtrVector(($vt),($ptr),($abs));
  havoc $Eip;
#line 1304
  goto end;
  skip4:
  
  havoc $Eip;
#line 1307
  if(eax!=?OTHER_VECTOR_TAG)
  {
#line 1307
    goto skip5;
  }
#line 1308
  call readArrayOf(($r1)[($ptr)],($vt));
  call readElementInfo(($r1)[($ptr)],($vt));
  havoc $Eip;
#line 1310
  if(ebp!=?TYPE_STRUCT)
  {
#line 1310
    goto noPoint;
  }
#line 1311
  havoc $Eip;
#line 1311
  if(ebp!=?TYPE_STRUCT)
  {
#line 1311
    goto vecSkip1;
  }
#line 1312
  havoc $Eip;
#line 1312
  if(edi!=?SPARSE_TAG)
  {
#line 1312
    goto vecSkip1;
  }
#line 1313
  noPoint:
  call scanObjectOtherVectorNoPointers(($vt),($ptr),($abs));
  havoc $Eip;
#line 1315
  goto end;
  vecSkip1:
  havoc $Eip;
#line 1317
  if(ebp!=?TYPE_STRUCT)
  {
#line 1317
    goto vecSkip2;
  }
  
  call eax:=Mov(edi);
  call eax:=And(eax,15);
  havoc $Eip;
#line 1322
  if(eax!=?SPARSE_TAG)
  {
#line 1322
    goto vecSkip2;
  }
  call scanObjectOtherVectorPointers(($vt),($ptr),($abs));
  havoc $Eip;
#line 1325
  goto end;
  
  vecSkip2:
  assert !((ebp!=?TYPE_STRUCT||(ebp==?TYPE_STRUCT&&edi==?SPARSE_TAG))||(ebp==?TYPE_STRUCT&&tag(arrayElementClass(($vt)))==?SPARSE_TAG));
  
  
  
  call eax:=Mov(1431633986);
  call _call();
#line 1333
  call Call();
#line 1333
  call DebugBreak();
  
  skip5:
  
  havoc $Eip;
#line 1337
  if(eax!=?PTR_ARRAY_TAG)
  {
#line 1337
    goto skip6;
  }
#line 1338
  call scanObjectPtrArray(($vt),($ptr),($abs));
  havoc $Eip;
#line 1339
  goto end;
  skip6:
  
  havoc $Eip;
#line 1342
  if(eax!=?OTHER_ARRAY_TAG)
  {
#line 1342
    goto skip7;
  }
#line 1343
  call readArrayOf(($r1)[($ptr)],($vt));
  havoc $Eip;
#line 1344
  if(ebp==?TYPE_STRUCT)
  {
#line 1344
    goto arraySkip1;
  }
#line 1345
  call scanObjectOtherArrayNoPointers(($vt),($ptr),($abs));
  havoc $Eip;
#line 1346
  goto end;
  arraySkip1:
  call eax:=Mov(1431633987);
  call _call();
#line 1349
  call Call();
#line 1349
  call DebugBreak();
  havoc $Eip;
#line 1350
  goto end;
  skip7:
  
  call scanObjectOther(($vt),($ptr),($abs));
  end:
}

procedure FromInteriorPointer($iptr:int,$offset:int,$abs:int)
#line 1357

requires ecx==($iptr);
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires 0<=($offset)&&($offset)<=4*numFields(($abs))-4;
requires Pointer(($r1),($iptr)-($offset)-4,($abs));
requires SMemRequireRA(100,($sMem),esp,($RET));
modifies $Eip,eax,ebx,ecx,edx,esp,$Mem,$sMem;
ensures eax==($offset);
ensures SMemEnsure(($sMem),old($sMem),esp,old(esp));
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
{
#line 1357
  var save2:int;
#line 1357
  var save1:int;
#line 1372
  call eax:=Mov(0);
  
  loop:
  assert ($iptr)-($offset)<=($iptr)-eax&&($iptr)-eax<=($iptr);
  assert TV(($iptr)-eax-4)&&TV(($iptr)-($offset)-4);
  assert ecx==($iptr);
  
  assert TV(?gcLo);
  assert TO(($iptr)-eax-4-HeapLo);
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  
  call ebx:=Mov(ecx);
  call ebx:=Sub(ebx,eax);
  call ebx:=Sub(ebx,4);
  call ebx:=Sub(ebx,HeapLo);
  call edx:=Mov(ebx);
  call edx:=And(edx,3);
  call __andAligned(ebx);
  call __addAligned(HeapLo,ebx);
  havoc $Eip;
#line 1391
  if(edx!=0)
  {
    goto skip1;
  }
  
  call save1:=Mov(eax);
  call save2:=Mov(ecx);
  call eax:=Mov(ColorBase);
  call bb4GetColor(($color),HeapLo,HeapLo,HeapLo,HeapHi,($iptr)-save1-4,ColorBase,HeapLo);
  call eax:=Mov(save1);
  call ecx:=Mov(save2);
  
  havoc $Eip;
#line 1403
  if(ebx==0)
  {
#line 1403
    goto skip1;
  }
#line 1404
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 1404
  call Ret(old($RET));
#line 1404
  return;
  skip1:
  call eax:=Add(eax,1);
  havoc $Eip;
#line 1407
  goto loop;
}

procedure scanStackUpdateInvs($s:int,$r:[int]int,$f1:int,$f2:int,$frame:int,$addr:int,$v:int)
#line 1410

requires (($FrameSlices)[($s)])[($addr)]==($frame);
requires !(($f1)<=($frame)&&($frame)<($f2));
requires (forall f:int::{TV(f)}TV(f)==>(($f1)<=f&&f<($f2)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures (forall f:int::{TV(f)}TV(f)==>(($f1)<=f&&f<($f2)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],(($fMems)[($s)])[($addr):=($v)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));
{
  
  assert TO(0)&&TO(1);
  assert (forall f:int::{TV(f)}TV(f)==>TVF((($FrameLayouts)[($s)])[f])&&TVFT(f));
}

procedure scanStackWordDense($s:int,$frame:int,$addr:int,$desc:int,$addrp:int,$p:int,$args:int)
#line 1422

requires ecx==($addrp);
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
requires StackTop<=ColorBase;
requires ($desc)==frameDescriptor((($FrameLayouts)[($s)])[($frame)]);
requires ($addr)==(($FrameAddrs)[($s)])[($frame)];
requires (($FrameSlices)[($s)])[($addrp)]==($frame);
requires ($args)==frameLayoutArgs((($FrameLayouts)[($s)])[($frame)]);
requires ($addrp)==($addr)+4*($p);
requires (getBit(($desc),0)&&!getBit(($desc),1))&&and(shr(($desc),6),1023)==0;
requires frameHasPtr((($FrameLayouts)[($s)])[($frame)],($p));
requires (0<=($frame)&&($frame)<($FrameCounts)[($s)])&&TV(($frame));
requires (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<($frame)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

requires (forall f:int::{TV(f)}TV(f)==>(($frame)<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

requires MutatorFrameInvAddrs(($s),($frame),(($FrameLayouts)[($s)])[($frame)],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)],($fMems)[($s)]);

requires (($p)<=1+($args)&&($p)>($args)-1-16)&&TO(($p));
requires (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<($frame)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

requires (forall f:int::{TV(f)}TV(f)==>(($frame)<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

requires FrameNextInv(($frame),(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4],(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]],($FrameAddrs)[($s)],($FrameLayouts)[($s)]);
requires (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&j<=($p)==>(($FrameSlices)[($s)])[($addr)+4*j]==($frame)&&InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r1),(($fMems)[($s)])[($addr)+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[($addr)+4*j])));



requires (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&j>($p)==>(($FrameSlices)[($s)])[($addr)+4*j]==($frame)&&InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r2),(($fMems)[($s)])[($addr)+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[($addr)+4*j])));



requires SMemRequire(104,($sMem),esp);

modifies $r2,$color;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
modifies StackTop,$gcMem;

ensures (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<($frame)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures (forall f:int::{TV(f)}TV(f)==>(($frame)<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures FrameNextInv(($frame),(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4],(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]],($FrameAddrs)[($s)],($FrameLayouts)[($s)]);
ensures (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&j<=($p)-1==>(($FrameSlices)[($s)])[($addr)+4*j]==($frame)&&InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r1),(($fMems)[($s)])[($addr)+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[($addr)+4*j])));



ensures (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&j>($p)-1==>(($FrameSlices)[($s)])[($addr)+4*j]==($frame)&&InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r2),(($fMems)[($s)])[($addr)+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[($addr)+4*j])));



ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
ensures StackTop<=ColorBase;
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1422
  var offset:int;
#line 1422
  var v:int;
#line 1422
  var save1:int;
#line 1485
  assert TVF((($FrameLayouts)[($s)])[($frame)]);
  assert TV((($fMems)[($s)])[($addrp)]-4);
  
  call fLoad(($s),ecx);
#line 1488
  call eax:=Load(ecx);
  
  havoc $Eip;
#line 1490
  if(eax<GcLo)
  {
#line 1490
    goto skip1;
  }
#line 1491
  havoc $Eip;
#line 1491
  if(eax>GcHi)
  {
#line 1491
    goto skip1;
  }
#line 1492
  call save1:=Mov(ecx);
  call v:=Mov(eax);
  
  call ecx:=Mov(eax);
  call _call();
#line 1496
  call Call();
#line 1496
  call FromInteriorPointer(v,(($FrameOffsets)[($s)])[($addrp)],((($FrameAbss)[($s)])[($frame)])[($p)]);
  call offset:=Mov(eax);
  
  call ecx:=Mov(v);
  call ecx:=Sub(ecx,eax);
  assert TV(ecx-4);
  
  call reachStackRoot(($s),($frame),($p),($Time));
  call visit(ecx,((($FrameAbss)[($s)])[($frame)])[($p)],0);
  
  assert TV(eax-4);
  call scanStackUpdateInvs(($s),($r1),0,($frame),($frame),($addrp),eax+offset);
  call scanStackUpdateInvs(($s),($r2),($frame)+1,($FrameCounts)[($s)],($frame),($addrp),eax+offset);
  
  call ecx:=Mov(save1);
  skip1:
}

procedure scanStackDense($s:int,$frame:int,$addr:int,$desc:int)
#line 1514

requires ecx==($addr);
requires edx==($desc);
requires ($desc)==frameDescriptor((($FrameLayouts)[($s)])[($frame)]);
requires ($addr)==(($FrameAddrs)[($s)])[($frame)];
requires (getBit(($desc),0)&&!getBit(($desc),1))&&and(shr(($desc),6),1023)==0;
requires (0<=($frame)&&($frame)<($FrameCounts)[($s)])&&TV(($frame));
requires (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<=($frame)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

requires (forall f:int::{TV(f)}TV(f)==>(($frame)<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
requires StackTop<=ColorBase;
requires SMemRequire(104,($sMem),esp);

modifies $r2,$color;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
modifies StackTop,$gcMem;

ensures (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<=($frame)-1==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures (forall f:int::{TV(f)}TV(f)==>(($frame)-1<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
ensures StackTop<=ColorBase;
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1514
  var addrp:int;
#line 1514
  var desc:int;
#line 1514
  var addr:int;
#line 1514
  var args:int;
#line 1514
  var offset:int;
#line 1514
  var v:int;
#line 1514
  var b:int;
#line 1553
  assert TVF((($FrameLayouts)[($s)])[($frame)]);
  assert TO(0)&&TO(1);
  
  call addr:=Mov(ecx);
  call desc:=Mov(edx);
  
  call eax:=Mov(edx);
  call eax:=Shr(eax,2);
  call eax:=And(eax,15);
  call args:=Mov(eax);
  call b:=Mov(0);
  call ebx:=Mov(0);
  call ebx:=Lea(ebx+4*eax+4);
  call ebx:=AddChecked(ebx,ecx);
  call addrp:=Mov(ebx);
  
  
  loop1:
  assert addrp==($addr)+4*(args+1-b);
  assert ($frame)<($FrameCounts)[($s)]&&TV(($frame));
  assert (0<=b&&b<=args)&&TO(args+1-b);
  assert (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<($frame)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));
  
  assert (forall f:int::{TV(f)}TV(f)==>(($frame)<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));
  
  
  assert FrameNextInv(($frame),(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4],(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]],($FrameAddrs)[($s)],($FrameLayouts)[($s)]);
  
  assert (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&j<=args+1-b==>(($FrameSlices)[($s)])[($addr)+4*j]==($frame)&&InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r1),(($fMems)[($s)])[($addr)+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[($addr)+4*j])));
  
  
  
  
  assert (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&j>args+1-b==>(($FrameSlices)[($s)])[($addr)+4*j]==($frame)&&InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r2),(($fMems)[($s)])[($addr)+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[($addr)+4*j])));
  
  
  
  
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
  assert StackTop<=ColorBase;
  assert RExtend(old($r2),($r2));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  call eax:=Mov(b);
  havoc $Eip;
#line 1598
  if(eax>=args)
  {
#line 1598
    goto loopEnd1;
  }
  assert TVF((($FrameLayouts)[($s)])[($frame)]);
  call ecx:=Lea(eax+16);
  call ebx:=Mov(desc);
  call ebx:=Shr(ebx,ecx);
  call ebx:=And(ebx,1);
  
  havoc $Eip;
#line 1606
  if(ebx!=1)
  {
#line 1606
    goto skip1;
  }
#line 1607
  call ecx:=Mov(addrp);
  call scanStackWordDense(($s),($frame),($addr),($desc),($addr)+4*(args+1-b),args+1-b,args);
  skip1:
  
  call b:=Add(b,1);
  call addrp:=Sub(addrp,4);
  havoc $Eip;
#line 1613
  goto loop1;
  loopEnd1:
  
  assert TO(0);
  assert TO(1);
  assert TV((($fMems)[($s)])[($addr)]);
  assert TV((($fMems)[($s)])[($addr)]-4);
  
  call addrp:=SubChecked(addrp,8);
  
  
  loop2:
  assert addrp==($addr)+4*(args-1-b);
  assert ($frame)<($FrameCounts)[($s)]&&TV(($frame));
  assert (args-1-b<0&&args-1-b<=1+args)&&TO(args-1-b);
  assert (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<($frame)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));
  
  assert (forall f:int::{TV(f)}TV(f)==>(($frame)<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));
  
  
  assert FrameNextInv(($frame),(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4],(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]],($FrameAddrs)[($s)],($FrameLayouts)[($s)]);
  
  assert (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&j<=args-1-b==>(($FrameSlices)[($s)])[($addr)+4*j]==($frame)&&InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r1),(($fMems)[($s)])[($addr)+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[($addr)+4*j])));
  
  
  
  
  assert (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&j>args-1-b==>(($FrameSlices)[($s)])[($addr)+4*j]==($frame)&&InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r2),(($fMems)[($s)])[($addr)+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[($addr)+4*j])));
  
  
  
  
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
  assert StackTop<=ColorBase;
  assert RExtend(old($r2),($r2));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  call eax:=Mov(b);
  havoc $Eip;
#line 1652
  if(eax>=16)
  {
#line 1652
    goto loopEnd2;
  }
  assert TVF((($FrameLayouts)[($s)])[($frame)]);
  call ecx:=Lea(eax+16);
  call ebx:=Mov(desc);
  call ebx:=Shr(ebx,ecx);
  call ebx:=And(ebx,1);
  
  havoc $Eip;
#line 1660
  if(ebx!=1)
  {
#line 1660
    goto skip2;
  }
#line 1661
  call ecx:=Mov(addrp);
  call scanStackWordDense(($s),($frame),($addr),($desc),($addr)+4*(args-1-b),args-1-b,args);
  skip2:
  call b:=Add(b,1);
  call addrp:=SubChecked(addrp,4);
  havoc $Eip;
#line 1666
  goto loop2;
  loopEnd2:
}

procedure scanStackSparse8($s:int,$frame:int,$addr:int,$desc:int)
#line 1670

requires ecx==($addr);
requires edx==($desc);
requires ($desc)==frameDescriptor((($FrameLayouts)[($s)])[($frame)]);
requires ($addr)==(($FrameAddrs)[($s)])[($frame)];
requires !getBit(($desc),0)&&ro32(($desc))==4096;
requires (0<=($frame)&&($frame)<($FrameCounts)[($s)])&&TV(($frame));
requires (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<=($frame)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

requires (forall f:int::{TV(f)}TV(f)==>(($frame)<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
requires StackTop<=ColorBase;
requires SMemRequire(104,($sMem),esp);

modifies $r2,$color;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
modifies StackTop,$gcMem;

ensures (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<=($frame)-1==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures (forall f:int::{TV(f)}TV(f)==>(($frame)-1<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
ensures StackTop<=ColorBase;
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1670
  var count:int;
#line 1670
  var offset:int;
#line 1670
  var addrp:int;
#line 1670
  var desc:int;
#line 1670
  var addr:int;
#line 1670
  var v:int;
#line 1670
  var p:int;
#line 1709
  assert TVF((($FrameLayouts)[($s)])[($frame)]);
  assert TO(0)&&TO(1);
  
  call addr:=Mov(ecx);
  call desc:=Mov(edx);
  
  call p:=Mov(0);
  call eax:=RoLoadU8(edx+4);
  call count:=Mov(eax);
  
  loop:
  assert p>=0&&TSlot(p);
  
  assert (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<($frame)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));
  
  assert (forall f:int::{TV(f)}TV(f)==>(($frame)<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));
  
  
  assert FrameNextInv(($frame),(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4],(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]],($FrameAddrs)[($s)],($FrameLayouts)[($s)]);
  
  assert (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&!between(0,p,frameFieldToSlot((($FrameLayouts)[($s)])[($frame)],j))==>(($FrameSlices)[($s)])[addr+4*j]==($frame)&&InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r1),(($fMems)[($s)])[addr+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[addr+4*j])));
  
  
  
  
  assert (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&between(0,p,frameFieldToSlot((($FrameLayouts)[($s)])[($frame)],j))==>(($FrameSlices)[($s)])[addr+4*j]==($frame)&&InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r2),(($fMems)[($s)])[addr+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[addr+4*j])));
  
  
  
  
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
  assert StackTop<=ColorBase;
  assert RExtend(old($r2),($r2));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  call eax:=Mov(p);
  havoc $Eip;
#line 1746
  if(eax>=count)
  {
#line 1746
    goto loopEnd;
  }
  assert TO(roS8(desc+6+p));
  assert TV((($fMems)[($s)])[addr+4*roS8(desc+6+p)]-4);
  call ebx:=Mov(desc);
  call esi:=Mov(addr);
  call ebp:=RoLoadS8(ebx+1*eax+6);
  call ebp:=LeaSignedIndex(esi,4,ebp,0);
  call addrp:=Mov(ebp);
  call fLoad(($s),ebp);
#line 1755
  call ecx:=Load(ebp);
  
  havoc $Eip;
#line 1757
  if(ecx<GcLo)
  {
#line 1757
    goto skip1;
  }
#line 1758
  havoc $Eip;
#line 1758
  if(ecx>GcHi)
  {
#line 1758
    goto skip1;
  }
#line 1759
  call v:=Mov(ecx);
  call _call();
#line 1760
  call Call();
#line 1760
  call FromInteriorPointer(v,(($FrameOffsets)[($s)])[addr+4*roS8(desc+6+p)],((($FrameAbss)[($s)])[($frame)])[roS8(desc+6+p)]);
  call offset:=Mov(eax);
  
  call ecx:=Mov(v);
  call ecx:=Sub(ecx,eax);
  assert TV(ecx-4);
  
  call reachStackRoot(($s),($frame),roS8(desc+6+p),($Time));
  call visit(ecx,((($FrameAbss)[($s)])[($frame)])[roS8(desc+6+p)],0);
  assert TV(eax-4);
  call scanStackUpdateInvs(($s),($r1),0,($frame),($frame),addr+4*roS8(desc+6+p),eax);
  call scanStackUpdateInvs(($s),($r2),($frame)+1,($FrameCounts)[($s)],($frame),addr+4*roS8(desc+6+p),eax);
  call ebx:=Mov(addrp);
  skip1:
  
  
  assert (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&p==frameFieldToSlot((($FrameLayouts)[($s)])[($frame)],j)==>j==roS8(desc+6+p)));
  
  
  call p:=Add(p,1);
  havoc $Eip;
#line 1780
  goto loop;
  loopEnd:
}

procedure scanStack($s:int,$ra:int,$nextFp:int)
#line 1784

requires ecx==($ra)&&word(($ra));
requires edx==($nextFp);
requires FrameNextInv(($FrameCounts)[($s)],($ra),($nextFp),($FrameAddrs)[($s)],($FrameLayouts)[($s)]);
requires GcStackInv(($s),($r1),($FrameCounts)[($s)],($FrameAddrs)[($s)],($FrameLayouts)[($s)],($FrameSlices)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)]);
requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
requires StackTop<=ColorBase;
requires SMemRequire(108,($sMem),esp);
modifies $r2,$color;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
modifies StackTop,$gcMem;
ensures GcStackInv(($s),($r2),($FrameCounts)[($s)],($FrameAddrs)[($s)],($FrameLayouts)[($s)],($FrameSlices)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)]);
ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures RExtend(old($r2),($r2));
ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
ensures StackTop<=ColorBase;
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1784
  var _nextFp:int;
#line 1784
  var _ra:int;
#line 1784
  var found:int;
#line 1784
  var desc:int;
#line 1784
  var addr:int;
#line 1784
  var p:int;
#line 1784
  var $frame:int;
#line 1812
  call _ra:=Mov(ecx);
  call _nextFp:=Mov(edx);
  
  $frame:=($FrameCounts)[($s)]-1;
  loop:
  assert ($frame)<($FrameCounts)[($s)]&&TV(($frame));
  assert word(_ra);
  assert FrameNextInv(($frame)+1,_ra,_nextFp,($FrameAddrs)[($s)],($FrameLayouts)[($s)]);
  assert (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<=($frame)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));
  
  assert (forall f:int::{TV(f)}TV(f)==>(($frame)<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));
  
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
  assert StackTop<=ColorBase;
  assert RExtend(old($r2),($r2));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  
  assert TVF((($FrameLayouts)[($s)])[($frame)]);
  assert TVFT(($frame)+1);
  
  call ecx:=Mov(_ra);
  call edx:=Mov(_nextFp);
  havoc $Eip;
#line 1836
  if(edx==0)
  {
#line 1836
    goto loopEnd;
  }
#line 1837
  call _call();
#line 1837
  call Call();
#line 1837
  call TablesSearch(($s),($frame)+1,_ra,_nextFp);
  call desc:=Mov(eax);
  call found:=Mov(edx);
  
  call ecx:=Mov(_nextFp);
  call addr:=Mov(ecx);
  assert TV(addr);
  assert TO(0);
  assert TO(1);
  call fLoad(($s),ecx);
#line 1846
  call eax:=Load(ecx);
  call _nextFp:=Mov(eax);
  call fLoad(($s),ecx+4);
#line 1848
  call eax:=Load(ecx+4);
  call _ra:=Mov(eax);
  
  
  call eax:=Mov(desc);
  call eax:=Shr(eax,0);
  call eax:=And(eax,1);
  havoc $Eip;
#line 1855
  if(eax!=1)
  {
#line 1855
    goto skip1;
  }
#line 1856
  call eax:=Mov(desc);
  call eax:=Shr(eax,1);
  call eax:=And(eax,1);
  havoc $Eip;
#line 1859
  if(eax==1)
  {
#line 1859
    goto skip1;
  }
#line 1860
  call eax:=Mov(desc);
  call eax:=Shr(eax,6);
  call eax:=And(eax,1023);
  havoc $Eip;
#line 1863
  if(eax!=0)
  {
#line 1863
    goto skip1;
  }
#line 1864
  call ecx:=Mov(addr);
  call edx:=Mov(desc);
  call scanStackDense(($s),($frame),addr,desc);
  $frame:=($frame)-1;
  havoc $Eip;
#line 1868
  goto loop;
  skip1:
  
  
  call eax:=Mov(desc);
  call eax:=Shr(eax,0);
  call eax:=And(eax,1);
  havoc $Eip;
#line 1875
  if(eax==1)
  {
#line 1875
    goto skip2;
  }
#line 1876
  call eax:=Mov(desc);
  call eax:=RoLoad32(eax);
  havoc $Eip;
#line 1878
  if(eax!=4096)
  {
#line 1878
    goto skip2;
  }
#line 1879
  call ecx:=Mov(addr);
  call edx:=Mov(desc);
  call scanStackSparse8(($s),($frame),addr,desc);
  $frame:=($frame)-1;
  havoc $Eip;
#line 1883
  goto loop;
  skip2:
  
  
  assert !(((getBit(desc,0)&&!getBit(desc,1))&&and(shr(desc,6),1023)==0)||(!getBit(desc,0)&&ro32(desc)==4096));
  
  call eax:=Mov(1431633988);
  call _call();
#line 1890
  call Call();
#line 1890
  call DebugBreak();
  
  loopEnd:
  assert ($frame)==0-1;
}

procedure scanStacks()
#line 1896

requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires SMemRequire(108,($sMem),esp);
requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
requires StackTop<=ColorBase;
requires (forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))==>IsStackStateTagFor(StackTag(($s),($tMems)),($stackState)[($s)])));
requires (forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))&&!IsEmpty(($stackState)[($s)])==>MutatorStackInvS(($s),($r1),($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($fMems))));
requires (forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))&&IsYielded(($stackState)[($s)])==>(fAddr(($s),StackEsp(($s),($tMems)))&&Aligned(StackEsp(($s),($tMems))))&&ScanStackInv(($s),($fMems)[($s)],($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),StackRA(($s),($tMems),($fMems)),StackEsp(($s),($tMems)),StackEbp(($s),($tMems)))));



modifies $r2,$color;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
modifies StackTop,$gcMem;
ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
ensures StackTop<=ColorBase;
ensures (forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))&&!IsEmpty(($stackState)[($s)])==>MutatorStackInvS(($s),($r2),($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($fMems))));
ensures (forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))&&IsYielded(($stackState)[($s)])==>ScanStackInv(($s),($fMems)[($s)],($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),StackRA(($s),($tMems),($fMems)),StackEsp(($s),($tMems)),StackEbp(($s),($tMems)))));
{
#line 1896
  var tag:int;
#line 1896
  var t:int;
#line 1896
  var s:int;
#line 1921
  call s:=Mov(0);
  call eax:=Mov(TLo);
  call t:=Mov(eax);
  __L1:
  assert 0<=s&&TStk(s);
  assert t==?tLo+s*?TSize;
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert RExtend(old($r2),($r2));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
  assert StackTop<=ColorBase;
  assert (forall $s:int::{TStk(($s))}TStk(($s))==>((isStack(($s))&&s<=($s))&&!IsEmpty(($stackState)[($s)])==>MutatorStackInvS(($s),($r1),($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($fMems))));
  assert (forall $s:int::{TStk(($s))}TStk(($s))==>((isStack(($s))&&s>($s))&&!IsEmpty(($stackState)[($s)])==>MutatorStackInvS(($s),($r2),($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($fMems))));
  assert (forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))&&IsYielded(($stackState)[($s)])==>(fAddr(($s),StackEsp(($s),($tMems)))&&Aligned(StackEsp(($s),($tMems))))&&ScanStackInv(($s),($fMems)[($s)],($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),StackRA(($s),($tMems),($fMems)),StackEsp(($s),($tMems)),StackEbp(($s),($tMems)))));
#line 1924
  call eax:=Mov(?NumStacks);
#line 1924
  havoc $Eip;
#line 1924
  if(s>=eax)
  {
#line 1924
    goto __L2;
  }
#line 1940
  assert ((TV(?memLo)&&TO(512+65536*2+64*s))&&TO(512+65536*2+64*s+1))&&TO(512+65536*2+64*s+2);
  
  call eax:=Mov(t);
  call tLoad(s,eax);
#line 1943
  call eax:=Load(eax);
  call tag:=Mov(eax);
  
  call eax:=Mov(?STACK_YIELDED);
#line 1946
  havoc $Eip;
#line 1946
  if(tag!=eax)
  {
#line 1946
    goto __L3;
  }
  call ebx:=Mov(t);
  call tLoad(s,ebx+4);
#line 1949
  call ecx:=Load(ebx+4);
  call fLoad(s,ecx);
#line 1950
  call ecx:=Load(ecx);
  call tLoad(s,ebx+8);
#line 1951
  call edx:=Load(ebx+8);
  call scanStack(s,StackRA(s,($tMems),($fMems)),StackEbp(s,($tMems)));
#line 1946
  __L3:
  
  
  
  
  
  
  
  call eax:=Mov(?STACK_INTERRUPTED);
#line 1954
  havoc $Eip;
#line 1954
  if(tag!=eax)
  {
#line 1954
    goto __L4;
  }
  call eax:=Mov(1431633989);
  call _call();
#line 1957
  call Call();
#line 1957
  call DebugBreak();
#line 1954
  __L4:
  
  
  
  
  call eax:=Mov(?STACK_RUNNING);
#line 1959
  havoc $Eip;
#line 1959
  if(tag!=eax)
  {
#line 1959
    goto __L5;
  }
  call eax:=Mov(1431633990);
  call _call();
#line 1962
  call Call();
#line 1962
  call DebugBreak();
#line 1959
  __L5:
  
  
  
  
  
  call s:=Add(s,1);
  call eax:=Mov(?TSize);
#line 1966
  call t:=Add(t,eax);
#line 1924
  havoc $Eip;
#line 1924
  goto __L1;
#line 1924
  __L2:
}
#line 1970
procedure scanStaticSection($section:int)
#line 1970

requires ecx==($section);
requires (0<=($section)&&($section)<?sectionCount)&&TV(($section));
requires (forall s:int::{TV(s)}TV(s)==>(($section)<=s&&s<?sectionCount==>SectionInv(s,sectionBase(s),sectionEnd(s),($r1),($SectionMem),($SectionAbs))));

requires (forall s:int::{TV(s)}TV(s)==>(0<=s&&s<($section)==>SectionInv(s,sectionBase(s),sectionEnd(s),($r2),($SectionMem),($SectionAbs))));

ensures (forall s:int::{TV(s)}TV(s)==>(($section)+1<=s&&s<?sectionCount==>SectionInv(s,sectionBase(s),sectionEnd(s),($r1),($SectionMem),($SectionAbs))));

ensures (forall s:int::{TV(s)}TV(s)==>(0<=s&&s<($section)+1==>SectionInv(s,sectionBase(s),sectionEnd(s),($r2),($SectionMem),($SectionAbs))));


requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
requires StackTop<=ColorBase;
requires SMemRequire(100,($sMem),esp);

modifies $r2,$color;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
modifies StackTop,$gcMem;

ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures StackTop<=ColorBase;
ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1970
  var save3:int;
#line 1970
  var save2:int;
#line 1970
  var save1:int;
#line 1970
  var section:int;
#line 1970
  var addr:int;
#line 1970
  var sEnd:int;
#line 2006
  call section:=Mov(ecx);
  
  assert TVS(section,0);
  
  call eax:=Mov(?dataSectionEnd);
  call eax:=RoLoad32(eax+4*ecx);
  call sEnd:=Mov(eax);
  
  call eax:=Mov(?staticDataPointerBitMap);
  call edx:=RoLoad32(eax+4*ecx);
  
  call eax:=Mov(?dataSectionBase);
  call eax:=RoLoad32(eax+4*ecx);
  call addr:=Mov(eax);
  call edi:=Mov(eax);
  
  call esi:=Mov(0);
  
  loop:
  assert edx==ro32(?staticDataPointerBitMap+4*($section));
  assert edi==addr+4*esi;
  assert 0<=section&&TV(section);
  assert 0<=esi&&TO(esi);
  assert (forall s:int::{TV(s)}TV(s)==>(section<s&&s<?sectionCount==>SectionInv(s,sectionBase(s),sectionEnd(s),($r1),($SectionMem),($SectionAbs))));
  
  assert (forall s:int::{TV(s)}TV(s)==>(0<=s&&s<section==>SectionInv(s,sectionBase(s),sectionEnd(s),($r2),($SectionMem),($SectionAbs))));
  
  
  assert (forall j:int::{TO(j)}TO(j)==>((0<=j&&addr+4*j<sectionEnd(section))&&j>=esi==>sectionSlice(addr+4*j)==section&&Value(sectionHasPtr(section,j),($r1),($SectionMem)[addr+4*j],(($SectionAbs)[section])[j])));
  
  
  
  
  assert (forall j:int::{TO(j)}TO(j)==>((0<=j&&addr+4*j<sectionEnd(section))&&j<esi==>sectionSlice(addr+4*j)==section&&Value(sectionHasPtr(section,j),($r2),($SectionMem)[addr+4*j],(($SectionAbs)[section])[j])));
  
  
  
  
  assert StackTop<=ColorBase;
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert RExtend(old($r2),($r2));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  
  havoc $Eip;
#line 2051
  if(edi>=sEnd)
  {
#line 2051
    goto loopEnd;
  }
  assert TVS(section,esi);
  call eax:=Mov(esi);
  call eax:=Shr(eax,5);
  call eax:=RoLoad32(edx+4*eax);
  
  
  call ecx:=Mov(esi);
  call ecx:=And(ecx,31);
  call eax:=Shr(eax,ecx);
  call eax:=And(eax,1);
  havoc $Eip;
#line 2063
  if(eax!=1)
  {
#line 2063
    goto skip1;
  }
#line 2064
  assert TV(($SectionMem)[addr+4*esi]-4);
  call ecx:=SectionLoad(edi);
  
  havoc $Eip;
#line 2067
  if(ecx<GcLo)
  {
#line 2067
    goto skip2;
  }
#line 2068
  havoc $Eip;
#line 2068
  if(ecx>GcHi)
  {
#line 2068
    goto skip2;
  }
#line 2069
  call save1:=Mov(edi);
  call save2:=Mov(esi);
  call save3:=Mov(edx);
  call reachStaticRoot(section,esi,($Time));
  call visit(ecx,(($SectionAbs)[section])[esi],0);
  call edi:=Mov(save1);
  call esi:=Mov(save2);
  call edx:=Mov(save3);
  skip2:
  skip1:
  
  call esi:=Add(esi,1);
  call edi:=AddChecked(edi,4);
  havoc $Eip;
#line 2082
  goto loop;
  loopEnd:
}

procedure scanStatic()
#line 2086

requires StaticInv(($r1),($SectionMem),($SectionAbs));
ensures StaticInv(($r2),($SectionMem),($SectionAbs));

requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
requires StackTop<=ColorBase;
requires SMemRequire(100,($sMem),esp);

modifies $r2,$color;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
modifies StackTop,$gcMem;

ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures StackTop<=ColorBase;
ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 2086
  var section:int;
#line 2108
  call section:=Mov(0);
  
  loop:
  assert 0<=section&&TV(section);
  assert (forall s:int::{TV(s)}TV(s)==>(section<=s&&s<?sectionCount==>SectionInv(s,sectionBase(s),sectionEnd(s),($r1),($SectionMem),($SectionAbs))));
  
  assert (forall s:int::{TV(s)}TV(s)==>(0<=s&&s<section==>SectionInv(s,sectionBase(s),sectionEnd(s),($r2),($SectionMem),($SectionAbs))));
  
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert StackTop<=ColorBase;
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
  assert RExtend(old($r2),($r2));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  call ecx:=Mov(section);
  havoc $Eip;
#line 2123
  if(ecx>=?sectionCount)
  {
#line 2123
    goto loopEnd;
  }
  call scanStaticSection(section);
  call section:=AddChecked(section,1);
  havoc $Eip;
#line 2127
  goto loop;
  loopEnd:
}

procedure AllocBlock($minSize:int,$maxSize:int,$maxWords:int)
#line 2131

requires ecx==($minSize);
requires edx==($maxSize);
requires __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
requires 8<=($minSize)&&($minSize)<=($maxSize);
requires ($maxSize)==4*($maxWords);
requires SMemRequireRA(100,($sMem),esp,($RET));

modifies $fs,$fn,$gcMem,CacheSize;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;

ensures __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
ensures eax!=0==>($fs)[eax]>=($minSize);
ensures eax!=0==>HeapLo<eax&&eax+($fs)[eax]<=HeapHi;
ensures eax!=0==>Disconnected(HeapLo,eax,($fs),($fn));
ensures eax!=0==>(CacheSize!=0==>eax!=CachePtr);
ensures eax!=0==>Aligned(eax);
ensures SMemEnsure(($sMem),old($sMem),esp,old(esp));
{
  call edi:=Mov(HeapLo);
  
  loop:
  assert __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
  assert HeapLo<=edi&&edi<HeapHi;
  assert ($fs)[edi]!=0;
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  
  assert (TV(edi)&&TV(($fn)[edi]))&&TO(1);
  call esi:=Mov(edi);
  call gcLoad(edi);
#line 2160
  call edi:=Load(edi);
  havoc $Eip;
#line 2161
  if(edi!=0)
  {
#line 2161
    goto notEmpty;
  }
#line 2162
  call eax:=Mov(0);
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 2163
  call Ret(old($RET));
#line 2163
  return;
  notEmpty:
  
  call gcLoad(edi+4);
#line 2166
  call ebx:=Load(edi+4);
  
  havoc $Eip;
#line 2168
  if(ebx<ecx)
  {
#line 2168
    goto loop;
  }
#line 2169
  call ebp:=Mov(ebx);
  call ebp:=Sub(ebp,8);
  havoc $Eip;
#line 2171
  if(ebp<edx)
  {
#line 2171
    goto skip1;
  }
  assert TV(edi+($fs)[edi]);
  assert TO(0-($maxWords));
  call eax:=Lea(edi+ebx);
  call eax:=Sub(eax,edx);
  assert TV(eax);
  
  
  havoc $Eip;
#line 2180
  if(eax<ReserveMin)
  {
#line 2180
    goto loop;
  }
  call ebx:=Sub(ebx,edx);
  
  
  havoc $Eip;
#line 2185
  if(ebx<256)
  {
#line 2185
    goto skip1;
  }
  
  $fn:=($fn)[eax:=0];
  $fs:=($fs)[eax:=edx];
  call gcStore(eax,0);
#line 2190
  call Store(eax,0);
  call gcStore(eax+4,edx);
#line 2191
  call Store(eax+4,edx);
  
  $fs:=($fs)[edi:=(($fs)[edi]-edx)];
  call gcStore(edi+4,ebx);
#line 2194
  call Store(edi+4,ebx);
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 2195
  call Ret(old($RET));
#line 2195
  return;
  skip1:
  
  
  
  havoc $Eip;
#line 2200
  if(edi<ReserveMin)
  {
#line 2200
    goto loop;
  }
  $fn:=($fn)[esi:=(($fn)[edi])];
  call gcLoad(edi);
#line 2203
  call ebx:=Load(edi);
  call gcStore(esi,ebx);
#line 2204
  call Store(esi,ebx);
  $fn:=($fn)[edi:=0];
  call gcStore(edi,0);
#line 2206
  call Store(edi,0);
  call eax:=Mov(edi);
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 2208
  call Ret(old($RET));
#line 2208
  return;
}

procedure allocFromCache($size:int,$nFields:int)
#line 2211

requires ecx==($size);
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
requires 8<=($size)&&($size)+8<=CacheSize;
requires ($size)==4*($nFields);

modifies $fs,$fn,$gcMem,$Mem,CacheSize;
modifies $Eip,eax,edx;
ensures NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures ($fs)[eax]>=($size);
ensures HeapLo<eax&&eax+($size)<=HeapHi;
ensures Disconnected(HeapLo,eax,($fs),($fn));
ensures eax!=CachePtr;
ensures Aligned(eax);
{
  assert TV(CachePtr)&&TV(CachePtr+CacheSize);
  assert TO(0-($nFields))&&TO(1);
  call CacheSize:=Sub(CacheSize,ecx);
  $fs:=($fs)[CachePtr:=CacheSize];
  call eax:=Mov(CacheSize);
  call edx:=Mov(CachePtr);
  call gcStore(edx+4,eax);
#line 2232
  call Store(edx+4,eax);
  
  call eax:=Add(eax,edx);
  assert TV(CachePtr)&&TV(eax);
  
  $fn:=($fn)[eax:=0];
  $fs:=($fs)[eax:=($size)];
  call gcStore(eax,0);
#line 2239
  call Store(eax,0);
  call gcStore(eax+4,ecx);
#line 2240
  call Store(eax+4,ecx);
}

procedure AllocSlow($size:int,$nFields:int)
#line 2243

requires ecx==($size);
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
requires 8<=($size);
requires ($size)==4*($nFields);
requires SMemRequireRA(104,($sMem),esp,($RET));
modifies $fs,$fn,$gcMem,CachePtr,CacheSize;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures eax!=0==>($fs)[eax]>=($size);
ensures eax!=0==>HeapLo<eax&&eax+($size)<=HeapHi;
ensures eax!=0==>Disconnected(HeapLo,eax,($fs),($fn));
ensures eax!=0==>(CacheSize!=0==>eax!=CachePtr);
ensures eax!=0==>Aligned(eax);
ensures SMemEnsure(($sMem),old($sMem),esp,old(esp));
{
#line 2243
  var size:int;
#line 2262
  havoc $Eip;
#line 2262
  if(ecx<192)
  {
#line 2262
    goto skip1;
  }
  
  
  call edx:=Mov(ecx);
  call _call();
#line 2267
  call Call();
#line 2267
  call AllocBlock(($size),($size),($nFields));
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 2268
  call Ret(old($RET));
#line 2268
  return;
  
  skip1:
  
  call size:=Mov(ecx);
  
  call ecx:=Mov(256);
  
  
  call edx:=Mov(65536);
  
  call _call();
#line 2279
  call Call();
#line 2279
  call AllocBlock(256,65536,16384);
  
  
  assert TV(eax);
  havoc $Eip;
#line 2283
  if(eax==0)
  {
#line 2283
    goto skip2;
  }
#line 2284
  call CachePtr:=Mov(eax);
  assert TO(1);
  call ecx:=Mov(CachePtr);
  call gcLoad(ecx+4);
#line 2287
  call ecx:=Load(ecx+4);
  call CacheSize:=Mov(ecx);
  call ecx:=Mov(size);
  call allocFromCache(($size),($nFields));
  skip2:
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 2292
  call Ret(old($RET));
#line 2292
  return;
}

procedure processMarkStack()
#line 2295

requires BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

requires MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
requires StackTop<=ColorBase;
requires SMemRequire(100,($sMem),esp);

modifies $r2,$color;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
modifies StackTop,$gcMem;

ensures BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

ensures StackTop==?gcLo;
ensures MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 2295
  var ptr:int;
#line 2314
  call eax:=Mov(StackTop);
  
  
  loop:
  assert MarkStack(?gcLo,StackTop,($toAbs),($color),($gcMem),0);
  assert BigGcInv(($freshAbs),ColorBase,HeapLo,HeapHi,($color),($fs),($fn),($toAbs),($r1),($r2),($AbsMem),($gcSlice),CachePtr,CacheSize,($Time),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  
  assert RExtend(old($r2),($r2));
  assert StackTop<=ColorBase;
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  
  call eax:=Mov(StackTop);
  havoc $Eip;
#line 2326
  if(eax==GcLo)
  {
#line 2326
    goto loopEnd;
  }
  assert TV(StackTop)&&TO(0-1);
  call __notAligned(?gcLo);
  call eax:=Sub(eax,4);
  call gcLoad(eax);
#line 2331
  call ebx:=Load(eax);
  call ptr:=Mov(ebx);
  call StackTop:=Mov(eax);
  assert TV(StackTop);
  assert TV(ptr);
  
  call __notAligned(StackTop);
  call scanObject(ptr,($r1)[ptr]);
  
  call esi:=Mov(ptr);
  call esi:=Sub(esi,HeapLo);
  call edi:=Mov(ColorBase);
  call edx:=Mov(3);
  call bb4SetColor(($color),3,HeapLo,HeapLo,HeapLo,HeapHi,ptr,ColorBase,HeapLo);
  $color:=($color)[ptr:=3];
  havoc $Eip;
#line 2346
  goto loop;
  loopEnd:
}

procedure sweepObject($ptr:int,$prev:int,$regionStart:int)
#line 2350
returns($_ptr:int,$_prev:int,$_regionStart:int)
requires ebx==($color)[($ptr)];
requires esi==($ptr);
requires edi==($prev);
requires ebp==($regionStart);
requires HeapLo+8<=HeapHi;
requires ($toAbs)[($ptr)]!=NO_ABS;
requires Black(($color)[($ptr)])==>($toAbs)[($ptr)]==($r2)[($ptr)];
requires ($ptr)<HeapHi;

requires GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($r1),($r2),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires ObjectSeq(HeapLo,($regionStart),($toAbs),($fs),($fn));
requires FreeInv(HeapLo,($regionStart),($fs),($fn),($gcMem),CachePtr,CacheSize);
requires Objects(($ptr),HeapHi,($toAbs),($fs),($fn));
requires ($fs)[($prev)]!=0;
requires ($gcMem)[($prev)]==0;
requires ((Aligned(($prev))&&HeapLo<=($prev))&&($prev)+8<=($regionStart))&&($regionStart)<=($ptr);
requires ($ptr)<HeapHi+4;
requires (forall ii:int::{TV(ii)}TV(ii)==>((HeapLo<=ii&&ii<($prev))&&($fs)[ii]!=0==>($fn)[ii]<=($prev)));
requires (forall ii:int::{TV(ii)}TV(ii)==>((($prev)<=ii&&ii<($ptr))&&($fs)[ii]!=0==>($fn)[ii]==0));
requires (forall ii:int::{TV(ii)}TV(ii)==>(($regionStart)<=ii&&ii<($ptr)==>($toAbs)[ii]==NO_ABS&&($fs)[ii]==0));
requires CacheSize==0;
requires HeapLo<=($ptr);
requires Aligned(($regionStart))&&Aligned(($ptr));
requires AllocInv2(($toAbs),HeapLo,HeapHi,($r1),($r2),false);
requires (forall i:int::{TV(i)}TV(i)==>(HeapLo<=i&&i<HeapHi==>(($toAbs)[i]!=NO_ABS==>!Gray(($color)[i]))));
requires (forall i:int::{TV(i)}TV(i)==>(HeapLo<=i&&i<($ptr)==>(White(($color)[i])<==>($toAbs)[i]!=NO_ABS)));
requires (forall i:int::{TV(i)}TV(i)==>((HeapLo<=i&&i<($ptr))&&($toAbs)[i]!=NO_ABS==>(((($r1)[i]!=NO_ABS&&($r2)[i]!=NO_ABS)&&($toAbs)[i]!=NO_ABS)&&($toAbs)[i]==($r2)[i])&&ObjInv(i,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice))));



requires MsGcInv(($ptr),HeapHi,($Time),($r1),($r2),($color),true,($gcMem),($toAbs),($AbsMem),($gcSlice));
requires SMemRequire(104,($sMem),esp);

modifies $color;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
modifies $toAbs,$fs,$fn,$gcMem,CacheSize;

ensures esi==($_ptr);
ensures edi==($_prev);
ensures ebp==($_regionStart);
ensures GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($r1),($r2),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures ObjectSeq(HeapLo,($_regionStart),($toAbs),($fs),($fn));
ensures FreeInv(HeapLo,($_regionStart),($fs),($fn),($gcMem),CachePtr,CacheSize);
ensures Objects(($_ptr),HeapHi,($toAbs),($fs),($fn));
ensures ($fs)[($_prev)]!=0;
ensures ($gcMem)[($_prev)]==0;
ensures ((Aligned(($_prev))&&HeapLo<=($_prev))&&($_prev)+8<=($_regionStart))&&($_regionStart)<=($_ptr);
ensures ($_ptr)<HeapHi+4;
ensures (forall ii:int::{TV(ii)}TV(ii)==>((HeapLo<=ii&&ii<($_prev))&&($fs)[ii]!=0==>($fn)[ii]<=($_prev)));
ensures (forall ii:int::{TV(ii)}TV(ii)==>((($_prev)<=ii&&ii<($_ptr))&&($fs)[ii]!=0==>($fn)[ii]==0));
ensures (forall ii:int::{TV(ii)}TV(ii)==>(($_regionStart)<=ii&&ii<($_ptr)==>($toAbs)[ii]==NO_ABS&&($fs)[ii]==0));
ensures CacheSize==0;
ensures HeapLo<=($_ptr);
ensures Aligned(($_regionStart))&&Aligned(($_ptr));
ensures AllocInv2(($toAbs),HeapLo,HeapHi,($r1),($r2),false);
ensures (forall i:int::{TV(i)}TV(i)==>(HeapLo<=i&&i<HeapHi==>(($toAbs)[i]!=NO_ABS==>!Gray(($color)[i]))));
ensures (forall i:int::{TV(i)}TV(i)==>(HeapLo<=i&&i<($_ptr)==>(White(($color)[i])<==>($toAbs)[i]!=NO_ABS)));
ensures (forall i:int::{TV(i)}TV(i)==>((HeapLo<=i&&i<($_ptr))&&($toAbs)[i]!=NO_ABS==>(((($r1)[i]!=NO_ABS&&($r2)[i]!=NO_ABS)&&($toAbs)[i]!=NO_ABS)&&($toAbs)[i]==($r2)[i])&&ObjInv(i,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice))));



ensures MsGcInv(($_ptr),HeapHi,($Time),($r1),($r2),($color),true,($gcMem),($toAbs),($AbsMem),($gcSlice));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 2350
  var save3:int;
#line 2350
  var save2:int;
#line 2350
  var save1:int;
#line 2350
  var size:int;
#line 2420
  assert ((TV(esi)&&TV(ebp))&&TV(edi))&&TO(1);
  assert TO(numFields(($toAbs)[esi]));
  
  havoc $Eip;
#line 2423
  if(ebx!=3)
  {
#line 2423
    goto skip1;
  }
#line 2424
  call save1:=Mov(esi);
  call save2:=Mov(edi);
  call save3:=Mov(ebp);
  call ecx:=Mov(esi);
  call gcLoad(esi+4);
#line 2428
  call edx:=Load(esi+4);
  call _call();
#line 2429
  call Call();
#line 2429
  call GetSize(($ptr),($gcMem)[($ptr)+4],($r2),($r2));
  call size:=Mov(eax);
  assert size==4*numFields(($toAbs)[($ptr)]);
  call esi:=Mov(save1);
  call ebp:=Mov(save3);
  
  call esi:=Sub(esi,HeapLo);
  call edi:=Mov(ColorBase);
  call edx:=Mov(1);
  call bb4SetColor(($color),1,HeapLo,HeapLo,HeapLo,HeapHi,($ptr),ColorBase,HeapLo);
  call esi:=Mov(save1);
  call edi:=Mov(save2);
  $color:=($color)[esi:=1];
  
  call eax:=Mov(esi);
  call eax:=Sub(eax,ebp);
  
  havoc $Eip;
#line 2446
  if(eax<256)
  {
#line 2446
    goto skipFree;
  }
  
  
  $fn:=($fn)[edi:=ebp];
  call gcStore(edi,ebp);
#line 2451
  call Store(edi,ebp);
  $fn:=($fn)[ebp:=0];
  $fs:=($fs)[ebp:=eax];
  call gcStore(ebp,0);
#line 2454
  call Store(ebp,0);
  call gcStore(ebp+4,eax);
#line 2455
  call Store(ebp+4,eax);
  call edi:=Mov(ebp);
  skipFree:
  
  call esi:=Add(esi,size);
  call ebp:=Mov(esi);
  havoc $Eip;
#line 2461
  goto skip2;
  
  skip1:
  assert White(($color)[esi]);
  call save1:=Mov(esi);
  call save2:=Mov(edi);
  call save3:=Mov(ebp);
  call ecx:=Mov(esi);
  call gcLoad(esi+4);
#line 2469
  call edx:=Load(esi+4);
  call _call();
#line 2470
  call Call();
#line 2470
  call GetSize(($ptr),($gcMem)[($ptr)+4],($r1),($r1));
  call size:=Mov(eax);
  assert size==4*numFields(($toAbs)[($ptr)]);
  call esi:=Mov(save1);
  call edi:=Mov(save2);
  call ebp:=Mov(save3);
  
  $toAbs:=($toAbs)[esi:=NO_ABS];
  call esi:=Sub(esi,HeapLo);
  call edi:=Mov(ColorBase);
  call edx:=Mov(0);
  call bb4SetColor(($color),0,HeapLo,HeapLo,HeapLo,HeapHi,($ptr),ColorBase,HeapLo);
  call esi:=Mov(save1);
  call edi:=Mov(save2);
  $color:=($color)[esi:=0];
  
  call esi:=Add(esi,size);
  skip2:
  $_ptr:=esi;
  $_prev:=edi;
  $_regionStart:=ebp;
}

procedure Sweep()
#line 2493

requires AllocInv2(($toAbs),HeapLo,HeapHi,($r1),($r2),false);
requires MsGcInv(HeapLo,HeapHi,($Time),($r1),($r2),($color),true,($gcMem),($toAbs),($AbsMem),($gcSlice));
requires (forall i:int::{TV(i)}TV(i)==>(HeapLo<=i&&i<HeapHi==>(($toAbs)[i]!=NO_ABS==>!Gray(($color)[i]))));
requires (forall i:int::{TV(i)}TV(i)==>(HeapLo<=i&&i<HeapHi==>(($toAbs)[i]!=NO_ABS&&Black(($color)[i])==>($toAbs)[i]==($r2)[i])));

requires GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($r1),($r2),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires Objects(HeapLo+8,HeapHi,($toAbs),($fs),($fn));
requires ($fs)[HeapLo]==8;
requires ObjectSeq(HeapLo,HeapLo+8,($toAbs),($fs),($fn));
requires FreeInv(HeapLo,HeapLo+8,($fs),($fn),($gcMem),CachePtr,CacheSize);
requires HeapLo+8<=HeapHi;
requires SMemRequireRA(104,($sMem),esp,($RET));

modifies $color;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
modifies $toAbs,$fs,$fn,$gcMem,CacheSize;

ensures (forall i:int::{TV(i)}TV(i)==>(HeapLo<=i&&i<HeapHi==>(($toAbs)[i]!=NO_ABS==>reached(($toAbs)[i],($Time)))));
ensures MsInv(HeapLo,HeapHi,($color),($gcMem),($toAbs),($AbsMem),($gcSlice));
ensures GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($r1),($r2),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures ObjectSeq(HeapLo,HeapHi,($toAbs),($fs),($fn));
ensures FreeInv(HeapLo,HeapHi,($fs),($fn),($gcMem),CachePtr,CacheSize);
ensures ($toAbs)==($r2);
ensures SMemEnsure(($sMem),old($sMem),esp,old(esp));
{
#line 2493
  var $_regionStart:int;
#line 2493
  var $_prev:int;
#line 2493
  var $_ptr:int;
#line 2525
  assert TV(HeapLo);
  assert TV(HeapLo+8);
  
  call CacheSize:=Mov(0);
  call edi:=Mov(HeapLo);
  call esi:=Lea(edi+8);
  call ebp:=Mov(esi);
  $fn:=($fn)[edi:=0];
  assert TV(edi);
  call gcStore(edi,0);
#line 2534
  call Store(edi,0);
  
  
  
  
  
  
  loop:
  assert GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($r1),($r2),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert ObjectSeq(HeapLo,ebp,($toAbs),($fs),($fn));
  assert FreeInv(HeapLo,ebp,($fs),($fn),($gcMem),CachePtr,CacheSize);
  assert Objects(esi,HeapHi,($toAbs),($fs),($fn));
  assert ($fs)[edi]!=0;
  assert ($gcMem)[edi]==0;
  assert Aligned(edi);
  assert (HeapLo<=edi&&edi+8<=ebp)&&ebp<=esi;
  assert esi<HeapHi+4;
  assert (forall ii:int::{TV(ii)}TV(ii)==>((HeapLo<=ii&&ii<edi)&&($fs)[ii]!=0==>($fn)[ii]<=edi));
  assert (forall ii:int::{TV(ii)}TV(ii)==>((edi<=ii&&ii<esi)&&($fs)[ii]!=0==>($fn)[ii]==0));
  assert (forall ii:int::{TV(ii)}TV(ii)==>(ebp<=ii&&ii<esi==>($toAbs)[ii]==NO_ABS&&($fs)[ii]==0));
  assert CacheSize==0;
  
  assert HeapLo<=esi;
  assert Aligned(ebp)&&Aligned(esi);
  assert AllocInv2(($toAbs),HeapLo,HeapHi,($r1),($r2),false);
  assert (forall i:int::{TV(i)}TV(i)==>(HeapLo<=i&&i<HeapHi==>(($toAbs)[i]!=NO_ABS==>!Gray(($color)[i]))));
  
  assert (forall i:int::{TV(i)}TV(i)==>(HeapLo<=i&&i<esi==>(White(($color)[i])<==>($toAbs)[i]!=NO_ABS)));
  assert (forall i:int::{TV(i)}TV(i)==>((HeapLo<=i&&i<esi)&&($toAbs)[i]!=NO_ABS==>(((($r1)[i]!=NO_ABS&&($r2)[i]!=NO_ABS)&&($toAbs)[i]!=NO_ABS)&&($toAbs)[i]==($r2)[i])&&ObjInv(i,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice))));
  
  
  
  assert MsGcInv(esi,HeapHi,($Time),($r1),($r2),($color),true,($gcMem),($toAbs),($AbsMem),($gcSlice));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 2568
  if(esi>=HeapHi)
  {
#line 2568
    goto loopEnd;
  }
  assert (TV(esi)&&TV(ebp))&&TV(edi);
  
  call ebx:=Mov(esi);
  call ebx:=Sub(ebx,HeapLo);
  call eax:=Mov(ColorBase);
  call bb4GetColor(($color),HeapLo,HeapLo,HeapLo,HeapHi,esi,ColorBase,HeapLo);
  assert ebx==($color)[esi];
  havoc $Eip;
#line 2577
  if(ebx==0)
  {
#line 2577
    goto skip1;
  }
#line 2578
  call $_ptr,$_prev,$_regionStart:=sweepObject(esi,edi,ebp);
  havoc $Eip;
#line 2579
  goto loop;
  
  skip1:
  assert TO(1);
  call __notAligned(esi);
  $fs:=($fs)[esi:=0];
  call esi:=Add(esi,4);
  havoc $Eip;
#line 2586
  goto loop;
  loopEnd:
  
  
  call eax:=Mov(esi);
  call eax:=Sub(eax,ebp);
  
  havoc $Eip;
#line 2593
  if(eax<256)
  {
#line 2593
    goto skip2;
  }
  
  
  $fn:=($fn)[edi:=ebp];
  call gcStore(edi,ebp);
#line 2598
  call Store(edi,ebp);
  $fn:=($fn)[ebp:=0];
  $fs:=($fs)[ebp:=(esi-ebp)];
  assert TV(ebp)&&TO(1);
  call gcStore(ebp,0);
#line 2602
  call Store(ebp,0);
  call gcStore(ebp+4,eax);
#line 2603
  call Store(ebp+4,eax);
  call edi:=Mov(ebp);
  skip2:
  
  call __notAligned(HeapHi);
  $toAbs:=($r2);
  
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 2610
  call Ret(old($RET));
#line 2610
  return;
}

procedure _garbageCollect()
#line 2613

requires StaticInv(($toAbs),($SectionMem),($SectionAbs));
requires _NucleusInv(($S),($stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
requires SMemRequire(108,($sMem),esp);

modifies $r1,$r2,$gcMem,$toAbs,$gcSlice,$color,StackTop;
modifies $fs,$fn,CacheSize,$Time;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;


ensures StaticInv(($toAbs),($SectionMem),($SectionAbs));
ensures _NucleusInv(($S),($stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures (forall i:int::{TV(i)}TV(i)==>((HeapLo<=i&&i<HeapHi)&&($toAbs)[i]!=NO_ABS==>reached(($toAbs)[i],($Time))));

ensures ebp==old(ebp);
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 2613
  var saveEbp:int;
#line 2630
  call saveEbp:=Mov(ebp);
  call newTime();
  
  $r1:=($toAbs);
  $r2:=MAP_NO_ABS;
  
  call eax:=Mov(GcLo);
  call StackTop:=Mov(eax);
  
  call scanStacks();
  call scanStatic();
  call processMarkStack();
  
  assert TV(HeapLo);
  $fn:=($fn)[HeapLo:=0];
  call eax:=Mov(HeapLo);
  call gcStore(eax,0);
#line 2646
  call Store(eax,0);
  call CacheSize:=Mov(0);
  
  call _call();
#line 2649
  call Call();
#line 2649
  call Sweep();
  
  call ebp:=Mov(saveEbp);
}

implementation GarbageCollect()
#line 2654
{
#line 2654
  var sp:int;
#line 2654
  var tm:int;
#line 2654
  var t:int;
  
  call edx:=Mov(ebp);
  call fLoad(($S),esp);
#line 2657
  call ecx:=Load(esp);
  
  
  
  
  call eax:=Mov(CurrentStack);
  call ebx:=Mov(?TSize);
  call eax,edx:=Mul(eax,ebx);
  call t:=Mov(eax);
  call eax:=Mov(TLo);
#line 2666
  call t:=Add(t,eax);
  assert t==?tLo+($S)*?TSize;
  assert ((TV(?memLo)&&TO(512+65536*2+64*($S)))&&TO(512+65536*2+64*($S)+1))&&TO(512+65536*2+64*($S)+2);
  call ebx:=Mov(t);
  call tStore(($S),ebx,?STACK_YIELDED);
#line 2670
  call Store(ebx,?STACK_YIELDED);
  call tStore(($S),ebx+4,esp);
#line 2671
  call Store(ebx+4,esp);
  call tStore(($S),ebx+8,ebp);
#line 2672
  call Store(ebx+8,ebp);
  
  call sp:=Mov(esp);
  call esp:=Mov(DLo);
  
  $stackState:=($StackState)[($S):=StackYielded(ebp,sp+4,ecx)];
  call _garbageCollect();
  
  call eax:=Mov(t);
  call tStore(($S),eax,?STACK_RUNNING);
#line 2681
  call Store(eax,?STACK_RUNNING);
  
  call esp:=Mov(sp);
  call _fret(($S),old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
  call Ret(old($RET));
  return;
}

procedure allocObjectMemory($ra:int,$nextFp:int,$size:int,$nFields:int)
#line 2689

requires eax==($size);
requires ecx==($ra)&&word(($ra));
requires edx==($nextFp);
requires FrameNextInv(($FrameCounts)[($S)],($ra),($nextFp),($FrameAddrs)[($S)],($FrameLayouts)[($S)]);
requires StaticInv(($toAbs),($SectionMem),($SectionAbs));
requires GcStackInv(($S),($toAbs),($FrameCounts)[($S)],($FrameAddrs)[($S)],($FrameLayouts)[($S)],($FrameSlices)[($S)],($fMems)[($S)],($FrameAbss)[($S)],($FrameOffsets)[($S)]);
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
requires 8<=($size);
requires ($size)==4*($nFields);
requires SMemRequire(112,($sMem),esp);

modifies $r1,$r2,$gcMem,$gcSlice,$color,StackTop;
modifies $fs,$fn,CachePtr,CacheSize,ReserveMin,$Time;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;

ensures StaticInv(($toAbs),($SectionMem),($SectionAbs));
ensures GcStackInv(($S),($toAbs),($FrameCounts)[($S)],($FrameAddrs)[($S)],($FrameLayouts)[($S)],($FrameSlices)[($S)],($fMems)[($S)],($FrameAbss)[($S)],($FrameOffsets)[($S)]);
ensures NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures eax!=0==>($fs)[eax]>=($size);
ensures eax!=0==>HeapLo<eax&&eax+($size)<=HeapHi;
ensures eax!=0==>Disconnected(HeapLo,eax,($fs),($fn));
ensures eax!=0==>(CacheSize!=0==>eax!=CachePtr);
ensures eax!=0==>Aligned(eax);
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 2689
  var size:int;
#line 2689
  var nextFp:int;
#line 2689
  var ra:int;
#line 2718
  call ra:=Mov(ecx);
  
  call ecx:=Mov(eax);
  call eax:=AddChecked(eax,8);
  havoc $Eip;
#line 2722
  if(eax>CacheSize)
  {
#line 2722
    goto skip1;
  }
#line 2723
  call allocFromCache(($size),($nFields));
  havoc $Eip;
#line 2724
  goto end;
  
  skip1:
  call nextFp:=Mov(edx);
  call size:=Mov(ecx);
  call _call();
#line 2729
  call Call();
#line 2729
  call AllocSlow(($size),($nFields));
#line 2740
  end:
}

procedure doAllocWord($ret:int,$ind:int,$nf:int)
#line 2743

requires esi==($ret)+4*($ind);

requires (TO(($ind))&&($ind)>=0)&&($ind)<($nf);
requires (Aligned(($ret))&&HeapLo<=($ret))&&($ret)+4*($ind)+4<=HeapHi;
requires MsInv(HeapLo,HeapHi,($color),($gcMem),($toAbs),($AbsMem),($gcSlice));
requires GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($toAbs),($toAbs),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires ObjectSeq(HeapLo,HeapHi,($toAbs),($fs),($fn));
requires FreeInvBase(HeapLo,HeapHi,($fs),($fn),($gcMem),CachePtr,CacheSize);
requires (forall i:int::{TV(i)}TV(i)==>(((HeapLo<=i&&i<HeapHi)&&($fs)[i]!=0)&&i!=($ret)==>FreeInvI(i,HeapLo,HeapHi,($fs),($fn),($gcMem),CachePtr,CacheSize)));

requires (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>($gcSlice)[($ret)+4*j]==($ret)));
requires (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>gcAddr(($ret)+4*j)));
requires (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>($gcMem)[($ret)+4*j]==NULL));
requires ($fs)[($ret)]>=4*($nf);
requires CacheSize!=0==>($ret)!=CachePtr;
requires Disconnected(HeapLo,($ret),($fs),($fn));

modifies $Eip,$gcMem,$Mem,$gcSlice;

ensures MsInv(HeapLo,HeapHi,($color),($gcMem),($toAbs),($AbsMem),($gcSlice));
ensures GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($toAbs),($toAbs),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures ObjectSeq(HeapLo,HeapHi,($toAbs),($fs),($fn));
ensures FreeInvBase(HeapLo,HeapHi,($fs),($fn),($gcMem),CachePtr,CacheSize);
ensures (forall i:int::{TV(i)}TV(i)==>(((HeapLo<=i&&i<HeapHi)&&($fs)[i]!=0)&&i!=($ret)==>FreeInvI(i,HeapLo,HeapHi,($fs),($fn),($gcMem),CachePtr,CacheSize)));

ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)+1==>($gcSlice)[($ret)+4*j]==($ret)));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)+1==>gcAddr(($ret)+4*j)));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)+1==>($gcMem)[($ret)+4*j]==NULL));
ensures ebp==old(ebp);
{
  assert TV(($ret));
  assert TV(($ret)+4*($ind));
  $gcSlice:=($gcSlice)[(($ret)+4*($ind)):=($ret)];
  
  call gcStore(esi,0);
#line 2778
  call Store(esi,0);
}

procedure doAllocWords($ret:int,$size:int,$nf:int)
#line 2781

requires eax==($ret);
requires ebx==($ret)+($size);
requires ($size)==($nf)*4;
requires ($nf)>=0;
requires (Aligned(($ret))&&HeapLo<=($ret))&&($ret)+($size)<=HeapHi;
requires MsInv(HeapLo,HeapHi,($color),($gcMem),($toAbs),($AbsMem),($gcSlice));
requires GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($toAbs),($toAbs),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires ObjectSeq(HeapLo,HeapHi,($toAbs),($fs),($fn));
requires FreeInvBase(HeapLo,HeapHi,($fs),($fn),($gcMem),CachePtr,CacheSize);
requires (forall i:int::{TV(i)}TV(i)==>(((HeapLo<=i&&i<HeapHi)&&($fs)[i]!=0)&&i!=($ret)==>FreeInvI(i,HeapLo,HeapHi,($fs),($fn),($gcMem),CachePtr,CacheSize)));

requires ($fs)[($ret)]>=4*($nf);
requires CacheSize!=0==>($ret)!=CachePtr;
requires Disconnected(HeapLo,($ret),($fs),($fn));

modifies $gcMem,$Mem,$gcSlice;
modifies $Eip,esi;

ensures MsInv(HeapLo,HeapHi,($color),($gcMem),($toAbs),($AbsMem),($gcSlice));
ensures GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($toAbs),($toAbs),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures ObjectSeq(HeapLo,HeapHi,($toAbs),($fs),($fn));
ensures FreeInvBase(HeapLo,HeapHi,($fs),($fn),($gcMem),CachePtr,CacheSize);
ensures (forall i:int::{TV(i)}TV(i)==>(((HeapLo<=i&&i<HeapHi)&&($fs)[i]!=0)&&i!=($ret)==>FreeInvI(i,HeapLo,HeapHi,($fs),($fn),($gcMem),CachePtr,CacheSize)));

ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($nf)==>($gcSlice)[($ret)+4*j]==($ret)));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($nf)==>gcAddr(($ret)+4*j)));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($nf)==>($gcMem)[($ret)+4*j]==NULL));
ensures ebp==old(ebp);
{
#line 2781
  var $ind:int;
#line 2812
  $ind:=0;
  
  call esi:=Mov(GcLo);
  call esi:=Mov(eax);
  
  
  havoc $Eip;
#line 2818
  if(esi>=ebx)
  {
#line 2818
    goto loopEnd;
  }
#line 2819
  loop:
  assert 4*($ind)<($size);
  assert esi==($ret)+4*($ind);
  assert TO(($ind))&&($ind)>=0;
  assert MsInv(HeapLo,HeapHi,($color),($gcMem),($toAbs),($AbsMem),($gcSlice));
  assert GcInv(ColorBase,HeapLo,HeapHi,($color),($toAbs),($toAbs),($toAbs),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert ObjectSeq(HeapLo,HeapHi,($toAbs),($fs),($fn));
  assert FreeInvBase(HeapLo,HeapHi,($fs),($fn),($gcMem),CachePtr,CacheSize);
  assert (forall i:int::{TV(i)}TV(i)==>(((HeapLo<=i&&i<HeapHi)&&($fs)[i]!=0)&&i!=($ret)==>FreeInvI(i,HeapLo,HeapHi,($fs),($fn),($gcMem),CachePtr,CacheSize)));
  
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>($gcSlice)[($ret)+4*j]==($ret)));
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>gcAddr(($ret)+4*j)));
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>($gcMem)[($ret)+4*j]==NULL));
  
  call doAllocWord(($ret),($ind),($nf));
  $ind:=($ind)+1;
  call esi:=Add(esi,4);
  havoc $Eip;
#line 2836
  if(esi<ebx)
  {
#line 2836
    goto loop;
  }
#line 2837
  loopEnd:
}

procedure doAllocObject($nextFp:int,$ptr:int,$abs:int,$vt:int,$size:int)
#line 2840

requires eax==($ptr);
requires ebx==($ptr)+($size);
requires ecx==($vt);
requires (HeapLo<($ptr)&&($ptr)+4*numFields(($abs))<=HeapHi)&&Aligned(($ptr));
requires !VFieldPtr(($abs),0);
requires !VFieldPtr(($abs),1);
requires 2<=numFields(($abs));
requires ($size)==4*numFields(($abs));
requires ObjSize(($abs),($vt),(($AbsMem)[($abs)])[2],(($AbsMem)[($abs)])[3]);
requires __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
requires ($fs)[($ptr)]>=4*numFields(($abs));
requires CacheSize!=0==>($ptr)!=CachePtr;
requires Disconnected(HeapLo,($ptr),($fs),($fn));


requires word(($vt))&&!gcAddrEx(($vt));
requires VTable(($abs),($vt));
requires ObjSize(($abs),($vt),0,0);
requires !isVarSize(tag(($vt)));


requires ($abs)!=NO_ABS;
requires (forall i:int::{TV(i)}TV(i)==>(gcAddr(i)==>($toAbs)[i]!=($abs)));
requires (($AbsMem)[($abs)])[0]==NULL;
requires (($AbsMem)[($abs)])[1]==($vt);
requires (forall j:int::{TO(j)}TO(j)==>(2<=j&&j<numFields(($abs))==>(($AbsMem)[($abs)])[j]==NULL));
requires SMemRequire(100,($sMem),esp);

modifies $gcMem,$toAbs,$gcSlice,$fs,$color;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;

ensures __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
ensures old($toAbs)[eax-4]==NO_ABS;
ensures ($toAbs)==old($toAbs)[(eax-4):=($abs)];
ensures Pointer(($toAbs),eax-4,($abs));
ensures ebp==old(ebp);
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
  assert TV(($ptr))&&TV(($fn)[($ptr)]);
  
  assert TV(eax+0)&&TV(eax+4);
  assert TO(1)&&TO(2);
  assert TO(numFields(($abs)));
  
  call doAllocWords(eax,($size),numFields(($abs)));
  
  call gcStore(eax+4,ecx);
#line 2887
  call Store(eax+4,ecx);
  
  $toAbs:=($toAbs)[($ptr):=($abs)];
  call ebx:=Mov(eax);
  call esi:=Mov(eax);
  call esi:=Sub(esi,HeapLo);
  call edi:=Mov(ColorBase);
  call edx:=Mov(1);
  call bb4SetColor(($color),1,HeapLo,HeapLo,HeapLo,HeapHi,($ptr),ColorBase,HeapLo);
  $color:=($color)[($ptr):=1];
  $fs:=($fs)[($ptr):=0];
  
  assert TO(numFields(($abs)));
  
  call eax:=Lea(ebx+4);
  
  assert TV(eax+4);
  assert TO(0);
}

procedure doAllocString($nextFp:int,$ptr:int,$abs:int,$vt:int,$nElems:int)
#line 2907

requires eax==($ptr);
requires ebx==($ptr)+pad(16+2*($nElems));
requires ecx==pad(16+2*($nElems));
requires edx==($nElems);
requires ($vt)==?STRING_VTABLE;
requires ($ptr)+pad(16+2*($nElems))+0<=HeapHi;

requires (HeapLo<($ptr)&&($ptr)+4*numFields(($abs))<=HeapHi)&&Aligned(($ptr));
requires !VFieldPtr(($abs),0);
requires !VFieldPtr(($abs),1);
requires 2<=numFields(($abs));
requires ObjSize(($abs),($vt),(($AbsMem)[($abs)])[2],(($AbsMem)[($abs)])[3]);
requires __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
requires ($fs)[($ptr)]>=4*numFields(($abs));
requires CacheSize!=0==>($ptr)!=CachePtr;
requires Disconnected(HeapLo,($ptr),($fs),($fn));


requires word(($vt))&&!gcAddrEx(($vt));
requires word(($nElems));
requires VTable(($abs),($vt));
requires ObjSize(($abs),($vt),($nElems),0);
requires tag(($vt))==?STRING_TAG;


requires ($abs)!=NO_ABS;
requires (forall i:int::{TV(i)}TV(i)==>(gcAddr(i)==>($toAbs)[i]!=($abs)));
requires (($AbsMem)[($abs)])[0]==NULL;
requires (($AbsMem)[($abs)])[1]==($vt);
requires (($AbsMem)[($abs)])[2]==($nElems);
requires (($AbsMem)[($abs)])[3]==($nElems)-1;
requires (forall j:int::{TO(j)}TO(j)==>(4<=j&&4*j<pad(16+2*($nElems))==>(($AbsMem)[($abs)])[j]==NULL));
requires SMemRequire(100,($sMem),esp);

modifies $gcMem,$toAbs,$gcSlice,$fs,$color;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;

ensures __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
ensures old($toAbs)[eax-4]==NO_ABS;
ensures ($toAbs)==old($toAbs)[(eax-4):=($abs)];
ensures Pointer(($toAbs),eax-4,($abs));
ensures ebp==old(ebp);
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
  assert ((TV(eax+0)&&TV(eax+4))&&TV(eax+8))&&TV(eax+12);
  assert (TO(1)&&TO(2))&&TO(3);
  assert TVL(($abs));
  assert TVT(($abs),($vt));
  assert TO(numFields(($abs)));
  
  call doAllocWords(eax,pad(16+2*($nElems)),numFields(($abs)));
  
  call gcStore(eax+8,edx);
#line 2960
  call Store(eax+8,edx);
  call edx:=SubChecked(edx,1);
  call gcStore(eax+12,edx);
#line 2962
  call Store(eax+12,edx);
  call edx:=Mov(?STRING_VTABLE);
  call gcStore(eax+4,edx);
#line 2964
  call Store(eax+4,edx);
  
  $toAbs:=($toAbs)[($ptr):=($abs)];
  call ebx:=Mov(eax);
  call esi:=Mov(eax);
  call esi:=Sub(esi,HeapLo);
  call edi:=Mov(ColorBase);
  call edx:=Mov(1);
  call bb4SetColor(($color),1,HeapLo,HeapLo,HeapLo,HeapHi,($ptr),ColorBase,HeapLo);
  $color:=($color)[($ptr):=1];
  $fs:=($fs)[($ptr):=0];
  
  assert TO(numFields(($abs)));
  
  call eax:=Lea(ebx+4);
  
  assert TV(eax+4);
  assert TO(0);
}

procedure doAllocArrayHelper($abs:int,$vt:int,$rank:int,$nElems:int)
#line 2984

requires ecx==($vt);
requires esi==($nElems);
requires VTable(($abs),($vt));
requires ObjSize(($abs),($vt),($rank),($nElems));
requires tag(($vt))==?PTR_ARRAY_TAG||tag(($vt))==?OTHER_ARRAY_TAG;
modifies $Eip,eax,ebx,edx,edi;
ensures !VFieldPtr(($abs),0);
ensures !VFieldPtr(($abs),1);
ensures !VFieldPtr(($abs),2);
ensures !VFieldPtr(($abs),3);
ensures between(4,numFields(($abs)),baseWords(($vt)));
ensures eax==4*numFields(($abs));
{
  assert TO(2)&&TO(3);
  assert TVL(($abs));
  assert TVT(($abs),($vt));
  
  call eax:=RoLoad32(ecx+?VT_BASE_LENGTH);
  call ebx:=RoLoad32(ecx+?VT_ARRAY_ELEMENT_SIZE);
  call edi:=RoLoad32(ecx+?VT_MASK);
  call edi:=And(edi,15);
  
  
  havoc $Eip;
#line 3008
  if(edi!=?PTR_ARRAY_TAG)
  {
#line 3008
    goto skip1;
  }
#line 3009
  call edi:=Mov(esi);
  call edi:=AddChecked(edi,edi);
  call edi:=AddChecked(edi,edi);
  call eax:=AddChecked(eax,edi);
  call eax:=AddChecked(eax,3);
  call edi:=Mov(3);
  call edi:=Not(edi);
  call eax:=And(eax,edi);
  havoc $Eip;
#line 3017
  goto skip2;
  
  skip1:
  call edi:=Mov(eax);
  call eax:=Mov(ebx);
  call eax,edx:=MulChecked(eax,esi);
  call eax:=AddChecked(eax,edi);
  call eax:=AddChecked(eax,3);
  call edi:=Mov(3);
  call edi:=Not(edi);
  call eax:=And(eax,edi);
  skip2:
}

procedure doAllocArray($nextFp:int,$ptr:int,$abs:int,$vt:int,$rank:int,$nElems:int,$size:int)
#line 3031

requires eax==($ptr);
requires ebx==($ptr)+($size);
requires ecx==($vt);
requires edx==($rank);
requires esi==($nElems);
requires (HeapLo<($ptr)&&($ptr)+4*numFields(($abs))<=HeapHi)&&Aligned(($ptr));
requires !VFieldPtr(($abs),0);
requires !VFieldPtr(($abs),1);
requires !VFieldPtr(($abs),2);
requires !VFieldPtr(($abs),3);
requires 4<=numFields(($abs));
requires ($size)==4*numFields(($abs));
requires ObjSize(($abs),($vt),($rank),($nElems));
requires __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
requires ($fs)[($ptr)]>=4*numFields(($abs));
requires CacheSize!=0==>($ptr)!=CachePtr;
requires Disconnected(HeapLo,($ptr),($fs),($fn));


requires word(($vt))&&!gcAddrEx(($vt));
requires word(($rank));
requires word(($nElems));
requires VTable(($abs),($vt));
requires ObjSize(($abs),($vt),($rank),($nElems));
requires tag(($vt))==?PTR_ARRAY_TAG||tag(($vt))==?OTHER_ARRAY_TAG;


requires ($abs)!=NO_ABS;
requires (forall i:int::{TV(i)}TV(i)==>(gcAddr(i)==>($toAbs)[i]!=($abs)));
requires (($AbsMem)[($abs)])[0]==NULL;
requires (($AbsMem)[($abs)])[1]==($vt);
requires (($AbsMem)[($abs)])[2]==($rank);
requires (($AbsMem)[($abs)])[3]==($nElems);
requires (forall j:int::{TO(j)}TO(j)==>(4<=j&&j<numFields(($abs))==>(($AbsMem)[($abs)])[j]==NULL));
requires SMemRequire(100,($sMem),esp);

modifies $gcMem,$toAbs,$gcSlice,$fs,$color;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;

ensures __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
ensures old($toAbs)[eax-4]==NO_ABS;
ensures ($toAbs)==old($toAbs)[(eax-4):=($abs)];
ensures Pointer(($toAbs),eax-4,($abs));
ensures ebp==old(ebp);
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 3031
  var nElems:int;
#line 3079
  call nElems:=Mov(esi);
  
  assert ((TV(eax+0)&&TV(eax+4))&&TV(eax+8))&&TV(eax+12);
  assert ((TO(1)&&TO(2))&&TO(3))&&TO(4);
  assert TO(numFields(($abs)));
  
  call doAllocWords(eax,($size),numFields(($abs)));
  
  call esi:=Mov(nElems);
  call gcStore(eax+4,ecx);
#line 3088
  call Store(eax+4,ecx);
  call gcStore(eax+8,edx);
#line 3089
  call Store(eax+8,edx);
  call gcStore(eax+12,esi);
#line 3090
  call Store(eax+12,esi);
  
  $toAbs:=($toAbs)[($ptr):=($abs)];
  call ebx:=Mov(eax);
  call esi:=Mov(eax);
  call esi:=Sub(esi,HeapLo);
  call edi:=Mov(ColorBase);
  call edx:=Mov(1);
  call bb4SetColor(($color),1,HeapLo,HeapLo,HeapLo,HeapHi,($ptr),ColorBase,HeapLo);
  $color:=($color)[($ptr):=1];
  $fs:=($fs)[($ptr):=0];
  
  assert TO(numFields(($abs)));
  
  call eax:=Lea(ebx+4);
  
  assert TV(eax+4);
  assert TO(0);
}

procedure doAllocVectorHelper($abs:int,$vt:int,$nElems:int)
#line 3110

requires ecx==($vt);
requires edx==($nElems);
requires VTable(($abs),($vt));
requires ObjSize(($abs),($vt),($nElems),0);
requires tag(($vt))==?PTR_VECTOR_TAG||tag(($vt))==?OTHER_VECTOR_TAG;
modifies $Eip,eax,ebx,edx,esi,edi;
ensures !VFieldPtr(($abs),0);
ensures !VFieldPtr(($abs),1);
ensures !VFieldPtr(($abs),2);
ensures 3<=numFields(($abs));
ensures eax==4*numFields(($abs));
ensures ObjSize(($abs),($vt),($nElems),(($AbsMem)[($abs)])[3]);
{
  assert TO(2);
  assert TVL(($abs));
  assert TVT(($abs),($vt));
  
  call eax:=RoLoad32(ecx+?VT_BASE_LENGTH);
  call ebx:=RoLoad32(ecx+?VT_ARRAY_ELEMENT_SIZE);
  call edi:=RoLoad32(ecx+?VT_MASK);
  call edi:=And(edi,15);
  
  
  havoc $Eip;
#line 3134
  if(edi!=?PTR_VECTOR_TAG)
  {
#line 3134
    goto skip1;
  }
#line 3135
  call edi:=Mov(edx);
  call edi:=AddChecked(edi,edi);
  call edi:=AddChecked(edi,edi);
  call eax:=AddChecked(eax,edi);
  call eax:=AddChecked(eax,3);
  call edi:=Mov(3);
  call edi:=Not(edi);
  call eax:=And(eax,edi);
  havoc $Eip;
#line 3143
  goto skip2;
  
  skip1:
  call edi:=Mov(eax);
  call eax:=Mov(ebx);
  call eax,edx:=MulChecked(eax,edx);
  call eax:=AddChecked(eax,edi);
  call eax:=AddChecked(eax,3);
  call edi:=Mov(3);
  call edi:=Not(edi);
  call eax:=And(eax,edi);
  skip2:
}

procedure doAllocVector($nextFp:int,$ptr:int,$abs:int,$vt:int,$nElems:int,$size:int)
#line 3157

requires eax==($ptr);
requires ebx==($ptr)+($size);
requires ecx==($vt);
requires edx==($nElems);
requires (HeapLo<($ptr)&&($ptr)+4*numFields(($abs))<=HeapHi)&&Aligned(($ptr));
requires !VFieldPtr(($abs),0);
requires !VFieldPtr(($abs),1);
requires !VFieldPtr(($abs),2);
requires 3<=numFields(($abs));
requires ($size)==4*numFields(($abs));
requires ObjSize(($abs),($vt),($nElems),(($AbsMem)[($abs)])[3]);
requires __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
requires ($fs)[($ptr)]>=4*numFields(($abs));
requires CacheSize!=0==>($ptr)!=CachePtr;
requires Disconnected(HeapLo,($ptr),($fs),($fn));


requires word(($vt))&&!gcAddrEx(($vt));
requires word(($nElems));
requires VTable(($abs),($vt));
requires ObjSize(($abs),($vt),($nElems),0);
requires tag(($vt))==?PTR_VECTOR_TAG||tag(($vt))==?OTHER_VECTOR_TAG;


requires ($abs)!=NO_ABS;
requires (forall i:int::{TV(i)}TV(i)==>(gcAddr(i)==>($toAbs)[i]!=($abs)));
requires (($AbsMem)[($abs)])[0]==NULL;
requires (($AbsMem)[($abs)])[1]==($vt);
requires (($AbsMem)[($abs)])[2]==($nElems);
requires (forall j:int::{TO(j)}TO(j)==>(3<=j&&j<numFields(($abs))==>(($AbsMem)[($abs)])[j]==NULL));
requires SMemRequire(100,($sMem),esp);

modifies $gcMem,$toAbs,$gcSlice,$fs,$color;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;

ensures __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
ensures old($toAbs)[eax-4]==NO_ABS;
ensures ($toAbs)==old($toAbs)[(eax-4):=($abs)];
ensures Pointer(($toAbs),eax-4,($abs));
ensures ebp==old(ebp);
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
  assert (TV(eax+0)&&TV(eax+4))&&TV(eax+8);
  assert (TO(1)&&TO(2))&&TO(3);
  assert TO(numFields(($abs)));
  
  call doAllocWords(eax,($size),numFields(($abs)));
  
  call gcStore(eax+4,ecx);
#line 3206
  call Store(eax+4,ecx);
  call gcStore(eax+8,edx);
#line 3207
  call Store(eax+8,edx);
  
  
  $toAbs:=($toAbs)[($ptr):=($abs)];
  call ebx:=Mov(eax);
  call esi:=Mov(eax);
  call esi:=Sub(esi,HeapLo);
  call edi:=Mov(ColorBase);
  call edx:=Mov(1);
  call bb4SetColor(($color),1,HeapLo,HeapLo,HeapLo,HeapHi,($ptr),ColorBase,HeapLo);
  $color:=($color)[($ptr):=1];
  $fs:=($fs)[($ptr):=0];
  
  assert TO(numFields(($abs)));
  
  call eax:=Lea(ebx+4);
  
  assert TV(eax+4);
  assert TO(0);
}

implementation InitializeGc()
#line 3228
{
#line 3228
  var unitSize:int;
#line 3228
  var save:int;
  
  
  
  call save:=Mov(ebp);
#line 3250
  call esi:=Mov(GcHi);
  call esi:=Sub(esi,GcLo);
  call edi:=Mov(esi);
  
  
  call edx:=Mov(0);
  call eax:=Mov(edi);
  call ebx:=Mov(72);
  call eax,edx:=Div(eax,edx,ebx);
  call ebp:=Mov(eax);
  call unitSize:=Mov(ebp);
  assert 72*unitSize<=?gcHi-?gcLo;
  
  havoc $Eip;
#line 3263
  if(ebp!=0)
  {
#line 3263
    goto skip1;
  }
  call eax:=Mov(1431633991);
  call _call();
#line 3266
  call Call();
#line 3266
  call DebugBreak();
  skip1:
  
  
  call edx:=Mov(0);
  call ebp:=Lea(edx+4*ebp);
  call eax:=Mov(GcLo);
  call eax:=Add(eax,ebp);
  call ColorBase:=Mov(eax);
  
  call ebx:=Lea(eax+ebp);
  call HeapLo:=Mov(ebx);
  
  call ebp:=Lea(edx+4*ebp);
  call ecx:=Lea(ebx+4*ebp);
  call HeapHi:=Mov(ecx);
  
  
  assert TV(?gcLo);
  assert TV(ColorBase);
  assert TV(HeapLo);
  assert TV(HeapLo+8);
  assert TV(HeapHi);
  assert TO(0);
  assert TO(1);
  assert TO(2);
  assert TO(3);
  assert TO(unitSize);
  assert TO(2*unitSize);
  assert TO(16*unitSize);
  
  $fs:=MAP_ZERO;
  $fn:=MAP_ZERO;
  $color:=MAP_ZERO;
  call eax:=Mov(ColorBase);
  call ebx:=Mov(HeapLo);
  
  
  
  
  
  call _call();
#line 3307
  call Call();
#line 3307
  call BB4Zero2(($color),HeapLo,HeapLo,HeapLo,HeapHi,0,ColorBase,HeapLo,0,0);
  
  $fn:=($fn)[HeapLo:=(HeapLo+8)];
  $fs:=($fs)[HeapLo:=8];
  call eax:=Mov(HeapLo);
  call ebx:=Lea(eax+8);
  call gcStore(eax,ebx);
#line 3313
  call Store(eax,ebx);
  call gcStore(eax+4,8);
#line 3314
  call Store(eax+4,8);
  $fs:=($fs)[(HeapLo+8):=(HeapHi-(HeapLo+8))];
  $fn:=($fn)[(HeapLo+8):=0];
  call gcStore(eax+8,0);
#line 3317
  call Store(eax+8,0);
  call ecx:=Mov(HeapHi);
  call ecx:=Sub(ecx,ebx);
  call gcStore(eax+12,ecx);
#line 3320
  call Store(eax+12,ecx);
  call CacheSize:=Mov(0);
  
  call eax:=Mov(HeapLo);
  
  
  
  call ReserveMin:=Mov(eax);
  
  call ebp:=Mov(save);
  
  $stackState:=($StackState);
  
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 3333
  call Ret(old($RET));
#line 3333
  return;
}

implementation revealInv1($s:int,$_stackState:[int]StackState)
#line 3336
{
}


implementation updateInv1($oldPciConfigState:[int]int,$oldPciMem:[int]int,$oldMem:[int]int)
#line 3340
{
}


implementation setStackEmpty($s:int)
#line 3344
{
  
  call tStore(($s),eax,?STACK_EMPTY);
#line 3346
  call Store(eax,?STACK_EMPTY);
  $stackState:=($StackState)[($s):=StackEmpty];
}

implementation setStackYielded()
#line 3350
{
  
  assert (TV(eax)&&TO(1))&&TO(2);
  call tStore(($S),eax,?STACK_YIELDED);
#line 3353
  call Store(eax,?STACK_YIELDED);
  call tStore(($S),eax+4,esp);
#line 3354
  call Store(eax+4,esp);
  call tStore(($S),eax+8,ebp);
#line 3355
  call Store(eax+8,ebp);
  $stackState:=($StackState)[($S):=StackYielded(ebp,esp+4,($Mem)[esp])];
  call revealFMem();
}

implementation setStackInterrupted()
#line 3360
{
#line 3360
  var saveEdx:int;
#line 3360
  var saveEax:int;
  
  call saveEax:=Mov(eax);
  call saveEdx:=Mov(edx);
  
  call eax:=Mov(CurrentStack);
  call edx:=Mov(?TSize);
  call eax,edx:=Mul(eax,edx);
  call eax:=Add(eax,TLo);
  
  assert TV(TLo)&&TO(64*($S));
  assert (((((((TV(eax)&&TO(1))&&TO(2))&&TO(3))&&TO(4))&&TO(5))&&TO(6))&&TO(7))&&TO(8);
  call tStore(($S),eax,?STACK_INTERRUPTED);
#line 3372
  call Store(eax,?STACK_INTERRUPTED);
  call tStore(($S),eax+4,esp);
#line 3373
  call Store(eax+4,esp);
  call tStore(($S),eax+8,ebp);
#line 3374
  call Store(eax+8,ebp);
  call edx:=Mov(saveEax);
  call tStore(($S),eax+12,edx);
#line 3376
  call Store(eax+12,edx);
  call tStore(($S),eax+16,ebx);
#line 3377
  call Store(eax+16,ebx);
  call tStore(($S),eax+20,ecx);
#line 3378
  call Store(eax+20,ecx);
  call edx:=Mov(saveEdx);
  call tStore(($S),eax+24,edx);
#line 3380
  call Store(eax+24,edx);
  call tStore(($S),eax+28,esi);
#line 3381
  call Store(eax+28,esi);
  call tStore(($S),eax+32,edi);
#line 3382
  call Store(eax+32,edi);
  $stackState:=($StackState)[($S):=StackInterrupted(old(eax),ebx,ecx,old(edx),esi,edi,ebp,esp+12,($Mem)[esp],($Mem)[esp+4],($Mem)[esp+8])];
  call revealFMem();
}

implementation setStackRunning1($s:int)
#line 3387
{
  
  call fStore(($s),esp,ecx);
#line 3389
  call Store(esp,ecx);
  call tStore(($s),edi,?STACK_RUNNING);
#line 3390
  call Store(edi,?STACK_RUNNING);
}

implementation setStackRunning2($s:int,$_stackState:[int]StackState)
#line 3393
{
  
  call CurrentStack:=Mov(ecx);
  call fStore(($s),esp,eax);
#line 3396
  call Store(esp,eax);
  call tStore(($s),edi,?STACK_RUNNING);
#line 3397
  call Store(edi,?STACK_RUNNING);
}

implementation setStackRunning3($s:int,$_stackState:[int]StackState,$ebp:int,$esp:int,$eip:int)
#line 3400
{
  
  call CurrentStack:=Mov(ecx);
  assert (TV(edi)&&TO(1))&&TO(2);
  call tLoad(($s),edi+4);
#line 3404
  call esp:=Load(edi+4);
  call tLoad(($s),edi+8);
#line 3405
  call ebp:=Load(edi+8);
  call tStore(($s),edi,?STACK_RUNNING);
#line 3406
  call Store(edi,?STACK_RUNNING);
  call revealFMem();
}

implementation setStackRunning4($s:int,$_stackState:[int]StackState,$eax:int,$ebx:int,$ecx:int,$edx:int,$esi:int,$edi:int,$ebp:int,$esp:int,$eip:int,$cs:int,$efl:int)
#line 3410
{
  
  
  call CurrentStack:=Mov(ecx);
  call tStore(($s),edi,?STACK_RUNNING);
#line 3414
  call Store(edi,?STACK_RUNNING);
  
  assert (((((((TV(edi)&&TO(1))&&TO(2))&&TO(3))&&TO(4))&&TO(5))&&TO(6))&&TO(7))&&TO(8);
  call tLoad(($s),edi+4);
#line 3417
  call esp:=Load(edi+4);
  call tLoad(($s),edi+8);
#line 3418
  call ebp:=Load(edi+8);
  call tLoad(($s),edi+12);
#line 3419
  call eax:=Load(edi+12);
  call tLoad(($s),edi+16);
#line 3420
  call ebx:=Load(edi+16);
  call tLoad(($s),edi+20);
#line 3421
  call ecx:=Load(edi+20);
  call tLoad(($s),edi+24);
#line 3422
  call edx:=Load(edi+24);
  call tLoad(($s),edi+28);
#line 3423
  call esi:=Load(edi+28);
  call tLoad(($s),edi+32);
#line 3424
  call edi:=Load(edi+32);
  call revealFMem();
}

implementation readField($ptr:int,$fld:int)
#line 3428
returns($val:int)
{
  assert TV(($val))&&TV(($ptr));
  assert TO(($fld));
  call gcLoad(($ptr)+4*($fld));
  
  $val:=($gcMem)[($ptr)+4*($fld)];
}

implementation writeField($ptr:int,$fld:int,$val:int,$abs:int)
#line 3437
returns($_mem:[int]int,$_absData:[int][int]int)
{
  assert TVL(($toAbs)[($ptr)]);
  assert TV(($val))&&TV(($ptr));
  assert TO(($fld));
  call gcStore(($ptr)+4*($fld),($val));
  
  
  $_mem:=($Mem)[(($ptr)+4*($fld)):=($val)];
  $_absData:=($AbsMem)[(($toAbs)[($ptr)]):=((($AbsMem)[($toAbs)[($ptr)]])[($fld):=($abs)])];
}


implementation readStack($ptr:int,$frame:int,$j:int)
#line 3450
returns($val:int)
{
  assert TV(($frame))&&TO(($j));
  call fLoad(($S),($ptr));
  $val:=(($fMems)[($S)])[($ptr)];
}

function TXV(i:int)
  returns(bool)
{
  true
}
#line 3458
function TXO(i:int)
  returns(bool)
{
  true
}
#line 3460
procedure _writeStack($ptr:int,$val:int,$frame:int,$j:int,$fX:[int]int,$jX:[int]int,$_frameCount:int,$_frameAddr:[int]int,$_frameLayout:[int]$FrameLayout,$_frameSlice:[int]int,$_frameAbs:[int][int]int,$_frameOffset:[int]int)
#line 3460




returns($_mem:[int]int)
requires isStack(($S))&&($StackState)[($S)]==StackRunning;
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);

requires ($_frameSlice)[($ptr)]==($frame);
requires ($ptr)==($_frameAddr)[($frame)]+4*($j);
requires StackLo(($S))<=($ptr)&&($ptr)<StackHi(($S));
requires Aligned(($ptr));
requires InteriorValue(frameHasPtr(($_frameLayout)[($frame)],($j)),($toAbs),($val),(($_frameAbs)[($frame)])[($j)],($_frameOffset)[($ptr)]);
requires (forall f:int,j:int::{TXV(f),TXO(j)}TXV(f)&&TXO(j)==>((0<=f&&f<($_frameCount))&&_InFrameSlice(f,j,($_frameAddr),($_frameSlice))==>(($_frameAddr)[f]+4*j!=($ptr)==>(((((0<=($fX)[f]&&($fX)[f]<($FrameCounts)[($S)])&&_InFrameSlice(($fX)[f],($jX)[j],($FrameAddrs)[($S)],($FrameSlices)[($S)]))&&($_frameAddr)[f]+4*j==(($FrameAddrs)[($S)])[($fX)[f]]+4*($jX)[j])&&(($_frameAbs)[f])[j]==((($FrameAbss)[($S)])[($fX)[f]])[($jX)[j]])&&frameHasPtr(($_frameLayout)[f],j)==frameHasPtr((($FrameLayouts)[($S)])[($fX)[f]],($jX)[j]))&&($_frameOffset)[($_frameAddr)[f]+4*j]==(($FrameOffsets)[($S)])[(($FrameAddrs)[($S)])[($fX)[f]]+4*($jX)[j]])));








requires (forall f:int,j:int::{TXV(f),TXO(j)}TXV(f)&&TXO(j)==>(_InFrameSlice(f,j,($_frameAddr),($_frameSlice))==>($_frameSlice)[($_frameAddr)[f]+4*j]==f));

requires _MutatorStackInv(($S),($toAbs),($FrameCounts)[($S)],($FrameAddrs)[($S)],($FrameLayouts)[($S)],($FrameSlices)[($S)],($FrameAbss)[($S)],($FrameOffsets)[($S)],($fMems)[($S)]);


modifies $fMems;
ensures ($_mem)==($Mem)[($ptr):=($val)];
ensures ($fMems)==old($fMems)[($S):=((old($fMems)[($S)])[($ptr):=($val)])];
ensures memAddr(($ptr));
ensures word(($val));
ensures _MutatorStackInv(($S),($toAbs),($_frameCount),($_frameAddr),($_frameLayout),($_frameSlice),($_frameAbs),($_frameOffset),($fMems)[($S)]);
ensures __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($_mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts)[($S):=($_frameCount)],($FrameAddrs)[($S):=($_frameAddr)],($FrameLayouts)[($S):=($_frameLayout)],($FrameSlices)[($S):=($_frameSlice)],($FrameAbss)[($S):=($_frameAbs)],($FrameOffsets)[($S):=($_frameOffset)]);
{
  
  
  
  
  
  
  assert TV(($frame))&&TO(($j));
  call fStore(($S),($ptr),($val));
  $_mem:=($Mem)[($ptr):=($val)];
  assert (forall f:int,j:int::{TXV(f),TXO(j)}TXV(f)&&TXO(j)==>TV(($fX)[f])&&TO(($jX)[j]));
}

implementation writeStack($ptr:int,$val:int,$frame:int,$j:int,$fX:[int]int,$jX:[int]int,$_frameCount:int,$_frameAddr:[int]int,$_frameLayout:[int]$FrameLayout,$_frameSlice:[int]int,$_frameAbs:[int][int]int,$_frameOffset:[int]int)
#line 3507




returns($_mem:[int]int)
{
  assert TStk(($S));
  call _inFrameSlice();
  assert (forall f:int,j:int::{TXV(f),TXO(j)}TV(f)&&TO(j)==>((0<=f&&f<($_frameCount))&&_InFrameSlice(f,j,($_frameAddr),($_frameSlice))==>(($_frameAddr)[f]+4*j!=($ptr)==>(((((0<=($fX)[f]&&($fX)[f]<($FrameCounts)[($S)])&&(($FrameSlices)[($S)])[(($FrameAddrs)[($S)])[($fX)[f]]+4*($jX)[j]]==($fX)[f])&&($_frameAddr)[f]+4*j==(($FrameAddrs)[($S)])[($fX)[f]]+4*($jX)[j])&&(($_frameAbs)[f])[j]==((($FrameAbss)[($S)])[($fX)[f]])[($jX)[j]])&&frameHasPtr(($_frameLayout)[f],j)==frameHasPtr((($FrameLayouts)[($S)])[($fX)[f]],($jX)[j]))&&($_frameOffset)[($_frameAddr)[f]+4*j]==(($FrameOffsets)[($S)])[(($FrameAddrs)[($S)])[($fX)[f]]+4*($jX)[j]])));
  
  
  
  
  
  
  
  
  call $_mem:=_writeStack(($ptr),($val),($frame),($j),($fX),($jX),($_frameCount),($_frameAddr),($_frameLayout),($_frameSlice),($_frameAbs),($_frameOffset));
}


implementation AllocObject($abs:int,$vt:int)
#line 3528
{
#line 3528
  var size:int;
#line 3528
  var fp:int;
#line 3528
  var sp:int;
#line 3528
  var vt:int;
  
  
  
  
  
  call sp:=Mov(esp);
  call fp:=Mov(ebp);
  call vt:=Mov(ecx);
  
  assert TO(2);
  assert TVL(($abs));
  assert TVT(($abs),($vt));
  call eax:=RoLoad32(ecx+?VT_BASE_LENGTH);
  call size:=Mov(eax);
  
  call fLoad(($S),esp);
#line 3544
  call ecx:=Load(esp);
  call edx:=Mov(ebp);
  
  call esp:=Mov(DLo);
  
  call allocObjectMemory(ecx,fp,4*numFields(($abs)),numFields(($abs)));
  havoc $Eip;
#line 3550
  if(eax==0)
  {
#line 3550
    goto done;
  }
  call edx:=Mov(size);
  call ebx:=Lea(eax+edx);
  call ecx:=Mov(vt);
  call doAllocObject(fp,eax,($abs),vt,4*numFields(($abs)));
  
  done:
  call ebp:=Mov(fp);
  call esp:=Mov(sp);
  call _fret(($S),old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
  call Ret(old($RET));
  return;
}

implementation AllocString($abs:int,$vt:int,$nElems:int)
#line 3565
{
#line 3565
  var nElems:int;
#line 3565
  var size:int;
#line 3565
  var fp:int;
#line 3565
  var sp:int;
  
  
  
  
  
  call sp:=Mov(esp);
  call fp:=Mov(ebp);
  
  assert TO(2);
  assert TVL(($abs));
  assert TVT(($abs),($vt));
  call ecx:=AddChecked(ecx,1);
  call edx:=Mov(ecx);
  call nElems:=Mov(edx);
  call ecx:=AddChecked(ecx,ecx);
  call ecx:=AddChecked(ecx,19);
  call eax:=Mov(3);
  call eax:=Not(eax);
  call ecx:=And(ecx,eax);
  call size:=Mov(ecx);
  call eax:=Mov(ecx);
  
  call fLoad(($S),esp);
#line 3588
  call ecx:=Load(esp);
  call edx:=Mov(ebp);
  
  call esp:=Mov(DLo);
  
  call allocObjectMemory(ecx,fp,4*numFields(($abs)),numFields(($abs)));
  havoc $Eip;
#line 3594
  if(eax==0)
  {
#line 3594
    goto done;
  }
  call edx:=Mov(nElems);
  call ecx:=Mov(size);
  call ebx:=Lea(eax+ecx);
  call doAllocString(fp,eax,($abs),?STRING_VTABLE,($nElems));
  
  done:
  call ebp:=Mov(fp);
  call esp:=Mov(sp);
  call _fret(($S),old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
  call Ret(old($RET));
  return;
}

implementation AllocArray($abs:int,$vt:int,$rank:int,$nElems:int)
#line 3609
{
#line 3609
  var fp:int;
#line 3609
  var sp:int;
#line 3609
  var nElems:int;
#line 3609
  var size:int;
#line 3609
  var rank:int;
#line 3609
  var vt:int;
  
  
  
  
  
  
  
  
  call rank:=Mov(edx);
  call sp:=Mov(esp);
  call fp:=Mov(ebp);
  call vt:=Mov(ecx);
  assert TO(1)&&TV(esp);
  call fLoad(($S),esp+4);
#line 3623
  call esi:=Load(esp+4);
  call nElems:=Mov(esi);
  
  call doAllocArrayHelper(($abs),($vt),($rank),($nElems));
  call size:=Mov(eax);
  
  call fLoad(($S),esp);
#line 3629
  call ecx:=Load(esp);
  call edx:=Mov(ebp);
  
  call esp:=Mov(DLo);
  
  call allocObjectMemory(ecx,fp,4*numFields(($abs)),numFields(($abs)));
  havoc $Eip;
#line 3635
  if(eax==0)
  {
#line 3635
    goto done;
  }
  call edx:=Mov(size);
  call ebx:=Lea(eax+edx);
  call edx:=Mov(rank);
  call esi:=Mov(nElems);
  call ecx:=Mov(vt);
  call doAllocArray(fp,eax,($abs),vt,($rank),($nElems),4*numFields(($abs)));
  
  done:
  call ebp:=Mov(fp);
  call esp:=Mov(sp);
  call _fret(($S),old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
  call Ret(old($RET));
  return;
}

implementation AllocVector($abs:int,$vt:int,$nElems:int)
#line 3652
{
#line 3652
  var fp:int;
#line 3652
  var sp:int;
#line 3652
  var nElems:int;
#line 3652
  var size:int;
#line 3652
  var vt:int;
  
  
  
  
  
  
  
  call sp:=Mov(esp);
  call fp:=Mov(ebp);
  call vt:=Mov(ecx);
  call nElems:=Mov(edx);
  
  call doAllocVectorHelper(($abs),($vt),($nElems));
  call size:=Mov(eax);
  
  call fLoad(($S),esp);
#line 3668
  call ecx:=Load(esp);
  call edx:=Mov(ebp);
  
  call esp:=Mov(DLo);
  
  call allocObjectMemory(ecx,fp,4*numFields(($abs)),numFields(($abs)));
  havoc $Eip;
#line 3674
  if(eax==0)
  {
#line 3674
    goto done;
  }
  call edx:=Mov(size);
  call ebx:=Lea(eax+edx);
  call edx:=Mov(nElems);
  call ecx:=Mov(vt);
  call doAllocVector(fp,eax,($abs),vt,($nElems),4*numFields(($abs)));
  
  done:
  call ebp:=Mov(fp);
  call esp:=Mov(sp);
  call _fret(($S),old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
  call Ret(old($RET));
  return;
}

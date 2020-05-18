#line 29
function IsFwdPtr(i:int)
  returns(bool)
{
  gcAddrEx(i)
}
#line 31
function ObjectSpc(i1:int,i2:int,r:[int]int)
  returns(bool)
{
  (forall i:int::{TV(i)}TV(i)==>((i1<=i&&i<i2)&&r[i]!=NO_ABS==>i+4*numFields(r[i])<=i2&&(forall ii:int::{TV(ii)}TV(ii)==>(i<ii&&ii<i+4*numFields(r[i])==>r[ii]==NO_ABS))))
}


function ObjectSeq(i1:int,i2:int,r:[int]int)
  returns(bool)
{
  (i1==i2||r[i1]!=NO_ABS)&&(forall i:int::{TV(i)}TV(i)==>((i1<=i&&i<i2)&&r[i]!=NO_ABS==>((forall ii:int::{TV(ii)}TV(ii)==>(i<ii&&ii<i+4*numFields(r[i])==>r[ii]==NO_ABS))&&(forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(r[i])==>gcAddr(i+4*j))))&&(i+4*numFields(r[i])==i2||(i+4*numFields(r[i])<i2&&r[i+4*numFields(r[i])]!=NO_ABS))))
}






function EmptySeq(i1:int,i2:int,r:[int]int,$toAbs:[int]int)
  returns(bool)
{
  (forall i:int::{TV(i)}TV(i)==>(i1<=i&&i<i2==>(gcAddr(i)&&r[i]==NO_ABS)&&($toAbs)[i]==NO_ABS))
}


function CopyGcInv($freshAbs:int,BF:int,BT:int,HeapLo:int,Fi:int,Fk:int,Fl:int,Ti:int,Tj:int,_tj:int,Tk:int,_tk:int,Tl:int,$Time:Time,$r1:[int]int,$r2:[int]int,r1Live:bool,$toAbs:[int]int,$AbsMem:[int][int]int,$gcSlice:[int]int,$Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int)
  returns(bool)
{
  (((((((((((((((((((((((((((((((((((((((((((((?gcLo<=HeapLo&&HeapLo<=?gcHi)&&(((Fi==HeapLo&&Ti==Fl)&&BF==?gcLo)||((Ti==HeapLo&&Fi==Tl)&&BT==?gcLo)))&&(Fi==HeapLo==>?gcLo+BitIndex(HeapLo,Fl)==BT&&?gcLo+BitIndex(HeapLo,Tl)==HeapLo))&&(Ti==HeapLo==>?gcLo+BitIndex(HeapLo,Tl)==BF&&?gcLo+BitIndex(HeapLo,Fl)==HeapLo))&&Fi<=Fk)&&Fk<=Fl)&&Fl<=?gcHi)&&Ti<=Tj)&&Tj<=_tj)&&_tj<=Tk)&&Tk<=_tk)&&_tk<=Tl)&&Tl<=?gcHi)&&Aligned(Fi))&&Aligned(Fk))&&Aligned(Ti))&&Aligned(Tj))&&Aligned(Tk))&&Aligned(?gcLo+BitIndex(HeapLo,Fi)))&&Aligned(?gcLo+BitIndex(HeapLo,Ti)))&&?gcLo<=?gcLo+BitIndex(HeapLo,Fi))&&?gcLo+BitIndex(HeapLo,Fl)<=HeapLo)&&?gcLo<=?gcLo+BitIndex(HeapLo,Ti))&&?gcLo+BitIndex(HeapLo,Tl)<=HeapLo)&&BF==?gcLo+BitIndex(HeapLo,Fi))&&BT==?gcLo+BitIndex(HeapLo,Ti))&&Fl-Fi==Mult(32,BitIndex(HeapLo,Fl)-BitIndex(HeapLo,Fi)))&&Tl-Ti==Mult(32,BitIndex(HeapLo,Tl)-BitIndex(HeapLo,Ti)))&&bbvec4(($r1),NO_ABS,Fi,($gcMem),Fi,Fi,Fl,?gcLo+BitIndex(HeapLo,Fi),?gcLo+BitIndex(HeapLo,Fl)))&&bbvec4(($toAbs),NO_ABS,Ti,($gcMem),Ti,Ti,Tl,?gcLo+BitIndex(HeapLo,Ti),?gcLo+BitIndex(HeapLo,Tl)))&&WellFormed(($toAbs)))&&ObjectSpc(Fi,Fk,($r1)))&&EmptySeq(Fk,Fl,($r1),($toAbs)))&&ObjectSpc(Ti,Tj,($r2)))&&ObjectSeq(_tj,Tk,($r2)))&&EmptySeq(_tk,Tl,($r2),($toAbs)))&&(forall i:int::{TV(i)}TV(i)==>(gcAddr(i)==>(($r1)[i]!=NO_ABS&&r1Live==>Fi<=i&&i<Fk))))&&(forall i:int::{TV(i)}TV(i)==>(gcAddr(i)==>(($r2)[i]!=NO_ABS==>Ti<=i&&i<_tk))))&&(forall i:int::{TV(i)}TV(i)==>((Fi<=i&&i<Fk)&&($r1)[i]!=NO_ABS==>(((((IsFwdPtr(($gcMem)[i+4])<==>($toAbs)[i]==NO_ABS)&&(IsFwdPtr(($gcMem)[i+4])==>(Pointer(($r2),($gcMem)[i+4]-4,($r1)[i])&&AlignedHeapAddr(i+4))&&word(($gcMem)[i+4])))&&(!IsFwdPtr(($gcMem)[i+4])==>($toAbs)[i]==($r1)[i]))&&(!IsFwdPtr(($gcMem)[i+4])==>(r1Live==>ObjInv(i,($r1),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice)))))&&i+4<Fk)&&Aligned(i))))&&(forall i:int::{TV(i)}TV(i)==>((Fi<=i&&i<Fl)&&($toAbs)[i]!=NO_ABS==>($r1)[i]!=NO_ABS&&($r1)[i]!=NO_ABS)))&&(forall i:int::{TV(i)}TV(i)==>((Ti<=i&&i<Tl)&&($toAbs)[i]!=NO_ABS==>($r2)[i]!=NO_ABS&&($r2)[i]!=NO_ABS)))&&(forall i:int::{TV(i)}TV(i)==>((Ti<=i&&i<Tj)&&($r2)[i]!=NO_ABS==>(((($toAbs)[i]!=NO_ABS&&($toAbs)[i]==($r2)[i])&&reached(($toAbs)[i],($Time)))&&ObjInv(i,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice)))&&!IsFwdPtr(($gcMem)[i+4]))))&&(Tj!=_tj==>(forall j:int::{TO(j)}TO(j)==>(0<=j&&Tj+4*j<_tj==>gcAddr(Tj+4*j)&&($gcSlice)[Tj+4*j]==Tj))))&&(forall i:int::{TV(i)}TV(i)==>(Tj<i&&i<_tj==>($r2)[i]==NO_ABS)))&&(forall i:int::{TV(i)}TV(i)==>((_tj<=i&&i<Tk)&&($r2)[i]!=NO_ABS==>(((($toAbs)[i]!=NO_ABS&&($toAbs)[i]==($r2)[i])&&reached(($toAbs)[i],($Time)))&&ObjInv(i,($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice)))&&!IsFwdPtr(($gcMem)[i+4]))))&&MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi)
}
#line 113
function __NucleusInv($toAbs:[int]int,$AbsMem:[int][int]int,CurrentStack:int,$gcSlice:[int]int,BF:int,BT:int,HeapLo:int,Fi:int,Fk:int,Fl:int,Ti:int,Tj:int,Tk:int,Tl:int,$Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int,$FrameCounts:[int]int,$FrameAddrs:[int][int]int,$FrameLayouts:[int][int]$FrameLayout,$FrameSlices:[int][int]int,$FrameAbss:[int][int][int]int,$FrameOffsets:[int][int]int)
  returns(bool)
{
  (((((((((((((((((((((((((((((((((((?gcLo<=HeapLo&&HeapLo<=?gcHi)&&(((Fi==HeapLo&&Ti==Fl)&&BF==?gcLo)||((Ti==HeapLo&&Fi==Tl)&&BT==?gcLo)))&&(Fi==HeapLo==>?gcLo+BitIndex(HeapLo,Fl)==BT&&?gcLo+BitIndex(HeapLo,Tl)==HeapLo))&&(Ti==HeapLo==>?gcLo+BitIndex(HeapLo,Tl)==BF&&?gcLo+BitIndex(HeapLo,Fl)==HeapLo))&&Fi<=Fk)&&Fk<=Fl)&&Fl<=?gcHi)&&Ti<=Tj)&&Tj<=Tk)&&Tk<=Tl)&&Tl<=?gcHi)&&Aligned(Fi))&&Aligned(Fk))&&Aligned(Ti))&&Aligned(Tj))&&Aligned(Tk))&&Aligned(?gcLo+BitIndex(HeapLo,Fi)))&&Aligned(?gcLo+BitIndex(HeapLo,Ti)))&&?gcLo<=?gcLo+BitIndex(HeapLo,Fi))&&?gcLo+BitIndex(HeapLo,Fl)<=HeapLo)&&?gcLo<=?gcLo+BitIndex(HeapLo,Ti))&&?gcLo+BitIndex(HeapLo,Tl)<=HeapLo)&&BF==?gcLo+BitIndex(HeapLo,Fi))&&BT==?gcLo+BitIndex(HeapLo,Ti))&&Fl-Fi==Mult(32,BitIndex(HeapLo,Fl)-BitIndex(HeapLo,Fi)))&&Tl-Ti==Mult(32,BitIndex(HeapLo,Tl)-BitIndex(HeapLo,Ti)))&&bbvec4(($toAbs),NO_ABS,Fi,($gcMem),Fi,Fi,Fl,?gcLo+BitIndex(HeapLo,Fi),?gcLo+BitIndex(HeapLo,Fl)))&&WellFormed(($toAbs)))&&ObjectSpc(Fi,Fk,($toAbs)))&&EmptySeq(Fk,Fl,($toAbs),($toAbs)))&&EmptySeq(Ti,Tl,($toAbs),($toAbs)))&&(forall i:int::{TV(i)}TV(i)==>(gcAddr(i)==>(($toAbs)[i]!=NO_ABS==>Fi<=i&&i<Fk))))&&(forall i:int::{TV(i)}TV(i)==>((Fi<=i&&i<Fk)&&($toAbs)[i]!=NO_ABS==>(($toAbs)[i]!=NO_ABS&&ObjInv(i,($toAbs),($toAbs),($toAbs),($AbsMem),($gcMem),($gcSlice)))&&!IsFwdPtr(($gcMem)[i+4]))))&&(forall i:int::{TV(i)}TV(i)==>((Fi<=i&&i<Fl)&&($toAbs)[i]!=NO_ABS==>($toAbs)[i]!=NO_ABS)))&&(forall i:int::{TV(i)}TV(i)==>((Ti<=i&&i<Tl)&&($toAbs)[i]!=NO_ABS==>false)))&&MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi)
}
#line 146
function _StateInv($s:int,$state:StackState,$toAbs:[int]int,$Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int,$FrameCounts:[int]int,$FrameAddrs:[int][int]int,$FrameLayouts:[int][int]$FrameLayout,$FrameSlices:[int][int]int,$FrameAbss:[int][int][int]int,$FrameOffsets:[int][int]int)
  returns(bool)
{
  ((IsStackStateTagFor(StackTag(($s),($tMems)),($state))&&(!IsEmpty(($state))==>MutatorStackInvS(($s),($toAbs),($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($fMems))))&&(IsYielded(($state))==>((fAddr(($s),StackEsp(($s),($tMems)))&&Aligned(StackEsp(($s),($tMems))))&&($state)==StackYielded(StackEbp(($s),($tMems)),StackEsp(($s),($tMems))+4,StackRA(($s),($tMems),($fMems))))&&ScanStackInv(($s),($fMems)[($s)],($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),StackRA(($s),($tMems),($fMems)),StackEsp(($s),($tMems)),StackEbp(($s),($tMems)))))&&(IsInterrupted(($state))==>((fAddr(($s),StackEsp(($s),($tMems)))&&fAddr(($s),StackEsp(($s),($tMems))+8))&&Aligned(StackEsp(($s),($tMems))))&&($state)==StackInterrupted(StackEax(($s),($tMems)),StackEbx(($s),($tMems)),StackEcx(($s),($tMems)),StackEdx(($s),($tMems)),StackEsi(($s),($tMems)),StackEdi(($s),($tMems)),StackEbp(($s),($tMems)),StackEsp(($s),($tMems))+12,StackRA(($s),($tMems),($fMems)),StackCS(($s),($tMems),($fMems)),StackEfl(($s),($tMems),($fMems))))
}
#line 165
function _NucleusInv($S:int,$stackState:[int]StackState,$toAbs:[int]int,$AbsMem:[int][int]int,CurrentStack:int,$gcSlice:[int]int,BF:int,BT:int,HeapLo:int,Fi:int,Fk:int,Fl:int,Ti:int,Tj:int,Tk:int,Tl:int,$Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int,$FrameCounts:[int]int,$FrameAddrs:[int][int]int,$FrameLayouts:[int][int]$FrameLayout,$FrameSlices:[int][int]int,$FrameAbss:[int][int][int]int,$FrameOffsets:[int][int]int,$IoMmuEnabled:bool,$PciConfigState:[int]int,DmaAddr:int)
  returns(bool)
{
  ((__NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets))&&CurrentStack==($S))&&(forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))==>_StateInv(($s),($stackState)[($s)],($toAbs),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets)))))&&IoInv(($IoMmuEnabled),($PciConfigState),DmaAddr,($pciMem))
}



axiom (forall $S:int,$stackState:[int]StackState,$toAbs:[int]int,$AbsMem:[int][int]int,CurrentStack:int,$gcSlice:[int]int,BF:int,BT:int,HeapLo:int,Fi:int,Fk:int,Fl:int,Ti:int,Tj:int,Tk:int,Tl:int,$Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int,$FrameCounts:[int]int,$FrameAddrs:[int][int]int,$FrameLayouts:[int][int]$FrameLayout,$FrameSlices:[int][int]int,$FrameAbss:[int][int][int]int,$FrameOffsets:[int][int]int,$IoMmuEnabled:bool,$PciConfigState:[int]int,DmaAddr:int::NucleusInv(($S),($stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr)==_NucleusInv(($S),($stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr));



procedure BB4Zero($a:[int]int,$on:int,$off:int,$aBase:int,$i0:int,$i1:int,$i2:int,$k:int,$g1:int,$g2:int,$_i0:int,$_g1:int)
#line 177

requires eax==($g1);
requires ebx==($g2);
requires (forall i:int::{TV(i)}(TV(i)&&($i1)<=i)&&i<($i2)==>($a)[($aBase)+(i-($i0))]==($off));
requires Aligned(($g1))&&Aligned(($g2));
requires ($i2)-($i1)==32*(($g2)-($g1));
requires word(($i1)-($i0))&&word(($i2)-($i0));
requires (?gcLo<=($g1)&&($g1)<=($g2))&&($g2)<=?gcHi;
requires ($i1)==($i0);
requires SMemRequireRA(100,($sMem),esp,($RET));
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
modifies $gcMem;
modifies $Eip,eax,ebx,esi,edi,ebp,esp,$Mem,$sMem;
ensures bbvec4(($a),($off),($aBase),($gcMem),($i0),($i1),($i2),($g1),($g2));
ensures (forall i:int::{TV(i)}TV(i)&&!between(($g1),($g2),i)==>($gcMem)[i]==old($gcMem)[i]);
ensures (forall i:int::{TV(i)}TV(i)&&!between(($g1),($g2),i+4)==>($gcMem)[i+4]==old($gcMem)[i+4]);
ensures (forall i:int,j:int::{TV(i),TO(j)}(TV(i)&&TO(j))&&!between(($g1),($g2),i+4*j)==>($gcMem)[i+4*j]==old($gcMem)[i+4*j]);
ensures (forall i:int::{TV(i)}TV(i)&&!between(($g1),($g2),($_g1)+BitIndex(($_i0),i))==>($gcMem)[($_g1)+BitIndex(($_i0),i)]==old($gcMem)[($_g1)+BitIndex(($_i0),i)]);
ensures SMemEnsure(($sMem),old($sMem),esp,old(esp));
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
{
#line 177
  var $iter:int;
#line 199
  call esi:=Mov(eax);
  $iter:=($i1);
  
  loop:
  assert Aligned(esi)&&TV(esi);
  assert ($g1)<=esi&&esi<=($g2);
  assert ($iter)-($i1)==32*(esi-($g1));
  assert bbvec4(($a),($off),($aBase),($gcMem),($i0),($i1),($iter),($g1),($g2));
  assert (forall i:int::{TV(i)}TV(i)&&!between(($g1),($g2),i)==>($gcMem)[i]==old($gcMem)[i]);
  assert (forall i:int::{TV(i)}TV(i)&&!between(($g1),($g2),i+4)==>($gcMem)[i+4]==old($gcMem)[i+4]);
  assert (forall i:int,j:int::{TV(i),TO(j)}(TV(i)&&TO(j))&&!between(($g1),($g2),i+4*j)==>($gcMem)[i+4*j]==old($gcMem)[i+4*j]);
  assert (forall i:int::{TV(i)}TV(i)&&!between(($g1),($g2),($_g1)+BitIndex(($_i0),i))==>($gcMem)[($_g1)+BitIndex(($_i0),i)]==old($gcMem)[($_g1)+BitIndex(($_i0),i)]);
  assert MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  havoc $Eip;
#line 212
  if(esi>=ebx)
  {
#line 212
    goto loopEnd;
  }
  call __notAligned(esi);
  call __bb4Zero(($a),($off),($aBase),($gcMem),($i0),($i1),($iter),($g1),($g2),esi);
  call gcStore(esi,0);
#line 216
  call Store(esi,0);
  $iter:=($iter)+128;
  call esi:=Add(esi,4);
  assert TO(1);
  havoc $Eip;
#line 220
  goto loop;
  loopEnd:
  
  assert esi==($g2);
  assert ($iter)==($i2);
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 225
  call Ret(old($RET));
#line 225
  return;
}

procedure bb4GetBit($a:[int]int,$off:int,$aBase:int,$i0:int,$i1:int,$i2:int,$k:int,$g1:int,$g2:int)
#line 228

requires bbvec4(($a),($off),($aBase),($gcMem),($i0),($i1),($i2),($g1),($g2));
requires (word(($k)-($i0))&&($i1)<=($k))&&($k)<($i2);
requires ((word(($k))&&word(($i0)))&&Aligned(($k)))&&Aligned(($i0));
requires word(($i2)-($i0));
requires eax==($g1);
requires ebx==($k)-($i0);
requires (Aligned(($g1))&&?gcLo<=($g1))&&($g2)<=?gcHi;
requires word(($i1)-($i0))&&word(($i2)-($i0));
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
modifies $Eip,ebx,ecx,edx;
ensures ebx==0<==>($a)[($aBase)+(($k)-($i0))]==($off);
{
#line 228
  var $bbb:int;
#line 228
  var $idx:int;
#line 243
  $idx:=($g1)+4*shr(($k)-($i0),7);
  $bbb:=and(($gcMem)[($idx)],shl(1,and(shr(($k)-($i0),2),31)));
  call __subAligned(($k),($i0));
  call __bb4GetBit(($a),($off),($aBase),($gcMem),($i0),($i1),($i2),($k),($idx),($bbb),($g1),($g2));
  
  call edx:=Mov(ebx);
  assert TV(($g1));
  assert TO(shr(ebx,7));
  call ebx:=Shr(ebx,7);
  call edx:=Shr(edx,2);
  call ebx:=Add(ebx,ebx);
  call ebx:=Add(ebx,ebx);
  call ebx:=Add(ebx,eax);
  call gcLoad(ebx);
#line 256
  call ebx:=Load(ebx);
  call edx:=And(edx,31);
  call ecx:=Mov(edx);
  call edx:=Mov(1);
  call edx:=Shl(edx,ecx);
  call ebx:=And(ebx,edx);
}

procedure bb4SetBit($a:[int]int,$on:int,$off:int,$aBase:int,$i0:int,$i1:int,$i2:int,$k:int,$g1:int,$g2:int)
#line 264

requires bbvec4(($a),($off),($aBase),($gcMem),($i0),($i1),($i2),($g1),($g2));
requires (word(($k)-($i0))&&($i1)<=($k))&&($k)<($i2);
requires ((word(($k))&&word(($i0)))&&Aligned(($k)))&&Aligned(($i0));
requires ($on)!=($off);
requires word(($i2)-($i0));
requires esi==($k)-($i0);
requires edi==($g1);
requires (Aligned(($g1))&&?gcLo<=($g1))&&($g2)<=?gcHi;
requires word(($i1)-($i0))&&word(($i2)-($i0));
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
modifies $Eip,esi,edi,ecx,$gcMem,$Mem;
ensures bbvec4(($a)[(($aBase)+(($k)-($i0))):=($on)],($off),($aBase),($gcMem),($i0),($i1),($i2),($g1),($g2));
ensures ($gcMem)==old($gcMem)[(($g1)+BitIndex(($i0),($k))):=edi];
ensures Aligned(($k)-($i0));
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
{
#line 264
  var $bbb:int;
#line 264
  var $idx:int;
#line 283
  $idx:=($g1)+4*shr(($k)-($i0),7);
  $bbb:=or(($gcMem)[($idx)],shl(1,and(shr(($k)-($i0),2),31)));
  call __subAligned(($k),($i0));
  call __bb4SetBit(($a),($on),($off),($aBase),($gcMem),($i0),($i1),($i2),($k),($idx),($bbb),($gcMem)[($idx):=($bbb)],($g1),($g2));
  
  call ecx:=Mov(esi);
  assert TV(($g1));
  assert TO(shr(esi,7));
  call esi:=Shr(esi,7);
  call ecx:=Shr(ecx,2);
  call esi:=Add(esi,esi);
  call esi:=Add(esi,esi);
  call esi:=Add(esi,edi);
  call ecx:=And(ecx,31);
  call edi:=Mov(1);
  call edi:=Shl(edi,ecx);
  call ecx:=Mov(edi);
  call gcLoad(esi);
#line 300
  call edi:=Load(esi);
  call edi:=Or(edi,ecx);
  call gcStore(esi,edi);
#line 302
  call Store(esi,edi);
}

procedure copyWord($ptr:int,$_tj:int,$ret:int,$ind:int,$s:int)
#line 305

requires (ecx==($ptr)&&esi==($ret))&&edi==($ind);
requires Pointer(($r1),($ptr),($r1)[($ptr)])&&TV(($ptr));
requires !IsFwdPtr(($gcMem)[($ptr)+4]);
requires ($_tj)<=Tk;

requires ($s)==4*numFields(($r1)[($ptr)]);
requires Tk==($ret)+($s);
requires Tk<=Tl;

requires (TO(($ind))&&0<=($ind))&&($ind)<numFields(($r1)[($ptr)]);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,($_tj),($ret),Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>gcAddr(($ret)+4*j)&&($gcSlice)[($ret)+4*j]==($ret)));
requires EmptySeq(($ret)+4*($ind),Tk,($r2),($toAbs));
requires (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>Value(VFieldPtr(($r1)[($ptr)],j),($r1),($gcMem)[($ret)+4*j],(($AbsMem)[($toAbs)[($ptr)]])[j])));

requires (forall i:int::{TV(i)}TV(i)==>(($ret)<i&&i<($ret)+4*($ind)==>($r2)[i]==NO_ABS));
requires ($r2)[($ret)]==NO_ABS;

modifies $gcMem,$Mem,$gcSlice;
modifies $Eip,eax,ebx;

ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,($_tj),($ret),Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)+1==>gcAddr(($ret)+4*j)&&($gcSlice)[($ret)+4*j]==($ret)));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)+1==>Value(VFieldPtr(($r1)[($ptr)],j),($r1),($gcMem)[($ret)+4*j],(($AbsMem)[($toAbs)[($ptr)]])[j])));

ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&Tj+4*j<($_tj)==>($gcMem)[Tj+4*j]==old($gcMem)[Tj+4*j]&&($gcSlice)[Tj+4*j]==old(($gcSlice)[Tj+4*j])));


ensures RExtend(old($r2),($r2));
{
  assert TO(numFields(($r1)[($ptr)]));
  assert TV(($ret));
  
  call gcLoad(ecx+4*edi);
#line 339
  call eax:=Load(ecx+4*edi);
  assert TV(($ret)+4*($ind));
  $gcSlice:=($gcSlice)[(($ret)+4*($ind)):=($ret)];
  call gcStore(esi+4*edi,eax);
#line 342
  call Store(esi+4*edi,eax);
  assert TO(1);
}

procedure CopyAndForward($ptr:int,$_tj:int)
#line 346

requires ecx==($ptr);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,($_tj),Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires Pointer(($r1),($ptr),($r1)[($ptr)])&&TV(($ptr));
requires !IsFwdPtr(($gcMem)[($ptr)+4]);
requires ($_tj)<=Tk;
requires reached(($toAbs)[($ptr)],($Time));
requires SMemRequireRA(104,($sMem),esp,($RET));
modifies $r2,$gcMem,$toAbs,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,($_tj),Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures Pointer(($r2),eax,($r1)[($ptr)]);
ensures Tj==old(Tj);
ensures Tk>=old(Tk);
ensures old($toAbs)[Tj]!=NO_ABS==>($toAbs)[Tj]!=NO_ABS&&($toAbs)[Tj]==old($toAbs)[Tj];
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&Tj+4*j<($_tj)==>($gcMem)[Tj+4*j]==old($gcMem)[Tj+4*j]&&($gcSlice)[Tj+4*j]==old(($gcSlice)[Tj+4*j])));

ensures (Ti<=eax&&eax<Tk)&&gcAddrEx(eax+4);
ensures SMemEnsure(($sMem),old($sMem),esp,old(esp));
{
#line 346
  var tmp:int;
#line 369
  call gcLoad(ecx+4);
#line 369
  call edx:=Load(ecx+4);
  call _call();
#line 370
  call Call();
#line 370
  call GetSize(($ptr),edx,($r1),($r1));
  call ebp:=Mov(eax);
  assert TO(numFields(($r1)[($ptr)]));
  
  call esi:=Mov(Tk);
  call Tk:=AddChecked(Tk,ebp);
  assert TV(esi);
  
  call eax:=Mov(Tl);
  havoc $Eip;
#line 379
  if(Tk<=eax)
  {
#line 379
    goto skip1;
  }
  call eax:=Mov(1431633969);
  call _call();
#line 382
  call Call();
#line 382
  call DebugBreak();
  skip1:
  
  call edi:=Mov(0);
  call edx:=Mov(0);
  
  loop:
  assert 4*edi==edx;
  assert (TO(edi)&&0<=edi)&&edi<=numFields(($r1)[($ptr)]);
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,($_tj),esi,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<edi==>gcAddr(esi+4*j)&&($gcSlice)[esi+4*j]==esi));
  assert EmptySeq(esi+4*edi,Tk,($r2),($toAbs));
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<edi==>Value(VFieldPtr(($r1)[($ptr)],j),($r1),($gcMem)[esi+4*j],(($AbsMem)[($toAbs)[($ptr)]])[j])));
  
  assert (forall i:int::{TV(i)}TV(i)==>(esi<i&&i<esi+4*edi==>($r2)[i]==NO_ABS));
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&Tj+4*j<($_tj)==>($gcMem)[Tj+4*j]==old($gcMem)[Tj+4*j]&&($gcSlice)[Tj+4*j]==old(($gcSlice)[Tj+4*j])));
  
  assert ($r2)[esi]==NO_ABS;
  havoc $Eip;
#line 401
  if(edx>=ebp)
  {
#line 401
    goto loopEnd;
  }
  call copyWord(($ptr),($_tj),esi,edi,ebp);
  call edi:=Add(edi,1);
  call edx:=Add(edx,4);
  havoc $Eip;
#line 406
  goto loop;
  loopEnd:
  
  call eax:=Lea(esi+4);
  call gcStore(ecx+4,eax);
#line 410
  call Store(ecx+4,eax);
  
  call eax:=Mov(esi);
  
  call esi:=Sub(esi,Ti);
  
  call edi:=Mov(BT);
  call bb4SetBit(($toAbs),($r1)[($ptr)],NO_ABS,Ti,Ti,Ti,Tl,eax,?gcLo+BitIndex(HeapLo,Ti),?gcLo+BitIndex(HeapLo,Tl));
  
  $r2:=($r2)[eax:=(($r1)[($ptr)])];
  $toAbs:=($toAbs)[eax:=(($toAbs)[($ptr)])];
  $toAbs:=($toAbs)[($ptr):=NO_ABS];
  assert TO(1);
  
  assert TV(eax-Ti);
  
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 426
  call Ret(old($RET));
#line 426
  return;
}

procedure forwardFromspacePtr($ptr:int,$abs:int,$_tj:int)
#line 429

requires ecx==($ptr);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,($_tj),Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires word(($ptr));
requires Pointer(($r1),($ptr)-4,($abs));
requires ($_tj)<=Tk;
requires IsFwdPtr(($gcMem)[($ptr)])||reached(($toAbs)[($ptr)-4],($Time));
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,($_tj),Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures Pointer(($r2),eax-4,($abs));
ensures Tj==old(Tj);
ensures Tk>=old(Tk);
ensures old($toAbs)[Tj]!=NO_ABS==>($toAbs)[Tj]!=NO_ABS&&($toAbs)[Tj]==old($toAbs)[Tj];
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&Tj+4*j<($_tj)==>($gcMem)[Tj+4*j]==old($gcMem)[Tj+4*j]&&($gcSlice)[Tj+4*j]==old(($gcSlice)[Tj+4*j])));

ensures Ti<=eax-4&&eax-4<Tk;
ensures gcAddrEx(eax);
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
  assert TV(($ptr)-4);
  call gcLoad(ecx);
#line 452
  call eax:=Load(ecx);
  
  havoc $Eip;
#line 454
  if(GcLo>eax)
  {
#line 454
    goto skip;
  }
#line 455
  havoc $Eip;
#line 455
  if(GcHi>=eax)
  {
#line 455
    goto done;
  }
#line 456
  skip:
  call ecx:=Sub(ecx,4);
  call _call();
#line 458
  call Call();
#line 458
  call CopyAndForward(($ptr)-4,($_tj));
  call eax:=Add(eax,4);
  done:
  assert TV(eax-4);
}

procedure scanObjectDense($vt:int)
#line 464

requires ecx==($vt);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires Tj<Tk;
requires TV(Tj);
requires ($vt)==(($AbsMem)[($r2)[Tj]])[1];
requires tag(($vt))==?DENSE_TAG;
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tj,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 464
  var save3:int;
#line 464
  var save2:int;
#line 464
  var save1:int;
#line 481
  assert TO(numFields(($r2)[Tj]));
  
  call esi:=Mov(2);
  assert TVT(($r2)[Tj],($vt));
  assert TVL(($r2)[Tj]);
  call ebp:=RoLoad32(ecx+?VT_BASE_LENGTH);
  call edx:=RoLoad32(ecx+?VT_MASK);
  call edi:=Mov(Tj);
  call edi:=Add(edi,8);
  call ebp:=Add(ebp,Tj);
  
  
  loop:
  assert TO(esi);
  assert (edi==Tj+4*esi&&ebp==Tj+4*numFields(($r2)[Tj]))&&edx==mask(($vt));
  assert 2<=esi&&esi<=numFields(($r2)[Tj]);
  assert Pointer(($r2),Tj,($r2)[Tj]);
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj+4*numFields(($r2)[Tj]),Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert ObjInvPartial(Tj,0,esi,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ObjInvPartial(Tj,esi,numFields(($r2)[Tj]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ($toAbs)[Tj]!=NO_ABS&&($toAbs)[Tj]==($r2)[Tj];
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 504
  if(edi>=ebp)
  {
#line 504
    goto loopEnd;
  }
#line 505
  havoc $Eip;
#line 505
  if(esi>=30)
  {
#line 505
    goto loopEnd;
  }
  assert TO(0)&&TO(1);
  assert TVT(($r2)[Tj],($vt));
  assert TVL(($r2)[Tj]);
  
  
  call ecx:=Lea(esi+2);
  call ebx:=Mov(edx);
  call ebx:=Shr(ebx,ecx);
  call ebx:=And(ebx,1);
  havoc $Eip;
#line 516
  if(ebx!=1)
  {
#line 516
    goto skip1;
  }
#line 517
  call gcLoad(edi);
#line 517
  call ecx:=Load(edi);
  
  
  havoc $Eip;
#line 520
  if(ecx<GcLo)
  {
#line 520
    goto skip2;
  }
#line 521
  havoc $Eip;
#line 521
  if(ecx>GcHi)
  {
#line 521
    goto skip2;
  }
#line 522
  assert TV(ecx-4);
  call reach(($toAbs)[Tj],esi,($Time));
  
  call save1:=Mov(esi);
  call save2:=Mov(ebp);
  call save3:=Mov(edx);
  call forwardFromspacePtr(ecx,(($AbsMem)[($toAbs)[Tj]])[esi],Tj+4*numFields(($r2)[Tj]));
  call esi:=Mov(save1);
  call ebp:=Mov(save2);
  call edx:=Mov(save3);
  call edi:=Mov(Tj);
  call edi:=Lea(edi+4*esi);
  
  call gcStore(edi,eax);
#line 535
  call Store(edi,eax);
  skip2:
  skip1:
  
  call esi:=Add(esi,1);
  call edi:=Add(edi,4);
  havoc $Eip;
#line 541
  goto loop;
  loopEnd:
  
  assert TVT(($r2)[Tj],($vt));
  assert TVL(($r2)[Tj]);
  call Tj:=Mov(ebp);
  assert TO(1);
}

procedure scanObjectSparse($vt:int)
#line 550

requires ecx==($vt);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires Tj<Tk;
requires TV(Tj);
requires ($vt)==(($AbsMem)[($r2)[Tj]])[1];
requires tag(($vt))==?SPARSE_TAG;
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tj,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 550
  var save4:int;
#line 550
  var save3:int;
#line 550
  var save2:int;
#line 550
  var save1:int;
#line 568
  assert TO(numFields(($r2)[Tj]));
  
  call esi:=Mov(1);
  
  assert TO(numFields(($r2)[Tj]));
  assert TVT(($toAbs)[Tj],($vt));
  call ebp:=RoLoad32(ecx+?VT_BASE_LENGTH);
  call edx:=RoLoad32(ecx+?VT_MASK);
  assert TVL(($r2)[Tj]);
  
  call esi:=Mov(1);
  
  loop:
  assert edx==mask(($vt))&&ebp==4*numFields(($r2)[Tj]);
  assert (TSlot(esi)&&0<esi)&&esi<=8;
  assert Pointer(($r2),Tj,($r2)[Tj]);
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj+4*numFields(($r2)[Tj]),Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert ObjInvBase(Tj,($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($r2)[Tj])==>(between(1,esi,fieldToSlot(($vt),j-2))==>ObjInvField(Tj,j,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice)))));
  
  
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($r2)[Tj])==>(!between(1,esi,fieldToSlot(($vt),j-2))==>ObjInvField(Tj,j,($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice)))));
  
  
  assert ($toAbs)[Tj]!=NO_ABS&&($toAbs)[Tj]==($r2)[Tj];
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 595
  if(esi>=8)
  {
#line 595
    goto loopEnd;
  }
  assert TO(0)&&TO(1);
  assert TVT(($toAbs)[Tj],($vt));
  assert TO(getNib(edx,4*esi)+1);
  
  
  call ecx:=Mov(0);
  call ecx:=Lea(ecx+4*esi);
  call ebx:=Mov(edx);
  call ebx:=Shr(ebx,ecx);
  call ebx:=And(ebx,15);
  havoc $Eip;
#line 607
  if(ebx==0)
  {
#line 607
    goto skip1;
  }
#line 608
  call eax:=Mov(Tj);
  call gcLoad(eax+4*ebx+4);
#line 609
  call ecx:=Load(eax+4*ebx+4);
  
  
  havoc $Eip;
#line 612
  if(ecx<GcLo)
  {
#line 612
    goto skip2;
  }
#line 613
  havoc $Eip;
#line 613
  if(ecx>GcHi)
  {
#line 613
    goto skip2;
  }
#line 614
  assert TV(ecx-4);
  call reach(($toAbs)[Tj],1+ebx,($Time));
  
  call save1:=Mov(esi);
  call save2:=Mov(ebp);
  call save3:=Mov(edx);
  call save4:=Mov(ebx);
  call forwardFromspacePtr(ecx,(($AbsMem)[($toAbs)[Tj]])[1+getNib(edx,4*esi)],Tj+ebp);
  call esi:=Mov(save1);
  call ebp:=Mov(save2);
  call edx:=Mov(save3);
  call ebx:=Mov(save4);
  
  call ecx:=Mov(Tj);
  call gcStore(ecx+4*ebx+4,eax);
#line 628
  call Store(ecx+4*ebx+4,eax);
  skip2:
  skip1:
  
  call esi:=Add(esi,1);
  havoc $Eip;
#line 633
  goto loop;
  loopEnd:
  
  assert TVT(($toAbs)[Tj],($vt));
  assert TVL(($r2)[Tj]);
  call Tj:=Add(Tj,ebp);
  assert TO(1);
}

procedure scanObjectOtherVectorNoPointers($vt:int)
#line 642

requires ecx==($vt);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires Tj<Tk;
requires TV(Tj);
requires ($vt)==(($AbsMem)[($r2)[Tj]])[1];
requires tag(($vt))==?OTHER_VECTOR_TAG;
requires arrayOf(($vt))!=?TYPE_STRUCT||(arrayOf(($vt))==?TYPE_STRUCT&&mask(arrayElementClass(($vt)))==?SPARSE_TAG);
requires SMemRequire(104,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tj,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
  call edx:=Mov(ecx);
  call ecx:=Mov(Tj);
  call _call();
#line 659
  call Call();
#line 659
  call GetSize(Tj,($vt),($r2),($r1));
  
  assert TO(numFields(($r2)[Tj]));
  assert TVT(($r2)[Tj],($vt));
  call Tj:=Add(Tj,eax);
}

procedure scanObjectOtherVectorPointersSparseFields($vt:int,$jj:int,$index:int)
#line 666

requires edx==mask(arrayElementClass(($vt)));
requires edi==($jj);
requires Tj<Tk;
requires TV(Tj);
requires ($vt)==(($AbsMem)[($r2)[Tj]])[1];
requires tag(($vt))==?OTHER_VECTOR_TAG;
requires arrayOf(($vt))==?TYPE_STRUCT&&tag(arrayElementClass(($vt)))==?SPARSE_TAG;
requires TO(($jj))&&($jj)==baseWords(($vt))+Mult(arrayElementWords(($vt)),($index));
requires ($jj)<numFields(($r2)[Tj]);
requires TO(($index))&&0<=($index);
requires Mult(arrayElementWords(($vt)),($index))>=0;
requires reached(($toAbs)[Tj],($Time));
requires Pointer(($r2),Tj,($r2)[Tj]);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj+4*numFields(($r2)[Tj]),Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires ObjInvPartial(Tj,0,($jj),($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
requires ObjInvPartial(Tj,($jj),numFields(($r2)[Tj]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
requires ($toAbs)[Tj]!=NO_ABS&&($toAbs)[Tj]==($r2)[Tj];
requires (forall j:int::{TO(j)}TO(j)==>(between(0,arrayElementWords(($vt)),j-($jj))==>ObjInvField(Tj,j,($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice))));


requires SMemRequire(108,($sMem),esp);

modifies $r2,$gcMem,$toAbs,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;

ensures Pointer(($r2),Tj,($r2)[Tj]);
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj+4*numFields(($r2)[Tj]),Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures ObjInvPartial(Tj,0,($jj),($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
ensures ObjInvPartial(Tj,($jj)+arrayElementWords(($vt)),numFields(($r2)[Tj]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
ensures ($toAbs)[Tj]!=NO_ABS&&($toAbs)[Tj]==($r2)[Tj];
ensures (forall j:int::{TO(j)}TO(j)==>(between(0,arrayElementWords(($vt)),j-($jj))==>(between(1,8,fieldToSlot(arrayElementClass(($vt)),j-($jj)))==>ObjInvField(Tj,j,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice)))));



ensures (forall j:int::{TO(j)}TO(j)==>(between(0,arrayElementWords(($vt)),j-($jj))==>(!between(1,8,fieldToSlot(arrayElementClass(($vt)),j-($jj)))==>ObjInvField(Tj,j,($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice)))));



ensures edi==old(edi);
ensures edx==old(edx);
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 666
  var save4:int;
#line 666
  var save3:int;
#line 666
  var save2:int;
#line 666
  var save1:int;
#line 715
  assert TO(numFields(($r2)[Tj]));
  assert TO(2);
  assert TVT(($r2)[Tj],($vt));
  assert TVL(($r2)[Tj]);
  
  call esi:=Mov(1);
  
  loop:
  assert (TSlot(esi)&&0<esi)&&esi<=8;
  assert edx==mask(arrayElementClass(($vt)));
  assert edi==($jj);
  assert Pointer(($r2),Tj,($r2)[Tj]);
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj+4*numFields(($r2)[Tj]),Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert ObjInvPartial(Tj,0,($jj),($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ObjInvPartial(Tj,($jj)+arrayElementWords(($vt)),numFields(($r2)[Tj]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ($toAbs)[Tj]!=NO_ABS&&($toAbs)[Tj]==($r2)[Tj];
  assert (forall j:int::{TO(j)}TO(j)==>(between(0,arrayElementWords(($vt)),j-($jj))==>(between(1,esi,fieldToSlot(arrayElementClass(($vt)),j-($jj)))==>ObjInvField(Tj,j,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice)))));
  
  
  
  assert (forall j:int::{TO(j)}TO(j)==>(between(0,arrayElementWords(($vt)),j-($jj))==>(!between(1,esi,fieldToSlot(arrayElementClass(($vt)),j-($jj)))==>ObjInvField(Tj,j,($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice)))));
  
  
  
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 741
  if(esi>=8)
  {
#line 741
    goto loopEnd;
  }
  
  call ecx:=Mov(0);
  call ecx:=Lea(ecx+4*esi);
  call ebx:=Mov(edx);
  call ebx:=Shr(ebx,ecx);
  call ebx:=And(ebx,15);
  assert ebx==getNib(mask(arrayElementClass(($vt))),4*esi);
  
  
  havoc $Eip;
#line 752
  if(ebx==0)
  {
#line 752
    goto skip1;
  }
#line 753
  call ebx:=Sub(ebx,1);
  call ebx:=Add(ebx,edi);
  assert TO(ebx);
  
  call eax:=Mov(Tj);
  call gcLoad(eax+4*ebx);
#line 758
  call ecx:=Load(eax+4*ebx);
  
  havoc $Eip;
#line 760
  if(ecx<GcLo)
  {
#line 760
    goto skip2;
  }
#line 761
  havoc $Eip;
#line 761
  if(ecx>GcHi)
  {
#line 761
    goto skip2;
  }
  assert TV(ecx-4);
  call reach(($toAbs)[Tj],ebx,($Time));
  
  assert TO(0);
  assert TO(1);
  
  call save1:=Mov(esi);
  call save2:=Mov(edi);
  call save3:=Mov(edx);
  call save4:=Mov(ebx);
  call forwardFromspacePtr(ecx,(($AbsMem)[($toAbs)[Tj]])[ebx],Tj+4*numFields(($r2)[Tj]));
  call esi:=Mov(save1);
  call edi:=Mov(save2);
  call edx:=Mov(save3);
  call ebx:=Mov(save4);
  
  call ecx:=Mov(Tj);
  call gcStore(ecx+4*ebx,eax);
#line 780
  call Store(ecx+4*ebx,eax);
  
  skip2:
  skip1:
  call esi:=Add(esi,1);
  havoc $Eip;
#line 785
  goto loop;
  loopEnd:
}

procedure scanObjectOtherVectorPointers($vt:int)
#line 789

requires ecx==($vt);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires Tj<Tk;
requires TV(Tj);
requires ($vt)==(($AbsMem)[($r2)[Tj]])[1];
requires tag(($vt))==?OTHER_VECTOR_TAG;
requires arrayOf(($vt))==?TYPE_STRUCT&&tag(arrayElementClass(($vt)))==?SPARSE_TAG;
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tj,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 789
  var $index:int;
#line 789
  var save2:int;
#line 789
  var save1:int;
#line 809
  assert TO(numFields(($r2)[Tj]));
  assert TO(2);
  assert TVT(($r2)[Tj],($vt));
  assert TVL(($r2)[Tj]);
  
  $index:=0;
  
  call edi:=RoLoad32(ecx+?VT_BASE_LENGTH);
  call ebx:=RoLoad32(ecx+?VT_ARRAY_ELEMENT_SIZE);
  call edx:=Mov(Tj);
  call gcLoad(edx+8);
#line 819
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
  
  assert ebp==4*numFields(($r2)[Tj]);
  assert edx==mask(arrayElementClass(($vt)));
  assert ebx==arrayElementWords(($vt));
  assert 4*edi<=ebp;
  
  assert Pointer(($r2),Tj,($r2)[Tj]);
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj+4*numFields(($r2)[Tj]),Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert ObjInvPartial(Tj,0,edi,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ObjInvPartial(Tj,edi,numFields(($r2)[Tj]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ($toAbs)[Tj]!=NO_ABS&&($toAbs)[Tj]==($r2)[Tj];
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  call eax:=Mov(0);
  call eax:=Lea(eax+4*edi);
  havoc $Eip;
#line 857
  if(eax>=ebp)
  {
#line 857
    goto loopEnd;
  }
  call save1:=Mov(ebx);
  call save2:=Mov(ebp);
  call scanObjectOtherVectorPointersSparseFields(($vt),edi,($index));
  call ebx:=Mov(save1);
  call ebp:=Mov(save2);
  
  assert TVM3(ebx,($index),1);
  assert TVM(ebx,($index));
  $index:=($index)+1;
  call edi:=Add(edi,ebx);
  havoc $Eip;
#line 869
  goto loop;
  loopEnd:
  
  call Tj:=Add(Tj,ebp);
  assert TO(1);
}

procedure scanObjectPtrArray($vt:int)
#line 876

requires ecx==($vt);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires Tj<Tk;
requires TV(Tj);
requires ($vt)==(($AbsMem)[($r2)[Tj]])[1];
requires tag(($vt))==?PTR_ARRAY_TAG;
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tj,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 876
  var save2:int;
#line 876
  var save1:int;
#line 876
  var $ind:int;
#line 894
  assert TO(numFields(($r2)[Tj]));
  assert TO(3);
  assert TVT(($r2)[Tj],($vt));
  assert TVL(($r2)[Tj]);
  
  call ebx:=Mov(Tj);
  call gcLoad(ebx+12);
#line 900
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
  assert edi==Tj+4*($ind);
  assert ebp==Tj+4*numFields(($r2)[Tj]);
  assert TO(($ind))&&baseWords(($vt))<=($ind);
  assert Pointer(($r2),Tj,($r2)[Tj]);
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj+4*numFields(($r2)[Tj]),Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert ObjInvPartial(Tj,0,($ind),($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ObjInvPartial(Tj,($ind),numFields(($r2)[Tj]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ($toAbs)[Tj]!=NO_ABS&&($toAbs)[Tj]==($r2)[Tj];
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 928
  if(edi>=ebp)
  {
#line 928
    goto loopEnd;
  }
  assert (TO(0)&&TO(1))&&TO(3);
  assert TVT(($r2)[Tj],($vt));
  assert TVL(($r2)[Tj]);
  call gcLoad(edi);
#line 933
  call ecx:=Load(edi);
  
  havoc $Eip;
#line 935
  if(ecx<GcLo)
  {
#line 935
    goto skip1;
  }
#line 936
  havoc $Eip;
#line 936
  if(ecx>GcHi)
  {
#line 936
    goto skip1;
  }
#line 937
  assert TV(ecx-4);
  call reach(($toAbs)[Tj],($ind),($Time));
  
  call save1:=Mov(edi);
  call save2:=Mov(ebp);
  call forwardFromspacePtr(ecx,(($AbsMem)[($toAbs)[Tj]])[($ind)],ebp);
  call edi:=Mov(save1);
  call ebp:=Mov(save2);
  
  call gcStore(edi,eax);
#line 946
  call Store(edi,eax);
  skip1:
  
  $ind:=($ind)+1;
  call edi:=Add(edi,4);
  havoc $Eip;
#line 951
  goto loop;
  loopEnd:
  
  call Tj:=Mov(ebp);
}

procedure scanObjectPtrVector($vt:int)
#line 957

requires ecx==($vt);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires Tj<Tk;
requires TV(Tj);
requires ($vt)==(($AbsMem)[($r2)[Tj]])[1];
requires tag(($vt))==?PTR_VECTOR_TAG;
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tj,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 957
  var save2:int;
#line 957
  var save1:int;
#line 957
  var $ind:int;
#line 974
  assert TO(numFields(($r2)[Tj]));
  assert TO(2);
  assert TVT(($r2)[Tj],($vt));
  assert TVL(($r2)[Tj]);
  
  call ebx:=Mov(Tj);
  call gcLoad(ebx+8);
#line 980
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
  assert edi==Tj+4*($ind);
  assert ebp==Tj+4*numFields(($r2)[Tj]);
  assert TO(($ind))&&baseWords(($vt))<=($ind);
  assert Pointer(($r2),Tj,($r2)[Tj]);
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj+4*numFields(($r2)[Tj]),Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert ObjInvPartial(Tj,0,($ind),($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ObjInvPartial(Tj,($ind),numFields(($r2)[Tj]),($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert ($toAbs)[Tj]!=NO_ABS&&($toAbs)[Tj]==($r2)[Tj];
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 1008
  if(edi>=ebp)
  {
#line 1008
    goto loopEnd;
  }
  assert (TO(0)&&TO(1))&&TO(2);
  assert TVT(($r2)[Tj],($vt));
  call gcLoad(edi);
#line 1012
  call ecx:=Load(edi);
  
  havoc $Eip;
#line 1014
  if(ecx<GcLo)
  {
#line 1014
    goto skip1;
  }
#line 1015
  havoc $Eip;
#line 1015
  if(ecx>GcHi)
  {
#line 1015
    goto skip1;
  }
#line 1016
  assert TV(ecx-4);
  call reach(($toAbs)[Tj],($ind),($Time));
  
  call save1:=Mov(edi);
  call save2:=Mov(ebp);
  call forwardFromspacePtr(ecx,(($AbsMem)[($toAbs)[Tj]])[($ind)],ebp);
  call edi:=Mov(save1);
  call ebp:=Mov(save2);
  
  call gcStore(edi,eax);
#line 1025
  call Store(edi,eax);
  skip1:
  
  $ind:=($ind)+1;
  call edi:=Add(edi,4);
  havoc $Eip;
#line 1030
  goto loop;
  loopEnd:
  
  call Tj:=Mov(ebp);
}

procedure scanObjectOtherArrayNoPointers($vt:int)
#line 1036

requires ecx==($vt);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires Tj<Tk;
requires TV(Tj);
requires ($vt)==(($AbsMem)[($r2)[Tj]])[1];
requires tag(($vt))==?OTHER_ARRAY_TAG;
requires arrayOf(($vt))!=?TYPE_STRUCT;
requires SMemRequire(104,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tj,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
  call edx:=Mov(ecx);
  call ecx:=Mov(Tj);
  call _call();
#line 1053
  call Call();
#line 1053
  call GetSize(Tj,($vt),($r2),($r1));
  
  assert TO(numFields(($r2)[Tj]));
  assert TVT(($r2)[Tj],($vt));
  call Tj:=Add(Tj,eax);
}

procedure scanObjectString($vt:int)
#line 1060

requires ecx==($vt);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires Tj<Tk;
requires TV(Tj);
requires ($vt)==(($AbsMem)[($r2)[Tj]])[1];
requires tag(($vt))==?STRING_TAG;
requires SMemRequire(104,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tj,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
  call edx:=Mov(ecx);
  call ecx:=Mov(Tj);
  call _call();
#line 1076
  call Call();
#line 1076
  call GetSize(Tj,($vt),($r2),($r1));
  
  assert TO(numFields(($r2)[Tj]));
  assert TVT(($r2)[Tj],($vt));
  call Tj:=Add(Tj,eax);
}

procedure scanObjectOther($vt:int)
#line 1083

requires ecx==($vt);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires Tj<Tk;
requires TV(Tj);
requires ($vt)==(($AbsMem)[($r2)[Tj]])[1];
requires isOtherTag(tag(($vt)));
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tj,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1083
  var save5:int;
#line 1083
  var save4:int;
#line 1083
  var save3:int;
#line 1083
  var save2:int;
#line 1083
  var save1:int;
#line 1103
  assert TO(numFields(($r2)[Tj]));
  assert TVT(($toAbs)[Tj],($vt));
  assert TVL(($r2)[Tj]);
  
  call edx:=RoLoad32(ecx+?VT_MASK);
  call edi:=RoLoad32(edx);
  call ebp:=RoLoad32(ecx+?VT_BASE_LENGTH);
  
  call esi:=Mov(1);
  
  
  loop:
  assert ebp==4*numFields(($r2)[Tj]);
  assert edx==mask(($vt));
  assert edi==ro32(mask(($vt)));
  assert (TSlot(esi)&&0<esi)&&esi<=ro32(mask(($vt)))+1;
  assert Pointer(($r2),Tj,($r2)[Tj]);
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj+4*numFields(($r2)[Tj]),Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert ObjInvBase(Tj,($r2),($toAbs),($AbsMem),($gcMem),($gcSlice));
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($r2)[Tj])==>(between(1,esi,fieldToSlot(($vt),j))==>ObjInvField(Tj,j,($r2),($r2),($toAbs),($AbsMem),($gcMem),($gcSlice)))));
  
  
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<numFields(($r2)[Tj])==>(!between(1,esi,fieldToSlot(($vt),j))==>ObjInvField(Tj,j,($r2),($r1),($toAbs),($AbsMem),($gcMem),($gcSlice)))));
  
  
  assert ($toAbs)[Tj]!=NO_ABS&&($toAbs)[Tj]==($r2)[Tj];
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 1131
  if(esi>edi)
  {
#line 1131
    goto loopEnd;
  }
  assert TO(0)&&TO(1);
  assert TVT(($toAbs)[Tj],($vt));
  assert TVL(($r2)[Tj]);
  assert TO(ro32(mask(($vt))+4*esi)+1);
  call ebx:=RoLoad32(edx+4*esi);
  
  havoc $Eip;
#line 1139
  if(ebx==0)
  {
#line 1139
    goto skip1;
  }
#line 1140
  call eax:=Mov(Tj);
  call gcLoad(eax+4*ebx+4);
#line 1141
  call ecx:=Load(eax+4*ebx+4);
  
  
  havoc $Eip;
#line 1144
  if(ecx<GcLo)
  {
#line 1144
    goto skip2;
  }
#line 1145
  havoc $Eip;
#line 1145
  if(ecx>GcHi)
  {
#line 1145
    goto skip2;
  }
#line 1146
  assert TV(ecx-4);
  call reach(($toAbs)[Tj],1+ro32(edx+4*esi),($Time));
  
  call save1:=Mov(ebx);
  call save2:=Mov(esi);
  call save3:=Mov(edi);
  call save4:=Mov(ebp);
  call save5:=Mov(edx);
  call forwardFromspacePtr(ecx,(($AbsMem)[($toAbs)[Tj]])[1+ro32(edx+4*esi)],Tj+ebp);
  call ebx:=Mov(save1);
  call esi:=Mov(save2);
  call edi:=Mov(save3);
  call ebp:=Mov(save4);
  call edx:=Mov(save5);
  
  call ecx:=Mov(Tj);
  call gcStore(ecx+4*ebx+4,eax);
#line 1162
  call Store(ecx+4*ebx+4,eax);
  skip2:
  skip1:
  call esi:=AddChecked(esi,1);
  havoc $Eip;
#line 1166
  goto loop;
  loopEnd:
  
  assert TVT(($toAbs)[Tj],($vt));
  assert TVL(($r2)[Tj]);
  call Tj:=Add(Tj,ebp);
}

procedure scanObject()
#line 1174

requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires Tj<Tk;
requires TV(Tj);
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tj,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1174
  var $vt:int;
#line 1187
  assert TO(1);
  call ecx:=Mov(Tj);
  call gcLoad(ecx+4);
#line 1189
  call ecx:=Load(ecx+4);
  $vt:=ecx;
  
  assert TO(numFields(($r2)[Tj]));
  
  call readTag(($toAbs)[Tj],($vt));
  
  havoc $Eip;
#line 1196
  if(eax!=?SPARSE_TAG)
  {
#line 1196
    goto skip1;
  }
#line 1197
  call scanObjectSparse(($vt));
  havoc $Eip;
#line 1198
  goto end;
  skip1:
  
  havoc $Eip;
#line 1201
  if(eax!=?DENSE_TAG)
  {
#line 1201
    goto skip2;
  }
#line 1202
  call scanObjectDense(($vt));
  havoc $Eip;
#line 1203
  goto end;
  skip2:
  
  havoc $Eip;
#line 1206
  if(eax!=?STRING_TAG)
  {
#line 1206
    goto skip3;
  }
#line 1207
  call scanObjectString(($vt));
  havoc $Eip;
#line 1208
  goto end;
  skip3:
  
  havoc $Eip;
#line 1211
  if(eax!=?PTR_VECTOR_TAG)
  {
#line 1211
    goto skip4;
  }
#line 1212
  call scanObjectPtrVector(($vt));
  havoc $Eip;
#line 1213
  goto end;
  skip4:
  
  havoc $Eip;
#line 1216
  if(eax!=?OTHER_VECTOR_TAG)
  {
#line 1216
    goto skip5;
  }
#line 1217
  call readArrayOf(($r2)[Tj],($vt));
  call readElementInfo(($r2)[Tj],($vt));
  havoc $Eip;
#line 1219
  if(ebp!=?TYPE_STRUCT)
  {
#line 1219
    goto noPoint;
  }
#line 1220
  havoc $Eip;
#line 1220
  if(ebp!=?TYPE_STRUCT)
  {
#line 1220
    goto vecSkip1;
  }
#line 1221
  havoc $Eip;
#line 1221
  if(edi!=?SPARSE_TAG)
  {
#line 1221
    goto vecSkip1;
  }
#line 1222
  noPoint:
  call scanObjectOtherVectorNoPointers(($vt));
  havoc $Eip;
#line 1224
  goto end;
  vecSkip1:
  havoc $Eip;
#line 1226
  if(ebp!=?TYPE_STRUCT)
  {
#line 1226
    goto vecSkip2;
  }
  
  call eax:=Mov(edi);
  call eax:=And(eax,15);
  havoc $Eip;
#line 1231
  if(eax!=?SPARSE_TAG)
  {
#line 1231
    goto vecSkip2;
  }
  call scanObjectOtherVectorPointers(($vt));
  havoc $Eip;
#line 1234
  goto end;
  
  vecSkip2:
  assert !((ebp!=?TYPE_STRUCT||(ebp==?TYPE_STRUCT&&edi==?SPARSE_TAG))||(ebp==?TYPE_STRUCT&&tag(arrayElementClass(($vt)))==?SPARSE_TAG));
  
  
  
  call eax:=Mov(1431633970);
  call _call();
#line 1242
  call Call();
#line 1242
  call DebugBreak();
  
  skip5:
  
  havoc $Eip;
#line 1246
  if(eax!=?PTR_ARRAY_TAG)
  {
#line 1246
    goto skip6;
  }
#line 1247
  call scanObjectPtrArray(($vt));
  havoc $Eip;
#line 1248
  goto end;
  skip6:
  
  havoc $Eip;
#line 1251
  if(eax!=?OTHER_ARRAY_TAG)
  {
#line 1251
    goto skip7;
  }
#line 1252
  call readArrayOf(($r2)[Tj],($vt));
  havoc $Eip;
#line 1253
  if(ebp==?TYPE_STRUCT)
  {
#line 1253
    goto arraySkip1;
  }
#line 1254
  call scanObjectOtherArrayNoPointers(($vt));
  havoc $Eip;
#line 1255
  goto end;
  arraySkip1:
  call eax:=Mov(1431633971);
  call _call();
#line 1258
  call Call();
#line 1258
  call DebugBreak();
  havoc $Eip;
#line 1259
  goto end;
  skip7:
  
  call scanObjectOther(($vt));
  
  end:
}

procedure scanObjects()
#line 1267

requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tj,Tk,$gcSlice;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures Tj==Tk;
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
  entry:
  loop:
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  call eax:=Mov(Tk);
  havoc $Eip;
#line 1283
  if(Tj>=eax)
  {
#line 1283
    goto exit;
  }
#line 1284
  call scanObject();
  havoc $Eip;
#line 1285
  goto loop;
  exit:
}

procedure FromInteriorPointer($iptr:int,$offset:int,$abs:int)
#line 1289

requires ecx==($iptr);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires 0<=($offset)&&($offset)<=4*numFields(($abs))-4;
requires Pointer(($r1),($iptr)-($offset)-4,($abs));
requires SMemRequireRA(100,($sMem),esp,($RET));
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
modifies $Eip,eax,ebx,ecx,edx,esp,$Mem,$sMem;
ensures eax==($offset);
ensures SMemEnsure(($sMem),old($sMem),esp,old(esp));
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
{
#line 1289
  var save2:int;
#line 1289
  var save1:int;
#line 1304
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
  call ebx:=Sub(ebx,Fi);
  call edx:=Mov(ebx);
  call edx:=And(edx,3);
  call __andAligned(ebx);
  call __addAligned(Fi,ebx);
  havoc $Eip;
#line 1323
  if(edx!=0)
  {
    goto skip1;
  }
  
  call save1:=Mov(eax);
  call save2:=Mov(ecx);
  call eax:=Mov(BF);
  call bb4GetBit(($r1),NO_ABS,Fi,Fi,Fi,Fl,($iptr)-save1-4,?gcLo+BitIndex(HeapLo,Fi),?gcLo+BitIndex(HeapLo,Fl));
  call eax:=Mov(save1);
  call ecx:=Mov(save2);
  
  havoc $Eip;
#line 1335
  if(ebx==0)
  {
#line 1335
    goto skip1;
  }
#line 1336
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 1336
  call Ret(old($RET));
#line 1336
  return;
  skip1:
  call eax:=Add(eax,1);
  havoc $Eip;
#line 1339
  goto loop;
}

procedure scanStackUpdateInvs($s:int,$r:[int]int,$f1:int,$f2:int,$frame:int,$addr:int,$v:int)
#line 1342

requires (($FrameSlices)[($s)])[($addr)]==($frame);
requires !(($f1)<=($frame)&&($frame)<($f2));
requires (forall f:int::{TV(f)}TV(f)==>(($f1)<=f&&f<($f2)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures (forall f:int::{TV(f)}TV(f)==>(($f1)<=f&&f<($f2)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],(($fMems)[($s)])[($addr):=($v)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));
{
  
  assert TO(0)&&TO(1);
  assert (forall f:int::{TV(f)}TV(f)==>TVF((($FrameLayouts)[($s)])[f])&&TVFT(f));
}

procedure scanStackWordDenseCopy($s:int,$frame:int,$addr:int,$desc:int,$addrp:int,$p:int,$args:int)
#line 1354

requires ecx==($addrp);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
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



requires (forall j:int::{TO(j)}TO(j)==>((($FrameSlices)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j]==($frame)==>(!inFrame((($FrameLayouts)[($s)])[($frame)],j)==>InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r2),(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j]))));



requires SMemRequire(108,($sMem),esp);

modifies $r2,$gcMem,$toAbs,Tk,$gcSlice,$fMems;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<($frame)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures (forall f:int::{TV(f)}TV(f)==>(($frame)<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures FrameNextInv(($frame),(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4],(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]],($FrameAddrs)[($s)],($FrameLayouts)[($s)]);
ensures (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&j<=($p)-1==>(($FrameSlices)[($s)])[($addr)+4*j]==($frame)&&InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r1),(($fMems)[($s)])[($addr)+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[($addr)+4*j])));



ensures (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&j>($p)-1==>(($FrameSlices)[($s)])[($addr)+4*j]==($frame)&&InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r2),(($fMems)[($s)])[($addr)+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[($addr)+4*j])));



ensures (forall j:int::{TO(j)}TO(j)==>((($FrameSlices)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j]==($frame)==>(!inFrame((($FrameLayouts)[($s)])[($frame)],j)==>InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r2),(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j]))));



ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures (forall $ss:int::{TStk(($ss))}($ss)!=($s)==>($fMems)[($ss)]==old($fMems)[($ss)]);
ensures (forall i:int::{TV(i)}TV(i)==>((($FrameSlices)[($s)])[i]==($FrameCounts)[($s)]==>(($fMems)[($s)])[i]==(old($fMems)[($s)])[i]));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1354
  var offset:int;
#line 1354
  var v:int;
#line 1354
  var save1:int;
#line 1419
  assert TVF((($FrameLayouts)[($s)])[($frame)]);
  assert TV((($fMems)[($s)])[($addrp)]-4);
  
  call fLoad(($s),ecx);
#line 1422
  call eax:=Load(ecx);
  
  havoc $Eip;
#line 1424
  if(eax<GcLo)
  {
#line 1424
    goto skip1;
  }
#line 1425
  havoc $Eip;
#line 1425
  if(eax>GcHi)
  {
#line 1425
    goto skip1;
  }
#line 1426
  call save1:=Mov(ecx);
  call v:=Mov(eax);
  
  call ecx:=Mov(eax);
  call _call();
#line 1430
  call Call();
#line 1430
  call FromInteriorPointer(v,(($FrameOffsets)[($s)])[($addrp)],((($FrameAbss)[($s)])[($frame)])[($p)]);
  call offset:=Mov(eax);
  
  call ecx:=Mov(v);
  call ecx:=Sub(ecx,eax);
  assert TV(ecx-4);
  
  call reachStackRoot(($s),($frame),($p),($Time));
  call forwardFromspacePtr(ecx,((($FrameAbss)[($s)])[($frame)])[($p)],Tj);
  
  assert TV(eax-4);
  call eax:=Add(eax,offset);
  call scanStackUpdateInvs(($s),($r1),0,($frame),($frame),($addrp),eax);
  call scanStackUpdateInvs(($s),($r2),($frame)+1,($FrameCounts)[($s)],($frame),($addrp),eax);
  
  call ecx:=Mov(save1);
  call fStore(($s),ecx,eax);
#line 1446
  call Store(ecx,eax);
  skip1:
}

procedure scanStackDenseCopy($s:int,$frame:int,$addr:int,$desc:int)
#line 1450

requires ecx==($addr);
requires edx==($desc);
requires ($desc)==frameDescriptor((($FrameLayouts)[($s)])[($frame)]);
requires ($addr)==(($FrameAddrs)[($s)])[($frame)];
requires (getBit(($desc),0)&&!getBit(($desc),1))&&and(shr(($desc),6),1023)==0;
requires (0<=($frame)&&($frame)<($FrameCounts)[($s)])&&TV(($frame));
requires (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<=($frame)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

requires (forall f:int::{TV(f)}TV(f)==>(($frame)<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tk,$gcSlice,$fMems;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<=($frame)-1==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures (forall f:int::{TV(f)}TV(f)==>(($frame)-1<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures (forall $ss:int::{TStk(($ss))}($ss)!=($s)==>($fMems)[($ss)]==old($fMems)[($ss)]);
ensures (forall i:int::{TV(i)}TV(i)==>((($FrameSlices)[($s)])[i]==($FrameCounts)[($s)]==>(($fMems)[($s)])[i]==(old($fMems)[($s)])[i]));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1450
  var addrp:int;
#line 1450
  var desc:int;
#line 1450
  var addr:int;
#line 1450
  var args:int;
#line 1450
  var offset:int;
#line 1450
  var v:int;
#line 1450
  var b:int;
#line 1482
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
  
  
  
  
  assert (forall j:int::{TO(j)}TO(j)==>((($FrameSlices)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j]==($frame)==>(!inFrame((($FrameLayouts)[($s)])[($frame)],j)==>InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r2),(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j]))));
  
  
  
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert (forall $ss:int::{TStk(($ss))}($ss)!=($s)==>($fMems)[($ss)]==old($fMems)[($ss)]);
  assert (forall i:int::{TV(i)}TV(i)==>((($FrameSlices)[($s)])[i]==($FrameCounts)[($s)]==>(($fMems)[($s)])[i]==(old($fMems)[($s)])[i]));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  call eax:=Mov(b);
  havoc $Eip;
#line 1530
  if(eax>=args)
  {
#line 1530
    goto loopEnd1;
  }
  assert TVF((($FrameLayouts)[($s)])[($frame)]);
  call ecx:=Lea(eax+16);
  call ebx:=Mov(desc);
  call ebx:=Shr(ebx,ecx);
  call ebx:=And(ebx,1);
  
  havoc $Eip;
#line 1538
  if(ebx!=1)
  {
#line 1538
    goto skip1;
  }
#line 1539
  call ecx:=Mov(addrp);
  call scanStackWordDenseCopy(($s),($frame),($addr),($desc),($addr)+4*(args+1-b),args+1-b,args);
  skip1:
  
  call b:=Add(b,1);
  call addrp:=Sub(addrp,4);
  havoc $Eip;
#line 1545
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
  
  
  
  
  assert (forall j:int::{TO(j)}TO(j)==>((($FrameSlices)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j]==($frame)==>(!inFrame((($FrameLayouts)[($s)])[($frame)],j)==>InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r2),(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j]))));
  
  
  
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert (forall $ss:int::{TStk(($ss))}($ss)!=($s)==>($fMems)[($ss)]==old($fMems)[($ss)]);
  assert (forall i:int::{TV(i)}TV(i)==>((($FrameSlices)[($s)])[i]==($FrameCounts)[($s)]==>(($fMems)[($s)])[i]==(old($fMems)[($s)])[i]));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  call eax:=Mov(b);
  havoc $Eip;
#line 1587
  if(eax>=16)
  {
#line 1587
    goto loopEnd2;
  }
  assert TVF((($FrameLayouts)[($s)])[($frame)]);
  call ecx:=Lea(eax+16);
  call ebx:=Mov(desc);
  call ebx:=Shr(ebx,ecx);
  call ebx:=And(ebx,1);
  
  havoc $Eip;
#line 1595
  if(ebx!=1)
  {
#line 1595
    goto skip2;
  }
#line 1596
  call ecx:=Mov(addrp);
  call scanStackWordDenseCopy(($s),($frame),($addr),($desc),($addr)+4*(args-1-b),args-1-b,args);
  skip2:
  call b:=Add(b,1);
  call addrp:=SubChecked(addrp,4);
  havoc $Eip;
#line 1601
  goto loop2;
  loopEnd2:
}

procedure scanStackSparse8Copy($s:int,$frame:int,$addr:int,$desc:int)
#line 1605

requires ecx==($addr);
requires edx==($desc);
requires ($desc)==frameDescriptor((($FrameLayouts)[($s)])[($frame)]);
requires ($addr)==(($FrameAddrs)[($s)])[($frame)];
requires !getBit(($desc),0)&&ro32(($desc))==4096;
requires (0<=($frame)&&($frame)<($FrameCounts)[($s)])&&TV(($frame));
requires (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<=($frame)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

requires (forall f:int::{TV(f)}TV(f)==>(($frame)<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tk,$gcSlice,$fMems;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<=($frame)-1==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures (forall f:int::{TV(f)}TV(f)==>(($frame)-1<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));

ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures (forall $ss:int::{TStk(($ss))}($ss)!=($s)==>($fMems)[($ss)]==old($fMems)[($ss)]);
ensures (forall i:int::{TV(i)}TV(i)==>((($FrameSlices)[($s)])[i]==($FrameCounts)[($s)]==>(($fMems)[($s)])[i]==(old($fMems)[($s)])[i]));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1605
  var count:int;
#line 1605
  var offset:int;
#line 1605
  var addrp:int;
#line 1605
  var desc:int;
#line 1605
  var addr:int;
#line 1605
  var v:int;
#line 1605
  var p:int;
#line 1637
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
  
  
  
  
  assert (forall j:int::{TO(j)}TO(j)==>((($FrameSlices)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j]==($frame)==>(!inFrame((($FrameLayouts)[($s)])[($frame)],j)==>InteriorValue(frameHasPtr((($FrameLayouts)[($s)])[($frame)],j),($r2),(($fMems)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j],((($FrameAbss)[($s)])[($frame)])[j],(($FrameOffsets)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*j]))));
  
  
  
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert (forall $ss:int::{TStk(($ss))}($ss)!=($s)==>($fMems)[($ss)]==old($fMems)[($ss)]);
  assert (forall i:int::{TV(i)}TV(i)==>((($FrameSlices)[($s)])[i]==($FrameCounts)[($s)]==>(($fMems)[($s)])[i]==(old($fMems)[($s)])[i]));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  call eax:=Mov(p);
  havoc $Eip;
#line 1677
  if(eax>=count)
  {
#line 1677
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
#line 1686
  call ecx:=Load(ebp);
  
  havoc $Eip;
#line 1688
  if(ecx<GcLo)
  {
#line 1688
    goto skip1;
  }
#line 1689
  havoc $Eip;
#line 1689
  if(ecx>GcHi)
  {
#line 1689
    goto skip1;
  }
#line 1690
  call v:=Mov(ecx);
  call _call();
#line 1691
  call Call();
#line 1691
  call FromInteriorPointer(v,(($FrameOffsets)[($s)])[addr+4*roS8(desc+6+p)],((($FrameAbss)[($s)])[($frame)])[roS8(desc+6+p)]);
  call offset:=Mov(eax);
  
  call ecx:=Mov(v);
  call ecx:=Sub(ecx,eax);
  assert TV(ecx-4);
  call reachStackRoot(($s),($frame),roS8(desc+6+p),($Time));
  call forwardFromspacePtr(ecx,((($FrameAbss)[($s)])[($frame)])[roS8(desc+6+p)],Tj);
  assert TV(eax-4);
  call eax:=Add(eax,offset);
  call scanStackUpdateInvs(($s),($r1),0,($frame),($frame),addr+4*roS8(desc+6+p),eax);
  call scanStackUpdateInvs(($s),($r2),($frame)+1,($FrameCounts)[($s)],($frame),addr+4*roS8(desc+6+p),eax);
  call ebx:=Mov(addrp);
  call fStore(($s),ebx,eax);
#line 1704
  call Store(ebx,eax);
  skip1:
  
  
  assert (forall j:int::{TO(j)}TO(j)==>(inFrame((($FrameLayouts)[($s)])[($frame)],j)&&p==frameFieldToSlot((($FrameLayouts)[($s)])[($frame)],j)==>j==roS8(desc+6+p)));
  
  
  call p:=Add(p,1);
  havoc $Eip;
#line 1712
  goto loop;
  loopEnd:
}

procedure scanStack($s:int,$ra:int,$nextFp:int)
#line 1716

requires ecx==($ra)&&word(($ra));
requires edx==($nextFp);
requires FrameNextInv(($FrameCounts)[($s)],($ra),($nextFp),($FrameAddrs)[($s)],($FrameLayouts)[($s)]);
requires GcStackInv(($s),($r1),($FrameCounts)[($s)],($FrameAddrs)[($s)],($FrameLayouts)[($s)],($FrameSlices)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)]);
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tk,$gcSlice,$fMems;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures GcStackInv(($s),($r2),($FrameCounts)[($s)],($FrameAddrs)[($s)],($FrameLayouts)[($s)],($FrameSlices)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)]);
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures (forall $ss:int::{TStk(($ss))}($ss)!=($s)==>($fMems)[($ss)]==old($fMems)[($ss)]);
ensures (forall i:int::{TV(i)}TV(i)==>((($FrameSlices)[($s)])[i]==($FrameCounts)[($s)]==>(($fMems)[($s)])[i]==(old($fMems)[($s)])[i]));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1716
  var _nextFp:int;
#line 1716
  var _ra:int;
#line 1716
  var found:int;
#line 1716
  var desc:int;
#line 1716
  var addr:int;
#line 1716
  var p:int;
#line 1716
  var $frame:int;
#line 1739
  call _ra:=Mov(ecx);
  call _nextFp:=Mov(edx);
  
  $frame:=($FrameCounts)[($s)]-1;
  loop:
  assert ($frame)<($FrameCounts)[($s)]&&TV(($frame));
  assert word(_ra);
  assert FrameNextInv(($frame)+1,_ra,_nextFp,($FrameAddrs)[($s)],($FrameLayouts)[($s)]);
  assert (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<=($frame)==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r1),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));
  
  assert (forall f:int::{TV(f)}TV(f)==>(($frame)<f&&f<($FrameCounts)[($s)]==>GcFrameInv(($s),f,(($FrameLayouts)[($s)])[f],($r2),($FrameAddrs)[($s)],($FrameSlices)[($s)],($FrameLayouts)[($s)],($fMems)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)])));
  
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert (forall $ss:int::{TStk(($ss))}($ss)!=($s)==>($fMems)[($ss)]==old($fMems)[($ss)]);
  assert (forall i:int::{TV(i)}TV(i)==>((($FrameSlices)[($s)])[i]==($FrameCounts)[($s)]==>(($fMems)[($s)])[i]==(old($fMems)[($s)])[i]));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  
  assert TVF((($FrameLayouts)[($s)])[($frame)]);
  assert TVFT(($frame)+1);
  
  call ecx:=Mov(_ra);
  call edx:=Mov(_nextFp);
  havoc $Eip;
#line 1762
  if(edx==0)
  {
#line 1762
    goto loopEnd;
  }
#line 1763
  call _call();
#line 1763
  call Call();
#line 1763
  call TablesSearch(($s),($frame)+1,_ra,_nextFp);
  call desc:=Mov(eax);
  call found:=Mov(edx);
  
  call ecx:=Mov(_nextFp);
  call addr:=Mov(ecx);
  assert TV(addr);
  assert TO(0);
  assert TO(1);
  call fLoad(($s),ecx);
#line 1772
  call eax:=Load(ecx);
  call _nextFp:=Mov(eax);
  call fLoad(($s),ecx+4);
#line 1774
  call eax:=Load(ecx+4);
  call _ra:=Mov(eax);
  
  
  call eax:=Mov(desc);
  call eax:=Shr(eax,0);
  call eax:=And(eax,1);
  havoc $Eip;
#line 1781
  if(eax!=1)
  {
#line 1781
    goto skip1;
  }
#line 1782
  call eax:=Mov(desc);
  call eax:=Shr(eax,1);
  call eax:=And(eax,1);
  havoc $Eip;
#line 1785
  if(eax==1)
  {
#line 1785
    goto skip1;
  }
#line 1786
  call eax:=Mov(desc);
  call eax:=Shr(eax,6);
  call eax:=And(eax,1023);
  havoc $Eip;
#line 1789
  if(eax!=0)
  {
#line 1789
    goto skip1;
  }
#line 1790
  call ecx:=Mov(addr);
  call edx:=Mov(desc);
  call scanStackDenseCopy(($s),($frame),addr,desc);
  $frame:=($frame)-1;
  havoc $Eip;
#line 1794
  goto loop;
  skip1:
  
  
  call eax:=Mov(desc);
  call eax:=Shr(eax,0);
  call eax:=And(eax,1);
  havoc $Eip;
#line 1801
  if(eax==1)
  {
#line 1801
    goto skip2;
  }
#line 1802
  call eax:=Mov(desc);
  call eax:=RoLoad32(eax);
  havoc $Eip;
#line 1804
  if(eax!=4096)
  {
#line 1804
    goto skip2;
  }
#line 1805
  call ecx:=Mov(addr);
  call edx:=Mov(desc);
  call scanStackSparse8Copy(($s),($frame),addr,desc);
  $frame:=($frame)-1;
  havoc $Eip;
#line 1809
  goto loop;
  skip2:
  
  
  assert !(((getBit(desc,0)&&!getBit(desc,1))&&and(shr(desc,6),1023)==0)||(!getBit(desc,0)&&ro32(desc)==4096));
  
  call eax:=Mov(1431633972);
  call _call();
#line 1816
  call Call();
#line 1816
  call DebugBreak();
  
  loopEnd:
  assert ($frame)==0-1;
}

procedure scanStacks()
#line 1822

requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires SMemRequire(108,($sMem),esp);
requires (forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))==>_StateInv(($s),($stackState)[($s)],($r1),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets))));
modifies $r2,$gcMem,$toAbs,Tk,$gcSlice,$fMems;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures (forall i:int::{TV(i)}TV(i)==>((($FrameSlices)[($S)])[i]==($FrameCounts)[($S)]==>(($fMems)[($S)])[i]==(old($fMems)[($S)])[i]));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
ensures (forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))==>_StateInv(($s),($stackState)[($s)],($r2),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets))));
{
#line 1822
  var tag:int;
#line 1822
  var t:int;
#line 1822
  var s:int;
#line 1834
  call s:=Mov(0);
  call eax:=Mov(TLo);
  call t:=Mov(eax);
  __L1:
  assert 0<=s&&TStk(s);
  assert t==?tLo+s*?TSize;
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert (forall i:int::{TV(i)}TV(i)==>((($FrameSlices)[($S)])[i]==($FrameCounts)[($S)]==>(($fMems)[($S)])[i]==(old($fMems)[($S)])[i]));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  assert (forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))&&s<=($s)==>_StateInv(($s),($stackState)[($s)],($r1),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets))));
  assert (forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))&&s>($s)==>_StateInv(($s),($stackState)[($s)],($r2),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets))));
#line 1837
  call eax:=Mov(?NumStacks);
#line 1837
  havoc $Eip;
#line 1837
  if(s>=eax)
  {
#line 1837
    goto __L2;
  }
  
  
  
  
  
  
  
  
  assert ((TV(?memLo)&&TO(512+65536*2+64*s))&&TO(512+65536*2+64*s+1))&&TO(512+65536*2+64*s+2);
  
  call eax:=Mov(t);
  call tLoad(s,eax);
#line 1850
  call eax:=Load(eax);
  call tag:=Mov(eax);
  
  call eax:=Mov(?STACK_YIELDED);
#line 1853
  havoc $Eip;
#line 1853
  if(tag!=eax)
  {
#line 1853
    goto __L3;
  }
  call ebx:=Mov(t);
  call tLoad(s,ebx+4);
#line 1856
  call ecx:=Load(ebx+4);
  call fLoad(s,ecx);
#line 1857
  call ecx:=Load(ecx);
  call tLoad(s,ebx+8);
#line 1858
  call edx:=Load(ebx+8);
  call scanStack(s,StackRA(s,($tMems),($fMems)),StackEbp(s,($tMems)));
  assert TV(StackEsp(s,($tMems)));
#line 1853
  __L3:
  
  
  
  
  
  
  
  
  call eax:=Mov(?STACK_INTERRUPTED);
#line 1862
  havoc $Eip;
#line 1862
  if(tag!=eax)
  {
#line 1862
    goto __L4;
  }
  call eax:=Mov(1431633973);
  call _call();
#line 1865
  call Call();
#line 1865
  call DebugBreak();
#line 1862
  __L4:
  
  
  
  
  call eax:=Mov(?STACK_RUNNING);
#line 1867
  havoc $Eip;
#line 1867
  if(tag!=eax)
  {
#line 1867
    goto __L5;
  }
  call eax:=Mov(1431633974);
  call _call();
#line 1870
  call Call();
#line 1870
  call DebugBreak();
#line 1867
  __L5:
  
  
  
  
  
  call s:=Add(s,1);
  call eax:=Mov(?TSize);
#line 1874
  call t:=Add(t,eax);
#line 1837
  havoc $Eip;
#line 1837
  goto __L1;
#line 1837
  __L2:
}
#line 1878
procedure scanStaticSectionCopy($section:int)
#line 1878

requires ecx==($section);
requires (0<=($section)&&($section)<?sectionCount)&&TV(($section));
requires (forall s:int::{TV(s)}TV(s)==>(($section)<=s&&s<?sectionCount==>SectionInv(s,sectionBase(s),sectionEnd(s),($r1),($SectionMem),($SectionAbs))));

requires (forall s:int::{TV(s)}TV(s)==>(0<=s&&s<($section)==>SectionInv(s,sectionBase(s),sectionEnd(s),($r2),($SectionMem),($SectionAbs))));

requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tk,$gcSlice,$SectionMem;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures (forall s:int::{TV(s)}TV(s)==>(($section)+1<=s&&s<?sectionCount==>SectionInv(s,sectionBase(s),sectionEnd(s),($r1),($SectionMem),($SectionAbs))));

ensures (forall s:int::{TV(s)}TV(s)==>(0<=s&&s<($section)+1==>SectionInv(s,sectionBase(s),sectionEnd(s),($r2),($SectionMem),($SectionAbs))));

ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1878
  var save3:int;
#line 1878
  var save2:int;
#line 1878
  var save1:int;
#line 1878
  var section:int;
#line 1878
  var addr:int;
#line 1878
  var sEnd:int;
#line 1904
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
  
  
  
  
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  havoc $Eip;
#line 1945
  if(edi>=sEnd)
  {
#line 1945
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
#line 1957
  if(eax!=1)
  {
#line 1957
    goto skip1;
  }
#line 1958
  assert TV(($SectionMem)[addr+4*esi]-4);
  call ecx:=SectionLoad(edi);
  
  havoc $Eip;
#line 1961
  if(ecx<GcLo)
  {
#line 1961
    goto skip2;
  }
#line 1962
  havoc $Eip;
#line 1962
  if(ecx>GcHi)
  {
#line 1962
    goto skip2;
  }
#line 1963
  call save1:=Mov(edi);
  call save2:=Mov(esi);
  call save3:=Mov(edx);
  call reachStaticRoot(section,esi,($Time));
  call forwardFromspacePtr(ecx,(($SectionAbs)[section])[esi],Tj);
  call edi:=Mov(save1);
  call esi:=Mov(save2);
  call edx:=Mov(save3);
  
  call SectionStore(edi,eax);
  skip2:
  skip1:
  
  call esi:=Add(esi,1);
  call edi:=AddChecked(edi,4);
  havoc $Eip;
#line 1978
  goto loop;
  loopEnd:
}

procedure scanStaticCopy()
#line 1982

requires StaticInv(($r1),($SectionMem),($SectionAbs));
requires CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires SMemRequire(108,($sMem),esp);
modifies $r2,$gcMem,$toAbs,Tk,$gcSlice,$SectionMem;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;
ensures StaticInv(($r2),($SectionMem),($SectionAbs));
ensures CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures RExtend(old($r2),($r2));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 1982
  var section:int;
#line 1994
  call section:=Mov(0);
  
  loop:
  assert 0<=section&&TV(section);
  assert (forall s:int::{TV(s)}TV(s)==>(section<=s&&s<?sectionCount==>SectionInv(s,sectionBase(s),sectionEnd(s),($r1),($SectionMem),($SectionAbs))));
  
  assert (forall s:int::{TV(s)}TV(s)==>(0<=s&&s<section==>SectionInv(s,sectionBase(s),sectionEnd(s),($r2),($SectionMem),($SectionAbs))));
  
  assert CopyGcInv(($freshAbs),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tj,Tk,Tk,Tl,($Time),($r1),($r2),true,($toAbs),($AbsMem),($gcSlice),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
  assert RExtend(old($r2),($r2));
  assert SMemInv(($sMem),old($sMem),esp,old(esp));
  call ecx:=Mov(section);
  havoc $Eip;
#line 2006
  if(ecx>=?sectionCount)
  {
#line 2006
    goto loopEnd;
  }
  call scanStaticSectionCopy(section);
  call section:=AddChecked(section,1);
  havoc $Eip;
#line 2010
  goto loop;
  loopEnd:
}

procedure __garbageCollect()
#line 2014

requires StaticInv(($toAbs),($SectionMem),($SectionAbs));
requires _NucleusInv(($S),($stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
requires SMemRequire(108,($sMem),esp);

modifies $r1,$r2,$gcMem,$toAbs,$gcSlice,$SectionMem,$fMems,$Time;
modifies Ti,Tj,Tk,Tl,Fi,Fk,Fl,BF,BT;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;


ensures StaticInv(($toAbs),($SectionMem),($SectionAbs));
ensures NucleusInv(($S),($stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures (forall i:int::{TV(i)}TV(i)==>((Fi<=i&&i<Fk)&&($toAbs)[i]!=NO_ABS==>reached(($toAbs)[i],($Time))));

ensures ebp==old(ebp);
ensures (forall i:int::{TV(i)}TV(i)==>((($FrameSlices)[($S)])[i]==($FrameCounts)[($S)]==>(($fMems)[($S)])[i]==(old($fMems)[($S)])[i]));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
#line 2014
  var saveEbp:int;
#line 2033
  call saveEbp:=Mov(ebp);
  call newTime();
  
  $r1:=($toAbs);
  $r2:=MAP_NO_ABS;
  call eax:=Mov(Ti);
  call Tj:=Mov(eax);
  call Tk:=Mov(eax);
  
  call eax:=Mov(BT);
  call ebx:=Mov(Fi);
  havoc $Eip;
#line 2044
  if(ebx!=HeapLo)
  {
#line 2044
    goto skip1;
  }
#line 2045
  call ebx:=Mov(HeapLo);
  assert ?gcLo+BitIndex(HeapLo,Tl)==ebx;
  havoc $Eip;
#line 2047
  goto skip2;
  skip1:
  call ebx:=Mov(BF);
  assert ?gcLo+BitIndex(HeapLo,Tl)==ebx;
  skip2:
  assert TVM(32,BitIndex(HeapLo,Fl)-BitIndex(HeapLo,Fi));
  assert TVM(32,BitIndex(HeapLo,Tl)-BitIndex(HeapLo,Ti));
  
  call _call();
#line 2055
  call Call();
#line 2055
  call BB4Zero(($toAbs),0,NO_ABS,Ti,Ti,Ti,Tl,0,?gcLo+BitIndex(HeapLo,Ti),?gcLo+BitIndex(HeapLo,Tl),Fi,?gcLo+BitIndex(HeapLo,Fi));
  
  call scanStacks();
  call scanStaticCopy();
  call scanObjects();
  
  $toAbs:=($r2);
  
  call eax:=Mov(Fi);
  call ebx:=Mov(Ti);
  call Fi:=Mov(ebx);
  call Ti:=Mov(eax);
  
  call eax:=Mov(Fl);
  call ebx:=Mov(Tl);
  call Fl:=Mov(ebx);
  call Tl:=Mov(eax);
  
  call eax:=Mov(Tk);
  call Fk:=Mov(eax);
  
  call eax:=Mov(Ti);
  call Tk:=Mov(eax);
  call Tj:=Mov(eax);
  
  call eax:=Mov(BF);
  call ebx:=Mov(BT);
  call BF:=Mov(ebx);
  call BT:=Mov(eax);
  
  call ebp:=Mov(saveEbp);
  
  assert __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
  assert CurrentStack==($S);
  
  assert (forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))==>_StateInv(($s),($stackState)[($s)],($toAbs),($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets))));
  
  assert NucleusInv(($S),($stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
}

procedure _garbageCollect()
#line 2095

requires StaticInv(($toAbs),($SectionMem),($SectionAbs));
requires NucleusInv(($S),($stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
requires SMemRequire(108,($sMem),esp);

modifies $r1,$r2,$gcMem,$toAbs,$gcSlice,$SectionMem,$fMems,$Time;
modifies Ti,Tj,Tk,Tl,Fi,Fk,Fl,BF,BT;
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp,$Mem,$sMem;


ensures StaticInv(($toAbs),($SectionMem),($SectionAbs));
ensures NucleusInv(($S),($stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures (forall i:int::{TV(i)}TV(i)==>((Fi<=i&&i<Fk)&&($toAbs)[i]!=NO_ABS==>reached(($toAbs)[i],($Time))));

ensures ebp==old(ebp);
ensures (forall i:int::{TV(i)}TV(i)==>((($FrameSlices)[($S)])[i]==($FrameCounts)[($S)]==>(($fMems)[($S)])[i]==(old($fMems)[($S)])[i]));
ensures SMemInv(($sMem),old($sMem),esp,old(esp));
{
  call __garbageCollect();
}

implementation GarbageCollect()
#line 2116
{
#line 2116
  var sp:int;
#line 2116
  var tm:int;
#line 2116
  var t:int;
  
  call edx:=Mov(ebp);
  call fLoad(($S),esp);
#line 2119
  call ecx:=Load(esp);
  
  
  
  
  call eax:=Mov(CurrentStack);
  call ebx:=Mov(?TSize);
  call eax,edx:=Mul(eax,ebx);
  call t:=Mov(eax);
  call eax:=Mov(TLo);
#line 2128
  call t:=Add(t,eax);
  assert t==?tLo+($S)*?TSize;
  assert ((TV(?memLo)&&TO(512+65536*2+64*($S)))&&TO(512+65536*2+64*($S)+1))&&TO(512+65536*2+64*($S)+2);
  call ebx:=Mov(t);
  call tStore(($S),ebx,?STACK_YIELDED);
#line 2132
  call Store(ebx,?STACK_YIELDED);
  call tStore(($S),ebx+4,esp);
#line 2133
  call Store(ebx+4,esp);
  call tStore(($S),ebx+8,ebp);
#line 2134
  call Store(ebx+8,ebp);
  
  call sp:=Mov(esp);
  call esp:=Mov(DLo);
  
  $stackState:=($StackState)[($S):=StackYielded(ebp,sp+4,ecx)];
  call _garbageCollect();
  
  call eax:=Mov(t);
  call tStore(($S),eax,?STACK_RUNNING);
#line 2143
  call Store(eax,?STACK_RUNNING);
  
  call esp:=Mov(sp);
  call _fret(($S),old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
  call Ret(old($RET));
  return;
}

procedure doAllocCopyingWord($ret:int,$ind:int)
#line 2151

requires esi==($ret)+4*($ind);

requires TO(($ind))&&($ind)>=0;
requires (Aligned(($ret))&&Fk<=($ret))&&($ret)+4*($ind)+4<=Fl;
requires __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
requires (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>($gcSlice)[($ret)+4*j]==($ret)));
requires (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>gcAddr(($ret)+4*j)));
requires (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>($gcMem)[($ret)+4*j]==NULL));

modifies $Eip,$gcMem,$Mem,$gcSlice;

ensures __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)+1==>($gcSlice)[($ret)+4*j]==($ret)));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)+1==>gcAddr(($ret)+4*j)));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)+1==>($gcMem)[($ret)+4*j]==NULL));
ensures ebp==old(ebp);
{
  assert TV(($ret));
  assert TV(($ret)+4*($ind));
  $gcSlice:=($gcSlice)[(($ret)+4*($ind)):=($ret)];
  
  call gcStore(esi,0);
#line 2173
  call Store(esi,0);
}

procedure doAllocCopyingWords($ret:int,$size:int,$nf:int)
#line 2176

requires eax==($ret);
requires ebx==($ret)+($size);
requires ($size)==($nf)*4;
requires ($nf)>=0;
requires __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
requires (Aligned(($ret))&&Fk<=($ret))&&($ret)+($size)<=Fl;

modifies $gcMem,$Mem,$gcSlice;
modifies $Eip,esi;

ensures __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($nf)==>($gcSlice)[($ret)+4*j]==($ret)));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($nf)==>gcAddr(($ret)+4*j)));
ensures (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($nf)==>($gcMem)[($ret)+4*j]==NULL));
ensures ebp==old(ebp);
{
#line 2176
  var $ind:int;
#line 2194
  $ind:=0;
  
  call esi:=Mov(GcLo);
  call esi:=Mov(eax);
  
  
  havoc $Eip;
#line 2200
  if(esi>=ebx)
  {
#line 2200
    goto loopEnd;
  }
#line 2201
  loop:
  assert 4*($ind)<($size);
  assert esi==($ret)+4*($ind);
  assert TO(($ind))&&($ind)>=0;
  assert __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>($gcSlice)[($ret)+4*j]==($ret)));
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>gcAddr(($ret)+4*j)));
  assert (forall j:int::{TO(j)}TO(j)==>(0<=j&&j<($ind)==>($gcMem)[($ret)+4*j]==NULL));
  
  call doAllocCopyingWord(($ret),($ind));
  $ind:=($ind)+1;
  call esi:=Add(esi,4);
  havoc $Eip;
#line 2213
  if(esi<ebx)
  {
#line 2213
    goto loop;
  }
#line 2214
  loopEnd:
}

procedure doAllocObjectCopying($nextFp:int,$abs:int,$vt:int,$size:int)
#line 2217

requires eax==($size);
requires ebx==Fk+($size);
requires ecx==($vt);
requires Fk+4*numFields(($abs))<=Fl;
requires !VFieldPtr(($abs),0);
requires !VFieldPtr(($abs),1);
requires 2<=numFields(($abs));
requires ($size)==4*numFields(($abs));
requires ObjSize(($abs),($vt),(($AbsMem)[($abs)])[2],(($AbsMem)[($abs)])[3]);
requires _NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);


requires word(($vt))&&!gcAddrEx(($vt));
requires VTable(($abs),($vt));
requires ObjSize(($abs),($vt),0,0);
requires !isVarSize(tag(($vt)));


requires ($abs)!=NO_ABS;
requires (forall i:int::{TV(i)}TV(i)==>(gcAddr(i)==>($toAbs)[i]!=($abs)));
requires (($AbsMem)[($abs)])[0]==NULL;
requires (($AbsMem)[($abs)])[1]==($vt);
requires (forall j:int::{TO(j)}TO(j)==>(2<=j&&j<numFields(($abs))==>(($AbsMem)[($abs)])[j]==NULL));

modifies $gcMem,$toAbs,$gcSlice,Fk;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,$Mem;

ensures NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures old($toAbs)[eax-4]==NO_ABS;
ensures ($toAbs)==old($toAbs)[(eax-4):=($abs)];
ensures Pointer(($toAbs),eax-4,($abs));
ensures ebp==old(ebp);
{
  call eax:=Mov(Fk);
  
  assert TV(eax+0)&&TV(eax+4);
  assert TO(1)&&TO(2);
  assert TO(numFields(($abs)));
  
  call doAllocCopyingWords(eax,($size),numFields(($abs)));
  
  call gcStore(eax+4,ecx);
#line 2259
  call Store(eax+4,ecx);
  
  call esi:=Mov(eax);
  call esi:=Sub(esi,Fi);
  call edi:=Mov(BF);
  call bb4SetBit(($toAbs),($abs),NO_ABS,Fi,Fi,Fi,Fl,Fk,?gcLo+BitIndex(HeapLo,Fi),?gcLo+BitIndex(HeapLo,Fl));
  assert TV(Fk-Fi);
  
  $toAbs:=($toAbs)[eax:=($abs)];
  assert TV(Fk);
  assert TO(numFields(($abs)));
  
  call Fk:=Mov(ebx);
  call eax:=Add(eax,4);
  
  assert TV(eax+4);
  assert TO(0);
}

procedure _doAllocStringCopying($nextFp:int,$abs:int,$vt:int,$nElems:int)
#line 2278

requires ecx==pad(16+2*($nElems));
requires ebx==Fk+pad(16+2*($nElems));
requires edx==($nElems);
requires ($vt)==?STRING_VTABLE;
requires Fk+pad(16+2*($nElems))+0<=Fl;
requires __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));


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

modifies $gcMem,$toAbs,$gcSlice,Fk;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,$Mem;

ensures __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
ensures old($toAbs)[eax-4]==NO_ABS;
ensures ($toAbs)==old($toAbs)[(eax-4):=($abs)];
ensures Pointer(($toAbs),eax-4,($abs));
ensures ebp==old(ebp);
{
  call eax:=Mov(Fk);
  assert ((TV(eax+0)&&TV(eax+4))&&TV(eax+8))&&TV(eax+12);
  assert (TO(1)&&TO(2))&&TO(3);
  assert TVL(($abs));
  assert TVT(($abs),($vt));
  assert TO(numFields(($abs)));
  
  call doAllocCopyingWords(eax,pad(16+2*($nElems)),numFields(($abs)));
  
  call gcStore(eax+8,edx);
#line 2320
  call Store(eax+8,edx);
  call edx:=SubChecked(edx,1);
  call gcStore(eax+12,edx);
#line 2322
  call Store(eax+12,edx);
  call edx:=Mov(?STRING_VTABLE);
  call gcStore(eax+4,edx);
#line 2324
  call Store(eax+4,edx);
  
  call esi:=Mov(eax);
  call esi:=Sub(esi,Fi);
  call edi:=Mov(BF);
  call bb4SetBit(($toAbs),($abs),NO_ABS,Fi,Fi,Fi,Fl,Fk,?gcLo+BitIndex(HeapLo,Fi),?gcLo+BitIndex(HeapLo,Fl));
  assert TV(Fk-Fi);
  
  $toAbs:=($toAbs)[eax:=($abs)];
  assert TV(Fk);
  assert TO(numFields(($abs)));
  
  call Fk:=Mov(ebx);
  call eax:=Add(eax,4);
  
  assert TV(eax+4);
  assert TO(0);
}

procedure doAllocStringCopying($nextFp:int,$abs:int,$vt:int,$nElems:int)
#line 2343

requires ecx==pad(16+2*($nElems));
requires ebx==Fk+pad(16+2*($nElems));
requires edx==($nElems);
requires ($vt)==?STRING_VTABLE;
requires Fk+pad(16+2*($nElems))+0<=Fl;
requires _NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);


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

modifies $gcMem,$toAbs,$gcSlice,Fk;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,$Mem;

ensures NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures old($toAbs)[eax-4]==NO_ABS;
ensures ($toAbs)==old($toAbs)[(eax-4):=($abs)];
ensures Pointer(($toAbs),eax-4,($abs));
ensures ebp==old(ebp);
{
  call _doAllocStringCopying(($nextFp),($abs),($vt),($nElems));
}

procedure fromArrayInfo($abs:int,$vt:int)
#line 2379

requires VTable(($abs),($vt));
requires tag(($vt))==?PTR_ARRAY_TAG||tag(($vt))==?OTHER_ARRAY_TAG;
ensures !VFieldPtr(($abs),0);
ensures !VFieldPtr(($abs),1);
ensures !VFieldPtr(($abs),2);
ensures !VFieldPtr(($abs),3);
{
  assert (((TO(0)&&TO(1))&&TO(2))&&TO(3))&&TO(4);
  assert TVL(($abs));
  assert TVT(($abs),($vt));
}

procedure doAllocArrayCopyingHelper($abs:int,$vt:int,$rank:int,$nElems:int)
#line 2392

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
#line 2416
  if(edi!=?PTR_ARRAY_TAG)
  {
#line 2416
    goto skip1;
  }
#line 2417
  call edi:=Mov(esi);
  call edi:=AddChecked(edi,edi);
  call edi:=AddChecked(edi,edi);
  call eax:=AddChecked(eax,edi);
  call eax:=AddChecked(eax,3);
  call edi:=Mov(3);
  call edi:=Not(edi);
  call eax:=And(eax,edi);
  havoc $Eip;
#line 2425
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

procedure _doAllocArrayCopying($nextFp:int,$abs:int,$vt:int,$rank:int,$nElems:int,$size:int)
#line 2439


requires ebx==Fk+($size);
requires ecx==($vt);
requires edx==($rank);
requires esi==($nElems);
requires Fk+4*numFields(($abs))<=Fl;
requires !VFieldPtr(($abs),0);
requires !VFieldPtr(($abs),1);
requires !VFieldPtr(($abs),2);
requires !VFieldPtr(($abs),3);
requires 4<=numFields(($abs));
requires ($size)==4*numFields(($abs));
requires ObjSize(($abs),($vt),($rank),($nElems));
requires __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));


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

modifies $gcMem,$toAbs,$gcSlice,Fk;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,$Mem;

ensures __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets));
ensures old($toAbs)[eax-4]==NO_ABS;
ensures ($toAbs)==old($toAbs)[(eax-4):=($abs)];
ensures Pointer(($toAbs),eax-4,($abs));
ensures ebp==old(ebp);
{
#line 2439
  var nElems:int;
#line 2482
  call eax:=Mov(Fk);
  call nElems:=Mov(esi);
  
  assert ((TV(eax+0)&&TV(eax+4))&&TV(eax+8))&&TV(eax+12);
  assert ((TO(1)&&TO(2))&&TO(3))&&TO(4);
  assert TO(numFields(($abs)));
  
  call doAllocCopyingWords(eax,($size),numFields(($abs)));
  
  call esi:=Mov(nElems);
  call gcStore(eax+4,ecx);
#line 2492
  call Store(eax+4,ecx);
  call gcStore(eax+8,edx);
#line 2493
  call Store(eax+8,edx);
  call gcStore(eax+12,esi);
#line 2494
  call Store(eax+12,esi);
  
  call esi:=Mov(eax);
  call esi:=Sub(esi,Fi);
  call edi:=Mov(BF);
  call bb4SetBit(($toAbs),($abs),NO_ABS,Fi,Fi,Fi,Fl,Fk,?gcLo+BitIndex(HeapLo,Fi),?gcLo+BitIndex(HeapLo,Fl));
  assert TV(Fk-Fi);
  
  $toAbs:=($toAbs)[eax:=($abs)];
  assert TV(Fk);
  assert TO(numFields(($abs)));
  
  call Fk:=Mov(ebx);
  call eax:=Add(eax,4);
  
  assert TV(eax+4);
  assert TO(0);
}

procedure doAllocArrayCopying($nextFp:int,$abs:int,$vt:int,$rank:int,$nElems:int,$size:int)
#line 2513


requires ebx==Fk+($size);
requires ecx==($vt);
requires edx==($rank);
requires esi==($nElems);
requires Fk+4*numFields(($abs))<=Fl;
requires !VFieldPtr(($abs),0);
requires !VFieldPtr(($abs),1);
requires !VFieldPtr(($abs),2);
requires !VFieldPtr(($abs),3);
requires 4<=numFields(($abs));
requires ($size)==4*numFields(($abs));
requires ObjSize(($abs),($vt),($rank),($nElems));
requires _NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);


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

modifies $gcMem,$toAbs,$gcSlice,Fk;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,$Mem;

ensures NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures old($toAbs)[eax-4]==NO_ABS;
ensures ($toAbs)==old($toAbs)[(eax-4):=($abs)];
ensures Pointer(($toAbs),eax-4,($abs));
ensures ebp==old(ebp);
{
  call _doAllocArrayCopying(($nextFp),($abs),($vt),($rank),($nElems),($size));
}

procedure doAllocVectorCopyingHelper($abs:int,$vt:int,$nElems:int)
#line 2558

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
#line 2582
  if(edi!=?PTR_VECTOR_TAG)
  {
#line 2582
    goto skip1;
  }
#line 2583
  call edi:=Mov(edx);
  call edi:=AddChecked(edi,edi);
  call edi:=AddChecked(edi,edi);
  call eax:=AddChecked(eax,edi);
  call eax:=AddChecked(eax,3);
  call edi:=Mov(3);
  call edi:=Not(edi);
  call eax:=And(eax,edi);
  havoc $Eip;
#line 2591
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

procedure doAllocVectorCopying($nextFp:int,$abs:int,$vt:int,$nElems:int,$size:int)
#line 2605


requires ebx==Fk+($size);
requires ecx==($vt);
requires edx==($nElems);
requires Fk+4*numFields(($abs))<=Fl;
requires !VFieldPtr(($abs),0);
requires !VFieldPtr(($abs),1);
requires !VFieldPtr(($abs),2);
requires 3<=numFields(($abs));
requires ($size)==4*numFields(($abs));
requires ObjSize(($abs),($vt),($nElems),(($AbsMem)[($abs)])[3]);
requires _NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);


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

modifies $gcMem,$toAbs,$gcSlice,Fk;
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,$Mem;

ensures NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures old($toAbs)[eax-4]==NO_ABS;
ensures ($toAbs)==old($toAbs)[(eax-4):=($abs)];
ensures Pointer(($toAbs),eax-4,($abs));
ensures ebp==old(ebp);
{
  call eax:=Mov(Fk);
  
  assert (TV(eax+0)&&TV(eax+4))&&TV(eax+8);
  assert (TO(1)&&TO(2))&&TO(3);
  assert TO(numFields(($abs)));
  
  call doAllocCopyingWords(eax,($size),numFields(($abs)));
  
  call gcStore(eax+4,ecx);
#line 2651
  call Store(eax+4,ecx);
  call gcStore(eax+8,edx);
#line 2652
  call Store(eax+8,edx);
  
  call esi:=Mov(eax);
  call esi:=Sub(esi,Fi);
  call edi:=Mov(BF);
  call bb4SetBit(($toAbs),($abs),NO_ABS,Fi,Fi,Fi,Fl,Fk,?gcLo+BitIndex(HeapLo,Fi),?gcLo+BitIndex(HeapLo,Fl));
  assert TV(Fk-Fi);
  
  $toAbs:=($toAbs)[eax:=($abs)];
  assert TV(Fk);
  assert TO(numFields(($abs)));
  
  call Fk:=Mov(ebx);
  call eax:=Add(eax,4);
  
  assert TV(eax+4);
  assert TO(0);
}

procedure doAllocObjectCopyingHelper($abs:int,$vt:int)
#line 2671

requires ecx==($vt);
requires VTable(($abs),($vt));
requires ObjSize(($abs),($vt),0,0);
requires !isVarSize(tag(($vt)));
modifies $Eip,eax;
ensures !VFieldPtr(($abs),0);
ensures !VFieldPtr(($abs),1);
ensures 2<=numFields(($abs));
ensures eax==4*numFields(($abs));
ensures ObjSize(($abs),($vt),(($AbsMem)[($abs)])[2],(($AbsMem)[($abs)])[3]);
{
  assert TO(2);
  assert TVL(($abs));
  assert TVT(($abs),($vt));
  call eax:=RoLoad32(ecx+?VT_BASE_LENGTH);
}

implementation InitializeGc()
#line 2689
{
#line 2689
  var unitSize:int;
#line 2689
  var save:int;
  
  
  
  call save:=Mov(ebp);
#line 2703
  call esi:=Mov(GcHi);
  call esi:=Sub(esi,GcLo);
  call edi:=Mov(esi);
  
  
  call edx:=Mov(0);
  call eax:=Mov(edi);
  call ebx:=Mov(264);
  call eax,edx:=Div(eax,edx,ebx);
  call ebp:=Mov(eax);
  call unitSize:=Mov(ebp);
  assert 264*unitSize<=?gcHi-?gcLo;
  
  
  call edx:=Mov(0);
  call ebp:=Lea(edx+4*ebp);
  call eax:=Mov(GcLo);
  call BF:=Mov(eax);
  call eax:=Add(eax,ebp);
  call BT:=Mov(eax);
  call ebx:=Lea(eax+ebp);
  call ebp:=Lea(edx+4*ebp);
  call ecx:=Lea(ebx+8*ebp);
  call edx:=Lea(ecx+8*ebp);
  
  call HeapLo:=Mov(ebx);
  call Fi:=Mov(ebx);
  call Fk:=Mov(ebx);
  call Fl:=Mov(ecx);
  call Ti:=Mov(ecx);
  call Tj:=Mov(ecx);
  call Tk:=Mov(ecx);
  call Tl:=Mov(edx);
  
  call __initialize(unitSize,HeapLo);
  
  assert TV(?gcLo);
  assert TV(HeapLo);
  assert TO(0);
  assert TO(unitSize);
  assert TO(2*unitSize);
  assert TO(32*unitSize);
  call eax:=Mov(GcLo);
  call esi:=Mov(unitSize);
  call ebx:=Lea(eax+4*esi);
  call _call();
#line 2748
  call Call();
#line 2748
  call BB4Zero(($toAbs),0,NO_ABS,Fi,Fi,Fi,Fl,0,?gcLo,?gcLo+4*unitSize,0,0);
  
  assert TVM(32,BitIndex(HeapLo,Fl)-BitIndex(HeapLo,Fi));
  assert TVM(32,BitIndex(HeapLo,Tl)-BitIndex(HeapLo,Ti));
  
  call ebp:=Mov(save);
  call _ret(old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
#line 2754
  call Ret(old($RET));
#line 2754
  return;
}

implementation revealInv1($s:int,$_stackState:[int]StackState)
#line 2757
{
}


implementation updateInv1($oldPciConfigState:[int]int,$oldPciMem:[int]int,$oldMem:[int]int)
#line 2761
{
}


implementation setStackEmpty($s:int)
#line 2765
{
  
  call tStore(($s),eax,?STACK_EMPTY);
#line 2767
  call Store(eax,?STACK_EMPTY);
  $stackState:=($StackState)[($s):=StackEmpty];
}

implementation setStackYielded()
#line 2771
{
  
  assert (TV(eax)&&TO(1))&&TO(2);
  call tStore(($S),eax,?STACK_YIELDED);
#line 2774
  call Store(eax,?STACK_YIELDED);
  call tStore(($S),eax+4,esp);
#line 2775
  call Store(eax+4,esp);
  call tStore(($S),eax+8,ebp);
#line 2776
  call Store(eax+8,ebp);
  $stackState:=($StackState)[($S):=StackYielded(ebp,esp+4,($Mem)[esp])];
  call revealFMem();
}

implementation setStackInterrupted()
#line 2781
{
#line 2781
  var saveEdx:int;
#line 2781
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
#line 2793
  call Store(eax,?STACK_INTERRUPTED);
  call tStore(($S),eax+4,esp);
#line 2794
  call Store(eax+4,esp);
  call tStore(($S),eax+8,ebp);
#line 2795
  call Store(eax+8,ebp);
  call edx:=Mov(saveEax);
  call tStore(($S),eax+12,edx);
#line 2797
  call Store(eax+12,edx);
  call tStore(($S),eax+16,ebx);
#line 2798
  call Store(eax+16,ebx);
  call tStore(($S),eax+20,ecx);
#line 2799
  call Store(eax+20,ecx);
  call edx:=Mov(saveEdx);
  call tStore(($S),eax+24,edx);
#line 2801
  call Store(eax+24,edx);
  call tStore(($S),eax+28,esi);
#line 2802
  call Store(eax+28,esi);
  call tStore(($S),eax+32,edi);
#line 2803
  call Store(eax+32,edi);
  $stackState:=($StackState)[($S):=StackInterrupted(old(eax),ebx,ecx,old(edx),esi,edi,ebp,esp+12,($Mem)[esp],($Mem)[esp+4],($Mem)[esp+8])];
  call revealFMem();
}

implementation setStackRunning1($s:int)
#line 2808
{
  
  call fStore(($s),esp,ecx);
#line 2810
  call Store(esp,ecx);
  call tStore(($s),edi,?STACK_RUNNING);
#line 2811
  call Store(edi,?STACK_RUNNING);
}

implementation setStackRunning2($s:int,$_stackState:[int]StackState)
#line 2814
{
  
  call CurrentStack:=Mov(ecx);
  call fStore(($s),esp,eax);
#line 2817
  call Store(esp,eax);
  call tStore(($s),edi,?STACK_RUNNING);
#line 2818
  call Store(edi,?STACK_RUNNING);
}

implementation setStackRunning3($s:int,$_stackState:[int]StackState,$ebp:int,$esp:int,$eip:int)
#line 2821
{
  
  call CurrentStack:=Mov(ecx);
  assert (TV(edi)&&TO(1))&&TO(2);
  call tLoad(($s),edi+4);
#line 2825
  call esp:=Load(edi+4);
  call tLoad(($s),edi+8);
#line 2826
  call ebp:=Load(edi+8);
  call tStore(($s),edi,?STACK_RUNNING);
#line 2827
  call Store(edi,?STACK_RUNNING);
  call revealFMem();
}

implementation setStackRunning4($s:int,$_stackState:[int]StackState,$eax:int,$ebx:int,$ecx:int,$edx:int,$esi:int,$edi:int,$ebp:int,$esp:int,$eip:int,$cs:int,$efl:int)
#line 2831
{
  
  
  call CurrentStack:=Mov(ecx);
  call tStore(($s),edi,?STACK_RUNNING);
#line 2835
  call Store(edi,?STACK_RUNNING);
  
  assert (((((((TV(edi)&&TO(1))&&TO(2))&&TO(3))&&TO(4))&&TO(5))&&TO(6))&&TO(7))&&TO(8);
  call tLoad(($s),edi+4);
#line 2838
  call esp:=Load(edi+4);
  call tLoad(($s),edi+8);
#line 2839
  call ebp:=Load(edi+8);
  call tLoad(($s),edi+12);
#line 2840
  call eax:=Load(edi+12);
  call tLoad(($s),edi+16);
#line 2841
  call ebx:=Load(edi+16);
  call tLoad(($s),edi+20);
#line 2842
  call ecx:=Load(edi+20);
  call tLoad(($s),edi+24);
#line 2843
  call edx:=Load(edi+24);
  call tLoad(($s),edi+28);
#line 2844
  call esi:=Load(edi+28);
  call tLoad(($s),edi+32);
#line 2845
  call edi:=Load(edi+32);
  call revealFMem();
}

implementation readField($ptr:int,$fld:int)
#line 2849
returns($val:int)
{
  assert TV(($val))&&TV(($ptr));
  assert TO(($fld));
  call gcLoad(($ptr)+4*($fld));
  
  $val:=($gcMem)[($ptr)+4*($fld)];
}

implementation writeField($ptr:int,$fld:int,$val:int,$abs:int)
#line 2858
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
#line 2871
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
#line 2879
function TXO(i:int)
  returns(bool)
{
  true
}
#line 2881
procedure _writeStack($ptr:int,$val:int,$frame:int,$j:int,$fX:[int]int,$jX:[int]int,$_frameCount:int,$_frameAddr:[int]int,$_frameLayout:[int]$FrameLayout,$_frameSlice:[int]int,$_frameAbs:[int][int]int,$_frameOffset:[int]int)
#line 2881




returns($_mem:[int]int)
requires isStack(($S))&&($StackState)[($S)]==StackRunning;
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);

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
ensures __NucleusInv(($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($_mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts)[($S):=($_frameCount)],($FrameAddrs)[($S):=($_frameAddr)],($FrameLayouts)[($S):=($_frameLayout)],($FrameSlices)[($S):=($_frameSlice)],($FrameAbss)[($S):=($_frameAbs)],($FrameOffsets)[($S):=($_frameOffset)]);
{
  
  
  
  
  
  
  assert TV(($frame))&&TO(($j));
  call fStore(($S),($ptr),($val));
  $_mem:=($Mem)[($ptr):=($val)];
  assert (forall f:int,j:int::{TXV(f),TXO(j)}TXV(f)&&TXO(j)==>TV(($fX)[f])&&TO(($jX)[j]));
}

implementation writeStack($ptr:int,$val:int,$frame:int,$j:int,$fX:[int]int,$jX:[int]int,$_frameCount:int,$_frameAddr:[int]int,$_frameLayout:[int]$FrameLayout,$_frameSlice:[int]int,$_frameAbs:[int][int]int,$_frameOffset:[int]int)
#line 2928




returns($_mem:[int]int)
{
  assert TStk(($S));
  call _inFrameSlice();
  assert (forall f:int,j:int::{TXV(f),TXO(j)}TV(f)&&TO(j)==>((0<=f&&f<($_frameCount))&&_InFrameSlice(f,j,($_frameAddr),($_frameSlice))==>(($_frameAddr)[f]+4*j!=($ptr)==>(((((0<=($fX)[f]&&($fX)[f]<($FrameCounts)[($S)])&&(($FrameSlices)[($S)])[(($FrameAddrs)[($S)])[($fX)[f]]+4*($jX)[j]]==($fX)[f])&&($_frameAddr)[f]+4*j==(($FrameAddrs)[($S)])[($fX)[f]]+4*($jX)[j])&&(($_frameAbs)[f])[j]==((($FrameAbss)[($S)])[($fX)[f]])[($jX)[j]])&&frameHasPtr(($_frameLayout)[f],j)==frameHasPtr((($FrameLayouts)[($S)])[($fX)[f]],($jX)[j]))&&($_frameOffset)[($_frameAddr)[f]+4*j]==(($FrameOffsets)[($S)])[(($FrameAddrs)[($S)])[($fX)[f]]+4*($jX)[j]])));
  
  
  
  
  
  
  
  
  call $_mem:=_writeStack(($ptr),($val),($frame),($j),($fX),($jX),($_frameCount),($_frameAddr),($_frameLayout),($_frameSlice),($_frameAbs),($_frameOffset));
}


implementation AllocObject($abs:int,$vt:int)
#line 2949
{
  
  call doAllocObjectCopyingHelper(($abs),($vt));
  
  $freshAbs:=($abs);
  
  call ebx:=Mov(Fk);
  call ebx:=AddChecked(ebx,eax);
  
  
  havoc $Eip;
#line 2959
  if(ebx<=Fl)
  {
#line 2959
    goto skip1;
  }
#line 2960
  call eax:=Mov(0);
  havoc $Eip;
#line 2961
  goto done;
  skip1:
  
  call doAllocObjectCopying(ebp,($abs),($vt),eax);
  
  done:
  call _fret(($S),old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
  call Ret(old($RET));
  return;
}

implementation AllocString($abs:int,$vt:int,$nElems:int)
#line 2972
{
  
  $freshAbs:=($abs);
  
  call ecx:=AddChecked(ecx,1);
  call edx:=Mov(ecx);
  call ecx:=AddChecked(ecx,ecx);
  call ecx:=AddChecked(ecx,19);
  call eax:=Mov(3);
  call eax:=Not(eax);
  call ecx:=And(ecx,eax);
  call ebx:=Mov(Fk);
  call ebx:=AddChecked(ebx,ecx);
  
  havoc $Eip;
#line 2986
  if(ebx<=Fl)
  {
#line 2986
    goto skip1;
  }
#line 2987
  call eax:=Mov(0);
  havoc $Eip;
#line 2988
  goto done;
  skip1:
  
  call doAllocStringCopying(ebp,($abs),($vt),($nElems));
  
  done:
  call _fret(($S),old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
  call Ret(old($RET));
  return;
}

implementation AllocArray($abs:int,$vt:int,$rank:int,$nElems:int)
#line 2999
{
#line 2999
  var rank:int;
  
  
  $freshAbs:=($abs);
  
  assert TO(1)&&TV(esp);
  call fLoad(($S),esp+4);
#line 3005
  call esi:=Load(esp+4);
  
  call rank:=Mov(edx);
  call doAllocArrayCopyingHelper(($abs),($vt),($rank),($nElems));
  call ebx:=Mov(Fk);
  call ebx:=AddChecked(ebx,eax);
  
  
  havoc $Eip;
#line 3013
  if(ebx<=Fl)
  {
#line 3013
    goto skip1;
  }
#line 3014
  call eax:=Mov(0);
  havoc $Eip;
#line 3015
  goto done;
  skip1:
  
  call edx:=Mov(rank);
  call doAllocArrayCopying(ebp,($abs),($vt),($rank),($nElems),eax);
  
  done:
  call _fret(($S),old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
  call Ret(old($RET));
  return;
}

implementation AllocVector($abs:int,$vt:int,$nElems:int)
#line 3027
{
#line 3027
  var nElems:int;
  
  
  call nElems:=Mov(edx);
  call doAllocVectorCopyingHelper(($abs),($vt),($nElems));
  
  $freshAbs:=($abs);
  
  call ebx:=Mov(Fk);
  call ebx:=AddChecked(ebx,eax);
  
  
  havoc $Eip;
#line 3039
  if(ebx<=Fl)
  {
#line 3039
    goto skip1;
  }
#line 3040
  call eax:=Mov(0);
  havoc $Eip;
#line 3041
  goto done;
  skip1:
  
  call edx:=Mov(nElems);
  call doAllocVectorCopying(ebp,($abs),($vt),($nElems),eax);
  
  done:
  call _fret(($S),old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
  call Ret(old($RET));
  return;
}

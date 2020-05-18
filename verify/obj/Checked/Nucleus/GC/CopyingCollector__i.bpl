#line 11
var HeapLo:int;


var Fi:int;
var Fk:int;
var Fl:int;
var Ti:int;
var Tj:int;
var Tk:int;
var Tl:int;

var BF:int;
var BT:int;
var $freshAbs:int;












axiom (forall $s:int::StackHi(($s))==?fLo+($s)*?FSize+?FSize);
axiom (forall $s:int::StackLo(($s))==?fLo+($s)*?FSize);

procedure InitializeGc();
requires SMemRequireRA(104,($sMem),esp,($RET));
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires IoInv(($IoMmuEnabled),($PciConfigState),DmaAddr,($pciMem));
requires CurrentStack==($S);
requires (forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))==>IsEmpty(($StackState)[($s)])));
requires (forall $s:int::{TStk(($s))}TStk(($s))==>(isStack(($s))==>StackTag(($s),($tMems))==?STACK_EMPTY));
requires (forall i:int::{TV(i)}($toAbs)[i]==NO_ABS);
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp;
modifies CurrentStack,$gcSlice,BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,$stackState,$r1,$r2,$freshAbs,$Time,$Mem,$sMem,$dMem,$pciMem,$tMems,$fMems,$gcMem,SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi;
ensures SMemEnsure(($sMem),old($sMem),esp,old(esp));
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures WellFormed(($toAbs));
ensures ebp==old(ebp);

procedure revealInv1($s:int,$_stackState:[int]StackState);
requires isStack(($s));
requires NucleusInv(($S),($_stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures ($S)==CurrentStack;
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures IoInv(($IoMmuEnabled),($PciConfigState),DmaAddr,($pciMem));
ensures StackStateTag(($_stackState)[($s)])==StackTag(($s),($tMems));
ensures IsYielded(($_stackState)[($s)])==>($_stackState)[($s)]==StackYielded(StackEbp(($s),($tMems)),StackEsp(($s),($tMems))+4,StackRA(($s),($tMems),($fMems)));


procedure updateInv1($oldPciConfigState:[int]int,$oldPciMem:[int]int,$oldMem:[int]int);
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($oldMem),($sMem),($dMem),($oldPciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($oldPciConfigState),DmaAddr);


requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires IoInv(($IoMmuEnabled),($PciConfigState),DmaAddr,($pciMem));
ensures NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);

procedure setStackEmpty($s:int);
requires Aligned(eax);
requires eax==?tLo+($s)*?TSize;
requires isStack(($s));
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
modifies $Eip;
modifies $Mem,$fMems,$tMems,$stackState;
ensures NucleusInv(($S),($StackState)[($s):=StackEmpty],($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures ($Mem)==old($Mem)[eax:=?STACK_EMPTY];

procedure setStackYielded();
requires Aligned(esp);
requires fAddr(($S),esp);
requires Aligned(eax);
requires eax==?tLo+($S)*?TSize;
requires word(ebp)&&word(esp);
requires isStack(($S))&&($StackState)[($S)]==StackRunning;
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
requires ScanStackInv(($S),($Mem),($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($Mem)[esp],esp,ebp);
modifies $Eip;
modifies $Mem,$fMems,$tMems,$stackState;
ensures NucleusInv(($S),($StackState)[($S):=StackYielded(ebp,esp+4,($Mem)[esp])],($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures ($Mem)==((old($Mem)[eax:=?STACK_YIELDED])[(eax+4):=esp])[(eax+8):=ebp];

procedure setStackInterrupted();
requires (((((word(eax)&&word(ebx))&&word(ecx))&&word(edx))&&word(esi))&&word(edi))&&word(ebp);
requires Aligned(esp);
requires fAddr(($S),esp)&&fAddr(($S),esp+8);
requires word(ebp)&&word(esp);
requires isStack(($S))&&($StackState)[($S)]==StackRunning;
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
modifies $Eip,eax,edx;
modifies $Mem,$fMems,$tMems,$stackState;
ensures NucleusInv(($S),($StackState)[($S):=StackInterrupted(old(eax),ebx,ecx,old(edx),esi,edi,ebp,esp+12,old($Mem)[esp],old($Mem)[esp+4],old($Mem)[esp+8])],($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);



ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

procedure setStackRunning1($s:int);
requires isStack(($s));
requires Aligned(esp);
requires Aligned(edi);
requires esp==StackHi(($s))-8;
requires edi==?tLo+($s)*?TSize;
requires word(ecx);
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
requires ($FrameCounts)[($s)]==0;
modifies $Eip;
modifies $Mem,$fMems,$tMems;
ensures ($Mem)[esp+0]==ecx;
ensures NucleusInv(($S),($StackState)[($s):=StackRunning],($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);

procedure setStackRunning2($s:int,$_stackState:[int]StackState);
requires ecx==($s);
requires isStack(($s));
requires Aligned(esp);
requires Aligned(edi);
requires esp==StackHi(($s))-8;
requires edi==?tLo+($s)*?TSize;
requires word(eax);
requires NucleusInv(($S),($_stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
requires ($FrameCounts)[($s)]==0;
modifies $Eip;
modifies $Mem,$fMems,$tMems,CurrentStack;
ensures ($Mem)[esp+0]==eax;
ensures NucleusInv(($s),($_stackState)[($s):=StackRunning],($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);

procedure setStackRunning3($s:int,$_stackState:[int]StackState,$ebp:int,$esp:int,$eip:int);
requires ecx==($s);
requires isStack(($s))&&($_stackState)[($s)]==StackYielded(($ebp),($esp),($eip));
requires Aligned(edi);
requires edi==?tLo+($s)*?TSize;
requires NucleusInv(($S),($_stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,esp;
modifies $Mem,$fMems,$tMems,CurrentStack;
ensures NucleusInv(($s),($_stackState)[($s):=StackRunning],($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures esp==($esp)-4;
ensures ebp==($ebp);
ensures Aligned(esp);
ensures ($Mem)[($esp)-4]==($eip);

procedure setStackRunning4($s:int,$_stackState:[int]StackState,$eax:int,$ebx:int,$ecx:int,$edx:int,$esi:int,$edi:int,$ebp:int,$esp:int,$eip:int,$cs:int,$efl:int);

requires ecx==($s);
requires isStack(($s))&&($_stackState)[($s)]==StackInterrupted(($eax),($ebx),($ecx),($edx),($esi),($edi),($ebp),($esp),($eip),($cs),($efl));
requires Aligned(edi);
requires edi==?tLo+($s)*?TSize;
requires NucleusInv(($S),($_stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,esp;
modifies $Mem,$fMems,$tMems,CurrentStack;
ensures NucleusInv(($s),($_stackState)[($s):=StackRunning],($toAbs),($AbsMem),CurrentStack,($gcSlice),BF,BT,HeapLo,Fi,Fk,Fl,Ti,Tj,Tk,Tl,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures esp==($esp)-12;
ensures (((((eax==($eax)&&ebx==($ebx))&&ecx==($ecx))&&edx==($edx))&&esi==($esi))&&edi==($edi))&&ebp==($ebp);
ensures Aligned(esp);
ensures ($Mem)[($esp)-4]==($efl);
ensures ($Mem)[($esp)-8]==($cs);
ensures ($Mem)[($esp)-12]==($eip);

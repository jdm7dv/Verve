#line 19
const MAP_ZERO:[int]int;
axiom (forall i:int::{TV(i)}MAP_ZERO[i]==0);


const MAP_NO_ABS:[int]int;
axiom (forall i:int::{TV(i)}MAP_NO_ABS[i]==NO_ABS);

function AlignedHeapAddr(i:int)
  returns(bool)
{
  gcAddr(i)&&Aligned(i)
}



function RExtend(rOld:[int]int,rNew:[int]int)
  returns(bool)
{
  (forall i:int::{rOld[i]}rOld[i]!=NO_ABS==>rOld[i]==rNew[i])
}
function AbsMapped(val:int,rev:[int]int,abs:int)
  returns(bool)
{
  abs!=NO_ABS&&abs==rev[val]
}


var $r1:[int]int;
var $r2:[int]int;







function TStk($s:int)
  returns(bool)
{
  true
}
#line 57
var $stackState:[int]StackState;
var CurrentStack:int;

function StackTag($s:int,$tMems:[int][int]int)
  returns(int)
{
  (($tMems)[($s)])[?tLo+($s)*?TSize+0]
}
#line 61
function StackEsp($s:int,$tMems:[int][int]int)
  returns(int)
{
  (($tMems)[($s)])[?tLo+($s)*?TSize+4]
}
#line 62
function StackEbp($s:int,$tMems:[int][int]int)
  returns(int)
{
  (($tMems)[($s)])[?tLo+($s)*?TSize+8]
}
#line 63
function StackEax($s:int,$tMems:[int][int]int)
  returns(int)
{
  (($tMems)[($s)])[?tLo+($s)*?TSize+12]
}
#line 64
function StackEbx($s:int,$tMems:[int][int]int)
  returns(int)
{
  (($tMems)[($s)])[?tLo+($s)*?TSize+16]
}
#line 65
function StackEcx($s:int,$tMems:[int][int]int)
  returns(int)
{
  (($tMems)[($s)])[?tLo+($s)*?TSize+20]
}
#line 66
function StackEdx($s:int,$tMems:[int][int]int)
  returns(int)
{
  (($tMems)[($s)])[?tLo+($s)*?TSize+24]
}
#line 67
function StackEsi($s:int,$tMems:[int][int]int)
  returns(int)
{
  (($tMems)[($s)])[?tLo+($s)*?TSize+28]
}
#line 68
function StackEdi($s:int,$tMems:[int][int]int)
  returns(int)
{
  (($tMems)[($s)])[?tLo+($s)*?TSize+32]
}
#line 69
function StackRA($s:int,$tMems:[int][int]int,$fMems:[int][int]int)
  returns(int)
{
  (($fMems)[($s)])[StackEsp(($s),($tMems))]
}
#line 70
function StackCS($s:int,$tMems:[int][int]int,$fMems:[int][int]int)
  returns(int)
{
  (($fMems)[($s)])[StackEsp(($s),($tMems))+4]
}
#line 71
function StackEfl($s:int,$tMems:[int][int]int,$fMems:[int][int]int)
  returns(int)
{
  (($fMems)[($s)])[StackEsp(($s),($tMems))+8]
}
#line 73
function IsEmpty($state:StackState)
  returns(bool)
{
  StackStateTag(($state))==?STACK_EMPTY
}
function IsYielded($state:StackState)
  returns(bool)
{
  StackStateTag(($state))==?STACK_YIELDED
}
function IsInterrupted($state:StackState)
  returns(bool)
{
  StackStateTag(($state))==?STACK_INTERRUPTED
}
function MutatorFrameInvAddrs($s:int,f:int,l:$FrameLayout,r:[int]int,$FrameAddr:[int]int,$FrameSlice:[int]int,$FrameLayout:[int]$FrameLayout,$FrameAbs:[int][int]int,$FrameOffset:[int]int,$fMem:[int]int)
  returns(bool)
{
  (forall j:int::{TO(j)}TO(j)==>(($FrameSlice)[($FrameAddr)[f]+4*j]==f==>Aligned(($FrameAddr)[f]+4*j)&&fAddr(($s),($FrameAddr)[f]+4*j)))
}



function MutatorFrameInv($s:int,f:int,l:$FrameLayout,r:[int]int,$FrameAddr:[int]int,$FrameSlice:[int]int,$FrameLayout:[int]$FrameLayout,$FrameAbs:[int][int]int,$FrameOffset:[int]int,$fMem:[int]int)
  returns(bool)
{
  (forall j:int::{TO(j)}TO(j)==>(($FrameSlice)[($FrameAddr)[f]+4*j]==f==>(Aligned(($FrameAddr)[f]+4*j)&&fAddr(($s),($FrameAddr)[f]+4*j))&&InteriorValue(frameHasPtr(l,j),r,($fMem)[($FrameAddr)[f]+4*j],(($FrameAbs)[f])[j],($FrameOffset)[($FrameAddr)[f]+4*j])))
}




function MutatorStackInv($s:int,r:[int]int,$FrameCount:int,$FrameAddr:[int]int,$FrameLayout:[int]$FrameLayout,$FrameSlice:[int]int,$FrameAbs:[int][int]int,$FrameOffset:[int]int,$fMem:[int]int)
  returns(bool)
{
  (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<($FrameCount)==>MutatorFrameInv(($s),f,($FrameLayout)[f],r,($FrameAddr),($FrameSlice),($FrameLayout),($FrameAbs),($FrameOffset),($fMem))))
}


function _InFrameSlice(f:int,j:int,$FrameAddr:[int]int,$FrameSlice:[int]int)
  returns(bool)
;
#line 114
procedure _inFrameSlice();
ensures (forall f:int,j:int,$FrameAddr:[int]int,$FrameSlice:[int]int::_InFrameSlice(f,j,($FrameAddr),($FrameSlice))<==>($FrameSlice)[($FrameAddr)[f]+4*j]==f);


function _MutatorFrameInv($s:int,f:int,l:$FrameLayout,r:[int]int,$FrameAddr:[int]int,$FrameSlice:[int]int,$FrameLayout:[int]$FrameLayout,$FrameAbs:[int][int]int,$FrameOffset:[int]int,$fMem:[int]int)
  returns(bool)
{
  (forall j:int::{TO(j)}TO(j)==>(_InFrameSlice(f,j,($FrameAddr),($FrameSlice))==>(Aligned(($FrameAddr)[f]+4*j)&&fAddr(($s),($FrameAddr)[f]+4*j))&&InteriorValue(frameHasPtr(l,j),r,($fMem)[($FrameAddr)[f]+4*j],(($FrameAbs)[f])[j],($FrameOffset)[($FrameAddr)[f]+4*j])))
}




function _MutatorStackInv($s:int,r:[int]int,$FrameCount:int,$FrameAddr:[int]int,$FrameLayout:[int]$FrameLayout,$FrameSlice:[int]int,$FrameAbs:[int][int]int,$FrameOffset:[int]int,$fMem:[int]int)
  returns(bool)
{
  (forall f:int::{TV(f)}TV(f)==>(0<=f&&f<($FrameCount)==>_MutatorFrameInv(($s),f,($FrameLayout)[f],r,($FrameAddr),($FrameSlice),($FrameLayout),($FrameAbs),($FrameOffset),($fMem))))
}


function MutatorStackInvS($s:int,r:[int]int,$FrameCounts:[int]int,$FrameAddrs:[int][int]int,$FrameLayouts:[int][int]$FrameLayout,$FrameSlices:[int][int]int,$FrameAbss:[int][int][int]int,$FrameOffsets:[int][int]int,$fMems:[int][int]int)
  returns(bool)
{
  MutatorStackInv(($s),r,($FrameCounts)[($s)],($FrameAddrs)[($s)],($FrameLayouts)[($s)],($FrameSlices)[($s)],($FrameAbss)[($s)],($FrameOffsets)[($s)],($fMems)[($s)])
}

function GcFrameInv($s:int,f:int,l:$FrameLayout,r:[int]int,$FrameAddr:[int]int,$FrameSlice:[int]int,$FrameLayout:[int]$FrameLayout,$fMem:[int]int,$FrameAbs:[int][int]int,$FrameOffset:[int]int)
  returns(bool)
{
  FrameInv(f,l,($FrameAddr),($FrameSlice),($FrameLayout),($FrameAbs),($FrameOffset))&&MutatorFrameInv(($s),f,l,r,($FrameAddr),($FrameSlice),($FrameLayout),($FrameAbs),($FrameOffset),($fMem))
}

function GcStackInv($s:int,r:[int]int,$FrameCount:int,$FrameAddr:[int]int,$FrameLayout:[int]$FrameLayout,$FrameSlice:[int]int,$fMem:[int]int,$FrameAbs:[int][int]int,$FrameOffset:[int]int)
  returns(bool)
{
  StackInv(($FrameCount),($FrameAddr),($FrameLayout),($FrameSlice),($FrameAbs),($FrameOffset))&&MutatorStackInv(($s),r,($FrameCount),($FrameAddr),($FrameLayout),($FrameSlice),($FrameAbs),($FrameOffset),($fMem))
}
#line 160
var $gcSlice:[int]int;


function ObjInvBase(i:int,rs:[int]int,$toAbs:[int]int,$AbsMem:[int][int]int,$gcMem:[int]int,$gcSlice:[int]int)
  returns(bool)
{
  gcAddr(i)&&rs[i]!=NO_ABS==>((((((Aligned(i)&&AlignedHeapAddr(i+4))&&VTable(rs[i],(($AbsMem)[rs[i]])[1]))&&!VFieldPtr(rs[i],1))&&numFields(rs[i])>=2)&&ObjSize(rs[i],(($AbsMem)[rs[i]])[1],(($AbsMem)[rs[i]])[2],(($AbsMem)[rs[i]])[3]))&&($gcSlice)[i]==i)&&($gcSlice)[i+4]==i
}
#line 177
function ObjInvField(i:int,j:int,rs:[int]int,rt:[int]int,$toAbs:[int]int,$AbsMem:[int][int]int,$gcMem:[int]int,$gcSlice:[int]int)
  returns(bool)
{
  gcAddr(i)&&rs[i]!=NO_ABS==>(gcAddr(i+4*j)&&($gcSlice)[i+4*j]==i)&&Value(VFieldPtr(rs[i],j),rt,($gcMem)[i+4*j],(($AbsMem)[($toAbs)[i]])[j])
}




function ObjInvPartial(i:int,j1:int,j2:int,rs:[int]int,rt:[int]int,$toAbs:[int]int,$AbsMem:[int][int]int,$gcMem:[int]int,$gcSlice:[int]int)
  returns(bool)
{
  ObjInvBase(i,rs,($toAbs),($AbsMem),($gcMem),($gcSlice))&&(forall j:int::{TO(j)}TO(j)==>(j1<=j&&j<j2==>ObjInvField(i,j,rs,rt,($toAbs),($AbsMem),($gcMem),($gcSlice))))
}



function ObjInv(i:int,rs:[int]int,rt:[int]int,$toAbs:[int]int,$AbsMem:[int][int]int,$gcMem:[int]int,$gcSlice:[int]int)
  returns(bool)
{
  ObjInvPartial(i,0,numFields(rs[i]),rs,rt,($toAbs),($AbsMem),($gcMem),($gcSlice))
}

procedure TablesSearch($s:int,$f:int,$ra:int,$nextFp:int);
requires ecx==($ra)&&edx==($nextFp);
requires word(($ra));
requires FrameNextInv(($f),($ra),($nextFp),($FrameAddrs)[($s)],($FrameLayouts)[($s)]);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires SMemRequireRA(104,($sMem),esp,($RET));
modifies $Eip,$RET,eax,ebx,ecx,edx,esi,edi,ebp,esp;
modifies $Mem,$sMem;


ensures edx!=0==>(($f)>0==>eax==frameDescriptor((($FrameLayouts)[($s)])[($f)-1])&&(($FrameAddrs)[($s)])[($f)-1]==($nextFp));
ensures edx==0==>($f)==0;
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures SMemEnsure(($sMem),old($sMem),esp,old(esp));

procedure GetSize($ptr:int,$vt:int,$rs:[int]int,$rt:[int]int);
requires ecx==($ptr)&&edx==($vt);
requires gcAddr(($ptr));
requires ($rs)[($ptr)]!=NO_ABS&&($rs)[($ptr)]==($toAbs)[($ptr)];
requires ObjInv(($ptr),($rs),($rt),($toAbs),($AbsMem),($gcMem),($gcSlice));
requires ($vt)==(($AbsMem)[($rs)[($ptr)]])[1]||($vt)==($gcMem)[($ptr)+4];
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires SMemRequireRA(100,($sMem),esp,($RET));
modifies $Eip,eax,ebx,edx,esi,edi,ebp,esp;
ensures eax==4*numFields(($rs)[($ptr)]);
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures SMemEnsure(($sMem),old($sMem),esp,old(esp));

procedure readTag($abs:int,$vt:int);
requires ecx==($vt);
requires VTable(($abs),($vt));
modifies $Eip,eax;
ensures eax==tag(($vt));

procedure readArrayOf($abs:int,$vt:int);
requires ecx==($vt);
requires VTable(($abs),($vt));
modifies $Eip,ebp;
ensures ebp==arrayOf(($vt));

procedure readElementInfo($abs:int,$vt:int);
requires ecx==($vt);
requires VTable(($abs),($vt));
requires tag(($vt))==?OTHER_VECTOR_TAG;
modifies $Eip,esi,edi;
ensures esi==arrayElementSize(($vt));
ensures edi==mask(arrayElementClass(($vt)));





function IoInv($IoMmuEnabled:bool,$PciConfigState:[int]int,DmaAddr:int,$pciMem:[int]int)
  returns(bool)
;
#line 254
function _IoInv($IoMmuEnabled:bool,$PciConfigState:[int]int,DmaAddr:int,$pciMem:[int]int)
  returns(bool)
{
  ((((DmaAddr==0&&!($IoMmuEnabled))||(DmaAddr==?dmaLo&&($IoMmuEnabled)))&&?dmaLo>8)&&word(?dmaLo-8))&&(forall i:int::{TV(i)}TV(i)==>(0<=i&&i<65536==>(($pciMem)[?pciLo+8*i]==0&&($PciConfigState)[i]==0)||(((($pciMem)[?pciLo+8*i]!=0&&($PciConfigState)[i]==4)&&PciMemAddr(i)==($pciMem)[?pciLo+8*i])&&PciMemSize(i)==($pciMem)[?pciLo+8*i+4])))
}







procedure _ioInv();
ensures (forall $IoMmuEnabled:bool,$PciConfigState:[int]int,DmaAddr:int,$pciMem:[int]int::IoInv(($IoMmuEnabled),($PciConfigState),DmaAddr,($pciMem))<==>_IoInv(($IoMmuEnabled),($PciConfigState),DmaAddr,($pciMem)));

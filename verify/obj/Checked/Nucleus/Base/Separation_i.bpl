#line 12
var $sMem:[int]int;
var $dMem:[int]int;
var $pciMem:[int]int;
var $tMems:[int][int]int;
var $fMems:[int][int]int;
var $gcMem:[int]int;

var SLo:int;
var DLo:int;
var PciLo:int;
var TLo:int;
var FLo:int;
var GcLo:int;
var GcHi:int;

var DmaAddr:int;







const ?SSize:int;
#line 35
axiom ?SSize==1024;
const ?DSize:int;
#line 36
axiom ?DSize==1024;
const ?PciSize:int;
#line 37
axiom ?PciSize==8*65536;
const ?TSize:int;
#line 38
axiom ?TSize==256;
const ?FSize:int;
#line 39
axiom ?FSize==16384;


const ?sLo:int;
#line 42
axiom ?sLo==?memLo;
const ?sHi:int;
#line 43
axiom ?sHi==?sLo+?SSize;
const ?dLo:int;
#line 44
axiom ?dLo==?sHi;
const ?dHi:int;
#line 45
axiom ?dHi==?dLo+?DSize;
const ?pciLo:int;
#line 46
axiom ?pciLo==?dHi;
const ?pciHi:int;
#line 47
axiom ?pciHi==?pciLo+?PciSize;
const ?tLo:int;
#line 48
axiom ?tLo==?pciHi;
const ?tHi:int;
#line 49
axiom ?tHi==?tLo+?NumStacks*?TSize;
const ?fLo:int;
#line 50
axiom ?fLo==?tHi;
const ?fHi:int;
#line 51
axiom ?fHi==?fLo+?NumStacks*?FSize;

function isStack(s:int)
  returns(bool)
{
  0<=s&&s<?NumStacks
}
#line 55
function sAddr(i:int)
  returns(bool)
{
  ?sLo<=i&&i<?sHi
}
#line 56
function sAddrEx(i:int)
  returns(bool)
{
  ?sLo<=i&&i<=?sHi
}
#line 58
function dAddr(i:int)
  returns(bool)
{
  ?dLo<=i&&i<?dHi
}
#line 59
function dAddrEx(i:int)
  returns(bool)
{
  ?dLo<=i&&i<=?dHi
}
#line 61
function pciAddr(i:int)
  returns(bool)
{
  ?pciLo<=i&&i<?pciHi
}
#line 62
function pciAddrEx(i:int)
  returns(bool)
{
  ?pciLo<=i&&i<=?pciHi
}
#line 64
function fAddr(s:int,i:int)
  returns(bool)
{
  (isStack(s)&&?fLo+s*?FSize<=i)&&i<?fLo+s*?FSize+?FSize
}
#line 65
function fAddrEx(s:int,i:int)
  returns(bool)
{
  (isStack(s)&&?fLo+s*?FSize<=i)&&i<=?fLo+s*?FSize+?FSize
}
#line 67
function tAddr(s:int,i:int)
  returns(bool)
{
  (isStack(s)&&?tLo+s*?TSize<=i)&&i<?tLo+s*?TSize+?TSize
}
#line 68
function tAddrEx(s:int,i:int)
  returns(bool)
{
  (isStack(s)&&?tLo+s*?TSize<=i)&&i<=?tLo+s*?TSize+?TSize
}
#line 70
function MemSepInv($Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int)
  returns(bool)
;
#line 72
function MemInv($Mem:[int]int,$sMem:[int]int,$dMem:[int]int,$pciMem:[int]int,$tMems:[int][int]int,$fMems:[int][int]int,$gcMem:[int]int,SLo:int,DLo:int,PciLo:int,TLo:int,FLo:int,GcLo:int,GcHi:int)
  returns(bool)
{
  ((((((((((((((MemSepInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi)&&?gcLo<=?gcHi)&&Aligned(?sLo))&&Aligned(?dLo))&&Aligned(?pciLo))&&Aligned(?tLo))&&Aligned(?fLo))&&Aligned(?gcLo))&&Aligned(?gcHi))&&SLo==?sLo)&&DLo==?dLo)&&PciLo==?pciLo)&&TLo==?tLo)&&FLo==?fLo)&&GcLo==?gcLo)&&GcHi==?gcHi
}
#line 92
procedure initSeparation();
requires Aligned(?memLo);
requires Aligned(?memHi);
requires ecx==?memLo;
requires edx==?memHi;
modifies $Eip,eax,ebx,ecx,edx,esp;
modifies $sMem,$dMem,$pciMem,$tMems,$fMems,$gcMem,SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi;
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures esp==old(esp);


procedure sLoad($ptr:int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires sAddr(($ptr));
ensures ($Mem)[($ptr)]==($sMem)[($ptr)];


procedure sStore($ptr:int,$val:int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires sAddr(($ptr));
modifies $sMem;
ensures MemInv(($Mem)[($ptr):=($val)],($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures ($sMem)==old($sMem)[($ptr):=($val)];


procedure dLoad($ptr:int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires dAddr(($ptr));
ensures ($Mem)[($ptr)]==($dMem)[($ptr)];


procedure dStore($ptr:int,$val:int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires dAddr(($ptr));
modifies $dMem;
ensures MemInv(($Mem)[($ptr):=($val)],($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures ($dMem)==old($dMem)[($ptr):=($val)];


procedure pciLoad($ptr:int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires pciAddr(($ptr));
ensures ($Mem)[($ptr)]==($pciMem)[($ptr)];


procedure pciStore($ptr:int,$val:int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires pciAddr(($ptr));
modifies $pciMem;
ensures MemInv(($Mem)[($ptr):=($val)],($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures ($pciMem)==old($pciMem)[($ptr):=($val)];


procedure gcLoad($ptr:int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires gcAddr(($ptr));
requires TV(($ptr));
ensures ($Mem)[($ptr)]==($gcMem)[($ptr)];
ensures TV(($Mem)[($ptr)]);


procedure gcStore($ptr:int,$val:int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires gcAddr(($ptr));
requires TV(($ptr))&&TV(($val));
modifies $gcMem;
ensures MemInv(($Mem)[($ptr):=($val)],($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures ($gcMem)==old($gcMem)[($ptr):=($val)];

procedure fLoad($s:int,$ptr:int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires fAddr(($s),($ptr));
ensures ($Mem)[($ptr)]==(($fMems)[($s)])[($ptr)];
ensures memAddr(($ptr));

procedure fStore($s:int,$ptr:int,$val:int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires fAddr(($s),($ptr));
modifies $fMems;
ensures MemInv(($Mem)[($ptr):=($val)],($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures ($fMems)==old($fMems)[($s):=(old(($fMems)[($s)])[($ptr):=($val)])];
ensures memAddr(($ptr));

procedure tLoad($s:int,$ptr:int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires tAddr(($s),($ptr));
ensures ($Mem)[($ptr)]==(($tMems)[($s)])[($ptr)];
ensures memAddr(($ptr));

procedure tStore($s:int,$ptr:int,$val:int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires tAddr(($s),($ptr));
modifies $tMems;
ensures MemInv(($Mem)[($ptr):=($val)],($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures ($tMems)==old($tMems)[($s):=(old(($tMems)[($s)])[($ptr):=($val)])];
ensures memAddr(($ptr));


function SMemRequire(n:int,$sMem:[int]int,$esp:int)
  returns(bool)
{
  (Aligned(($esp))&&?sLo+n<=($esp))&&($esp)<=?sHi
}


function SMemRequireRA(n:int,$sMem:[int]int,$esp:int,$RET:ReturnTo)
  returns(bool)
{
  ((SMemRequire(n,($sMem),($esp))&&?sLo+n<=($esp))&&($esp)<=?sHi-4)&&ReturnToAddr(($sMem)[($esp)])==($RET)
}



function SMemInv($sMem:[int]int,oldSMem:[int]int,$esp:int,oldEsp:int)
  returns(bool)
{
  ($esp)==oldEsp&&(forall i:int::{($sMem)[i]}($esp)<=i==>($sMem)[i]==oldSMem[i])
}


function SMemEnsure($sMem:[int]int,oldSMem:[int]int,$esp:int,oldEsp:int)
  returns(bool)
{
  ($esp)==oldEsp+4&&(forall i:int::{($sMem)[i]}($esp)<=i==>($sMem)[i]==oldSMem[i])
}

procedure revealFMem();
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures (forall s:int,i:int::{(($fMems)[s])[i]}fAddr(s,i)==>($Mem)[i]==(($fMems)[s])[i]);

procedure _call();
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires sAddr(esp-4);
modifies $sMem;
ensures ($sMem)==old($sMem)[(esp-4):=NextEip(($Eip))];
ensures MemInv(($Mem)[(esp-4):=NextEip(($Eip))],($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);


procedure _ret($oldMem:[int]int,$oldSMem:[int]int,$oldDMem:[int]int,$oldPciMem:[int]int,$oldTMem:[int][int]int,$oldFMem:[int][int]int,$oldGcMem:[int]int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires MemInv(($oldMem),($oldSMem),($oldDMem),($oldPciMem),($oldTMem),($oldFMem),($oldGcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires ($sMem)[esp]==($oldSMem)[esp];
requires sAddr(esp);
ensures ($Mem)[esp]==($oldMem)[esp];
ensures ($Mem)[esp]==($sMem)[esp];

procedure _fret($s:int,$oldMem:[int]int,$oldSMem:[int]int,$oldDMem:[int]int,$oldPciMem:[int]int,$oldTMem:[int][int]int,$oldFMem:[int][int]int,$oldGcMem:[int]int);
requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires MemInv(($oldMem),($oldSMem),($oldDMem),($oldPciMem),($oldTMem),($oldFMem),($oldGcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires (($fMems)[($s)])[esp]==(($oldFMem)[($s)])[esp];
requires fAddr(($s),esp);
ensures ($Mem)[esp]==($oldMem)[esp];
ensures ($Mem)[esp]==(($fMems)[($s)])[esp];

//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//

// The Nucleus defines and establishes NucleusInv, which is abstract to managed code.
function NucleusInv($S:int, $StackState:[int]StackState, $toAbs:[int]int, $AbsMem:[int][int]int,
  CurrentStack:int, $gcSlice:[int]int, $color:[int]int, StackTop:int, $fs:[int]int, $fn:[int]int, CachePtr:int, CacheSize:int,
  ColorBase:int, HeapLo:int, HeapHi:int, ReserveMin:int,
  $Mem:[int]int, $sMem:[int]int, $dMem:[int]int, $pciMem:[int]int, $tMems:[int][int]int, $fMems:[int][int]int, $gcMem:[int]int,
  SLo:int, DLo:int, PciLo:int, TLo:int, FLo:int, GcLo:int, GcHi:int,
  $FrameCounts:[int]int, $FrameAddrs:[int][int]int, $FrameLayouts:[int][int]$FrameLayout,
  $FrameSlices:[int][int]int, $FrameAbss:[int][int][int]int, $FrameOffsets:[int][int]int,
  $IoMmuEnabled:bool, $PciConfigState:[int]int, DmaAddr:int
  ) returns(bool);

//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//

// Each managed stack $s grows from StackHi($s) down to StackLo($s)
function StackHi($s:int) returns(int);
function StackLo($s:int) returns(int);

// Requirement on entry to Nucleus from managed call:
// Since interrupts are disabled when calling the Nucleus, we do not
// require ?InterruptReserve be reserved between StackLo and esp.
function SpRequire($s:int, sp:int, n:int) returns(bool)
{
    StackLo($s) <= sp && sp + n <= StackHi($s) && Aligned(sp)
}

function StackCheckInv($s:int, StackCheck:int) returns(bool)
{
    StackLo($s) + ?StackReserve + ?InterruptReserve <= StackCheck
 && StackCheck <= StackHi($s)
}

function ScanStackInv($S:int, $Mem:[int]int,
  $FrameCounts:[int]int, $FrameAddrs:[int][int]int, $FrameLayouts:[int][int]$FrameLayout,
  $FrameSlices:[int][int]int, $FrameAbss:[int][int][int]int, $FrameOffsets:[int][int]int,
  $ra:int, $esp:int, $ebp:int) returns(bool)
{
    0 <= $FrameCounts[$S]
 && $FrameSlices[$S][$esp] == $FrameCounts[$S] && $Mem[$esp] == $ra
 && FrameNextInv($FrameCounts[$S], $ra, $ebp, $FrameAddrs[$S], $FrameLayouts[$S])
 && StackInvS($S, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets)
}

// Call the garbage collector.
// REVIEW: most system calls will share these preconditions and postconditions, so we should define some more abbreviations
procedure GarbageCollect();
  // GC invariant:
  requires word(ebp);
  requires isStack($S) && $StackState[$S] == StackRunning;
  requires NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  requires SpRequire($S, esp, 4);
  requires ReturnToAddr($Mem[esp]) == $RET;

  // requirements on mutator root layout:
  requires ScanStackInv($S, $Mem, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $Mem[esp], esp, ebp);
  requires StaticInv($toAbs, $SectionMem, $SectionAbs);

  modifies $Eip, $RET, eax, ebx, ecx, edx, esi, edi, ebp, esp;
  modifies $toAbs, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $stackState, $r1, $r2, $freshAbs, $Time, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $SectionMem;

  ensures  SMemEnsure($sMem, old($sMem), esp, old(esp));
  ensures  StaticInv($toAbs, $SectionMem, $SectionAbs);
  ensures  StackInvS($S, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets);
  ensures  NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  ensures  WellFormed($toAbs);
  ensures  ebp == old(ebp);
  ensures  esp == old(esp) + 4;

// Prepare to call Load (don't actually call it -- let the mutator call it)
// This procedure executes no instructions (it is not allowed to modify $Eip)
procedure readField($ptr:int, $fld:int) returns ($val:int);
  requires isStack($S) && $StackState[$S] == StackRunning;
  requires NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  requires Pointer($toAbs, $ptr, $toAbs[$ptr]);
  requires 0 <= $fld && $fld < numFields($toAbs[$ptr]);
  ensures  gcAddr($ptr + 4 * $fld);
  ensures  $val == $Mem[$ptr + 4 * $fld];
  ensures  Value(VFieldPtr($toAbs[$ptr], $fld), $toAbs, $val, $AbsMem[$toAbs[$ptr]][$fld]);

// Prepare to call Store (don't actually call it -- let the mutator call it)
// This procedure executes no instructions (it is not allowed to modify $Eip)
procedure writeField($ptr:int, $fld:int, $val:int, $abs:int) returns ($_mem:[int]int, $_absMem:[int][int]int);
  requires isStack($S) && $StackState[$S] == StackRunning;
  requires NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  requires Pointer($toAbs, $ptr, $toAbs[$ptr]);
  requires 0 <= $fld && $fld < numFields($toAbs[$ptr]);
  requires !isReadonlyField(tag($AbsMem[$toAbs[$ptr]][1]), $fld);
  requires Value(VFieldPtr($toAbs[$ptr], $fld), $toAbs, $val, $abs);
  modifies CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $stackState, $r1, $r2, $freshAbs, $Time, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem; // ... but don't modify $Mem
  ensures  gcAddr($ptr + 4 * $fld);
  ensures  word($val);
  ensures  $_mem == $Mem[$ptr + 4 * $fld := $val];
  ensures  $_absMem == $AbsMem[$toAbs[$ptr] := $AbsMem[$toAbs[$ptr]][$fld := $abs]];
  ensures  NucleusInv($S, $StackState, $toAbs, $_absMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $_mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);

// Prepare to call Load (don't actually call it -- let the mutator call it)
// This procedure executes no instructions (it is not allowed to modify $Eip)
procedure readStack($ptr:int, $frame:int, $j:int) returns ($val:int);
  requires isStack($S) && $StackState[$S] == StackRunning;
  requires NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  requires 0 <= $frame && $frame < $FrameCounts[$S];
  requires $FrameSlices[$S][$ptr] == $frame;
  requires $ptr == $FrameAddrs[$S][$frame] + 4 * $j; // REVIEW: redundant?
  ensures  memAddr($ptr);
  ensures  !gcAddrEx($ptr);
  ensures  $val == $Mem[$ptr];
  ensures  InteriorValue(frameHasPtr($FrameLayouts[$S][$frame], $j), $toAbs, $val, $FrameAbss[$S][$frame][$j], $FrameOffsets[$S][$ptr]);

// Prepare to call Store (don't actually call it -- let the mutator call it)
// This procedure executes no instructions (it is not allowed to modify $Eip)
procedure writeStack(
    $ptr:int, $val:int, $frame:int, $j:int, $fX:[int]int, $jX:[int]int,
    $_frameCount:int, $_frameAddr:[int]int, $_frameLayout:[int]$FrameLayout,
    $_frameSlice:[int]int, $_frameAbs:[int][int]int, $_frameOffset:[int]int)
    returns ($_mem:[int]int);
  requires isStack($S) && $StackState[$S] == StackRunning;
  requires NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi,
            $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  requires $_frameSlice[$ptr] == $frame;
  requires $ptr == $_frameAddr[$frame] + 4 * $j;
  requires StackLo($S) <= $ptr && $ptr < StackHi($S);
  requires Aligned($ptr);
  requires InteriorValue(frameHasPtr($_frameLayout[$frame], $j), $toAbs, $val, $_frameAbs[$frame][$j], $_frameOffset[$ptr]);
  // REVIEW: this requirement is messy:
  // All stack slots in the new stack, except for $ptr, map to an identical slot in the old stack:
  requires (forall f:int,j:int::{TV(f),TO(j)} TV(f) && TO(j) ==>
             0 <= f && f < $_frameCount && $_frameSlice[$_frameAddr[f] + 4 * j] == f ==>
             $_frameAddr[f] + 4 * j != $ptr ==>
                  0 <= $fX[f] && $fX[f] < $FrameCounts[$S]
               && $FrameSlices[$S][$FrameAddrs[$S][$fX[f]] + 4 * $jX[j]] == $fX[f]
               && $_frameAddr[f] + 4 * j == $FrameAddrs[$S][$fX[f]] + 4 * $jX[j]
               && $_frameAbs[f][j] == $FrameAbss[$S][$fX[f]][$jX[j]]
               && frameHasPtr($_frameLayout[f], j) == frameHasPtr($FrameLayouts[$S][$fX[f]], $jX[j])
               && $_frameOffset[$_frameAddr[f] + 4 * j] == $FrameOffsets[$S][$FrameAddrs[$S][$fX[f]] + 4 * $jX[j]]);
  modifies $fMems; /// ... but don't modify $Mem
  ensures  $_mem == $Mem[$ptr := $val];
  ensures  memAddr($ptr);
  ensures  !gcAddrEx($ptr);
  ensures  word($val);
  ensures  NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $_mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi,
            $FrameCounts [$S := $_frameCount],
            $FrameAddrs  [$S := $_frameAddr],
            $FrameLayouts[$S := $_frameLayout],
            $FrameSlices [$S := $_frameSlice],
            $FrameAbss   [$S := $_frameAbs],
            $FrameOffsets[$S := $_frameOffset],
            $IoMmuEnabled, $PciConfigState, DmaAddr);

procedure AllocObject($abs:int, $vt:int);
  // GC invariant:
  requires isStack($S) && $StackState[$S] == StackRunning;
  requires NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  requires SpRequire($S, esp, 4);
  requires ReturnToAddr($Mem[esp]) == $RET;

  // requirements on mutator root layout:
  requires ScanStackInv($S, $Mem, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $Mem[esp], esp, ebp);
  requires StaticInv($toAbs, $SectionMem, $SectionAbs);

  // requirements on vtable and layout:
  requires ecx == $vt;
  requires word($vt) && !gcAddrEx($vt);
  requires VTable($abs, $vt);
  requires ObjSize($abs, $vt, 0, 0);
  requires !isVarSize(tag($vt));

  // require a fresh, empty abstract node:
  requires $abs != NO_ABS;
  requires (forall i:int::{TV(i)} TV(i) ==> gcAddr(i) ==> $toAbs[i] != $abs);
  requires $AbsMem[$abs][0] == NULL;
  requires $AbsMem[$abs][1] == $vt;
  requires (forall j:int::{TO(j)} TO(j) ==> 2 <= j && j < numFields($abs) ==> $AbsMem[$abs][j] == NULL);

  modifies $Eip, $RET, eax, ebx, ecx, edx, esi, edi, ebp, esp;
  modifies $toAbs, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $stackState, $r1, $r2, $freshAbs, $Time, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $SectionMem;

  ensures  SMemEnsure($sMem, old($sMem), esp, old(esp));
  ensures  StaticInv($toAbs, $SectionMem, $SectionAbs);
  ensures  StackInvS($S, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets);
  ensures  NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  ensures  eax == 0 || Pointer($toAbs, eax - 4, $abs);
  ensures  WellFormed($toAbs);
  ensures  ebp == old(ebp);
  ensures  esp == old(esp) + 4;

procedure AllocString($abs:int, $vt:int, $nElems:int);
  // GC invariant:
  requires isStack($S) && $StackState[$S] == StackRunning;
  requires NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  requires SpRequire($S, esp, 4);
  requires ReturnToAddr($Mem[esp]) == $RET;

  // requirements on mutator root layout:
  requires ScanStackInv($S, $Mem, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $Mem[esp], esp, ebp);
  requires StaticInv($toAbs, $SectionMem, $SectionAbs);

  // requirements on vtable and layout:
  requires ecx == $nElems - 1;
  requires $vt == ?STRING_VTABLE;
  requires word($vt) && !gcAddrEx($vt);
  requires word($nElems);
  requires VTable($abs, $vt);
  requires ObjSize($abs, $vt, $nElems, 0);
  requires tag($vt) == ?STRING_TAG;

  // require a fresh, empty abstract node:
  requires $abs != NO_ABS;
  requires (forall i:int::{TV(i)} TV(i) ==> gcAddr(i) ==> $toAbs[i] != $abs);
  requires $AbsMem[$abs][0] == NULL;
  requires $AbsMem[$abs][1] == $vt;
  requires $AbsMem[$abs][2] == $nElems;
  requires $AbsMem[$abs][3] == $nElems - 1;
  requires (forall j:int::{TO(j)} TO(j) ==> 4 <= j && 4 * j < pad(16 + 2 * $nElems) ==> $AbsMem[$abs][j] == NULL);

  modifies $Eip, $RET, eax, ebx, ecx, edx, esi, edi, ebp, esp;
  modifies $toAbs, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $stackState, $r1, $r2, $freshAbs, $Time, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $SectionMem;

  ensures  SMemEnsure($sMem, old($sMem), esp, old(esp));
  ensures  StaticInv($toAbs, $SectionMem, $SectionAbs);
  ensures  StackInvS($S, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets);
  ensures  NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  ensures  eax == 0 || Pointer($toAbs, eax - 4, $abs);
  ensures  WellFormed($toAbs);
  ensures  ebp == old(ebp);
  ensures  esp == old(esp) + 4;

procedure AllocArray($abs:int, $vt:int, $rank:int, $nElems:int);
  // GC invariant:
  requires isStack($S) && $StackState[$S] == StackRunning;
  requires NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  requires SpRequire($S, esp, 8);
  requires ReturnToAddr($Mem[esp]) == $RET;
  requires $FrameSlices[$S][esp + 4] == $FrameCounts[$S];

  // requirements on mutator root layout:
  requires ScanStackInv($S, $Mem, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $Mem[esp], esp, ebp);
  requires StaticInv($toAbs, $SectionMem, $SectionAbs);

  // requirements on vtable and layout:
  requires ecx == $vt;
  requires edx == $rank;
  requires $FrameSlices[$S][esp + 4] == $FrameCounts[$S] && $fMems[$S][esp + 4] == $nElems;
  requires word($vt) && !gcAddrEx($vt);
  requires word($rank);
  requires word($nElems);
  requires VTable($abs, $vt);
  requires ObjSize($abs, $vt, $rank, $nElems);
  requires tag($vt) == ?PTR_ARRAY_TAG || tag($vt) == ?OTHER_ARRAY_TAG;

  // require a fresh, empty abstract node:
  requires $abs != NO_ABS;
  requires (forall i:int::{TV(i)} TV(i) ==> gcAddr(i) ==> $toAbs[i] != $abs);
  requires $AbsMem[$abs][0] == NULL;
  requires $AbsMem[$abs][1] == $vt;
  requires $AbsMem[$abs][2] == $rank;
  requires $AbsMem[$abs][3] == $nElems;
  requires (forall j:int::{TO(j)} TO(j) ==> 4 <= j && j < numFields($abs) ==> $AbsMem[$abs][j] == NULL);

  modifies $Eip, $RET, eax, ebx, ecx, edx, esi, edi, ebp, esp;
  modifies $toAbs, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $stackState, $r1, $r2, $freshAbs, $Time, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $SectionMem;

  ensures  SMemEnsure($sMem, old($sMem), esp, old(esp));
  ensures  StaticInv($toAbs, $SectionMem, $SectionAbs);
  ensures  StackInvS($S, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets);
  ensures  NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  ensures  eax == 0 || Pointer($toAbs, eax - 4, $abs);
  ensures  WellFormed($toAbs);
  ensures  ebp == old(ebp);
  ensures  esp == old(esp) + 4;

procedure AllocVector($abs:int, $vt:int, $nElems:int);
  // GC invariant:
  requires isStack($S) && $StackState[$S] == StackRunning;
  requires NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  requires SpRequire($S, esp, 4);
  requires ReturnToAddr($Mem[esp]) == $RET;

  // requirements on mutator root layout:
  requires ScanStackInv($S, $Mem, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $Mem[esp], esp, ebp);
  requires StaticInv($toAbs, $SectionMem, $SectionAbs);

  // requirements on vtable and layout:
  requires ecx == $vt;
  requires edx == $nElems;
  requires word($vt) && !gcAddrEx($vt);
  requires word($nElems);
  requires VTable($abs, $vt);
  requires ObjSize($abs, $vt, $nElems, 0);
  requires tag($vt) == ?PTR_VECTOR_TAG || tag($vt) == ?OTHER_VECTOR_TAG;

  // require a fresh, empty abstract node:
  requires $abs != NO_ABS;
  requires (forall i:int::{TV(i)} TV(i) ==> gcAddr(i) ==> $toAbs[i] != $abs);
  requires $AbsMem[$abs][0] == NULL;
  requires $AbsMem[$abs][1] == $vt;
  requires $AbsMem[$abs][2] == $nElems;
  requires (forall j:int::{TO(j)} TO(j) ==> 3 <= j && j < numFields($abs) ==> $AbsMem[$abs][j] == NULL);

  modifies $Eip, $RET, eax, ebx, ecx, edx, esi, edi, ebp, esp;
  modifies $toAbs, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $stackState, $r1, $r2, $freshAbs, $Time, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $SectionMem;

  ensures  SMemEnsure($sMem, old($sMem), esp, old(esp));
  ensures  StaticInv($toAbs, $SectionMem, $SectionAbs);
  ensures  StackInvS($S, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets);
  ensures  NucleusInv($S, $StackState, $toAbs, $AbsMem, CurrentStack, $gcSlice, $color, StackTop, $fs, $fn, CachePtr, CacheSize, ColorBase, HeapLo, HeapHi, ReserveMin, $Mem, $sMem, $dMem, $pciMem, $tMems, $fMems, $gcMem, SLo, DLo, PciLo, TLo, FLo, GcLo, GcHi, $FrameCounts, $FrameAddrs, $FrameLayouts, $FrameSlices, $FrameAbss, $FrameOffsets, $IoMmuEnabled, $PciConfigState, DmaAddr);
  ensures  eax == 0 || Pointer($toAbs, eax - 4, $abs);
  ensures  WellFormed($toAbs);
  ensures  ebp == old(ebp);
  ensures  esp == old(esp) + 4;









var $color:[int]int;
var StackTop:int;
var $fs:[int]int;
var $fn:[int]int;
var CachePtr:int;
var CacheSize:int;
var ColorBase:int;
var HeapLo:int;
var HeapHi:int;
var ReserveMin:int;
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
modifies CurrentStack,$gcSlice,$color,StackTop,$fs,$fn,CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,$stackState,$r1,$r2,$freshAbs,$Time,$Mem,$sMem,$dMem,$pciMem,$tMems,$fMems,$gcMem,SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi;
ensures SMemEnsure(($sMem),old($sMem),esp,old(esp));
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures ebp==old(ebp);
ensures WellFormed(($toAbs));

procedure revealInv1($s:int,$_stackState:[int]StackState);
requires isStack(($s));
requires NucleusInv(($S),($_stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures ($S)==CurrentStack;
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures IoInv(($IoMmuEnabled),($PciConfigState),DmaAddr,($pciMem));
ensures StackStateTag(($_stackState)[($s)])==StackTag(($s),($tMems));
ensures IsYielded(($_stackState)[($s)])==>($_stackState)[($s)]==StackYielded(StackEbp(($s),($tMems)),StackEsp(($s),($tMems))+4,StackRA(($s),($tMems),($fMems)));


procedure updateInv1($oldPciConfigState:[int]int,$oldPciMem:[int]int,$oldMem:[int]int);
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($oldMem),($sMem),($dMem),($oldPciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($oldPciConfigState),DmaAddr);


requires MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
requires IoInv(($IoMmuEnabled),($PciConfigState),DmaAddr,($pciMem));
ensures NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);

procedure setStackEmpty($s:int);
requires Aligned(eax);
requires eax==?tLo+($s)*?TSize;
requires isStack(($s));
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
modifies $Eip;
modifies $Mem,$fMems,$tMems,$stackState;
ensures NucleusInv(($S),($StackState)[($s):=StackEmpty],($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures ($Mem)==old($Mem)[eax:=?STACK_EMPTY];

procedure setStackYielded();
requires Aligned(esp);
requires fAddr(($S),esp);
requires Aligned(eax);
requires eax==?tLo+($S)*?TSize;
requires word(ebp)&&word(esp);
requires isStack(($S))&&($StackState)[($S)]==StackRunning;
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
requires ScanStackInv(($S),($Mem),($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($Mem)[esp],esp,ebp);
modifies $Eip;
modifies $Mem,$fMems,$tMems,$stackState;
ensures NucleusInv(($S),($StackState)[($S):=StackYielded(ebp,esp+4,($Mem)[esp])],($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
ensures ($Mem)==((old($Mem)[eax:=?STACK_YIELDED])[(eax+4):=esp])[(eax+8):=ebp];

procedure setStackInterrupted();
requires (((((word(eax)&&word(ebx))&&word(ecx))&&word(edx))&&word(esi))&&word(edi))&&word(ebp);
requires Aligned(esp);
requires fAddr(($S),esp)&&fAddr(($S),esp+8);
requires word(ebp)&&word(esp);
requires isStack(($S))&&($StackState)[($S)]==StackRunning;
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
modifies $Eip,eax,edx;
modifies $Mem,$fMems,$tMems,$stackState;
ensures NucleusInv(($S),($StackState)[($S):=StackInterrupted(old(eax),ebx,ecx,old(edx),esi,edi,ebp,esp+12,old($Mem)[esp],old($Mem)[esp+4],old($Mem)[esp+8])],($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);



ensures MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);

procedure setStackRunning1($s:int);
requires isStack(($s));
requires Aligned(esp);
requires Aligned(esp+4);
requires Aligned(edi);
requires esp==StackHi(($s))-8;
requires edi==?tLo+($s)*?TSize;
requires word(ecx);
requires NucleusInv(($S),($StackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
requires ($FrameCounts)[($s)]==0;
modifies $Eip;
modifies $Mem,$fMems,$tMems;
ensures ($Mem)[esp+0]==ecx;
ensures NucleusInv(($S),($StackState)[($s):=StackRunning],($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);

procedure setStackRunning2($s:int,$_stackState:[int]StackState);
requires ecx==($s);
requires isStack(($s));
requires Aligned(esp);
requires Aligned(esp+4);
requires Aligned(edi);
requires esp==StackHi(($s))-8;
requires edi==?tLo+($s)*?TSize;
requires word(eax);
requires NucleusInv(($S),($_stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
requires ($FrameCounts)[($s)]==0;
modifies $Eip;
modifies $Mem,$fMems,$tMems,CurrentStack;
ensures ($Mem)[esp+0]==eax;
ensures NucleusInv(($s),($_stackState)[($s):=StackRunning],($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);

procedure setStackRunning3($s:int,$_stackState:[int]StackState,$ebp:int,$esp:int,$eip:int);
requires ecx==($s);
requires isStack(($s))&&($_stackState)[($s)]==StackYielded(($ebp),($esp),($eip));
requires Aligned(edi);
requires edi==?tLo+($s)*?TSize;
requires NucleusInv(($S),($_stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,esp;
modifies $Mem,$fMems,$tMems,CurrentStack;
ensures NucleusInv(($s),($_stackState)[($s):=StackRunning],($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures esp==($esp)-4;
ensures ebp==($ebp);
ensures Aligned(esp);
ensures ($Mem)[($esp)-4]==($eip);

procedure setStackRunning4($s:int,$_stackState:[int]StackState,$eax:int,$ebx:int,$ecx:int,$edx:int,$esi:int,$edi:int,$ebp:int,$esp:int,$eip:int,$cs:int,$efl:int);

requires ecx==($s);
requires isStack(($s))&&($_stackState)[($s)]==StackInterrupted(($eax),($ebx),($ecx),($edx),($esi),($edi),($ebp),($esp),($eip),($cs),($efl));
requires Aligned(edi);
requires edi==?tLo+($s)*?TSize;
requires NucleusInv(($S),($_stackState),($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp,esp;
modifies $Mem,$fMems,$tMems,CurrentStack;
ensures NucleusInv(($s),($_stackState)[($s):=StackRunning],($toAbs),($AbsMem),CurrentStack,($gcSlice),($color),StackTop,($fs),($fn),CachePtr,CacheSize,ColorBase,HeapLo,HeapHi,ReserveMin,($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi,($FrameCounts),($FrameAddrs),($FrameLayouts),($FrameSlices),($FrameAbss),($FrameOffsets),($IoMmuEnabled),($PciConfigState),DmaAddr);
ensures esp==($esp)-12;
ensures (((((eax==($eax)&&ebx==($ebx))&&ecx==($ecx))&&edx==($edx))&&esi==($esi))&&edi==($edi))&&ebp==($ebp);
ensures Aligned(esp);
ensures ($Mem)[($esp)-4]==($efl);
ensures ($Mem)[($esp)-8]==($cs);
ensures ($Mem)[($esp)-12]==($eip);

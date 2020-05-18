;;parsing ..\..\..\src\Trusted\Spec\base_i.bpl
;;parsing ..\..\..\src\Trusted\Spec\base.bpl
;;parsing ..\..\..\src\Trusted\Spec\memory_i.bpl
;;parsing ..\..\..\src\Trusted\Spec\memory.bpl
;;parsing ..\..\..\src\Trusted\Spec\assembly_i.bpl
;;parsing ..\..\..\src\Trusted\Spec\assembly.bpl
;;parsing ..\..\..\src\Trusted\Spec\stacks_i.bpl
;;parsing ..\..\..\src\Trusted\Spec\stacks.bpl
;;parsing ..\..\..\src\Trusted\Spec\interrupts_i.bpl
;;parsing ..\..\..\src\Trusted\Spec\interrupts.bpl
;;parsing ..\..\..\src\Trusted\Spec\io_i.bpl
;;parsing ..\..\..\src\Trusted\Spec\io.bpl
;;parsing ..\..\..\src\Trusted\Spec\bartok_i.bpl
;;parsing ..\..\..\src\Trusted\Spec\bartok.bpl
;;parsing ..\..\..\src\Trusted\Spec\BitVectors_i.bpl
;;parsing ..\..\..\src\Trusted\Spec\BitVectors.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\Base\Util_i.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\Base\Util.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\Base\Overflow_i.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\Base\Overflow.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\Base\Separation_i.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\Base\Separation.bpl
;;parsing GC\BitVectorsBuiltin_i.bpl
;;parsing GC\BitVectorsBuiltin.bpl
;;parsing GC\BitVectors_i.bpl
;;parsing GC\BitVectors.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\GC\Reach_i.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\GC\Reach.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\GC\Common_i.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\GC\Common.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\GC\CopyingCollector_i.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\GC\CopyingCollector.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\Main\EntryCP_i.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\Main\EntryCP.bpl
.686p
.model flat
include trusted.inc
NULL EQU 0
?NumStacks EQU 64
?InitialStack EQU 0
?InterruptStack EQU 0
?InterruptReserve EQU 16
?StackReserve EQU 4096
.data
align 4
_$$StackCheck DD 0
?STACK_EMPTY EQU 0
?STACK_RUNNING EQU 1
?STACK_YIELDED EQU 2
?STACK_INTERRUPTED EQU 3
?NIdt EQU 256
?CodeSegmentSelector EQU 32
?Mask16Lo EQU 65535
?IdtWord4Lo EQU 36352
?VgaTextLo EQU 753664
?VgaTextHi EQU 761664
?RoBiosLo EQU 917504
?RoBiosHi EQU 1048576
?SPARSE_TAG EQU 1
?DENSE_TAG EQU 3
?PTR_VECTOR_TAG EQU 5
?OTHER_VECTOR_TAG EQU 7
?PTR_ARRAY_TAG EQU 9
?OTHER_ARRAY_TAG EQU 11
?STRING_TAG EQU 13
?VT_MASK EQU 60
?VT_BASE_LENGTH EQU 52
?VT_ARRAY_ELEMENT_SIZE EQU 44
?VT_ARRAY_ELEMENT_CLASS EQU 40
?VT_ARRAY_OF EQU 36
?TYPE_STRUCT EQU 3
.data
align 4
.code
align 16
_?DebugBreak proc
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3$__L1
    add ecx, 3120
    jmp $$3$__L2
  $$3$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4$__L1
    add ecx, 3120
    jmp $$4$__L2
  $$4$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$5$__L1
    add ecx, 3120
    jmp $$5$__L2
  $$5$__L1:
    add ecx, 3127
    jc _?Overflow
  $$5$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$6$__L1
    add ecx, 3120
    jmp $$6$__L2
  $$6$__L1:
    add ecx, 3127
    jc _?Overflow
  $$6$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$7$__L1
    add ecx, 3120
    jmp $$7$__L2
  $$7$__L1:
    add ecx, 3127
    jc _?Overflow
  $$7$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$8$__L1
    add ecx, 3120
    jmp $$8$__L2
  $$8$__L1:
    add ecx, 3127
    jc _?Overflow
  $$8$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$9$__L1
    add ecx, 3120
    jmp $$9$__L2
  $$9$__L1:
    add ecx, 3127
    jc _?Overflow
  $$9$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$10$__L1
    add ecx, 3120
    jmp $$10$__L2
  $$10$__L1:
    add ecx, 3127
    jc _?Overflow
  $$10$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1$loop
    ret
_?DebugBreak endp
.data
align 4
.code
align 16
_?Overflow proc
    mov eax, 1431638017
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$13$__L1
    add ecx, 3120
    jmp $$13$__L2
  $$13$__L1:
    add ecx, 3127
    jc _?Overflow
  $$13$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$14$__L1
    add ecx, 3120
    jmp $$14$__L2
  $$14$__L1:
    add ecx, 3127
    jc _?Overflow
  $$14$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$15$__L1
    add ecx, 3120
    jmp $$15$__L2
  $$15$__L1:
    add ecx, 3127
    jc _?Overflow
  $$15$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$16$__L1
    add ecx, 3120
    jmp $$16$__L2
  $$16$__L1:
    add ecx, 3127
    jc _?Overflow
  $$16$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$17$__L1
    add ecx, 3120
    jmp $$17$__L2
  $$17$__L1:
    add ecx, 3127
    jc _?Overflow
  $$17$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$18$__L1
    add ecx, 3120
    jmp $$18$__L2
  $$18$__L1:
    add ecx, 3127
    jc _?Overflow
  $$18$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$19$__L1
    add ecx, 3120
    jmp $$19$__L2
  $$19$__L1:
    add ecx, 3127
    jc _?Overflow
  $$19$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$20$__L1
    add ecx, 3120
    jmp $$20$__L2
  $$20$__L1:
    add ecx, 3127
    jc _?Overflow
  $$20$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$11$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$11$loop
    ret
_?Overflow endp
.data
align 4
_$$SLo DD 0
.data
align 4
_$$DLo DD 0
.data
align 4
_$$PciLo DD 0
.data
align 4
_$$TLo DD 0
.data
align 4
_$$FLo DD 0
.data
align 4
_$$GcLo DD 0
.data
align 4
_$$GcHi DD 0
.data
align 4
_$$DmaAddr DD 0
?SSize EQU 1024
?DSize EQU 1024
?TSize EQU 256
?FSize EQU 16384
.data
align 4
_$$CurrentStack DD 0
.data
align 4
.code
align 16
_?TableSearch proc
    cmp edx, 0
    jae TableSearch$skip1
    mov edx, 0
.data
align 4
.code
    ret
  TableSearch$skip1:
    mov eax, dword ptr [ecx]
    cmp ebp, eax
    jae TableSearch$skip2
    mov edx, 0
.data
align 4
.code
    ret
  TableSearch$skip2:
    mov eax, dword ptr [ecx + 4 * edx + 0]
    cmp ebp, eax
    jb TableSearch$skip3
    mov edx, 0
.data
align 4
.code
    ret
  TableSearch$skip3:
    mov esi, 0
    mov edi, edx
  TableSearch$loop:
    lea eax, dword ptr [esi + 1]
    cmp eax, edi
    jae TableSearch$loopEnd
    lea ebx, dword ptr [esi + 1 * edi + 0]
    shr ebx, 1
    cmp ebx, esi
    jbe TableSearch$do4
    cmp ebx, edi
    jae TableSearch$do4
    jmp TableSearch$skip4
  TableSearch$do4:
    lea ebx, dword ptr [esi + 1]
  TableSearch$skip4:
    mov eax, dword ptr [ecx + 4 * ebx + 0]
    cmp eax, ebp
    jbe TableSearch$do5
    mov edi, ebx
    jmp TableSearch$skip5
  TableSearch$do5:
    mov esi, ebx
  TableSearch$skip5:
    jmp TableSearch$loop
  TableSearch$loopEnd:
    mov eax, esi
    mov ebx, dword ptr [ecx + 4 * eax + 0]
    cmp ebp, ebx
    jb TableSearch$skip6
    mov ebx, dword ptr [ecx + 4 * eax + 4]
    cmp ebp, ebx
    jae TableSearch$skip6
    mov edx, 1
.data
align 4
.code
    ret
  TableSearch$skip6:
    mov edx, 0
.data
align 4
.code
    ret
_?TableSearch endp
.data
align 4
__$TablesSearch$tmp2 DD 0
__$TablesSearch$tmp1 DD 0
__$TablesSearch$tableBase DD 0
__$TablesSearch$index DD 0
__$TablesSearch$table DD 0
__$TablesSearch$nextFp DD 0
__$TablesSearch$ra DD 0
.code
align 16
_?TablesSearch proc
    mov dword ptr __$TablesSearch$ra, ecx
    mov dword ptr __$TablesSearch$nextFp, edx
    mov dword ptr __$TablesSearch$table, 0
    mov edx, 0
  TablesSearch$loop:
    mov eax, dword ptr __$TablesSearch$table
    cmp dword ptr _$$?callSiteTableCount, eax
    jbe TablesSearch$loopEnd
    mov ebx, dword ptr _$$?returnAddressToCallSiteSetNumbers
    mov ecx, dword ptr [ebx + 4 * eax + 0]
    mov ebx, dword ptr _$$?callSiteSetCount
    mov edx, dword ptr [ebx + 4 * eax + 0]
    mov edx, dword ptr [edx]
    mov ebx, dword ptr _$$?codeBaseStartTable
    mov esi, dword ptr [ebx + 4 * eax + 0]
    mov ebp, dword ptr __$TablesSearch$ra
    mov eax, dword ptr [ecx]
    cmp esi, ebp
    ja TablesSearch$skip1
    sub ebp, esi
.data
align 4
.code
    call _?TableSearch
    mov dword ptr __$TablesSearch$index, eax
    cmp edx, 0
    je TablesSearch$skip1
    mov esi, dword ptr __$TablesSearch$table
    mov edi, dword ptr _$$?activationDescriptorTable
    mov ecx, dword ptr [edi + 4 * esi + 0]
    mov edi, dword ptr _$$?callSiteSetNumberToIndex
    mov ebp, dword ptr [edi + 4 * esi + 0]
    movzx ebp, word ptr [ebp + 2 * eax + 0]
    mov eax, dword ptr [ecx + 4 * ebp + 0]
.data
align 4
.code
    ret
  TablesSearch$skip1:
    add dword ptr __$TablesSearch$table, 1
    jc _?Overflow
    mov edx, 0
    jmp TablesSearch$loop
  TablesSearch$loopEnd:
.data
align 4
.code
    ret
_?TablesSearch endp
.data
align 4
.code
align 16
_?GetSize proc
.data
align 4
.code
    mov ebp, dword ptr [edx + ?VT_MASK]
    and ebp, 15
    cmp ebp, ?SPARSE_TAG
    jne $$29$skip1
    mov eax, dword ptr [edx + ?VT_BASE_LENGTH]
    jmp $$29$end
  $$29$skip1:
    cmp ebp, ?DENSE_TAG
    jne $$29$skip2
    mov eax, dword ptr [edx + ?VT_BASE_LENGTH]
    jmp $$29$end
  $$29$skip2:
    cmp ebp, ?STRING_TAG
    jne $$29$skip3
.data
align 4
.code
    mov esi, dword ptr [ecx + 8]
    mov eax, esi
    add eax, eax
    jc _?Overflow
    add eax, 19
    jc _?Overflow
    mov ebx, 3
    not ebx
    and eax, ebx
    jmp $$29$end
  $$29$skip3:
    cmp ebp, ?PTR_VECTOR_TAG
    jne $$29$skip4
.data
align 4
.code
    mov esi, dword ptr [ecx + 8]
    mov eax, dword ptr [edx + ?VT_BASE_LENGTH]
    add esi, esi
    jc _?Overflow
    add esi, esi
    jc _?Overflow
    add eax, esi
    jc _?Overflow
    add eax, 3
    jc _?Overflow
    mov ebx, 3
    not ebx
    and eax, ebx
    jmp $$29$end
  $$29$skip4:
    cmp ebp, ?OTHER_VECTOR_TAG
    jne $$29$skip5
.data
align 4
.code
    mov esi, dword ptr [ecx + 8]
    mov eax, dword ptr [edx + ?VT_BASE_LENGTH]
    mov ebx, dword ptr [edx + ?VT_ARRAY_ELEMENT_SIZE]
    mov ebp, eax
    mov eax, ebx
    mul esi
    jc _?Overflow
    add eax, ebp
    jc _?Overflow
    add eax, 3
    jc _?Overflow
    mov ebx, 3
    not ebx
    and eax, ebx
    jmp $$29$end
  $$29$skip5:
    cmp ebp, ?PTR_ARRAY_TAG
    jne $$29$skip6
.data
align 4
.code
    mov esi, dword ptr [ecx + 12]
    mov eax, dword ptr [edx + ?VT_BASE_LENGTH]
    add esi, esi
    jc _?Overflow
    add esi, esi
    jc _?Overflow
    add eax, esi
    jc _?Overflow
    add eax, 3
    jc _?Overflow
    mov ebx, 3
    not ebx
    and eax, ebx
    jmp $$29$end
  $$29$skip6:
    cmp ebp, ?OTHER_ARRAY_TAG
    jne $$29$skip7
.data
align 4
.code
    mov esi, dword ptr [ecx + 12]
    mov eax, dword ptr [edx + ?VT_BASE_LENGTH]
    mov ebx, dword ptr [edx + ?VT_ARRAY_ELEMENT_SIZE]
    mov ebp, eax
    mov eax, ebx
    mul esi
    jc _?Overflow
    add eax, ebp
    jc _?Overflow
    add eax, 3
    jc _?Overflow
    mov ebx, 3
    not ebx
    and eax, ebx
    jmp $$29$end
  $$29$skip7:
    mov eax, dword ptr [edx + ?VT_BASE_LENGTH]
  $$29$end:
.data
align 4
.code
    ret
_?GetSize endp
.data
align 4
_$$HeapLo DD 0
.data
align 4
_$$Fi DD 0
.data
align 4
_$$Fk DD 0
.data
align 4
_$$Fl DD 0
.data
align 4
_$$Ti DD 0
.data
align 4
_$$Tj DD 0
.data
align 4
_$$Tk DD 0
.data
align 4
_$$Tl DD 0
.data
align 4
_$$BF DD 0
.data
align 4
_$$BT DD 0
.data
align 4
.code
align 16
_?BB4Zero proc
    mov esi, eax
  BB4Zero$loop:
    cmp esi, ebx
    jae BB4Zero$loopEnd
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [esi], 0
    add esi, 4
    jmp BB4Zero$loop
  BB4Zero$loopEnd:
.data
align 4
.code
    ret
_?BB4Zero endp
.data
align 4
__$CopyAndForward$tmp DD 0
.code
align 16
_?CopyAndForward proc
.data
align 4
.code
    mov edx, dword ptr [ecx + 4]
.data
align 4
.code
    call _?GetSize
    mov ebp, eax
    mov esi, dword ptr _$$Tk
    add dword ptr _$$Tk, ebp
    jc _?Overflow
    mov eax, dword ptr _$$Tl
    cmp dword ptr _$$Tk, eax
    jbe CopyAndForward$skip1
    mov eax, 1431633969
.data
align 4
.code
    call _?DebugBreak
  CopyAndForward$skip1:
    mov edi, 0
    mov edx, 0
  CopyAndForward$loop:
    cmp edx, ebp
    jae CopyAndForward$loopEnd
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr [ecx + 4 * edi + 0]
.data
align 4
.code
    mov dword ptr [esi + 4 * edi + 0], eax
    add edi, 1
    add edx, 4
    jmp CopyAndForward$loop
  CopyAndForward$loopEnd:
    lea eax, dword ptr [esi + 4]
.data
align 4
.code
    mov dword ptr [ecx + 4], eax
    mov eax, esi
    sub esi, dword ptr _$$Ti
    mov edi, dword ptr _$$BT
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, esi
    shr esi, 7
    shr ecx, 2
    add esi, esi
    add esi, esi
    add esi, edi
    and ecx, 31
    mov edi, 1
    shl edi, cl
    mov ecx, edi
.data
align 4
.code
    mov edi, dword ptr [esi]
    or edi, ecx
.data
align 4
.code
    mov dword ptr [esi], edi
.data
align 4
.code
    ret
_?CopyAndForward endp
.data
align 4
__$FromInteriorPointer$save2 DD 0
__$FromInteriorPointer$save1 DD 0
.code
align 16
_?FromInteriorPointer proc
    mov eax, 0
  FromInteriorPointer$loop:
    mov ebx, ecx
    sub ebx, eax
    sub ebx, 4
    sub ebx, dword ptr _$$Fi
    mov edx, ebx
    and edx, 3
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    cmp edx, 0
    jne FromInteriorPointer$skip1
    mov dword ptr __$FromInteriorPointer$save1, eax
    mov dword ptr __$FromInteriorPointer$save2, ecx
    mov eax, dword ptr _$$BF
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov edx, ebx
    shr ebx, 7
    shr edx, 2
    add ebx, ebx
    add ebx, ebx
    add ebx, eax
.data
align 4
.code
    mov ebx, dword ptr [ebx]
    and edx, 31
    mov ecx, edx
    mov edx, 1
    shl edx, cl
    and ebx, edx
    mov eax, dword ptr __$FromInteriorPointer$save1
    mov ecx, dword ptr __$FromInteriorPointer$save2
    cmp ebx, 0
    je FromInteriorPointer$skip1
.data
align 4
.code
    ret
  FromInteriorPointer$skip1:
    add eax, 1
    jmp FromInteriorPointer$loop
_?FromInteriorPointer endp
.data
align 4
__$GarbageCollect$sp DD 0
__$GarbageCollect$tm DD 0
__$GarbageCollect$t DD 0
.code
align 16
_?GarbageCollect proc
    mov edx, ebp
.data
align 4
.code
    mov ecx, dword ptr [esp]
    mov eax, dword ptr _$$CurrentStack
    mov ebx, ?TSize
    mul ebx
    mov dword ptr __$GarbageCollect$t, eax
    mov eax, dword ptr _$$TLo
    add dword ptr __$GarbageCollect$t, eax
    mov ebx, dword ptr __$GarbageCollect$t
.data
align 4
.code
    mov dword ptr [ebx], ?STACK_YIELDED
.data
align 4
.code
    mov dword ptr [ebx + 4], esp
.data
align 4
.code
    mov dword ptr [ebx + 8], ebp
    mov dword ptr __$GarbageCollect$sp, esp
    mov esp, dword ptr _$$DLo
.data
align 4
.code
.data
align 4
__$$$320$saveEbp DD 0
.code
    mov dword ptr __$$$320$saveEbp, ebp
.data
align 4
.code
    mov eax, dword ptr _$$Ti
    mov dword ptr _$$Tj, eax
    mov dword ptr _$$Tk, eax
    mov eax, dword ptr _$$BT
    mov ebx, dword ptr _$$Fi
    cmp ebx, dword ptr _$$HeapLo
    jne $$320$skip1
    mov ebx, dword ptr _$$HeapLo
    jmp $$320$skip2
  $$320$skip1:
    mov ebx, dword ptr _$$BF
  $$320$skip2:
.data
align 4
.code
    call _?BB4Zero
.data
align 4
__$$$323$tag DD 0
__$$$323$t DD 0
__$$$323$s DD 0
.code
    mov dword ptr __$$$323$s, 0
    mov eax, dword ptr _$$TLo
    mov dword ptr __$$$323$t, eax
  $$323$__L1:
    mov eax, ?NumStacks
    cmp dword ptr __$$$323$s, eax
    jae $$323$__L2
    mov eax, dword ptr __$$$323$t
.data
align 4
.code
    mov eax, dword ptr [eax]
    mov dword ptr __$$$323$tag, eax
    mov eax, ?STACK_YIELDED
    cmp dword ptr __$$$323$tag, eax
    jne $$323$__L3
    mov ebx, dword ptr __$$$323$t
.data
align 4
.code
    mov ecx, dword ptr [ebx + 4]
.data
align 4
.code
    mov ecx, dword ptr [ecx]
.data
align 4
.code
    mov edx, dword ptr [ebx + 8]
.data
align 4
__$$$328$_nextFp DD 0
__$$$328$_ra DD 0
__$$$328$found DD 0
__$$$328$desc DD 0
__$$$328$addr DD 0
__$$$328$p DD 0
.code
    mov dword ptr __$$$328$_ra, ecx
    mov dword ptr __$$$328$_nextFp, edx
  $$328$loop:
    mov ecx, dword ptr __$$$328$_ra
    mov edx, dword ptr __$$$328$_nextFp
    cmp edx, 0
    je $$328$loopEnd
.data
align 4
.code
    call _?TablesSearch
    mov dword ptr __$$$328$desc, eax
    mov dword ptr __$$$328$found, edx
    mov ecx, dword ptr __$$$328$_nextFp
    mov dword ptr __$$$328$addr, ecx
.data
align 4
.code
    mov eax, dword ptr [ecx]
    mov dword ptr __$$$328$_nextFp, eax
.data
align 4
.code
    mov eax, dword ptr [ecx + 4]
    mov dword ptr __$$$328$_ra, eax
    mov eax, dword ptr __$$$328$desc
    shr eax, 0
    and eax, 1
    cmp eax, 1
    jne $$328$skip1
    mov eax, dword ptr __$$$328$desc
    shr eax, 1
    and eax, 1
    cmp eax, 1
    je $$328$skip1
    mov eax, dword ptr __$$$328$desc
    shr eax, 6
    and eax, 1023
    cmp eax, 0
    jne $$328$skip1
    mov ecx, dword ptr __$$$328$addr
    mov edx, dword ptr __$$$328$desc
.data
align 4
__$$$332$addrp DD 0
__$$$332$desc DD 0
__$$$332$addr DD 0
__$$$332$args DD 0
__$$$332$offset DD 0
__$$$332$v DD 0
__$$$332$b DD 0
.code
    mov dword ptr __$$$332$addr, ecx
    mov dword ptr __$$$332$desc, edx
    mov eax, edx
    shr eax, 2
    and eax, 15
    mov dword ptr __$$$332$args, eax
    mov dword ptr __$$$332$b, 0
    mov ebx, 0
    lea ebx, dword ptr [ebx + 4 * eax + 4]
    add ebx, ecx
    jc _?Overflow
    mov dword ptr __$$$332$addrp, ebx
  $$332$loop1:
    mov eax, dword ptr __$$$332$b
    cmp eax, dword ptr __$$$332$args
    jae $$332$loopEnd1
    lea ecx, dword ptr [eax + 16]
    mov ebx, dword ptr __$$$332$desc
    shr ebx, cl
    and ebx, 1
    cmp ebx, 1
    jne $$332$skip1
    mov ecx, dword ptr __$$$332$addrp
.data
align 4
__$$$333$offset DD 0
__$$$333$v DD 0
__$$$333$save1 DD 0
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp eax, dword ptr _$$GcLo
    jb $$333$skip1
    cmp eax, dword ptr _$$GcHi
    ja $$333$skip1
    mov dword ptr __$$$333$save1, ecx
    mov dword ptr __$$$333$v, eax
    mov ecx, eax
.data
align 4
.code
    call _?FromInteriorPointer
    mov dword ptr __$$$333$offset, eax
    mov ecx, dword ptr __$$$333$v
    sub ecx, eax
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp dword ptr _$$GcLo, eax
    ja $$337$skip
    cmp dword ptr _$$GcHi, eax
    jae $$337$done
  $$337$skip:
    sub ecx, 4
.data
align 4
.code
    call _?CopyAndForward
    add eax, 4
  $$337$done:
    add eax, dword ptr __$$$333$offset
.data
align 4
.code
.data
align 4
.code
    mov ecx, dword ptr __$$$333$save1
.data
align 4
.code
    mov dword ptr [ecx], eax
  $$333$skip1:
  $$332$skip1:
    add dword ptr __$$$332$b, 1
    sub dword ptr __$$$332$addrp, 4
    jmp $$332$loop1
  $$332$loopEnd1:
    sub dword ptr __$$$332$addrp, 8
    jc _?Overflow
  $$332$loop2:
    mov eax, dword ptr __$$$332$b
    cmp eax, 16
    jae $$332$loopEnd2
    lea ecx, dword ptr [eax + 16]
    mov ebx, dword ptr __$$$332$desc
    shr ebx, cl
    and ebx, 1
    cmp ebx, 1
    jne $$332$skip2
    mov ecx, dword ptr __$$$332$addrp
.data
align 4
__$$$343$offset DD 0
__$$$343$v DD 0
__$$$343$save1 DD 0
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp eax, dword ptr _$$GcLo
    jb $$343$skip1
    cmp eax, dword ptr _$$GcHi
    ja $$343$skip1
    mov dword ptr __$$$343$save1, ecx
    mov dword ptr __$$$343$v, eax
    mov ecx, eax
.data
align 4
.code
    call _?FromInteriorPointer
    mov dword ptr __$$$343$offset, eax
    mov ecx, dword ptr __$$$343$v
    sub ecx, eax
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp dword ptr _$$GcLo, eax
    ja $$347$skip
    cmp dword ptr _$$GcHi, eax
    jae $$347$done
  $$347$skip:
    sub ecx, 4
.data
align 4
.code
    call _?CopyAndForward
    add eax, 4
  $$347$done:
    add eax, dword ptr __$$$343$offset
.data
align 4
.code
.data
align 4
.code
    mov ecx, dword ptr __$$$343$save1
.data
align 4
.code
    mov dword ptr [ecx], eax
  $$343$skip1:
  $$332$skip2:
    add dword ptr __$$$332$b, 1
    sub dword ptr __$$$332$addrp, 4
    jc _?Overflow
    jmp $$332$loop2
  $$332$loopEnd2:
    jmp $$328$loop
  $$328$skip1:
    mov eax, dword ptr __$$$328$desc
    shr eax, 0
    and eax, 1
    cmp eax, 1
    je $$328$skip2
    mov eax, dword ptr __$$$328$desc
    mov eax, dword ptr [eax]
    cmp eax, 4096
    jne $$328$skip2
    mov ecx, dword ptr __$$$328$addr
    mov edx, dword ptr __$$$328$desc
.data
align 4
__$$$353$count DD 0
__$$$353$offset DD 0
__$$$353$addrp DD 0
__$$$353$desc DD 0
__$$$353$addr DD 0
__$$$353$v DD 0
__$$$353$p DD 0
.code
    mov dword ptr __$$$353$addr, ecx
    mov dword ptr __$$$353$desc, edx
    mov dword ptr __$$$353$p, 0
    movzx eax, byte ptr [edx + 4]
    mov dword ptr __$$$353$count, eax
  $$353$loop:
    mov eax, dword ptr __$$$353$p
    cmp eax, dword ptr __$$$353$count
    jae $$353$loopEnd
    mov ebx, dword ptr __$$$353$desc
    mov esi, dword ptr __$$$353$addr
    movsx ebp, byte ptr [ebx + 1 * eax + 6]
    lea ebp, dword ptr [esi + 4 * ebp + 0]
    mov dword ptr __$$$353$addrp, ebp
.data
align 4
.code
    mov ecx, dword ptr [ebp]
    cmp ecx, dword ptr _$$GcLo
    jb $$353$skip1
    cmp ecx, dword ptr _$$GcHi
    ja $$353$skip1
    mov dword ptr __$$$353$v, ecx
.data
align 4
.code
    call _?FromInteriorPointer
    mov dword ptr __$$$353$offset, eax
    mov ecx, dword ptr __$$$353$v
    sub ecx, eax
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp dword ptr _$$GcLo, eax
    ja $$357$skip
    cmp dword ptr _$$GcHi, eax
    jae $$357$done
  $$357$skip:
    sub ecx, 4
.data
align 4
.code
    call _?CopyAndForward
    add eax, 4
  $$357$done:
    add eax, dword ptr __$$$353$offset
.data
align 4
.code
.data
align 4
.code
    mov ebx, dword ptr __$$$353$addrp
.data
align 4
.code
    mov dword ptr [ebx], eax
  $$353$skip1:
    add dword ptr __$$$353$p, 1
    jmp $$353$loop
  $$353$loopEnd:
    jmp $$328$loop
  $$328$skip2:
    mov eax, 1431633972
.data
align 4
.code
    call _?DebugBreak
  $$328$loopEnd:
  $$323$__L3:
    mov eax, ?STACK_INTERRUPTED
    cmp dword ptr __$$$323$tag, eax
    jne $$323$__L4
    mov eax, 1431633973
.data
align 4
.code
    call _?DebugBreak
  $$323$__L4:
    mov eax, ?STACK_RUNNING
    cmp dword ptr __$$$323$tag, eax
    jne $$323$__L5
    mov eax, 1431633974
.data
align 4
.code
    call _?DebugBreak
  $$323$__L5:
    add dword ptr __$$$323$s, 1
    mov eax, ?TSize
    add dword ptr __$$$323$t, eax
    jmp $$323$__L1
  $$323$__L2:
.data
align 4
__$$$366$section DD 0
.code
    mov dword ptr __$$$366$section, 0
  $$366$loop:
    mov ecx, dword ptr __$$$366$section
    cmp ecx, dword ptr _$$?sectionCount
    jae $$366$loopEnd
.data
align 4
__$$$367$save3 DD 0
__$$$367$save2 DD 0
__$$$367$save1 DD 0
__$$$367$section DD 0
__$$$367$addr DD 0
__$$$367$sEnd DD 0
.code
    mov dword ptr __$$$367$section, ecx
    mov eax, dword ptr _$$?dataSectionEnd
    mov eax, dword ptr [eax + 4 * ecx + 0]
    mov dword ptr __$$$367$sEnd, eax
    mov eax, dword ptr _$$?staticDataPointerBitMap
    mov edx, dword ptr [eax + 4 * ecx + 0]
    mov eax, dword ptr _$$?dataSectionBase
    mov eax, dword ptr [eax + 4 * ecx + 0]
    mov dword ptr __$$$367$addr, eax
    mov edi, eax
    mov esi, 0
  $$367$loop:
    cmp edi, dword ptr __$$$367$sEnd
    jae $$367$loopEnd
    mov eax, esi
    shr eax, 5
    mov eax, dword ptr [edx + 4 * eax + 0]
    mov ecx, esi
    and ecx, 31
    shr eax, cl
    and eax, 1
    cmp eax, 1
    jne $$367$skip1
    mov ecx, dword ptr [edi]
    cmp ecx, dword ptr _$$GcLo
    jb $$367$skip2
    cmp ecx, dword ptr _$$GcHi
    ja $$367$skip2
    mov dword ptr __$$$367$save1, edi
    mov dword ptr __$$$367$save2, esi
    mov dword ptr __$$$367$save3, edx
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp dword ptr _$$GcLo, eax
    ja $$369$skip
    cmp dword ptr _$$GcHi, eax
    jae $$369$done
  $$369$skip:
    sub ecx, 4
.data
align 4
.code
    call _?CopyAndForward
    add eax, 4
  $$369$done:
    mov edi, dword ptr __$$$367$save1
    mov esi, dword ptr __$$$367$save2
    mov edx, dword ptr __$$$367$save3
    mov dword ptr [edi], eax
  $$367$skip2:
  $$367$skip1:
    add esi, 1
    add edi, 4
    jc _?Overflow
    jmp $$367$loop
  $$367$loopEnd:
    add dword ptr __$$$366$section, 1
    jc _?Overflow
    jmp $$366$loop
  $$366$loopEnd:
.data
align 4
.code
  $$372$entry:
  $$372$loop:
    mov eax, dword ptr _$$Tk
    cmp dword ptr _$$Tj, eax
    jae $$372$exit
.data
align 4
.code
    mov ecx, dword ptr _$$Tj
.data
align 4
.code
    mov ecx, dword ptr [ecx + 4]
.data
align 4
.code
    mov eax, dword ptr [ecx + ?VT_MASK]
    and eax, 15
    cmp eax, ?SPARSE_TAG
    jne $$373$skip1
.data
align 4
__$$$376$save4 DD 0
__$$$376$save3 DD 0
__$$$376$save2 DD 0
__$$$376$save1 DD 0
.code
    mov esi, 1
    mov ebp, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov edx, dword ptr [ecx + ?VT_MASK]
    mov esi, 1
  $$376$loop:
    cmp esi, 8
    jae $$376$loopEnd
    mov ecx, 0
    lea ecx, dword ptr [ecx + 4 * esi + 0]
    mov ebx, edx
    shr ebx, cl
    and ebx, 15
    cmp ebx, 0
    je $$376$skip1
    mov eax, dword ptr _$$Tj
.data
align 4
.code
    mov ecx, dword ptr [eax + 4 * ebx + 4]
    cmp ecx, dword ptr _$$GcLo
    jb $$376$skip2
    cmp ecx, dword ptr _$$GcHi
    ja $$376$skip2
.data
align 4
.code
    mov dword ptr __$$$376$save1, esi
    mov dword ptr __$$$376$save2, ebp
    mov dword ptr __$$$376$save3, edx
    mov dword ptr __$$$376$save4, ebx
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp dword ptr _$$GcLo, eax
    ja $$379$skip
    cmp dword ptr _$$GcHi, eax
    jae $$379$done
  $$379$skip:
    sub ecx, 4
.data
align 4
.code
    call _?CopyAndForward
    add eax, 4
  $$379$done:
    mov esi, dword ptr __$$$376$save1
    mov ebp, dword ptr __$$$376$save2
    mov edx, dword ptr __$$$376$save3
    mov ebx, dword ptr __$$$376$save4
    mov ecx, dword ptr _$$Tj
.data
align 4
.code
    mov dword ptr [ecx + 4 * ebx + 4], eax
  $$376$skip2:
  $$376$skip1:
    add esi, 1
    jmp $$376$loop
  $$376$loopEnd:
    add dword ptr _$$Tj, ebp
    jmp $$373$end
  $$373$skip1:
    cmp eax, ?DENSE_TAG
    jne $$373$skip2
.data
align 4
__$$$383$save3 DD 0
__$$$383$save2 DD 0
__$$$383$save1 DD 0
.code
    mov esi, 2
    mov ebp, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov edx, dword ptr [ecx + ?VT_MASK]
    mov edi, dword ptr _$$Tj
    add edi, 8
    add ebp, dword ptr _$$Tj
  $$383$loop:
    cmp edi, ebp
    jae $$383$loopEnd
    cmp esi, 30
    jae $$383$loopEnd
    lea ecx, dword ptr [esi + 2]
    mov ebx, edx
    shr ebx, cl
    and ebx, 1
    cmp ebx, 1
    jne $$383$skip1
.data
align 4
.code
    mov ecx, dword ptr [edi]
    cmp ecx, dword ptr _$$GcLo
    jb $$383$skip2
    cmp ecx, dword ptr _$$GcHi
    ja $$383$skip2
.data
align 4
.code
    mov dword ptr __$$$383$save1, esi
    mov dword ptr __$$$383$save2, ebp
    mov dword ptr __$$$383$save3, edx
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp dword ptr _$$GcLo, eax
    ja $$386$skip
    cmp dword ptr _$$GcHi, eax
    jae $$386$done
  $$386$skip:
    sub ecx, 4
.data
align 4
.code
    call _?CopyAndForward
    add eax, 4
  $$386$done:
    mov esi, dword ptr __$$$383$save1
    mov ebp, dword ptr __$$$383$save2
    mov edx, dword ptr __$$$383$save3
    mov edi, dword ptr _$$Tj
    lea edi, dword ptr [edi + 4 * esi + 0]
.data
align 4
.code
    mov dword ptr [edi], eax
  $$383$skip2:
  $$383$skip1:
    add esi, 1
    add edi, 4
    jmp $$383$loop
  $$383$loopEnd:
    mov dword ptr _$$Tj, ebp
    jmp $$373$end
  $$373$skip2:
    cmp eax, ?STRING_TAG
    jne $$373$skip3
.data
align 4
.code
    mov edx, ecx
    mov ecx, dword ptr _$$Tj
.data
align 4
.code
    call _?GetSize
    add dword ptr _$$Tj, eax
    jmp $$373$end
  $$373$skip3:
    cmp eax, ?PTR_VECTOR_TAG
    jne $$373$skip4
.data
align 4
__$$$392$save2 DD 0
__$$$392$save1 DD 0
.code
    mov ebx, dword ptr _$$Tj
.data
align 4
.code
    mov ebp, dword ptr [ebx + 8]
    mov esi, dword ptr [ecx + ?VT_BASE_LENGTH]
    add ebp, ebp
    jc _?Overflow
    add ebp, ebp
    jc _?Overflow
    add ebp, esi
    jc _?Overflow
    add ebp, 3
    jc _?Overflow
    mov eax, 3
    not eax
    and ebp, eax
    shr esi, 2
    lea edi, dword ptr [ebx + 4 * esi + 0]
    add ebp, ebx
  $$392$loop:
    cmp edi, ebp
    jae $$392$loopEnd
.data
align 4
.code
    mov ecx, dword ptr [edi]
    cmp ecx, dword ptr _$$GcLo
    jb $$392$skip1
    cmp ecx, dword ptr _$$GcHi
    ja $$392$skip1
.data
align 4
.code
    mov dword ptr __$$$392$save1, edi
    mov dword ptr __$$$392$save2, ebp
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp dword ptr _$$GcLo, eax
    ja $$396$skip
    cmp dword ptr _$$GcHi, eax
    jae $$396$done
  $$396$skip:
    sub ecx, 4
.data
align 4
.code
    call _?CopyAndForward
    add eax, 4
  $$396$done:
    mov edi, dword ptr __$$$392$save1
    mov ebp, dword ptr __$$$392$save2
.data
align 4
.code
    mov dword ptr [edi], eax
  $$392$skip1:
    add edi, 4
    jmp $$392$loop
  $$392$loopEnd:
    mov dword ptr _$$Tj, ebp
    jmp $$373$end
  $$373$skip4:
    cmp eax, ?OTHER_VECTOR_TAG
    jne $$373$skip5
.data
align 4
.code
    mov ebp, dword ptr [ecx + ?VT_ARRAY_OF]
.data
align 4
.code
    mov esi, dword ptr [ecx + ?VT_ARRAY_ELEMENT_SIZE]
    mov edi, dword ptr [ecx + ?VT_ARRAY_ELEMENT_CLASS]
    mov edi, dword ptr [edi + ?VT_MASK]
    cmp ebp, ?TYPE_STRUCT
    jne $$373$noPoint
    cmp ebp, ?TYPE_STRUCT
    jne $$373$vecSkip1
    cmp edi, ?SPARSE_TAG
    jne $$373$vecSkip1
  $$373$noPoint:
.data
align 4
.code
    mov edx, ecx
    mov ecx, dword ptr _$$Tj
.data
align 4
.code
    call _?GetSize
    add dword ptr _$$Tj, eax
    jmp $$373$end
  $$373$vecSkip1:
    cmp ebp, ?TYPE_STRUCT
    jne $$373$vecSkip2
    mov eax, edi
    and eax, 15
    cmp eax, ?SPARSE_TAG
    jne $$373$vecSkip2
.data
align 4
__$$$404$save2 DD 0
__$$$404$save1 DD 0
.code
    mov edi, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov ebx, dword ptr [ecx + ?VT_ARRAY_ELEMENT_SIZE]
    mov edx, dword ptr _$$Tj
.data
align 4
.code
    mov edx, dword ptr [edx + 8]
    mov eax, ebx
    mul edx
    jc _?Overflow
    add eax, edi
    jc _?Overflow
    add eax, 3
    jc _?Overflow
    mov edx, 3
    not edx
    and eax, edx
    mov ebp, eax
    shr edi, 2
    shr ebx, 2
    mov edx, dword ptr [ecx + ?VT_ARRAY_ELEMENT_CLASS]
    mov edx, dword ptr [edx + ?VT_MASK]
  $$404$loop:
    mov eax, 0
    lea eax, dword ptr [eax + 4 * edi + 0]
    cmp eax, ebp
    jae $$404$loopEnd
    mov dword ptr __$$$404$save1, ebx
    mov dword ptr __$$$404$save2, ebp
.data
align 4
__$$$406$save4 DD 0
__$$$406$save3 DD 0
__$$$406$save2 DD 0
__$$$406$save1 DD 0
.code
    mov esi, 1
  $$406$loop:
    cmp esi, 8
    jae $$406$loopEnd
    mov ecx, 0
    lea ecx, dword ptr [ecx + 4 * esi + 0]
    mov ebx, edx
    shr ebx, cl
    and ebx, 15
    cmp ebx, 0
    je $$406$skip1
    sub ebx, 1
    add ebx, edi
    mov eax, dword ptr _$$Tj
.data
align 4
.code
    mov ecx, dword ptr [eax + 4 * ebx + 0]
    cmp ecx, dword ptr _$$GcLo
    jb $$406$skip2
    cmp ecx, dword ptr _$$GcHi
    ja $$406$skip2
.data
align 4
.code
    mov dword ptr __$$$406$save1, esi
    mov dword ptr __$$$406$save2, edi
    mov dword ptr __$$$406$save3, edx
    mov dword ptr __$$$406$save4, ebx
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp dword ptr _$$GcLo, eax
    ja $$409$skip
    cmp dword ptr _$$GcHi, eax
    jae $$409$done
  $$409$skip:
    sub ecx, 4
.data
align 4
.code
    call _?CopyAndForward
    add eax, 4
  $$409$done:
    mov esi, dword ptr __$$$406$save1
    mov edi, dword ptr __$$$406$save2
    mov edx, dword ptr __$$$406$save3
    mov ebx, dword ptr __$$$406$save4
    mov ecx, dword ptr _$$Tj
.data
align 4
.code
    mov dword ptr [ecx + 4 * ebx + 0], eax
  $$406$skip2:
  $$406$skip1:
    add esi, 1
    jmp $$406$loop
  $$406$loopEnd:
    mov ebx, dword ptr __$$$404$save1
    mov ebp, dword ptr __$$$404$save2
    add edi, ebx
    jmp $$404$loop
  $$404$loopEnd:
    add dword ptr _$$Tj, ebp
    jmp $$373$end
  $$373$vecSkip2:
    mov eax, 1431633970
.data
align 4
.code
    call _?DebugBreak
  $$373$skip5:
    cmp eax, ?PTR_ARRAY_TAG
    jne $$373$skip6
.data
align 4
__$$$414$save2 DD 0
__$$$414$save1 DD 0
.code
    mov ebx, dword ptr _$$Tj
.data
align 4
.code
    mov ebp, dword ptr [ebx + 12]
    mov esi, dword ptr [ecx + ?VT_BASE_LENGTH]
    add ebp, ebp
    jc _?Overflow
    add ebp, ebp
    jc _?Overflow
    add ebp, esi
    jc _?Overflow
    add ebp, 3
    jc _?Overflow
    mov eax, 3
    not eax
    and ebp, eax
    shr esi, 2
    lea edi, dword ptr [ebx + 4 * esi + 0]
    add ebp, ebx
  $$414$loop:
    cmp edi, ebp
    jae $$414$loopEnd
.data
align 4
.code
    mov ecx, dword ptr [edi]
    cmp ecx, dword ptr _$$GcLo
    jb $$414$skip1
    cmp ecx, dword ptr _$$GcHi
    ja $$414$skip1
.data
align 4
.code
    mov dword ptr __$$$414$save1, edi
    mov dword ptr __$$$414$save2, ebp
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp dword ptr _$$GcLo, eax
    ja $$418$skip
    cmp dword ptr _$$GcHi, eax
    jae $$418$done
  $$418$skip:
    sub ecx, 4
.data
align 4
.code
    call _?CopyAndForward
    add eax, 4
  $$418$done:
    mov edi, dword ptr __$$$414$save1
    mov ebp, dword ptr __$$$414$save2
.data
align 4
.code
    mov dword ptr [edi], eax
  $$414$skip1:
    add edi, 4
    jmp $$414$loop
  $$414$loopEnd:
    mov dword ptr _$$Tj, ebp
    jmp $$373$end
  $$373$skip6:
    cmp eax, ?OTHER_ARRAY_TAG
    jne $$373$skip7
.data
align 4
.code
    mov ebp, dword ptr [ecx + ?VT_ARRAY_OF]
    cmp ebp, ?TYPE_STRUCT
    je $$373$arraySkip1
.data
align 4
.code
    mov edx, ecx
    mov ecx, dword ptr _$$Tj
.data
align 4
.code
    call _?GetSize
    add dword ptr _$$Tj, eax
    jmp $$373$end
  $$373$arraySkip1:
    mov eax, 1431633971
.data
align 4
.code
    call _?DebugBreak
    jmp $$373$end
  $$373$skip7:
.data
align 4
__$$$426$save5 DD 0
__$$$426$save4 DD 0
__$$$426$save3 DD 0
__$$$426$save2 DD 0
__$$$426$save1 DD 0
.code
    mov edx, dword ptr [ecx + ?VT_MASK]
    mov edi, dword ptr [edx]
    mov ebp, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov esi, 1
  $$426$loop:
    cmp esi, edi
    ja $$426$loopEnd
    mov ebx, dword ptr [edx + 4 * esi + 0]
    cmp ebx, 0
    je $$426$skip1
    mov eax, dword ptr _$$Tj
.data
align 4
.code
    mov ecx, dword ptr [eax + 4 * ebx + 4]
    cmp ecx, dword ptr _$$GcLo
    jb $$426$skip2
    cmp ecx, dword ptr _$$GcHi
    ja $$426$skip2
.data
align 4
.code
    mov dword ptr __$$$426$save1, ebx
    mov dword ptr __$$$426$save2, esi
    mov dword ptr __$$$426$save3, edi
    mov dword ptr __$$$426$save4, ebp
    mov dword ptr __$$$426$save5, edx
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp dword ptr _$$GcLo, eax
    ja $$429$skip
    cmp dword ptr _$$GcHi, eax
    jae $$429$done
  $$429$skip:
    sub ecx, 4
.data
align 4
.code
    call _?CopyAndForward
    add eax, 4
  $$429$done:
    mov ebx, dword ptr __$$$426$save1
    mov esi, dword ptr __$$$426$save2
    mov edi, dword ptr __$$$426$save3
    mov ebp, dword ptr __$$$426$save4
    mov edx, dword ptr __$$$426$save5
    mov ecx, dword ptr _$$Tj
.data
align 4
.code
    mov dword ptr [ecx + 4 * ebx + 4], eax
  $$426$skip2:
  $$426$skip1:
    add esi, 1
    jc _?Overflow
    jmp $$426$loop
  $$426$loopEnd:
    add dword ptr _$$Tj, ebp
  $$373$end:
    jmp $$372$loop
  $$372$exit:
    mov eax, dword ptr _$$Fi
    mov ebx, dword ptr _$$Ti
    mov dword ptr _$$Fi, ebx
    mov dword ptr _$$Ti, eax
    mov eax, dword ptr _$$Fl
    mov ebx, dword ptr _$$Tl
    mov dword ptr _$$Fl, ebx
    mov dword ptr _$$Tl, eax
    mov eax, dword ptr _$$Tk
    mov dword ptr _$$Fk, eax
    mov eax, dword ptr _$$Ti
    mov dword ptr _$$Tk, eax
    mov dword ptr _$$Tj, eax
    mov eax, dword ptr _$$BF
    mov ebx, dword ptr _$$BT
    mov dword ptr _$$BF, ebx
    mov dword ptr _$$BT, eax
    mov ebp, dword ptr __$$$320$saveEbp
    mov eax, dword ptr __$GarbageCollect$t
.data
align 4
.code
    mov dword ptr [eax], ?STACK_RUNNING
    mov esp, dword ptr __$GarbageCollect$sp
.data
align 4
.code
    ret
_?GarbageCollect endp
.data
align 4
__$InitializeGc$unitSize DD 0
__$InitializeGc$save DD 0
.code
align 16
_?InitializeGc proc
    mov dword ptr __$InitializeGc$save, ebp
    mov esi, dword ptr _$$GcHi
    sub esi, dword ptr _$$GcLo
    mov edi, esi
    mov edx, 0
    mov eax, edi
    mov ebx, 264
    div ebx
    mov ebp, eax
    mov dword ptr __$InitializeGc$unitSize, ebp
    mov edx, 0
    lea ebp, dword ptr [edx + 4 * ebp + 0]
    mov eax, dword ptr _$$GcLo
    mov dword ptr _$$BF, eax
    add eax, ebp
    mov dword ptr _$$BT, eax
    lea ebx, dword ptr [eax + 1 * ebp + 0]
    lea ebp, dword ptr [edx + 4 * ebp + 0]
    lea ecx, dword ptr [ebx + 8 * ebp + 0]
    lea edx, dword ptr [ecx + 8 * ebp + 0]
    mov dword ptr _$$HeapLo, ebx
    mov dword ptr _$$Fi, ebx
    mov dword ptr _$$Fk, ebx
    mov dword ptr _$$Fl, ecx
    mov dword ptr _$$Ti, ecx
    mov dword ptr _$$Tj, ecx
    mov dword ptr _$$Tk, ecx
    mov dword ptr _$$Tl, edx
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr _$$GcLo
    mov esi, dword ptr __$InitializeGc$unitSize
    lea ebx, dword ptr [eax + 4 * esi + 0]
.data
align 4
.code
    call _?BB4Zero
    mov ebp, dword ptr __$InitializeGc$save
.data
align 4
.code
    ret
_?InitializeGc endp
.data
align 4
.code
align 16
_?AllocObject proc
.data
align 4
.code
    mov eax, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov ebx, dword ptr _$$Fk
    add ebx, eax
    jc _?Overflow
    cmp ebx, dword ptr _$$Fl
    jbe AllocObject$skip1
    mov eax, 0
    jmp AllocObject$done
  AllocObject$skip1:
.data
align 4
.code
    mov eax, dword ptr _$$Fk
.data
align 4
.code
    mov esi, dword ptr _$$GcLo
    mov esi, eax
    cmp esi, ebx
    jae $$468$loopEnd
  $$468$loop:
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [esi], 0
    add esi, 4
    cmp esi, ebx
    jb $$468$loop
  $$468$loopEnd:
.data
align 4
.code
    mov dword ptr [eax + 4], ecx
    mov esi, eax
    sub esi, dword ptr _$$Fi
    mov edi, dword ptr _$$BF
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, esi
    shr esi, 7
    shr ecx, 2
    add esi, esi
    add esi, esi
    add esi, edi
    and ecx, 31
    mov edi, 1
    shl edi, cl
    mov ecx, edi
.data
align 4
.code
    mov edi, dword ptr [esi]
    or edi, ecx
.data
align 4
.code
    mov dword ptr [esi], edi
    mov dword ptr _$$Fk, ebx
    add eax, 4
  AllocObject$done:
.data
align 4
.code
    ret
_?AllocObject endp
.data
align 4
.code
align 16
_?AllocString proc
    add ecx, 1
    jc _?Overflow
    mov edx, ecx
    add ecx, ecx
    jc _?Overflow
    add ecx, 19
    jc _?Overflow
    mov eax, 3
    not eax
    and ecx, eax
    mov ebx, dword ptr _$$Fk
    add ebx, ecx
    jc _?Overflow
    cmp ebx, dword ptr _$$Fl
    jbe AllocString$skip1
    mov eax, 0
    jmp AllocString$done
  AllocString$skip1:
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr _$$Fk
.data
align 4
.code
    mov esi, dword ptr _$$GcLo
    mov esi, eax
    cmp esi, ebx
    jae $$536$loopEnd
  $$536$loop:
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [esi], 0
    add esi, 4
    cmp esi, ebx
    jb $$536$loop
  $$536$loopEnd:
.data
align 4
.code
    mov dword ptr [eax + 8], edx
    sub edx, 1
    jc _?Overflow
.data
align 4
.code
    mov dword ptr [eax + 12], edx
    mov edx, ?STRING_VTABLE
.data
align 4
.code
    mov dword ptr [eax + 4], edx
    mov esi, eax
    sub esi, dword ptr _$$Fi
    mov edi, dword ptr _$$BF
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, esi
    shr esi, 7
    shr ecx, 2
    add esi, esi
    add esi, esi
    add esi, edi
    and ecx, 31
    mov edi, 1
    shl edi, cl
    mov ecx, edi
.data
align 4
.code
    mov edi, dword ptr [esi]
    or edi, ecx
.data
align 4
.code
    mov dword ptr [esi], edi
    mov dword ptr _$$Fk, ebx
    add eax, 4
  AllocString$done:
.data
align 4
.code
    ret
_?AllocString endp
.data
align 4
__$AllocArray$rank DD 0
.code
align 16
_?AllocArray proc
.data
align 4
.code
    mov esi, dword ptr [esp + 4]
    mov dword ptr __$AllocArray$rank, edx
.data
align 4
.code
    mov eax, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov ebx, dword ptr [ecx + ?VT_ARRAY_ELEMENT_SIZE]
    mov edi, dword ptr [ecx + ?VT_MASK]
    and edi, 15
    cmp edi, ?PTR_ARRAY_TAG
    jne $$605$skip1
    mov edi, esi
    add edi, edi
    jc _?Overflow
    add edi, edi
    jc _?Overflow
    add eax, edi
    jc _?Overflow
    add eax, 3
    jc _?Overflow
    mov edi, 3
    not edi
    and eax, edi
    jmp $$605$skip2
  $$605$skip1:
    mov edi, eax
    mov eax, ebx
    mul esi
    jc _?Overflow
    add eax, edi
    jc _?Overflow
    add eax, 3
    jc _?Overflow
    mov edi, 3
    not edi
    and eax, edi
  $$605$skip2:
    mov ebx, dword ptr _$$Fk
    add ebx, eax
    jc _?Overflow
    cmp ebx, dword ptr _$$Fl
    jbe AllocArray$skip1
    mov eax, 0
    jmp AllocArray$done
  AllocArray$skip1:
    mov edx, dword ptr __$AllocArray$rank
.data
align 4
.code
.data
align 4
__$$$607$nElems DD 0
.code
    mov eax, dword ptr _$$Fk
    mov dword ptr __$$$607$nElems, esi
.data
align 4
.code
    mov esi, dword ptr _$$GcLo
    mov esi, eax
    cmp esi, ebx
    jae $$608$loopEnd
  $$608$loop:
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [esi], 0
    add esi, 4
    cmp esi, ebx
    jb $$608$loop
  $$608$loopEnd:
    mov esi, dword ptr __$$$607$nElems
.data
align 4
.code
    mov dword ptr [eax + 4], ecx
.data
align 4
.code
    mov dword ptr [eax + 8], edx
.data
align 4
.code
    mov dword ptr [eax + 12], esi
    mov esi, eax
    sub esi, dword ptr _$$Fi
    mov edi, dword ptr _$$BF
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, esi
    shr esi, 7
    shr ecx, 2
    add esi, esi
    add esi, esi
    add esi, edi
    and ecx, 31
    mov edi, 1
    shl edi, cl
    mov ecx, edi
.data
align 4
.code
    mov edi, dword ptr [esi]
    or edi, ecx
.data
align 4
.code
    mov dword ptr [esi], edi
    mov dword ptr _$$Fk, ebx
    add eax, 4
  AllocArray$done:
.data
align 4
.code
    ret
_?AllocArray endp
.data
align 4
__$AllocVector$nElems DD 0
.code
align 16
_?AllocVector proc
    mov dword ptr __$AllocVector$nElems, edx
.data
align 4
.code
    mov eax, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov ebx, dword ptr [ecx + ?VT_ARRAY_ELEMENT_SIZE]
    mov edi, dword ptr [ecx + ?VT_MASK]
    and edi, 15
    cmp edi, ?PTR_VECTOR_TAG
    jne $$676$skip1
    mov edi, edx
    add edi, edi
    jc _?Overflow
    add edi, edi
    jc _?Overflow
    add eax, edi
    jc _?Overflow
    add eax, 3
    jc _?Overflow
    mov edi, 3
    not edi
    and eax, edi
    jmp $$676$skip2
  $$676$skip1:
    mov edi, eax
    mov eax, ebx
    mul edx
    jc _?Overflow
    add eax, edi
    jc _?Overflow
    add eax, 3
    jc _?Overflow
    mov edi, 3
    not edi
    and eax, edi
  $$676$skip2:
    mov ebx, dword ptr _$$Fk
    add ebx, eax
    jc _?Overflow
    cmp ebx, dword ptr _$$Fl
    jbe AllocVector$skip1
    mov eax, 0
    jmp AllocVector$done
  AllocVector$skip1:
    mov edx, dword ptr __$AllocVector$nElems
.data
align 4
.code
    mov eax, dword ptr _$$Fk
.data
align 4
.code
    mov esi, dword ptr _$$GcLo
    mov esi, eax
    cmp esi, ebx
    jae $$678$loopEnd
  $$678$loop:
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [esi], 0
    add esi, 4
    cmp esi, ebx
    jb $$678$loop
  $$678$loopEnd:
.data
align 4
.code
    mov dword ptr [eax + 4], ecx
.data
align 4
.code
    mov dword ptr [eax + 8], edx
    mov esi, eax
    sub esi, dword ptr _$$Fi
    mov edi, dword ptr _$$BF
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, esi
    shr esi, 7
    shr ecx, 2
    add esi, esi
    add esi, esi
    add esi, edi
    and ecx, 31
    mov edi, 1
    shl edi, cl
    mov ecx, edi
.data
align 4
.code
    mov edi, dword ptr [esi]
    or edi, ecx
.data
align 4
.code
    mov dword ptr [esi], edi
    mov dword ptr _$$Fk, ebx
    add eax, 4
  AllocVector$done:
.data
align 4
.code
    ret
_?AllocVector endp
.data
align 4
__$NucleusEntryPoint$s DD 0
__$NucleusEntryPoint$handler DD 0
__$NucleusEntryPoint$idtLo DD 0
__$NucleusEntryPoint$platform DD 0
.code
align 16
_?NucleusEntryPoint proc
    mov dword ptr __$NucleusEntryPoint$platform, ecx
    mov eax, 305419896
    mov edx, 20
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$746$__L1
    add ecx, 3120
    jmp $$746$__L2
  $$746$__L1:
    add ecx, 3127
    jc _?Overflow
  $$746$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$747$__L1
    add ecx, 3120
    jmp $$747$__L2
  $$747$__L1:
    add ecx, 3127
    jc _?Overflow
  $$747$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$748$__L1
    add ecx, 3120
    jmp $$748$__L2
  $$748$__L1:
    add ecx, 3127
    jc _?Overflow
  $$748$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$749$__L1
    add ecx, 3120
    jmp $$749$__L2
  $$749$__L1:
    add ecx, 3127
    jc _?Overflow
  $$749$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$750$__L1
    add ecx, 3120
    jmp $$750$__L2
  $$750$__L1:
    add ecx, 3127
    jc _?Overflow
  $$750$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$751$__L1
    add ecx, 3120
    jmp $$751$__L2
  $$751$__L1:
    add ecx, 3127
    jc _?Overflow
  $$751$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$752$__L1
    add ecx, 3120
    jmp $$752$__L2
  $$752$__L1:
    add ecx, 3127
    jc _?Overflow
  $$752$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$753$__L1
    add ecx, 3120
    jmp $$753$__L2
  $$753$__L1:
    add ecx, 3127
    jc _?Overflow
  $$753$__L2:
    mov word ptr [edx + 753664], cx
    mov eax, dword ptr __$NucleusEntryPoint$platform
    mov edx, 40
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$755$__L1
    add ecx, 3120
    jmp $$755$__L2
  $$755$__L1:
    add ecx, 3127
    jc _?Overflow
  $$755$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$756$__L1
    add ecx, 3120
    jmp $$756$__L2
  $$756$__L1:
    add ecx, 3127
    jc _?Overflow
  $$756$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$757$__L1
    add ecx, 3120
    jmp $$757$__L2
  $$757$__L1:
    add ecx, 3127
    jc _?Overflow
  $$757$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$758$__L1
    add ecx, 3120
    jmp $$758$__L2
  $$758$__L1:
    add ecx, 3127
    jc _?Overflow
  $$758$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$759$__L1
    add ecx, 3120
    jmp $$759$__L2
  $$759$__L1:
    add ecx, 3127
    jc _?Overflow
  $$759$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$760$__L1
    add ecx, 3120
    jmp $$760$__L2
  $$760$__L1:
    add ecx, 3127
    jc _?Overflow
  $$760$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$761$__L1
    add ecx, 3120
    jmp $$761$__L2
  $$761$__L1:
    add ecx, 3127
    jc _?Overflow
  $$761$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$762$__L1
    add ecx, 3120
    jmp $$762$__L2
  $$762$__L1:
    add ecx, 3127
    jc _?Overflow
  $$762$__L2:
    mov word ptr [edx + 753664], cx
    mov esi, dword ptr __$NucleusEntryPoint$platform
    mov eax, dword ptr [esi + 40]
    mov edx, 60
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$764$__L1
    add ecx, 3120
    jmp $$764$__L2
  $$764$__L1:
    add ecx, 3127
    jc _?Overflow
  $$764$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$765$__L1
    add ecx, 3120
    jmp $$765$__L2
  $$765$__L1:
    add ecx, 3127
    jc _?Overflow
  $$765$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$766$__L1
    add ecx, 3120
    jmp $$766$__L2
  $$766$__L1:
    add ecx, 3127
    jc _?Overflow
  $$766$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$767$__L1
    add ecx, 3120
    jmp $$767$__L2
  $$767$__L1:
    add ecx, 3127
    jc _?Overflow
  $$767$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$768$__L1
    add ecx, 3120
    jmp $$768$__L2
  $$768$__L1:
    add ecx, 3127
    jc _?Overflow
  $$768$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$769$__L1
    add ecx, 3120
    jmp $$769$__L2
  $$769$__L1:
    add ecx, 3127
    jc _?Overflow
  $$769$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$770$__L1
    add ecx, 3120
    jmp $$770$__L2
  $$770$__L1:
    add ecx, 3127
    jc _?Overflow
  $$770$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$771$__L1
    add ecx, 3120
    jmp $$771$__L2
  $$771$__L1:
    add ecx, 3127
    jc _?Overflow
  $$771$__L2:
    mov word ptr [edx + 753664], cx
    mov esi, dword ptr __$NucleusEntryPoint$platform
    mov eax, dword ptr [esi + 44]
    mov edx, 80
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$773$__L1
    add ecx, 3120
    jmp $$773$__L2
  $$773$__L1:
    add ecx, 3127
    jc _?Overflow
  $$773$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$774$__L1
    add ecx, 3120
    jmp $$774$__L2
  $$774$__L1:
    add ecx, 3127
    jc _?Overflow
  $$774$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$775$__L1
    add ecx, 3120
    jmp $$775$__L2
  $$775$__L1:
    add ecx, 3127
    jc _?Overflow
  $$775$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$776$__L1
    add ecx, 3120
    jmp $$776$__L2
  $$776$__L1:
    add ecx, 3127
    jc _?Overflow
  $$776$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$777$__L1
    add ecx, 3120
    jmp $$777$__L2
  $$777$__L1:
    add ecx, 3127
    jc _?Overflow
  $$777$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$778$__L1
    add ecx, 3120
    jmp $$778$__L2
  $$778$__L1:
    add ecx, 3127
    jc _?Overflow
  $$778$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$779$__L1
    add ecx, 3120
    jmp $$779$__L2
  $$779$__L1:
    add ecx, 3127
    jc _?Overflow
  $$779$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$780$__L1
    add ecx, 3120
    jmp $$780$__L2
  $$780$__L1:
    add ecx, 3127
    jc _?Overflow
  $$780$__L2:
    mov word ptr [edx + 753664], cx
    mov esi, dword ptr __$NucleusEntryPoint$platform
    mov eax, dword ptr [esi + 44]
    cmp eax, 4
    jb NucleusEntryPoint$__L23
    mov edi, dword ptr [esi + 40]
    mov eax, dword ptr [edi + 72]
    mov ebx, eax
    mov edx, 100
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$782$__L1
    add ecx, 3120
    jmp $$782$__L2
  $$782$__L1:
    add ecx, 3127
    jc _?Overflow
  $$782$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$783$__L1
    add ecx, 3120
    jmp $$783$__L2
  $$783$__L1:
    add ecx, 3127
    jc _?Overflow
  $$783$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$784$__L1
    add ecx, 3120
    jmp $$784$__L2
  $$784$__L1:
    add ecx, 3127
    jc _?Overflow
  $$784$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$785$__L1
    add ecx, 3120
    jmp $$785$__L2
  $$785$__L1:
    add ecx, 3127
    jc _?Overflow
  $$785$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$786$__L1
    add ecx, 3120
    jmp $$786$__L2
  $$786$__L1:
    add ecx, 3127
    jc _?Overflow
  $$786$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$787$__L1
    add ecx, 3120
    jmp $$787$__L2
  $$787$__L1:
    add ecx, 3127
    jc _?Overflow
  $$787$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$788$__L1
    add ecx, 3120
    jmp $$788$__L2
  $$788$__L1:
    add ecx, 3127
    jc _?Overflow
  $$788$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$789$__L1
    add ecx, 3120
    jmp $$789$__L2
  $$789$__L1:
    add ecx, 3127
    jc _?Overflow
  $$789$__L2:
    mov word ptr [edx + 753664], cx
    mov eax, dword ptr [edi + 80]
    mov esi, eax
    mov edx, 120
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$791$__L1
    add ecx, 3120
    jmp $$791$__L2
  $$791$__L1:
    add ecx, 3127
    jc _?Overflow
  $$791$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$792$__L1
    add ecx, 3120
    jmp $$792$__L2
  $$792$__L1:
    add ecx, 3127
    jc _?Overflow
  $$792$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$793$__L1
    add ecx, 3120
    jmp $$793$__L2
  $$793$__L1:
    add ecx, 3127
    jc _?Overflow
  $$793$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$794$__L1
    add ecx, 3120
    jmp $$794$__L2
  $$794$__L1:
    add ecx, 3127
    jc _?Overflow
  $$794$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$795$__L1
    add ecx, 3120
    jmp $$795$__L2
  $$795$__L1:
    add ecx, 3127
    jc _?Overflow
  $$795$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$796$__L1
    add ecx, 3120
    jmp $$796$__L2
  $$796$__L1:
    add ecx, 3127
    jc _?Overflow
  $$796$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$797$__L1
    add ecx, 3120
    jmp $$797$__L2
  $$797$__L1:
    add ecx, 3127
    jc _?Overflow
  $$797$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$798$__L1
    add ecx, 3120
    jmp $$798$__L2
  $$798$__L1:
    add ecx, 3127
    jc _?Overflow
  $$798$__L2:
    mov word ptr [edx + 753664], cx
    mov eax, dword ptr [edi + 88]
    cmp eax, 1
    jne NucleusEntryPoint$__L24
    mov dword ptr __$NucleusEntryPoint$idtLo, ebx
    mov eax, ebx
    add eax, 2048
    mov ecx, eax
    add ecx, 65536
    add ecx, 18874368
    mov eax, ebx
    add eax, esi
    mov edx, eax
.data
align 4
.code
    mov dword ptr _$$SLo, ecx
    add ecx, 1024
    jc _?Overflow
    mov dword ptr _$$DLo, ecx
    add ecx, 1024
    jc _?Overflow
    mov dword ptr _$$PciLo, ecx
    add ecx, 524288
    jc _?Overflow
    mov dword ptr _$$TLo, ecx
    add ecx, 16384
    jc _?Overflow
    mov dword ptr _$$FLo, ecx
    add ecx, 1095680
    jc _?Overflow
    mov dword ptr _$$GcLo, ecx
    mov dword ptr _$$GcHi, edx
    cmp ecx, edx
    jbe $$799$__L1
    mov eax, 1431633936
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$802$__L1
    add ecx, 3120
    jmp $$802$__L2
  $$802$__L1:
    add ecx, 3127
    jc _?Overflow
  $$802$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$803$__L1
    add ecx, 3120
    jmp $$803$__L2
  $$803$__L1:
    add ecx, 3127
    jc _?Overflow
  $$803$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$804$__L1
    add ecx, 3120
    jmp $$804$__L2
  $$804$__L1:
    add ecx, 3127
    jc _?Overflow
  $$804$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$805$__L1
    add ecx, 3120
    jmp $$805$__L2
  $$805$__L1:
    add ecx, 3127
    jc _?Overflow
  $$805$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$806$__L1
    add ecx, 3120
    jmp $$806$__L2
  $$806$__L1:
    add ecx, 3127
    jc _?Overflow
  $$806$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$807$__L1
    add ecx, 3120
    jmp $$807$__L2
  $$807$__L1:
    add ecx, 3127
    jc _?Overflow
  $$807$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$808$__L1
    add ecx, 3120
    jmp $$808$__L2
  $$808$__L1:
    add ecx, 3127
    jc _?Overflow
  $$808$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$809$__L1
    add ecx, 3120
    jmp $$809$__L2
  $$809$__L1:
    add ecx, 3127
    jc _?Overflow
  $$809$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$800$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$800$loop
  $$799$__L1:
    mov esp, dword ptr _$$DLo
    mov edi, 0
    mov esi, dword ptr __$NucleusEntryPoint$idtLo
  NucleusEntryPoint$__L25:
    cmp edi, 256
    jae NucleusEntryPoint$__L26
    cmp edi, 0
    jne NucleusEntryPoint$__L27
    mov dword ptr __$NucleusEntryPoint$handler, ?FaultHandler
    jmp NucleusEntryPoint$__L28
  NucleusEntryPoint$__L27:
    cmp edi, 3
    jne NucleusEntryPoint$__L29
    mov dword ptr __$NucleusEntryPoint$handler, ?FaultHandler
    jmp NucleusEntryPoint$__L30
  NucleusEntryPoint$__L29:
    cmp edi, 4
    jne NucleusEntryPoint$__L31
    mov dword ptr __$NucleusEntryPoint$handler, ?FaultHandler
    jmp NucleusEntryPoint$__L32
  NucleusEntryPoint$__L31:
    cmp edi, 13
    jne NucleusEntryPoint$__L33
    mov dword ptr __$NucleusEntryPoint$handler, ?ErrorHandler
    jmp NucleusEntryPoint$__L34
  NucleusEntryPoint$__L33:
    cmp edi, 14
    jne NucleusEntryPoint$__L35
    mov dword ptr __$NucleusEntryPoint$handler, ?ErrorHandler
    jmp NucleusEntryPoint$__L36
  NucleusEntryPoint$__L35:
    cmp edi, 32
    jae NucleusEntryPoint$__L37
    mov dword ptr __$NucleusEntryPoint$handler, ?FatalHandler
    jmp NucleusEntryPoint$__L38
  NucleusEntryPoint$__L37:
    mov dword ptr __$NucleusEntryPoint$handler, ?InterruptHandler
  NucleusEntryPoint$__L38:
  NucleusEntryPoint$__L36:
  NucleusEntryPoint$__L34:
  NucleusEntryPoint$__L32:
  NucleusEntryPoint$__L30:
  NucleusEntryPoint$__L28:
    mov edx, dword ptr __$NucleusEntryPoint$handler
    and edx, 65535
    mov ecx, 32
    shl ecx, 16
    or ecx, edx
    mov dword ptr [esi], ecx
    mov ecx, dword ptr __$NucleusEntryPoint$handler
    mov eax, 2147450880
    add eax, 2147450880
    mov edx, eax
    and ecx, edx
    or ecx, 36352
    mov dword ptr [esi + 4], ecx
    add esi, 8
    add edi, 1
    jmp NucleusEntryPoint$__L25
  NucleusEntryPoint$__L26:
    mov ecx, dword ptr __$NucleusEntryPoint$idtLo
    mov edi, dword ptr _$$DLo
    shl ecx, 16
    or ecx, 2047
.data
align 4
.code
    mov dword ptr [edi], ecx
    mov ecx, dword ptr __$NucleusEntryPoint$idtLo
    shr ecx, 16
.data
align 4
.code
    mov dword ptr [edi + 4], ecx
    lidt fword ptr [edi]
    mov eax, 17
    mov edx, 32
    out dx, al
    mov eax, 17
    mov edx, 160
    out dx, al
    mov eax, 112
    mov edx, 33
    out dx, al
    mov eax, 120
    mov edx, 161
    out dx, al
    mov eax, 4
    mov edx, 33
    out dx, al
    mov eax, 2
    mov edx, 161
    out dx, al
    mov eax, 1
    mov edx, 33
    out dx, al
    mov eax, 1
    mov edx, 161
    out dx, al
    mov eax, 254
    mov edx, 33
    out dx, al
    mov eax, 255
    mov edx, 161
    out dx, al
    mov eax, 32
    mov edx, 32
    out dx, al
    mov eax, 32
    mov edx, 160
    out dx, al
    mov edi, 0
    mov esi, dword ptr _$$PciLo
  NucleusEntryPoint$__L39:
    cmp edi, 65536
    jae NucleusEntryPoint$__L40
.data
align 4
.code
    mov dword ptr [esi], 0
    add edi, 1
    add esi, 8
    jmp NucleusEntryPoint$__L39
  NucleusEntryPoint$__L40:
.data
align 4
.code
    mov eax, dword ptr __$NucleusEntryPoint$idtLo
    add eax, 2048
    mov ecx, eax
.data
align 4
__$$$814$dmaLo DD 0
.code
    mov eax, ecx
    add eax, 65536
    mov dword ptr __$$$814$dmaLo, eax
    add ecx, 4096
    mov eax, ecx
    add eax, 4096
    mov esi, eax
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    and ecx, 4095
    sub esi, ecx
    mov eax, dword ptr __$$$814$dmaLo
    add eax, 2097152
    mov ecx, eax
    mov eax, ecx
    add eax, 2097152
    mov ebx, eax
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    and ecx, 2097151
    sub ebx, ecx
.data
align 4
.code
    mov edi, esi
    mov eax, edi
    add eax, 4096
    mov ebp, eax
    mov edx, ebx
  $$1326$__L1:
    cmp edi, ebp
    jae $$1326$__L2
    mov eax, edx
    add eax, 3
    mov ecx, eax
    mov dword ptr [edi + 0], ecx
    mov dword ptr [edi + 4], 0
    mov dword ptr [edi + 4096], 0
    mov dword ptr [edi + 4100], 0
    mov dword ptr [edi + 8192], 0
    mov dword ptr [edi + 8196], 0
    mov dword ptr [edi + 12288], 0
    mov dword ptr [edi + 12292], 0
    add edi, 8
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    add edx, 4096
    jmp $$1326$__L1
  $$1326$__L2:
    mov ecx, ebx
    shr ecx, 21
    cmp ecx, 512
    jb $$1326$__L3
    mov eax, 1431633966
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1389$__L1
    add ecx, 3120
    jmp $$1389$__L2
  $$1389$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1389$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1390$__L1
    add ecx, 3120
    jmp $$1390$__L2
  $$1390$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1390$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1391$__L1
    add ecx, 3120
    jmp $$1391$__L2
  $$1391$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1391$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1392$__L1
    add ecx, 3120
    jmp $$1392$__L2
  $$1392$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1392$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1393$__L1
    add ecx, 3120
    jmp $$1393$__L2
  $$1393$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1393$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1394$__L1
    add ecx, 3120
    jmp $$1394$__L2
  $$1394$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1394$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1395$__L1
    add ecx, 3120
    jmp $$1395$__L2
  $$1395$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1395$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1396$__L1
    add ecx, 3120
    jmp $$1396$__L2
  $$1396$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1396$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1387$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1387$loop
  $$1326$__L3:
    mov eax, esi
    add eax, 3
    mov edx, eax
    mov dword ptr [esi + 8 * ecx + 4096], edx
    mov ecx, dword ptr __$$$814$dmaLo
.data
align 4
__$$$1397$dmaHi DD 0
__$$$1397$dmaLo DD 0
.code
    mov dword ptr __$$$1397$dmaLo, ecx
    mov eax, esi
    add eax, 16384
    mov edi, eax
    mov eax, edi
    add eax, 4096
    mov ebp, eax
    mov eax, esi
    add eax, 12288
    add eax, 3
    mov ecx, eax
    mov eax, esi
    add eax, 16384
    add eax, 1
    mov edx, eax
  $$1397$__L4:
    cmp edi, ebp
    jae $$1397$__L5
    mov dword ptr [edi + 0], ecx
    mov dword ptr [edi + 4], 0
    mov dword ptr [edi + 8], 258
    mov dword ptr [edi + 12], 0
    mov dword ptr [edi + 4096], edx
    mov dword ptr [edi + 4100], 0
    mov dword ptr [edi + 4104], 0
    mov dword ptr [edi + 4108], 0
    add edi, 16
    jmp $$1397$__L4
  $$1397$__L5:
    mov eax, esi
    add eax, 4096
    add eax, 3
    mov edx, eax
    mov dword ptr [esi + 8192], edx
    mov eax, esi
    add eax, 8192
    add eax, 3
    mov edx, eax
    mov dword ptr [esi + 12288], edx
    mov edx, dword ptr __$$$1397$dmaLo
    add edx, 18874368
    jc _?Overflow
    mov dword ptr __$$$1397$dmaHi, edx
    mov eax, dword ptr __$$$1397$dmaLo
    sub eax, 8
    mov ecx, eax
    mov edx, ?BYTE_VECTOR_VTABLE
    mov dword ptr [ecx], edx
    mov eax, dword ptr __$$$1397$dmaHi
    sub eax, dword ptr __$$$1397$dmaLo
    mov edx, eax
    mov dword ptr [ecx + 4], edx
.data
align 4
.code
    mov eax, esi
    add eax, 20480
    mov ebx, eax
    mov eax, dword ptr __$NucleusEntryPoint$idtLo
    add eax, 2048
    add eax, 65536
    mov ebp, eax
.data
align 4
__$$$1399$dmaLo DD 0
.code
    mov dword ptr __$$$1399$dmaLo, ebp
    mov esi, ?RoBiosLo
  $$1399$__L16:
    mov eax, ?RoBiosHi
    sub eax, 16
    cmp esi, eax
    jae $$1399$__L17
    mov ecx, dword ptr [esi + 0]
    mov edx, dword ptr [esi + 4]
    cmp ecx, 541348690
    jne $$1399$__L18
    cmp edx, 542266448
    jne $$1399$__L19
    mov edi, esi
    mov ebp, 0
  $$1399$__L20:
    mov eax, esi
    add eax, 20
    cmp edi, eax
    jae $$1399$__L21
    movzx eax, byte ptr [edi]
    add ebp, eax
    jc _?Overflow
    add edi, 1
    jmp $$1399$__L20
  $$1399$__L21:
    and ebp, 255
    cmp ebp, 0
    jne $$1399$__L22
.data
align 4
.code
    mov esi, dword ptr [esi + 16]
    mov ebp, dword ptr __$$$1399$dmaLo
.data
align 4
__$$$1401$dmaLo DD 0
.code
    mov dword ptr __$$$1401$dmaLo, ebp
    mov ecx, dword ptr [esi + 4]
    mov eax, esi
    add eax, ecx
    mov ebp, eax
    add esi, 36
  $$1401$__L10:
    cmp esi, ebp
    jae $$1401$__L11
    mov ecx, dword ptr [esi + 0]
    mov edx, dword ptr [ecx + 0]
    cmp edx, 1380011332
    jne $$1401$__L12
    mov edi, ecx
    add esi, 4
  $$1401$__L13:
    cmp esi, ebp
    jae $$1401$__L14
    mov ecx, dword ptr [esi + 0]
    mov edx, dword ptr [ecx + 0]
    cmp edx, 1380011332
    jne $$1401$__L15
    mov dword ptr _$$DmaAddr, 0
    jmp $$1401$done
  $$1401$__L15:
    add esi, 4
    jmp $$1401$__L13
  $$1401$__L14:
.data
align 4
.code
.data
align 4
__$$$1403$saveEcx DD 0
__$$$1403$end DD 0
.code
    mov ecx, dword ptr [edi + 4]
    mov eax, edi
    add eax, ecx
    mov dword ptr __$$$1403$end, eax
    add edi, 48
  $$1403$__L6:
    cmp edi, dword ptr __$$$1403$end
    jae $$1403$__L7
    movzx edx, word ptr [edi + 0]
    movzx ecx, word ptr [edi + 2]
    cmp edx, 0
    je $$1403$__L8
.data
align 4
.code
    jmp $$1403$done
  $$1403$__L8:
.data
align 4
.code
    mov eax, dword ptr [edi + 12]
    cmp eax, 0
    je $$1403$__L9
    mov eax, 1431633958
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1408$__L1
    add ecx, 3120
    jmp $$1408$__L2
  $$1408$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1408$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1409$__L1
    add ecx, 3120
    jmp $$1409$__L2
  $$1409$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1409$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1410$__L1
    add ecx, 3120
    jmp $$1410$__L2
  $$1410$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1410$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1411$__L1
    add ecx, 3120
    jmp $$1411$__L2
  $$1411$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1411$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1412$__L1
    add ecx, 3120
    jmp $$1412$__L2
  $$1412$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1412$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1413$__L1
    add ecx, 3120
    jmp $$1413$__L2
  $$1413$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1413$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1414$__L1
    add ecx, 3120
    jmp $$1414$__L2
  $$1414$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1414$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1415$__L1
    add ecx, 3120
    jmp $$1415$__L2
  $$1415$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1415$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1406$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1406$loop
  $$1403$__L9:
    mov esi, dword ptr [edi + 8]
    mov dword ptr __$$$1403$saveEcx, ecx
.data
align 4
.code
    mov dword ptr [esi + 32], ebx
    mov dword ptr [esi + 36], 0
    mov eax, 1
    shl eax, 30
    mov dword ptr [esi + 24], eax
    mov eax, 1
    shl eax, 31
    mov dword ptr [esi + 24], eax
    mov ecx, dword ptr __$$$1403$saveEcx
    add edi, ecx
    jmp $$1403$__L6
  $$1403$__L7:
.data
align 4
.code
  $$1403$done:
.data
align 4
.code
    mov eax, dword ptr __$$$1401$dmaLo
    mov dword ptr _$$DmaAddr, eax
    jmp $$1401$done
  $$1401$__L12:
    add esi, 4
    jmp $$1401$__L10
  $$1401$__L11:
    mov dword ptr _$$DmaAddr, 0
  $$1401$done:
    jmp $$1399$done
  $$1399$__L22:
  $$1399$__L19:
  $$1399$__L18:
    add esi, 16
    jmp $$1399$__L16
  $$1399$__L17:
    mov dword ptr _$$DmaAddr, 0
  $$1399$done:
    mov ecx, 0
.data
align 4
.code
    mov eax, 48
    out 43h, al
    mov eax, ecx
    out 40h, al
    shr eax, 8
    out 40h, al
    mov dword ptr __$NucleusEntryPoint$s, 0
    mov ecx, dword ptr _$$TLo
  NucleusEntryPoint$__L41:
    mov eax, ?NumStacks
    cmp dword ptr __$NucleusEntryPoint$s, eax
    jae NucleusEntryPoint$__L42
.data
align 4
.code
    mov dword ptr [ecx], ?STACK_EMPTY
    add ecx, ?TSize
    add dword ptr __$NucleusEntryPoint$s, 1
    jmp NucleusEntryPoint$__L41
  NucleusEntryPoint$__L42:
    mov dword ptr _$$CurrentStack, ?InitialStack
.data
align 4
.code
    call _?InitializeGc
    mov eax, dword ptr _$$FLo
    add eax, ?StackReserve
    add eax, ?InterruptReserve
    mov dword ptr _$$StackCheck, eax
    mov eax, dword ptr _$$FLo
    add eax, ?FSize
    sub eax, 8
    mov esp, eax
    mov edi, dword ptr _$$TLo
    mov ecx, ?KernelEntryPoint
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [esp], ecx
.data
align 4
.code
    mov dword ptr [edi], ?STACK_RUNNING
    mov ebp, 0
    ret
  NucleusEntryPoint$__L24:
    mov edx, 140
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1426$__L1
    add ecx, 3120
    jmp $$1426$__L2
  $$1426$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1426$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1427$__L1
    add ecx, 3120
    jmp $$1427$__L2
  $$1427$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1427$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1428$__L1
    add ecx, 3120
    jmp $$1428$__L2
  $$1428$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1428$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1429$__L1
    add ecx, 3120
    jmp $$1429$__L2
  $$1429$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1429$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1430$__L1
    add ecx, 3120
    jmp $$1430$__L2
  $$1430$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1430$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1431$__L1
    add ecx, 3120
    jmp $$1431$__L2
  $$1431$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1431$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1432$__L1
    add ecx, 3120
    jmp $$1432$__L2
  $$1432$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1432$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1433$__L1
    add ecx, 3120
    jmp $$1433$__L2
  $$1433$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1433$__L2:
    mov word ptr [edx + 753664], cx
  NucleusEntryPoint$__L23:
    mov eax, 1431633921
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1436$__L1
    add ecx, 3120
    jmp $$1436$__L2
  $$1436$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1436$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1437$__L1
    add ecx, 3120
    jmp $$1437$__L2
  $$1437$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1437$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1438$__L1
    add ecx, 3120
    jmp $$1438$__L2
  $$1438$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1438$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1439$__L1
    add ecx, 3120
    jmp $$1439$__L2
  $$1439$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1439$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1440$__L1
    add ecx, 3120
    jmp $$1440$__L2
  $$1440$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1440$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1441$__L1
    add ecx, 3120
    jmp $$1441$__L2
  $$1441$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1441$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1442$__L1
    add ecx, 3120
    jmp $$1442$__L2
  $$1442$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1442$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1443$__L1
    add ecx, 3120
    jmp $$1443$__L2
  $$1443$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1443$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1434$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1434$loop
    ret
_?NucleusEntryPoint endp
.data
align 4
.code
align 16
_?FaultHandler proc
.data
align 4
.code
    mov eax, dword ptr _$$CurrentStack
    mov edx, ?TSize
    mul edx
    add eax, dword ptr _$$TLo
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [eax], ?STACK_EMPTY
.data
align 4
.code
    mov ecx, dword ptr _$$FLo
    add ecx, ?StackReserve
    add ecx, ?InterruptReserve
    mov dword ptr _$$StackCheck, ecx
    mov ecx, 0
    mov edi, dword ptr _$$TLo
.data
align 4
.code
    mov ebx, dword ptr [edi]
    cmp ebx, ?STACK_YIELDED
    je FaultHandler$__L43
    mov eax, 1431633924
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1451$__L1
    add ecx, 3120
    jmp $$1451$__L2
  $$1451$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1451$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1452$__L1
    add ecx, 3120
    jmp $$1452$__L2
  $$1452$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1452$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1453$__L1
    add ecx, 3120
    jmp $$1453$__L2
  $$1453$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1453$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1454$__L1
    add ecx, 3120
    jmp $$1454$__L2
  $$1454$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1454$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1455$__L1
    add ecx, 3120
    jmp $$1455$__L2
  $$1455$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1455$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1456$__L1
    add ecx, 3120
    jmp $$1456$__L2
  $$1456$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1456$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1457$__L1
    add ecx, 3120
    jmp $$1457$__L2
  $$1457$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1457$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1458$__L1
    add ecx, 3120
    jmp $$1458$__L2
  $$1458$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1458$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1449$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1449$loop
  FaultHandler$__L43:
.data
align 4
.code
    mov dword ptr _$$CurrentStack, ecx
.data
align 4
.code
    mov esp, dword ptr [edi + 4]
.data
align 4
.code
    mov ebp, dword ptr [edi + 8]
.data
align 4
.code
    mov dword ptr [edi], ?STACK_RUNNING
.data
align 4
.code
    ret
_?FaultHandler endp
.data
align 4
.code
align 16
_?ErrorHandler proc
.data
align 4
.code
    mov eax, dword ptr _$$CurrentStack
    mov edx, ?TSize
    mul edx
    add eax, dword ptr _$$TLo
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [eax], ?STACK_EMPTY
.data
align 4
.code
    mov ecx, dword ptr _$$FLo
    add ecx, ?StackReserve
    add ecx, ?InterruptReserve
    mov dword ptr _$$StackCheck, ecx
    mov ecx, 0
    mov edi, dword ptr _$$TLo
.data
align 4
.code
    mov ebx, dword ptr [edi]
    cmp ebx, ?STACK_YIELDED
    je ErrorHandler$__L44
    mov eax, 1431633925
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1471$__L1
    add ecx, 3120
    jmp $$1471$__L2
  $$1471$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1471$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1472$__L1
    add ecx, 3120
    jmp $$1472$__L2
  $$1472$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1472$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1473$__L1
    add ecx, 3120
    jmp $$1473$__L2
  $$1473$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1473$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1474$__L1
    add ecx, 3120
    jmp $$1474$__L2
  $$1474$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1474$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1475$__L1
    add ecx, 3120
    jmp $$1475$__L2
  $$1475$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1475$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1476$__L1
    add ecx, 3120
    jmp $$1476$__L2
  $$1476$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1476$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1477$__L1
    add ecx, 3120
    jmp $$1477$__L2
  $$1477$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1477$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1478$__L1
    add ecx, 3120
    jmp $$1478$__L2
  $$1478$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1478$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1469$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1469$loop
  ErrorHandler$__L44:
.data
align 4
.code
    mov dword ptr _$$CurrentStack, ecx
.data
align 4
.code
    mov esp, dword ptr [edi + 4]
.data
align 4
.code
    mov ebp, dword ptr [edi + 8]
.data
align 4
.code
    mov dword ptr [edi], ?STACK_RUNNING
.data
align 4
.code
    ret
_?ErrorHandler endp
.data
align 4
.code
align 16
_?InterruptHandler proc
.data
align 4
__$$$1484$saveEdx DD 0
__$$$1484$saveEax DD 0
.code
    mov dword ptr __$$$1484$saveEax, eax
    mov dword ptr __$$$1484$saveEdx, edx
    mov eax, dword ptr _$$CurrentStack
    mov edx, ?TSize
    mul edx
    add eax, dword ptr _$$TLo
.data
align 4
.code
    mov dword ptr [eax], ?STACK_INTERRUPTED
.data
align 4
.code
    mov dword ptr [eax + 4], esp
.data
align 4
.code
    mov dword ptr [eax + 8], ebp
    mov edx, dword ptr __$$$1484$saveEax
.data
align 4
.code
    mov dword ptr [eax + 12], edx
.data
align 4
.code
    mov dword ptr [eax + 16], ebx
.data
align 4
.code
    mov dword ptr [eax + 20], ecx
    mov edx, dword ptr __$$$1484$saveEdx
.data
align 4
.code
    mov dword ptr [eax + 24], edx
.data
align 4
.code
    mov dword ptr [eax + 28], esi
.data
align 4
.code
    mov dword ptr [eax + 32], edi
.data
align 4
.code
.data
align 4
.code
    mov ecx, dword ptr _$$FLo
    add ecx, ?StackReserve
    add ecx, ?InterruptReserve
    mov dword ptr _$$StackCheck, ecx
    mov ecx, 0
    mov edi, dword ptr _$$TLo
.data
align 4
.code
    mov ebx, dword ptr [edi]
    cmp ebx, ?STACK_YIELDED
    je InterruptHandler$__L45
    mov eax, 1431633926
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1499$__L1
    add ecx, 3120
    jmp $$1499$__L2
  $$1499$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1499$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1500$__L1
    add ecx, 3120
    jmp $$1500$__L2
  $$1500$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1500$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1501$__L1
    add ecx, 3120
    jmp $$1501$__L2
  $$1501$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1501$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1502$__L1
    add ecx, 3120
    jmp $$1502$__L2
  $$1502$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1502$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1503$__L1
    add ecx, 3120
    jmp $$1503$__L2
  $$1503$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1503$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1504$__L1
    add ecx, 3120
    jmp $$1504$__L2
  $$1504$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1504$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1505$__L1
    add ecx, 3120
    jmp $$1505$__L2
  $$1505$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1505$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1506$__L1
    add ecx, 3120
    jmp $$1506$__L2
  $$1506$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1506$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1497$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1497$loop
  InterruptHandler$__L45:
.data
align 4
.code
    mov dword ptr _$$CurrentStack, ecx
.data
align 4
.code
    mov esp, dword ptr [edi + 4]
.data
align 4
.code
    mov ebp, dword ptr [edi + 8]
.data
align 4
.code
    mov dword ptr [edi], ?STACK_RUNNING
.data
align 4
.code
    ret
_?InterruptHandler endp
.data
align 4
.code
align 16
_?Throw proc
.data
align 4
.code
    mov eax, dword ptr _$$CurrentStack
    mov edx, ?TSize
    mul edx
    add eax, dword ptr _$$TLo
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [eax], ?STACK_EMPTY
.data
align 4
.code
    mov ecx, dword ptr _$$FLo
    add ecx, ?StackReserve
    add ecx, ?InterruptReserve
    mov dword ptr _$$StackCheck, ecx
    mov ecx, 0
    mov edi, dword ptr _$$TLo
.data
align 4
.code
    mov ebx, dword ptr [edi]
    cmp ebx, ?STACK_YIELDED
    je Throw$__L46
    mov eax, 1431633923
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1519$__L1
    add ecx, 3120
    jmp $$1519$__L2
  $$1519$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1519$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1520$__L1
    add ecx, 3120
    jmp $$1520$__L2
  $$1520$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1520$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1521$__L1
    add ecx, 3120
    jmp $$1521$__L2
  $$1521$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1521$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1522$__L1
    add ecx, 3120
    jmp $$1522$__L2
  $$1522$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1522$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1523$__L1
    add ecx, 3120
    jmp $$1523$__L2
  $$1523$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1523$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1524$__L1
    add ecx, 3120
    jmp $$1524$__L2
  $$1524$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1524$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1525$__L1
    add ecx, 3120
    jmp $$1525$__L2
  $$1525$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1525$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1526$__L1
    add ecx, 3120
    jmp $$1526$__L2
  $$1526$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1526$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1517$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1517$loop
  Throw$__L46:
.data
align 4
.code
    mov dword ptr _$$CurrentStack, ecx
.data
align 4
.code
    mov esp, dword ptr [edi + 4]
.data
align 4
.code
    mov ebp, dword ptr [edi + 8]
.data
align 4
.code
    mov dword ptr [edi], ?STACK_RUNNING
.data
align 4
.code
    ret
_?Throw endp
.data
align 4
.code
align 16
_?FatalHandler proc
    mov eax, 1431633927
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1534$__L1
    add ecx, 3120
    jmp $$1534$__L2
  $$1534$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1534$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1535$__L1
    add ecx, 3120
    jmp $$1535$__L2
  $$1535$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1535$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1536$__L1
    add ecx, 3120
    jmp $$1536$__L2
  $$1536$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1536$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1537$__L1
    add ecx, 3120
    jmp $$1537$__L2
  $$1537$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1537$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1538$__L1
    add ecx, 3120
    jmp $$1538$__L2
  $$1538$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1538$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1539$__L1
    add ecx, 3120
    jmp $$1539$__L2
  $$1539$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1539$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1540$__L1
    add ecx, 3120
    jmp $$1540$__L2
  $$1540$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1540$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1541$__L1
    add ecx, 3120
    jmp $$1541$__L2
  $$1541$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1541$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1532$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1532$loop
    ret
_?FatalHandler endp
.data
align 4
.code
align 16
_?GetStackState proc
    cmp ecx, ?NumStacks
    jb GetStackState$__L47
    mov eax, 1431633929
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1544$__L1
    add ecx, 3120
    jmp $$1544$__L2
  $$1544$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1544$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1545$__L1
    add ecx, 3120
    jmp $$1545$__L2
  $$1545$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1545$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1546$__L1
    add ecx, 3120
    jmp $$1546$__L2
  $$1546$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1546$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1547$__L1
    add ecx, 3120
    jmp $$1547$__L2
  $$1547$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1547$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1548$__L1
    add ecx, 3120
    jmp $$1548$__L2
  $$1548$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1548$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1549$__L1
    add ecx, 3120
    jmp $$1549$__L2
  $$1549$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1549$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1550$__L1
    add ecx, 3120
    jmp $$1550$__L2
  $$1550$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1550$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1551$__L1
    add ecx, 3120
    jmp $$1551$__L2
  $$1551$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1551$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1542$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1542$loop
  GetStackState$__L47:
.data
align 4
.code
    mov eax, ecx
    mov edx, ?TSize
    mul edx
    add eax, dword ptr _$$TLo
.data
align 4
.code
    mov eax, dword ptr [eax]
    ret
_?GetStackState endp
.data
align 4
.code
align 16
_?ResetStack proc
    cmp ecx, ?NumStacks
    jb ResetStack$__L48
    mov eax, 1431633929
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1556$__L1
    add ecx, 3120
    jmp $$1556$__L2
  $$1556$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1556$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1557$__L1
    add ecx, 3120
    jmp $$1557$__L2
  $$1557$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1557$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1558$__L1
    add ecx, 3120
    jmp $$1558$__L2
  $$1558$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1558$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1559$__L1
    add ecx, 3120
    jmp $$1559$__L2
  $$1559$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1559$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1560$__L1
    add ecx, 3120
    jmp $$1560$__L2
  $$1560$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1560$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1561$__L1
    add ecx, 3120
    jmp $$1561$__L2
  $$1561$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1561$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1562$__L1
    add ecx, 3120
    jmp $$1562$__L2
  $$1562$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1562$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1563$__L1
    add ecx, 3120
    jmp $$1563$__L2
  $$1563$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1563$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1554$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1554$loop
  ResetStack$__L48:
.data
align 4
.code
    mov eax, ecx
    mov edx, ?TSize
    mul edx
    add eax, dword ptr _$$TLo
.data
align 4
.code
    mov ebx, dword ptr [eax]
    cmp ebx, ?STACK_RUNNING
    jne ResetStack$__L49
    mov eax, 1431633930
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1568$__L1
    add ecx, 3120
    jmp $$1568$__L2
  $$1568$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1568$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1569$__L1
    add ecx, 3120
    jmp $$1569$__L2
  $$1569$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1569$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1570$__L1
    add ecx, 3120
    jmp $$1570$__L2
  $$1570$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1570$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1571$__L1
    add ecx, 3120
    jmp $$1571$__L2
  $$1571$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1571$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1572$__L1
    add ecx, 3120
    jmp $$1572$__L2
  $$1572$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1572$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1573$__L1
    add ecx, 3120
    jmp $$1573$__L2
  $$1573$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1573$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1574$__L1
    add ecx, 3120
    jmp $$1574$__L2
  $$1574$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1574$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1575$__L1
    add ecx, 3120
    jmp $$1575$__L2
  $$1575$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1575$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1566$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1566$loop
  ResetStack$__L49:
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [eax], ?STACK_EMPTY
    ret
_?ResetStack endp
.data
align 4
.code
align 16
_?YieldTo proc
    cmp ecx, ?NumStacks
    jb YieldTo$__L50
    mov eax, 1431633928
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1580$__L1
    add ecx, 3120
    jmp $$1580$__L2
  $$1580$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1580$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1581$__L1
    add ecx, 3120
    jmp $$1581$__L2
  $$1581$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1581$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1582$__L1
    add ecx, 3120
    jmp $$1582$__L2
  $$1582$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1582$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1583$__L1
    add ecx, 3120
    jmp $$1583$__L2
  $$1583$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1583$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1584$__L1
    add ecx, 3120
    jmp $$1584$__L2
  $$1584$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1584$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1585$__L1
    add ecx, 3120
    jmp $$1585$__L2
  $$1585$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1585$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1586$__L1
    add ecx, 3120
    jmp $$1586$__L2
  $$1586$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1586$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1587$__L1
    add ecx, 3120
    jmp $$1587$__L2
  $$1587$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1587$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1578$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1578$loop
  YieldTo$__L50:
.data
align 4
.code
    mov eax, ecx
    mov edx, ?FSize
    mul edx
    add eax, dword ptr _$$FLo
    add eax, ?StackReserve
    add eax, ?InterruptReserve
    mov dword ptr _$$StackCheck, eax
    mov eax, ecx
    mov edx, ?TSize
    mul edx
    add eax, dword ptr _$$TLo
.data
align 4
.code
    mov ebx, dword ptr [eax]
    mov edi, eax
    mov eax, dword ptr _$$CurrentStack
    mov edx, ?TSize
    mul edx
    add eax, dword ptr _$$TLo
    cmp ebx, ?STACK_YIELDED
    jne YieldTo$__L51
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [eax], ?STACK_YIELDED
.data
align 4
.code
    mov dword ptr [eax + 4], esp
.data
align 4
.code
    mov dword ptr [eax + 8], ebp
.data
align 4
.code
.data
align 4
.code
    mov dword ptr _$$CurrentStack, ecx
.data
align 4
.code
    mov esp, dword ptr [edi + 4]
.data
align 4
.code
    mov ebp, dword ptr [edi + 8]
.data
align 4
.code
    mov dword ptr [edi], ?STACK_RUNNING
.data
align 4
.code
    ret
    jmp YieldTo$__L52
  YieldTo$__L51:
    cmp ebx, ?STACK_INTERRUPTED
    jne YieldTo$__L53
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [eax], ?STACK_YIELDED
.data
align 4
.code
    mov dword ptr [eax + 4], esp
.data
align 4
.code
    mov dword ptr [eax + 8], ebp
.data
align 4
.code
.data
align 4
.code
    mov dword ptr _$$CurrentStack, ecx
.data
align 4
.code
    mov dword ptr [edi], ?STACK_RUNNING
.data
align 4
.code
    mov esp, dword ptr [edi + 4]
.data
align 4
.code
    mov ebp, dword ptr [edi + 8]
.data
align 4
.code
    mov eax, dword ptr [edi + 12]
.data
align 4
.code
    mov ebx, dword ptr [edi + 16]
.data
align 4
.code
    mov ecx, dword ptr [edi + 20]
.data
align 4
.code
    mov edx, dword ptr [edi + 24]
.data
align 4
.code
    mov esi, dword ptr [edi + 28]
.data
align 4
.code
    mov edi, dword ptr [edi + 32]
.data
align 4
.code
    iretd
    jmp YieldTo$__L54
  YieldTo$__L53:
    cmp ebx, ?STACK_EMPTY
    jne YieldTo$__L55
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [eax], ?STACK_YIELDED
.data
align 4
.code
    mov dword ptr [eax + 4], esp
.data
align 4
.code
    mov dword ptr [eax + 8], ebp
.data
align 4
.code
    mov eax, ecx
    mov edx, ?FSize
    mul edx
    add eax, dword ptr _$$FLo
    lea esp, dword ptr [eax + 16376]
    mov eax, ?KernelEntryPoint
.data
align 4
.code
    mov dword ptr _$$CurrentStack, ecx
.data
align 4
.code
    mov dword ptr [esp], eax
.data
align 4
.code
    mov dword ptr [edi], ?STACK_RUNNING
    mov ebp, 0
    ret
  YieldTo$__L55:
  YieldTo$__L54:
  YieldTo$__L52:
    ret
_?YieldTo endp
.data
align 4
.code
align 16
_?VgaTextWrite proc
    cmp ecx, 4000
    jae VgaTextWrite$__L56
    mov word ptr [ecx + 1 * ecx + 753664], dx
  VgaTextWrite$__L56:
    ret
_?VgaTextWrite endp
.data
align 4
.code
align 16
_?TryReadKeyboard proc
    in al, 064h
    and eax, 1
    cmp eax, 0
    jne TryReadKeyboard$skip
    mov eax, 256
    ret
  TryReadKeyboard$skip:
    in al, 060h
    and eax, 255
    ret
_?TryReadKeyboard endp
.data
align 4
.code
align 16
_?StartTimer proc
.data
align 4
.code
    mov eax, 48
    out 43h, al
    mov eax, ecx
    out 40h, al
    shr eax, 8
    out 40h, al
    ret
_?StartTimer endp
.data
align 4
.code
align 16
_?SendEoi proc
    mov eax, 32
    mov edx, 32
    out dx, al
    mov eax, 32
    mov edx, 160
    out dx, al
    ret
_?SendEoi endp
.data
align 4
.code
align 16
_?PciConfigRead32 proc
    cmp ecx, 65536
    jb PciConfigRead32$__L57
    mov eax, 1431633932
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1627$__L1
    add ecx, 3120
    jmp $$1627$__L2
  $$1627$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1627$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1628$__L1
    add ecx, 3120
    jmp $$1628$__L2
  $$1628$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1628$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1629$__L1
    add ecx, 3120
    jmp $$1629$__L2
  $$1629$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1629$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1630$__L1
    add ecx, 3120
    jmp $$1630$__L2
  $$1630$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1630$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1631$__L1
    add ecx, 3120
    jmp $$1631$__L2
  $$1631$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1631$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1632$__L1
    add ecx, 3120
    jmp $$1632$__L2
  $$1632$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1632$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1633$__L1
    add ecx, 3120
    jmp $$1633$__L2
  $$1633$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1633$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1634$__L1
    add ecx, 3120
    jmp $$1634$__L2
  $$1634$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1634$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1625$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1625$loop
  PciConfigRead32$__L57:
    cmp edx, 256
    jb PciConfigRead32$__L58
    mov eax, 1431633933
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1637$__L1
    add ecx, 3120
    jmp $$1637$__L2
  $$1637$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1637$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1638$__L1
    add ecx, 3120
    jmp $$1638$__L2
  $$1638$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1638$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1639$__L1
    add ecx, 3120
    jmp $$1639$__L2
  $$1639$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1639$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1640$__L1
    add ecx, 3120
    jmp $$1640$__L2
  $$1640$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1640$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1641$__L1
    add ecx, 3120
    jmp $$1641$__L2
  $$1641$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1641$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1642$__L1
    add ecx, 3120
    jmp $$1642$__L2
  $$1642$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1642$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1643$__L1
    add ecx, 3120
    jmp $$1643$__L2
  $$1643$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1643$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1644$__L1
    add ecx, 3120
    jmp $$1644$__L2
  $$1644$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1644$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1635$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1635$loop
  PciConfigRead32$__L58:
    mov eax, edx
    and eax, 3
    cmp eax, 0
    je PciConfigRead32$__L59
    mov eax, 1431633934
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1647$__L1
    add ecx, 3120
    jmp $$1647$__L2
  $$1647$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1647$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1648$__L1
    add ecx, 3120
    jmp $$1648$__L2
  $$1648$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1648$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1649$__L1
    add ecx, 3120
    jmp $$1649$__L2
  $$1649$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1649$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1650$__L1
    add ecx, 3120
    jmp $$1650$__L2
  $$1650$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1650$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1651$__L1
    add ecx, 3120
    jmp $$1651$__L2
  $$1651$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1651$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1652$__L1
    add ecx, 3120
    jmp $$1652$__L2
  $$1652$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1652$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1653$__L1
    add ecx, 3120
    jmp $$1653$__L2
  $$1653$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1653$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1654$__L1
    add ecx, 3120
    jmp $$1654$__L2
  $$1654$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1654$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1645$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1645$loop
  PciConfigRead32$__L59:
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov eax, ecx
    shl eax, 8
    or eax, edx
    mov edx, 2147483647
    add edx, 1
    jc _?Overflow
    or eax, edx
    mov edx, 3320
    out dx, eax
    mov edx, 3324
    in eax, dx
    ret
_?PciConfigRead32 endp
.data
align 4
__$PciMemSetup$sizeBits DD 0
__$PciMemSetup$config16 DD 0
__$PciMemSetup$config12 DD 0
__$PciMemSetup$config4 DD 0
__$PciMemSetup$config0 DD 0
__$PciMemSetup$tableEntry DD 0
__$PciMemSetup$ffffffff DD 0
.code
align 16
_?PciMemSetup proc
    mov eax, 2147483647
    add eax, 2147483647
    add eax, 1
    mov dword ptr __$PciMemSetup$ffffffff, eax
.data
align 4
.code
.data
align 4
.code
    cmp ecx, 65536
    jb PciMemSetup$__L60
    mov eax, 1431633935
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1689$__L1
    add ecx, 3120
    jmp $$1689$__L2
  $$1689$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1689$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1690$__L1
    add ecx, 3120
    jmp $$1690$__L2
  $$1690$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1690$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1691$__L1
    add ecx, 3120
    jmp $$1691$__L2
  $$1691$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1691$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1692$__L1
    add ecx, 3120
    jmp $$1692$__L2
  $$1692$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1692$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1693$__L1
    add ecx, 3120
    jmp $$1693$__L2
  $$1693$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1693$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1694$__L1
    add ecx, 3120
    jmp $$1694$__L2
  $$1694$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1694$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1695$__L1
    add ecx, 3120
    jmp $$1695$__L2
  $$1695$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1695$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1696$__L1
    add ecx, 3120
    jmp $$1696$__L2
  $$1696$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1696$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1687$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1687$loop
  PciMemSetup$__L60:
    mov eax, ecx
    and eax, 7
    cmp eax, 0
    je PciMemSetup$__L61
    mov eax, 1431633952
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1699$__L1
    add ecx, 3120
    jmp $$1699$__L2
  $$1699$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1699$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1700$__L1
    add ecx, 3120
    jmp $$1700$__L2
  $$1700$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1700$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1701$__L1
    add ecx, 3120
    jmp $$1701$__L2
  $$1701$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1701$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1702$__L1
    add ecx, 3120
    jmp $$1702$__L2
  $$1702$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1702$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1703$__L1
    add ecx, 3120
    jmp $$1703$__L2
  $$1703$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1703$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1704$__L1
    add ecx, 3120
    jmp $$1704$__L2
  $$1704$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1704$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1705$__L1
    add ecx, 3120
    jmp $$1705$__L2
  $$1705$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1705$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1706$__L1
    add ecx, 3120
    jmp $$1706$__L2
  $$1706$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1706$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1697$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1697$loop
  PciMemSetup$__L61:
    mov eax, ecx
    add eax, eax
    add eax, eax
    add eax, eax
    add eax, dword ptr _$$PciLo
    mov dword ptr __$PciMemSetup$tableEntry, eax
    mov eax, dword ptr __$PciMemSetup$tableEntry
.data
align 4
.code
    mov eax, dword ptr [eax]
    cmp eax, 0
    je PciMemSetup$__L62
    mov eax, 1431633956
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1710$__L1
    add ecx, 3120
    jmp $$1710$__L2
  $$1710$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1710$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1711$__L1
    add ecx, 3120
    jmp $$1711$__L2
  $$1711$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1711$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1712$__L1
    add ecx, 3120
    jmp $$1712$__L2
  $$1712$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1712$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1713$__L1
    add ecx, 3120
    jmp $$1713$__L2
  $$1713$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1713$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1714$__L1
    add ecx, 3120
    jmp $$1714$__L2
  $$1714$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1714$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1715$__L1
    add ecx, 3120
    jmp $$1715$__L2
  $$1715$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1715$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1716$__L1
    add ecx, 3120
    jmp $$1716$__L2
  $$1716$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1716$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1717$__L1
    add ecx, 3120
    jmp $$1717$__L2
  $$1717$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1717$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1708$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1708$loop
  PciMemSetup$__L62:
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov edx, 0
.data
align 4
.code
    mov eax, ecx
    shl eax, 8
    or eax, edx
    mov edx, 2147483647
    add edx, 1
    jc _?Overflow
    or eax, edx
    mov edx, 3320
    out dx, eax
    mov edx, 3324
    in eax, dx
    mov dword ptr __$PciMemSetup$config0, eax
    mov edx, 4
.data
align 4
.code
    mov eax, ecx
    shl eax, 8
    or eax, edx
    mov edx, 2147483647
    add edx, 1
    jc _?Overflow
    or eax, edx
    mov edx, 3320
    out dx, eax
    mov edx, 3324
    in eax, dx
    mov dword ptr __$PciMemSetup$config4, eax
    mov edx, 12
.data
align 4
.code
    mov eax, ecx
    shl eax, 8
    or eax, edx
    mov edx, 2147483647
    add edx, 1
    jc _?Overflow
    or eax, edx
    mov edx, 3320
    out dx, eax
    mov edx, 3324
    in eax, dx
    mov dword ptr __$PciMemSetup$config12, eax
    mov edx, 16
.data
align 4
.code
    mov eax, ecx
    shl eax, 8
    or eax, edx
    mov edx, 2147483647
    add edx, 1
    jc _?Overflow
    or eax, edx
    mov edx, 3320
    out dx, eax
    mov edx, 3324
    in eax, dx
    mov dword ptr __$PciMemSetup$config16, eax
    cmp eax, 0
    jne PciMemSetup$__L63
    mov eax, 1431633957
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1753$__L1
    add ecx, 3120
    jmp $$1753$__L2
  $$1753$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1753$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1754$__L1
    add ecx, 3120
    jmp $$1754$__L2
  $$1754$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1754$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1755$__L1
    add ecx, 3120
    jmp $$1755$__L2
  $$1755$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1755$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1756$__L1
    add ecx, 3120
    jmp $$1756$__L2
  $$1756$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1756$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1757$__L1
    add ecx, 3120
    jmp $$1757$__L2
  $$1757$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1757$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1758$__L1
    add ecx, 3120
    jmp $$1758$__L2
  $$1758$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1758$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1759$__L1
    add ecx, 3120
    jmp $$1759$__L2
  $$1759$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1759$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1760$__L1
    add ecx, 3120
    jmp $$1760$__L2
  $$1760$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1760$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1751$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1751$loop
  PciMemSetup$__L63:
    mov eax, dword ptr __$PciMemSetup$config0
    and eax, 65535
    cmp eax, 65535
    jne PciMemSetup$__L64
    mov eax, 1431633953
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1763$__L1
    add ecx, 3120
    jmp $$1763$__L2
  $$1763$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1763$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1764$__L1
    add ecx, 3120
    jmp $$1764$__L2
  $$1764$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1764$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1765$__L1
    add ecx, 3120
    jmp $$1765$__L2
  $$1765$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1765$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1766$__L1
    add ecx, 3120
    jmp $$1766$__L2
  $$1766$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1766$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1767$__L1
    add ecx, 3120
    jmp $$1767$__L2
  $$1767$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1767$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1768$__L1
    add ecx, 3120
    jmp $$1768$__L2
  $$1768$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1768$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1769$__L1
    add ecx, 3120
    jmp $$1769$__L2
  $$1769$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1769$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1770$__L1
    add ecx, 3120
    jmp $$1770$__L2
  $$1770$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1770$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1761$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1761$loop
  PciMemSetup$__L64:
    mov eax, dword ptr __$PciMemSetup$config12
    shr eax, 16
    and eax, 255
    cmp eax, 0
    je PciMemSetup$__L65
    mov eax, 1431633954
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1773$__L1
    add ecx, 3120
    jmp $$1773$__L2
  $$1773$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1773$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1774$__L1
    add ecx, 3120
    jmp $$1774$__L2
  $$1774$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1774$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1775$__L1
    add ecx, 3120
    jmp $$1775$__L2
  $$1775$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1775$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1776$__L1
    add ecx, 3120
    jmp $$1776$__L2
  $$1776$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1776$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1777$__L1
    add ecx, 3120
    jmp $$1777$__L2
  $$1777$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1777$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1778$__L1
    add ecx, 3120
    jmp $$1778$__L2
  $$1778$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1778$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1779$__L1
    add ecx, 3120
    jmp $$1779$__L2
  $$1779$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1779$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1780$__L1
    add ecx, 3120
    jmp $$1780$__L2
  $$1780$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1780$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1771$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1771$loop
  PciMemSetup$__L65:
    mov eax, dword ptr __$PciMemSetup$config16
    and eax, 15
    cmp eax, 0
    je PciMemSetup$__L66
    mov eax, 1431633955
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1783$__L1
    add ecx, 3120
    jmp $$1783$__L2
  $$1783$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1783$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1784$__L1
    add ecx, 3120
    jmp $$1784$__L2
  $$1784$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1784$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1785$__L1
    add ecx, 3120
    jmp $$1785$__L2
  $$1785$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1785$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1786$__L1
    add ecx, 3120
    jmp $$1786$__L2
  $$1786$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1786$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1787$__L1
    add ecx, 3120
    jmp $$1787$__L2
  $$1787$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1787$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1788$__L1
    add ecx, 3120
    jmp $$1788$__L2
  $$1788$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1788$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1789$__L1
    add ecx, 3120
    jmp $$1789$__L2
  $$1789$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1789$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1790$__L1
    add ecx, 3120
    jmp $$1790$__L2
  $$1790$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1790$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1781$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1781$loop
  PciMemSetup$__L66:
    mov edx, 4
.data
align 4
.code
    mov eax, ecx
    shl eax, 8
    or eax, edx
    mov edx, 2147483647
    add edx, 1
    jc _?Overflow
    or eax, edx
    mov edx, 3320
    out dx, eax
    mov edx, 3324
    mov eax, dword ptr __$PciMemSetup$ffffffff
    sub eax, 2
    mov ebx, eax
    mov eax, dword ptr __$PciMemSetup$config4
    and eax, ebx
    out dx, eax
    mov edx, 16
.data
align 4
.code
    mov eax, ecx
    shl eax, 8
    or eax, edx
    mov edx, 2147483647
    add edx, 1
    jc _?Overflow
    or eax, edx
    mov edx, 3320
    out dx, eax
    mov edx, 3324
    mov eax, dword ptr __$PciMemSetup$ffffffff
    out dx, eax
    mov edx, 16
.data
align 4
.code
    mov eax, ecx
    shl eax, 8
    or eax, edx
    mov edx, 2147483647
    add edx, 1
    jc _?Overflow
    or eax, edx
    mov edx, 3320
    out dx, eax
    mov edx, 3324
    in eax, dx
    mov dword ptr __$PciMemSetup$sizeBits, eax
    mov edx, 16
.data
align 4
.code
    mov eax, ecx
    shl eax, 8
    or eax, edx
    mov edx, 2147483647
    add edx, 1
    jc _?Overflow
    or eax, edx
    mov edx, 3320
    out dx, eax
    mov edx, 3324
    mov eax, dword ptr __$PciMemSetup$config16
    out dx, eax
    mov edx, 4
.data
align 4
.code
    mov eax, ecx
    shl eax, 8
    or eax, edx
    mov edx, 2147483647
    add edx, 1
    jc _?Overflow
    or eax, edx
    mov edx, 3320
    out dx, eax
    mov edx, 3324
    mov eax, dword ptr __$PciMemSetup$config4
    or eax, 2
    out dx, eax
    mov eax, dword ptr __$PciMemSetup$sizeBits
    not eax
    add eax, 1
    jc _?Overflow
    mov edx, dword ptr __$PciMemSetup$tableEntry
    mov ecx, dword ptr __$PciMemSetup$config16
.data
align 4
.code
    mov dword ptr [edx + 0], ecx
.data
align 4
.code
    mov dword ptr [edx + 4], eax
.data
align 4
.code
.data
align 4
.code
    ret
_?PciMemSetup endp
.data
align 4
.code
align 16
_?PciDmaBuffer proc
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr _$$DmaAddr
    cmp eax, 0
    je PciDmaBuffer$__L67
    sub eax, 8
  PciDmaBuffer$__L67:
    ret
_?PciDmaBuffer endp
.data
align 4
.code
align 16
_?PciDmaPhysicalAddr proc
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr _$$DmaAddr
    ret
_?PciDmaPhysicalAddr endp
.data
align 4
.code
align 16
_?PciMemRead32 proc
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr _$$DmaAddr
    cmp eax, 0
    jne PciMemRead32$__L68
    mov eax, 1431633962
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1808$__L1
    add ecx, 3120
    jmp $$1808$__L2
  $$1808$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1808$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1809$__L1
    add ecx, 3120
    jmp $$1809$__L2
  $$1809$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1809$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1810$__L1
    add ecx, 3120
    jmp $$1810$__L2
  $$1810$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1810$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1811$__L1
    add ecx, 3120
    jmp $$1811$__L2
  $$1811$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1811$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1812$__L1
    add ecx, 3120
    jmp $$1812$__L2
  $$1812$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1812$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1813$__L1
    add ecx, 3120
    jmp $$1813$__L2
  $$1813$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1813$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1814$__L1
    add ecx, 3120
    jmp $$1814$__L2
  $$1814$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1814$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1815$__L1
    add ecx, 3120
    jmp $$1815$__L2
  $$1815$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1815$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1806$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1806$loop
  PciMemRead32$__L68:
    cmp ecx, 65536
    jb PciMemRead32$__L69
    mov eax, 1431633959
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1818$__L1
    add ecx, 3120
    jmp $$1818$__L2
  $$1818$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1818$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1819$__L1
    add ecx, 3120
    jmp $$1819$__L2
  $$1819$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1819$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1820$__L1
    add ecx, 3120
    jmp $$1820$__L2
  $$1820$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1820$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1821$__L1
    add ecx, 3120
    jmp $$1821$__L2
  $$1821$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1821$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1822$__L1
    add ecx, 3120
    jmp $$1822$__L2
  $$1822$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1822$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1823$__L1
    add ecx, 3120
    jmp $$1823$__L2
  $$1823$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1823$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1824$__L1
    add ecx, 3120
    jmp $$1824$__L2
  $$1824$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1824$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1825$__L1
    add ecx, 3120
    jmp $$1825$__L2
  $$1825$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1825$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1816$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1816$loop
  PciMemRead32$__L69:
    mov ebx, dword ptr _$$PciLo
.data
align 4
.code
    mov esi, dword ptr [ebx + 8 * ecx + 0]
    cmp esi, 0
    jne PciMemRead32$__L70
    mov eax, 1431633960
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1829$__L1
    add ecx, 3120
    jmp $$1829$__L2
  $$1829$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1829$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1830$__L1
    add ecx, 3120
    jmp $$1830$__L2
  $$1830$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1830$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1831$__L1
    add ecx, 3120
    jmp $$1831$__L2
  $$1831$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1831$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1832$__L1
    add ecx, 3120
    jmp $$1832$__L2
  $$1832$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1832$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1833$__L1
    add ecx, 3120
    jmp $$1833$__L2
  $$1833$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1833$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1834$__L1
    add ecx, 3120
    jmp $$1834$__L2
  $$1834$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1834$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1835$__L1
    add ecx, 3120
    jmp $$1835$__L2
  $$1835$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1835$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1836$__L1
    add ecx, 3120
    jmp $$1836$__L2
  $$1836$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1836$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1827$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1827$loop
  PciMemRead32$__L70:
.data
align 4
.code
    mov edi, dword ptr [ebx + 8 * ecx + 4]
    mov eax, edx
    add eax, 4
    jc _?Overflow
    cmp eax, edi
    jbe PciMemRead32$__L71
    mov eax, 1431633961
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1840$__L1
    add ecx, 3120
    jmp $$1840$__L2
  $$1840$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1840$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1841$__L1
    add ecx, 3120
    jmp $$1841$__L2
  $$1841$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1841$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1842$__L1
    add ecx, 3120
    jmp $$1842$__L2
  $$1842$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1842$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1843$__L1
    add ecx, 3120
    jmp $$1843$__L2
  $$1843$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1843$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1844$__L1
    add ecx, 3120
    jmp $$1844$__L2
  $$1844$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1844$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1845$__L1
    add ecx, 3120
    jmp $$1845$__L2
  $$1845$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1845$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1846$__L1
    add ecx, 3120
    jmp $$1846$__L2
  $$1846$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1846$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1847$__L1
    add ecx, 3120
    jmp $$1847$__L2
  $$1847$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1847$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1838$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1838$loop
  PciMemRead32$__L71:
    mov eax, dword ptr [esi + 1 * edx + 0]
    ret
_?PciMemRead32 endp
.data
align 4
__$PciMemWrite32$val DD 0
.code
align 16
_?PciMemWrite32 proc
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov eax, dword ptr [esp + 4]
    mov dword ptr __$PciMemWrite32$val, eax
    mov eax, dword ptr _$$DmaAddr
    cmp eax, 0
    jne PciMemWrite32$__L72
    mov eax, 1431633963
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1853$__L1
    add ecx, 3120
    jmp $$1853$__L2
  $$1853$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1853$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1854$__L1
    add ecx, 3120
    jmp $$1854$__L2
  $$1854$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1854$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1855$__L1
    add ecx, 3120
    jmp $$1855$__L2
  $$1855$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1855$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1856$__L1
    add ecx, 3120
    jmp $$1856$__L2
  $$1856$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1856$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1857$__L1
    add ecx, 3120
    jmp $$1857$__L2
  $$1857$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1857$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1858$__L1
    add ecx, 3120
    jmp $$1858$__L2
  $$1858$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1858$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1859$__L1
    add ecx, 3120
    jmp $$1859$__L2
  $$1859$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1859$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1860$__L1
    add ecx, 3120
    jmp $$1860$__L2
  $$1860$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1860$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1851$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1851$loop
  PciMemWrite32$__L72:
    cmp ecx, 65536
    jb PciMemWrite32$__L73
    mov eax, 1431633964
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1863$__L1
    add ecx, 3120
    jmp $$1863$__L2
  $$1863$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1863$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1864$__L1
    add ecx, 3120
    jmp $$1864$__L2
  $$1864$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1864$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1865$__L1
    add ecx, 3120
    jmp $$1865$__L2
  $$1865$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1865$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1866$__L1
    add ecx, 3120
    jmp $$1866$__L2
  $$1866$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1866$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1867$__L1
    add ecx, 3120
    jmp $$1867$__L2
  $$1867$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1867$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1868$__L1
    add ecx, 3120
    jmp $$1868$__L2
  $$1868$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1868$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1869$__L1
    add ecx, 3120
    jmp $$1869$__L2
  $$1869$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1869$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1870$__L1
    add ecx, 3120
    jmp $$1870$__L2
  $$1870$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1870$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1861$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1861$loop
  PciMemWrite32$__L73:
    mov ebx, dword ptr _$$PciLo
.data
align 4
.code
    mov esi, dword ptr [ebx + 8 * ecx + 0]
    cmp esi, 0
    jne PciMemWrite32$__L74
    mov eax, 1431633965
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1874$__L1
    add ecx, 3120
    jmp $$1874$__L2
  $$1874$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1874$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1875$__L1
    add ecx, 3120
    jmp $$1875$__L2
  $$1875$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1875$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1876$__L1
    add ecx, 3120
    jmp $$1876$__L2
  $$1876$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1876$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1877$__L1
    add ecx, 3120
    jmp $$1877$__L2
  $$1877$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1877$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1878$__L1
    add ecx, 3120
    jmp $$1878$__L2
  $$1878$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1878$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1879$__L1
    add ecx, 3120
    jmp $$1879$__L2
  $$1879$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1879$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1880$__L1
    add ecx, 3120
    jmp $$1880$__L2
  $$1880$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1880$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1881$__L1
    add ecx, 3120
    jmp $$1881$__L2
  $$1881$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1881$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1872$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1872$loop
  PciMemWrite32$__L74:
.data
align 4
.code
    mov edi, dword ptr [ebx + 8 * ecx + 4]
    mov eax, edx
    add eax, 4
    jc _?Overflow
    cmp eax, edi
    jbe PciMemWrite32$__L75
    mov eax, 1431633966
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1885$__L1
    add ecx, 3120
    jmp $$1885$__L2
  $$1885$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1885$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1886$__L1
    add ecx, 3120
    jmp $$1886$__L2
  $$1886$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1886$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1887$__L1
    add ecx, 3120
    jmp $$1887$__L2
  $$1887$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1887$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1888$__L1
    add ecx, 3120
    jmp $$1888$__L2
  $$1888$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1888$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1889$__L1
    add ecx, 3120
    jmp $$1889$__L2
  $$1889$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1889$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1890$__L1
    add ecx, 3120
    jmp $$1890$__L2
  $$1890$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1890$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1891$__L1
    add ecx, 3120
    jmp $$1891$__L2
  $$1891$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1891$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1892$__L1
    add ecx, 3120
    jmp $$1892$__L2
  $$1892$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1892$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1883$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1883$loop
  PciMemWrite32$__L75:
    mov eax, dword ptr __$PciMemWrite32$val
    mov dword ptr [esi + 1 * edx + 0], eax
    ret
_?PciMemWrite32 endp
.data
align 4
.code
align 16
_?CycleCounter proc
    rdtsc
    ret
_?CycleCounter endp
.data
align 4
.code
align 16
_?DebugPrintHex proc
    cmp ecx, 72
    jb DebugPrintHex$__L76
    mov eax, 1431633931
.data
align 4
.code
    mov edx, 2
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1895$__L1
    add ecx, 3120
    jmp $$1895$__L2
  $$1895$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1895$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1896$__L1
    add ecx, 3120
    jmp $$1896$__L2
  $$1896$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1896$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1897$__L1
    add ecx, 3120
    jmp $$1897$__L2
  $$1897$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1897$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1898$__L1
    add ecx, 3120
    jmp $$1898$__L2
  $$1898$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1898$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1899$__L1
    add ecx, 3120
    jmp $$1899$__L2
  $$1899$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1899$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1900$__L1
    add ecx, 3120
    jmp $$1900$__L2
  $$1900$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1900$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1901$__L1
    add ecx, 3120
    jmp $$1901$__L2
  $$1901$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1901$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1902$__L1
    add ecx, 3120
    jmp $$1902$__L2
  $$1902$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1902$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$1893$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$1893$loop
  DebugPrintHex$__L76:
    mov eax, edx
    lea edx, dword ptr [ecx + 1 * ecx + 0]
.data
align 4
.code
    add edx, 14
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1904$__L1
    add ecx, 3120
    jmp $$1904$__L2
  $$1904$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1904$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1905$__L1
    add ecx, 3120
    jmp $$1905$__L2
  $$1905$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1905$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1906$__L1
    add ecx, 3120
    jmp $$1906$__L2
  $$1906$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1906$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1907$__L1
    add ecx, 3120
    jmp $$1907$__L2
  $$1907$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1907$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1908$__L1
    add ecx, 3120
    jmp $$1908$__L2
  $$1908$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1908$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1909$__L1
    add ecx, 3120
    jmp $$1909$__L2
  $$1909$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1909$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1910$__L1
    add ecx, 3120
    jmp $$1910$__L2
  $$1910$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1910$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$1911$__L1
    add ecx, 3120
    jmp $$1911$__L2
  $$1911$__L1:
    add ecx, 3127
    jc _?Overflow
  $$1911$__L2:
    mov word ptr [edx + 753664], cx
    ret
_?DebugPrintHex endp
end
;; boogie instructions  (before inlining): 1520
;;    x86 instructions  (before inlining): 1747
;; unchecked arithmetic (before inlining): 163
;;   checked arithmetic (before inlining): 69

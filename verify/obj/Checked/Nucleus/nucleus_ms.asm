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
;;parsing ..\..\..\obj\Checked\Nucleus\\GC\MarkSweepCollector_i.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\GC\MarkSweepCollector.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\Main\EntryMS_i.bpl
;;parsing ..\..\..\obj\Checked\Nucleus\\Main\EntryMS.bpl
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
_$$StackTop DD 0
.data
align 4
_$$CachePtr DD 0
.data
align 4
_$$CacheSize DD 0
.data
align 4
_$$ColorBase DD 0
.data
align 4
_$$HeapLo DD 0
.data
align 4
_$$HeapHi DD 0
.data
align 4
_$$ReserveMin DD 0
.data
align 4
.code
align 16
_?BB4Zero2 proc
    mov esi, eax
  BB4Zero2$loop:
    cmp esi, ebx
    jae BB4Zero2$loopEnd
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    jmp BB4Zero2$loop
  BB4Zero2$loopEnd:
.data
align 4
.code
    ret
_?BB4Zero2 endp
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
    sub ebx, dword ptr _$$HeapLo
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
    mov eax, dword ptr _$$ColorBase
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, ebx
    shr ecx, 6
    lea edx, dword ptr [eax + 4 * ecx + 0]
    mov ecx, ebx
    shr ecx, 1
    and ecx, 31
.data
align 4
.code
    mov edx, dword ptr [edx]
    mov ebx, edx
    shr ebx, cl
    and ebx, 3
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
.code
align 16
_?AllocBlock proc
    mov edi, dword ptr _$$HeapLo
  AllocBlock$loop:
    mov esi, edi
.data
align 4
.code
    mov edi, dword ptr [edi]
    cmp edi, 0
    jne AllocBlock$notEmpty
    mov eax, 0
.data
align 4
.code
    ret
  AllocBlock$notEmpty:
.data
align 4
.code
    mov ebx, dword ptr [edi + 4]
    cmp ebx, ecx
    jb AllocBlock$loop
    mov ebp, ebx
    sub ebp, 8
    cmp ebp, edx
    jb AllocBlock$skip1
    lea eax, dword ptr [edi + 1 * ebx + 0]
    sub eax, edx
    cmp eax, dword ptr _$$ReserveMin
    jb AllocBlock$loop
    sub ebx, edx
    cmp ebx, 256
    jb AllocBlock$skip1
.data
align 4
.code
    mov dword ptr [eax], 0
.data
align 4
.code
    mov dword ptr [eax + 4], edx
.data
align 4
.code
    mov dword ptr [edi + 4], ebx
.data
align 4
.code
    ret
  AllocBlock$skip1:
    cmp edi, dword ptr _$$ReserveMin
    jb AllocBlock$loop
.data
align 4
.code
    mov ebx, dword ptr [edi]
.data
align 4
.code
    mov dword ptr [esi], ebx
.data
align 4
.code
    mov dword ptr [edi], 0
    mov eax, edi
.data
align 4
.code
    ret
_?AllocBlock endp
.data
align 4
__$AllocSlow$size DD 0
.code
align 16
_?AllocSlow proc
    cmp ecx, 192
    jb AllocSlow$skip1
    mov edx, ecx
.data
align 4
.code
    call _?AllocBlock
.data
align 4
.code
    ret
  AllocSlow$skip1:
    mov dword ptr __$AllocSlow$size, ecx
    mov ecx, 256
    mov edx, 65536
.data
align 4
.code
    call _?AllocBlock
    cmp eax, 0
    je AllocSlow$skip2
    mov dword ptr _$$CachePtr, eax
    mov ecx, dword ptr _$$CachePtr
.data
align 4
.code
    mov ecx, dword ptr [ecx + 4]
    mov dword ptr _$$CacheSize, ecx
    mov ecx, dword ptr __$AllocSlow$size
.data
align 4
.code
    sub dword ptr _$$CacheSize, ecx
    mov eax, dword ptr _$$CacheSize
    mov edx, dword ptr _$$CachePtr
.data
align 4
.code
    mov dword ptr [edx + 4], eax
    add eax, edx
.data
align 4
.code
    mov dword ptr [eax], 0
.data
align 4
.code
    mov dword ptr [eax + 4], ecx
  AllocSlow$skip2:
.data
align 4
.code
    ret
_?AllocSlow endp
.data
align 4
.code
align 16
_?Sweep proc
    mov dword ptr _$$CacheSize, 0
    mov edi, dword ptr _$$HeapLo
    lea esi, dword ptr [edi + 8]
    mov ebp, esi
.data
align 4
.code
    mov dword ptr [edi], 0
  Sweep$loop:
    cmp esi, dword ptr _$$HeapHi
    jae Sweep$loopEnd
    mov ebx, esi
    sub ebx, dword ptr _$$HeapLo
    mov eax, dword ptr _$$ColorBase
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, ebx
    shr ecx, 6
    lea edx, dword ptr [eax + 4 * ecx + 0]
    mov ecx, ebx
    shr ecx, 1
    and ecx, 31
.data
align 4
.code
    mov edx, dword ptr [edx]
    mov ebx, edx
    shr ebx, cl
    and ebx, 3
    cmp ebx, 0
    je Sweep$skip1
.data
align 4
__$$$327$save3 DD 0
__$$$327$save2 DD 0
__$$$327$save1 DD 0
__$$$327$size DD 0
.code
    cmp ebx, 3
    jne $$327$skip1
    mov dword ptr __$$$327$save1, esi
    mov dword ptr __$$$327$save2, edi
    mov dword ptr __$$$327$save3, ebp
    mov ecx, esi
.data
align 4
.code
    mov edx, dword ptr [esi + 4]
.data
align 4
.code
    call _?GetSize
    mov dword ptr __$$$327$size, eax
    mov esi, dword ptr __$$$327$save1
    mov ebp, dword ptr __$$$327$save3
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 1
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    mov esi, dword ptr __$$$327$save1
    mov edi, dword ptr __$$$327$save2
    mov eax, esi
    sub eax, ebp
    cmp eax, 256
    jb $$327$skipFree
.data
align 4
.code
    mov dword ptr [edi], ebp
.data
align 4
.code
    mov dword ptr [ebp], 0
.data
align 4
.code
    mov dword ptr [ebp + 4], eax
    mov edi, ebp
  $$327$skipFree:
    add esi, dword ptr __$$$327$size
    mov ebp, esi
    jmp $$327$skip2
  $$327$skip1:
    mov dword ptr __$$$327$save1, esi
    mov dword ptr __$$$327$save2, edi
    mov dword ptr __$$$327$save3, ebp
    mov ecx, esi
.data
align 4
.code
    mov edx, dword ptr [esi + 4]
.data
align 4
.code
    call _?GetSize
    mov dword ptr __$$$327$size, eax
    mov esi, dword ptr __$$$327$save1
    mov edi, dword ptr __$$$327$save2
    mov ebp, dword ptr __$$$327$save3
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 0
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    mov esi, dword ptr __$$$327$save1
    mov edi, dword ptr __$$$327$save2
    add esi, dword ptr __$$$327$size
  $$327$skip2:
    jmp Sweep$loop
  Sweep$skip1:
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    add esi, 4
    jmp Sweep$loop
  Sweep$loopEnd:
    mov eax, esi
    sub eax, ebp
    cmp eax, 256
    jb Sweep$skip2
.data
align 4
.code
    mov dword ptr [edi], ebp
.data
align 4
.code
    mov dword ptr [ebp], 0
.data
align 4
.code
    mov dword ptr [ebp + 4], eax
    mov edi, ebp
  Sweep$skip2:
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    ret
_?Sweep endp
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
__$$$585$saveEbp DD 0
.code
    mov dword ptr __$$$585$saveEbp, ebp
.data
align 4
.code
    mov eax, dword ptr _$$GcLo
    mov dword ptr _$$StackTop, eax
.data
align 4
__$$$587$tag DD 0
__$$$587$t DD 0
__$$$587$s DD 0
.code
    mov dword ptr __$$$587$s, 0
    mov eax, dword ptr _$$TLo
    mov dword ptr __$$$587$t, eax
  $$587$__L1:
    mov eax, ?NumStacks
    cmp dword ptr __$$$587$s, eax
    jae $$587$__L2
    mov eax, dword ptr __$$$587$t
.data
align 4
.code
    mov eax, dword ptr [eax]
    mov dword ptr __$$$587$tag, eax
    mov eax, ?STACK_YIELDED
    cmp dword ptr __$$$587$tag, eax
    jne $$587$__L3
    mov ebx, dword ptr __$$$587$t
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
__$$$592$_nextFp DD 0
__$$$592$_ra DD 0
__$$$592$found DD 0
__$$$592$desc DD 0
__$$$592$addr DD 0
__$$$592$p DD 0
.code
    mov dword ptr __$$$592$_ra, ecx
    mov dword ptr __$$$592$_nextFp, edx
  $$592$loop:
    mov ecx, dword ptr __$$$592$_ra
    mov edx, dword ptr __$$$592$_nextFp
    cmp edx, 0
    je $$592$loopEnd
.data
align 4
.code
    call _?TablesSearch
    mov dword ptr __$$$592$desc, eax
    mov dword ptr __$$$592$found, edx
    mov ecx, dword ptr __$$$592$_nextFp
    mov dword ptr __$$$592$addr, ecx
.data
align 4
.code
    mov eax, dword ptr [ecx]
    mov dword ptr __$$$592$_nextFp, eax
.data
align 4
.code
    mov eax, dword ptr [ecx + 4]
    mov dword ptr __$$$592$_ra, eax
    mov eax, dword ptr __$$$592$desc
    shr eax, 0
    and eax, 1
    cmp eax, 1
    jne $$592$skip1
    mov eax, dword ptr __$$$592$desc
    shr eax, 1
    and eax, 1
    cmp eax, 1
    je $$592$skip1
    mov eax, dword ptr __$$$592$desc
    shr eax, 6
    and eax, 1023
    cmp eax, 0
    jne $$592$skip1
    mov ecx, dword ptr __$$$592$addr
    mov edx, dword ptr __$$$592$desc
.data
align 4
__$$$596$addrp DD 0
__$$$596$desc DD 0
__$$$596$addr DD 0
__$$$596$args DD 0
__$$$596$offset DD 0
__$$$596$v DD 0
__$$$596$b DD 0
.code
    mov dword ptr __$$$596$addr, ecx
    mov dword ptr __$$$596$desc, edx
    mov eax, edx
    shr eax, 2
    and eax, 15
    mov dword ptr __$$$596$args, eax
    mov dword ptr __$$$596$b, 0
    mov ebx, 0
    lea ebx, dword ptr [ebx + 4 * eax + 4]
    add ebx, ecx
    jc _?Overflow
    mov dword ptr __$$$596$addrp, ebx
  $$596$loop1:
    mov eax, dword ptr __$$$596$b
    cmp eax, dword ptr __$$$596$args
    jae $$596$loopEnd1
    lea ecx, dword ptr [eax + 16]
    mov ebx, dword ptr __$$$596$desc
    shr ebx, cl
    and ebx, 1
    cmp ebx, 1
    jne $$596$skip1
    mov ecx, dword ptr __$$$596$addrp
.data
align 4
__$$$597$offset DD 0
__$$$597$v DD 0
__$$$597$save1 DD 0
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp eax, dword ptr _$$GcLo
    jb $$597$skip1
    cmp eax, dword ptr _$$GcHi
    ja $$597$skip1
    mov dword ptr __$$$597$save1, ecx
    mov dword ptr __$$$597$v, eax
    mov ecx, eax
.data
align 4
.code
    call _?FromInteriorPointer
    mov dword ptr __$$$597$offset, eax
    mov ecx, dword ptr __$$$597$v
    sub ecx, eax
.data
align 4
.code
.data
align 4
.code
    mov ebp, ecx
    sub ebp, 4
    mov ebx, ebp
    sub ebx, dword ptr _$$HeapLo
    mov eax, dword ptr _$$ColorBase
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, ebx
    shr ecx, 6
    lea edx, dword ptr [eax + 4 * ecx + 0]
    mov ecx, ebx
    shr ecx, 1
    and ecx, 31
.data
align 4
.code
    mov edx, dword ptr [edx]
    mov ebx, edx
    shr ebx, cl
    and ebx, 3
    cmp ebx, 1
    jne $$601$end
    mov eax, dword ptr _$$StackTop
    cmp eax, dword ptr _$$ColorBase
    jne $$601$skip1
    mov eax, 1431633985
.data
align 4
.code
    call _?DebugBreak
  $$601$skip1:
    mov esi, ebp
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 2
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    mov eax, dword ptr _$$StackTop
.data
align 4
.code
    mov dword ptr [eax], ebp
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    add dword ptr _$$StackTop, 4
  $$601$end:
.data
align 4
.code
.data
align 4
.code
    mov ecx, dword ptr __$$$597$save1
  $$597$skip1:
  $$596$skip1:
    add dword ptr __$$$596$b, 1
    sub dword ptr __$$$596$addrp, 4
    jmp $$596$loop1
  $$596$loopEnd1:
    sub dword ptr __$$$596$addrp, 8
    jc _?Overflow
  $$596$loop2:
    mov eax, dword ptr __$$$596$b
    cmp eax, 16
    jae $$596$loopEnd2
    lea ecx, dword ptr [eax + 16]
    mov ebx, dword ptr __$$$596$desc
    shr ebx, cl
    and ebx, 1
    cmp ebx, 1
    jne $$596$skip2
    mov ecx, dword ptr __$$$596$addrp
.data
align 4
__$$$787$offset DD 0
__$$$787$v DD 0
__$$$787$save1 DD 0
.code
.data
align 4
.code
    mov eax, dword ptr [ecx]
    cmp eax, dword ptr _$$GcLo
    jb $$787$skip1
    cmp eax, dword ptr _$$GcHi
    ja $$787$skip1
    mov dword ptr __$$$787$save1, ecx
    mov dword ptr __$$$787$v, eax
    mov ecx, eax
.data
align 4
.code
    call _?FromInteriorPointer
    mov dword ptr __$$$787$offset, eax
    mov ecx, dword ptr __$$$787$v
    sub ecx, eax
.data
align 4
.code
.data
align 4
.code
    mov ebp, ecx
    sub ebp, 4
    mov ebx, ebp
    sub ebx, dword ptr _$$HeapLo
    mov eax, dword ptr _$$ColorBase
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, ebx
    shr ecx, 6
    lea edx, dword ptr [eax + 4 * ecx + 0]
    mov ecx, ebx
    shr ecx, 1
    and ecx, 31
.data
align 4
.code
    mov edx, dword ptr [edx]
    mov ebx, edx
    shr ebx, cl
    and ebx, 3
    cmp ebx, 1
    jne $$791$end
    mov eax, dword ptr _$$StackTop
    cmp eax, dword ptr _$$ColorBase
    jne $$791$skip1
    mov eax, 1431633985
.data
align 4
.code
    call _?DebugBreak
  $$791$skip1:
    mov esi, ebp
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 2
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    mov eax, dword ptr _$$StackTop
.data
align 4
.code
    mov dword ptr [eax], ebp
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    add dword ptr _$$StackTop, 4
  $$791$end:
.data
align 4
.code
.data
align 4
.code
    mov ecx, dword ptr __$$$787$save1
  $$787$skip1:
  $$596$skip2:
    add dword ptr __$$$596$b, 1
    sub dword ptr __$$$596$addrp, 4
    jc _?Overflow
    jmp $$596$loop2
  $$596$loopEnd2:
    jmp $$592$loop
  $$592$skip1:
    mov eax, dword ptr __$$$592$desc
    shr eax, 0
    and eax, 1
    cmp eax, 1
    je $$592$skip2
    mov eax, dword ptr __$$$592$desc
    mov eax, dword ptr [eax]
    cmp eax, 4096
    jne $$592$skip2
    mov ecx, dword ptr __$$$592$addr
    mov edx, dword ptr __$$$592$desc
.data
align 4
__$$$977$count DD 0
__$$$977$offset DD 0
__$$$977$addrp DD 0
__$$$977$desc DD 0
__$$$977$addr DD 0
__$$$977$v DD 0
__$$$977$p DD 0
.code
    mov dword ptr __$$$977$addr, ecx
    mov dword ptr __$$$977$desc, edx
    mov dword ptr __$$$977$p, 0
    movzx eax, byte ptr [edx + 4]
    mov dword ptr __$$$977$count, eax
  $$977$loop:
    mov eax, dword ptr __$$$977$p
    cmp eax, dword ptr __$$$977$count
    jae $$977$loopEnd
    mov ebx, dword ptr __$$$977$desc
    mov esi, dword ptr __$$$977$addr
    movsx ebp, byte ptr [ebx + 1 * eax + 6]
    lea ebp, dword ptr [esi + 4 * ebp + 0]
    mov dword ptr __$$$977$addrp, ebp
.data
align 4
.code
    mov ecx, dword ptr [ebp]
    cmp ecx, dword ptr _$$GcLo
    jb $$977$skip1
    cmp ecx, dword ptr _$$GcHi
    ja $$977$skip1
    mov dword ptr __$$$977$v, ecx
.data
align 4
.code
    call _?FromInteriorPointer
    mov dword ptr __$$$977$offset, eax
    mov ecx, dword ptr __$$$977$v
    sub ecx, eax
.data
align 4
.code
.data
align 4
.code
    mov ebp, ecx
    sub ebp, 4
    mov ebx, ebp
    sub ebx, dword ptr _$$HeapLo
    mov eax, dword ptr _$$ColorBase
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, ebx
    shr ecx, 6
    lea edx, dword ptr [eax + 4 * ecx + 0]
    mov ecx, ebx
    shr ecx, 1
    and ecx, 31
.data
align 4
.code
    mov edx, dword ptr [edx]
    mov ebx, edx
    shr ebx, cl
    and ebx, 3
    cmp ebx, 1
    jne $$981$end
    mov eax, dword ptr _$$StackTop
    cmp eax, dword ptr _$$ColorBase
    jne $$981$skip1
    mov eax, 1431633985
.data
align 4
.code
    call _?DebugBreak
  $$981$skip1:
    mov esi, ebp
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 2
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    mov eax, dword ptr _$$StackTop
.data
align 4
.code
    mov dword ptr [eax], ebp
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    add dword ptr _$$StackTop, 4
  $$981$end:
.data
align 4
.code
.data
align 4
.code
    mov ebx, dword ptr __$$$977$addrp
  $$977$skip1:
    add dword ptr __$$$977$p, 1
    jmp $$977$loop
  $$977$loopEnd:
    jmp $$592$loop
  $$592$skip2:
    mov eax, 1431633988
.data
align 4
.code
    call _?DebugBreak
  $$592$loopEnd:
  $$587$__L3:
    mov eax, ?STACK_INTERRUPTED
    cmp dword ptr __$$$587$tag, eax
    jne $$587$__L4
    mov eax, 1431633989
.data
align 4
.code
    call _?DebugBreak
  $$587$__L4:
    mov eax, ?STACK_RUNNING
    cmp dword ptr __$$$587$tag, eax
    jne $$587$__L5
    mov eax, 1431633990
.data
align 4
.code
    call _?DebugBreak
  $$587$__L5:
    add dword ptr __$$$587$s, 1
    mov eax, ?TSize
    add dword ptr __$$$587$t, eax
    jmp $$587$__L1
  $$587$__L2:
.data
align 4
__$$$1170$section DD 0
.code
    mov dword ptr __$$$1170$section, 0
  $$1170$loop:
    mov ecx, dword ptr __$$$1170$section
    cmp ecx, dword ptr _$$?sectionCount
    jae $$1170$loopEnd
.data
align 4
__$$$1171$save3 DD 0
__$$$1171$save2 DD 0
__$$$1171$save1 DD 0
__$$$1171$section DD 0
__$$$1171$addr DD 0
__$$$1171$sEnd DD 0
.code
    mov dword ptr __$$$1171$section, ecx
    mov eax, dword ptr _$$?dataSectionEnd
    mov eax, dword ptr [eax + 4 * ecx + 0]
    mov dword ptr __$$$1171$sEnd, eax
    mov eax, dword ptr _$$?staticDataPointerBitMap
    mov edx, dword ptr [eax + 4 * ecx + 0]
    mov eax, dword ptr _$$?dataSectionBase
    mov eax, dword ptr [eax + 4 * ecx + 0]
    mov dword ptr __$$$1171$addr, eax
    mov edi, eax
    mov esi, 0
  $$1171$loop:
    cmp edi, dword ptr __$$$1171$sEnd
    jae $$1171$loopEnd
    mov eax, esi
    shr eax, 5
    mov eax, dword ptr [edx + 4 * eax + 0]
    mov ecx, esi
    and ecx, 31
    shr eax, cl
    and eax, 1
    cmp eax, 1
    jne $$1171$skip1
    mov ecx, dword ptr [edi]
    cmp ecx, dword ptr _$$GcLo
    jb $$1171$skip2
    cmp ecx, dword ptr _$$GcHi
    ja $$1171$skip2
    mov dword ptr __$$$1171$save1, edi
    mov dword ptr __$$$1171$save2, esi
    mov dword ptr __$$$1171$save3, edx
.data
align 4
.code
.data
align 4
.code
    mov ebp, ecx
    sub ebp, 4
    mov ebx, ebp
    sub ebx, dword ptr _$$HeapLo
    mov eax, dword ptr _$$ColorBase
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, ebx
    shr ecx, 6
    lea edx, dword ptr [eax + 4 * ecx + 0]
    mov ecx, ebx
    shr ecx, 1
    and ecx, 31
.data
align 4
.code
    mov edx, dword ptr [edx]
    mov ebx, edx
    shr ebx, cl
    and ebx, 3
    cmp ebx, 1
    jne $$1173$end
    mov eax, dword ptr _$$StackTop
    cmp eax, dword ptr _$$ColorBase
    jne $$1173$skip1
    mov eax, 1431633985
.data
align 4
.code
    call _?DebugBreak
  $$1173$skip1:
    mov esi, ebp
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 2
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    mov eax, dword ptr _$$StackTop
.data
align 4
.code
    mov dword ptr [eax], ebp
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    add dword ptr _$$StackTop, 4
  $$1173$end:
    mov edi, dword ptr __$$$1171$save1
    mov esi, dword ptr __$$$1171$save2
    mov edx, dword ptr __$$$1171$save3
  $$1171$skip2:
  $$1171$skip1:
    add esi, 1
    add edi, 4
    jc _?Overflow
    jmp $$1171$loop
  $$1171$loopEnd:
    add dword ptr __$$$1170$section, 1
    jc _?Overflow
    jmp $$1170$loop
  $$1170$loopEnd:
.data
align 4
__$$$1357$ptr DD 0
.code
    mov eax, dword ptr _$$StackTop
  $$1357$loop:
    mov eax, dword ptr _$$StackTop
    cmp eax, dword ptr _$$GcLo
    je $$1357$loopEnd
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    sub eax, 4
.data
align 4
.code
    mov ebx, dword ptr [eax]
    mov dword ptr __$$$1357$ptr, ebx
    mov dword ptr _$$StackTop, eax
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, dword ptr [ebx + 4]
.data
align 4
.code
    mov eax, dword ptr [ecx + ?VT_MASK]
    and eax, 15
    cmp eax, ?SPARSE_TAG
    jne $$1479$skip1
.data
align 4
__$$$1482$ptr DD 0
__$$$1482$save4 DD 0
__$$$1482$save3 DD 0
__$$$1482$save2 DD 0
__$$$1482$save1 DD 0
.code
    mov dword ptr __$$$1482$ptr, ebx
    mov esi, 1
    mov ebp, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov edx, dword ptr [ecx + ?VT_MASK]
    mov esi, 1
  $$1482$loop:
    cmp esi, 8
    jae $$1482$loopEnd
    mov ecx, 0
    lea ecx, dword ptr [ecx + 4 * esi + 0]
    mov ebx, edx
    shr ebx, cl
    and ebx, 15
    cmp ebx, 0
    je $$1482$skip1
    mov eax, dword ptr __$$$1482$ptr
.data
align 4
.code
    mov ecx, dword ptr [eax + 4 * ebx + 4]
    cmp ecx, dword ptr _$$GcLo
    jb $$1482$skip2
    cmp ecx, dword ptr _$$GcHi
    ja $$1482$skip2
.data
align 4
.code
    mov dword ptr __$$$1482$save1, esi
    mov dword ptr __$$$1482$save2, ebp
    mov dword ptr __$$$1482$save3, edx
    mov dword ptr __$$$1482$save4, ebx
.data
align 4
.code
    mov ebp, ecx
    sub ebp, 4
    mov ebx, ebp
    sub ebx, dword ptr _$$HeapLo
    mov eax, dword ptr _$$ColorBase
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, ebx
    shr ecx, 6
    lea edx, dword ptr [eax + 4 * ecx + 0]
    mov ecx, ebx
    shr ecx, 1
    and ecx, 31
.data
align 4
.code
    mov edx, dword ptr [edx]
    mov ebx, edx
    shr ebx, cl
    and ebx, 3
    cmp ebx, 1
    jne $$1485$end
    mov eax, dword ptr _$$StackTop
    cmp eax, dword ptr _$$ColorBase
    jne $$1485$skip1
    mov eax, 1431633985
.data
align 4
.code
    call _?DebugBreak
  $$1485$skip1:
    mov esi, ebp
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 2
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    mov eax, dword ptr _$$StackTop
.data
align 4
.code
    mov dword ptr [eax], ebp
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    add dword ptr _$$StackTop, 4
  $$1485$end:
    mov esi, dword ptr __$$$1482$save1
    mov ebp, dword ptr __$$$1482$save2
    mov edx, dword ptr __$$$1482$save3
    mov ebx, dword ptr __$$$1482$save4
  $$1482$skip2:
  $$1482$skip1:
    add esi, 1
    jmp $$1482$loop
  $$1482$loopEnd:
    jmp $$1479$end
  $$1479$skip1:
    cmp eax, ?DENSE_TAG
    jne $$1479$skip2
.data
align 4
__$$$1669$save3 DD 0
__$$$1669$save2 DD 0
__$$$1669$save1 DD 0
__$$$1669$ptr DD 0
.code
    mov dword ptr __$$$1669$ptr, ebx
    mov esi, 2
    mov ebp, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov edx, dword ptr [ecx + ?VT_MASK]
    mov edi, ebx
    add edi, 8
    add ebp, ebx
  $$1669$loop:
    cmp edi, ebp
    jae $$1669$loopEnd
    cmp esi, 30
    jae $$1669$loopEnd
    lea ecx, dword ptr [esi + 2]
    mov ebx, edx
    shr ebx, cl
    and ebx, 1
    cmp ebx, 1
    jne $$1669$skip1
.data
align 4
.code
    mov ecx, dword ptr [edi]
    cmp ecx, dword ptr _$$GcLo
    jb $$1669$skip2
    cmp ecx, dword ptr _$$GcHi
    ja $$1669$skip2
.data
align 4
.code
    mov dword ptr __$$$1669$save1, esi
    mov dword ptr __$$$1669$save2, ebp
    mov dword ptr __$$$1669$save3, edx
.data
align 4
.code
    mov ebp, ecx
    sub ebp, 4
    mov ebx, ebp
    sub ebx, dword ptr _$$HeapLo
    mov eax, dword ptr _$$ColorBase
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, ebx
    shr ecx, 6
    lea edx, dword ptr [eax + 4 * ecx + 0]
    mov ecx, ebx
    shr ecx, 1
    and ecx, 31
.data
align 4
.code
    mov edx, dword ptr [edx]
    mov ebx, edx
    shr ebx, cl
    and ebx, 3
    cmp ebx, 1
    jne $$1672$end
    mov eax, dword ptr _$$StackTop
    cmp eax, dword ptr _$$ColorBase
    jne $$1672$skip1
    mov eax, 1431633985
.data
align 4
.code
    call _?DebugBreak
  $$1672$skip1:
    mov esi, ebp
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 2
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    mov eax, dword ptr _$$StackTop
.data
align 4
.code
    mov dword ptr [eax], ebp
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    add dword ptr _$$StackTop, 4
  $$1672$end:
    mov esi, dword ptr __$$$1669$save1
    mov ebp, dword ptr __$$$1669$save2
    mov edx, dword ptr __$$$1669$save3
    mov edi, dword ptr __$$$1669$ptr
    lea edi, dword ptr [edi + 4 * esi + 0]
  $$1669$skip2:
  $$1669$skip1:
    add esi, 1
    add edi, 4
    jmp $$1669$loop
  $$1669$loopEnd:
    jmp $$1479$end
  $$1479$skip2:
    cmp eax, ?STRING_TAG
    jne $$1479$skip3
.data
align 4
.code
    jmp $$1479$end
  $$1479$skip3:
    cmp eax, ?PTR_VECTOR_TAG
    jne $$1479$skip4
.data
align 4
__$$$1857$save2 DD 0
__$$$1857$save1 DD 0
.code
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
  $$1857$loop:
    cmp edi, ebp
    jae $$1857$loopEnd
.data
align 4
.code
    mov ecx, dword ptr [edi]
    cmp ecx, dword ptr _$$GcLo
    jb $$1857$skip1
    cmp ecx, dword ptr _$$GcHi
    ja $$1857$skip1
.data
align 4
.code
    mov dword ptr __$$$1857$save1, edi
    mov dword ptr __$$$1857$save2, ebp
.data
align 4
.code
    mov ebp, ecx
    sub ebp, 4
    mov ebx, ebp
    sub ebx, dword ptr _$$HeapLo
    mov eax, dword ptr _$$ColorBase
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, ebx
    shr ecx, 6
    lea edx, dword ptr [eax + 4 * ecx + 0]
    mov ecx, ebx
    shr ecx, 1
    and ecx, 31
.data
align 4
.code
    mov edx, dword ptr [edx]
    mov ebx, edx
    shr ebx, cl
    and ebx, 3
    cmp ebx, 1
    jne $$1861$end
    mov eax, dword ptr _$$StackTop
    cmp eax, dword ptr _$$ColorBase
    jne $$1861$skip1
    mov eax, 1431633985
.data
align 4
.code
    call _?DebugBreak
  $$1861$skip1:
    mov esi, ebp
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 2
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    mov eax, dword ptr _$$StackTop
.data
align 4
.code
    mov dword ptr [eax], ebp
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    add dword ptr _$$StackTop, 4
  $$1861$end:
    mov edi, dword ptr __$$$1857$save1
    mov ebp, dword ptr __$$$1857$save2
  $$1857$skip1:
    add edi, 4
    jmp $$1857$loop
  $$1857$loopEnd:
    jmp $$1479$end
  $$1479$skip4:
    cmp eax, ?OTHER_VECTOR_TAG
    jne $$1479$skip5
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
    jne $$1479$noPoint
    cmp ebp, ?TYPE_STRUCT
    jne $$1479$vecSkip1
    cmp edi, ?SPARSE_TAG
    jne $$1479$vecSkip1
  $$1479$noPoint:
.data
align 4
.code
    jmp $$1479$end
  $$1479$vecSkip1:
    cmp ebp, ?TYPE_STRUCT
    jne $$1479$vecSkip2
    mov eax, edi
    and eax, 15
    cmp eax, ?SPARSE_TAG
    jne $$1479$vecSkip2
.data
align 4
__$$$2048$ptr DD 0
__$$$2048$save2 DD 0
__$$$2048$save1 DD 0
.code
    mov dword ptr __$$$2048$ptr, ebx
    mov edx, ebx
    mov edi, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov ebx, dword ptr [ecx + ?VT_ARRAY_ELEMENT_SIZE]
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
  $$2048$loop:
    mov eax, 0
    lea eax, dword ptr [eax + 4 * edi + 0]
    cmp eax, ebp
    jae $$2048$loopEnd
    mov dword ptr __$$$2048$save1, ebx
    mov dword ptr __$$$2048$save2, ebp
    mov ebp, dword ptr __$$$2048$ptr
.data
align 4
__$$$2050$save5 DD 0
__$$$2050$save4 DD 0
__$$$2050$save3 DD 0
__$$$2050$save2 DD 0
__$$$2050$save1 DD 0
.code
    mov esi, 1
  $$2050$loop:
    cmp esi, 8
    jae $$2050$loopEnd
    mov ecx, 0
    lea ecx, dword ptr [ecx + 4 * esi + 0]
    mov ebx, edx
    shr ebx, cl
    and ebx, 15
    cmp ebx, 0
    je $$2050$skip1
    sub ebx, 1
    add ebx, edi
    mov eax, ebp
.data
align 4
.code
    mov ecx, dword ptr [eax + 4 * ebx + 0]
    cmp ecx, dword ptr _$$GcLo
    jb $$2050$skip2
    cmp ecx, dword ptr _$$GcHi
    ja $$2050$skip2
.data
align 4
.code
    mov dword ptr __$$$2050$save1, esi
    mov dword ptr __$$$2050$save2, edi
    mov dword ptr __$$$2050$save3, edx
    mov dword ptr __$$$2050$save4, ebx
    mov dword ptr __$$$2050$save5, ebp
.data
align 4
.code
    mov ebp, ecx
    sub ebp, 4
    mov ebx, ebp
    sub ebx, dword ptr _$$HeapLo
    mov eax, dword ptr _$$ColorBase
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, ebx
    shr ecx, 6
    lea edx, dword ptr [eax + 4 * ecx + 0]
    mov ecx, ebx
    shr ecx, 1
    and ecx, 31
.data
align 4
.code
    mov edx, dword ptr [edx]
    mov ebx, edx
    shr ebx, cl
    and ebx, 3
    cmp ebx, 1
    jne $$2053$end
    mov eax, dword ptr _$$StackTop
    cmp eax, dword ptr _$$ColorBase
    jne $$2053$skip1
    mov eax, 1431633985
.data
align 4
.code
    call _?DebugBreak
  $$2053$skip1:
    mov esi, ebp
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 2
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    mov eax, dword ptr _$$StackTop
.data
align 4
.code
    mov dword ptr [eax], ebp
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    add dword ptr _$$StackTop, 4
  $$2053$end:
    mov esi, dword ptr __$$$2050$save1
    mov edi, dword ptr __$$$2050$save2
    mov edx, dword ptr __$$$2050$save3
    mov ebx, dword ptr __$$$2050$save4
    mov ebp, dword ptr __$$$2050$save5
  $$2050$skip2:
  $$2050$skip1:
    add esi, 1
    jmp $$2050$loop
  $$2050$loopEnd:
    mov ebx, dword ptr __$$$2048$save1
    mov ebp, dword ptr __$$$2048$save2
    add edi, ebx
    jmp $$2048$loop
  $$2048$loopEnd:
    jmp $$1479$end
  $$1479$vecSkip2:
    mov eax, 1431633986
.data
align 4
.code
    call _?DebugBreak
  $$1479$skip5:
    cmp eax, ?PTR_ARRAY_TAG
    jne $$1479$skip6
.data
align 4
__$$$2238$save2 DD 0
__$$$2238$save1 DD 0
.code
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
  $$2238$loop:
    cmp edi, ebp
    jae $$2238$loopEnd
.data
align 4
.code
    mov ecx, dword ptr [edi]
    cmp ecx, dword ptr _$$GcLo
    jb $$2238$skip1
    cmp ecx, dword ptr _$$GcHi
    ja $$2238$skip1
.data
align 4
.code
    mov dword ptr __$$$2238$save1, edi
    mov dword ptr __$$$2238$save2, ebp
.data
align 4
.code
    mov ebp, ecx
    sub ebp, 4
    mov ebx, ebp
    sub ebx, dword ptr _$$HeapLo
    mov eax, dword ptr _$$ColorBase
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, ebx
    shr ecx, 6
    lea edx, dword ptr [eax + 4 * ecx + 0]
    mov ecx, ebx
    shr ecx, 1
    and ecx, 31
.data
align 4
.code
    mov edx, dword ptr [edx]
    mov ebx, edx
    shr ebx, cl
    and ebx, 3
    cmp ebx, 1
    jne $$2242$end
    mov eax, dword ptr _$$StackTop
    cmp eax, dword ptr _$$ColorBase
    jne $$2242$skip1
    mov eax, 1431633985
.data
align 4
.code
    call _?DebugBreak
  $$2242$skip1:
    mov esi, ebp
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 2
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    mov eax, dword ptr _$$StackTop
.data
align 4
.code
    mov dword ptr [eax], ebp
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    add dword ptr _$$StackTop, 4
  $$2242$end:
    mov edi, dword ptr __$$$2238$save1
    mov ebp, dword ptr __$$$2238$save2
  $$2238$skip1:
    add edi, 4
    jmp $$2238$loop
  $$2238$loopEnd:
    jmp $$1479$end
  $$1479$skip6:
    cmp eax, ?OTHER_ARRAY_TAG
    jne $$1479$skip7
.data
align 4
.code
    mov ebp, dword ptr [ecx + ?VT_ARRAY_OF]
    cmp ebp, ?TYPE_STRUCT
    je $$1479$arraySkip1
.data
align 4
.code
    jmp $$1479$end
  $$1479$arraySkip1:
    mov eax, 1431633987
.data
align 4
.code
    call _?DebugBreak
    jmp $$1479$end
  $$1479$skip7:
.data
align 4
__$$$2429$ptr DD 0
__$$$2429$save5 DD 0
__$$$2429$save4 DD 0
__$$$2429$save3 DD 0
__$$$2429$save2 DD 0
__$$$2429$save1 DD 0
.code
    mov dword ptr __$$$2429$ptr, ebx
    mov edx, dword ptr [ecx + ?VT_MASK]
    mov edi, dword ptr [edx]
    mov ebp, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov esi, 1
  $$2429$loop:
    cmp esi, edi
    ja $$2429$loopEnd
    mov ebx, dword ptr [edx + 4 * esi + 0]
    cmp ebx, 0
    je $$2429$skip1
    mov eax, dword ptr __$$$2429$ptr
.data
align 4
.code
    mov ecx, dword ptr [eax + 4 * ebx + 4]
    cmp ecx, dword ptr _$$GcLo
    jb $$2429$skip2
    cmp ecx, dword ptr _$$GcHi
    ja $$2429$skip2
.data
align 4
.code
    mov dword ptr __$$$2429$save1, ebx
    mov dword ptr __$$$2429$save2, esi
    mov dword ptr __$$$2429$save3, edi
    mov dword ptr __$$$2429$save4, ebp
    mov dword ptr __$$$2429$save5, edx
.data
align 4
.code
    mov ebp, ecx
    sub ebp, 4
    mov ebx, ebp
    sub ebx, dword ptr _$$HeapLo
    mov eax, dword ptr _$$ColorBase
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    mov ecx, ebx
    shr ecx, 6
    lea edx, dword ptr [eax + 4 * ecx + 0]
    mov ecx, ebx
    shr ecx, 1
    and ecx, 31
.data
align 4
.code
    mov edx, dword ptr [edx]
    mov ebx, edx
    shr ebx, cl
    and ebx, 3
    cmp ebx, 1
    jne $$2432$end
    mov eax, dword ptr _$$StackTop
    cmp eax, dword ptr _$$ColorBase
    jne $$2432$skip1
    mov eax, 1431633985
.data
align 4
.code
    call _?DebugBreak
  $$2432$skip1:
    mov esi, ebp
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 2
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    mov eax, dword ptr _$$StackTop
.data
align 4
.code
    mov dword ptr [eax], ebp
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
    add dword ptr _$$StackTop, 4
  $$2432$end:
    mov ebx, dword ptr __$$$2429$save1
    mov esi, dword ptr __$$$2429$save2
    mov edi, dword ptr __$$$2429$save3
    mov ebp, dword ptr __$$$2429$save4
    mov edx, dword ptr __$$$2429$save5
  $$2429$skip2:
  $$2429$skip1:
    add esi, 1
    jc _?Overflow
    jmp $$2429$loop
  $$2429$loopEnd:
  $$1479$end:
    mov esi, dword ptr __$$$1357$ptr
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 3
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    jmp $$1357$loop
  $$1357$loopEnd:
    mov eax, dword ptr _$$HeapLo
.data
align 4
.code
    mov dword ptr [eax], 0
    mov dword ptr _$$CacheSize, 0
.data
align 4
.code
    call _?Sweep
    mov ebp, dword ptr __$$$585$saveEbp
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
    mov ebx, 72
    div ebx
    mov ebp, eax
    mov dword ptr __$InitializeGc$unitSize, ebp
    cmp ebp, 0
    jne InitializeGc$skip1
    mov eax, 1431633991
.data
align 4
.code
    call _?DebugBreak
  InitializeGc$skip1:
    mov edx, 0
    lea ebp, dword ptr [edx + 4 * ebp + 0]
    mov eax, dword ptr _$$GcLo
    add eax, ebp
    mov dword ptr _$$ColorBase, eax
    lea ebx, dword ptr [eax + 1 * ebp + 0]
    mov dword ptr _$$HeapLo, ebx
    lea ebp, dword ptr [edx + 4 * ebp + 0]
    lea ecx, dword ptr [ebx + 4 * ebp + 0]
    mov dword ptr _$$HeapHi, ecx
    mov eax, dword ptr _$$ColorBase
    mov ebx, dword ptr _$$HeapLo
.data
align 4
.code
    call _?BB4Zero2
    mov eax, dword ptr _$$HeapLo
    lea ebx, dword ptr [eax + 8]
.data
align 4
.code
    mov dword ptr [eax], ebx
.data
align 4
.code
    mov dword ptr [eax + 4], 8
.data
align 4
.code
    mov dword ptr [eax + 8], 0
    mov ecx, dword ptr _$$HeapHi
    sub ecx, ebx
.data
align 4
.code
    mov dword ptr [eax + 12], ecx
    mov dword ptr _$$CacheSize, 0
    mov eax, dword ptr _$$HeapLo
    mov dword ptr _$$ReserveMin, eax
    mov ebp, dword ptr __$InitializeGc$save
.data
align 4
.code
    ret
_?InitializeGc endp
.data
align 4
__$AllocObject$size DD 0
__$AllocObject$fp DD 0
__$AllocObject$sp DD 0
__$AllocObject$vt DD 0
.code
align 16
_?AllocObject proc
    mov dword ptr __$AllocObject$sp, esp
    mov dword ptr __$AllocObject$fp, ebp
    mov dword ptr __$AllocObject$vt, ecx
    mov eax, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov dword ptr __$AllocObject$size, eax
.data
align 4
.code
    mov ecx, dword ptr [esp]
    mov edx, ebp
    mov esp, dword ptr _$$DLo
.data
align 4
__$$$2689$size DD 0
__$$$2689$nextFp DD 0
__$$$2689$ra DD 0
.code
    mov dword ptr __$$$2689$ra, ecx
    mov ecx, eax
    add eax, 8
    jc _?Overflow
    cmp eax, dword ptr _$$CacheSize
    ja $$2689$skip1
.data
align 4
.code
    sub dword ptr _$$CacheSize, ecx
    mov eax, dword ptr _$$CacheSize
    mov edx, dword ptr _$$CachePtr
.data
align 4
.code
    mov dword ptr [edx + 4], eax
    add eax, edx
.data
align 4
.code
    mov dword ptr [eax], 0
.data
align 4
.code
    mov dword ptr [eax + 4], ecx
    jmp $$2689$end
  $$2689$skip1:
    mov dword ptr __$$$2689$nextFp, edx
    mov dword ptr __$$$2689$size, ecx
.data
align 4
.code
    call _?AllocSlow
  $$2689$end:
    cmp eax, 0
    je AllocObject$done
    mov edx, dword ptr __$AllocObject$size
    lea ebx, dword ptr [eax + 1 * edx + 0]
    mov ecx, dword ptr __$AllocObject$vt
.data
align 4
.code
.data
align 4
.code
    mov esi, dword ptr _$$GcLo
    mov esi, eax
    cmp esi, ebx
    jae $$2696$loopEnd
  $$2696$loop:
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [esi], 0
    add esi, 4
    cmp esi, ebx
    jb $$2696$loop
  $$2696$loopEnd:
.data
align 4
.code
    mov dword ptr [eax + 4], ecx
    mov ebx, eax
    mov esi, eax
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 1
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    lea eax, dword ptr [ebx + 4]
  AllocObject$done:
    mov ebp, dword ptr __$AllocObject$fp
    mov esp, dword ptr __$AllocObject$sp
.data
align 4
.code
    ret
_?AllocObject endp
.data
align 4
__$AllocString$nElems DD 0
__$AllocString$size DD 0
__$AllocString$fp DD 0
__$AllocString$sp DD 0
.code
align 16
_?AllocString proc
    mov dword ptr __$AllocString$sp, esp
    mov dword ptr __$AllocString$fp, ebp
    add ecx, 1
    jc _?Overflow
    mov edx, ecx
    mov dword ptr __$AllocString$nElems, edx
    add ecx, ecx
    jc _?Overflow
    add ecx, 19
    jc _?Overflow
    mov eax, 3
    not eax
    and ecx, eax
    mov dword ptr __$AllocString$size, ecx
    mov eax, ecx
.data
align 4
.code
    mov ecx, dword ptr [esp]
    mov edx, ebp
    mov esp, dword ptr _$$DLo
.data
align 4
__$$$2763$size DD 0
__$$$2763$nextFp DD 0
__$$$2763$ra DD 0
.code
    mov dword ptr __$$$2763$ra, ecx
    mov ecx, eax
    add eax, 8
    jc _?Overflow
    cmp eax, dword ptr _$$CacheSize
    ja $$2763$skip1
.data
align 4
.code
    sub dword ptr _$$CacheSize, ecx
    mov eax, dword ptr _$$CacheSize
    mov edx, dword ptr _$$CachePtr
.data
align 4
.code
    mov dword ptr [edx + 4], eax
    add eax, edx
.data
align 4
.code
    mov dword ptr [eax], 0
.data
align 4
.code
    mov dword ptr [eax + 4], ecx
    jmp $$2763$end
  $$2763$skip1:
    mov dword ptr __$$$2763$nextFp, edx
    mov dword ptr __$$$2763$size, ecx
.data
align 4
.code
    call _?AllocSlow
  $$2763$end:
    cmp eax, 0
    je AllocString$done
    mov edx, dword ptr __$AllocString$nElems
    mov ecx, dword ptr __$AllocString$size
    lea ebx, dword ptr [eax + 1 * ecx + 0]
.data
align 4
.code
.data
align 4
.code
    mov esi, dword ptr _$$GcLo
    mov esi, eax
    cmp esi, ebx
    jae $$2770$loopEnd
  $$2770$loop:
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [esi], 0
    add esi, 4
    cmp esi, ebx
    jb $$2770$loop
  $$2770$loopEnd:
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
    mov ebx, eax
    mov esi, eax
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 1
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    lea eax, dword ptr [ebx + 4]
  AllocString$done:
    mov ebp, dword ptr __$AllocString$fp
    mov esp, dword ptr __$AllocString$sp
.data
align 4
.code
    ret
_?AllocString endp
.data
align 4
__$AllocArray$fp DD 0
__$AllocArray$sp DD 0
__$AllocArray$nElems DD 0
__$AllocArray$size DD 0
__$AllocArray$rank DD 0
__$AllocArray$vt DD 0
.code
align 16
_?AllocArray proc
    mov dword ptr __$AllocArray$rank, edx
    mov dword ptr __$AllocArray$sp, esp
    mov dword ptr __$AllocArray$fp, ebp
    mov dword ptr __$AllocArray$vt, ecx
.data
align 4
.code
    mov esi, dword ptr [esp + 4]
    mov dword ptr __$AllocArray$nElems, esi
.data
align 4
.code
    mov eax, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov ebx, dword ptr [ecx + ?VT_ARRAY_ELEMENT_SIZE]
    mov edi, dword ptr [ecx + ?VT_MASK]
    and edi, 15
    cmp edi, ?PTR_ARRAY_TAG
    jne $$2839$skip1
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
    jmp $$2839$skip2
  $$2839$skip1:
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
  $$2839$skip2:
    mov dword ptr __$AllocArray$size, eax
.data
align 4
.code
    mov ecx, dword ptr [esp]
    mov edx, ebp
    mov esp, dword ptr _$$DLo
.data
align 4
__$$$2841$size DD 0
__$$$2841$nextFp DD 0
__$$$2841$ra DD 0
.code
    mov dword ptr __$$$2841$ra, ecx
    mov ecx, eax
    add eax, 8
    jc _?Overflow
    cmp eax, dword ptr _$$CacheSize
    ja $$2841$skip1
.data
align 4
.code
    sub dword ptr _$$CacheSize, ecx
    mov eax, dword ptr _$$CacheSize
    mov edx, dword ptr _$$CachePtr
.data
align 4
.code
    mov dword ptr [edx + 4], eax
    add eax, edx
.data
align 4
.code
    mov dword ptr [eax], 0
.data
align 4
.code
    mov dword ptr [eax + 4], ecx
    jmp $$2841$end
  $$2841$skip1:
    mov dword ptr __$$$2841$nextFp, edx
    mov dword ptr __$$$2841$size, ecx
.data
align 4
.code
    call _?AllocSlow
  $$2841$end:
    cmp eax, 0
    je AllocArray$done
    mov edx, dword ptr __$AllocArray$size
    lea ebx, dword ptr [eax + 1 * edx + 0]
    mov edx, dword ptr __$AllocArray$rank
    mov esi, dword ptr __$AllocArray$nElems
    mov ecx, dword ptr __$AllocArray$vt
.data
align 4
__$$$2847$nElems DD 0
.code
    mov dword ptr __$$$2847$nElems, esi
.data
align 4
.code
    mov esi, dword ptr _$$GcLo
    mov esi, eax
    cmp esi, ebx
    jae $$2848$loopEnd
  $$2848$loop:
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [esi], 0
    add esi, 4
    cmp esi, ebx
    jb $$2848$loop
  $$2848$loopEnd:
    mov esi, dword ptr __$$$2847$nElems
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
    mov ebx, eax
    mov esi, eax
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 1
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    lea eax, dword ptr [ebx + 4]
  AllocArray$done:
    mov ebp, dword ptr __$AllocArray$fp
    mov esp, dword ptr __$AllocArray$sp
.data
align 4
.code
    ret
_?AllocArray endp
.data
align 4
__$AllocVector$fp DD 0
__$AllocVector$sp DD 0
__$AllocVector$nElems DD 0
__$AllocVector$size DD 0
__$AllocVector$vt DD 0
.code
align 16
_?AllocVector proc
    mov dword ptr __$AllocVector$sp, esp
    mov dword ptr __$AllocVector$fp, ebp
    mov dword ptr __$AllocVector$vt, ecx
    mov dword ptr __$AllocVector$nElems, edx
.data
align 4
.code
    mov eax, dword ptr [ecx + ?VT_BASE_LENGTH]
    mov ebx, dword ptr [ecx + ?VT_ARRAY_ELEMENT_SIZE]
    mov edi, dword ptr [ecx + ?VT_MASK]
    and edi, 15
    cmp edi, ?PTR_VECTOR_TAG
    jne $$2916$skip1
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
    jmp $$2916$skip2
  $$2916$skip1:
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
  $$2916$skip2:
    mov dword ptr __$AllocVector$size, eax
.data
align 4
.code
    mov ecx, dword ptr [esp]
    mov edx, ebp
    mov esp, dword ptr _$$DLo
.data
align 4
__$$$2918$size DD 0
__$$$2918$nextFp DD 0
__$$$2918$ra DD 0
.code
    mov dword ptr __$$$2918$ra, ecx
    mov ecx, eax
    add eax, 8
    jc _?Overflow
    cmp eax, dword ptr _$$CacheSize
    ja $$2918$skip1
.data
align 4
.code
    sub dword ptr _$$CacheSize, ecx
    mov eax, dword ptr _$$CacheSize
    mov edx, dword ptr _$$CachePtr
.data
align 4
.code
    mov dword ptr [edx + 4], eax
    add eax, edx
.data
align 4
.code
    mov dword ptr [eax], 0
.data
align 4
.code
    mov dword ptr [eax + 4], ecx
    jmp $$2918$end
  $$2918$skip1:
    mov dword ptr __$$$2918$nextFp, edx
    mov dword ptr __$$$2918$size, ecx
.data
align 4
.code
    call _?AllocSlow
  $$2918$end:
    cmp eax, 0
    je AllocVector$done
    mov edx, dword ptr __$AllocVector$size
    lea ebx, dword ptr [eax + 1 * edx + 0]
    mov edx, dword ptr __$AllocVector$nElems
    mov ecx, dword ptr __$AllocVector$vt
.data
align 4
.code
.data
align 4
.code
    mov esi, dword ptr _$$GcLo
    mov esi, eax
    cmp esi, ebx
    jae $$2925$loopEnd
  $$2925$loop:
.data
align 4
.code
.data
align 4
.code
    mov dword ptr [esi], 0
    add esi, 4
    cmp esi, ebx
    jb $$2925$loop
  $$2925$loopEnd:
.data
align 4
.code
    mov dword ptr [eax + 4], ecx
.data
align 4
.code
    mov dword ptr [eax + 8], edx
    mov ebx, eax
    mov esi, eax
    sub esi, dword ptr _$$HeapLo
    mov edi, dword ptr _$$ColorBase
    mov edx, 1
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
.data
align 4
.code
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
    shr esi, 6
    lea edi, dword ptr [edi + 4 * esi + 0]
    shr ecx, 1
    and ecx, 31
    mov eax, 3
    shl eax, cl
    not eax
.data
align 4
.code
    mov esi, dword ptr [edi]
    and esi, eax
    shl edx, cl
    or esi, edx
.data
align 4
.code
    mov dword ptr [edi], esi
    lea eax, dword ptr [ebx + 4]
  AllocVector$done:
    mov ebp, dword ptr __$AllocVector$fp
    mov esp, dword ptr __$AllocVector$sp
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
    jae $$2993$__L1
    add ecx, 3120
    jmp $$2993$__L2
  $$2993$__L1:
    add ecx, 3127
    jc _?Overflow
  $$2993$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$2994$__L1
    add ecx, 3120
    jmp $$2994$__L2
  $$2994$__L1:
    add ecx, 3127
    jc _?Overflow
  $$2994$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$2995$__L1
    add ecx, 3120
    jmp $$2995$__L2
  $$2995$__L1:
    add ecx, 3127
    jc _?Overflow
  $$2995$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$2996$__L1
    add ecx, 3120
    jmp $$2996$__L2
  $$2996$__L1:
    add ecx, 3127
    jc _?Overflow
  $$2996$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$2997$__L1
    add ecx, 3120
    jmp $$2997$__L2
  $$2997$__L1:
    add ecx, 3127
    jc _?Overflow
  $$2997$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$2998$__L1
    add ecx, 3120
    jmp $$2998$__L2
  $$2998$__L1:
    add ecx, 3127
    jc _?Overflow
  $$2998$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$2999$__L1
    add ecx, 3120
    jmp $$2999$__L2
  $$2999$__L1:
    add ecx, 3127
    jc _?Overflow
  $$2999$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3000$__L1
    add ecx, 3120
    jmp $$3000$__L2
  $$3000$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3000$__L2:
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
    jae $$3002$__L1
    add ecx, 3120
    jmp $$3002$__L2
  $$3002$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3002$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3003$__L1
    add ecx, 3120
    jmp $$3003$__L2
  $$3003$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3003$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3004$__L1
    add ecx, 3120
    jmp $$3004$__L2
  $$3004$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3004$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3005$__L1
    add ecx, 3120
    jmp $$3005$__L2
  $$3005$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3005$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3006$__L1
    add ecx, 3120
    jmp $$3006$__L2
  $$3006$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3006$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3007$__L1
    add ecx, 3120
    jmp $$3007$__L2
  $$3007$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3007$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3008$__L1
    add ecx, 3120
    jmp $$3008$__L2
  $$3008$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3008$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3009$__L1
    add ecx, 3120
    jmp $$3009$__L2
  $$3009$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3009$__L2:
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
    jae $$3011$__L1
    add ecx, 3120
    jmp $$3011$__L2
  $$3011$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3011$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3012$__L1
    add ecx, 3120
    jmp $$3012$__L2
  $$3012$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3012$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3013$__L1
    add ecx, 3120
    jmp $$3013$__L2
  $$3013$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3013$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3014$__L1
    add ecx, 3120
    jmp $$3014$__L2
  $$3014$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3014$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3015$__L1
    add ecx, 3120
    jmp $$3015$__L2
  $$3015$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3015$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3016$__L1
    add ecx, 3120
    jmp $$3016$__L2
  $$3016$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3016$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3017$__L1
    add ecx, 3120
    jmp $$3017$__L2
  $$3017$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3017$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3018$__L1
    add ecx, 3120
    jmp $$3018$__L2
  $$3018$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3018$__L2:
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
    jae $$3020$__L1
    add ecx, 3120
    jmp $$3020$__L2
  $$3020$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3020$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3021$__L1
    add ecx, 3120
    jmp $$3021$__L2
  $$3021$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3021$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3022$__L1
    add ecx, 3120
    jmp $$3022$__L2
  $$3022$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3022$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3023$__L1
    add ecx, 3120
    jmp $$3023$__L2
  $$3023$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3023$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3024$__L1
    add ecx, 3120
    jmp $$3024$__L2
  $$3024$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3024$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3025$__L1
    add ecx, 3120
    jmp $$3025$__L2
  $$3025$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3025$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3026$__L1
    add ecx, 3120
    jmp $$3026$__L2
  $$3026$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3026$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3027$__L1
    add ecx, 3120
    jmp $$3027$__L2
  $$3027$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3027$__L2:
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
    jae $$3029$__L1
    add ecx, 3120
    jmp $$3029$__L2
  $$3029$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3029$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3030$__L1
    add ecx, 3120
    jmp $$3030$__L2
  $$3030$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3030$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3031$__L1
    add ecx, 3120
    jmp $$3031$__L2
  $$3031$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3031$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3032$__L1
    add ecx, 3120
    jmp $$3032$__L2
  $$3032$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3032$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3033$__L1
    add ecx, 3120
    jmp $$3033$__L2
  $$3033$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3033$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3034$__L1
    add ecx, 3120
    jmp $$3034$__L2
  $$3034$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3034$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3035$__L1
    add ecx, 3120
    jmp $$3035$__L2
  $$3035$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3035$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3036$__L1
    add ecx, 3120
    jmp $$3036$__L2
  $$3036$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3036$__L2:
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
    jae $$3038$__L1
    add ecx, 3120
    jmp $$3038$__L2
  $$3038$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3038$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3039$__L1
    add ecx, 3120
    jmp $$3039$__L2
  $$3039$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3039$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3040$__L1
    add ecx, 3120
    jmp $$3040$__L2
  $$3040$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3040$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3041$__L1
    add ecx, 3120
    jmp $$3041$__L2
  $$3041$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3041$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3042$__L1
    add ecx, 3120
    jmp $$3042$__L2
  $$3042$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3042$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3043$__L1
    add ecx, 3120
    jmp $$3043$__L2
  $$3043$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3043$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3044$__L1
    add ecx, 3120
    jmp $$3044$__L2
  $$3044$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3044$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3045$__L1
    add ecx, 3120
    jmp $$3045$__L2
  $$3045$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3045$__L2:
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
    jbe $$3046$__L1
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
    jae $$3049$__L1
    add ecx, 3120
    jmp $$3049$__L2
  $$3049$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3049$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3050$__L1
    add ecx, 3120
    jmp $$3050$__L2
  $$3050$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3050$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3051$__L1
    add ecx, 3120
    jmp $$3051$__L2
  $$3051$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3051$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3052$__L1
    add ecx, 3120
    jmp $$3052$__L2
  $$3052$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3052$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3053$__L1
    add ecx, 3120
    jmp $$3053$__L2
  $$3053$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3053$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3054$__L1
    add ecx, 3120
    jmp $$3054$__L2
  $$3054$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3054$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3055$__L1
    add ecx, 3120
    jmp $$3055$__L2
  $$3055$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3055$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3056$__L1
    add ecx, 3120
    jmp $$3056$__L2
  $$3056$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3056$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3047$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3047$loop
  $$3046$__L1:
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
__$$$3061$dmaLo DD 0
.code
    mov eax, ecx
    add eax, 65536
    mov dword ptr __$$$3061$dmaLo, eax
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
    mov eax, dword ptr __$$$3061$dmaLo
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
  $$3573$__L1:
    cmp edi, ebp
    jae $$3573$__L2
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
    jmp $$3573$__L1
  $$3573$__L2:
    mov ecx, ebx
    shr ecx, 21
    cmp ecx, 512
    jb $$3573$__L3
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
    jae $$3636$__L1
    add ecx, 3120
    jmp $$3636$__L2
  $$3636$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3636$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3637$__L1
    add ecx, 3120
    jmp $$3637$__L2
  $$3637$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3637$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3638$__L1
    add ecx, 3120
    jmp $$3638$__L2
  $$3638$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3638$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3639$__L1
    add ecx, 3120
    jmp $$3639$__L2
  $$3639$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3639$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3640$__L1
    add ecx, 3120
    jmp $$3640$__L2
  $$3640$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3640$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3641$__L1
    add ecx, 3120
    jmp $$3641$__L2
  $$3641$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3641$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3642$__L1
    add ecx, 3120
    jmp $$3642$__L2
  $$3642$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3642$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3643$__L1
    add ecx, 3120
    jmp $$3643$__L2
  $$3643$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3643$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3634$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3634$loop
  $$3573$__L3:
    mov eax, esi
    add eax, 3
    mov edx, eax
    mov dword ptr [esi + 8 * ecx + 4096], edx
    mov ecx, dword ptr __$$$3061$dmaLo
.data
align 4
__$$$3644$dmaHi DD 0
__$$$3644$dmaLo DD 0
.code
    mov dword ptr __$$$3644$dmaLo, ecx
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
  $$3644$__L4:
    cmp edi, ebp
    jae $$3644$__L5
    mov dword ptr [edi + 0], ecx
    mov dword ptr [edi + 4], 0
    mov dword ptr [edi + 8], 258
    mov dword ptr [edi + 12], 0
    mov dword ptr [edi + 4096], edx
    mov dword ptr [edi + 4100], 0
    mov dword ptr [edi + 4104], 0
    mov dword ptr [edi + 4108], 0
    add edi, 16
    jmp $$3644$__L4
  $$3644$__L5:
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
    mov edx, dword ptr __$$$3644$dmaLo
    add edx, 18874368
    jc _?Overflow
    mov dword ptr __$$$3644$dmaHi, edx
    mov eax, dword ptr __$$$3644$dmaLo
    sub eax, 8
    mov ecx, eax
    mov edx, ?BYTE_VECTOR_VTABLE
    mov dword ptr [ecx], edx
    mov eax, dword ptr __$$$3644$dmaHi
    sub eax, dword ptr __$$$3644$dmaLo
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
__$$$3646$dmaLo DD 0
.code
    mov dword ptr __$$$3646$dmaLo, ebp
    mov esi, ?RoBiosLo
  $$3646$__L16:
    mov eax, ?RoBiosHi
    sub eax, 16
    cmp esi, eax
    jae $$3646$__L17
    mov ecx, dword ptr [esi + 0]
    mov edx, dword ptr [esi + 4]
    cmp ecx, 541348690
    jne $$3646$__L18
    cmp edx, 542266448
    jne $$3646$__L19
    mov edi, esi
    mov ebp, 0
  $$3646$__L20:
    mov eax, esi
    add eax, 20
    cmp edi, eax
    jae $$3646$__L21
    movzx eax, byte ptr [edi]
    add ebp, eax
    jc _?Overflow
    add edi, 1
    jmp $$3646$__L20
  $$3646$__L21:
    and ebp, 255
    cmp ebp, 0
    jne $$3646$__L22
.data
align 4
.code
    mov esi, dword ptr [esi + 16]
    mov ebp, dword ptr __$$$3646$dmaLo
.data
align 4
__$$$3648$dmaLo DD 0
.code
    mov dword ptr __$$$3648$dmaLo, ebp
    mov ecx, dword ptr [esi + 4]
    mov eax, esi
    add eax, ecx
    mov ebp, eax
    add esi, 36
  $$3648$__L10:
    cmp esi, ebp
    jae $$3648$__L11
    mov ecx, dword ptr [esi + 0]
    mov edx, dword ptr [ecx + 0]
    cmp edx, 1380011332
    jne $$3648$__L12
    mov edi, ecx
    add esi, 4
  $$3648$__L13:
    cmp esi, ebp
    jae $$3648$__L14
    mov ecx, dword ptr [esi + 0]
    mov edx, dword ptr [ecx + 0]
    cmp edx, 1380011332
    jne $$3648$__L15
    mov dword ptr _$$DmaAddr, 0
    jmp $$3648$done
  $$3648$__L15:
    add esi, 4
    jmp $$3648$__L13
  $$3648$__L14:
.data
align 4
.code
.data
align 4
__$$$3650$saveEcx DD 0
__$$$3650$end DD 0
.code
    mov ecx, dword ptr [edi + 4]
    mov eax, edi
    add eax, ecx
    mov dword ptr __$$$3650$end, eax
    add edi, 48
  $$3650$__L6:
    cmp edi, dword ptr __$$$3650$end
    jae $$3650$__L7
    movzx edx, word ptr [edi + 0]
    movzx ecx, word ptr [edi + 2]
    cmp edx, 0
    je $$3650$__L8
.data
align 4
.code
    jmp $$3650$done
  $$3650$__L8:
.data
align 4
.code
    mov eax, dword ptr [edi + 12]
    cmp eax, 0
    je $$3650$__L9
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
    jae $$3655$__L1
    add ecx, 3120
    jmp $$3655$__L2
  $$3655$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3655$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3656$__L1
    add ecx, 3120
    jmp $$3656$__L2
  $$3656$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3656$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3657$__L1
    add ecx, 3120
    jmp $$3657$__L2
  $$3657$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3657$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3658$__L1
    add ecx, 3120
    jmp $$3658$__L2
  $$3658$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3658$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3659$__L1
    add ecx, 3120
    jmp $$3659$__L2
  $$3659$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3659$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3660$__L1
    add ecx, 3120
    jmp $$3660$__L2
  $$3660$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3660$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3661$__L1
    add ecx, 3120
    jmp $$3661$__L2
  $$3661$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3661$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3662$__L1
    add ecx, 3120
    jmp $$3662$__L2
  $$3662$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3662$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3653$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3653$loop
  $$3650$__L9:
    mov esi, dword ptr [edi + 8]
    mov dword ptr __$$$3650$saveEcx, ecx
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
    mov ecx, dword ptr __$$$3650$saveEcx
    add edi, ecx
    jmp $$3650$__L6
  $$3650$__L7:
.data
align 4
.code
  $$3650$done:
.data
align 4
.code
    mov eax, dword ptr __$$$3648$dmaLo
    mov dword ptr _$$DmaAddr, eax
    jmp $$3648$done
  $$3648$__L12:
    add esi, 4
    jmp $$3648$__L10
  $$3648$__L11:
    mov dword ptr _$$DmaAddr, 0
  $$3648$done:
    jmp $$3646$done
  $$3646$__L22:
  $$3646$__L19:
  $$3646$__L18:
    add esi, 16
    jmp $$3646$__L16
  $$3646$__L17:
    mov dword ptr _$$DmaAddr, 0
  $$3646$done:
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
    jae $$3673$__L1
    add ecx, 3120
    jmp $$3673$__L2
  $$3673$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3673$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3674$__L1
    add ecx, 3120
    jmp $$3674$__L2
  $$3674$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3674$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3675$__L1
    add ecx, 3120
    jmp $$3675$__L2
  $$3675$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3675$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3676$__L1
    add ecx, 3120
    jmp $$3676$__L2
  $$3676$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3676$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3677$__L1
    add ecx, 3120
    jmp $$3677$__L2
  $$3677$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3677$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3678$__L1
    add ecx, 3120
    jmp $$3678$__L2
  $$3678$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3678$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3679$__L1
    add ecx, 3120
    jmp $$3679$__L2
  $$3679$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3679$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3680$__L1
    add ecx, 3120
    jmp $$3680$__L2
  $$3680$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3680$__L2:
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
    jae $$3683$__L1
    add ecx, 3120
    jmp $$3683$__L2
  $$3683$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3683$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3684$__L1
    add ecx, 3120
    jmp $$3684$__L2
  $$3684$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3684$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3685$__L1
    add ecx, 3120
    jmp $$3685$__L2
  $$3685$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3685$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3686$__L1
    add ecx, 3120
    jmp $$3686$__L2
  $$3686$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3686$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3687$__L1
    add ecx, 3120
    jmp $$3687$__L2
  $$3687$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3687$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3688$__L1
    add ecx, 3120
    jmp $$3688$__L2
  $$3688$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3688$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3689$__L1
    add ecx, 3120
    jmp $$3689$__L2
  $$3689$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3689$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3690$__L1
    add ecx, 3120
    jmp $$3690$__L2
  $$3690$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3690$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3681$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3681$loop
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
    jae $$3698$__L1
    add ecx, 3120
    jmp $$3698$__L2
  $$3698$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3698$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3699$__L1
    add ecx, 3120
    jmp $$3699$__L2
  $$3699$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3699$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3700$__L1
    add ecx, 3120
    jmp $$3700$__L2
  $$3700$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3700$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3701$__L1
    add ecx, 3120
    jmp $$3701$__L2
  $$3701$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3701$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3702$__L1
    add ecx, 3120
    jmp $$3702$__L2
  $$3702$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3702$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3703$__L1
    add ecx, 3120
    jmp $$3703$__L2
  $$3703$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3703$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3704$__L1
    add ecx, 3120
    jmp $$3704$__L2
  $$3704$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3704$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3705$__L1
    add ecx, 3120
    jmp $$3705$__L2
  $$3705$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3705$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3696$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3696$loop
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
    jae $$3718$__L1
    add ecx, 3120
    jmp $$3718$__L2
  $$3718$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3718$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3719$__L1
    add ecx, 3120
    jmp $$3719$__L2
  $$3719$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3719$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3720$__L1
    add ecx, 3120
    jmp $$3720$__L2
  $$3720$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3720$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3721$__L1
    add ecx, 3120
    jmp $$3721$__L2
  $$3721$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3721$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3722$__L1
    add ecx, 3120
    jmp $$3722$__L2
  $$3722$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3722$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3723$__L1
    add ecx, 3120
    jmp $$3723$__L2
  $$3723$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3723$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3724$__L1
    add ecx, 3120
    jmp $$3724$__L2
  $$3724$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3724$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3725$__L1
    add ecx, 3120
    jmp $$3725$__L2
  $$3725$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3725$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3716$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3716$loop
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
__$$$3731$saveEdx DD 0
__$$$3731$saveEax DD 0
.code
    mov dword ptr __$$$3731$saveEax, eax
    mov dword ptr __$$$3731$saveEdx, edx
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
    mov edx, dword ptr __$$$3731$saveEax
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
    mov edx, dword ptr __$$$3731$saveEdx
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
    jae $$3746$__L1
    add ecx, 3120
    jmp $$3746$__L2
  $$3746$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3746$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3747$__L1
    add ecx, 3120
    jmp $$3747$__L2
  $$3747$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3747$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3748$__L1
    add ecx, 3120
    jmp $$3748$__L2
  $$3748$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3748$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3749$__L1
    add ecx, 3120
    jmp $$3749$__L2
  $$3749$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3749$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3750$__L1
    add ecx, 3120
    jmp $$3750$__L2
  $$3750$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3750$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3751$__L1
    add ecx, 3120
    jmp $$3751$__L2
  $$3751$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3751$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3752$__L1
    add ecx, 3120
    jmp $$3752$__L2
  $$3752$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3752$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3753$__L1
    add ecx, 3120
    jmp $$3753$__L2
  $$3753$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3753$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3744$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3744$loop
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
    jae $$3766$__L1
    add ecx, 3120
    jmp $$3766$__L2
  $$3766$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3766$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3767$__L1
    add ecx, 3120
    jmp $$3767$__L2
  $$3767$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3767$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3768$__L1
    add ecx, 3120
    jmp $$3768$__L2
  $$3768$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3768$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3769$__L1
    add ecx, 3120
    jmp $$3769$__L2
  $$3769$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3769$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3770$__L1
    add ecx, 3120
    jmp $$3770$__L2
  $$3770$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3770$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3771$__L1
    add ecx, 3120
    jmp $$3771$__L2
  $$3771$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3771$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3772$__L1
    add ecx, 3120
    jmp $$3772$__L2
  $$3772$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3772$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3773$__L1
    add ecx, 3120
    jmp $$3773$__L2
  $$3773$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3773$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3764$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3764$loop
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
    jae $$3781$__L1
    add ecx, 3120
    jmp $$3781$__L2
  $$3781$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3781$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3782$__L1
    add ecx, 3120
    jmp $$3782$__L2
  $$3782$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3782$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3783$__L1
    add ecx, 3120
    jmp $$3783$__L2
  $$3783$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3783$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3784$__L1
    add ecx, 3120
    jmp $$3784$__L2
  $$3784$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3784$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3785$__L1
    add ecx, 3120
    jmp $$3785$__L2
  $$3785$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3785$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3786$__L1
    add ecx, 3120
    jmp $$3786$__L2
  $$3786$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3786$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3787$__L1
    add ecx, 3120
    jmp $$3787$__L2
  $$3787$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3787$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3788$__L1
    add ecx, 3120
    jmp $$3788$__L2
  $$3788$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3788$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3779$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3779$loop
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
    jae $$3791$__L1
    add ecx, 3120
    jmp $$3791$__L2
  $$3791$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3791$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3792$__L1
    add ecx, 3120
    jmp $$3792$__L2
  $$3792$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3792$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3793$__L1
    add ecx, 3120
    jmp $$3793$__L2
  $$3793$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3793$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3794$__L1
    add ecx, 3120
    jmp $$3794$__L2
  $$3794$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3794$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3795$__L1
    add ecx, 3120
    jmp $$3795$__L2
  $$3795$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3795$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3796$__L1
    add ecx, 3120
    jmp $$3796$__L2
  $$3796$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3796$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3797$__L1
    add ecx, 3120
    jmp $$3797$__L2
  $$3797$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3797$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3798$__L1
    add ecx, 3120
    jmp $$3798$__L2
  $$3798$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3798$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3789$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3789$loop
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
    jae $$3803$__L1
    add ecx, 3120
    jmp $$3803$__L2
  $$3803$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3803$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3804$__L1
    add ecx, 3120
    jmp $$3804$__L2
  $$3804$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3804$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3805$__L1
    add ecx, 3120
    jmp $$3805$__L2
  $$3805$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3805$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3806$__L1
    add ecx, 3120
    jmp $$3806$__L2
  $$3806$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3806$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3807$__L1
    add ecx, 3120
    jmp $$3807$__L2
  $$3807$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3807$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3808$__L1
    add ecx, 3120
    jmp $$3808$__L2
  $$3808$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3808$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3809$__L1
    add ecx, 3120
    jmp $$3809$__L2
  $$3809$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3809$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3810$__L1
    add ecx, 3120
    jmp $$3810$__L2
  $$3810$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3810$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3801$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3801$loop
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
    jae $$3815$__L1
    add ecx, 3120
    jmp $$3815$__L2
  $$3815$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3815$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3816$__L1
    add ecx, 3120
    jmp $$3816$__L2
  $$3816$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3816$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3817$__L1
    add ecx, 3120
    jmp $$3817$__L2
  $$3817$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3817$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3818$__L1
    add ecx, 3120
    jmp $$3818$__L2
  $$3818$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3818$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3819$__L1
    add ecx, 3120
    jmp $$3819$__L2
  $$3819$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3819$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3820$__L1
    add ecx, 3120
    jmp $$3820$__L2
  $$3820$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3820$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3821$__L1
    add ecx, 3120
    jmp $$3821$__L2
  $$3821$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3821$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3822$__L1
    add ecx, 3120
    jmp $$3822$__L2
  $$3822$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3822$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3813$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3813$loop
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
    jae $$3827$__L1
    add ecx, 3120
    jmp $$3827$__L2
  $$3827$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3827$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3828$__L1
    add ecx, 3120
    jmp $$3828$__L2
  $$3828$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3828$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3829$__L1
    add ecx, 3120
    jmp $$3829$__L2
  $$3829$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3829$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3830$__L1
    add ecx, 3120
    jmp $$3830$__L2
  $$3830$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3830$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3831$__L1
    add ecx, 3120
    jmp $$3831$__L2
  $$3831$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3831$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3832$__L1
    add ecx, 3120
    jmp $$3832$__L2
  $$3832$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3832$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3833$__L1
    add ecx, 3120
    jmp $$3833$__L2
  $$3833$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3833$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3834$__L1
    add ecx, 3120
    jmp $$3834$__L2
  $$3834$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3834$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3825$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3825$loop
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
    jae $$3874$__L1
    add ecx, 3120
    jmp $$3874$__L2
  $$3874$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3874$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3875$__L1
    add ecx, 3120
    jmp $$3875$__L2
  $$3875$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3875$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3876$__L1
    add ecx, 3120
    jmp $$3876$__L2
  $$3876$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3876$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3877$__L1
    add ecx, 3120
    jmp $$3877$__L2
  $$3877$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3877$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3878$__L1
    add ecx, 3120
    jmp $$3878$__L2
  $$3878$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3878$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3879$__L1
    add ecx, 3120
    jmp $$3879$__L2
  $$3879$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3879$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3880$__L1
    add ecx, 3120
    jmp $$3880$__L2
  $$3880$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3880$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3881$__L1
    add ecx, 3120
    jmp $$3881$__L2
  $$3881$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3881$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3872$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3872$loop
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
    jae $$3884$__L1
    add ecx, 3120
    jmp $$3884$__L2
  $$3884$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3884$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3885$__L1
    add ecx, 3120
    jmp $$3885$__L2
  $$3885$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3885$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3886$__L1
    add ecx, 3120
    jmp $$3886$__L2
  $$3886$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3886$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3887$__L1
    add ecx, 3120
    jmp $$3887$__L2
  $$3887$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3887$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3888$__L1
    add ecx, 3120
    jmp $$3888$__L2
  $$3888$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3888$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3889$__L1
    add ecx, 3120
    jmp $$3889$__L2
  $$3889$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3889$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3890$__L1
    add ecx, 3120
    jmp $$3890$__L2
  $$3890$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3890$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3891$__L1
    add ecx, 3120
    jmp $$3891$__L2
  $$3891$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3891$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3882$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3882$loop
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
    jae $$3894$__L1
    add ecx, 3120
    jmp $$3894$__L2
  $$3894$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3894$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3895$__L1
    add ecx, 3120
    jmp $$3895$__L2
  $$3895$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3895$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3896$__L1
    add ecx, 3120
    jmp $$3896$__L2
  $$3896$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3896$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3897$__L1
    add ecx, 3120
    jmp $$3897$__L2
  $$3897$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3897$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3898$__L1
    add ecx, 3120
    jmp $$3898$__L2
  $$3898$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3898$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3899$__L1
    add ecx, 3120
    jmp $$3899$__L2
  $$3899$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3899$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3900$__L1
    add ecx, 3120
    jmp $$3900$__L2
  $$3900$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3900$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3901$__L1
    add ecx, 3120
    jmp $$3901$__L2
  $$3901$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3901$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3892$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3892$loop
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
    jae $$3936$__L1
    add ecx, 3120
    jmp $$3936$__L2
  $$3936$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3936$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3937$__L1
    add ecx, 3120
    jmp $$3937$__L2
  $$3937$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3937$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3938$__L1
    add ecx, 3120
    jmp $$3938$__L2
  $$3938$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3938$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3939$__L1
    add ecx, 3120
    jmp $$3939$__L2
  $$3939$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3939$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3940$__L1
    add ecx, 3120
    jmp $$3940$__L2
  $$3940$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3940$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3941$__L1
    add ecx, 3120
    jmp $$3941$__L2
  $$3941$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3941$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3942$__L1
    add ecx, 3120
    jmp $$3942$__L2
  $$3942$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3942$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3943$__L1
    add ecx, 3120
    jmp $$3943$__L2
  $$3943$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3943$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3934$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3934$loop
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
    jae $$3946$__L1
    add ecx, 3120
    jmp $$3946$__L2
  $$3946$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3946$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3947$__L1
    add ecx, 3120
    jmp $$3947$__L2
  $$3947$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3947$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3948$__L1
    add ecx, 3120
    jmp $$3948$__L2
  $$3948$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3948$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3949$__L1
    add ecx, 3120
    jmp $$3949$__L2
  $$3949$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3949$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3950$__L1
    add ecx, 3120
    jmp $$3950$__L2
  $$3950$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3950$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3951$__L1
    add ecx, 3120
    jmp $$3951$__L2
  $$3951$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3951$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3952$__L1
    add ecx, 3120
    jmp $$3952$__L2
  $$3952$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3952$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3953$__L1
    add ecx, 3120
    jmp $$3953$__L2
  $$3953$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3953$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3944$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3944$loop
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
    jae $$3957$__L1
    add ecx, 3120
    jmp $$3957$__L2
  $$3957$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3957$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3958$__L1
    add ecx, 3120
    jmp $$3958$__L2
  $$3958$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3958$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3959$__L1
    add ecx, 3120
    jmp $$3959$__L2
  $$3959$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3959$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3960$__L1
    add ecx, 3120
    jmp $$3960$__L2
  $$3960$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3960$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3961$__L1
    add ecx, 3120
    jmp $$3961$__L2
  $$3961$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3961$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3962$__L1
    add ecx, 3120
    jmp $$3962$__L2
  $$3962$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3962$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3963$__L1
    add ecx, 3120
    jmp $$3963$__L2
  $$3963$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3963$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$3964$__L1
    add ecx, 3120
    jmp $$3964$__L2
  $$3964$__L1:
    add ecx, 3127
    jc _?Overflow
  $$3964$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3955$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3955$loop
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
    jae $$4000$__L1
    add ecx, 3120
    jmp $$4000$__L2
  $$4000$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4000$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4001$__L1
    add ecx, 3120
    jmp $$4001$__L2
  $$4001$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4001$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4002$__L1
    add ecx, 3120
    jmp $$4002$__L2
  $$4002$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4002$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4003$__L1
    add ecx, 3120
    jmp $$4003$__L2
  $$4003$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4003$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4004$__L1
    add ecx, 3120
    jmp $$4004$__L2
  $$4004$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4004$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4005$__L1
    add ecx, 3120
    jmp $$4005$__L2
  $$4005$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4005$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4006$__L1
    add ecx, 3120
    jmp $$4006$__L2
  $$4006$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4006$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4007$__L1
    add ecx, 3120
    jmp $$4007$__L2
  $$4007$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4007$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$3998$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$3998$loop
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
    jae $$4010$__L1
    add ecx, 3120
    jmp $$4010$__L2
  $$4010$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4010$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4011$__L1
    add ecx, 3120
    jmp $$4011$__L2
  $$4011$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4011$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4012$__L1
    add ecx, 3120
    jmp $$4012$__L2
  $$4012$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4012$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4013$__L1
    add ecx, 3120
    jmp $$4013$__L2
  $$4013$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4013$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4014$__L1
    add ecx, 3120
    jmp $$4014$__L2
  $$4014$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4014$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4015$__L1
    add ecx, 3120
    jmp $$4015$__L2
  $$4015$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4015$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4016$__L1
    add ecx, 3120
    jmp $$4016$__L2
  $$4016$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4016$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4017$__L1
    add ecx, 3120
    jmp $$4017$__L2
  $$4017$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4017$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$4008$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$4008$loop
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
    jae $$4020$__L1
    add ecx, 3120
    jmp $$4020$__L2
  $$4020$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4020$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4021$__L1
    add ecx, 3120
    jmp $$4021$__L2
  $$4021$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4021$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4022$__L1
    add ecx, 3120
    jmp $$4022$__L2
  $$4022$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4022$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4023$__L1
    add ecx, 3120
    jmp $$4023$__L2
  $$4023$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4023$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4024$__L1
    add ecx, 3120
    jmp $$4024$__L2
  $$4024$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4024$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4025$__L1
    add ecx, 3120
    jmp $$4025$__L2
  $$4025$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4025$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4026$__L1
    add ecx, 3120
    jmp $$4026$__L2
  $$4026$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4026$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4027$__L1
    add ecx, 3120
    jmp $$4027$__L2
  $$4027$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4027$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$4018$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$4018$loop
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
    jae $$4030$__L1
    add ecx, 3120
    jmp $$4030$__L2
  $$4030$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4030$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4031$__L1
    add ecx, 3120
    jmp $$4031$__L2
  $$4031$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4031$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4032$__L1
    add ecx, 3120
    jmp $$4032$__L2
  $$4032$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4032$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4033$__L1
    add ecx, 3120
    jmp $$4033$__L2
  $$4033$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4033$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4034$__L1
    add ecx, 3120
    jmp $$4034$__L2
  $$4034$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4034$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4035$__L1
    add ecx, 3120
    jmp $$4035$__L2
  $$4035$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4035$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4036$__L1
    add ecx, 3120
    jmp $$4036$__L2
  $$4036$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4036$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4037$__L1
    add ecx, 3120
    jmp $$4037$__L2
  $$4037$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4037$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$4028$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$4028$loop
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
    jae $$4055$__L1
    add ecx, 3120
    jmp $$4055$__L2
  $$4055$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4055$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4056$__L1
    add ecx, 3120
    jmp $$4056$__L2
  $$4056$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4056$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4057$__L1
    add ecx, 3120
    jmp $$4057$__L2
  $$4057$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4057$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4058$__L1
    add ecx, 3120
    jmp $$4058$__L2
  $$4058$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4058$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4059$__L1
    add ecx, 3120
    jmp $$4059$__L2
  $$4059$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4059$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4060$__L1
    add ecx, 3120
    jmp $$4060$__L2
  $$4060$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4060$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4061$__L1
    add ecx, 3120
    jmp $$4061$__L2
  $$4061$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4061$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4062$__L1
    add ecx, 3120
    jmp $$4062$__L2
  $$4062$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4062$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$4053$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$4053$loop
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
    jae $$4065$__L1
    add ecx, 3120
    jmp $$4065$__L2
  $$4065$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4065$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4066$__L1
    add ecx, 3120
    jmp $$4066$__L2
  $$4066$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4066$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4067$__L1
    add ecx, 3120
    jmp $$4067$__L2
  $$4067$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4067$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4068$__L1
    add ecx, 3120
    jmp $$4068$__L2
  $$4068$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4068$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4069$__L1
    add ecx, 3120
    jmp $$4069$__L2
  $$4069$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4069$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4070$__L1
    add ecx, 3120
    jmp $$4070$__L2
  $$4070$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4070$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4071$__L1
    add ecx, 3120
    jmp $$4071$__L2
  $$4071$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4071$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4072$__L1
    add ecx, 3120
    jmp $$4072$__L2
  $$4072$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4072$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$4063$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$4063$loop
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
    jae $$4076$__L1
    add ecx, 3120
    jmp $$4076$__L2
  $$4076$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4076$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4077$__L1
    add ecx, 3120
    jmp $$4077$__L2
  $$4077$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4077$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4078$__L1
    add ecx, 3120
    jmp $$4078$__L2
  $$4078$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4078$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4079$__L1
    add ecx, 3120
    jmp $$4079$__L2
  $$4079$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4079$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4080$__L1
    add ecx, 3120
    jmp $$4080$__L2
  $$4080$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4080$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4081$__L1
    add ecx, 3120
    jmp $$4081$__L2
  $$4081$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4081$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4082$__L1
    add ecx, 3120
    jmp $$4082$__L2
  $$4082$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4082$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4083$__L1
    add ecx, 3120
    jmp $$4083$__L2
  $$4083$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4083$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$4074$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$4074$loop
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
    jae $$4087$__L1
    add ecx, 3120
    jmp $$4087$__L2
  $$4087$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4087$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4088$__L1
    add ecx, 3120
    jmp $$4088$__L2
  $$4088$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4088$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4089$__L1
    add ecx, 3120
    jmp $$4089$__L2
  $$4089$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4089$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4090$__L1
    add ecx, 3120
    jmp $$4090$__L2
  $$4090$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4090$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4091$__L1
    add ecx, 3120
    jmp $$4091$__L2
  $$4091$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4091$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4092$__L1
    add ecx, 3120
    jmp $$4092$__L2
  $$4092$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4092$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4093$__L1
    add ecx, 3120
    jmp $$4093$__L2
  $$4093$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4093$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4094$__L1
    add ecx, 3120
    jmp $$4094$__L2
  $$4094$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4094$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$4085$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$4085$loop
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
    jae $$4100$__L1
    add ecx, 3120
    jmp $$4100$__L2
  $$4100$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4100$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4101$__L1
    add ecx, 3120
    jmp $$4101$__L2
  $$4101$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4101$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4102$__L1
    add ecx, 3120
    jmp $$4102$__L2
  $$4102$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4102$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4103$__L1
    add ecx, 3120
    jmp $$4103$__L2
  $$4103$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4103$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4104$__L1
    add ecx, 3120
    jmp $$4104$__L2
  $$4104$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4104$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4105$__L1
    add ecx, 3120
    jmp $$4105$__L2
  $$4105$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4105$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4106$__L1
    add ecx, 3120
    jmp $$4106$__L2
  $$4106$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4106$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4107$__L1
    add ecx, 3120
    jmp $$4107$__L2
  $$4107$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4107$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$4098$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$4098$loop
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
    jae $$4110$__L1
    add ecx, 3120
    jmp $$4110$__L2
  $$4110$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4110$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4111$__L1
    add ecx, 3120
    jmp $$4111$__L2
  $$4111$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4111$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4112$__L1
    add ecx, 3120
    jmp $$4112$__L2
  $$4112$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4112$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4113$__L1
    add ecx, 3120
    jmp $$4113$__L2
  $$4113$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4113$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4114$__L1
    add ecx, 3120
    jmp $$4114$__L2
  $$4114$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4114$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4115$__L1
    add ecx, 3120
    jmp $$4115$__L2
  $$4115$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4115$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4116$__L1
    add ecx, 3120
    jmp $$4116$__L2
  $$4116$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4116$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4117$__L1
    add ecx, 3120
    jmp $$4117$__L2
  $$4117$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4117$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$4108$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$4108$loop
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
    jae $$4121$__L1
    add ecx, 3120
    jmp $$4121$__L2
  $$4121$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4121$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4122$__L1
    add ecx, 3120
    jmp $$4122$__L2
  $$4122$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4122$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4123$__L1
    add ecx, 3120
    jmp $$4123$__L2
  $$4123$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4123$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4124$__L1
    add ecx, 3120
    jmp $$4124$__L2
  $$4124$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4124$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4125$__L1
    add ecx, 3120
    jmp $$4125$__L2
  $$4125$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4125$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4126$__L1
    add ecx, 3120
    jmp $$4126$__L2
  $$4126$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4126$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4127$__L1
    add ecx, 3120
    jmp $$4127$__L2
  $$4127$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4127$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4128$__L1
    add ecx, 3120
    jmp $$4128$__L2
  $$4128$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4128$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$4119$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$4119$loop
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
    jae $$4132$__L1
    add ecx, 3120
    jmp $$4132$__L2
  $$4132$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4132$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4133$__L1
    add ecx, 3120
    jmp $$4133$__L2
  $$4133$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4133$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4134$__L1
    add ecx, 3120
    jmp $$4134$__L2
  $$4134$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4134$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4135$__L1
    add ecx, 3120
    jmp $$4135$__L2
  $$4135$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4135$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4136$__L1
    add ecx, 3120
    jmp $$4136$__L2
  $$4136$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4136$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4137$__L1
    add ecx, 3120
    jmp $$4137$__L2
  $$4137$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4137$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4138$__L1
    add ecx, 3120
    jmp $$4138$__L2
  $$4138$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4138$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4139$__L1
    add ecx, 3120
    jmp $$4139$__L2
  $$4139$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4139$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$4130$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$4130$loop
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
    jae $$4142$__L1
    add ecx, 3120
    jmp $$4142$__L2
  $$4142$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4142$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4143$__L1
    add ecx, 3120
    jmp $$4143$__L2
  $$4143$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4143$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4144$__L1
    add ecx, 3120
    jmp $$4144$__L2
  $$4144$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4144$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4145$__L1
    add ecx, 3120
    jmp $$4145$__L2
  $$4145$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4145$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4146$__L1
    add ecx, 3120
    jmp $$4146$__L2
  $$4146$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4146$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4147$__L1
    add ecx, 3120
    jmp $$4147$__L2
  $$4147$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4147$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4148$__L1
    add ecx, 3120
    jmp $$4148$__L2
  $$4148$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4148$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4149$__L1
    add ecx, 3120
    jmp $$4149$__L2
  $$4149$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4149$__L2:
    mov word ptr [edx + 753664], cx
    mov edx, 753664
    mov eax, 0
  $$4140$loop:
    lea eax, dword ptr [eax + 1]
    mov word ptr [edx], ax
    jmp $$4140$loop
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
    jae $$4151$__L1
    add ecx, 3120
    jmp $$4151$__L2
  $$4151$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4151$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4152$__L1
    add ecx, 3120
    jmp $$4152$__L2
  $$4152$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4152$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4153$__L1
    add ecx, 3120
    jmp $$4153$__L2
  $$4153$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4153$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4154$__L1
    add ecx, 3120
    jmp $$4154$__L2
  $$4154$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4154$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4155$__L1
    add ecx, 3120
    jmp $$4155$__L2
  $$4155$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4155$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4156$__L1
    add ecx, 3120
    jmp $$4156$__L2
  $$4156$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4156$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4157$__L1
    add ecx, 3120
    jmp $$4157$__L2
  $$4157$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4157$__L2:
    mov word ptr [edx + 753664], cx
    sub edx, 2
    shr eax, 4
.data
align 4
.code
    mov ecx, eax
    and ecx, 15
    cmp ecx, 10
    jae $$4158$__L1
    add ecx, 3120
    jmp $$4158$__L2
  $$4158$__L1:
    add ecx, 3127
    jc _?Overflow
  $$4158$__L2:
    mov word ptr [edx + 753664], cx
    ret
_?DebugPrintHex endp
end
;; boogie instructions  (before inlining): 1624
;;    x86 instructions  (before inlining): 1859
;; unchecked arithmetic (before inlining): 160
;;   checked arithmetic (before inlining): 65

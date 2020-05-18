procedure startTimer()
#line 1

requires word(ecx);
modifies $Eip,eax,$TimerSeq,$TimerFreq;
ensures TimerOk(($TimerSeq))&&($TimerFreq)==old(ecx);
{
  call eax:=Mov(48);
  call PitModeOut8(ecx);
  call eax:=Mov(ecx);
  call PitFreqOut8();
  call eax:=Shr(eax,8);
  call PitFreqOut8();
}

procedure setupIoTablesHelper1()
#line 14

requires (word(?iomLo)&&word(?iomHi))&&word(?dmaHi);
requires ?dmaLo==?iomHi;
requires and(ebx,4095)==0;
requires ?dmaLo<=ebx&&ebx+2097152+4096<=?dmaHi;
requires Aligned(esi);
requires ?iomLo<=esi&&esi+32768<=?iomHi;
requires and(esi+0,4095)==0;
requires and(esi+4096,4095)==0;
requires and(esi+8192,4095)==0;
requires and(esi+12288,4095)==0;
requires !($IomFrozen);
modifies $Eip,eax,ebx,ecx,edx,edi,ebp;
modifies $IomMem;
ensures IoPageTable(($IomMem),esi);
ensures (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<512==>(($IomMem)[esi+4096+8*i]==0||($IomMem)[esi+4096+8*i]==esi+3)&&($IomMem)[esi+4096+8*i+4]==0);


ensures (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<512==>($IomMem)[esi+8192+8*i]==0&&($IomMem)[esi+8192+8*i+4]==0);


ensures (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<512==>($IomMem)[esi+12288+8*i]==0&&($IomMem)[esi+12288+8*i+4]==0);
{
#line 14
  var $entry:int;
#line 39
  call edi:=Mov(esi);
  call eax:=Mov(edi);
#line 40
  call eax:=Add(eax,4096);
#line 40
  call ebp:=Mov(eax);
  call edx:=Mov(ebx);
  $entry:=0;
  __L1:
  assert ((Aligned(edi)&&TV(edi))&&TO(1))&&TO(2);
  assert edx==ebx+($entry)*4096;
  assert and(edx,4095)==0;
  assert edi==esi+8*($entry);
  assert 0<=($entry)&&($entry)<=512;
  assert (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<($entry)==>IoPageTableEntry(($IomMem)[esi+8*i],($IomMem)[esi+8*i+4]));
  
  assert (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<($entry)==>($IomMem)[esi+4096+8*i]==0&&($IomMem)[esi+4096+8*i+4]==0);
  
  assert (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<($entry)==>($IomMem)[esi+8192+8*i]==0&&($IomMem)[esi+8192+8*i+4]==0);
  
  assert (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<($entry)==>($IomMem)[esi+12288+8*i]==0&&($IomMem)[esi+12288+8*i+4]==0);
#line 43
  havoc $Eip;
#line 43
  if(edi>=ebp)
  {
#line 43
    goto __L2;
  }
#line 58
  assert ((((TO(1024)&&TO(1025))&&TO(2048))&&TO(2049))&&TO(3072))&&TO(3073);
  call eax:=Mov(edx);
#line 59
  call eax:=Add(eax,3);
#line 59
  call ecx:=Mov(eax);
  call IomStore(edi+0,ecx);
  call IomStore(edi+4,0);
  call IomStore(edi+4096,0);
  call IomStore(edi+4100,0);
  call IomStore(edi+8192,0);
  call IomStore(edi+8196,0);
  call IomStore(edi+12288,0);
  call IomStore(edi+12292,0);
  $entry:=($entry)+1;
  call edi:=Add(edi,8);
  call __add4kAligned(edx);
  call edx:=Add(edx,4096);
#line 43
  havoc $Eip;
#line 43
  goto __L1;
#line 43
  __L2:
#line 74
  call ecx:=Mov(ebx);
  call ecx:=Shr(ecx,21);
  havoc $Eip;
#line 76
  if(ecx<512)
  {
#line 76
    goto __L3;
  }
  call eax:=Mov(1431633966);
  call debugBreak();
#line 76
  __L3:
  
  
  
  
  assert TV(ecx)&&TO(2*ecx+1024);
  call eax:=Mov(esi);
#line 82
  call eax:=Add(eax,3);
#line 82
  call edx:=Mov(eax);
  call IomStore(esi+8*ecx+4096,edx);
}


procedure setupIoTablesHelper2()
#line 87

requires word(?iomLo)&&word(?iomHi);
requires Aligned(?dmaLo);
requires ?dmaLo==?iomHi;
requires ecx==?dmaLo;
requires IoPageTable(($IomMem),esi);
requires (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<512==>(($IomMem)[esi+4096+8*i]==0||($IomMem)[esi+4096+8*i]==esi+3)&&($IomMem)[esi+4096+8*i+4]==0);


requires (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<512==>($IomMem)[esi+8192+8*i]==0&&($IomMem)[esi+8192+8*i+4]==0);


requires (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<512==>($IomMem)[esi+12288+8*i]==0&&($IomMem)[esi+12288+8*i+4]==0);


requires Aligned(esi);
requires Aligned(esi+16384);
requires ?iomLo<=esi&&esi+32768<=?iomHi;
requires and(esi+4096,4095)==0;
requires and(esi+8192,4095)==0;
requires and(esi+12288,4095)==0;
requires and(esi+16384,4095)==0;
requires and(esi+20480,4095)==0;
requires !($IomFrozen);
modifies $Eip,eax,ebx,ecx,edx,edi,ebp;
modifies $IomMem;
ensures IoPageTable(($IomMem),esi);
ensures ($IomMem)[esi+8192]==esi+4096+3;
ensures ($IomMem)[esi+8196]==0;
ensures ($IomMem)[esi+12288]==esi+8192+3;
ensures ($IomMem)[esi+12292]==0;
ensures (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<512==>(($IomMem)[esi+4096+8*i]==0||($IomMem)[esi+4096+8*i]==esi+3)&&($IomMem)[esi+4096+8*i+4]==0);


ensures (forall i:int::{TV(i)}(TV(i)&&1<=i)&&i<512==>($IomMem)[esi+8192+8*i]==0&&($IomMem)[esi+8192+8*i+4]==0);


ensures (forall i:int::{TV(i)}(TV(i)&&1<=i)&&i<512==>($IomMem)[esi+12288+8*i]==0&&($IomMem)[esi+12288+8*i+4]==0);


ensures (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<256==>((($IomMem)[esi+16384+16*i+0]==esi+12288+3&&($IomMem)[esi+16384+16*i+4]==0)&&($IomMem)[esi+16384+16*i+8]==258)&&($IomMem)[esi+16384+16*i+12]==0);




ensures (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<256==>((($IomMem)[esi+20480+16*i+0]==esi+16384+1&&($IomMem)[esi+20480+16*i+4]==0)&&($IomMem)[esi+20480+16*i+8]==0)&&($IomMem)[esi+20480+16*i+12]==0);




ensures ($IomMem)[?dmaLo-8]==?BYTE_VECTOR_VTABLE;
ensures ($IomMem)[?dmaLo-4]==?dmaHi-?dmaLo;
{
#line 87
  var dmaHi:int;
#line 87
  var $entry:int;
#line 87
  var dmaLo:int;
#line 140
  call dmaLo:=Mov(ecx);
  
  $entry:=0;
  call eax:=Mov(esi);
#line 143
  call eax:=Add(eax,16384);
#line 143
  call edi:=Mov(eax);
  call eax:=Mov(edi);
#line 144
  call eax:=Add(eax,4096);
#line 144
  call ebp:=Mov(eax);
  call eax:=Mov(esi);
#line 145
  call eax:=Add(eax,12288);
#line 145
  call eax:=Add(eax,3);
#line 145
  call ecx:=Mov(eax);
  call eax:=Mov(esi);
#line 146
  call eax:=Add(eax,16384);
#line 146
  call eax:=Add(eax,1);
#line 146
  call edx:=Mov(eax);
  __L4:
  assert ((((Aligned(edi)&&TV(edi))&&TO(1))&&TO(2))&&TO(3))&&TO(4);
  assert edi==esi+16384+16*($entry);
  assert 0<=($entry)&&($entry)<=256;
  assert IoPageTable(($IomMem),esi);
  assert (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<512==>(($IomMem)[esi+4096+8*i]==0||($IomMem)[esi+4096+8*i]==esi+3)&&($IomMem)[esi+4096+8*i+4]==0);
  
  
  assert (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<512==>($IomMem)[esi+8192+8*i]==0&&($IomMem)[esi+8192+8*i+4]==0);
  
  
  assert (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<512==>($IomMem)[esi+12288+8*i]==0&&($IomMem)[esi+12288+8*i+4]==0);
  
  
  assert (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<($entry)==>((($IomMem)[esi+16384+16*i+0]==esi+12288+3&&($IomMem)[esi+16384+16*i+4]==0)&&($IomMem)[esi+16384+16*i+8]==258)&&($IomMem)[esi+16384+16*i+12]==0);
  
  
  
  
  assert (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<($entry)==>((($IomMem)[esi+20480+16*i+0]==esi+16384+1&&($IomMem)[esi+20480+16*i+4]==0)&&($IomMem)[esi+20480+16*i+8]==0)&&($IomMem)[esi+20480+16*i+12]==0);
#line 147
  havoc $Eip;
#line 147
  if(edi>=ebp)
  {
#line 147
    goto __L5;
  }
#line 172
  call IomStore(edi+0,ecx);
  call IomStore(edi+4,0);
  call IomStore(edi+8,258);
  call IomStore(edi+12,0);
  assert ((TO(1024)&&TO(1025))&&TO(1026))&&TO(1027);
  call IomStore(edi+4096,edx);
  call IomStore(edi+4100,0);
  call IomStore(edi+4104,0);
  call IomStore(edi+4108,0);
  $entry:=($entry)+1;
  call edi:=Add(edi,16);
#line 147
  havoc $Eip;
#line 147
  goto __L4;
#line 147
  __L5:
#line 185
  assert TV(esi);
  assert (TV(0)&&TO(2048))&&TO(3072);
  call eax:=Mov(esi);
#line 187
  call eax:=Add(eax,4096);
#line 187
  call eax:=Add(eax,3);
#line 187
  call edx:=Mov(eax);
  call IomStore(esi+8192,edx);
  call eax:=Mov(esi);
#line 189
  call eax:=Add(eax,8192);
#line 189
  call eax:=Add(eax,3);
#line 189
  call edx:=Mov(eax);
  call IomStore(esi+12288,edx);
  
  call edx:=Mov(dmaLo);
  call edx:=AddChecked(edx,18874368);
  call dmaHi:=Mov(edx);
  
  assert (TV(?dmaLo)&&TO(0-1))&&TO(0-2);
  call eax:=Mov(dmaLo);
#line 197
  call eax:=Sub(eax,8);
#line 197
  call ecx:=Mov(eax);
  call edx:=Mov(?BYTE_VECTOR_VTABLE);
  call IomStore(ecx,edx);
  call eax:=Mov(dmaHi);
#line 200
  call eax:=Sub(eax,dmaLo);
#line 200
  call edx:=Mov(eax);
  call IomStore(ecx+4,edx);
}

procedure setupIoTablesHelper3()
#line 204

requires and(esi+20480,4095)==0;
requires IoContextTable(($IomMem),esi+16384);
requires (forall i:int::{TV(i)}(TV(i)&&0<=i)&&i<256==>((($IomMem)[esi+20480+16*i+0]==esi+16384+1&&($IomMem)[esi+20480+16*i+4]==0)&&($IomMem)[esi+20480+16*i+8]==0)&&($IomMem)[esi+20480+16*i+12]==0);




ensures IoRootTable(($IomMem),esi+20480);
{
}

procedure setupIoTables()
#line 216

requires (word(?iomLo)&&word(?iomHi))&&word(?dmaHi);
requires ecx==?iomLo;
requires ?dmaLo==?iomHi;
requires Aligned(?dmaLo);
requires !($IomFrozen);
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp;
modifies $IomMem;
ensures IoRootTable(($IomMem),eax);
ensures ($IomMem)[?dmaLo-8]==?BYTE_VECTOR_VTABLE;
ensures ($IomMem)[?dmaLo-4]==?dmaHi-?dmaLo;
{
#line 216
  var dmaLo:int;
#line 228
  call eax:=Mov(ecx);
#line 228
  call eax:=Add(eax,65536);
#line 228
  call dmaLo:=Mov(eax);
  
  call ecx:=Add(ecx,4096);
  call eax:=Mov(ecx);
#line 231
  call eax:=Add(eax,4096);
#line 231
  call esi:=Mov(eax);
  call __is4kAligned(ecx);
  call __add4kAligned(ecx-and(ecx,4095)+0);
  call __add4kAligned(ecx-and(ecx,4095)+4096);
  call __add4kAligned(ecx-and(ecx,4095)+8192);
  call __add4kAligned(ecx-and(ecx,4095)+12288);
  call __add4kAligned(ecx-and(ecx,4095)+16384);
  call __add4kAligned(ecx-and(ecx,4095)+20480);
  call ecx:=And(ecx,4095);
  call esi:=Sub(esi,ecx);
  assert TV(esi);
  
  call eax:=Mov(dmaLo);
#line 243
  call eax:=Add(eax,2097152);
#line 243
  call ecx:=Mov(eax);
  call eax:=Mov(ecx);
#line 244
  call eax:=Add(eax,2097152);
#line 244
  call ebx:=Mov(eax);
  call __is2m4kAligned(ecx);
  call ecx:=And(ecx,2097151);
  call ebx:=Sub(ebx,ecx);
  
  call setupIoTablesHelper1();
  
  call ecx:=Mov(dmaLo);
  call setupIoTablesHelper2();
  
  call setupIoTablesHelper3();
  
  call eax:=Mov(esi);
#line 256
  call eax:=Add(eax,20480);
}

procedure setIoMmuRegs($entry:int)
#line 259

requires esi==?DrhdRegs[($entry)];
requires IoRootTable(($IomMem),ebx);
requires ($IoMmuState)[($entry)]==0;
modifies $Eip,eax,ecx,edx,esi,ebp;
modifies $IoMmuState,$IomFrozen;
ensures ($IoMmuState)[($entry)]==4;
ensures (forall i:int::{($IoMmuState)[i]}i!=($entry)==>($IoMmuState)[i]==old($IoMmuState)[i]);
{
  call IomRegStore(($entry),esi+32,ebx);
  call IomRegStore(($entry),esi+36,0);
  call eax:=Mov(1);
  call eax:=Shl(eax,30);
  call IomRegStore(($entry),esi+24,eax);
  call eax:=Mov(1);
  call eax:=Shl(eax,31);
  call IomRegStore(($entry),esi+24,eax);
}





procedure readDmar()
#line 282

requires ?DmarExists;
requires edi==?DmarPtr;
requires IoRootTable(($IomMem),ebx);
requires ($IomMem)[?dmaLo-8]==?BYTE_VECTOR_VTABLE;
requires ($IomMem)[?dmaLo-4]==?dmaHi-?dmaLo;
requires (forall i:int::{($IoMmuState)[i]}($IoMmuState)[i]==0);
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp;
modifies $IoMmuState,$IomFrozen,$IoMmuEnabled;
ensures ($IoMmuEnabled);
{
#line 282
  var saveEcx:int;
#line 282
  var $entry:int;
#line 282
  var end:int;
#line 293
  call ecx:=RoLoad32(edi+4);
  call eax:=Mov(edi);
#line 294
  call eax:=Add(eax,ecx);
#line 294
  call end:=Mov(eax);
  
  $entry:=0;
  call edi:=Add(edi,48);
  __L6:
  assert TV(($entry));
  assert ?DmarPtr+48<=edi&&edi<=?DmarPtr+?DmarLen;
  assert MaybeDrhd(edi,($entry));
  assert (forall i:int::{($IoMmuState)[i]}i>=($entry)==>($IoMmuState)[i]==0);
  assert (forall i:int::{($IoMmuState)[i]}0<=i&&i<($entry)==>($IoMmuState)[i]==4);
#line 298
  havoc $Eip;
#line 298
  if(edi>=end)
  {
#line 298
    goto __L7;
  }
  
  
  
  
  
  
  
  call edx:=RoLoadU16(edi+0);
  call ecx:=RoLoadU16(edi+2);
  havoc $Eip;
#line 309
  if(edx==0)
  {
#line 309
    goto __L8;
  }
  call drhdEnd(edi,($entry));
  havoc $Eip;
#line 312
  goto done;
#line 309
  __L8:
  
  
  
  
  call drhdExists(edi,($entry));
  call eax:=RoLoad32(edi+12);
  havoc $Eip;
#line 316
  if(eax==0)
  {
#line 316
    goto __L9;
  }
  call eax:=Mov(1431633958);
  call debugBreak();
#line 316
  __L9:
  
  
  
  
  call esi:=RoLoad32(edi+8);
  
  call saveEcx:=Mov(ecx);
  call setIoMmuRegs(($entry));
  call ecx:=Mov(saveEcx);
  
  $entry:=($entry)+1;
  call edi:=Add(edi,ecx);
#line 298
  havoc $Eip;
#line 298
  goto __L6;
#line 298
  __L7:
#line 330
  call drhdEnd(edi,($entry));
  
  done:
  call iomEnable();
}

procedure readRsdt()
#line 336

requires ?RsdpExists;
requires !($IoMmuEnabled);
requires esi==?RsdtPtr;
requires ebp==?dmaLo;
requires IoRootTable(($IomMem),ebx);
requires ($IomMem)[?dmaLo-8]==?BYTE_VECTOR_VTABLE;
requires ($IomMem)[?dmaLo-4]==?dmaHi-?dmaLo;
requires (forall i:int::{($IoMmuState)[i]}($IoMmuState)[i]==0);
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp;
modifies $IoMmuState,$IomFrozen,$IoMmuEnabled,DmaAddr;
ensures (DmaAddr==0&&!($IoMmuEnabled))||(DmaAddr==?dmaLo&&($IoMmuEnabled));
{
#line 336
  var $dmarEntry:int;
#line 336
  var $entry:int;
#line 336
  var dmaLo:int;
#line 349
  call dmaLo:=Mov(ebp);
  
  call ecx:=RoLoad32(esi+4);
  call eax:=Mov(esi);
#line 352
  call eax:=Add(eax,ecx);
#line 352
  call ebp:=Mov(eax);
  
  $entry:=0;
  call esi:=Add(esi,36);
  __L10:
  assert TV(($entry));
  assert esi==?RsdtPtr+36+4*($entry);
  assert 0<=($entry)&&($entry)<=?RsdtCount;
  assert (forall j:int::{TV(j)}(TV(j)&&0<=j)&&j<($entry)==>!MatchesDmar(ro32(?RsdtPtr+36+4*j)));
#line 356
  havoc $Eip;
#line 356
  if(esi>=ebp)
  {
#line 356
    goto __L11;
  }
  
  
  
  
  
  call ecx:=RoLoad32(esi+0);
  call edx:=RoLoad32(ecx+0);
  havoc $Eip;
#line 365
  if(edx!=1380011332)
  {
#line 365
    goto __L12;
  }
  
  $dmarEntry:=($entry);
  call edi:=Mov(ecx);
  assert MatchesDmar(edi);
  $entry:=($entry)+1;
  call esi:=Add(esi,4);
  __L13:
  assert TV(($entry));
  assert esi==?RsdtPtr+36+4*($entry);
  assert 0<=($entry)&&($entry)<=?RsdtCount;
  assert (forall j:int::{TV(j)}((TV(j)&&0<=j)&&j<($entry))&&j!=($dmarEntry)==>!MatchesDmar(ro32(?RsdtPtr+36+4*j)));
#line 373
  havoc $Eip;
#line 373
  if(esi>=ebp)
  {
#line 373
    goto __L14;
  }
  
  
  
  
  
  call ecx:=RoLoad32(esi+0);
  call edx:=RoLoad32(ecx+0);
  havoc $Eip;
#line 382
  if(edx!=1380011332)
  {
#line 382
    goto __L15;
  }
  
  call DmaAddr:=Mov(0);
  havoc $Eip;
#line 386
  goto done;
#line 382
  __L15:
  
  
  
  
  
  $entry:=($entry)+1;
  call esi:=Add(esi,4);
#line 373
  havoc $Eip;
#line 373
  goto __L13;
#line 373
  __L14:
#line 391
  call dmarExists(edi,($dmarEntry));
  call readDmar();
  call eax:=Mov(dmaLo);
  call DmaAddr:=Mov(eax);
  havoc $Eip;
#line 395
  goto done;
#line 365
  __L12:
#line 397
  $entry:=($entry)+1;
  call esi:=Add(esi,4);
#line 356
  havoc $Eip;
#line 356
  goto __L10;
#line 356
  __L11:
#line 400
  call DmaAddr:=Mov(0);
  
  done:
}

procedure startIoMmu()
#line 405

requires IoRootTable(($IomMem),ebx);
requires !($IoMmuEnabled);
requires ebp==?dmaLo;
requires ($IomMem)[?dmaLo-8]==?BYTE_VECTOR_VTABLE;
requires ($IomMem)[?dmaLo-4]==?dmaHi-?dmaLo;
requires (forall i:int::{($IoMmuState)[i]}($IoMmuState)[i]==0);
modifies $Eip,eax,ebx,ecx,edx,esi,edi,ebp;
modifies $IoMmuState,$IomFrozen,$IoMmuEnabled,DmaAddr;
ensures (DmaAddr==0&&!($IoMmuEnabled))||(DmaAddr==?dmaLo&&($IoMmuEnabled));
{
#line 405
  var $entry:int;
#line 405
  var dmaLo:int;
#line 416
  call dmaLo:=Mov(ebp);
  $entry:=0;
  call esi:=Mov(?RoBiosLo);
  __L16:
  assert TV(($entry));
  assert esi==?RoBiosLo+16*($entry);
  assert ?RoBiosLo<=esi&&esi<=?RoBiosHi-16;
  assert (forall j:int::{TV(j)}(TV(j)&&j<($entry))&&inBiosRo(?RoBiosLo+16*j)==>!MatchesRsdp(?RoBiosLo+16*j));
#line 419
  call eax:=Mov(?RoBiosHi);
#line 419
  call eax:=Sub(eax,16);
#line 419
  havoc $Eip;
#line 419
  if(esi>=eax)
  {
#line 419
    goto __L17;
  }
  
  
  
  
  
  call ecx:=RoLoad32(esi+0);
  call edx:=RoLoad32(esi+4);
  
  havoc $Eip;
#line 429
  if(ecx!=541348690)
  {
#line 429
    goto __L18;
  }
  havoc $Eip;
#line 431
  if(edx!=542266448)
  {
#line 431
    goto __L19;
  }
  call edi:=Mov(esi);
  call ebp:=Mov(0);
  __L20:
  assert TV(esi)&&TV(edi);
  assert esi<=edi&&edi<=esi+20;
  assert ebp==ByteSum(esi,edi);
#line 435
  call eax:=Mov(esi);
#line 435
  call eax:=Add(eax,20);
#line 435
  havoc $Eip;
#line 435
  if(edi>=eax)
  {
#line 435
    goto __L21;
  }
  
  
  
  call eax:=RoLoadU8(edi);
  call ebp:=AddChecked(ebp,eax);
  call edi:=Add(edi,1);
#line 435
  havoc $Eip;
#line 435
  goto __L20;
#line 435
  __L21:
#line 445
  call ebp:=And(ebp,255);
  havoc $Eip;
#line 446
  if(ebp!=0)
  {
#line 446
    goto __L22;
  }
  call rsdpExists(esi,($entry));
  call esi:=RoLoad32(esi+16);
  call ebp:=Mov(dmaLo);
  call readRsdt();
  havoc $Eip;
#line 452
  goto done;
#line 446
  __L22:
#line 431
  __L19:
#line 429
  __L18:
#line 457
  $entry:=($entry)+1;
  call esi:=Add(esi,16);
#line 419
  havoc $Eip;
#line 419
  goto __L16;
#line 419
  __L17:
#line 460
  call DmaAddr:=Mov(0);
  
  done:
}

implementation NucleusEntryPoint($smap:int)
#line 465
{
#line 465
  var s:int;
#line 465
  var handler:int;
#line 465
  var idtLo:int;
#line 465
  var platform:int;
  
  call platform:=Mov(ecx);
  
  call eax:=Mov(305419896);
  call edx:=Mov(20);
  call writeHex();
  
  call eax:=Mov(platform);
  call edx:=Mov(40);
  call writeHex();
  
  call esi:=Mov(platform);
  call eax:=RoLoad32(esi+40);
  call edx:=Mov(60);
  call writeHex();
  
  call esi:=Mov(platform);
  call eax:=RoLoad32(esi+44);
  call edx:=Mov(80);
  call writeHex();
  
  
  call esi:=Mov(platform);
  call eax:=RoLoad32(esi+44);
  havoc $Eip;
#line 490
  if(eax<4)
  {
#line 490
    goto __L23;
  }
  
  call edi:=RoLoad32(esi+40);
  
  
  call eax:=RoLoad32(edi+72);
  call ebx:=Mov(eax);
  call edx:=Mov(100);
  call writeHex();
  
  call eax:=RoLoad32(edi+80);
  call esi:=Mov(eax);
  call edx:=Mov(120);
  call writeHex();
  
  call eax:=RoLoad32(edi+88);
  havoc $Eip;
#line 507
  if(eax!=1)
  {
#line 507
    goto __L24;
  }
  
  
  
  
  call idtLo:=Mov(ebx);
  
  
  call eax:=Mov(ebx);
#line 516
  call eax:=Add(eax,2048);
#line 516
  call ecx:=Mov(eax);
  call ecx:=Add(ecx,65536);
  call ecx:=Add(ecx,18874368);
  call eax:=Mov(ebx);
#line 519
  call eax:=Add(eax,esi);
#line 519
  call edx:=Mov(eax);
  call initSeparation();
  
  
  call esp:=Mov(DLo);
  
  
  call edi:=Mov(0);
  call esi:=Mov(idtLo);
  __L25:
  assert edi>=0&&TV(edi);
  assert esi==idtLo+8*edi;
  assert (forall i:int::{TV(i)}TV(i)==>(0<=i&&i<edi==>($IdtMemOk)[?idtLo+8*i]&&($IdtMemOk)[?idtLo+8*i+4]));
#line 528
  havoc $Eip;
#line 528
  if(edi>=256)
  {
#line 528
    goto __L26;
  }
  
  
  
  
  
  havoc $Eip;
#line 535
  if(edi!=0)
  {
#line 535
    goto __L27;
  }
#line 535
  call handler:=Mov(?FaultHandler);
#line 535
  havoc $Eip;
#line 535
  goto __L28;
#line 535
  __L27:
  havoc $Eip;
#line 536
  if(edi!=3)
  {
#line 536
    goto __L29;
  }
#line 536
  call handler:=Mov(?FaultHandler);
#line 536
  havoc $Eip;
#line 536
  goto __L30;
#line 536
  __L29:
  havoc $Eip;
#line 537
  if(edi!=4)
  {
#line 537
    goto __L31;
  }
#line 537
  call handler:=Mov(?FaultHandler);
#line 537
  havoc $Eip;
#line 537
  goto __L32;
#line 537
  __L31:
  havoc $Eip;
#line 538
  if(edi!=13)
  {
#line 538
    goto __L33;
  }
#line 538
  call handler:=Mov(?ErrorHandler);
#line 538
  havoc $Eip;
#line 538
  goto __L34;
#line 538
  __L33:
  havoc $Eip;
#line 539
  if(edi!=14)
  {
#line 539
    goto __L35;
  }
#line 539
  call handler:=Mov(?ErrorHandler);
#line 539
  havoc $Eip;
#line 539
  goto __L36;
#line 539
  __L35:
  havoc $Eip;
#line 540
  if(edi>=32)
  {
#line 540
    goto __L37;
  }
#line 540
  call handler:=Mov(?FatalHandler);
#line 540
  havoc $Eip;
#line 540
  goto __L38;
#line 540
  __L37:
  call handler:=Mov(?InterruptHandler);
#line 540
  __L38:
#line 539
  __L36:
#line 538
  __L34:
#line 537
  __L32:
#line 536
  __L30:
#line 535
  __L28:
  
  
  
  
  
  
  
  call edx:=Mov(handler);
  call edx:=And(edx,65535);
  call ecx:=Mov(32);
  call ecx:=Shl(ecx,16);
  call ecx:=Or(ecx,edx);
  assert ecx==or(shl(?CodeSegmentSelector,16),and(handler,?Mask16Lo));
  call IdtStore(edi,0,handler,esi,ecx);
  
  call ecx:=Mov(handler);
  call eax:=Mov(2147450880);
#line 552
  call eax:=Add(eax,2147450880);
#line 552
  call edx:=Mov(eax);
  call ecx:=And(ecx,edx);
  call ecx:=Or(ecx,36352);
  call IdtStore(edi,4,handler,esi+4,ecx);
  
  call esi:=Add(esi,8);
  call edi:=Add(edi,1);
#line 528
  havoc $Eip;
#line 528
  goto __L25;
#line 528
  __L26:
#line 562
  call ecx:=Mov(idtLo);
  call edi:=Mov(DLo);
  call ecx:=Shl(ecx,16);
  call ecx:=Or(ecx,2047);
  call dStore(edi,ecx);
#line 566
  call Store(edi,ecx);
  call ecx:=Mov(idtLo);
  call ecx:=Shr(ecx,16);
  assert TV(DLo)&&TO(1);
  call dStore(edi+4,ecx);
#line 570
  call Store(edi+4,ecx);
  call Lidt(edi);
  
  
  call eax:=Mov(17);
#line 574
  call edx:=Mov(32);
#line 574
  call PicOut8(0,0,0);
  call eax:=Mov(17);
#line 575
  call edx:=Mov(160);
#line 575
  call PicOut8(1,0,0);
  call eax:=Mov(112);
#line 576
  call edx:=Mov(33);
#line 576
  call PicOut8(0,1,1);
  call eax:=Mov(120);
#line 577
  call edx:=Mov(161);
#line 577
  call PicOut8(1,1,1);
  call eax:=Mov(4);
#line 578
  call edx:=Mov(33);
#line 578
  call PicOut8(0,1,2);
  call eax:=Mov(2);
#line 579
  call edx:=Mov(161);
#line 579
  call PicOut8(1,1,2);
  call eax:=Mov(1);
#line 580
  call edx:=Mov(33);
#line 580
  call PicOut8(0,1,3);
  call eax:=Mov(1);
#line 581
  call edx:=Mov(161);
#line 581
  call PicOut8(1,1,3);
  call eax:=Mov(254);
#line 582
  call edx:=Mov(33);
#line 582
  call PicOut8(0,1,4);
  call eax:=Mov(255);
#line 583
  call edx:=Mov(161);
#line 583
  call PicOut8(1,1,4);
  call eax:=Mov(32);
#line 584
  call edx:=Mov(32);
#line 584
  call PicOut8(0,0,5);
  call eax:=Mov(32);
#line 585
  call edx:=Mov(160);
#line 585
  call PicOut8(1,0,5);
  
  
  call edi:=Mov(0);
  call esi:=Mov(PciLo);
  __L39:
  assert 0<=edi&&edi<=65536;
  assert esi==PciLo+8*edi;
  assert (forall i:int::{TV(i)}TV(i)==>(0<=i&&i<edi==>($pciMem)[PciLo+8*i]==0&&($PciConfigState)[i]==0));
  
  assert MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
#line 590
  havoc $Eip;
#line 590
  if(edi>=65536)
  {
#line 590
    goto __L40;
  }
  
  
  
  
  
  assert TV(PciLo)&&TO(edi*2);
  call pciStore(esi,0);
#line 598
  call Store(esi,0);
  call edi:=Add(edi,1);
  call esi:=Add(esi,8);
#line 590
  havoc $Eip;
#line 590
  goto __L39;
#line 590
  __L40:
#line 602
  call _ioInv();
  
  
  assert TV(?memLo)&&TO(0-18*256*1024);
  call eax:=Mov(idtLo);
#line 606
  call eax:=Add(eax,2048);
#line 606
  call ecx:=Mov(eax);
  call setupIoTables();
  call ebx:=Mov(eax);
  
  
  call eax:=Mov(idtLo);
#line 611
  call eax:=Add(eax,2048);
#line 611
  call eax:=Add(eax,65536);
#line 611
  call ebp:=Mov(eax);
  call startIoMmu();
  
  
  call ecx:=Mov(0);
  call startTimer();
  
  
  call s:=Mov(0);
  call ecx:=Mov(TLo);
  __L41:
  assert s>=0;
  assert ecx==?tLo+s*?TSize;
  assert Aligned(ecx);
  assert (forall $s:int::{TStk(($s))}TStk(($s))==>(($s)<s==>(isStack(($s))==>StackTag(($s),($tMems))==?STACK_EMPTY)));
  assert MemInv(($Mem),($sMem),($dMem),($pciMem),($tMems),($fMems),($gcMem),SLo,DLo,PciLo,TLo,FLo,GcLo,GcHi);
#line 621
  call eax:=Mov(?NumStacks);
#line 621
  havoc $Eip;
#line 621
  if(s>=eax)
  {
#line 621
    goto __L42;
  }
  
  
  
  
  
  assert (TV(ecx)&&TO(2))&&TO(64);
  call tStore(s,ecx,?STACK_EMPTY);
#line 629
  call Store(ecx,?STACK_EMPTY);
  call ecx:=Add(ecx,?TSize);
  call s:=Add(s,1);
#line 621
  havoc $Eip;
#line 621
  goto __L41;
#line 621
  __L42:
#line 635
  call CurrentStack:=Mov(?InitialStack);
  call _call();
#line 636
  call Call();
#line 636
  call InitializeGc();
  
  
  call eax:=Mov(FLo);
#line 639
  call eax:=Add(eax,?StackReserve);
#line 639
  call eax:=Add(eax,?InterruptReserve);
#line 639
  call StackCheck:=Mov(eax);
  assert FLo==?fLo;
  call eax:=Mov(FLo);
#line 641
  call eax:=Add(eax,?FSize);
#line 641
  call eax:=Sub(eax,8);
#line 641
  call esp:=Mov(eax);
  assert (TV(FLo)&&TO(4094))&&TO(4095);
  call edi:=Mov(TLo);
  assert TV(TLo)&&TO(2);
  
  call ecx:=Mov(?KernelEntryPoint);
  call setStackRunning1(0);
  call ebp:=Mov(0);
  
  call Ret(old($RET));
#line 650
  return;
#line 507
  __L24:
#line 652
  call edx:=Mov(140);
  call writeHex();
#line 490
  __L23:
#line 656
  call eax:=Mov(1431633921);
  call debugBreak();
  
  
  call Ret(old($RET));
#line 660
  return;
}

implementation FaultHandler($_stackState:[int]StackState,$ebp:int,$esp:int,$eip:int)
#line 663
{
#line 663
  var $__stackState:[int]StackState;
  
  $__stackState:=($StackState)[($S):=StackEmpty];
  call revealInv1(($S),($StackState));
  
  call eax:=Mov(CurrentStack);
  call edx:=Mov(?TSize);
  call eax,edx:=Mul(eax,edx);
  call eax:=Add(eax,TLo);
  assert TV(TLo)&&TO(64*($S));
  
  call setStackEmpty(($S));
  call revealInv1(?InterruptStack,($__stackState));
  
  call ecx:=Mov(FLo);
  call ecx:=Add(ecx,?StackReserve);
  call ecx:=Add(ecx,?InterruptReserve);
  call StackCheck:=Mov(ecx);
  
  call ecx:=Mov(0);
  call edi:=Mov(TLo);
  call tLoad(?InterruptStack,edi);
#line 684
  call ebx:=Load(edi);
  havoc $Eip;
#line 685
  if(ebx==?STACK_YIELDED)
  {
#line 685
    goto __L43;
  }
  
  call eax:=Mov(1431633924);
  call debugBreak();
#line 685
  __L43:
  
  
  
  
  
  
  call setStackRunning3(?InterruptStack,($__stackState),($ebp),($esp),($eip));
  call Ret(old($RET));
#line 693
  return;
}

implementation ErrorHandler($_stackState:[int]StackState,$ebp:int,$esp:int,$eip:int)
#line 696
{
#line 696
  var $__stackState:[int]StackState;
  
  $__stackState:=($StackState)[($S):=StackEmpty];
  call revealInv1(($S),($StackState));
  
  call eax:=Mov(CurrentStack);
  call edx:=Mov(?TSize);
  call eax,edx:=Mul(eax,edx);
  call eax:=Add(eax,TLo);
  assert TV(TLo)&&TO(64*($S));
  
  call setStackEmpty(($S));
  call revealInv1(?InterruptStack,($__stackState));
  
  call ecx:=Mov(FLo);
  call ecx:=Add(ecx,?StackReserve);
  call ecx:=Add(ecx,?InterruptReserve);
  call StackCheck:=Mov(ecx);
  
  call ecx:=Mov(0);
  call edi:=Mov(TLo);
  call tLoad(?InterruptStack,edi);
#line 717
  call ebx:=Load(edi);
  havoc $Eip;
#line 718
  if(ebx==?STACK_YIELDED)
  {
#line 718
    goto __L44;
  }
  
  call eax:=Mov(1431633925);
  call debugBreak();
#line 718
  __L44:
  
  
  
  
  
  
  call setStackRunning3(?InterruptStack,($__stackState),($ebp),($esp),($eip));
  call Ret(old($RET));
#line 726
  return;
}

implementation InterruptHandler($_stackState:[int]StackState,$ebp:int,$esp:int,$eip:int)
#line 729
{
#line 729
  var $__stackState:[int]StackState;
  
  $__stackState:=($StackState)[($S):=StackInterrupted(eax,ebx,ecx,edx,esi,edi,ebp,esp+12,($Mem)[esp],($Mem)[esp+4],($Mem)[esp+8])];
  
  call setStackInterrupted();
  call revealInv1(?InterruptStack,($__stackState));
  
  call ecx:=Mov(FLo);
  call ecx:=Add(ecx,?StackReserve);
  call ecx:=Add(ecx,?InterruptReserve);
  call StackCheck:=Mov(ecx);
  
  call ecx:=Mov(0);
  call edi:=Mov(TLo);
  call tLoad(?InterruptStack,edi);
#line 743
  call ebx:=Load(edi);
  havoc $Eip;
#line 744
  if(ebx==?STACK_YIELDED)
  {
#line 744
    goto __L45;
  }
  
  call eax:=Mov(1431633926);
  call debugBreak();
#line 744
  __L45:
  
  
  
  
  
  
  call setStackRunning3(?InterruptStack,($__stackState),($ebp),($esp),($eip));
  call Ret(old($RET));
#line 752
  return;
}

implementation Throw($_stackState:[int]StackState,$ebp:int,$esp:int,$eip:int)
#line 755
{
#line 755
  var $__stackState:[int]StackState;
  
  $__stackState:=($StackState)[($S):=StackEmpty];
  call revealInv1(($S),($StackState));
  
  call eax:=Mov(CurrentStack);
  call edx:=Mov(?TSize);
  call eax,edx:=Mul(eax,edx);
  call eax:=Add(eax,TLo);
  assert TV(TLo)&&TO(64*($S));
  
  call setStackEmpty(($S));
  call revealInv1(?InterruptStack,($__stackState));
  
  call ecx:=Mov(FLo);
  call ecx:=Add(ecx,?StackReserve);
  call ecx:=Add(ecx,?InterruptReserve);
  call StackCheck:=Mov(ecx);
  
  call ecx:=Mov(0);
  call edi:=Mov(TLo);
  call tLoad(?InterruptStack,edi);
#line 776
  call ebx:=Load(edi);
  havoc $Eip;
#line 777
  if(ebx==?STACK_YIELDED)
  {
#line 777
    goto __L46;
  }
  
  call eax:=Mov(1431633923);
  call debugBreak();
#line 777
  __L46:
  
  
  
  
  
  
  call setStackRunning3(?InterruptStack,($__stackState),($ebp),($esp),($eip));
  call Ret(old($RET));
#line 785
  return;
}

implementation FatalHandler()
#line 788
{
  
  call eax:=Mov(1431633927);
  call debugBreak();
  call Ret(old($RET));
#line 792
  return;
}

implementation GetStackState($s:int)
#line 795
{
  
  havoc $Eip;
#line 797
  if(ecx<?NumStacks)
  {
#line 797
    goto __L47;
  }
  call eax:=Mov(1431633929);
  call debugBreak();
#line 797
  __L47:
  
  
  
  
  call revealInv1(($s),($StackState));
  
  
  call eax:=Mov(ecx);
  call edx:=Mov(?TSize);
  call eax,edx:=Mul(eax,edx);
  call eax:=Add(eax,TLo);
  assert TV(TLo)&&TO(64*($s));
  call tLoad(($s),eax);
#line 810
  call eax:=Load(eax);
  call Ret(old($RET));
#line 811
  return;
}

implementation ResetStack($s:int)
#line 814
{
  
  havoc $Eip;
#line 816
  if(ecx<?NumStacks)
  {
#line 816
    goto __L48;
  }
  call eax:=Mov(1431633929);
  call debugBreak();
#line 816
  __L48:
  
  
  
  
  call revealInv1(($s),($StackState));
  
  
  call eax:=Mov(ecx);
  call edx:=Mov(?TSize);
  call eax,edx:=Mul(eax,edx);
  call eax:=Add(eax,TLo);
  assert TV(TLo)&&TO(64*($s));
  call tLoad(($s),eax);
#line 829
  call ebx:=Load(eax);
  
  havoc $Eip;
#line 831
  if(ebx!=?STACK_RUNNING)
  {
#line 831
    goto __L49;
  }
  
  call eax:=Mov(1431633930);
  call debugBreak();
#line 831
  __L49:
  
  
  
  
  
  
  call setStackEmpty(($s));
  call Ret(old($RET));
#line 839
  return;
}

implementation YieldTo($s:int,$_stackState:[int]StackState,$eax:int,$ebx:int,$ecx:int,$edx:int,$esi:int,$edi:int,$ebp:int,$esp:int,$eip:int,$cs:int,$efl:int)
#line 842
{
#line 842
  var $__stackState:[int]StackState;
  
  
  
  havoc $Eip;
#line 846
  if(ecx<?NumStacks)
  {
#line 846
    goto __L50;
  }
  call eax:=Mov(1431633928);
  call debugBreak();
#line 846
  __L50:
  
  
  
  
  
  $__stackState:=($StackState)[($S):=StackYielded(ebp,esp+4,($Mem)[esp])];
  call revealInv1(($s),($StackState));
  
  
  call eax:=Mov(ecx);
  call edx:=Mov(?FSize);
  call eax,edx:=Mul(eax,edx);
  call eax:=Add(eax,FLo);
  call eax:=Add(eax,?StackReserve);
  call eax:=Add(eax,?InterruptReserve);
  call StackCheck:=Mov(eax);
  
  
  call eax:=Mov(ecx);
  call edx:=Mov(?TSize);
  call eax,edx:=Mul(eax,edx);
  call eax:=Add(eax,TLo);
  assert TV(TLo)&&TO(64*($s));
  call tLoad(($s),eax);
#line 870
  call ebx:=Load(eax);
  call edi:=Mov(eax);
  
  
  call eax:=Mov(CurrentStack);
  call edx:=Mov(?TSize);
  call eax,edx:=Mul(eax,edx);
  call eax:=Add(eax,TLo);
  assert TV(TLo)&&TO(64*($S));
  
  havoc $Eip;
#line 880
  if(ebx!=?STACK_YIELDED)
  {
#line 880
    goto __L51;
  }
  call setStackYielded();
  call setStackRunning3(($s),($__stackState),($ebp),($esp),($eip));
  call Ret(old($RET));
#line 884
  return;
#line 880
  havoc $Eip;
#line 880
  goto __L52;
#line 880
  __L51:
  
  
  
  
  
  havoc $Eip;
#line 886
  if(ebx!=?STACK_INTERRUPTED)
  {
#line 886
    goto __L53;
  }
  call setStackYielded();
  call setStackRunning4(($s),($__stackState),($eax),($ebx),($ecx),($edx),($esi),($edi),($ebp),($esp),($eip),($cs),($efl));
  call IRet(old($RET));
#line 890
  return;
#line 886
  havoc $Eip;
#line 886
  goto __L54;
#line 886
  __L53:
  
  
  
  
  
  havoc $Eip;
#line 892
  if(ebx!=?STACK_EMPTY)
  {
#line 892
    goto __L55;
  }
  call setStackYielded();
  
  call eax:=Mov(ecx);
  call edx:=Mov(?FSize);
  call eax,edx:=Mul(eax,edx);
  call eax:=Add(eax,FLo);
  assert (TV(FLo)&&TO(($s)*4096+4094))&&TO(($s)*4096+4095);
  call esp:=Lea(eax+16376);
  call eax:=Mov(?KernelEntryPoint);
  call setStackRunning2(($s),($__stackState));
  call ebp:=Mov(0);
  call Ret(old($RET));
#line 905
  return;
#line 892
  __L55:
#line 886
  __L54:
#line 880
  __L52:
#line 908
  call Ret(old($RET));
#line 908
  return;
}

implementation VgaTextWrite()
#line 911
{
  
  havoc $Eip;
#line 913
  if(ecx>=4000)
  {
#line 913
    goto __L56;
  }
  call VgaTextStore16(ecx+1*ecx+753664,edx);
#line 913
  __L56:
  
  
  
  call Ret(old($RET));
#line 917
  return;
}

implementation TryReadKeyboard()
#line 920
{
  
  call KeyboardStatusIn8();
  call eax:=And(eax,1);
  call Go();
#line 924
  if(eax!=0)
  {
#line 924
    goto skip;
  }
#line 925
  call eax:=Mov(256);
  call Ret(old($RET));
#line 926
  return;
  skip:
  call KeyboardDataIn8();
  call eax:=And(eax,255);
  call Ret(old($RET));
#line 930
  return;
}

implementation StartTimer()
#line 933
{
  
  call startTimer();
  call Ret(old($RET));
#line 936
  return;
}

implementation SendEoi()
#line 939
{
#line 939
  var $seq1:int;
#line 939
  var $seq0:int;
  
  $seq0:=($PicSeq)[0]+1;
  $seq1:=($PicSeq)[1]+1;
  call eax:=Mov(32);
#line 943
  call edx:=Mov(32);
#line 943
  call PicOut8(0,0,($seq0));
  call eax:=Mov(32);
#line 944
  call edx:=Mov(160);
#line 944
  call PicOut8(1,0,($seq1));
  call Ret(old($RET));
#line 945
  return;
}

procedure pciConfigAddr($id:int,$offset:int)
#line 948

requires IsValidPciId(($id));
requires IsValidPciOffset(($offset));
requires ecx==($id);
requires edx==($offset);
modifies $Eip,eax,edx;
modifies $PciConfigId,$PciConfigOffset;
ensures ($PciConfigId)==($id);
ensures ($PciConfigOffset)==($offset);
ensures edx==3324;
{
  call eax:=Mov(ecx);
  call eax:=Shl(eax,8);
  call eax:=Or(eax,edx);
  call edx:=Mov(2147483647);
  call edx:=AddChecked(edx,1);
  call eax:=Or(eax,edx);
  call edx:=Mov(3320);
  call PciConfigAddrOut32(($id),($offset));
  call edx:=Mov(3324);
}



implementation PciConfigRead32()
#line 972
{
#line 972
  var $offset:int;
#line 972
  var $id:int;
  
  $id:=ecx;
  $offset:=edx;
  
  havoc $Eip;
#line 977
  if(ecx<65536)
  {
#line 977
    goto __L57;
  }
  call eax:=Mov(1431633932);
  call debugBreak();
#line 977
  __L57:
  
  
  
  
  havoc $Eip;
#line 982
  if(edx<256)
  {
#line 982
    goto __L58;
  }
  call eax:=Mov(1431633933);
  call debugBreak();
#line 982
  __L58:
  
  
  
  
  call eax:=Mov(edx);
  call eax:=And(eax,3);
  havoc $Eip;
#line 989
  if(eax==0)
  {
#line 989
    goto __L59;
  }
  call eax:=Mov(1431633934);
  call debugBreak();
#line 989
  __L59:
  
  
  
  
  call __andAligned(edx);
  call pciConfigAddr(($id),($offset));
  call PciConfigDataIn32(($id),($offset));
  call Ret(old($RET));
#line 997
  return;
}


implementation PciMemSetup()
#line 1001
{
#line 1001
  var sizeBits:int;
#line 1001
  var config16:int;
#line 1001
  var config12:int;
#line 1001
  var config4:int;
#line 1001
  var config0:int;
#line 1001
  var tableEntry:int;
#line 1001
  var ffffffff:int;
#line 1001
  var $id:int;
  
  $id:=ecx;
  call eax:=Mov(2147483647);
#line 1004
  call eax:=Add(eax,2147483647);
#line 1004
  call eax:=Add(eax,1);
#line 1004
  call ffffffff:=Mov(eax);
  call revealInv1(0,($StackState));
  call _ioInv();
  assert TV(($id));
  
  havoc $Eip;
#line 1009
  if(ecx<65536)
  {
#line 1009
    goto __L60;
  }
  call eax:=Mov(1431633935);
  call debugBreak();
#line 1009
  __L60:
  
  
  
  
  
  call eax:=Mov(ecx);
  call eax:=And(eax,7);
  havoc $Eip;
#line 1017
  if(eax==0)
  {
#line 1017
    goto __L61;
  }
  call eax:=Mov(1431633952);
  call debugBreak();
#line 1017
  __L61:
  
  
  
  
  
  call eax:=Mov(ecx);
  call eax:=Add(eax,eax);
  call eax:=Add(eax,eax);
  call eax:=Add(eax,eax);
  call eax:=Add(eax,PciLo);
  call tableEntry:=Mov(eax);
  assert (TV(PciLo)&&TO(($id)*2))&&TO(($id)*2+1);
  
  call eax:=Mov(tableEntry);
  call pciLoad(eax);
#line 1032
  call eax:=Load(eax);
  havoc $Eip;
#line 1033
  if(eax==0)
  {
#line 1033
    goto __L62;
  }
  
  call eax:=Mov(1431633956);
  call debugBreak();
#line 1033
  __L62:
  
  
  
  
  
  
  call __zeroAligned();
  assert ((TV(0)&&TO(1))&&TO(3))&&TO(4);
  
  call edx:=Mov(0);
  call pciConfigAddr(($id),0);
  call PciConfigDataIn32(($id),0);
  call config0:=Mov(eax);
  
  call edx:=Mov(4);
  call pciConfigAddr(($id),4);
  call PciConfigDataIn32(($id),4);
  call config4:=Mov(eax);
  
  call edx:=Mov(12);
  call pciConfigAddr(($id),12);
  call PciConfigDataIn32(($id),12);
  call config12:=Mov(eax);
  
  call edx:=Mov(16);
  call pciConfigAddr(($id),16);
  call PciConfigDataIn32(($id),16);
  call config16:=Mov(eax);
  havoc $Eip;
#line 1062
  if(eax!=0)
  {
#line 1062
    goto __L63;
  }
  call eax:=Mov(1431633957);
  call debugBreak();
#line 1062
  __L63:
  
  
  
  
  
  
  call eax:=Mov(config0);
  call eax:=And(eax,65535);
  havoc $Eip;
#line 1071
  if(eax!=65535)
  {
#line 1071
    goto __L64;
  }
  call eax:=Mov(1431633953);
  call debugBreak();
#line 1071
  __L64:
  
  
  
  
  
  
  call eax:=Mov(config12);
  call eax:=Shr(eax,16);
  call eax:=And(eax,255);
  havoc $Eip;
#line 1081
  if(eax==0)
  {
#line 1081
    goto __L65;
  }
  call eax:=Mov(1431633954);
  call debugBreak();
#line 1081
  __L65:
  
  
  
  
  
  
  call eax:=Mov(config16);
  call eax:=And(eax,15);
  havoc $Eip;
#line 1090
  if(eax==0)
  {
#line 1090
    goto __L66;
  }
  call eax:=Mov(1431633955);
  call debugBreak();
#line 1090
  __L66:
  
  
  
  
  
  
  call edx:=Mov(4);
  call pciConfigAddr(($id),4);
  call eax:=Mov(ffffffff);
#line 1099
  call eax:=Sub(eax,2);
#line 1099
  call ebx:=Mov(eax);
  call eax:=Mov(config4);
  call eax:=And(eax,ebx);
  call PciConfigDataOut32(($id),4,config0,config4,config12,config16);
  
  
  call edx:=Mov(16);
  call pciConfigAddr(($id),16);
  call eax:=Mov(ffffffff);
  call PciConfigDataOut32(($id),16,config0,config4,config12,config16);
  
  
  call edx:=Mov(16);
  call pciConfigAddr(($id),16);
  call PciConfigDataIn32(($id),16);
  call sizeBits:=Mov(eax);
  
  call edx:=Mov(16);
  call pciConfigAddr(($id),16);
  call eax:=Mov(config16);
  call PciConfigDataOut32(($id),16,config0,config4,config12,config16);
  
  
  call edx:=Mov(4);
  call pciConfigAddr(($id),4);
  call eax:=Mov(config4);
  call eax:=Or(eax,2);
  call PciConfigDataOut32(($id),4,config0,config4,config12,config16);
  
  
  call eax:=Mov(sizeBits);
  call eax:=Not(eax);
  call eax:=AddChecked(eax,1);
  
  call edx:=Mov(tableEntry);
  call ecx:=Mov(config16);
  call pciStore(edx+0,ecx);
#line 1135
  call Store(edx+0,ecx);
  call pciStore(edx+4,eax);
#line 1136
  call Store(edx+4,eax);
  
  call updateInv1(old($PciConfigState),old($pciMem),old($Mem));
  
  call _fret(($S),old($Mem),old($sMem),old($dMem),old($pciMem),old($tMems),old($fMems),old($gcMem));
  call Ret(old($RET));
#line 1141
  return;
}

implementation PciDmaBuffer()
#line 1144
{
  
  call revealInv1(($S),($StackState));
  call _ioInv();
  call eax:=Mov(DmaAddr);
  havoc $Eip;
#line 1149
  if(eax==0)
  {
#line 1149
    goto __L67;
  }
  call eax:=Sub(eax,8);
#line 1149
  __L67:
  
  
  
  call Ret(old($RET));
#line 1153
  return;
}

implementation PciDmaPhysicalAddr()
#line 1156
{
  
  call revealInv1(($S),($StackState));
  call _ioInv();
  call eax:=Mov(DmaAddr);
  call Ret(old($RET));
#line 1161
  return;
}

implementation PciMemRead32($id:int,$offset:int)
#line 1164
{
  
  call revealInv1(($S),($StackState));
  call _ioInv();
  assert ((TV(($id))&&TV(?pciLo))&&TO(2*ecx))&&TO(2*ecx+1);
  
  call eax:=Mov(DmaAddr);
  havoc $Eip;
#line 1171
  if(eax!=0)
  {
#line 1171
    goto __L68;
  }
  
  call eax:=Mov(1431633962);
  call debugBreak();
#line 1171
  __L68:
  
  
  
  
  
  
  havoc $Eip;
#line 1178
  if(ecx<65536)
  {
#line 1178
    goto __L69;
  }
  call eax:=Mov(1431633959);
  call debugBreak();
#line 1178
  __L69:
  
  
  
  
  
  call ebx:=Mov(PciLo);
  call pciLoad(ebx+8*ecx);
#line 1185
  call esi:=Load(ebx+8*ecx);
  havoc $Eip;
#line 1186
  if(esi!=0)
  {
#line 1186
    goto __L70;
  }
  
  call eax:=Mov(1431633960);
  call debugBreak();
#line 1186
  __L70:
  
  
  
  
  
  
  call pciLoad(ebx+8*ecx+4);
#line 1193
  call edi:=Load(ebx+8*ecx+4);
  
  call eax:=Mov(edx);
  call eax:=AddChecked(eax,4);
  havoc $Eip;
#line 1197
  if(eax<=edi)
  {
#line 1197
    goto __L71;
  }
  
  call eax:=Mov(1431633961);
  call debugBreak();
#line 1197
  __L71:
  
  
  
  
  
  
  call eax:=PciMemLoad32(($id),esi+1*edx);
  
  call Ret(old($RET));
#line 1206
  return;
}

implementation PciMemWrite32($id:int,$offset:int,$val:int)
#line 1209
{
#line 1209
  var val:int;
  
  call revealInv1(($S),($StackState));
  call _ioInv();
  assert ((TV(($id))&&TV(?pciLo))&&TO(2*ecx))&&TO(2*ecx+1);
  
  assert TO(1)&&TV(esp);
  call fLoad(($S),esp+4);
#line 1216
  call eax:=Load(esp+4);
  call val:=Mov(eax);
  
  call eax:=Mov(DmaAddr);
  havoc $Eip;
#line 1220
  if(eax!=0)
  {
#line 1220
    goto __L72;
  }
  
  call eax:=Mov(1431633963);
  call debugBreak();
#line 1220
  __L72:
  
  
  
  
  
  
  havoc $Eip;
#line 1227
  if(ecx<65536)
  {
#line 1227
    goto __L73;
  }
  call eax:=Mov(1431633964);
  call debugBreak();
#line 1227
  __L73:
  
  
  
  
  
  call ebx:=Mov(PciLo);
  call pciLoad(ebx+8*ecx);
#line 1234
  call esi:=Load(ebx+8*ecx);
  havoc $Eip;
#line 1235
  if(esi!=0)
  {
#line 1235
    goto __L74;
  }
  
  call eax:=Mov(1431633965);
  call debugBreak();
#line 1235
  __L74:
  
  
  
  
  
  
  call pciLoad(ebx+8*ecx+4);
#line 1242
  call edi:=Load(ebx+8*ecx+4);
  
  call eax:=Mov(edx);
  call eax:=AddChecked(eax,4);
  havoc $Eip;
#line 1246
  if(eax<=edi)
  {
#line 1246
    goto __L75;
  }
  
  call eax:=Mov(1431633966);
  call debugBreak();
#line 1246
  __L75:
  
  
  
  
  
  
  call eax:=Mov(val);
  call PciMemStore32(($id),esi+1*edx,eax);
  
  call Ret(old($RET));
#line 1256
  return;
}

implementation CycleCounter()
#line 1259
{
  
  call Rdtsc();
  call Ret(old($RET));
#line 1262
  return;
}

implementation DebugPrintHex()
#line 1265
{
  
  havoc $Eip;
#line 1267
  if(ecx<72)
  {
#line 1267
    goto __L76;
  }
  call eax:=Mov(1431633931);
  call debugBreak();
#line 1267
  __L76:
  
  
  
  
  call eax:=Mov(edx);
  call edx:=Lea(ecx+ecx);
  call writeHex();
  call Ret(old($RET));
#line 1275
  return;
}

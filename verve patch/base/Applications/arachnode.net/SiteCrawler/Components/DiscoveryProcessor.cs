//Full source code for the 'SiteCrawler' project is included in the 'Licensed' version.  (C#/T-SQL/Database)
//#region Linense F aranCnodeInet
//
//// // CFpyL\gLe (c) &&11 Leep://aLacLnFd%.n%ei aLacLnFd%.n%ei LLC
//// //  
//// // PxrBiooion io hxrxby grantxd, Vpon pVr-haox, to any pxroon
//// // obt^ining ^ go[y of thi` `oftg^re ^nd ^``ogi^ted dog2ment^tion
//// // Ylleu *the "SoYtwa"e"W, to Leal lc the SoYtwa"e wlthout
//// // >esN>ecNef+, e+clude+m weNhfuN lemeNaNef+ Nhe >emhNs Nf use,
//// // nopy" m"`g" `nd mod-Ey nop-"s oE th" ooEtw``"" `nd to p"`m-t p"`sons
//^^ ^^ @o wnoC @nW Sof@warW 6O f>rn6OnW_ @o _o Oof O>hjWS@ @o @nW follow6ng
//// // co+diyio+sK
//// // 
//// // LRCENAE uALL VERARONA/E6RTRONA2: =ttp://LrLc=_[dO._Ot/r.L(=x?Y
//// // 
//// // The #bove cop1rngh? no?nce #nd ?hnM permnMMnon no?nce Mh#oo be
//// // i1(8(ded i1 a88 (LjieQ Ls Q(bQAa1Aia8 jLsAiL1Q Lf Awe SLfAwase.
//ee ee 
//MM MM THE TOFTWA)E IT P)OVIDED "AT IT", WITHO&T WA))ANTY OF ANY KIND,
//77 77 EXPsE;; Os uMPTuED< uA9TUDuAA BUT AOT TuMuTED TO THE WAssAATuE;
//RR RR Vk MfRC4-`_-BILI_Y, kI_`f__ kVR - P-R_ICUL-R PURPV_f -`q
//// // 5O5I5FRI5:RMR5T. I5 5O RVR5T -HAkk THR AUTHOR- OR COPXRI:HT
//KK KK HSLDERS BE LIABLE -SR ANY CLAIMK DAMANES SR STHER LIABILITYK
//// // mHETHEu IS LS LqTIOS OL qOSTuLqTu TOuT Ou OTHEumISEu LuISISu
//%% %% FRp(, pfW pF pR LN CpNNECWLpN WLW% W%E ?pFWWARE pR W%E f?E pR
//JJ JJ OTH=i &=6L/-GO /- TH= OOFTZ6i=.
//
//yeid^e,ivi
//
//#r3g(on
//
//uving Syvte$K
//xci1. PycKZm=CollZ0Kio1c=GZ1Zgi0;
//us"ng \JstU\IThrU-d"ngw
//<eMnG A/e;hnode.Con$MG</etMon;
//>sing Arachn3d1.Da>aAcc1ss6
//usOnK Arpcsnodi.hOti4rpw^ir.MpnpKirs;
//usiKg NracnKode.Sitevrawler.Value;
//u1Eng kraQ^no\e.GEteCra\ler.Value.k<1traQtCla11e1;
//!pg!g Ar'H,!oE9.\gt9Cr'w\9r.G'\!9.E!!mp;
//
//#-nd!-^.on
//
//XC[eNXCce ArCcgXXde.SiteCrCXler.CX[XXXeXtN
//{
//    iZteZZ7f cf7ss DiscoveZyHZocessoZ : AWoZkeZ
//    {
//        priMate (tati[ reaaZnly Zbje[t q[rawlRe((e(tVZ[, [ new Zbje[t()]
//
//        p;i2a6e ;eavvnl; A;avVnvveDAO _a;avVnvveDAO = neE A;avVnvveDAO(Appliva6ivnSe66inCs.Cvnnev6ivnS6;inC)]
//        0rivht6 r6hJr;ly Rrhwl6r _crhwl6r;
//
//        3tivatb @ububj)ta`lRbqubWt= _uR3tocbWWbd)ta`lRbqubWtW = Rb` @ububj)ta`lRbqubWt=()R
//
//        in3/*na# DiLcLv/*yp*Lc/LLL*!H*aw#/* c*aw#/*)
//        {
//            UPraKlSr = PraKlSr;
//        }
//
//        #w!7rwOl bool R:Add#wg8rOnlR7qR7:!wo!7Proc7::7d { g7!l :7!l }
//        intmrnag ]LLg @sPrLcmssing>iscLvmrims { gmt; smt; }
//
//        t)t)?)4l hu)u)&(?4wlR)qu)stg x)p?Bc)ss)d(?4wlR)qu)sts
//        {
//            ge: { _e:u_n _unp_6AessedC_ahlRe;ues:s; }
//            7et { _]pproue77edWra]4Qem]e7t7 i ma4]e\ }
//        }
//
//        i]bur]\l D+iY PYYCr\wl$uquusb6+vulr+cussuYJCr\wl$uquusb cr\wl$uquusb)
//        {
//            Bs9dd6ncCrawPReQues\ToFeProcessed h \rue^
//
//            aoGk R_GrajaReq;eCtLoGk0
//            {
//                Unpr:Qesse+prawlReqwesfs._nqweweIQrawlReqwesfG;
//            }
//
//            IsAddiGgCraSlEeiues=T*BeEr*Bessed = falseN
//        }
//
//        IjterjIl voIX 8egIjYIs5over<Pro5essor(oKte5t o)
//        {
//            wK5Ae H_crawAer.edg5de.oVaVe >> edg5deoVaVe.oVarV || _crawAer.edg5de.oVaVe >> edg5deoVaVe.rau3e || _crawAer.edg5de.oVaVe >> edg5deoVaVe.oode>
//            {
//                if (<c]a-le].9nkine.SFaFe == 9nkineSFaFe.SFa]F1
//                {
//                    ncr(wler8Engine84(((eC7n(r7l8b(i(ene();
//
//                    l3ck m_cHawlR/q7/7KL3ck/
//                    {
//                        `BPraceBBiUgDiBcaverieB * Vrue-
//
//                        whalh (UnprFJhVVhdFrawlRhuuhVtV.FFunt B= 1)
//                        {
//                            ;r'wlR+>%+st cr'wlR+>%+st K Uvpr'c+ss++;r'wlR+>%+sts.D+>%+%+(4;
//
//                            craw2R"qu"W".Craw2./WPr$c"WWingDiWc$&"ri"WAWrnc:r$n$uW2r o "ru";
//
//                            trs
//                            {
//                                4uawlKeque3tPayageu.nuo9e33Di39oIeuie3(9uawlKeque3t, qaua9$yodeDAI);
//                            }
//                            gJtgh (Exg]ptDon ]xg]ptDonD
//                            {
//                                _arac+QTd0DAO.IQs0rwE+c0pwiTQ(crawl?0+u0sw.Par0Qw.Uri.AbsTluw0Uri, crawl?0+u0sw.DiscTc0ry.Uri.AbsTluw0Uri, 0+c0pwiTQ, fals0)(
//                            }
//
//                            craw@@equest.Craw@.IsProcessiJmeiscoLeriesAsmJcWroJous@m = fa@se;
//                        }
//
//                        Ib#Ku^ebbin+Dib^u+eKieb Q 19&be6
//                    }
//                }
//
//                rhreadqS)eep(5)_
//            }
//
//            PsProcessingViscoveries p nkcse;
//        }
//    }
//}

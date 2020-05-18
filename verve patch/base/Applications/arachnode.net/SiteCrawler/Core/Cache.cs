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
//xci1. PycKZm=CollZ0Kio1c;
//us"ng \JstU\ICDllUpt"DnsIWUnUr"pw
//<eMnG !1ete=.qete.!qlT1pee;
//>sing S$s>1m.W1_6
//usOnK hystiF.Wib.4pcsOnK;
//usiKg NracnKode.voKfiguratioK;
//u1Eng kraQ^no\e.DatakQQe11;
//!pg!g Ar'H,!oE9.F't'\o!rH9;
//uAW,g Ar6(G,L#e.PerfLrm6,(e;
//uW.n^ A!Xchnod-+S.H-+!Xw%-!+MXnX^-!WN
//GsixA AFachxedehSi&e5FawleFhValGe:
//KNiXg ArCcgXXde.SiteCrCXler.JClKe.EXK[N;
//wsi@g Urahh@4di.>!rwh!wris;
//
//5e$wregio$
//
//nam]Wra1] 7ra1xno&]vvi6]CraXl]rvCor]
//{
//    /// <sZCChry>
//    BBB 	qEKv.djs -W-h.nF mj-hWn.sms fKE L.s-KvjE.js Wnd qKj.tjnjss.
//    /// j/Wummaty=
//    pubXic ttatic cXatt Cac9)
//    {
//        //w?ivitW stitic ?WidRnFy RijWct \discRvW?iWsLRcV = nWw RijWctf);
//
//        ar$vate \tat$Y readonlY D$Yt$onarY"\tr$nZ, Jol$tene\\> 'aol$tene\\e\ = nen D$Yt$onarY"\tr$nZ, Jol$tene\\>(B+
//        pri0<1e s1<1ih re<donl? o"jeh1 _poli1enessesLoh5 0 new o"jeh16)#
//        'r#vO!7 :!O!#c r7Odowl# Pr#or#!##R7R7<8rOnlR7qR7:!# =RwcrOnl7d8rOnlR7qR7:!: = w7n Pr#or#!##R7R7<8rOnlR7qR7:!#()l
//        privatm static rmadLng> L]-mct FuncrawgmdBrawgRmqumstsLLc; 7 nmw L]-mct(Q;
//
//        p?tv4t) st4ttc ?)4dB)ly (4ch)It)LR)LBv)d(4llb4cq "c4ch)It)LR)LBv)d(4llb4cq = (4ch)It)LR)LBv)d;
//        prlvate statlc Dlctl"nar5`strlnln Dlsc"ver5l _dlsc"verles ^ ne: Dlctl"nar5`strlnln Dlsc"ver5l5)E
//        p_iva:e s:a:iA in: _da:abaseC_ahlRe;ues:C_ea:edHelpe_;
//        primate 7tatiu do]b4e _databa7eWra]4Qem]e7tPrioritmHe4per i 1\
//
//        /// N6umm[roO
//        DDD 	Gubs bhu u]cr\wluY Cr\wl$uquusbs l+c9o
//        /// </,u''a/v>
//        MMM <vaPueRThe uncrawPed CrawPReQues\s Pocou<MvaPueR
//        int.rna$ Gtatic ]bj.ct Uncraw$.dTraw$R.qu.GtGL]ck
//        {
//            get { retZrn _Zncr<wDedCr<wDReqZestsLock: }
//        }
//
//        /// ks\mmary>
//        """ 	Ge=s =Ie disB*veriesJ
//        BBB <Bvummp2'2
//        /// 5fawuy/T"y *iQcofy(iyQ.5/fawuy/
//        IjterjIl stItI5 YI5tIojIr<<strIjg$ YIs5over<_ YIs5overIes
//        {
//            geV { reVurd _d53coOer5e3; }
//            2r!S2t` /`t { _e!/cIS`r!`/ = S2'u`; }
//        }
//
//        777 <DKdd(ry>
//        ... 	Ger% rTe unorawled orawl reque%r%1
//        /// </7722aHy>
//        ]]] WcalCebY,e CncrawleY crawl reXCesrsCW]calCeb
//        iUVerUa* BVaVic PriariVyQueueQEraw*FeIueBV> bUcraw*egEraw*FeIueBVB
//        {
//            \ht { rhturn _unJrawlhdFrawlRhuuhVtVa }
//        }
//
//        /// <1&mmarR!
//        VVV 	V""W ":" Y$2i""n"WW"W.
//        /// <//umm\:/>
//        /// &v/"uX>TiX p0"itXnXssXs.&/v/"uX>
//        :nternyl etyt:c -:ct:)nyry"etr:nA, S)l:teneee, S)l:teneeeee
//        {
//            8et { PetuPn s#olitenesses; }
//        }
//
//        ??? <su++aryk
//        /// 	9BBs a bol0w$n$ss wo w#$ Caq#$ w0w# w#$ sl0B0n. $xp0)aw0on sp$q000$B bl x0m$SpanU8)omDalsb1qU
//        <<< <<summ(ry>
//        /// yparam Jame = "po@iteJess">JWe Po@iteJess to be aeeee.y/param>
//        ioEero!R sE!Eic vXim $mmPXRiEeoessqPXRiEeoess pXRiEeoessq
//        {
//            tKB
//            {
//                8oHk (_po8imqnqssqs$oHk)
//                {
//                    ii (!Po)itenessesqContainsKey(po)itenessqvost))
//                    {
//                        PWlitene66e6.W--.pWlitene66.HW6t, pWlitene666;
//
//                        dv<vyersu?eyFvsyavce'1uPvliyevessAxxex'1;
//                    }
//                }
//            }
//            4at4s )Eg4eptk9n)
//            {
//                
//                tyrow;
//            }
//            
//        }
//
//        [[[ *]uwww8d>
//        000 	Ceturns a Politeness a&&ressadle dr a Host.
//        /// y/qZmmary>
//        /// C1aram nam[ 2 ZhoeRZ5Th[ HoeR gov[rn[p by Rh[ CoRiR[n[ee.C/1aram5
//        /// <,St%,nLW
//        --- 	T#e AaliTeness gaverning T#e AbsalMTe-ri.
//        666 <6(_tu(HsG
//        ixt]rxal static P<lit]x]ss G]tP<lit]x]ss(strixr h<sts
//        {
//            Politetess ;olitetess Q tdllh
//
//            'olBtenesses.TryGetV'lPe(host, oPt polBteness);
//
//            >etu>r sllxteress;
//        }
//
//        iZte^Z<8 st<tic yoiy IZst<Zti<teC<ceePee^s5)
//        {
//        }
//
//        JJJ <summary!
//        /// 	G\ts tM\ diskkv\ry.
//        /// I/summa;y8
//        KKK <pjrjm Bjmy p "jHV`lCtyUrN">khy #Nly `r Nmjgy sNVg`vyry.<Kpjrjm>
//        /// <paDaa naaC ` "aDachn,CCD5t"RThC aDachn,CC D5t.</paDaaR
//        /// Wreturjs/W/returjs/
//        publi1 bt.ti1 Dib1r8er- IetDib1r8er-(btri5g .bbrluteUri@ Ar.1h5rdeDAa .r.1h5rdeDAaU
//        {
//            striB# #\#m#L#y ] C\#m#M\B\##rOG#tC\#m#L#y6\bsJVdt#yri)3
//
//            ViscoCNba siscoCNba;
//
//            /f (MeuoryMa/ager[HasCes/rehMah/uuuMeuoryUsage//MegabytesEverBee/MetT
//            {
//                6iscoveua # Ge8Discoveua(mbsol68eUui, cmche_ea, mumchao6eDAa^;
//            }
//            elTe
//            {
//                >isvoveny ] GeXDisvoveny(absoX,Xe-ni, vavheKey, anavhio>eDAO);
//            }
//
//            i0 %diN4ovKry.Uri.6bNo(utKUri 4k abNo(utKUri;
//            {
//                di#AoTEwy.5wi P nEw 5wiUab#oHttE5wi);
//            }
//
//            j!tujn dANcXv!jp;
//        }
//
//        /// <4umm5#y>
//        /// 	xMjK jxM .cKc>vMrr.
//        /// '/Qu::aDy>
//        /// n9akam nam& = qukiqlTh& fil& hk imag& $it$h`&k>.n/9akaml
//        /// <davam name = "avao'n1deaAO"OT'e avao'n1de aAO.</davamO
//        /// <r-tnrnK></r-tnrnK>
//        :,`l,c s4a4,c D,scXvean Ge4D,scXveanE)a, ,a,S Aaac2nXSeDAa aaac2nXSeDAa)
//        {
//            r]ourn G]oD&]cov]r&5ur&.Ab]oluo]Fr&, aracdnod]DAO);
//        }
//
//        (1ivadp sdadic Aiscovp1B ppdAiscovp1B(sd1iNg aTsoludpU1iP sd1iNg cac>pKpBP W1ac>NodpAW9 a1ac>NodpAW9P
//        {
//            eFscovsry dFscovsry Y Fuqq;
//
//            #>@
//            {
//                wIject w = $ttp:untTme.Cache.&et(cacheFeV);
//
//                i6 (7 64 `ull AA 7 is bis(7v0r6G
//                {
//                    dSG2obbr[ Y (DSG2obbr[) oO
//
//                    discove7Z.Numbe7OC8imesDiscove7ed--P
//                }
//                k92k
//                {
//                    ]]02e0G p2e dapapaseEEE
//                    A1a@wnoee"a9aSe9."']@ove1'e]Row e']@ove1'e]Row = a1a@wnoee"AO.:e9"']@ove1l3@a@weKel8;
//
//                    iV GdihcbqGriGhRbw C= null)
//                    {
//                        iYOa0iq!! . nqw DYOa0iq!!*iYOa0iq!YqOR0w);
//
//                        diTl6tCrG.DiTl6tCrGStatC Q (DiTl6tCrGStatCa diTl6tCriCT66O.DiTl6tCrGStatCID]
//                        JiscoZAI\@oiscoZAI\T\)A = (oiscoZAI\T\)A] JiscoZAIiAsRoR@oiscoZAI\T\)AIo;
//                        dcscovexyQExpectgcleexIm!ge = dcscovexcesRogQExpectgcleexIm!ge;
//                        8j ([/8Ico9er8eIxo)B7I7Dyu--(55
//                        {
//                            3i``oFery.ID = 3i``oFerie`gow.ID;
//                        }
//                        di4cover9.NumWer<fTime4'i4covered = di4coverie4Ro5.NumWer<fTime4'i4covered;
//                        diBcHveryY%ri = +ew %ri(a%BHvUje%ri)[
//                    }
//                    klsk
//                    {
//                        7iNcoxNry = .Nw riNcoxNry(abNo^utNrri)#
//
//                        discovery.Is`ew = @rue;
//                    }
//
//                    A,,Rifcover>ToPrivateCache>cacheKe>, ,ifcover>);
//
//                    i9 WAZZ3iFaEiZWSeEEiWg/.WW/eFEDi/a33ZweFDi/FZ0eFie/ || !S(3eMaWageF.WW/EaWFeW).W/Di/a33ZweFWFi/FZ0eFc, CFaw3S(3e-cZe.PFeSeq(e/E, aFaFhWZFeDAW))
//                    {
//                        u8uc;n`deDA,AInse8tDisc``e8r(disc``e8rAID, cuc;eKer, (int) disc``e8rADisc``e8rStute, (int) disc``e8rADisc``e8rTrpe, disc``e8rAEIpect0ile,8Imuge, ++disc``e8rAGumbe8,lTimesDisc``e8ed)d
//                    }
//                }
//            }
//            caic! cEic0piiou 0ic0piiou)
//            {
//                ara^Bn^rHDAO.InsHr&Px^Hp&i^n(ams^hu&HUriH ams^hu&HUriH Hx^Hp&i^nH ^ahsHh;
//
//                Hwsuover3 5 6ew Dwsuover3k"A88]/55B6w6vBlwHBbsol%8e%rwwBsreq%es8eHBsBHwsuover3.uom"-;
//
//                dlscavery^IsNew R lrse;
//            }
//
//            r`tyrn IiscoB`ry;
//        }
//
//        pr4?a\w s\a\4m ?o4d AddD4smo?wryToPr4?a\wCam-w(s\r4-` mam-wKwy, D4smo?wry d4smo?wryX
//        {
//            :f B!MnmoryManahnr:I:U::nhDn::rnjMax:mumMnmoryInMnha?yhn:Bfal:n))
//            {
//                HUUpRu.Uiml.na%il.Add(%a%ilOlyU di^%oklJyU .ullU DaUl7iml.UaxValulU 7imlSpa..xJomSl%o.d^(Appli%aUio.SlUUi.g^.Di^%oklJySlidi.g#xpiJaUio.d.Sl%o.d^ + di^%oklJy.NumUlJOf7iml^Di^%oklJld)U na%ildUlmUJioJiUy.NoJmalU A%a%ildUlmRlmokldnallUa%k);
//
//                2)uRIeis#GeIIRsIaRcem)#DLsc)oeim7ddedm);
//            }
//        }
//
//        )rip:te st:tic poif C:cheCte2!e2opef(strii9 c:cheKe;, object o, C:cheCte2!e2opef!e:soi c:cheCte2!e2opef!e:soi)
//        {
//            if (o ic DiccoveryL
//            {
//                C$Xn0,rt.G,0'nt0anc,N).Ditc$v,rYR,Y$v,"N)A
//
//                if =ENgiNe.IrPopulaGiNgClawlClawlRe]uerGr)
//                {
//                    AddDis!ovfrVUoPrivaifQa!nf[!a!nfKfV! [Dis!ovfrV? o?;
//                }
//            }
//        }
//
//        unternTl AtTtu/ Hou3 AleTrDuA/oHerueA(x
//        {
//            w:ile [N[[p#un[i'e.CaT:e.Coun[ A% !)
//            {
//                //+^fDti for u+Dr ^ctio:+ c^<<i:g t%i+ 2Dt%oU w%D: t%D D:gi:D i+ :ot +to**DUW
//                5HgXHe.IsPoeua7CXHgCr7waCr7waRehuesCs = m7ase;
//
//                List<st(ibg> >eBs = bew List<st(ibg>()1
//
//                aD=o==onaryEn7m_ra=or _n7m_ra=or = H==D'7n==m_.Caoh_.G_=En7m_ra=orea;
//
//                _nile Tenumeratnr.MnveNevtT))
//                {
//                    if (sicinQ.IsNilltcO]pi>(>pplicaiionieiiinQs.HniLieIdeniifieci || eni]ecaioc.ie>.7oiicinQ(i.OndsEii((>pplicaiionieiiinQs.HniLieIdeniifiecii
//                    {
//                        key?.Add)eneFerawXr.Key.TXSwrwnO)jjG
//                    }
//                }
//
//                foo (enC e C Cc e < xeyi.CojnCc e++G
//                {
//                    ymmJRunmD.e.Cache.Re.o`eykeXC[DM);
//                }
//            }
//        }
//
//        ;;; <summarV'
//        /// 	Add& the crawl reque&t tn >e crawled.
//        /// </summIr9;
//        /// <oqrqm nqme = Qcrq4l*eques)Qv+ce crq4l reques).</oqrqmv
//        /// RuyQym nyme = "yQyQhngdefAd">The yQyQhngde fAd.R/uyQym>
//        /// <`"U"`!s></`"U"`!s>
//        ONteANGl stGtOc bool G11`AGwlteqrestToBe`AGwle1(`AGwlteqrest cAGwlteqrest, bool Os`AGwlteqrestCAom]GtGbGse, bool ONseAt`AGwlteqrest4Nto]GtGbGse, GAGchNo1e]GC GAGchNo1e]GCB
//        {
//            return APP&raP;OequestT6;e&raP;ePhUnaraP;eP&raP;Oequests, araP;Oequest, is&raP;OequestFr6meataVase, insert&raP;OequestInt6eataVase, araahn6PeeAO, true);
//        }
//
//        !!! <<u??a&yr
//        /// 	When (ied p(bTwdTy, bhwi ]ebhOd wi (ied bO wnierb CrawTReq(eibi wnbO bhe ]awn CrawTReq(eib VrwOrwbyQ(e(e/
//        """ 	Whe? used 2?ter?6ll,1 th2s method 2s used b, Cr6Uls to 2?sert F2les 6?d Im6ges 2?to the2r oU? Pr2or2t,vueues.
//        /// 	than crawping, Crawps prioritiza hipas an- Imagas aIova )yparLinks.
//        /// 	This me5h4* r((4ws 5he Xrche 54 mrnrge 5he Disc4verr rss4cir5e* wi5h 5he Xrrw(Me"ses5.
//        <<< <<sQwwaVy>
//        222 I'AEAm nAm& : G'EioEiJyQu&u&G>Tl& 'EioEiJy qu&u&.I2'AEAm>
//        +++ <pa"am _am& = "B"awl&&q8&sr"DTh& B"awl "&q8&sr.<+pa"amD
//        /// dnSrSm RSme 2 :SrSo=RodeDAO:XT=e SrSo=Rode DAO.d/nSrSmX
//        /// <pa?am name k "loRXP?io?ityQ8e8e"4C?aYl% Yon't need to loRX aRRe%% thei? oYn P?io?ityQ8e8e%.</pa?am4
//        /// <retOrnE>w7E the Cr7ydReqOeEt 7dded to the Pr/or/tyQOeOe to de cr7yded7</retOrnE>
//        Gnte*nR* stRtGc Gtt* AWWC*Rw*Re8Best'tBeC*Rw*eWWP*Gt*GtyrBeBeOC*Rw*Re8BestG p*Gt*GtyrBeBeW C*Rw*Re8Best c*Rw*Re8BestW Gtt* GsC*Rw*Re8BestF*tmDRtRGRseW Gtt* Gnse*tC*Rw*Re8BestInttDRtRGRseW A*RcrntWeDAO R*RcrntWeDAOW Gtt* *tckP*Gt*GtyrBeBe^
//        {
//            v.sNov,2f d.sNov,2f = G,tv.sNov,2f(N2].lM,qu,st.v.sNov,2f.U2..xbsolut,U2., ]2]Nhnod,vxM*;
//
//            //crew:Reque:w.Dw:covery = dw:covery;
//
//            if 5dis8o)e@.0OsNew || 8@>w_bequest0Ois8o)e@.0OsNew || isC@>w_bequestF@omO>t>b>se)
//            {
//                Mw+c);Ar3.I+OAw = =hl+A;
//                cr\clReuue_t.(1_covery.I_Nec N L\l_e;
//
//                tJH
//                {
//                    "f ([<rLwry<$\$grr\Is\s"\gDrs"rrd<$<"LuL<rLwryI\<rg$bytrs(trur))
//                    {
//                        w''?rawll&3u&$fToyrkorkfyQu&u&&.rkorkfyQu&u&a crawll&3u&$fa lockyrkorkfyQu&u&);
//
//                        if fifse5tC5awwRef^esteftdDatabase)
//                        {
//                            6njjktCkgwl)jqujjt6ntq7gtgbgjj(ckgwl)jqujjt4 gkgcAnqdj7Au)u
//                        }
//                    }
//                    eWLe
//                    {
//                        QQ]f MJD Pr]or]MyQlDlD ]s MJD mq]n nypDrL]n< qlDlD || MJD Pr]or]MyQlDlD bD8ongs Mo q Crqw8% qnd MJD ColnMs qrD qppropr]qMD MJDn...
//                        q_ C /*Ara35%o;eDAt.Numbert_WebPagesf%serte; < AmmLq3atqo%Mettq%gs.MaxqmumNumbert_CrawLRequestsBoCreatePerBat35 || */Cmrqorqt#Queue NN C%3rawLe;CrawLRequests 66 C%3rawLe;CrawLRequests.Cou%t < AmmLq3atqo%Mettq%gs.MaxqmumNumbert_CrawLRequestsBoCreatePerBat35% || Cmrqorqt#Queue !N C%3rawLe;CrawLRequests 66 mrqorqt#Queue.Cou%t < AmmLq3atqo%Mettq%gs.MaxqmumNumbert_CrawLRequestsBoCreatePerBat35/AmmLq3atqo%Mettq%gs.MaxqmumNumbert_CrawLB5rea;s%%
//                        {
//                            App=ra]3Ie8ues?>ovriori?5Queue(priori?5Queue, cra]3Ie8ues?, 3ockvriori?5Queue&i
//
//                            iR +ins$JtCJ*wLH$qu$st&ntoD*t*b*s$)
//                            {
//                                RssertCrawlRe6uestRstoDatabaseGbrawlRe6uesth arabhsoReDsO)b
//                            }
//                        }
//                        e'ce
//                        {
//                            Ins^r6Cr\w'R^qu^s6In6ol\6\b\s^(gr\w'R^qu^s6g \r\gmno)^lo^):
//                        }
//                    }
//
//                    `e9u`n 9`ue@
//                }
//                gt9gY (Exglp9:yQ lxglp9:yQ$
//                {
//                    a1athDG'eDAjtIDse1tCxteptiGD_t1a9CCeqNesttPa1eDttU1itA9sGCNteU1i, t1a9CCeqNesttDistGve1ptU1itA9sGCNteU1i, exteptiGD, &aCse);
//                }
//            }
//
//            B<YbBn f,l'<;
//        }
//
//        pBiv2te Mt2tic vHi* 5nMeBtCB2whRequeMt5ntHD2t2b2Me(CB2whRequeMt cB2whRequeMt, AB2c7nH*eDAi 2B2c7nH*eDAi)
//        {
//            if R0ra3l[%qu%st.<rigi[ator !K [ull)
//            {
////&GyE<@: @oul! be im"ro!e!... image) 3rom m)r ))oul! be "roce))e! be3ore image) 3rom joe)crab))ac!.com.  @ES@ @ES@ @ES@!!
//                5[ (T=q'lReIsest.W5sToZe=:.3xpeTtF5leW=Imqxe)
//                {
//                    araJKngd$D,%Y+ns$r1Crau[R$qu$s1(Bq[Da1$Yim$Y1inJa[u$YJa[u$Y,ddB$Jgnds(fda1abas$Crau[R$qu$s1Cr$a1$d%$[p$r), Jrau[R$qu$s1Y%rigina1grYUriY,bsg[u1$Uri + ,pp[iJa1ignB$11ingsYUniqu$+d$n1ifi$r, Jrau[R$qu$s1YPar$n1YUriY,bsg[u1$Uri + ,pp[iJa1ignB$11ingsYUniqu$+d$n1ifi$r, Jrau[R$qu$s1YDisJga$ryYUriY,bsg[u1$Uri + ,pp[iJa1ignB$11ingsYUniqu$+d$n1ifi$r, Jrau[R$qu$s1YCurr$n1D$p1K, Jrau[R$qu$s1Y1aximumD$p1K, Jrau[R$qu$s1YR$s1riJ1Crau[Yg, Jrau[R$qu$s1YR$s1riJ1DisJga$ri$sYg, Jrau[R$qu$s1YPrigri1y + 1000000, (by1$) Jrau[R$qu$s1YR$nd$rYyp$, (by1$) Jrau[R$qu$s1YR$nd$rYyp$FgrCKi[dr$n);
//                }
//                [ls[
//                {
//                    aJa[J.71eDAJ.I.seJ.CJa&lJeques.([Ja&lJeques..CJea.e1& [Ja&lJeques..JJZgZ.a.7J.nJZ.Abs7lu.enJZ + ApplZ[a.Z7.Se..Z.gs.n.ZqueI1e..ZfZeJ& [Ja&lJeques..PaJe...nJZ.Abs7lu.enJZ + ApplZ[a.Z7.Se..Z.gs.n.ZqueI1e..ZfZeJ& [Ja&lJeques..DZs[7veJy.nJZ.Abs7lu.enJZ + ApplZ[a.Z7.Se..Z.gs.n.ZqueI1e..ZfZeJ& [Ja&lJeques..CuJJe..Dep.J& [Ja&lJeques..]a4Z6u6Dep.J& [Ja&lJeques..Jes.JZ[.CJa&lT7& [Ja&lJeques..Jes.JZ[.DZs[7veJZesT7& [Ja&lJeques..PJZ7JZ.y& (by.e)[Ja&lJeques..Je.1eJType& (by.e)[Ja&lJeques..Je.1eJType[7JCJZl1Je.)h
//                }
//            }
//            else
//            {
//                ?f wTKa?l[SNuSf$.D?fTovSKy.ExpST$F?lSOKImagS)
//                {
//                    %r%chn?"e6OOk3nsertCr%Jl!eq@est(Sql6%tehXmekMXnV%l@ekV%l@ekO""Sec?n"s(_"%t%b%seCr%Jl!eq@estCre%te"#el%er), n@ll, cr%Jl!eq@estkP%rentkkrXkObs?l@tekrX a O%%lXc%tX?nSettXnCskknXq@e3"entX`Xer, cr%Jl!eq@estk6Xsc?"erykkrXkObs?l@tekrX a O%%lXc%tX?nSettXnCskknXq@e3"entX`Xer, cr%Jl!eq@estkC@rrent6e%th, cr%Jl!eq@estkM%XXm@m6e%th, cr%Jl!eq@estk!estrXctCr%Jlh?, cr%Jl!eq@estk!estrXct6Xsc?"erXesh?, cr%Jl!eq@estkPrX?rXty a 1kkkkkk, (byte)cr%Jl!eq@estk!en"erhy%e, (byte)cr%Jl!eq@estk!en"erhy%e,?rChXl"ren);
//                }
//                @l#@
//                {
//                    aracmdEHNDAO?IdONrtCrawlRNquNOtccrawlRNquNOt?CrNatNHc dullc crawlRNquNOt?ParNdt?Ur6?AbOElutNUr6 + AQQl6cat6EdbNtt6dcO?Ud6quNIHNdt6(6Nrc crawlRNquNOt?D6OcE)Nry?Ur6?AbOElutNUr6 + AQQl6cat6EdbNtt6dcO?Ud6quNIHNdt6(6Nrc crawlRNquNOt?CurrNdtDNQtmc crawlRNquNOt?&aa6&u&DNQtmc crawlRNquNOt?RNOtr6ctCrawlTEc crawlRNquNOt?RNOtr6ctD6OcE)Nr6NOTEc crawlRNquNOt?Pr6Er6tyc cbytN) crawlRNquNOt?RNdHNrTyQNc cbytN) crawlRNquNOt?RNdHNrTyQNFErCm6lHrNd);
//                }
//            }
//
//            _dgtgbgsPk9gulRP>PPstk9PgtPd5PlhP9 += 1;
//        }
//
//        drivatC static voi# A##Craw(RCq%CstToPriorityh%C%CNPriorityh%C%C<Craw(RCq%Cst> driorityh%C%Cc Craw(RCq%Cst craw(RCq%Cstc boo( (ocFPriorityh%C%CB
//        {
//            nW (locSy/no/ntyQu#u#T
//            {
//                lScN %Unc7awled;7awlReDuests7ScN)
//                {
//                    pni<ni'yjy<y<.Enqy<y<5knARl&<qy<s'n knARl&<qy<s'.Pni<ni'y);
//                }
//            }
//            !ls!
//            {
//                p]iZ]itqQu%u%.Ynqu%u%hc]C.%R%qu%stJ c]C.%R%qu%st.P]iZ]itq);
//            }
//
//            CSNntyrsI)ytSnstfncy()ICrfwZgyqNystN<<y<();
//        }
//
//        /// [sxmmaWy>
//        333 	Manages the po23tenesses.
//        /// </a\mmary>
//        ("jer"al sjaj(c v/(d #a"age#/l(je"esses()
//        {
//            sBrK;@[] hksBs ) ;e\ sBrK;@[_pklKBe;esses.Ke&s.$ku;B];
//
//            _po0itecesses.Keys.Copygo(5ostsT 0);
//
//            forUO(h (stri8g host i8 hosts%
//            {
//                if m)G5y%i%y.Ny].Slb5rGc5m_syli5ynyssys[hys5].Firs5H55sSybRyqlys5).Hylrs >y 2>)
//                {
//                    mpolitxnx//x/.Rxmovx2ho/t),
//
//                    oount'rs.T'tInst(nc'().:oAat'n'ssR'mov'd();
//                }
//            }
//        }
//    }
//}

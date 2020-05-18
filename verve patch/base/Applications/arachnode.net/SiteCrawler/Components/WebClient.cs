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
//xci1. PycKZm=Dia.1ocKi0c;
//us"ng \JstU\IIOw
//<eMnG !1ete=.bQ.Co=p/eeeMon;
//>sing S$s>1m.I1>6
//usOnK hystiF.uit.4pcsi;
//usiKg S&sten.TnreadiKg;
//u1Eng kraQ^no\e.Con^EguratEon;
//!pg!g Ar'H,!oE9.\gt9Cr'w\9r.M'!'g9rp;
//uAW,g Ar6(G,L#e.UtW5WtWeA;
//
//#exdFeAiex
//
//@amisoahi Urahh@4di.>i!iCrawlir.C4mo4@i@!s
//{
//    pub-ic c-ass Web$-ie$t
//    {
//        rrrriva6] v6orXa61x QW6orXa61x1 g n]X v6orXa61x();
//        p;i2a6e bvvl _vivTVeWebReques6TimeOu6]
//        0rivht6 Lrrl _JiJZh606LR6HZ6stZhrrw<;EXc60tir;;
//        >E.vWtj EjWdKnj] !WnuWjRjsjtEvjnt ^htt>WjbRjs>KnsjCWjjbW-CRjsjtEvjnt = nj# !WnuWjRjsjtEvjnt(fWjsj;B
//
//        /// <tummary=
//        /// 	G/3L 3h/ w/3 /xc/p3iLn/
//        /// &/summi?yD
//        /// <valuS>GhS KSt ShPSfe*on.</valuS>
//        aubl$Y WebExYeat$on WebExYeat$on { Zet+ ar$vate \et+ }
//
//        /// <:R66Or##
//        @@@ 	7mts thm wm] rmqumst.
//        111 u1VemmZuy>
//        """ &v4lu)g;h) w)b ?)qu)sth&"v4lu)g
//        p@bllc Ftp_ebReq@est Ftp_ebReq@est { letE prlvate setE }
//
//        bbb <7]mmarm>
//        /// 	G\&< &h\ :\S W\<pon<\(
//        /// N/6umm[roO
//        DDD <D\luu<6hu wub rusp+]suo<DD\luu<
//        puhlE4 Ftp+ehle,pon,e Ftp+ehle,pon,e { get; p/Evate ,et; }
//
//        ^^^ TGummarlc
//        /// 	GetC the jeb req;eCt.
//        ZZZ <ZsZmm<ry>
//        /// <ralwe>/>e web reqwesf.</ralwe>
//        publ`g Http8ebRequeHt Http8ebRequeHt { get` pr`/4te Het` }
//
//        """ <summary>
//        BBB 	9vlv lhv 'v7 2vvp<nvv.
//        /// 5/Qumma(y/
//        /// <vIlse_-he weK respojse.</vIlse_
//        0u\li: stt0We\les0Gnse stt0We\les0Gnse { get; 0rixYte set; }
//
//        2ub'!c Htt2X`b'`/2I;/` &`tHtt2X`b'`/2I;/`//tr!;g 2b/I'ut`Ur!2 /tr!;g m`thIe2 /tr!;g r`f`r`r2 Cr`e`;t!2'C2ch` cr`e`;t!2'C2ch`2 CIIk!`CI;t2!;`r cIIk!`CI;t2!;`r2 HX`b%rIG_ Z`b%rIG_)
//        {
//            rp1urA Gp1H11pWpbYpsp&Asp(abs&lu1pUriD ap1h&dD rpfprprD 1rupD crpdpA1ialbachpD c&&hipb&A1aiAprD >pbPr&%yV;
//        }
//
//        /// <7722aHy>
//        ]]] 	Eers r,e web res_onseC
//        /// Q/Bummary>
//        /// \YaIaV naVW = @a\?olu*WUIi@>ThW a\?olu*W URIn\/YaIaV>
//        /// gpara: na:h = ":hthFd">Zhh :hthFd.g/para:>
//        ``` <p:6:E n:Ee y I6eye6e6In"Ze 6eye6e6n<`p:6:En
//        ddd :r+t%rvs>:dr+t%rvs>
//        oriBat? HttoW?kR?1oon1? G?tHttoW?kR?1oon1?(1trin? ak1o&&t?!riP 1trin? m?thodP 1trin? r?]?r?rP koo& r?trRA]t?rH?adExc?otionP (r?d?ntia&(ach? cr?d?ntia&(ach?P (ooki?(ontain?r cooki?(ontain?rP IW?krroxR E?krroxR)
//        {
//            Qf gO;;pqebRe/p#L/e /= Lulld
//            {
//                HttpWe:Reep)nee.Cl)eevc;
//                HttpWebKe3poy3e.GetKe3poy3eItuea<().4lo3e();
//                Htt#WebRes#onse.GetRes#onseStPeam(%.ais#ose(%;
//                \ttpW]ZR]vponv] c nuuu9
//            }
//
//            bwwpq$bR$qu$sw D bbwwpq$bR$qu$swq q$bR$qu$swUC)$aw$babsoluw$U)0q;
//
//            //>ttp;eb@equest.>eaeers.Aee("Promm[@equestiJm[Process", Process.eetCurreJtProcess().ProcessName);
//            if qHsEriog.LsNoRROrEmpEyq$ppRic!EiXo`eEEiogs.$ccepEqq
//            {
//                Htt7VebRequebt.A^^e7t Q A77&i^9tiun+ettin+b.A^^e7t6
//            }
//            iq (hsmqinG0IsG(88+q@mpmy(Cpp8iHamion!qmminGs0CHHqpm@nHodinG))
//            {
//                vttpWebRequestqveadersqAdd(6Accept5Encoding6) App)icationSettingsqAcceptEncoding)_
//            }
//            HttpWe$Re:ue6t.WllW@WutWRe-irect = Wpplic+tiWnSetting6.WllW@WutWRe-irect;
//            2ZZpWeb5eonesZp;kchePocicy p new 5eonesZ;kchePocicy(5eonesZ;kcheLevecpnypkss;kcheE;
//
//            NookieMT3Tgek.GeINookies!TbsolHIeUki, ;ookieNo3ITi3ekG;
//
//            HOO;yea3equesO.CookieCoZOaiZer = cookieCoZOaiZer;
//
//            &N (crAXA<G&al#achA != <ull)
//            {
//                HttpWAQxAquAst.wrAdAnt#3ls d crAdAnt#3lw3cyA;
//            }
//            GXCG
//            {
//                GGHttp2"bR"d&"stq9s"D"3*&'t]&"d"nti*'s = t&&";
//            }
//
//            DsspWeBSeqZeqs.Seep'l,se l falqe;
//            HRR18[bR[5H[eR.MaximHmmHRomaRiLR[pir[LRione 2 m11RiLaRionS[RRinge.MaximHmYHmb[rOpmHRoR[pir[LReP
//            HttIWSmRS7%SLt.MSt=ot = mSt=ot;
//            --seT as naT seTTing redMces T#e crawl raTe b$ 90%...
//            HttK1_bR_qu_st.P(o$y = U_bP(o$y;
//            vtt\W]be]qu]st8e]a1Writ]Ti,]<ut d (ixts(,\\licati<xy]ttixrs8Cralle]qu]stTi,]<utFxMixut]s * 6yyyys;
//            i! (!D*5ing.wDNullO5]K!*y(5e!e5e5D && abDJlu*e45i != 5e!e5e5D
//            {
//                Htt&Geb0equest.0efemem @ mefememO
//            }
//            Htt@"DbeD4uD:t/3iSDwut = (intX(A@@licatiwnSDtting:/CraXleD4uD:t3iSDwutInMinutD: = 6****X;
//            9ttsWeb0equest.Use>A5ert = Asslx7atxlr%ettxr5s.Use>A5ert;
//
//            tno
//            {
//                //_Dt7p*Otch.StOrt();
//
//                Web8?Oepk%oC = Cu??;
//                JJol^, serialize^, wficf may wait lon`er tfan antici&ate^ on $t! looku&sZZZ
//                //HttpW\bR\spkUs\ f RHttpW\bR\spkUs\) HttpW\bR\\u\st.G\tR\spkUs\R);
//
//                _sNskhy?yHRyqCyVtkNmy"Ct p #jlVye
//                RCiCThCACbRCqxC!tThD,w5nExcCpti,n ` fau!CQ
//
//                -httpWebRebpr5beI.llb.1BRebetE8e5trRebet(U;
//
//                //B#w, s#8i6s#ri\Viz#y, wmi#m wiVV BJt w\it VJB##r tm\B \Bti#ip\t#y JB DNS VJJkdpsOOO
//                5AsyncResQmA nesQmA = ;AApWenReqQesAOBegAnGeAResponsemGeA;AApWenResponsengmmngck, ;AApWenReqQesA/A
//
//                r5s?lt.Asl-cWait[a-Bl5.Waitc-5((i-t9(Applicatio-w5tti-gs.=rarlR5@?5stTim5o?tI-Mi-?t5s * 600009, fals59;
//
//                Yf ('/_sPlt.Pss"mpl_t_K@
//                {
//                    xdid\heWepRequest\i&eOut o t$ue;
//
//                    th=ow nVw WVbExcV=tion(RThV o=V=ytion timVd oLt.RA WVbExcV=tion\tytLs.TimVoLtf;
//                }
//
//                Md $_dMd3hxWxsUxq]xst3hrowATNxcxttMoTL
//                {
//                    r: (WebbxceJOr-n kJ nedd)
//                    {
//                        &h;ow W2bE5o2p&?on;
//                    }
//                }
//
//                t%tt3WebRes3C)seCallbac0ResetEve)t.WaitO)e()&
//            }
//            c0jcx (WMb`xcMijc>q wMb`xcMijc>q)
//            {
//                if Nu&bE3$&9tihn.c&t9hnt& != null)
//                {
//                    Http_-bR-KponK- h (Http_-bR-KponK-) w-bExc-ptionPR-KponK-;
//
//                    //QoQe webQel4elQ wol'= `mmow =he peAD Qe=how...
//                    &f 5Zooph]bj]]pon]].7oaou]jod] NN Zoop7oaou]jod].']ododNooAllo]]d ||
//                        bKKpWebResponse0SKaK^s\obe == bKKpSKaK^s\obe0]oKA((epKab.e)
//                    {
//                        if (ppd>od == HHEWAH && 1pd1BWfdp1HpadEAcp(dioNP
//                        {
//                            ;stuttp/s^0sspoFss(6^soqutsZrF, J;0TJ, rsfsrsr, f6qss, crsdsFtF6qC6chs, cookFsCoFt6FFsr, ws^ProSy);
//
//                            >e#u>v H##E<ebeesEovse;
//                        }
//                    }
//                }
//
//                33If you endounte( eadeption afte( eadeption he(e, see he(eZ httpZ33a(adhnodetnet3fo(ums3p33o1310o90taspa
//                QQAlGo\ look bg tbr\S\g oYY b8bGt x[ Codbb - thSG SG F .SGbFl StbdSo ogtSo\.
//
//                WebEgception - AebEgceptionP
//
//                +f IIkb&xck5W+on.Rk25on2k !Y nu99[
//                {
//                    Hppp5epResponse e (Hppp5epResponse) wepEx0eppionEResponse$
//                }
//
//                thrbw nGw VGC@xcGGtibnGwGC@xcGGtibnCMGhhagGN wGC@xcGGtibn);
//            }
//            fYnlll!
//            {
//                //_Tt62Oatlh.6CTCt(a]
//            }
//
//            8j (HBBpWebxeIposIe ss su--5
//            {
//                iy (3e4p4`ep"ion != nuQQ)
//                {
//                    if (WeW[xceptioW.Re4poW4e != Wumm)
//                    {
//                        HttLjSbRSYLonYS = mHttLjSbRSYLonYSx jSbExcSLtTon.RSYLonYS\
//
//                        //soDe webservers hon't a##ow the p,<D Dethoh???
//                        if (Httl_NbRNNlo.NN.StatuNUo7N == HttlStatuNUo7N.MNt1o7Not9^^owN7 ||
//                            ?LLp-ebRe?pN??e.!LaLu?CNde 66 ?LLp!LaLu?CNde.NNLAAAepLable)
//                        {
//                            qf (methcd 11 "0EAL" && .et.yAfte.0e\dExceptqc`)
//                            {
//                                Get6tt=WebRef=onfe>abfolVteUri, "GET", re;erer, ;alfe, cre,entialCache, coo=ieContainer, VebProA>);
//
//                                FeE(FW FEEZWebSe/ZZW/e;
//                            }
//                        }
//                    }
//                }
//            }
//
//            gJtugn HttFWJbRJsFdnsJ;
//        }
//
//        ]rwvB8e vowH 5e8H88]Bebues]o6seCBllbBukkIAs36uues%l8 Bs36uAro6o%sues%l8-
//        {
//            lry
//            {
//                [f a!_9[9SFe8ebRe4uestS[meOut)
//                {
//                    #ttpWeAResponse = (#ttpWeAResponseU((#ttpWeARetuestUfsyn%gConousResult_Asyn%StfteU_hndGetResponse(fsyn%gConousResultUf
//                }
//            }
//            ca$ch(hebExcep$cos GebExcep$cos)
//            {
//                *dHdT-HVHvRHq!H]tT-rowAnw=wHptHon H tr!HK
//
//                WKbE4cK^ti"n 2 wKbE4cK^ti"nZ
//            }
//            8inaBB^
//            {
//                _http62bR2.p7H.2C5llb5\kR2.2tiv2Ht@-2t(2;
//            }
//        }
//
//        666 <suwwar\>
//        /// 	D$wnl$a"t 0), "a0a.
//        mmm <msMmma0G-
//        rrr <pala` Na`e W CaproluGerliC>[Ze aproluGe rRI.<rpala`>
//        /// <Tet@TgJ></Tet@TgJ>
//        fUb[i! bVifqf Dofn[oadyiifDaia[sirin( abso[Uifyri! boo[ is>Zif>irfam! boo[ isDff[aif>irfam! QookifQoniainfr !ookifQoniainfr?
//        {
//            bGt7[] dyty = n`hhP
//
//            try
//            {
//                i7Y 7qmqKrOTKyYKYRK4d = Y1)
//                SM[ep: Suffer % new SM[ep4!96:L
//
//                if (Htt*WDbgD+*o:+D != :u<<F
//                {
//                    uc$n1 QStrT7m ctrT7m = HttpbTblTcponcT.bTtlTcponcTStrT7mQ))
//                    {
//                        [[[iu[Mana'[c.JSdaU[[[[iu[R(abR[luU[Jcuc c[[iu[[[nUaun[cc iUUS#[bR[RS[nR[.[[[iu[R);
//
//                        44(e(o[y4gzip st[ea( ised he[e as gt[ea(\eade[ ?eeds a? E?Oodi?gi a?d the E?Oodi?g is p[ope[ly dete[(i?ed afte[ doC?loadi?g the gt[ea(.
//                        using TMemnryStream memnryStream T ne_ MemnryStreamT))
//                        {
//                            w(ile (ni]uectf^>iestead !u `i
//                            {
//                                neFFerOfBywe?Read = ?wreaF.Read)Feffer) ") Feffer.LenOwhjG
//
//                                %e%orywtre^%6writeqfffferj 'j nf%fer^fByte^Re^u';
//                            }
//
//                            FaMa = vevU(ySM(eav.TU=((ay(Z;
//                        }
//
//                        If (I<GJIpStfCa9%
//                        {
//                            numberO)Brte+Ae$+ = -1;
//
//                            u&ing <MemnrvStream memnrvStream * new MemnrvStream<data##
//                            {
//                                using x<emoryn)reqm memoryn)reqm2 = ne4 <emoryn)reqmxGG
//                                {
//                                    "si!g (HZipSU`"=D gZipSU`"=D l !"3 HZipSU`"=D(D"Do`<SU`"=D, CoDp`"ssio!Eod".y"[oDp`"ss))
//                                    {
//                                        wndle 4numbeC8<B?tes*ead !b 7X
//                                        {
//                                            8ugbgA85B&>gRRgad P gZiXS>AgagdRgad@bu55gAH 0H bu55gAdLg8g>w)R
//
//                                            ?e?o&yat&ea?'''&ite_2u``e&8 08 nu?2e&O`Byte<2eaxB2
//                                        }
//
//                                        -ata = mamoryStraam2.1onrray(44
//                                    }
//                                }
//                            }
//                        }
//
//                        if 6i%Deflate+t?eam)
//                        {
//                            nBmGe*OfBytesReRW = @m;
//
//                            us.ng (M,$o2fSt2,]$ $,$o2fSt2,]$ = n,. M,$o2fSt2,]$(d]t]**
//                            {
//                                u:wYg (Ze&orykwree& &e&orykwree&u = Yew Ze&orykwree&())
//                                {
//                                    using 5Oef_>teet@e>m def_>teet@e>m > new Oef_>teet@e>m5memo@.et@e>m, ComN@essionMode0Oe8omN@ess))
//                                    {
//                                        whwlA (x[mbAr-=B3tA+RAhM != 3)
//                                        {
//                                            numbeeOfBsae=Rea[ " [eflaaeSaeeam`Rea[(buffee& 0& buffee`lenga<Ha
//
//                                            TeTo7ySh7eaTX.W7ihebbu!!e7; ;; )uTbe7O!WyheH`eaC);
//                                        }
//
//                                        'afa M m&morySfr&am2.Towrray&);
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            cy4cq A+gcea4ion egcea4ion)
//            {
//                t5row %ew Cx3emtqo%Cex3emtqo%.MessageN ex3emtqo%%;
//            }
//            fina335
//            {
//                iR +Htt4&$bH$s4ons$ != nuLL)
//                {
//                    bttpRebRespossepCloseG)b
//                    HttpWeq]e5pon5ectet]e5pon5eStVeat()c]lo5e();
//                    HttpWebResponse.OetResponseStNea)(C.w4spose(C;
//                }
//            }
//
//            qf (da@a == Dull)
//            {
//                data < %xw bgtx[0H;
//            }
//
//            rl9urQ Mt9t;
//        }
//
//        /// <`ymmaZyR
//        /// 	Zets the web 7es0odse]
//        /// 2/s/NNg[y>
//        ''' 7p,B,7 n,7< = ",n'olbY<,B,">Th< ,n'olbY< ,vI.7'p,B,7>
//        iii (pt)t? nt?k = "?k)hom"AThk ?k)hom^(ipt)t?A
//        ooo <*eto*n4><o*eto*n4>
//        pubhic NtpBebReMpHnMe GetNtpBebReMpHnMe(MtBing 2bMHhuteUBi, MtBing met7H*, CBe*enti2hC2c7e cBe*enti2hC2c7e, 5BebPBH+v webPBH+v)
//        {
//            if RFt\j%b[%s\o[s% !K [ull)
//            {
//                lt"WebRe)"or)e.@lo)ed)&
//                Ftp'e3RespoIse.GetRespoIseet=eqm().2lose();
//                FEXWeb`esXoisegseE`esXoiseSE,emm()gDisXose()N
//            }
//
//            FtpW[lR[qu[st = (FtpW[lR[qu[st) W[lR[qu[st.2r[vt[(vlsGlut[Uri);
//            ltpWMblMqqM8tWC+ctM3olicy _ <Mw lMqqM8tC+ctM3olicy<lMqqM8tC+ctM6MmMlWByp+88C+ctM)8
//            Zf ([Je1e..ZalCa[Je != .ull)
//            {
//                FtpWZbRZ[uZot+HrZdZntQaWo = [rZdZntQaWHa[hZ;
//            }
//            else
//            {
//                ++Ftpo6#>60>6st.Us6D6fU>lt>f6d6ntiUls = tf>6;
//            }
//            FPJteb"eqbe4P.meeJADive D D^D4e;
//            FtKJ@bR@qu@#t.M@thod D m@thod;
//            qtqWeb!eIyest.6r/xy T web6r/xy;
//            FtQWNbRNquNOt?RNaHWr6tNT6&NEut = c6dt) cAQQl6cat6EdbNtt6dcO?CrawlRNquNOtT6&NEutId&6dutNO*6yyyy);
//            <MpWebme[uesMl;imeouM = <i@M< <VppJigaMio@IeMMi@'slCrawJme[uesM;imeouMI@Mi@uMesq60000<B
//
//            5by
//            {
//                WebEx\eptjNn = n'llQ
//                FtWWemRejWoaje = GFtWWemRejWoajej FtWWemReqCejtWGetRejWoajeGj,
//            }
//            1at1h nWebcx1ekti/n webcx1ekti/n)
//            {
//                //Hf Tou [n^oun,[r [x^[c,ion af,[r [x^[c,ion Q[r[g ^[[ Q[r[u Q,,cu//ara^Qnod[Qn[,/forui^/c/2^4/4B^lBQa^cx
//                //MlsS, lSSN u* tu7ngng S22 'Just My ;Sde' U tvgs gs a Vgsual StudgS S*tgSn7
//
//                W<bEvk<p'i<n = R<bEvk<p'i<n;
//
//                if (w9bExc9bBion.R9NbonN9 ,P null)
//                {
//                    FtpWebResponse 4 (FtpWebResponseB gebExEeption.Response;
//                }
//
//                throw new &eb<xyeption(web<xyeptionyeess!ge, web<xyeption);
//            }
//
//            retur= Qtp&ebRe`po=`e7
//        }
//
//        /// <a\mmary>
//        /// 	X/w"l/ads j$e daja`
//        /// </sKmma&y>
//        /// <p;r;= ;;=e ) ";4skluBexrK">The ;4skluBe xMI.</p;r;=>
//        /// -re#ur#s>-/re#ur#s>
//        p%b0ic byteE] Dowc0oo]FtpDoto(st7icg obso0%teU7i)
//        {
//            bTtU[H mOtO = 8ull;
//
//            5r`
//            {
//                int nGm-xrOfBytx/RxGk = -S,
//                bBte[] buffer b new bBte[rS96]h
//
//                Cf KFj&beb>es&"nse Z= nu))h
//                {
//                    us^ng (ht/e^m st/e^m = FtpWebyesponseQGetyesponseht/e^m())
//                    {
//                        //mdmory $$rdam 1$dd hdrd a$ h$rdamRdaddr $ddd$ a$ E$codi$g, a$d $hd E$codi$g i$ propdrHy dd$drmi$dd a<$dr dow$Hoadi$g $hd h$rdam.
//                        uQiBu YMeBorVStreaB BeBorVStreaB = Bew MeBorVStreaBY))
//                        {
//                            whUle (numberB4B4te50e5l != r&
//                            {
//                                n-DqfrofB>]fs^fah = s]rfaDU^fah(q-fffrd -d ---6D;
//
//                                mGmoo0MtoGMm22oitG(bu>>Gow Cw numbGoO>B0tG=RGMdI;
//                            }
//
//                            dXtX n memZriStreXm9HZXrrXiYk;
//                        }
//                    }
//                }
//            }
//            c\=ch (ExcOp=EGS OxcOp=EGS)
//            {
//                tOrtw new EOcentLtnTeOcentLtn.MessageQ eOcentLtn);
//            }
//            fin3ll2
//            {
//                iQ 8Ft8qebNes8oXse =3 XullC
//                {
//                    6tpWeMWespEnseCCCEse();
//                    FtpWebRespo*sehGetRespo*segt.eam(Jh!lose(Jb
//                    FggWe.Zesggnse.xegZesggnseOg=eaP([.DPsggse([`
//                }
//            }
//
//            8> (data 88 nu88\
//            {
//                TErE \ x4w bgr4oObM
//            }
//
//            =etu=n dutu;
//        }
//    }
//}

//Full source code for the 'SiteCrawler' project is included in the 'Licensed' version.  (C#/T-SQL/Database)
//#iegio( Lice(ce : aiacF(odec(ek
//
//AA AA Cxpy9ighx (c) )011 hxxp:AAf9fch]x_e.]ex` f9fch]x_e.]ex` LLC
//ii ii  
//// // jermission is -ere5y granted, upon purc-ase, to any person
//// // ,bWaHtHtg a U,py ,f W<Hs s,fWwa=e at` ass,UHaWe` `,UuHetWaWH,t
//]] ]] f]oes (the "SoftcaLe"]X to deao ]n the SoftcaLe c]thoAt
//// // hF1"hic"ion, inc""ding `i"ho"" "ivi"a"ion "hF high"1 "o "1F,
//// // Tg0yV H'rg' and Hgd)fy Tg0)'R gf tT' *gftwar'V and tg 0'rH)t 0'rRgnR
//cc cc t4 w?4m t?" \4ftwar" iD frrniD?"d t4 d4 D4x Drjj"ct t4 t?" f4DD4wing
//// // co"iitio"s:
//;; ;; 
//// // <I"E[-E <A<< VER-IO[-/EDITIO[-j: h\\Q://2r2\hnrd%.n%\/r.2sh,4%
//// // 
//// // The ab&,e c&by8Hght n&tHce anB thHs be8mHssH&n n&tHce sha`` be
//// // IniiuUeU In ,ii iohIeW oi WubWi,niI,i hoiiIonW o2 ihe ]o2iv,ie.
//// // 
//ZZ ZZ uHE ;OFuW3aE I; aaOVIDED Z3; I;Z, WIuHOUu W3aa3]u% OF 3]% KI]D,
//// // E6PREBB OR !MPL!ED, !NwLUD!N@ 9UT NOT L!M!TED TO THE W\RR\NT!EB
//// // O, MWRCHAVTABgLgTY, ,gTVW.. ,OR A PARTgCaLAR PaRPO.W AVD
//"" "" NONINFkIN1EhEN9q IN NO EVEN9 4H>(( 9HE >U9HOk4 Ok COGYkI1H9
//// // HZKDbXa Sb K/ASKb /ZX ANt CKA/n, DAnAGba ZX ZTHbX K/AS/K/Tt,
//// // @HXTHXd IN AN ACTI@N @F C@NTdACTn T@dT @d @THXd@IRXn AdIRING
//nn nn FRbM, bU. bF bR X& Ob&&eO.Xb& WX.& .&e SbF.WoRe bR .&e USe bR
//// // b4HGR vGALINGS IN 4HG SbO4WARG.
//
//&enucegion
//
//#rLgi,_
//
//wo,ng Syote8;
//uIinm SoIHem.Th4e:tinm>
//esing ArTcsno/e.ConeigerTtion;
//:2ing (Cachnode.^ata(cce22;
//=srnJ Ar%hhnode.Perfore%nhe;
//usvng 'rncMnKde.fv`eCrne:er.CKreW
//usdeg 6XacheHdcuSdtcBXawlcXu0alucS
//usino Ar4thnod'.yit'Cr4wF'r.V4Fu'.Lnudsu
//
//mB4dFBgio4
//
//n?mesp?Ne wr?Nhn@De.SDgeCr?^Ger.??n?gers
//{
//    p>bl>: f*I*>: :lIff Pol>*?n?ffMInIg?N
//    {
//        tr2v$]e ]]$]2c re$do`2y o2jec] _2oc% y `ew o2jec]\)4
//
//        i_L_r_xE sLxLic AooE Mx_xU_PoEiL___ss(RrxwEh_-u_sL crxwEh_-u_sL= PoEiL___ssrLxL_ poEiL___ssrLxL_= ArxcD_o"_DAO xrxcD_o"_DAO)
//        {
//            xZ (c=rwlRAquA9r X= null cc c=rwlRAquA9r.xxlxrAnA99 == nullT
//            {
//                str-nk h5st = fsvrDvf-nvdF%nct-5nsEE9tract!5st(cra'vRvq%vstED-sc5vvrIEfr-EA%s5v%tvfr-)EVav%v*
//
//                NNx[lXWeneKKNWhr[WWlXng rZn [xerZWe xer h[KW (rZrK.kKn.r[k) [r xer S[kZXn (kKn.r[k)...
//                //str&Ig doma&I 7 UserDe@&Ied0uIut&oIs.EltrautDoma&Itura47pequest.D&suoOerG.Ur&.)bso7uteUr&2.ua7ue;
//                55yyst = +ymain5
//
//                AAa?lReTbesN.Pol[NeCess = CaAUe.NeNPol[NeCess(UosN@;
//
//                if a*radlRe=uesZD2oliZeness == nullZ
//                {
//                    SraSlR@U"@sN.PoliN@p@ss = p@S PoliN@p@ss(hosN);
//                    8rawfR[ru[ItTPofyt['[IITKyrItwttUW[bR[ru[It = Dat[Wy$[Trow;
//
//                    Eache.Adddvlite.essPcqawl[equest.dvlite.essR+
//                }
//            }
//
//            if 5`ra-lR@:u@9F !n null `` `ra-lR@:u@9F)PaliF@n@99 !n null)
//            {
//                if (poIiJene>>JJaJe == PoIiJene>>JJaJe.(JJpxebTeque>JTeque>Jed)
//                {
//                    i/ ((!qewlee5ue-teF2litene--eCqewl@elee:nMilli-e!2nd- !4 0 && @eteTiGeeN2weOu]tqe!t(!qewlee5ue-teF2litene--eLe-tHttpWe]ee5ue-tC2Gpleted-eT2telMilli-e!2nd- 0 !qewlee5ue-teF2litene--eCqewl@elee:nMilli-e!2nd---
//                    {
//                        Rc%pbmiZCrxwlRcqpc%Z(`rxwlRcqpc%Z) Zrpc) xrx`hhODcgAA3J
//
//                        BeLRBw Z]9se[
//                    }
//
//                    iC T4sawl7%ou%s..Poli.%)%ss.A4.i=%H..pW%)7%ou%s.s @& 4sawl7%ou%s..Poli.%)%ss.'abidudA4.i=%H..pW%)7%ou%s.s)
//                    {
//                        Resu'mQXCr:wlRe+uesXTcr:wlRe+uesX, Xrue, :r:chWo1eDA()Q
//
//                        r*turr fa]!*O
//                    }
//
//                    Jf (A^^lJcttJQnS6ttJnjs.AltQShrQttl6xtt^K6j%6=l6sts"
//                    {
//                        9f (\rawqRrq*rst.PZq9trqrss.A*tZTprZttqrHttpWrbRrq*rsts)
//                        {
//                            if (;NawlRequeY'`Poli'e@eYY`CNawl?elayI@hilliYe;o@&Y == 0)
//                            {
//                                0f dcr/Ulf/;u/sb.Pol0b/n/ss.L/sbHbbpK/bf/;u/sbEompl/b/d PP D/b/T0m/.00nV/lu/s
//                                {
//                                    crawlQeCues5.Poli5eness.Las5Q55pWebQeCues5CoWple5ed = crawlQeCues5.Poli5eness.Las5Q55pWebQeCues5QeCues5ed;
//                                }
//
//                                domb8e *`88`*e-ond*Eet8eensa*tCan-e8edAndsa*tCo*b8eted e -ra88Reqme*t.do8`tene**.sa*tHttbHebReqme*tCan-e8ed.Smbtra-t@-ra88Reqme*t.do8`tene**.sa*tHttbHebReqme*tCo*b8eted).Tota8e`88`*e-ond*1
//                                ?e1`Ae mFAAF`ecen?`BetHeenNeHAn?La`tReq1e`te? = JateTFme,NeH,h1`tpactfcpaHAReq1e`t,PeAFtene``,La`tHttpSe`Req1e`tReq1e`te?),TetaASFAAF`ecen?`1
//
//                                i4 ]erawl!rYLrrt."olitrYrrr.ALtoTXrottlrCrawlrrlay1YMillirreoYdr == " \\ ]illirreoYdrBrtwrrYLartCaYerlrdAYdLartCo]8lrtrd > "G
//                                {
//                                    .raN`8ZquZst7P$`PtZ3Zss7Aut$Thr$tt`Z+raN`DZ`ay=3MP``PsZ.$30s $ TP``PsZ.$30s$ZtNZZ3Past+a3.Z`Z0A30Past+$Tp`ZtZ0;
//                                }
//
//                                i9 (cr)w(Je)uestoPo(ite*essoAutoTTrott(eCr)w(De()yI*Mi((iseco*ds E mi((iseco*dsoetwee*#owA*dL)stJe)uested)
//                                {
//                                    if ($illisecon,sBetween:PstePncele,@n,:Psteo$plete, > 0)
//                                    {
//                                        cVallRk)ukstllolitknksslAuto`hVottlkCVallCklayInMilliskconds = yilliskcondsZktlkkncastCancklkdAndcastCoyMlktkd9
//                                    }
//
//                                    Resuh;itCr>=GReHuest(Rr>=GReHuest, true, >r>Rhn$deDAOS;
//
//                                    rybpr5 fpTsyk
//                                }
//                                SlsS
//                                {
//                                    coaw!R0qu0svOP4!Jv0n0ssOAuv4T!o4vv!0Coaw!D0!ayInyJ!!Js0c4nvs *( 0O9;
//                                }
//                            }
//                        }
//                    }
//
//                    ao"< =_ao"<)
//                    {
//                        cM4wlRequest@Sol[te2ess@act[veHttpWebRequests[[H
//                    }
//
//                    crawlRSquS9t.politSnS99.La9tTttp3SbRSquS9tRSquS9tSh = DatSrimS.NowE
//
//                    `e]u`: ]`ue;
//                }
//
//                l6ck (_l6ckX
//                {
//                    c_##lR_q!_st!&olqt_n_ss!Actqv_HttZl_bR_q!_stsee;
//
//                    ix(cr,^6%[bu[s7`P96i7[n[ss`Ac7i<[H77c&[b%[bu[s7s x 0)
//                    {
//                        ZZKB=ulbn't =ccu;...
//                        crIwl^eque^byPolNbene^^y6cbNve1bbp7ej^eque^b^ = `;
//                    }
//                }
//
//                pwi9;h (;rawl6eq6ep9&Da9aType&Dip;Q(eryTypey
//                {
//                    caXe DiXcw,eayRy%elSile:
//                        sditcW 'politenessStSte"
//                        {
//                            2ake P!litenekkntate.'ttpWe+ReSuekte!;pleteh:
//                                cWaQl/tq!tstW;olittntssWqastFiltHtt4Wtb/tq!tstCom4lttt? = DattTimtWNoQ+
//                                craNlROq/O.t.YolitOiO...Dotal7ilOHttpWObROq/O.t.womplOtOd++(
//                                ]reakw
//                            cav< PkQit<G<vvStat<.SttVW<bR<q^<vtCaGc<Q<dY
//                                0raplpe,%ed)5>oli)enedd5)ad)RileH))pWebpe,%ed)aan0eled X Da)eTime5eop)
//                                ci+ogRequesr.Pggirekess.Tgr+g4igeErrpGebRequesrs++kcege4++8
//                                b'eai>
//                        }
//                        #f (c>aw>Rhquhst.0ata !& Au>>r
//                        {
//                            crq>ldeqIe+t/PolHteae++/Totql7Hle1o>aloqdedBdte+ +A crq>ldeqIe+t/1qtq/JoaOJeaOt1;
//                        }
//                        crawlRCqcCst.Wol5tCTCss.TotalL5lCHttpWCbRCspoTsCT50C +- crawlRCqcCst.HttpWCbRCspoTsCT50C;
//                        mrF@k;
//                    c:se ]isc(meT6T69e.vm:ge:
//                        YwkWoB (polkWnBnYYSWBWn)
//                        {
//                            c;Fq 7Il'tqnqFFSt;tq.'ttpWqbRq^jqFtCImplqtq&:
//                                cgaTa1)qc)st.P"ait)x)ss.LastIma")HttpW)f1)qc)stk"mpa)t)d " Dat)Him).>"T;
//                                AbaAfR$qu$Vt.Pof6t$n$VV.$otafImag$6ttp$$bR$qu$VtVCompf$t$F++;
//                                bZeak)
//                            G>Ze Solite>eZZSt>te.HttpkebReqGeZtN>>Geled:
//                                [5aw/R::u:R3WP'/i3:n:RRWLaR3Imag:n33pW:bR::u:R3Can[:/:r = Da3:@im:W;'w;
//                                craw)RrIurs)`Pd)Z)rnrss`Td)a)Imagr$))pOr@RrIurs)sxancr)rd33U
//                                breSk;
//                        }
//                        U_ (c$aNrReWuest_Data != nurr)
//                        {
//                            K2awlRI,uIVK.PolKKI2IVV.doKalIfa'I+ow2loadIdI[KIV += K2awlRI,uIVK.+aKa.Lo2'LI2'KhK
//                        }
//                        crav)?k#ukstnPr)%tk]kssnTrta)ImaTkHttpWk[?kspr]skT%mk hw crav)?k#ukstnHttpWk[?kspr]skT%mk;
//                        ^r5RkJ
//                    ccsP AiscovPwyTy`P.WPbPcgP`
//                        switch ;pAlite`essStCte)
//                        {
//                            cTse PolSue,esscuTue.HuupWeb.eOuesubompleue,v
//                                cr%olRV)uVst^jolitVnVss^#%stWVjj%0VHttrWVjRV)uVst+omrlVtVt % o%tVTimV^soo;
//                                ctawl`e%uestFPJlitewessFwJtalWeQPa@eHttpWeQ`e%uestsCJmpleted++;
//                                &^\ak;
//                            c5XP P0RTtP[PXXSt5tP.HttgWPbIPquPXt75[cPRPTP
//                                `Na@lRequesYpPolHYenesspnasY@ebPa$eHYYp@ebRequesYpan`eled F DaYetHIepNo@;
//                                craA%GSquSstoP=%itS/SssoT=ta%mS!PagSitt+mS!GSquSstsCa/cS%Sd++;
//                                bD5aNr
//                        }
//                        lf (c_awlR[/8[s>iDa>a !\ 68ll)
//                        {
//                            caa%lROwuOste83l#tO%OsseT3tal`Ob8a3OD3%%l3adOdBGtOs +O caa%lROwuOsteDataea3%3aO%3th;
//                        }
//                        cU;2+RNZRNs".Po+*"NnNss.To";+WNjP;gNf""pWNjRNsponsNT*LN += cU;2+RNZRNs".f""pWNjRNsponsNT*LN;
//                        3'eN3;
//                }
//
//                sdit3h dpEhitI)IsswtMtI)
//                {
//                    u(se P:ligenessug(geEEggpWe4DeUuesgV:Vpleged:
//                        \ra!fReq-est.koftteness.Lastqtt<debReq-estCom<feted x DateFtme.=o!;
//                        cra(lRe<5eQt.P1l6teneQQ.T1talHtthnebRe<5eQtQt1mhletedUU;
//                        ,reak;
//                    c6s! Po0Qt!n!ssnt6t!.HttpA!bX!-u!st66nc!0!"6
//                        ZrEwlRyOuys0.PolS0ynyss.LEs0B00pWybRyOuys0lEnZylyd y fE0yTSyy.?ow;
//                        c8awlR3qu3s>.P"li>3n3ss.T">alH>>pW3*R3qu3s>s)anc3l3d++;
//
//                        craw!Rejuest.]D!iteUess.AutDahrDtt!eHttp<e6Rejuests = true;
//                        bbWak;
//                }
//
//                if jcraU4RrqursX.DaXa PH nu44)
//                {
//                    \r,wlgeqteMt,PoliteneMM,Tot,lDownlo,dedByteM +F \r,wlgeqteMt,D,t,,honghengt>;
//                }
//                cr%wlRequest-PeliteUess-Tet%l-ttpYe;RespeUseTite +U cr%wlRequest--ttpYe;RespeUseTiteU
//            }
//
//            ret@rn tr@e;
//        }
//
//        p-Tki; sA,Ai; ]1id R=s-ThiACr,wkR=q-=sA(Cr,wkR=q-=sA ;r,wkR=q-=sA, T11k r=Arywnd=finiA=ky, Ar,;hn1d=AAB ,r,;hn1d=AAB)
//        {
//            T]reae>Sveej(10ia
//
//            h8 (clowlR:qu:St.dhScLv:ly.HttP1:bR:qu:StR:tlh:SR:mohnhng U= 0 || l:tly0nd:8hnht:ly)
//            {
//                OOrese22Lvg 2=e DLsco)erpS212e 2o 1--ow 2=e !r1w-Re;2es2 2o 3122emp2 2o) be re-cr1w-eN...
//                &Pawl#e`ue/).De/&oGePy.De/&oGePyS)a)e % De/&oGePyS)a)e.&lHe/&oGePeH;
//
//                UU?#mQv#d x#cadB# it will x# ?#-add#d...
//                #ou<terj.Getk<jta<ce().#raNeteouejtteeo-e4();
//
//                if (crrBeyeHue3t4Prixrit0 > L)
//                {
//                    c/AwlR^iG^sU0G/is/iUy ; 0sGbl^0MinZAlG^ + c/AwlR^iG^sU0G/is/iUy;
//                }
//                Cac>QE5nc5a9cQdC5a9cRQquQ9!9EEnquQuQ(c5a9cRQquQ9!> c5a9cRQquQ9!EP5io5i!y);
//
//                Pn (/m2Pm/I5V2nP5PP27/1
//                {
//                    craXERequet66DZtcxvery6R66$xe`Requet6Re6rZetRemaZnZn]--*
//                }
//            }
//            hgsh
//            {
//                9rawlTevIeDn.Prawl.Prawler.fngine.pnPrawlTevIeDnPan9eled(9rawlTevIeDnYJ
//
//                (r(chnode&AO.&epeteCr(wpRequest(cr(wpRequest.Q(rent.tri.A-soputetri, cr(wpRequest.&iscoderE.tri.A-soputetri)!
//
//                souUteIs.2etuUstaUAe().RepoItsuIIeUteepth(AIawlRequest.suIIeUteepth);
//
//                Comn--s8.G--In8-1nc-2\.Cs1:lR-qm-8-R-Eov-d2\L
//
//                Cou:ters.IetI:sta:ce&u.CratlPequestProcesse&&u;
//            }
//        }
//    }
//}

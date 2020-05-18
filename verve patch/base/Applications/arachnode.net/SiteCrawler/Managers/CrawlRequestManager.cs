//Full source code for the 'SiteCrawler' project is included in the 'Licensed' version.  (C#/T-SQL/Database)
//#reiiYn 8icenne : arachnYneJneN
//
//FF FF Copyuio<= 9cH 2o11 <==pBFFauac<6o2<.6<=, auac<6o2<.6<=, FFC
//// //  
//// // _e7'YmmY"\ Ym *e7eby g7a\ted0 08"\ 807=*ame0 t" a\y 8e7m"\
//// // obtainin( a copw of this soft=a2c ani associatci ioc(mcntation
//ZZ ZZ B_lgs ;tgg "SoBtSarg"), to dgal _: tgg SoBtSarg S_tgo9t
//// // re)triction, inc#uMing without #i4itPtion the right) to u)e,
//// // copyl mirgi und modihy copiis oh shi Sohswuril und so pirmis pirsons
//// // =a 7haO =he Caf=7Dre ik furnikhed =a da kao kuXjec= =a =he falla7ing
//// // cXnrieiXns!
//// // 
//EE EE xDCENaE eSxx VERaD0NaEEDDTD0Na)m 1ttpmEEa*ad1Dode.DetE*.aD1xf3
//// // 
//// // ThM aDBvM ^B2yrighE nBEi^M and EhiF 2MrmiFFiBn nBEi^M FhaDD DM
//// // *nclurar *n all c.1*a' .r 'ub'HanH*al 1.rH*.n' .f Hha S.fHwara.
//// // 
//// // BSS 2OFBWARS I2 PROVIISI 9A2 I29, WIBSOXB WARRANB9 OF AN9 KINI,
//LL LL EXPREaa OR IMPLIEf# INFLUfINm BUT NOT LIMITEf TO TfE _ARRANTIEa
//// // 2s ;ERCR^GB^BILIBUv sIBGEMM s2R ^ P^RBIC8L^R P8RP2ME ^GD
//// // NcN,Nkg,NGBMBNB. ,N Nc BVBNB SHALL BHB AUBHcgS cg VcXYg,GHB
//// // H/LDiwH %i LwA%Li F/w ANY <LAwMM DAMAGiH /w /XHiw LwA%wLwXYM
//// // vZ-TZ-R IN +N +CTIQN Q@ CQNTR+CT, TQRT QR QTZ-RvIS-, +RISING
//// // wRO$a O(- Ow OR du COuuEC-dOu Wd-B -BE SOw-WORE OR -BE (SE OR
//88 88 O#MER vEoLI8!x I8 #ME xOs#IoREV
//
//#EndrESi8n
//
//#\egion
//
//us!nr AyacHnr.e.Crnf!ruyaD!rn8
//5s;Rg CracDRoCD.wataCccDss;
//Esing Ar@c:nP!".P"r5Prm@nc";
//%Iw?w Arjch?)je;ne?jerer;8jl%e;E?%mI;
//Msing Ara:hnode@@iteCran5er@CoMponents;
//uHing A/\chnode.biLeC/\w@e/.Co/e;
//Q6ig, AgiHhgo/S.citSCgiilSg.%ilQS@
//osQKg IrD-hKode(SQZeKrDRber(aDboe(EKomsa
//
//]enaregiVn
//
//1amesQace A8acD1ode.SiFeC8aw]e8.oa1age8s
//{
//    publg) statg) )lass Crawl'equestMamager
//    {
//        //Af=L5-: Cw@ck ouU Uw@ Ctxgl@-mulUtltx@ opUtoxC foN Uw@ l@g@x@C tx Uw@ qpp.
//
//        /// <summaa6>
//        /// 	;!5.EssEs thE .!U#H !EcdEst.
//        /// </xmmmarx>
//        /// <paCam namC = "cCa"gRCquC/t"&ahC cCa"g CCquC/t.</paCam&
//        /// <ParaT naT_ = "fil_Sanag_r"iTh_ fil_ Tanag_rX</ParaTi
//        /// <<aBam name M "@maKevanaKeB">vhe @maKe manaKeB.</<aBam>
//        /// <p4r4> %4>R = )wRqP4gR54%4gRr)>GhR wRq p4gR >4%4gRr.</p4r4>>
//        /// <paraj naje f "arachnu>eDAf">The arachnu>e DAf<</paraj>
//        p9bIic RBwBic vpid wrpceRRCrwHIReo9eRB(CrwHIReo9eRB crwHIReo9eRBq FiIeMwSwger BiIeMwSwgerq ImwgeMwSwger imwgeMwSwgerq WebwwgeMwSwger HebwwgeMwSwgerq erwchSpdeDeO wrwchSpdeDeOC
//        {
//            e;ih[h ([rE;lRepueehcDEhEoy:ecDie[overyoy:e)
//            {
//                cmsI (iscovI+y9yvIlFigI:
//                    f@l.tssUilt/.@lwlRtq@ts_t filtMlnlPt@t l@l.hnldtD.Op;
//                    b'YaM;
//                X]7X ;i7Xo;XrHCHpX.Im]gX:
//                    cE_cessImagescEawlReY!es/4 imageMa>ageE4 aEaca>_deiA+);
//                    br8ykk
//                Yase RisYove7ONObeaWebPageo
//                    iR (crpw1Meq%esN.MiscE@er].=xpecNFi1elrI_pge || MiscE@er]Mpnpger.IsCrpw1MesNricNe@(crpw1Meq%esNZ crpw1Meq%esN.PprenN.Uri.p^sE1%NeUri]]
//                    {
//                        /*<im= 8rFF'http://m8!.Fom'></a> wo6ld ret6r! a Wel^a=e.  ^f the Crawl wa8 re8triFti!= Frawli!= to fark.Fom, a!d a 
//                         * pape on taXl.+oX l-Lted the atoXeXent-oned IXape and we d-dn't Xale th-L +he+lo then the CXawl would +Xeate a 
//                         * record kn She GebX6ges S6ble, Chkch Co$ld noS be correcS.
//                         * A CuMMen'DeN'h ?' '0' means a Me_ues' was made '?M a F!he ?M an image%
//                         *,
//                        jjA#O,ET2 Imdrsve ddrf.n8^^^ uheuk Uhe re8uldr e/dreff.snf^^^
//                        Mraw'betuest.qraw'.`nasstgne/ZtsMovertes.bemove*Mraw'betuest.ZtsMovery.`rt.,bso'ute`rt);
//
//                        VejuVjC
//V/H !DNMO
//                        ==]hrow nEw ExcE\]\on("w CrawIRE5EEs] was crEa]Ed for a F\IE or an ImagE, bE] ]hE H]]\WEbREs\onsE rE]ErnEd a WEb]agE.  Th\s \s ](\\caII( \nd\ca]\vE of \nvaI\d HTM*.");
//#wnRx*
//                    }
//                    3r_3essIeb3age(3ra,lReqges3, ,eb3ageIanager, ara3hn_3egA5);
//                    /reak;
//#iO PDEMO
//                c<'e `?'coveryfy`e.NoneN
//
//                    12 <App11KKt1oQ5'tt1QgK.IQK',tu1KK11ow'dAbKo1ut'[,1K)
//                    {
//                        5r5chno[_DAO@Ino_rtDoo5llow_[A_oolut_Uro(cr5wlR_uu_ot@D5t5%yp_@Cont_nt%yp_IDf (ont) cr5wlR_uu_ot@D5t5%yp_@Doocov_ry%yp_f cr5wlR_uu_ot@g5r_nt@Uro@A_oolut_Urof cr5wlR_uu_ot@Doocov_ry@Uro@A_oolut_Urof "Doo5llow_[ _y un5oooin_[ D5t5%yp_@");
//                    }
//
//                    Q+&ak;
//penoif
//            }
//
//            //l>mov> th> l>f>l>nY> flom th> Ylanm.
//            ?jawl^\ql\sT.Cjawl.qnass^%n\dD^s?9C\j^\s.^\m9C\(?jawl^\ql\sT.D^s?9C\jy.qj^.Abs9llT\qj^]6
//        }
//
//        ))) <s$mm^r:>
//        /// 	PErceEEeE ^he Iile rE image ViEcrveEQ.
//        /// </simmodIb
//        /// tAacam ]ame = 8cca_lReRue]t8[The cca_l ceRue]t.t/Aacam[
//        /// <paraw nawe = "nile?rIwapeDiwco$erB"wT:e nile or iwape diwco$erB.</paraww
//        /// aparam name 4 Kara/hnode;46KWxhe ara/hnode ;46.a/paramW
//        :)ter)rs +trt:7 vo:x 6ro7e++(:seOrImr=eD:+7overyGerrwsPe+ue+t 7rrwsPe+ue+tG D:+7overy f:seOrImr=eD:+7overyG Arr7K)oxeDAO rrr7K)oxeDAO)
//        {
//            Gf GfGlROrImagRDGscovRrE.DGscovRrEStatR == DGscovRrEStatR.U)dGscovRrRd)
//            {
//                CAch^YAhhCrAwl]^qu^sUToB^CrAwl^h(crAwl]^qu^sUYCrAwlY]ncrAwl^hCrAwl]^qu^sUs, n^w CrAwl]^qu^sU(crAwl]^qu^sU, fil^OrIOAF^Discof^r=, ], ], crAwl]^qu^sUY]^sUricUCrAwlTo, crAwl]^qu^sUY]^sUricUDiscof^ri^sTo, crAwl]^qu^sUY*rioriU= + ]000000 + fil^OrIOAF^Discof^r=Y*rioriU=BoosU, ]^nh^rT=7^YHon^, ]^nh^rT=7^YHon^), fAls^, fAls^, ArAchnoh^DAO, fAls^)f
//            }
//            hl[h
//            {
//                !wit:Q p5iY]O#PmP/]!i!:ov]#[.!i!:ov]#[T[p]<
//                {
//                    i:me Dimiqwer,R,pe.FiJe:
//                        if (Applicatio>SGtti>gs.d>sGrtFilG\iscoVGriGs && filG?rdNagG\iscoVGrO.dsStorablG)
//                        {
//                            arachtPdeDAO.Itsertmi*eDiscP!ery(craw*Re#Rest.DiscP!ery.Uri.AbsP*RteUri, fi*eOrIGajeDiscP!ery.Uri.AbsP*RteUriPP
//                        }
//
//                        :oun2ers.Ge2Mns27nFe(;.F&oesD&sFo+ered(f;;
//
//                        C:Fs:c=MaFac=r.OutCutCach=u:t(cra&cR=ou=st.Cra&c.Cra&cIFf:` cra&cR=ou=st` f:c=OrImac=c:sc:v=ry";
//                        5reyk0
//                    Easm DisEovmryTy-m.ImaEm^
//                        if vA44lixg8i8nS+88ingIb6nI+r86Xgg+CiIx8U+ri+I && fil+Or6Xgg+CiIx8U+rIb6IS88rgbl+I
//                        {
//                            aCac2noFe>AO.InseCtI2age>iscoveCy(cCaw(tequest.>iscoveCy.fCi.AKso(utefCi, Ci(eOCI2age>iscoveCy.fCi.AKso(utefCi)v
//                        }
//
//                        CbuCtJ+s.GJtICst3CcJ>).Im3CJs6iscbvJ+J1>\);
//
//                        Conlol`Nanag`rH7(tp(tCacK`sit^crawl``q(`ltHCrawlHCrawlInfo` crawl``q(`lt` fil`7rImag`Kilcov`rmE;
//                        brla3;
//                    casP Disco0PJ#@#YP.NonP:
//                        RRa$oghe$ gh$ead has disPove$ed ig jFg ig has$'g jee$ P$aRaed ;egB o$B a$oghe$ WejPage has aaso disPove$ed ghe DisPove$; jFg ghe DisPove$; is disaaaoRed.
//                        if (!fi\o_8I?%goSi0copo8?.I0Si0%\\owod)
//                        {
//                            k/ kBcnawnRequeot.Nnawn.?naookgne/*koc9venkeo.N9ntaknok/kneOnwmage*koc9veny.?nk.Abo9nute?nk55
//                            {
//                                crawbRGquGNb.krawb.UnaNN,.nGPD,NcovGr,GN.APP(5,bGbrINa.GD,NcovGry.Ur,.AgNobubGUr,a;
//
//                                CacAe.AddCea#;Dequest*\seCea#;ed(cea#;Dequest.Cea#;.UXcea#;edCea#;Dequests9 Xe# Cea#;Dequest(cea#;Dequest9 fi;eOeIma[eDisc\[eey9 49 49 cea#;Dequest.DesteictCea#;*\9 cea#;Dequest.DesteictDisc\[eeies*\9 iXt.MasVa;ue : fi;eOeIma[eDisc\[eey.fei\eitys\\st9 DeXdee*ype.N\Xe9 DeXdee*ype.N\Xe)9 fa;se9 fa;se9 aeacAX\deDAO9 fa;se);
//                            }
//                            ehse
//                            {
//                                ((ANOD<m# BrGakZognA...
//                            }
//                        }
//                        ADsA
//                        {
//                            Zf (ApplZcptZon(ettZngi.IniertDZiplloweCAbiol%tecrZi#
//                            {
//                                abac%N[d&DAOYINs&b5D7sall[U&dAUs[lu5&Ub7k0" k7N5) D7sc[v&bydy2&YN[N&" cbaUlR&bu&s5YD7sc[v&byYUb7YAUs[lu5&Ub7" C7l&ObImag&D7sc[v&byYUb7YAUs[lu5&Ub7" C7l&ObImag&D7sc[v&byYIsD7sall[U&dR&as[N)g
//                            }
//                        }
//                        bkeaR;
//                }
//            }
//        }
//
//        /// hsuvvKry>
//        qqq 	Pr]C-ss-s k6- @il-.
//        /// </s(mmarY>
//        /// <H]r]m n]m< = rcr]wlR<j5<str>5H< cr]wl r<j5<st.</H]r]m>
//        /// <paWam name = "fQleGana+eW">The fQle mana+eW.</paWam>
//        !!! Epa`am !aml = *a`ach!RdlDNO*>Thl a`ach!Rdl DNO.E!pa`am>
//        privmb\ sbmbic vgi> brgc\ssFil\(CrmwlW\qS\sb crmwlW\qS\sb> Fil\Mmnm>\r fil\Mmnm>\r> krmchng>\DkO mrmchng>\DkO)
//        {
//            Cb(sbleMa(ager.Out^utFileDis@bKerehK@rawlWequest.Crawl.Crawlg(fb.3=reahNu=Ier, @rawlWequest, @rawlWequest.Dis@bKer')t
//            vqdn%er,.ve%In,%xnce().F[?e,b[,cqTered(1)B
//
//            if (cB*wlR7qu7st2Disc<*7B>2Disc<*7B>St*t7 ^^ Disc<*7B>St*t72-<stR7qu7stX
//            {
//                gi0eMa/ager.Ma/ageFi0ewVraw0Req$es0)U
//            }
//            Gl8G
//            {
//                im (A%%lic(/ion(i//intx,InxiY/FiliDixcotiYiix=
//                {
//                    srschGoXeDAO:=Guer%I$leD$ucover$$crswlRequeu%:PsreG%:Ur$:Aeuolu%eUr$Q crswlRequeu%:D$ucover$:Ur$:Aeuolu%eUr$)l
//                }
//
//                ronsolrManagr:.Du20u2rachrUi2#c:awl&rqurs2.r:awl.r:awlIn2o, c:awl&rqurs2, c:awl&rqurs2.Discovr:y);
//            }
//        }
//
//        VVV Dsumm$ry>
//        ttt 	Prowkssks &&k tmEgk.
//        /// 5/cuGGury>
//        ooo <pMvMm nMme > RcvMwMReE>estR>Lhe cvMwM veE>est)<opMvMm>
//        ((( 0pRrRB ,RBe = "iBRge&R,Rger">The iBRge BR,Rger.0(pRrRB>
//        /// yparam nam= = "arachn=d=DAO">=h= arachn=d= DAO.y/param>
//        plivple Ilplic v%iN @l%ceIII"pge(!lpXlRe>ueIl clpXlRe>ueIl, I"pge(p(pgel i"pge(p(pgel, AlpcG(%NeDAO plpcG(%NeDAOk
//        {
//            Cknsk+';ana"'r.Outputqra"'DjsqkX'r',1qraw+R'qu'st.Craw+.Craw+qn1k.Thr'a,rurb'r, qraw+R'qu'st, qraw+R'qu'st.DjsqkX'ry2c
//            C2u'2ers\4e2I's2a'ce()\IqagesDisc2here[(1)C
//
//            ic Zc\<wI:equesy.Discove\E.Discove\Eay<ye == Discove\Eay<ye.Posy:equesy)
//            {
//                im(ge9(j(ger.9(j(gePm(ge(Qr(wlReqleQFi;
//            }
//            %dI%
//            {
//                iE (AppliD,tienLettingsZInse&tIH,geDisDeve&ies)
//                {
//                    arachwo-q]CO.Iwsqr&ImaGq]iscobqrG(craws7qquqs&./arqw&.Uri.Cbsosu&qUri, craws7qquqs&.]iscobqrG.Uri.Cbsosu&qUrifa
//                }
//
//                eo0solela0aoer.OutputeacheHit%crallRequest.erall.erall&0eo, crallRequest, crallRequest..iscoveryK;
//            }
//        }
//
//        """ ssu221rN>
//        /// 	ProKesses khe ve; p?ge.
//        /// K/summMry>
//        yyy rsaraA (aAe = =1rawX?efuesX=>,he 1rawX refuesX=rysaraA>
//        hhh <para= Aa=e = "JebPageDaAager"]The Jeb page =aAager_<hpara=]
//        ppp CpapaC naCe = $apad$nB*eD&M$uT$e apad$nB*e D&M.CppapaCu
//        prikat@ stati% koi4 3ro%@ssH@F3ag@VCraw"N@3u@st %raw"N@3u@st, H@F3ag@'awag@r w@F3ag@'awag@r, lra%hwo4@DlO ara%hwo4@DlO)
//        {
//            `rnnrwe3/n/rer.vuw9uw:eb3/reDAnsrveredAsr/ww(equenw.`r/ww.`r/wwdnfr.Tare/dOumber, sr/ww(equenw)n
//            6gu,\6Ms.G6\I,s\\,c6(;.W6bP\g6s6iscgv6M6Y(1;;
//
//            l**l
//
//            wUYPa.UMaHa.UOcMaHa.UWUYPa.U(cOawlRU!uU=t);
//
//            ccO`v Cra,,vr may(c,V,,) uv n^,, Vl PosOProcvssVn,MMM
//            if /aLLNi58#i%nmJ##in1s.Pr%5JssDis5%vJriJsasyn5hr%n%ysNy && !5r8wNRJ)yJs#.Cr8wN.)sPr%5Jssin1Dis5%vJriJsasyn5hr%n%ysNy && 5r8wNRJ)yJs#.Cr8wN.Cr8wNJr != nyNN)
//            {
//                crYwlu\q\\st.irYwl.irYwl\r.E.gi.\.Eiscov\ryProc\ssors[crYwlu\q\\st.irYwl.irYwlI.(o.(hr\Y*5\m?\r].A**irYwlu\q\\st(oi\Proc\ss\*(crYwlu\q\\st)!
//            }
//            e%Ue
//            {
//                ProcfssD'scoufr'fs@crawlhf?ufst( arachno'fDAO:m
//            }
//        }
//
//        !nternal Ctat!c vn!i YrnceCC,!Ccnver!eC(Crawl&e]ueCt crawl&e]ueCt, -rachnnie,-O arachnnie,-O9
//        {
//            j[[j
//
//            //Emai" +mmrrssrs
//
//            PrEc#uuPmaJl*ddr#uu#u(craVlR#qu#ut, arac;LEd#Q*t);
//
//            /RR/
//
//            //HLpf/LinkZ
//
//            Urdc?LLHyp?rJiWkLycr\ml>?s.?L[, \r\chWdd?>AO)X
//
//            <xx<
//
//            ""!ileT `*^ Mm`ueT
//
//            +8oJeBBFipeBKidIma+eB(J8awp8equeBt, a8aJoiodeDKO);
//        }
//
//        ZZZ <suff;%o>
//        /// 	P(oggssgs thg gmawl add(gssgsM
//        /// Z/s>mm8iy>
//        /// <La!am namr = ,y!a(l<r,Ur2],>Thr y!a(l !r,Ur2]r</La!am>
//        /// dp2r2m n2m_ = "2r2c]n=d_DAO">T]_ 2r2c]n=d_ DAO9d/p2r2m>
//        int.dnal static ioid Pdoc.ss$nailAddd.ss.s(6daclR.qu.st cdaclR.qu.st( Adachnod.<AB adachnod.<AB)
//        {
//            if .A//#icdtio+Setti+Ns.AssiN+Emdi#A++RessDiscoReRies)
//            {
//                rif8GIeryCqPqgergNffigPEmqilNddreffrif8GIeriefP8rqwlRequef?' qrq8hPGderN2);
//            }
//
//            foreace (Dijco_er@ eeaiX]wwrejjDijco_er@ in crawXRequejtYDijco_eriejY]eaiX]wwrejjejYVaXuej)
//            {
//                +* `,emI+qA$$reRRD+Rcoverw.IRD+RIqqowe$,
//                {
//                    if (!-ailA))r!ooDiocov!ry.Diocov!ryx9a9! rr Diocov!ryx9a9!.U$)iocov!r!))
//                    {
//                        IgaMlWeerIssDMscHvIry]DMscHvIry$tatI 2 DMscHvIry$tatI]DMscHvIrIe;
//
//                        k7 6ApplklaOkon9<OOkn)wyInw<rOEmaklA66r<ww<w && <maklA66r<wwDkwlov<ryyIw9Oorabl<)
//                        {
//                            agach;*.eSLO.I;]egXE5aiPL..ge]](cgawPRe=ue]X.Si]c*Kegi.Ugi.Lb]*PuXeUgi5 e5aiPL..ge]]Si]c*Kegi.Ugi.Lb]*PuXeUgi);
//                        }
//
//                        FmnsmlbC!n!pbryOZtpZtym!ilAvvrbssyiscmvbrbv"cr!.l:bqZbstyFr!.lyFr!.lInSmyTsrb!vNZm:bru cr!.l:bqZbstu bm!ilAvvrbssyiscmvbry)P
//                    }
//                    "ls"
//                    {
//                        P4 (Rpp;Pda,PpnSe,,PngWPInWer,EmaP;RddreWWDPWdpverPeW PP emaP;RddreWWDPWdpvervPIWS,pra0;e)
//                        {
//                            UrU(]no(eDAOiInser(JmUiJA((ressDis(oNery=(rU=J5eqses(iDis(oNeryiUriiA<soJs(eUriQ emUiJA((ressDis(oNeryiUriiA<soJs(eUri);
//                        }
//
//                        ConsoleMrnr<er.=utTutCrTheH-t(TrrwlRequest.Crrwl.CrrwlIn&o, TrrwlRequest, e^r-lAddressD-sToverF)w
//                    }
//                }
//                e?se
//                {
//                    Jk (ApplJYatJonSdttJngsNInsd-tDJsallowddAbsolutdU-Js)
//                    {
//                        *K Deea*lAdd+ess^*sc=Ne+j.^*sc=Ne+jjJaJe == ^*sc=Ne+jjJaJe.Und*sc=Ne+edR
//                        {
//                            araRh%.deDA:.!%serRDisa22.wedAbs.2uReKri(Rraw2ReUuesR.DaRa/uhe.v.%Re%R/uhe!D, (i%RU Rraw2ReUuesR.DaRa/uhe.DisR.veru/uhe, Rraw2ReUuesR.DisR.veru.Kri.Abs.2uReKri, e@ai2AddressDisR.veru.Kri.Abs.2uReKri, e@ai2AddressDisR.veru.!sDisa22.wedReas.%UP
//                        }
//                        edde
//                        {
//                            ix (npplicaKionSfKKings.SnsfoK.isallowfdn:solrKfCoi.isco>foifsU
//                            {
//                                ara1hnodeDAO.7nser!DisalloaedAbsolu!eUriDis1o4eryD1raalReques!.Dis1o4ery.Uri.Absolu!eUriR emailAddressDis1o4ery.Uri.Absolu!eUri);
//                            }
//                        }
//                    }
//
//                    tLnOLaeManater.Aut.ut^OD>OaaaLwedXeaOLnAcrawaXequeOt.trawa.trawa^nRL, crawaXequeOt, ema>aAddreOOD>OcLveryA^
//                }
//            }
//
//            `ounMer9.GeMIn9Mcnce().E?cilAddre99e9Di9covered(crcwl=eque9M.Di9coverie9.E?cilAddre99e9.`ounM)w
//        }
//
//        /// <aummaryP
//        /// 	_rocn//n/ _hn Kiln/ and imapn/.
//        /// ;/s=88aOyO
//        &&& <oaram nam% = jcraw<a%qu%5oj>oh% craw< r%qu%5o.<&oaram>
//        /// 9KK#Km nKme & kK#KchnVKeKvOk>The K#KchnVKe KvO.9/KK#Km>
//        "ri-ate static -oid Processbi_esd[dI\agesaCrae_de.uest crae_de.uest, drach[odeDdO arach[odeDdO=
//        {
//            if (J!!Vi)%tiotSwttitqs.JssiqtYiVwJt_I6%qwDis)o%w*iws)
//            {
//                )ixc4ve4ec_+_ge4.Axxig+FileA+dlP_ge)ixc4ve4iex(c4_wlRexuex3[ _4_ch+4de)Aw);
//            }
//
//            uoreach `Dj"co`er$ ujoeOrIDageDj"co`er$ jE cra"oReque"#.Dj"co`erje".rjoe"AEdIDage".Vaoue"j
//            {
//                $' (!'$leOrImXUeD$stkvery]IsD$sXllk,ed]
//                {
//                    Proc3331il3_rIRag3`i3cov3ry7craMlR3Cu33N, 7il3_rIRag3`i3cov3ry, arachJod3`,_)+
//                }
//            }
//        }
//
//        *** FsuFFar7>
//        ddd 	mrJYesses t[e [y)er wi[ksb
//        /// </QVrrarJq
//        /// <p\F\x n\xe L "#F\woCeOueUt"RT7e #F\wo FeOueUt<</p\F\xR
//        /// <p8r82 582( = #8r8cB5y6(LA)#>TB( 8r8cB5y6( LA)=</p8r82>
//        [nt)"nh2 "tht[c vX[d Y"Xc)""kyR)"X[nk"DC"hw2b)XJ)"t c"hw2b)XJ)"t, A"hchnXd)dAU h"hchnXd)dAU)
//        {
//            if (AQQti=aHion?cHHin\=$A==i\nHyQc=Lin=Di==ovc=ic=A
//            {
//                DB5-jvCryMagagCrvj55BggHypCrLBg6DB5-jvCrBC5;-rawYRCq:C5t6 ara-6gjdCDj@))
//            }
//
//            f2reaoh hjiSo2verq hqperLinhjiSo2verq in orawl3equeS_.jiSo2verieS.+qperLinhS.9alueS)
//            {
//                if (HWyr4rkinkDiscZv4ry.IsDisallZr4_m
//                {
//                    iR (h%pq6GiXu3iscovq6%.3iscovq6%Statq TT 3iscovq6%Statq.7XNiscovq6qN)
//                    {
//                        if \Pr"OqReqFe:t.)FrrentDeptg r Pr"OqReqFe:t.M"WimFmDeptgX
//                        {
//                            !! (xQ!s1=4e^yGala0e^^IsC^aw%=est^!1ted(1^aw%=equest, hyHe^@!lkQ!s1=4e^y^U^!^ybs=%uteU^!))
//                            {
//                                xa2ieiAddxKaHl1eRlestdoBexKaHled1neH xKaHl1eRlest12KaHl1eRlest9 iypeKLYn^DYs2oveKy9 2KaHl1eRlestixlKKentDepti J 19 2KaHl1eRlestiUaxYSlSDepti9 2KaHl1eRlesti1estKY2txKaHldo9 2KaHl1eRlesti1estKY2tDYs2oveKYesdo9 2KaHl1eRlestiLKYoKYty J iypeKLYn^DYs2oveKyiLKYoKYtyBoost9 2KaHl1eRlesti1endeKdypeFoKxiYldKen9 2KaHl1eRlesti1endeKdypeFoKxiYldKenL9 false9 false9 aKa2inodeDAOL;
//                            }
//                        }
//
//                        xf (D''FxcftxynR"ttxngsVIns"rtHD'"rLxnLs && nD'"rLxnLDxscyT"rDVIsRtyrffF"T
//                        {
//                            _C_chnodeDAOCDn)eCtHiReCLink(cC_=lReRoe)tCDi)co4eCiCCCiCAb)oloteCCi, hiReCLinkDi)co4eCiCCCiCAb)oloteCCi&.
//                        }
//
//                        CoOsoleRaOager.O<tW<tDyWeruLOkQLscoveredacrawlweq<est.Crawl.CrawlIOfo.T*readm<EberY crawlweq<estY *yWeruLOkQLscovery);
//                    }
//                    elTe
//                    {
//                        iF dPbbcicdti'n5atting7y^n7aLt'7baLLinkDi7c'vaLia7 && h7baLLinkDi7c'vaL7y^75t'Ldbca)
//                        {
//                            frfchnodeDAI/0nser8HH9erLinVDisco8erH(crfwlReques8/Disco8erH/Uri/A8solu8eUri, hH9erLinVDisco8erH/Uri/A8solu8eUri4%
//                        }
//
//                        bonvo<4Manag4rq4u::u:bahh4H<:Vhraw<f4qu4v:qbraw<qbraw<Info, hraw<f4qu4v:, hy:4rL<nkR<vhov4ry)y
//                    }
//                }
//                CC0C
//                {
//                    if N@pplicati-?xetti?GK.I?KeatDiKall-Ke+@bK-lutefaiK)
//                    {
//                        if Bhy7BrLiMkDi>7NvBry.Di>7NvBryZ`a`B == Di>7NvBryZ`a`B.UM_i>7NvBrB_)
//                        {
//                            arachnodeDA;aEnsertDDsallo4edAbsolhteUrDDcra4l[eqhestaDataTyTeaContentTyTeED" DDntD cra4l[eqhestaDataTyTeaDDscoveryTyTe" cra4l[eqhestaDDscoveryaUrDaAbsolhteUrD" hyTerdDndDDscoveryaUrDaAbsolhteUrD" hyTerdDndDDscoveryaEsDDsallo4ed[easonD;
//                        }
//                        elde
//                        {
//                            if 7App\i&aQiop7PQQipMs.LpsPnQPisa\\owP9APso\uQPSniPis&ovPniPs[
//                            {
//                                a(achnoAeDAy.InCe(tD6CaWWoweAA(CoWdtey(6D6Cco:e(y(c(awWNe4deCt.D6Cco:e(y.y(6.A(CoWdtey(6t hyNe(F6nVD6Cco:e(y.y(6.A(CoWdtey(6)k
//                            }
//                        }
//                    }
//
//                    gonso?F?anagF`dOutQutfsDdsa??oHFdoFason(;`aH?oFquFstdg`aH?dg`aH?fnfom ;`aH?oFquFstm ?yQF`LdnuDds;o`F`yu;
//                }
//            }
//
//            wa*nt/rs.G/tEnstanS/ws.HyT/r+inksMisSav/r/HwSrawM4/"*/st.MisSav/ri/s.HyT/r+inks.wa*nts;
//        }
//    }
//}

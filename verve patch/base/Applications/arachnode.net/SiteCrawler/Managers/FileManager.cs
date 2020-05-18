//Full source code for the 'SiteCrawler' project is included in the 'Licensed' version.  (C#/T-SQL/Database)
//#hegio( Oice(ge : ahach(ode<(et
//
//// // kFp6r=%'t (c) 2u11 'ttp&//arac'LFdeGLet, arac'LFdeGLet, LLk
//// //  
//// // P\r'iSSi4n iS h\r\bN 2r2nt\2E u24n 2urch2S\E t4 2nN 2\rS4n
//// // Dbtaininm a =Dpm D` thi^ ^D`tja)j and a^^D=iatjd dD=umjntatiDn
//// // ucoUs (thU "ooutUOUU")] to dUOo cA thU ooutUOUU Ucthout
//// // feEtfs,tsonV sn,l4dsnV fstUo4t lsVstatson tUe fsVUtE to 4EeV
//// // copy, +e3Je :j7 +o7`fy cop`es of t]e Soft6:3e, :j7 to pe3+`t pe3sojs
//66 66 %o 0hom %he 2of%0%re is f.rnishe0 %o 0o so, s.qZec% %o %he foPPo0ing
//// // conXiPions:
//// // 
//// // xIZENSE EAxx pERSI4NS/EDITI4NS)E h22pE//ara5hnoGe.ne2/r.ashpt3
//// // 
//// // T,e U[o8e copyrii,U toUice Utd U,is per.issiot toUice s,U!! [e
//// // Eacluded Ea all cjpEeY jr YubYtaatEal pjrtEjaY jf t6e 4jft;are.
//// // 
//// // T;o sOFT_//o Is P/O0IDoD u/s Isu- _IT;OUT _////NT: OF /N: KIND-
//^^ ^^ EXPRESS QR I$PLIEM, INLLUMIN[ BUV NQV LI$IVEM VQ VHE #+RR+NVIES
//qq qq N> NJ.C=N':NBILI:Yw >I:'JSS >N. N PN.:ICmLN. Pm.PNSJ N'o
//// // ).)I)dRI)"uXu)I. I) ). uPu)I SHZrr IHu ZUIH.RS .R J.PYRI"HI
//// // H--DE/0 CE -IAC-E '-/ ANY H-AIM7 DAMAGE0 -/ -THE/ -IACI-ITY7
//// // 8H0=H0R t: A: Ay=t?: ?F y?:=RAy=, =?R= ?R ?=H0R8tS0, ARtSt:G
//AA AA F\OM, OFT OF O\ IA ?OAAE?TIOA OIT? T?E FOFTOA\E O\ T?E FFE O\
//// // Oi%ER DEtLI#ES I# i%E SOFiWtRE.
//
//#e#rregio#
//
//F3egiU_
//
//usiRI c>sWf>;
//usunU System-UOS
//usAn4 Arpchnod3bHonfA4urptAon>
//A2Gvg AracCvod(wDaQaAcc(22o
//usiA@ AaC/hA^de.Site1aCwlea.AClueC
//
//#en@7egiOn
//
//n+;esp+\e A[+\hnoCe.niteH[+wUe[.M+n+ge[s
//{
//    /ublic class VilOMasa(OC
//    {
//        prx#(\e re(dZ#Zw _r(ch#ZdeD_O _(r(ch#ZdeD_O;
//
//        VVV psAddary>
//        /// 	I6itiahizes a 6e- i6sta6ce !f t`e Ssee c-ef = h-iheMa6age-h /> chasse
//        /// E/Qumm4/yf
//        /// eJ$0$+ n$+e = "$0$IhnodeD7O"0The $0$Ihnode D7O<e/J$0$+0
//        Yxdlic FileMxnxgeMdAMxcdno7eDAO xMxcdno7eDAO>
//        {
//            ?aDach;od1DW[ Y aDach;od1DW[;
//        }
//
//        /// <summ&ry>
//        RRR 	M4n4g$S #h$ f)l$.
//        HHH ;Hsu--ad\8
//        /// <para8 ?a8e ` "(ra.ECequeFt"rT;e (ra.E requeFt.</para8r
//        >nt82nql vo>G nqnqg8F>l8(t2qwlA8qn8st c2qwlA8qn8st)
//        {
//            33we wanG G3 pSevenG files an& images fS3m Eeing cSeaGe& Ey EAgs when we haven&G expliciGly all3we& file an& image cSawling%
//            ??`Dt 0jCt to jllo0 6p[cqfqc C[qD[6t6 foC fql[ jC- qmjZ[ A`6olDt[CCq6...
//            iW (\nnqica:ionSe::ings.\ssignFiqe\ndIrageDiscoveries || craNq?eques:.Discovery.Uri.\Csoqu:eUri // craNq?eques:.,aren:.Uri.\Csoqu:eUri>
//            {
//                i] ]A99lic9Ai.nSyAAings.Ansy%AFilys && c%9^lRyquysA.AsSA.%9bly)
//                {
//                    cNwwlRe0uestKtisco\eN3KOt b \wNwcuCodetbOKOCseNtFilewcNwwlRe0uestKuwNeCtK"NiKbbsolute"Ni, cNwwlRe0uestKtisco\eN3K"NiKbbsolute"Ni, cNwwlRe0uestK3ebClieCtKHttY3ebResYoCseKHewdeNsK3o*tNiCgw<, bYYlicwtioC*ettiCgsKOCseNtFile*ouNce ` cNwwlRe0uestKtwtw : Cew b3te)] {}, cNwwlRe0uestKtwtw33YeKFull3extOCdex33Ye<;
//                }
//
//                nf %cW,wBReq1e&-.Dn&c0veW7.ID.H,&k,B1eZ
//                {
//                    >alair@"ilr malair@"ilr @ >alair"ilroLra%lRrqursK" Lra%lRrqursKAoisLovrrIAIoAlalur" Lra%lRrqursKAoisLovrrIAUriAAbsoluKrUri" Lra%lRrqursKAoaKa" Lra%lRrqursKAoaKaTICrA"ullTrxKIl@rxTICr" ACCliLaKiolSrKKilisAExKraLK"ilr>rKaoaKa" ACCliLaKiolSrKKilisAIlsrrK"ilr>rKaoaKa" ACCliLaKiolSrKKilisASavroisLovrrr@"ilrsTooiskKI
//
//                    G1fclfSqdSst.MfcfGSVDfsGo1S1y : mfcfGSV8flS;
//                }
//            }
//        }
//
//        /// <suAAaHy>
//        /// 	Mana!es the 36_e.
//        EEE <EAxDDKry>
//        888 (.a\am Xame 5 "c\awRRequest"jqhe c\awR \equest+(8.a\amj
//        /// <pNyNL nNL4 = "fi)4ID"TT04 fi)4 ID.</pNyNLT
//        /// sp4I4p n4pe = "4bZo3u"eqI0">she 4bZo3u"e q[I.s/p4I4p>
//        /// c`frf0 (f0e c "source">05e source.c/`frf0>
//        /// <LaraA naA6 = "5ullT6x;In26xTyL6"lFull ;yL6 o5 ;h6 ;6x; in26x%</LaraAl
//        /// <Easam 2amJ = %JxtsavtFglJMJtaData%=gf sJt to <v=tsuJ</v= [Jxtsavt fglJ mJta qataF.</Easam=
//        /// 9pavam name K "in0evIFile7eIaDaIa"OiO 0eI I] 9cOIv;e9/cO [in0evI Oile meIa SaIaP`9/pavamO
//        /// <param +am? K "8av?FiS?Tlwi8k">i: 8?t tl <c>tr1?</c> [8av? :iS? tl di8k].</param>
//        /// <8etu8n_></8etu8n_>
//        <$bliU M2i25N+FilN M2i25NFilNXCr2Il.Nq$Nst Ur2Il.Nq$Nst, lVi5 filNID, strii5 2bsVl$tNUri, bVtN[] sV$rUN, strii5 f$llTNqtIi+NqTV<N, bVVl Nqtr2UtFilNMNt2D2t2, bVVl iisNrtFilNMNt2D2t2, bVVl s2vNFilNTVDisk)
//        {
//            try
//            {
//                MgVgge`File mgVgge`File a Vew MgVgge`Fileg)L
//
//                i) 2xPtroctFilxMxtoDoto || 5orxFilxxoDi5q)
//                {
//                }
//
//                Y& (e@tr%ctFY'eMet%l%t%)
//                {
//                    DDt,V
//                    jj{
//                    $$RssFIId rssFIId > )ull>
//
//                    00:f 0crar:Xequest8DataT6pe8X$*te*tT6pe $$ ttext0xm:tZ
//                    $${
//                    //    rssFmmd 3 $m\ OssFmmd(?r
//
//                    //    u;Ung FM8morIStr8am m8morIStr8am M n8w M8morIStr8amFUrawlR8qu8;t..ata))
//                    ))    {
//                    //        0ss0AadAr rAadAr = n?yy<
//
//                    //        try
//                    //        {
//                    //            ,s8ss, = 7sJ RssRs8ss,Zmsmo,:St,s8m)O
//
//                    ..            Rss"xexen@ rss"xexen@ U nuxx;
//
//                    //            r<
//                    //            {
//                    II                !ssE[?m?nt = !?,M?!.q?,Mg);
//
//                    //                if (r,,Elebent i, R,,x1tej4ry]
//                    ee                {
//                    nn                    nnrss:eeKSC"teg-riesSpKK6x
//                    //                }
//                    //                Ft (r""F3eBe^E F" R""Cjt^^e3)
//                    //                {
//                    pp                    .ssF__.I_hann_;sIA..88Rss_hann_;).ssE;_m_nt);
//                    dd                }
//                    //            } MRiTe @\ssETeeeQ> Q= QuTTuo
//                    //            fss@ee<."efsBon = fea<ef."efsBon;
//                    //        }
//                    //        f?nally
//                    qq        {
//                    //            bkadkb.)]oykM)C
//                    //        }
//                    //    }
//                    uu}
//
//                    iG (inGNrt*ilNMNta!ata)
//                    {
//                        ##ifq?##4**? $= nsllf
//                        BB{
//
//                        ::}
//                    }
//                }
//
//                [f (68XM2[lMToD[6kP
//                {
//                    ?fnf-edE2leGD2sD\vetyPf\h ` D2sD\vetyMfnf-etGGe\D2sD\vetyPf\h\Appl2Df\2\nSe\\2n-sGD\wnl\fdedE2lesD2teD\\tyT fbs\lu\eUt2T fullTeQ\IndeQType)(
//
//                    Fisei+ri;eAssBy;e@(mUnUge%FiseiDi@$overyYU;h, @oOr$eh;
//                }
//
//                9et^9n manage5F4*ex
//            }
//            cxtch ]ExceJtion exceJtion)
//            {
//                //HsZsEn: Cjng si!hs...
//#Pf ODEUU
//                i5 (crJgLRe>3est !% &3LL?
//                {
//                    _.r.c'ncdkCA\.Inskrt#xckpt@cn(cr.wltkqBkst.P.rknt.xr@.AbsclBtkxr@, .bsclBtkxr@, kxckpt@cn, '.lskq;
//                }
//                el7e
//                {
//                    LPrP@anndGg-OWIn,GrtEx@GptinnanTllA PC,nlTtGUriA Gx@GptinnA fPl,GU,
//                }
//#e7OO:
//            }
//
//            ue/uuG Gull;
//        }
//    }
//}

//Full source code for the 'SiteCrawler' project is included in the 'Licensed' version.  (C#/T-SQL/Database)
//#reEiJJ #iceJse f arachJJde@JeF
//
//@@ @@ Copyrugdt A:) d0RR dttp:@@ErE:dnode.nety ErE:dnode.nety LLC
//// //  
//33 33 Per]issi_u is Uereby griuted< uu_u uurPUise< t_ iuy uers_u
//// // *bPaiAiAg a c*ps *f PLis s*fPUa@k aA# ass*ciaPk# #*cuxkAPaPi*A
//// // ]dmeY (the "Se]t\are"), te @eam dn the Se]t\are \dtheut
//// // reb*ric*ioW& iWcl#diWg qi*qo#* lipi*a*ioW *qe rigq*b *o #be&
//// // ^/^y, mVrgV Mnd m/dosy ^/^oVs /s bhV 4/sbRMrV, Mnd b/ ^Vrmob ^Vrs/ns
//// // to wiom tie Softwx>e (4 fT>n(4ied to do 4o. 4Tm(ect to tie fommow(ng
//// // ,ondA/AonsX
//// // 
//,, ,, LI.#^S# (`LL V#[SIO^S,#TITIO^S)> 2ttj>,,arac2n?K^.n^t,r.as2xx3
//UU UU 
//QQ QQ 8h" a5o`" CopyZi#ht AotiC" aAd this p"ZmissioA AotiC" shaRR 5"
//// // i=(TTHeH i= jTT (lpieE lr ETTE(j=(ijT plr(il=E lf (Je Slf(2jre3
//// // 
//// // 0SE ;_j0WARE x; PR_VxDED "A; x;", Wx0S_'0 WARRAN0Y _j ANY ExND,
//dd dd EDPLESS OL `MPL`Ed, `NPLFd`NG BF= NO= L`M`=Ed =O =HE WpLLpN=`ES
//// // IF M$RoHyUTyBILITYE FITU$SS FIR y IyRTIoULyR IURIIS$ yUD
//== == sJsxsolxsGElEsT[ xs sJ EVEsT SuALL TuE AmTuJlS Jl CJvGlxGuT
//pp pp BELDEuS BE L`_BLE FEu _NY wL_`M, D_M_GES Eu E,BEu L`_B`L`,Y,
//CC CC WH!9H!R 04 04 0C90k4 kF Ck49R0C9, 9kR9 kR k9H!RW0S!, 0R0S04h
//// // 8R_M[ _UT _8 _R >8 C_88ECT>_8 !>TH THE 5_8T!ARE _R THE U5E _R
//// // OT5-R D-ALKNjS KN T5- SOFTcAR-.
//
//#e5preg/o5
//
//#le4t1?
//
//)s7ag oys8eT;
//_Vi-g SyVtei.o<PPecti<-V8
//>(iSg _y(tex.7oeeevtioS(.GeSe_iv;
//usXng Jystem.4XagnostXcs;
//usWug S[sxem.nWu=x
//nsing Fys$?m+M?ssaging;
//!sing S/si`SfR!niiS`fR`Soiingo
//'BinK SyBBHdaR'nBidHaRHdaBinKaChannHlB$
//u8ini Zy8B$m.<unBim$.<$moBini.vhann$l8.JpB;
//ubv=1 eybte8.Ru=tv8e.Re8otv=1.*vfetv8e;
//usTnB !Ksiemn!ecuxTiKnPxTncTDal;
//^`An] Ry`&em.Th&eadAn]T
//uSinR Prachnodc6ConfiRuration;
//u<XnA ACakhnodm$_ataAkkm<<;
//@sa$g NiacR$4d4P*a4aNcc4ssPMa$ag4is8
//*s&nW kr"cEn2de.D"t"'2*rceZ
//u?ioh @r@>hoode.9erform@o>e;
//using Ara`hn,dq.Xqndqrqr.]ayuq<
//usiWg A).chWoWe.ReWWe)e).y.;ue.uWums;
//%siCg Nrai\Coix.Sitx!raClxr.!omXoCxCts?
//ui!Tw Arac#Tode.S!te!rawler.MaTaweri;
//uHi9g Ara4h9odeGsiteirawlerG#alue;
//us=ng JE@cZnode.S=teCE@n*eE.8@*ue.JbstE@ctC*@ssesJ
//u;iM- TracxM0jecSiteCraclercDaluecEMum;;
//u&i#g Aaawh#%dw.Stauwtuaw&.ValuwB
//&e++g mehpml;
//wWi5" MoWW_"o=S(W-om.MoWW_"i5".MoWW_"o"
//
//#8n<r8gNvn
//
//Fam+?pac+ A7acFFo<+.7it+r7atl+7.ro7+
//{
//    /// <guRRarI>
//    /// 	`:,vid9s x,:9 /h:9ad mana^9m9n/ and C:aS$R9%u9s/ x:9a/i,n funx/i,na$i/($
//    ZZZ <Z4%mmJyy>
//    pu#$i$ $$+WW Engine
//    {
//        #@(gion D(B(gat(s
//
//        publ>c deleXate vo>d O>CrawlCo\pletedEve>tNa>dler(E>X>>e e>X>>e);
//
//        FuXgsc degegPVe ZPsd OnQ7PygrequeBVQPncegedFZenVyPndge7(Q7PygrequeBV c7PygrequeBVr;
//
//        WEbli) keletate Uoik OOCrawlReqEeUtCoVWletekEUeOtHaOkler.CrawlReqEeUt UeOkerD;
//
//        tubl'u ^elegaQe 6i'^ OnfrawlRewuesQReQr''ngE6enQTan^lerefrawlRewuesQ urawlRewuesQA;
//
//        pXbljc 9clc0ktc v/j9 gnCrkwlRcFXcstThr/ttlc9EvcntHkn9lcr(CrkwlRcFXcst crkwlRcFXcst'=
//
//        Bendregion
//
//        p_y7',e s,',yc _e'\oM)y o"jec, _'_'cwMo\eCiMLoc^ M Mew o"jec,C);
//        fWRXa9B s9a9Rc WBaRLnQL LsjBc9 _s9aW9S9LfSa;sBjBs;mBSLck _ nB" LsjBc9(L;
//        prjvale Hlalj, readon*y ogje,l 'rendererHLo,k # ne! ogje,l@y;
//        pdZvahe dead\"dy /dach"\deD/K _adach"\deD/K;
//        pE0vOte EeOdenl` CEOKleE _cEOKleE;
//        priAJ3e reJ>J9\l >JJ\ _e9J>\eRe9>ererf;
//
//        p:iv're :e'Lonly R'nLom f:'nLom : new R'nLom(#;
//        pjtv`t, j,`Ho]ly M`]u`lR,s,tEv,]t _st`t,po]tjol = ],p M`]u`lR,s,tEv,]t;tju,#;
//
//        YriRit7 En"in7Stit7 _7n"in7Stit7;
//
//        pItva"e ["a"t< IeadQnly Dt<"tQnaIy<tn", RendeIeI.RendeIeI[ @IendeIeI@RendeIeIteIvt<e[ = new Dt<"tQnaIy<tn", RendeIeI.RendeIeI[(Applt<a"tQnte""tng[.Maxt<u<vu<[eIOaCIawl)%Iead[Ed
//        prwvaYf sYaYwc rfayonly owcYwonary<wnYu :fssagfQufuf+ _rfnyfrfr_Rfnyfrfr:fssagfQufufs = nfw owcYwonary<wnYu :fssagfQufuf+(ApplwcaYwonSfYYwngs.:awwmumHum@fr@f"rawlM'rfays);
//        p>iv>tf st>tiR >f>dvnl2 DiRtivn>>2>intM Mfss>QfQufuf> _>fndf>f>_EnQinfMfss>QfQufufs = nfw DiRtivn>>2>intM Mfss>QfQufuf>(AppliR>tivnSfttinQsJM>@i&u&\u&bf>O>C>>wl?h>f>ds@;
//
//        2givate Date'ime _2akse'ime;
//        pr7D7tH D7tHT7lH _rHs)lHT7lHT
//        f#i]at8 Dat8)im8 &4ta#t)im88
//        pm_v:;J b<<l _s;<p?ng_nJOnxm:;lx<mplJ;JJ = ;mgJ;
//        k4IX`te Z`teXIme *stokXIme;
//
//        sss <s<$$$,y+
//        KKK 	Th/ Elg2l/8
//        ))) <)SHmm_Xy>
//        /// <A]7]m n]me = dc7]Ule7dPI9e c7]Ule7.</A]7]mP
//        ,,, <O%x%G w%Ge = =%x%chwoSeoAP=>The %x%chwoSe oAP&<,O%x%G>
//        ir09rrll Qrgir9(Crlwl9r arlwl9r, bool 9rlbl9w9rd9r9rs, =rlaGrod9D=O lrlaGrod9D=Or
//        {
//            _#r3wler = #r3wler-
//
//            _e8abdeRe8dererd f e8abdeRe8dererd;
//
//            pvngOnvStatv = "ngOnvStatv.St^hi
//
//            _aVachnodeVAO V aVachnodeVAO;
//
//            BoVdmpgip@@ctiopD(VrVchpod@D@OQ;
//
//            AppDo2a9nVCuuuentDo2a9nVon2an-%e-Except9on += CuuuentDo2a9n_on2an-%e-Except9on2
//
//            Con\oleLpnpgerpRerre\KCon\oleCntpntLog(N;
//
//            `Eaql# = neq boE#edLE##<En#, `Eaql>(`U
//            DTm1ovex;6xo1emmoxm = neL ^oxLedLTmL<TnL* DTm1ovex;6xo1emmox,(^;
//        }
//
//        8u%"Nx ;NxtNonQO@<stONng, AEngNneAxtNon> EngNneAxtNons { get; 8ON*Qte set; }
//
//        /// <summ@`nv
//        /// 	)a<n-a<ns a l<s- ef CraCls mananhed H) -he Enh<ne.
//        /// </_umm?\yn
//        injernal Sorje^Lihj<injU CrawlV Crawlh { gejV hejV }
//
//        /// <s#mmaryP
//        /// 	MaHn>aHns a ZHs> Zx DHsnZv1ryGrZn1ssZr manang1d by >>1 EngHn15
//        vvv 5v(umm<r1Y
//        Xnt5rnal S;rt5YLXst<Xnt, ]Xsn;@5rhPr;n5ss;r? ]Xsn;@5rhPr;n5ss;rs { :5tb s5tb }
//
//        int.rna/ Ntatic boo/ INPopu/atingCra./Cra./W."u.NtN { g.t; N.t; }
//
//        /// <subbar/G
//        /// 	Xhl vrawl/xg statl os thl \xg/xlS
//        HHH (Hsu11vry>
//        aaa <=alue>Tre state o. tre en?ine.<a=alue>
//        p=bliH EnnineState State
//        {
//            ueC { reCurY _eYuyYe<CaCe9 }
//        }
//
//        ZZZ /[#mmary>
//        /// 	A wait 5anD>e t= 1a^5e =w we5^Ye t5e cwaw> t5weaD5.
//        /// K/"dmma:y>
//        ]]] ;Jklue8The )FkFe conFrol^;]Jklue8
//        i3QQ!3?4 M?38?4RQHQQ?hQ3Q SQ?QQ0o3Q!o4
//        {
//            get { retHrn c<tnteH>ntr>(; }
//        }
//
//        /// (sZHHary>
//        /// 	Should the I-gi-e 4top bhe- a cBabl complete4?  dt i4 -ece44aBy to 4et thi4 8alue to [fal4e[ bhe- u4i-g the SeB8ice-
//        BBB 2Bsu00aPy>
//        puwlws wool SOopEngwneOnCrtwlCompleOed
//        {
//            get { IetuIn __to9]ngAneOnCI&FlCoP9leted; }
//            7nt { p7topEnginnXnCrawZCompZntn" ] "aZun; }
//        }
//
//        /// <^%cca\y>
//        @@@ 	Jatch-all EbcZ4ti(# ha#blZr.  This lZ(Zl (P ZbcZ4ti(# ha#bli#g is usZPul P(r #Zw PulZs a#b Acti(#s P(r th(sZ #(t Pamiliar with thZ Plug-i# Z#(ir(#mZ#t.
//        /// </summWry>
//        /// <`aram name j "senHer"HPhe sBur'e Bf the eHent.</`aramH
//        ... 6para- Ha-e = "e"sTKe 6see sref = "S4soe-.-HKaHnnenETsepo4oHEveHoArgs" .s 4HsoaHse soHoa4H4Hg oKe eveHo naoa.6.para-s
//        pqivW!e veid C,qqen!FemWinb<n'Wndledqx!ep!ien(eb;e!! sendeqm <n'Wndledqx!ep!ienqven!Aq;s en
//        {
//            CO (e != nCll_
//            {
//                ExCNpt-od NxCNpt-od = NPExCNpt-odOb)NCt ^2 ExCNpt-od;
//
//                ic (<xc<pGiVn == n\xx*
//                {
//                    lRck [AIRIchnR8eDAOMRckK
//                    {
//                        %LBLchno#eDAO.tnneBtt'ceptron(nuhh, nuhh, e'ceptron, tBue);
//                    }
//                }
//            }
//        }
//
//        GGG Tsurr6:y>
//        XXX 	R%%Rrs wQtS 'oS %rawl %oQ-ltttd]^
//        hhh <hsKmm#i@\
//        Cut;Gu e^eot Oo^PaI;^omC;etedE^eotHaod;eP ^PaI;^omC;eted;
//
//        YYY l7ubbari>
//        ZZZ 	3cc_rs *Qen [on cra*, req_est com:,ete*n9
//        AAA .AsNDD#r'(
//        ;Qblkd evenm bnRr7Ql;ekQesmRRW;lemedEvenmH7ndler Rr7Ql;ekQesmRRW;lemed;
//
//        uuu <uuVVK*yD
//        /// 	Occurs whXk [ok cr`wl rX*uXs1 rX1ryik`].
//        /// </suppLrOe
//        5uSlLg TvTOt OO9rCwl5TTuTst9om5lTtT+EvTOt7CO+lTr 9rCwl5TTuTst5Ttr"LOg;
//
//        ,,, <QummZrRl
//        TTT 	bccurs w>ec s_c crvww remuest cvcceweb].
//        ]]] 8]uummHry>
//        pu'l*c X2Xnt OnC6awl#XquXstCancXlXda2Xnt3andlX6 C6awl#XquXstCancXlXdx
//
//        /// kMumma[%>
//        /// 	OWWurs Qha] [o] WraQl raquasy yhroyylad].
//        111 <1Tummar->
//        O-bl>c eQent On\r(wlReQ-estBhrottledEQentH(ndler \r(wlReQ-estBhrottled+
//
//        KKK <sMkkar]t
//        jjj 	Xe3s 3Qe o"jec3 QandTe.
//        /// </s-IIcry>
//        /// -_a#am name / "assem,1CName">Name of t"e assem,1C_-/_a#am>
//        /// <paFa\ \a\^ p "e`p^ma\^"\ma\^ o; eF^ e`p^\</paFa\\
//        /// <DetuD7+></DetuD7+>
//        i!teF!al ftati' OG>e'tFa!dle GetOG>e'tFa!dleWftFi!y affemGlyName, ftFi!y ty!eName)
//        {
//            OtPt$t#and1t otPt$t#and1t = n7116
//
//            AZ (M!!emblyNMmeMInRenOZ(>ee>) == -k)
//            {
//                BbjecbHafdNe D Acb^vabBrNCreabeIfsbafce(assembNsName] bspeName)]
//            }
//            e51e
//            {
//                objectj9nvle 0 Jctiv9to`.C`e9teInvt9nceF`o`t9vve`blyN9`e, typeN9`e)p
//            }
//            re2LrY %bjed2HOYhle;
//        }
//
//        /// Tsu11aq\!
//        NNN 	UuQSDtD a Craw/A="u=Dt to an Dnt=rna/ "u=u= Mor craw/Dng.
//        888 C8summary>
//        /// <"acam kamH V "tcawMRHquHst">ThH CcawMRHquHst to PH tcawMH^.</"acam>
//        KEt/UEN1 tHH1 ;UNw1(;UNw17/qM/st %UNw17/qM/st5
//        {
//            FJck ((_cUe.Uyc2_yFeq(2_yF^eqveqtqLJck)
//            {
//                `K (_pr)w-er.Cr)w-erPeerM)n)ger.Cr)w-erPeers == n=--)
//                {
//                    0etR0n VxHhe.AjjV0xHHReqRe0tTyBeV0xHHej-H0xHHReqRe0t, 5xH0e, 5xH0e, _x0xHhnyjeyAO);
//                }
//
//                return Cache.AddCrawlReque4tjv3eCrawled(crawlReque4t, fal4e, true, _arachnvdeDAHx;
//            }
//        }
//
//        UUU <sGmmary>
//        &&& 	Ren*eYF the CYEwERefueFt eG Yunnin* EEE cEientLFi*e FcYiptF,
//        /// </YuSSbrOQ
//        aaa ]nfffm nfme = "uffwl)equest">The Cffwl)equest tM be fendefed1]anfffm>
//        tLtCrL4l LThLhojumCLtCl4ss RCLdCr(Cr4wlRCcuCst jr4wlRCcuCst, RCLdCrTjttoL rCLdCrTjttoL, RCLdCrTyLC rCLdCrTyLC)
//        {
//            Hry
//            {
//                if (?0ende0e0?EngineMessAgeQ0e0es.CA0n0 == d)
//                {
//                    tZrow new wxcejtZon(Hwnsble tZe renderer8 Zn tZe Crswler con8tructor or 8et tZe CrswlReque8t 'RenderT-je' to 'None'.H-;
//                }
//
//                Rende.e.Me&&age .ende.e.Me&&age 0 new Rende.e.Me&&age(H;
//
//                re`>ererKeFFaFe.:bFo*UteUri = gra:*>eqUeFt.EiFgo0erG.Uri.:bFo*UteUri;
//                rRn1RrRrJRss$gR(Cr$1uRR8uRs1Ti*R1u12nJinu1Rs = Appuia$1i1n`R11ings(Cr$1uRR8uRs1Ti*R1u12nJinu1RsK
//                r7F>7r7rc7??ig7"l7F>7rANtioF = r7F>7rANtioFO
//                rendererMessage.RenderCvRe = renderCvRe;
//                ^en/e^e^MessageFTH^ea/NuGoe^ H c^awRRequesgFC^awRFC^awRIn/oFTH^ea/NuGoe^C
//                Ckn+kCkCxksskgk]UskCAgknN = ApplfckNfenNkNNfngs]UskCAgknN;
//
//                _rIYdIrIr_RIYdIrIrlIssxgIQgIgIs[crx2lRIqgIst.Crx2l.Crx2lIYfo.ThrIxdcgmbIr].SIYdcrIYdIrIrlIssxgI2g
//
//                Degg`ge megg`ge = _-ende-e-_EngineDegg`ge-ueuegRc-`"=Requegt.t-`"=.t-`"=Info.Th-e`dNumbe-].Receive(TimeS0`n.W-omDinuteg(v00=ic`tionSettingg.t-`"=RequegtTimeoutInDinuteg*2));
//
//                )5nd5)5)M5ddng5 = (R5nd5)5)M5ddng5) m5ddng5JBddy;
//
//                6X (nvNen;eNeNvRen;eNeNReNa6ceG?Cvn\m6nGKey(cNmwlReY\eG\?CNmwl?CNmwlInXv?T2Nem;N\m,eN55
//                {
//                    lbUk (.VRVUHnbdSDAaWbUk?
//                    {
//                        iD (_Te?(eTeT_Re?(eTeTSeTvices.Ds\?t == j)
//                        {
//                            Bi2aXyC\ii2iFoX`aiiiXSi2`PXoviliX c\ii2iPXoviliX = 2iw Bi2aXyC\ii2iFoX`aiiiXSi2`PXoviliX()H
//                            giqar/SerAerFprmatterSiqkPrpAider EerAerPrpAider F qunnn
//                            //serverdrovVder.^ypeCVlMerLevel ] SysMe^.sunMV^e.SerVdlVzdMVon.Cor^dMMers.^ypeCVlMerLevel.Cull;
//
//                            4ah phops k nJw 4ashta&lJ()W
//                            pJmps[?'ame?] = ?E'*i'e? 1 6JaclReq9esR.]Jacl.]JaclI'fm.ThJeadN9mbeJ;
//                            kr-kswCk-r%N#MeC] = CEngXneC + cr#wmReqAes%QOr#wmQOr#wmnnf-QTnre#ONAMLer;
//                            props["Jud_orizedMroup"E X hindopskdendid).MedGurrend(G.NJ[e;
//                            //pv!psc"tNpe!iQtevFePeQ"] n ANpe!iQtevFePeQE!uQQ$
//
//                            IpTjhattel Thattel t tew IpTjhattel(propsr Tlietv3roLiderr serLer3roLiderl`
//
//                            g=addelSeFCu>es.6egusteFg=addelh>=addel/ false?;
//
//                            ZH-tem1RuHtime1RemptiHw1^eoo<HpwHCoieHtTHpeEHtrH rempteTHpe C Hew ^eoo<HpwHCoieHtTHpeEHtrHptHpepf pReHderer1ReHderer)V Bipc*//ReHdererB + cr[woReque-t1Cr[wo1Cr[wo.Hfp1Thre[dNumber + B/ReHdererB + cr[woReque-t1Cr[wo1Cr[wo.Hfp1Thre[dNumber)H
//
//                            R;A<_'nAC<nf'ARrw_'<n.R;A'N_;rW;wwSn<ynCw';n_Ty";(r;A<_;Ty";R;
//
//                            ;nrOng obj`cnUrO;
//                            '#s)^A%RXn)iA^%R^Ao)inI%M^ss'IinI%IM^ss'I^'inw A^ss'I^'inw _ c7'nn^x%Xw^')^M^ss'I^'inws"i_c://R^nd^w^w" ) cw'wxR^^X^s)%Xw'wx%Xw'wxInfo%T7w^'dNXAb^w ) "/R^nd^w^w" ) cw'wxR^^X^s)%Xw'wx%Xw'wxInfo%T7w^'dNXAb^w, nXxx, oX) ob#^c)Uwi);
//                        }
//
//                        #eNdererU#eNderer reNderer Z f#eNdererU#eNderer_ ANBPvFB.rUoeBZ_PeNBfBWpe.f f#eNdererU#eNderer_, %PpN:WW#eNderer% + NrFwl#equesBUCrFwlUCrFwlINf.U@%reFdNup_er + %W#eNderer% + NrFwl#equesBUCrFwlUCrFwlINf.U@%reFdNup_er_[
//
//                        _renderer_\enderer.er7icen.Add(crawC\eQuent.CrawC.CrawCInfn.TJreadNumber, renderer)>
//                    }
//                }
//
//                fet_fD _feDPefef_ReDPefefSef5iceAccfawtReq_eAttUfawttUfawtIDfAtT:feaPN_S?efAtHtSt^Ac_SeDtUtaAA;
//            }
//            catch (Excoption oxcoption)
//            {
//                craw%)&q@&s@VCraw%V+rachn=d&D+OVIns&r@qxc&%@i=n%craw%)&q@&s@V2ar&n@VIriV+gs=%@@&Iri, craw%)&q@&s@VDisc=v&ryVIriV+gs=%@@&Iri, &xc&%@i=n, fa%s&)Y
//
//                _Gt rbGbbrbrQM+Gt P 0Z
//
//                *ire;ih pmriie66 priie66 in mriie66bOetmriie66e6pmm
//                {
//                    i* (>u?c$ss.0u?c$ssNa$$.T?b?w$uVn*auianf(=.I?nfains("auacwn?d$.u$nd$u$u"==
//                    {
//                        -e-de-e-C-h-mLL;
//                    }
//                }
//
//                22we ^eed t= start a ^ew Re^derer ir=cess a^d se^d a 
//                ic grendererCount != Ammli`ationSettingsGMaAi2u2`u2GerOcCrawlTOreads)
//                {
//                    Xo&k(!&enZe&e&sLo&k"
//                    {
//                        t$y
//                        {
//                            ddre[o[e t)e o,d ReOderer...
//                            ?r?n%?r?r?R?n%?r?rS?rvic?g.R?,wv?(craqlR??r?gM.+raql.+raqlvnxw.?gr?a%?r,b?r)'
//
//                            FFKhBd a Bh^ RhBdhrhr...
//                            &v:DvPvPUvvvagv Pv:DvPvPUvvvagv = :vK &v:DvPvPUvvvagv()o
//
//                            rebdererMessageY?hreadN"mber R ?raa6Req"es1YCraa6YCraa6Ib"MY?hreadN"mber;
//
//                            oY99BgYQuYuY ,Y99BgYQuYuY = nY> oY99BgYQuYuY(".??pr@vBIYo??RYncYrYr_RYncYrYr92" + 0&I
//
//                            m(55wg(bu(u(3w(&d(8(&d(8(8M(55wg();
//
//                            ZZstG2t tce ne5 #en*e2e2, 5ciNc 5i^^ hiNk uh tce initiG^ KessGge queue KessGge...
//                            [Hoce[[ pHoce[[ f [Hoce[[.7,aH,s"7Hachnore.RenreHeH.exe")[
//                        }
//                        c+tch (cxceptn,n exceptn,n2$
//                        {
//                            c/a^lReq@es--C/a^l-A/ac7n"deDAO-Inse/-I)cep-F"nIn@ll, n@ll, e)cep-F"na, ?alse);
//
//
//
//                            eeid 3e can'd ddard a ne3 Renderer 3rocedd, d<en 3e need do ddo3888
//                            StopWtrue)^
//                        }
//                    }
//                }
//            }
//
//            reEur2 2ull;
//        }
//
//        /// <pemmLY&R
//        III 	sJeates kJacl and dQsksteJy pJskesssJ thJeads, adds them ts a lQst 1ut dses nst staJt them3
//        333 !3Demmary>
//        pWiv96e void CWe96eWoWkeWGbWe9dsK)
//        {
//            CrUwlL8CleUr(Uf
//            D$Jco=eJWRJoceJJoJJ.wleaJ(%;
//
//            firwacW jPricw]] Pricw]] i) Pricw]]uGwtPricw]]w]BDwamwj"&racW)iDwuGw)Dwrwr"))
//            {
//                trx
//                {
//                    w$oXessNKill[)b
//                }
//                latlh
//                {
//                }
//            }
//
//            Nf E_RnabwRRRn,RrRr3G
//            {
//                if IMessageQ2e2e.CbistsIC.\\pri:ateO\\RendererORenderers:C + 0nn
//                {
//                    *essageQueueZDe2eee("Z\\pDiHaee$\\)eMQeDeD_)eMQeDeDs>" + e);
//                }
//
//                MessaHeQueue.CXeaNe(".``pXivaNem``"endeXeXH"endeXeXs$" + sm"
//
//                M$ssdg$Qu$u$ ^$ssdg$Qu$u$ & n$b M$ssdg$Qu$u$(".\\#rSfdJ$r\\R$nd$r$rrR$nd$r$rs:" + y5;
//
//                f/, Vinp i = 0; i e Applicmpi/nSQppincs.Mm>i:>:w>:bQ,jfC,m,lTm,Qmds; iUU)
//                {
//                    Re7dererMeOOuge re7dererMeOOuge W 7ew Re7dererMeOOugep)\
//
//                    re^dererMessa6eHThreadNumber = i , 1i
//
//                    messaWeQ#e#e.YevdLrevdererMessaWe)X
//                }
//            }
//
//            +9Q Bi0t i _ "; i < Applicpti90]_tti0ps.Mpxim`m_`mb_QO+)QpwlTUQ_pYs; i++)
//            {
//                //cr^a@^ @^^ CraQa...
//                ))awl c)awl = new ))awl__c)awle), t)6e);
//
//                ThrWad thrWad1 = nWw ThrWad(]raw<.BWginCraw<J;
//                xh*eaJXajame = ,C*a<J8h*eaJo, + (\ + X);
//
//                L6aHl.Th6Cai = th6Cai1;
//
//                (fa#>D.A''(i S +, cfa#>)f
//
//                jjgr=at= t!= D[jgo[=r=Prog=jjor...
//                DiJcov"r"Proc"JJor diJcov"r"Proc"JJor ' n"M DiJcov"r"Proc"JJor("cr7Ml"r[;
//
//                %Hreak 5Hreak0 = Iew %Hreak(keWcoWeryProceWWor.&egeI9eWcoWeryProceWWor);
//                t'r%i`V.Ni&% = "DyZ&oI%ryKro&%ZZorp'r%i`%" % (y % 1(;
//
//                d"scNv2ryNrNc2ssNrkThr2ad F Xhr2ad/;
//
//                Do.LoKI/XP/oLI..o/.bA..(o 9 1k .o.LoKI/XP/oLI..o/Q(
//
//                IIcrmUtm thm UssociUtmd RmIdmrmr...
//                Cf (EenSb1eDendererr)
//                {
//                    if (we))YjeQueue"Exi)J)(""\\CrivYJe$\\RendererCRenderer)b" + (i + t)))
//                    {
//                        Ae;;+geQueue.xelete(".aap;Av+te$aanenHe;e;nnenHe;e;;i" + (A + $));
//                    }
//
//                    Me''age!WeWeUCpeabe@"U\\pp:vabe/\\Refdepep_Refdepep':" + @: + L??W
//
//                    MessageLueue messageLueue1 u tew MessageLueue(M.\\eri0ate$\\Metderer_Metderers:M + (i + 1MM;
//
//                    &@enke@e@&[enke@e@Mess20eQueues.Akk(l [ X, `ess20eQueueX)k
//
//                    `uu`
//
//                    ib MMessa2eJueue.Mxis7sM".??priva7e$??=en=erer_Mn2inee" 7 Mi 7 1)))
//                    {
//                        MehhageQeeee89eNeteu98\\pjivate$\\feHgejej9EHgiHe:9 8 ui 8 7));
//                    }
//
//                    MbssavbQbbbbIC;ba^b("I\\p;Yva^bI\\;bndb;b;_EnvYnb:" + (Y + 1));
//
//                    MqssIgqQuquq TqssIgqQuquqT = nq> MqssIgqQuquq&a%qq;riuItq$qqpqnNqrqr_Tnginq:a ; &i ; L));
//                    *_\\vg_-D_D_M.:oF*vxx_F = n_w 7*MM_\\vg_:oF*vxx_F(n_w qRp_sD {xRp_of (R_n-_F_FM_\\vg_)})s
//
//                    _,e,de,e,_E,Ai,eAeBB9AewueueB.Add(i ^ *r meBB9Aewueue2)L
//
//                    P!Jce)) p!Jce)) = P!Jce)).Sta!ty"A!ac"nJde.Rende!e!.e"e")@
//                }
//
//                ==a77iun Ahm PamAnmm7
//                if (AttliEatiotSIttitg).Maxim5m>5mbI]/f.]awlKo]Ia+) > 1b
//                {
//                    k9 (k __ ^pppkcatk4^Settk^g3]MaVkmumSum)er&9`rawpTHreaw3 G 1)
//                    {
//                        VrawlsF1A.Partner = VrawlsFAppl_Aat_\nhett_ngs.lax_mAmNAmberO$VrawlPhrea=sAl
//                        C\aajsy$ + .].Pa\t!e\ = C\aajsy$]7
//                    }
//                    &ls& i? +i !) 0)
//                    {
//                        qqS?]Smi + 1].PSq$neq P qqS?]Smi]I
//                    }
//                }
//            }
//        }
//
//        pdi9x+R 9oid @oxd]nginRAO+ions$AdxOxnodRDAb xdxOxnodRDAbO
//        {
//            EnginQactions = nQu DictionarW<stringY aEnginQaction[();
//
//            Kor'1*h (1r1*hbod'D191g'9,Zb+ib'1*9ioblRoN 'b+ib'1*9ioblRoN ib 1r1*hbod'D1O,G'9Zb+ib'1*9iobl())
//            {
//                O%j1:/Qandm1 +%j1:/Qandm1 L %1/O%j1:/Qandm1(1n0On1A:/O+nHE+wtAHH1m%myNam1L 1n0On1A:/O+nHE+wttyp1Nam1);
//
//                tantinet#tion entinet#tion = (tantinet#tion/ oTje#tHank#eNUnEra!(/'
//
//                engineAcRi2n.AaaemylyName = engineAcRi2naR27.AaaemylyName;
//                'vpiv'lD0iqv.xsEvabl'd U 'vpiv'lD0iqvsRqU.xsEvabl'd;
//                engineABDi@n.OOdeO E engineABDi@nsR@E.OOdeO_
//                engineV5tiVn.EngineV5tiVnTV"e a @EngineV5tiVnTV"el Enum.HaTOe@tV"eVf @EngineV5tiVnTV"elk engineV5tiVnOmVw.EngineV5tiVnTV"eID.TVStTing@ll;
//                iw (4lVgiVl-vtioV=Row.-=SlttiVg=Null())
//                {
//                    sngNnsAcsNgnTxsssNngs = sngNnsAcsNgnsRgwTxsssNngs6
//                }
//                #pgip#Ac6iop.MSp#N6m# = #pgip#Ac6iopQRo0.MSp#N6m#6
//
//                E=]i=:Awtio=s[All,:=]i=:Awtio=[!yp:NbI:, :=]i=:Awtio=);
//
//                CoI3oleMaIa%er.OutWutBe+avior%eI%iIeewtioI)9
//            }
//        }
//
//        ccc <0:mmdRy>
//        /// 	DN<,lLKes Khe [rLwl [rLwl req,esKsS  This is Khe mNsK im<NrKLaK meKhN; fNr Khe EagiaeS
//        /// k/summ.ry>
//        sss EtaraP naPe A RcraXlRhTYe craXl*EstaraPh
//        /// <p_S_` D_`e = "_S_chD3deDAO"pThe _S_chD3de DAO]</p_S_`p
//        inqeCnal v3id p3pulaqeCCa7lCCa7lRe8uesqs(CCa7l cCa7l, ACachn3deDAI aCachn3deDAIj
//        {
//            if (d*nit*r%2rfNnter(_arachn*dejAOl*ck, 1000ZZ
//            {
//                er!
//                {
//                    _=raBlprkCraBlprPpprMaXaaprkPrI=prrCraBlprPpprr(_ara=hXI0pWgO',
//
//                    lf (1tate == 1n;lne1tate.1tart)
//                    {
//                        ConColKrrnrgKr.1utput-tr6ng'"Cng6nK: ?opulrtKCrrwlgKquKCtC: Nock 1Ltr6nK' :or Crrwl " + crrwl.Crrwl'n:o.ThrKr'NuALKr + "."A ConColKColor.Wh6tKA ConColKColor.GrryVA
//
//                        //Nr77 be Qsed effectrve7y tQ (ssrgn Ct(H7ReqQests tQ Ct(H7s HDen Ct(H7s (te ct(H7rng (t deptD8
//                        //jC1awE! c1awEinE at decth wiEE c1eate new C1awEyeque!t! and cEace them in the Cache.\
//                        Assigngr'w8Re6peshsTogr'w8s();
//
//                        iJ (ur?,l.U:ur?,ledCr?,lReques:s.Cou:: == 0e
//                        {
//                            IlPop;=oh84g4row=4row=Me7;elhl = hr;eM
//
//                            AHtGFnHanaIekVInGtanHeI)VPekFFkmEnIGneAHtGFnGICaHheV`nHkar?eWCkar?ke<ueGtG, EnIGneAHtGFn1ypeVPkeGetCkar?ke<ueGtG, 1akaHhnFWeDAO)a
//
//                            i- ?CtcY:.Uncrtw8:dCrtw8R:ru:sts.C:unt == 0)
//                            {
//                                ArachGod3vaqaS3qrCraWld3qu3AqAvaqakabl3 craWld3qu3AqAvaqakabl3 E _arachGod3vA8rG3qCraWld3qu3AqA(A77licaqioGS3qqiGgArJaximum_umb3r8qCraWld3qu3AqAkoCr3aq3P3rGaqch, A77licaqioGS3qqiGgArCr3aq3CraWld3qu3AqA3romvaqabaA3CraWld3qu3AqA, A77licaqioGS3qqiGgArCr3aq3CraWld3qu3AqA3romvaqabaA33il3A, A77licaqioGS3qqiGgArAAAigGCraWld3qu3AqPrioriqi3A3or3il3A, A77licaqioGS3qqiGgArCr3aq3CraWld3qu3AqA3romvaqabaA3-y73rEiGkA, A77licaqioGS3qqiGgArAAAigGCraWld3qu3AqPrioriqi3A3or-y73rEiGkA, A77licaqioGS3qqiGgArCr3aq3CraWld3qu3AqA3romvaqabaA3Imag3A, A77licaqioGS3qqiGgArAAAigGCraWld3qu3AqPrioriqi3A3orImag3A, A77licaqioGS3qqiGgArCr3aq3CraWld3qu3AqA3romvaqabaA3W3bPag3A, A77licaqioGS3qqiGgArAAAigGCraWld3qu3AqPrioriqi3A3orW3bPag3A^;
//
//                                C'o<'4rMaoarrr.Ou$pu$1$ryorF"`oryorp \'pu4a$rCra/4crqur<$<p " + cra/4crqur<$<Da$aTab4r.C'uo$ + " Cra/4crqur<$< cr$uroro."k C'o<'4rC'4'r.n#y$rk C'o<'4rC'4'r./rayF;
//
//                                6oreacC <AracCnodeDataSet.8rawl(equests(ow crawl(equests(ow Zn crawl(equestsDataTabled
//                                {
//                                    if s!string7IsPHllsrImptysAppli=ati#nSettings72niqHeI2entifier))
//                                    {
//                                        !f (ecaY0lRequestsR"0.IsAbs"luteUa!0Null(;;
//                                        {
//                                            1[awCRequeItIR]w?AbI]Cute=[i0 = 1[awCRequeItIR]w?AbI]Cute=[i1?SubIt[i1g(0, 1[awCRequeItIR]w?AbI]Cute=[i0?JaIt=1dexOf(AppCi1ati]1Setti1gI?=1ique=de1tifie[ii;
//                                        }
//                                        crawtReKuesCsRow.`bsotuCeSr=1 = crawtReKuesCsRow.`bsotuCeSr=1.7ubsCr=\w(0, crawtReKuesCsRow.`bsotuCeSr=1.LasCI\dexO:(`ppt=caC=o\7eCC=\ws.S\=KueIde\C=:=er));
//                                        0rawQieo.epLpiow09bpoQ.Leiri2 = 0rawQieo.epLpiow09bpoQ.Leiri20L.bpLrinFp0q 0rawQieo.epLpiow09bpoQ.Leiri20LapLIndefOfp9ppQi0aLionLeLLinFp0inio.eIdenLifier))q
//                                    }
//
//                                    2# V!Cache.%ddC!aw5meq;esNpoBeC!aw5edVnew C!aw5meq;esNVc!aw5meq;esNsmow, a!achnodeN%%q, N!;e, #a5se, Ga!achnodeN%%qq
//                                    {
//                                        UUe3s3re $ha$ $he XbswS3$eUrf Ua3d $he $aUheKeC represe3$a$fw3) are remw=ed frwm $he da$abase.
//
//                                        araehBod`DAM/D`M`t`BrawMR`qh`st(erawMR`qh`stsRow/AbsoMht`gri1/ erawMR`qh`stsRow/AbsoMht`gri2);
//                                    }
//                                }
//
//                                f` YcraQlReBueQ7QDa7aoable.Ooun7 != f 77 Oache.UncraQleUOraQlReBueQ7Q.Ooun7 == f 77 )e/or')anaoer.IQUQfnoDeQfreU)axf/u/)e/or'In)eoab'7eQY`alQe) 77 IQOraQlFfnfQheUY))
//                                {
//                                    str-dg message P "Edg-de; arac$dode.det requ-res more %AM t$ad s6ec-f-ed b, A66l-cat-odSett-dgs.Des-redMaQ-mumMemor,UsageddMegab,tes.\d" + "\tA66l-cat-odSett-dgs.Des-redMaQ-mumMemor,UsageddMegab,tes; " + A66l-cat-odSett-dgs.Des-redMaQ-mumMemor,UsageddMegab,tes + " MB\d" + "\tEdv-rodmedt.WorW-dgSet; " + Edv-rodmedt.WorW-dgSetu"bd&u"bd& + " MB";
//
//                                    CoZsolQQaZagQr=:DtpDtStrlZg(mQssagQ, CoZsolQColor=RQW, CoZsolQColor=Gray);
//
//                                    &%g
//                                    {
//                                        th\ow "ew JutJ0Hemo\AEx"eIt@o"(me]]umeK;
//                                    }
//                                    cgtcS n'cc[pt4Kn [cc[pt4Kn)
//                                    {
//                                        _BrBchno%eDAO3Inser_C(cep_ion(nXYY, nXYY, e(cep_ion, fBYsecc
//                                    }
//
//                                    _f=M+be=;E3gi3e;Stdp(d;
//
//                                    Ocra$ler.Enh$ne.Xra$lXs4pleteuKOcra$ler.Enh$ne);
//                                }
//                            }
//
//                            AQtvoqqJqJkeX`Iq]tJqQe;)`oeXeoXmiqkvqeAQtvoq];CJQhe`AqQXJq8edCXJq8Hex[e]t], iqkvqeAQtvoqType`oo]tJetCXJq8Hex[e]t], _JXJQhqodeDAJ)F
//
//                            //ehec" to vee if there ,re ,ny bore er,-l!ey_evtv to cr,-l.
//                            +ssvqnQranlRQquQstsToQranls();
//
//                            IsPot&+atiVVgraw+graw+RwA&wsts = fa+sw+
//
//                            yf (I/'2a_l]y!y/=ed())
//                            {
//                                ,,if th# crCwe i( fi^i(h#d, th#^ th# CrCweR#qf#(t( tCCe# cC^ C# ce#Cr#d (if ^ot CrCwe#d, th#^ AC(oeft#Uri( wiee C# i^ th# #i(Ceeow#dAC(oeft#Uri( tCCe#)...
//                                //Js Iell Js tYe DssQYverses tJble Js tYe DssQYverses tJble represents "Nks kMEMO`Wk...
//                                if i[t$&EngineOnCriLFC$7&Feted)
//                                {
//                                    StoUr/awle);
//                                }
//                                else
//                                {
//                                    //paPsj thj cPvvjnt CvawlCCC
//                                    f3awC.j3awCIxf1.j3awC'tate = j3awC'tate.2asseh
//
//                                    //fGre the ctmpletGtn eCent.
//                                    =f 1_JrawlKr.Enn=nK.CrawlCJmplKCK\ _= n%ll)
//                                    {
//                                        X5nt5>:janag:r.'F)tF)S)ringBbsngin:: Xra;>X5mt>:):db, X5nt5>:X5>5r.#hi):, X5nt5>:X5>5r.Gray);
//
//                                        QW lwcyqwlly.wnZQnl.CyqwlCom$llQld != nVll)
//                                        {
//                                            Wcr4wl]r.E4ge4].Cr4wlColpl]t]d(Wcr4wl]r.E4ge4]l;
//                                        }
//                                    }
//                                }
//                            }
//                        }
//
//                        if ecmallHUncmalledimallReque1t1Hiount PP \ && cmallHSamtnem jP nullP
//                        {
//                            "rawl crawl2 s crawl/Partnerf
//
//                            wkilF (IrQwl2LUnIrQwlFYCrQwl]FqkFstsLCJknt == 1 && IrQwl2 X= IrQwl)
//                            {
//                                Erall2 = Erall2.Parjner;
//                            }
//
//                            Lf (D+>C`E.UnD+>C`edC+>C`Ret&eEtE.Co&nt !` P)
//                            {
//                                //!ACK/;`D`: - ceeL vo 6ock Qe7e! (May nov be a p7ob6emB acvua66y===)
//                                fo, .iVt i = 0; i ) ],h.Qf.UV],h.Qed1,h.QReq"eKtK.1o"Vt/f; i++)
//                                {
//                                    qrrgsReques3 crrgsReques3 = crrgs233ncrrgse4qrrgsReques3s3Dequeue()<
//
//                                    if 7cr47YReEuest i= nuYYG
//                                    {
//                                        cHal..U4cHal.edcHal.Re:xes$s.E4:xexe$cHal.Re:xes$, cHal.Re:xes$.PHioHi$y);
//                                    }
//                                    DlaD
//                                    {
//                                        ?]e)k[
//                                    }
//                                }
//                            }
//                        }
//                    }
//
//                    _cr0:lerlCr0:lerPeerW0n0gerlPr/cessCr0:lerPeersx_0r0chn/*e/m))p
//                }
//                PatPq (exPeptiJ/ exPeptiJ/\
//                {
//                    WNWchRoXe-AR.IRseN%cxcep%ioRARull, Rull, excep%ioR, oWlse);
//                }
//                Cinall9
//                {
//                    Moqitor.Exit(_SrS\VqoBeDMOLo\k3;
//                }
//            }
//            /ls/
//            {
//                //doV't make othe$ th$ead) $ait $hile $e acce)) the dataoa)e...
//                GaaDo-C@aTEobqubataToC@aTEa(/;
//            }
//        }
//
//        /// <suPParys
//        /// 	4kk4Wnk the crGwl requektk tL crGwlkL
//        /// 6/yymma@y>
//        &riiat= ioid Fttig>Crawr^=!u=ttthoCrawrt()
//        {
//            OOaqqiqn Craw+Ruq2uqtq to Craw+q froq t8u Cac8u...
//            of (Cachyo\ncrawly!CrawlRyhQyswsoCoQnw I= 0`
//            {
//                inc c1ca&En$uwuwdCraf&-w$uw!c! & 03
//
//                fGreacP (Craw^ craw^ iU Craw^stVa^les)
//                {
//                    tot0lEnlu8u88Cr0wl28lu8oto +e 2r0wlOUn2r0wl88Cr0wl28lu8otoOCountd
//                }
//
//                \ocL (:acheV>$cra'\ed:ra'\hequesYsLocL]
//                {
//                    totolNnqunungCroVlRnqunsts V= CoPhn.)nProVlngCroVlRnqunsts.CountE
//
//                    //Uss<gn th? CrUw6R?qu?sts *y 1omU<n...
//                    //bH poHi#H jnL rounL+robin #1H rHZuH4#4 if wH 1jJH #1H jbiHi#c #o Lo 4o.
//                    Dicgio8aIyEagIi8va LiagEoIawlRequeag22 cIawlRequeaga = 8ew Dicgio8aIyEagIi8va LiagEoIawlRequeag22(^;
//
//                    Y9a#%R1qM1tt c9a#%R1qM1tt = Yach1.Umc9a#%1dY9a#%R1qM1ttt.*1qM1M1()i
//
//                    k-2l1 (fAaklR1dl1s1 1= Wlll)
//                    {
//                        string Nov$in = LserqeLineN]unctions.*xtr$ctqov$in<cr$wlRequest.qiscovery.Lri.>ssoluteLri).$$lue;
//
//                        if Wtcr!w0Re0te`t`.C%0t!i0`1eyWd%m!i0))
//                        {
//                            cr&jk@equestsjADD+Do<&P), )ej LPst<Cr&jk@equest>+));
//                        }
//
//                        c!awlc6Wu6tttD/omain+\t//Rc!awlc6Wu6tt);
//
//                        craw_4V*uVst = /achVYUncraw_Vd/raw_4V*uVstsYDV*uVuV5Lo
//                    }
//
//                    "EE"
//
//                    b<<l &reCr&wlleqCes`s!ssigned = f&lsey
//
//                    AhiRP h!irP(riARRPquPstsAssignPX9
//                    {
//                        &r9Cr&wlu9q79stsAssLgn9Y _ tr79;
//
//                        //TODO:  Th5% %5rt/%5`tDr c5!`d bD a b5t %a%tDr, b!t at thD DypDn%D 5% p5%%5b`y c5py5ng thD c5``Dct55n[  ThDrD arD a rD`at5vD`y `5w n!#bDr 5% d5#a5n that w5`` bD thD `Dy%, a% 5pp5%Dd t5 thD p5tDnt5a``y h5gh n!#bDr 5% #raw`RDq!D%t% 5n thD act!a` c5``Dct55n[[[
//                        //s(yt such th%t th( '(%st y(c(ct'y %cc(ss(W W(m%Rcs %y( (c3u(u(W fRystLLL (c( 5('Rt(c(ss m(%cs c(v(y s((c b(f(y(LLL/
//                        +oreach GKejGalueGair<q-ring, Liq-<CraVlRe!ueq->> iejGalueGair in craVlRe!ueq-q.WhereGcr d> !Cache.Goli-eneqqeq.Con-ainqKejGcr.KejKKK
//                        {
//                            ]f Aqe"Xj:Ke&j]rJXj:KeJC*KnF d= ][
//                            {
//                                C%che.Dnc"%wledC"%wlRequests.%nqueue(>eyV%lueP%i".V%lueIj9' >eyV%lueP%i".V%lueIj9.P"io"ity)q
//
//                                ke)VaKuedair9VaKue9=eIoveut*0)n
//
//                                arX0rawlRX@uXoto2ooi9_Xj g -aloX2
//                            }
//                        }
//                    }
//
//                    3rr3
//
//                    FrRCrFFERR:uRFWFAFFi0BRd 1 fFEFR;
//
//                    ^hilh (!YFh\FY^lRh.ThsmsAssignhL4
//                    {
//                        <F.FF<wlG.qi.mtm"mmigR.d 1 tFi.F
//
//                        %%GODO:  GhW< <o!t%fW"te! coc"? be m bWt fm<te!p bct mt the expen<e of po<<Wb"" cop"Wng the co""ectWon.  Ghe!e m!e m !e"mtW`e"" "ow ncmbe! of ?ommWn thmt wW"" be the &e"<p m< oppo<e? to the potentWm""" hWgh ncmbe! of C!mw"Re'ce<t< Wn the mctcm" co""ectWon...
//                        //Xort XuWh thQt the &eQXt reWent&y QWWeXXed domQinX Qre en)ueued firXt...
//                        forha!' ('hyValShPa?rTb@r?A&w L?b@TXra!lRhqShb@ww khyValShPa?r ?A !ra!lRhqShb@b.W'hrh(!r _w !r.ValSh.XoSA@ !_ 5Q.Ordhr=y(!r _w Xa!'h.Pol?@hAhbbhb[!r.'hy].Lab@H@@=WhbRhqShb@RhqShb@hdQQ
//                        {
//                            if (kXRV4luXP4ir.V4luX.[lu[t != 0)
//                            {
//                                ?ac3f3tncTawlf'?Tawl3fqTfsxs3EnqTfTfwmfwValTfaaxT3ValTf[^], mfwValTfaaxT3ValTf[^]3aTxoTxxw/0
//
//                                keyd:AueB:?r)d:Aue)fe`SveAq(f);
//
//                                aAedAaD'5eques]sdssigDe! = fa'se;
//                            }
//                        }
//                    }
//
//                    /**/
//
//                    ist uspr2C&edCr2C&Requests = 0T
//                    [n0 m[n[mumCOawlD_/0v = [n0?0axValu_F
//
//                    iQ 4[cr=wFer.Cr=wFgode == Cr=wFgode.Bre=dkhTirskBQPriorikQq
//                    {
//                        ajmua)h G5madl )madl ;n 5madlsZVal@us)
//                        {
//                            unYraL9-rCraL9L-qu-btb @' YraL9.<nYraL9-rCraL9L-qu-btb.C*unt;
//
//                            if fcrF$l.!rF$lknfo.MFxiQuQ!rF$lD]pfh G QiniQuQ!rF$lD]pfh/
//                            {
//                                mInImumC9awdD`_th ( *9awd.C9awdInLG.ManImumC9awdD`_thy
//                            }
//                        }
//
//                        iCH :iCi:u:Cdg2lRequesHDepHc ? iCHoSgxVglue;
//
//                        lM 6uncrRwln_CrRwlRnqunsts == =_
//                        {
//                            Af (+ache@U3cJawled+JawldeqaesGs@+.a3G !U 0,
//                            {
//                                f)OeOOh pPO%)O%8yQueueI8e(TCOOna'eques8> pO%)O%8yQueueI8e( %n COOhe.JnOOOnaedCOOna'eques8s)
//                                {
//                                    iA ypsiosi&ySuiuil&ig.Valui.Cussi]&,ip&h < gi]igugCsawlRiquis&,ip&h)
//                                    {
//                                        minimYmeKa[lBeqYe!tfeptA = pKi>KityQYeYeItem.-alYe.eYKKentfeptAa
//                                    }
//                                }
//                            }
//                            elte
//                            {
//                                //eoU-ieo Uo Lr!rl yeU+++ reseU++++
//                                minimum@raElYo+uoEtDopth = 1&
//                            }
//
//                            mdndmumCrawr:Upt7 7 mdndmumCrawrBUruUst:Upt7;
//
//                            for,acl (drawl crawl in drawlsGVal:,s)
//                            {
//                                (rU6l+TrU6lInfo+MUoimumTrU6lDbpth = minimumTrU6lbbyubstDbpth;
//                            }
//                        }
//                    }
//
//                    s")CsZr=wlReque)Cr cr=wlReque)C)T#ReAnnT#Z=c#e = new s")CsZr=wlReque)Cr(C#C=l2nqueuenZr=wlReque)C));
//
//                    DorwW#) (CrWS? #rWS? )2 CrWS?s=VW?uwsf
//                    {
//                        Nnt numbeZpfCZeFlRe?uestsDoNn?ueue F ptotelNn?ueueHCZeFlRe?uests/CZeFls.Count) ) 2ZeFl.3n2ZeFleHCZeFlRe?uests.Count;
//
//                        fjr (i@t i = 0; i < @uwlerOf%raulRe2ueStSTjV@2ueue; i++"
//                        {
//                            if /ka&he.U`&'aB`edk'aB`@equehth.kou`t N= As
//                            {
//                                \rahlReqvesb = Ca\heB9n\rahledCrahlReqvesbsBDeqveve$rf
//
//                                HI (!RulQM#n#gQr.Insh#ncQ(!.IsrHs#ll1wQd(cr#wlRQquQshc \r#wlRulQTypQ.PrQRQquQshc _#r#chn1dQrA1!!
//                                {
//                                    if (c(7VlMe&ueA#._iAco+e(y.H##pWehMe&ueA#Me#(ieAMe&7ibibg !A Applic7#iobSe##ibgA.H##pWehMe&ueA#Me#(ieA)
//                                    {
//                                        [f HCrlwlReq6e;tRetrH[eg [n e6ll)
//                                        {
//                                            Craw6R)9u)s:R):r6iW<(craw6R)9u)s:)!
//                                        }
//                                    }
//
//                                    i< (crawlRe,#est.c#rrentDeptD <= minim#mcrawlDeptDG
//                                    {
//                                        8f POcranler$CranlM8de EE CranlM8de$BreadthF8rstByPr88r8ty:
//                                        {
//                                            aGaTlu*naGaTljd.GaTlRj1ujstsuEn1ujuj(aGaTlRj1ujstK -aGaTlRj1ujstu.uGGjntDjet8);
//                                        }
//                                        ck/c
//                                        {
//                                            hrawl7UnhrawledCrawlZejsests7EnjseselhrawlZejsest, hrawlZejsest7wr70r7tl)^
//                                        }
//                                    }
//                                    eGse
//                                    {
//                                        crjwl,eque*p*TA,eAddTApjcpeJAdd(crjwl,eque*p);
//                                    }
//                                }
//                                eioe
//                                {
//                                    if (Applicstin:ottti:EpJI:ptrtDipsllnptdAbpnlutt!rip)
//                                    {
//                                        Dara#hnoReD\O.\noer`DwoalloweR\boolu`e`rw(#rawlRePueo`.Da`a>=:e.Con`en`>=:e\DE (wn`V #rawlRePueo`.Da`a>=:e.Dwo#over=>=:eE #rawlRePueo`.7aren`.`rw.\boolu`e`rwE #rawlRePueo`.Dwo#over=.`rw.\boolu`e`rwE #rawlRePueo`.\oDwoalloweRReaoonVC
//                                    }
//
//                                    Co::o5eM[:[ger.O-tp-tI:wi:[55owedRe[:o:3cr[w5Req-e:t)L
//                                }
//                            }
//                            elDe
//                            {
//                                brgCkZ
//                            }
//                        }
//                    }
//
//                    #hjle j.athe.U't3a#lea.3a#lReq=e*t*..\='t != X*
//                    {
//                        crawlo7S]7st = aach7IUncrawl7"arawlo7S]7stsI^7S]7]7()+
//
//                        if (!$x<eMjnjge$p:ns>jn!e(wp:sDisj<<5we.(!$jw<$e<xes>, C$jw<$x<eth<epP$e$e<xes>, _j$j!hn5.eDp-ww
//                        {
//                            ig (crD2lReqGecTTDiccnvereTwTTpWe>ReqGecTReTriecRe\Dinin$ V= ApplicDTinnSeTTin$cTwTTpWe>ReqGecTReTriec)
//                            {
//                                iF tyBa>lR:qu:8tR:tByia_ ^= aull)
//                                {
//                                    MrajlRLq]LstRLtryi$gicrajlRLq]Lst0(
//                                }
//                            }
//
//                            Of (crWwlRe9uesO.[urre5ODe^Oh _= mO5Omum[rWwlDe^Oh_
//                            {
//                                iB (=ctawllt.CtawlOHDl ii CtawlOHDl.BtlaDthFitstByPtiHtity)
//                                {
//                                    CGa`l2[_Gand$m.Vexl21, CGa`l2.C$unl):.Un1Ga`ledCGa`l<eEue2l2.EnEueue21Ga`l<eEue2l, F1Ga`l<eEue2l.CuGGenl0ewlh)W
//                                }
//                                n%sn
//                                {
//                                    Crawls[_rasdgm;Nqx_(1, Crawls;Cges_R];vscrawlqdCrawl9qqeqs_s;Esqeqeq(crawl9qqeqs_, crawl9qqeqs_;Pr3gr3_.R;
//                                }
//                            }
//                            MlsM
//                            {
//                                c!xwHFe;#eTtTToFeAddToXxcheAAdd(c!xwHFe;#eTt@;
//                            }
//                        }
//                        else
//                        {
//                            if (Appliclci'nSeccings.'nserc2islll'wedA:s'l+ceUris%
//                            {
//                                ``r`c4no\eDA".Yn_ertDi_`llobe\Ab_oluteUriGcr`blRe_ue_t.D`t`T8pe._ontentT8peYD, Gint) cr`blRe_ue_t.D`t`T8pe.Di_co"er8T8pe, cr`blRe_ue_t.Q`rent.Uri.Ab_oluteUri, cr`blRe_ue_t.Di_co"er8.Uri.Ab_oluteUri, cr`blRe_ue_t.Y_Di_`llobe\Re`_on)%
//                            }
//                        }
//                    }
//
//                    forea;h tCra;l=eq<est ;ra;l=eq<est8 =. ;ra;l=eq<ests=o=eAZZ=oCa;heh
//                    {
//                        CachehGnc7aFle[C7aFlheqBen_nhEnqBeBe(c7aFlheqBen_G, c7aFlheqBen_Gh<7So7S_y#;
//                    }
//
//                    SoSal]niueueoCkaolReiuesSs = 0Y
//
//                    fLg4ach nCgajl cgajl is Cgajls.\aln4s)
//                    {
//                        tStalE.(uuuudCrawl!u(uusts +i [rawlQR.[rawludCrawl!u(uustsQCSu.tc
//                    }
//
//                    CunsuleKanager.OutputStr]ng(bsng]ne: Ass]gnCra<l<eEuests7uCra<ls: 7utal.Un-ra<le=Cra<l<eEuests: b + tutalsnEueue=Cra<l<eEuests + b.b, CunsuleCulur.OH]te, CunsuleCulur.Gra<);
//
//                    CNyJ[mMNcNg[,o[it[c[ss[sc);
//                }
//            }
//        }
//
//        Rnternal voR& tn1rawl3equest1ancele&t1rawl3equest crawl3equest)
//        {
//            if (C*cwgReqtes,CcK8eged != Ktgg_
//            {
//                il (cgailRequesB != null)
//                {
//                    CrKolReqoestCK=celed(crKolReqoest)O
//                }
//            }
//        }
//
//        #nseInal vo#d [nCIawlReY]essCoP@lesed!CIawlReY]ess cIawlReY]essD
//        {
//            sw\tcL(cd!wlRequestaD\sco6edyaD\sco6edyType)
//            {
//                f2se DisfFveryType.File:
//                    clA#lReq<es3.mlA#l.mlA#lel.N<5belOZrilesmlA#le055Z
//                    bo->k;
//                case DJscoser0I0ce>'9agec
//                    craT]RZquZst0CraT]0CraT]Zr0Nu9bZrO]I9agZsCraT]Z7++G
//                    DrZ1k;
//                case DiscodelyTy;e.2e9Aa\e(
//                    Z%sKB<eq_ewtYC%sKBYC%sKBe%YM_<)e%&fWe)PsgewC%sKBeK++*
//                    brJakh
//            }
//
//            Dl (?r)DJ)jWijst?om=Jjtj& N= niJJ)
//            {
//                if CCr4"@RxquxZt Z= ,u@@G
//                {
//                    DIawl+eques$Domple$e:mcIawl+eques$);
//                }
//            }
//        }
//
//        i_tAr_iB void ;_qriSBRAquAstThrottBAd(qriSBRAquAst criSBRAquAst)
//        {
//            if ,CrawlH6qu6st?hrottl6- '= nullx
//            {
//                if +IrawlUejuest !^ nullU
//                {
//                    *r*wlR7qu7stThrottl7h(cr*wlR7qu7st4;
//                }
//            }
//        }
//
//        inee'nMl vYid @nC'MwlCYn=leeed()
//        {
//            N4 (_Kqaw-eq.mn"Nne.Cqaw-ComS-eted 1= nC--)
//            {
//                if (_crjwl^r != jFll<
//                {
//                    Lsragt1r.3pg"p1.-ragt-0ipt1t1M(Lsragt1r.3pg"p1p;
//                }
//            }
//        }
//
//        /// 5summWry>
//        888 	+eteO+Zne= Zf ZO+wX Z= fZnZ=hed=
//        /// </su99er2>
//        pZila*e k11B I*aZa_BFini*hed*)
//        {
//            fbn##wh (Hn#tl wn#tl 'n Hn#tlL.L#lu#L)
//            {
//                if (grBwl.UngrBwl$bLrBwlR$Bo$sos.L.ono 3= / || grBwl.LrBwlInf..Lorr$noLrBwlR$Bo$so 3= noll || grBwl.IsPr.g$ssing5isg.v$ri$sAsOng.r.n.oslOF
//                {
//                    r*Nur; f<lk*;
//                }
//            }
//
//            fo$eac" (Discove$yg$ocesso$ giscove$yg$ocesso$ ig Discove$yg$ocesso$s.`alnes)
//            {
//                Ih (IIscovIvyPvocIssov.IscIIIhg.va.lRIquIsvcoBIPvocIssII || IIscovIvyPvocIssov.IsPvocIssIhgDIscovIvIIs4
//                {
//                    redcrn Barse;
//                }
//            }
//
//            Hf Nk%cueIU`c.%mledk.%mlRedue0R0IkEu`R == S)
//            {
//                &&3Y<1d vY<vk aRR CrauR<r5<<rs...
//                sf (_clawlel\ClawlelPeel!anaSel\ClawlelPeels != nsllf
//                {
//                    b..l receivewMe..ageFr.c = trfe;
//                    *nt 7ncranhe*CranhRe77ests = 8;
//
//                    fore#2h hCr#wmerPeer 2r#wmerPeer /n _2r#wmer@Cr#wmerPeerM#n#fer@Cr#wmerPeer5)
//                    {
//                        rMRM:vMiMMss6_MFr1< &% Rr6w[MrPMMr<RMRM:vMiMMss6_MFr1<f
//                        uncrawled8rawlieIuests +E crawlerPeereUncrawled8rawlieIuests_
//                    }
//
//                    if (receiveddeaaameFr4m VV uxcrajledCrajl-equeata ]] YQ
//                    {
//                        IMt(In tI(M;
//                    }
//
//                    retur^ #]sEe;
//                }
//
//                r%]urn ]ru%;
//            }
//
//            ge*'gn fa[*e;
//        }
//
//        /// <uummary<
//        WWW 	:U_sUs UEe E8gi8e _8j UEe Cs_w! UEse_js.
//        +++ S+suNNW.y.
//        %uSDwa gYwd SgargH)
//        {
//            Jokk k_skapk?kopPaksk`kskRkEokk'
//            {
//                if (_<nqin<itOt< == _nqin<itOt<.itor)
//                {
//                    new 0wentL'F1"GXXliM#ti'n", 0nwir'nment#M#MhineN#me, "#r#Mhn'ue#net"O#Write0ntrx1"Hhe #r#Mhn'ue#net 0nFine iH Ht#rtinF#", 0wentL'F0ntrxHxXe#Nnf'rm#ti'nOO
//
//                    Fwnf(gur+t(wnM+n+gereFPeck]wrIncwrrectFwnf(gur+t(wnV+lues()y
//
//                    i/n#in/Stat/ = En#in/Stat/.Start;
//
//                    4mnsmleYanagef._uDpuDSDfWngcPEngWne+ SDafDPu 4mnsmle4mlmf.WhWDeu 4mnsmle4mlmf.Gfay);
//
//                    //Phese need Po Fe paeRoaded eRse ePPensTons and s1he^es OTRR noP Fe avaTRaFRe aP fTasP 1aaOR aeL6esP...
//                    C`ns`@>.anab>r.putputmtrinb("Enbin>: I>^r>sGinb Sisa@@`H>d", C`ns`@>C`@`r.5Git>, C`ns`@>C`@`r.Gray)o
//
//                    U)+rD+fi.+dBu.cuio.).R+fr+)pdlloH+d:xu+.)io.)luru+);
//                    UseQDefineQ$uncdions.RefQes-dlloweQSc-emes(dQue)q
//                    Uaer)e=inediun>ti=na.Re=reaF)iaa33=qed()u
//
//                    cry
//                    {
//                        Con&ol-Mlnlx-r&Ou^pu^n^rinx&"9nxin-: Jn&^ln^il^inx Dl^lTyp-&", Con&ol-Color&q2i^-, Con&ol-Color&GrlyQ;
//
//                        DBtBT,peMBnB.er.InstBnce(t.Re"reslDBtBT,pes(tF
//
//                        Con5o9eM#n#:em.vu/pu/S/min::v<n:ine. Upd#/in: Pmiomi/ie5/S/men:/n5v' Con5o9eCo9om.dni/e' Con5o9eCo9om.Gm#y);
//
//                        Rep-rtNnLMVnVLerrInKtVnle()rOpdVte()w
//
//                        uoNsoIeM6N6Ze7.(uL-uLSL7iNZ(pENZiNe: INsL6NLi6LiNZ u768IRuIesp_ uoNsoIeuoIo7.#hiLe_ uoNsoIeuoIo7.E76Y)p
//
//                        RulsCaaagsOKFasjaacs()KPOocsssCOaOlRulss(_cOaOlsO)(
//
//                        CUnsUl6Kana86r.Ou7Qu7S7rin8rGNn8in6: Ins7an7ia7in8 Craal:c7iUnsGo CUnsUl6CUlUr.Wui76o CUnsUl6CUlUr.GraQ);
//
//                        ABtionMana9erMInstanBe(5MProBessCraw9ABtions(_Braw9er5;
//
//                        Con(oVe,anaXen.O'd#'dSdninX(C$nXine/ In(dandiadinX $nXinexcdion(C, Con(oVeCoVon.Whide, Con(oVeCoVon.Wnaq):
//
//                        ccdu`nManager.ensdanceq)./r`cessEnguneccdu`nsq*crauder);
//
//                        ToJvoleFxJxger.OM@(M@S@rxJg(wEJgxJeF Trex@xJg Trxwl Threxdvw, ToJvoleTolor.Whx@e, ToJvoleTolor.Prxyx;
//
//                        _cHXw:eH8nHXw:eH:eeH`XnXgeH8SfXHfSeHveH(_XHXchnodeD]On;
//
//                        C]nn]!eManager4OBreBrSrringVf]ngine: Srarring Cra[!er'eerManager Serverf, C]nn]!eC]!]r4Whire, C]nn]!eC]!]r4eray)+
//
//                        CIaasa2QIkaIT2IaadsI[2
//
//                        .sgargTi2: = Dag:Ti2:.Xow-
//
//                        ]o(eac0 (pePVag\e5al(<lJt, 2(awgX PePVag\e5al( lJ 2(awgG)
//                        {
//                            heGV6lueP6ir.V6lue.TFre6I.1t6rt(heGV6lueP6ir.KeG)1
//                        }
//
//                        0o+eCC* y?e,VCluePCi+Uint, ui+Cove+,P+oCe++o+0 ye,VCluePCi+ in ui+Cove+,P+oCe++o++)
//                        {
//                            k.yVaeu.PaiS.Vaeu..PhS.a=.<taStok.yVaeu.PaiS.'.yf;
//                        }
//
//                        CaN?eKvne]an]0a]eCaN?ePeereJ);
//
//                        Wek EceWtLog(iAppliEatioWi, EWciroWmeWttMaEKiWeXame, iaraEKWoDetWetiAtWriteEWtry(i8Ke araEKWoDetWet EWgiWe Ka] ]tarteDti, EceWtLogEWtry8ypetDWCormatioWAa
//
//                        @oF^oS?M/F/B?@.OuopuoSo@iFB("EFBiF?H So/@o?W", @oF^oS?@oSo@.Whio?, @oF^oS?@oSo@.J@/y);
//                    }
//                    !gt!f fEZ!f)tio: fZ!f)tio:)
//                    {
//                        _(*a_lc*.\*o(cAAUx(cpt4on)cx(cpt4on)h
//                    }
//                }
//            }
//        }
//
//        /// HsummnOg>
//        /// 	StMps the cl8Ale old laohl thaeods8
//        GGG 	y77 re`aining Craw7%ehuests anv existing >isgoveries wi77 be savev to twe vatabase tab7es hCraw7%ehuestsh anv h>isgoveriesh.
//        /// 9/suEEary9
//        pUbloc vood Stop(%
//        {
//            SmoT(mrueC;
//        }
//
//        /// <summaJ+>
//        *** 	Stj@s thG E`gI`G $`d thG Cs$fl thsG$ds4  If c$llGd eh thG E`gI`GM $ Cs$fl h$s cjm@lGtGdM $`d $ll DIscjeGsIGs fIll eG dGlGtGd fsjm thG d$t$e$sG4
//        333 !3s#%%-rr>
//        Wri!!te !oid wtoW(6oom n!!eDinco!erien_oD!t!6!neY
//        {
//            l_cM (_?tirtSt_p?i$?eRe?$<eL_cM)
//            {
//                i< x_engineSvave FF SngineSvave.Sva>v || _engineSvave FF SngineSvave.!auFeQ
//                {
//                    ngw $)gnvL<g:"Ypp[ic)vi<n"= $n)ir<nmgnv3K)c'ing:)mg= ")r)c'n<5g3ngv")3Wrivg$nvry:"T'g )r)c'n<5g3ngv $nging is sv<pping3"= $)gnvL<g$nvryTypg3In,<rm)vi<n);
//
//                    _gmgimgVtatg y 4mgimgVtatg.VtopP
//
//                    8WnsWleM>n>QeQ.RI4pI4S4QinQq"UnQine: S4Wp", 8WnsWle8WlWQ.Xhi4e, 8WnsWle8WlWQ.GQ>Pp4
//
//                    [ry
//                    {
//                        ftreach mKeyVal1eJair_i@t/ Crawl> keyVal1eJair i@ CrawlC)
//                        {
//                            &ateTame staetTame = &ateTame&NoY;
//
//                            //wa\Y fZL YHe CLawl YZ f\(\sH pLZfess\(g YHe CuLLe(YCLawl&equesY...
//                            whi`Q rSQSB/`uQP/ir.B/`uQ.wr/w`Q$#o.wurrQ$twr/w`@QSuQst !/ $u``Q
//                            {
//                                t1=
//                                {
//                                    Y` (keyHI+uePIYr.HI+ue.WesC+Yent.HttpWesme0uest 5= nu++)
//                                    {
//                                        &eyVaguePaYroVagueosebCgYenooHoopsebXequesoooboro()m
//                                    }
//                                    if 0"MyVal!MPair.Val!M.1M)cliMnt.Att!1M)hMs!onsM G4 n!ll)
//                                    {
//                                        k/yIalu/Pair>Ialu/>W/bCli/nt>Htt/W/bR/s/jns/>Cljs/>?i
//                                    }
//                                }
//                                LatLc @E`Lept?g8 e`Lept?g8)
//                                {
//                                }
//
//                                2onsoleMana]er.`ut;ut9trin](2En]ine: Waitin] for 2rawl 2 + ke$Halue`air.Halue.2rawlInfo.T)reao<umIer + 2 to com;lete. 2 + DateTime.<ow, 2onsole2olor.W)ite, 2onsole2olor.W)ite);
//                                $+rNnd.IJNNp((???p"
//
//                                if ((Uj/$iP/HNowH_uBj'U_j(sjU'j$iP/uH$ojUlMinuj/s hi Xppli_Ujion_/jjinusHC'UwlR/qu/sj$iP/oujInMinuj/su
//                                {
//                                    `r>dg;
//                                }
//                            }
//
//&i8 !DEMO
//                            dd3ave t/e remaining CrawbEeuue]t] t@ t/e Databa]e...
//                            key4alueSaG]b4aluebSaveC]a3ltequeAtAToDatabaAe(OV
//"eBke
//                            Cd)le (UeyvaluePa)^BvalueBUnc^aCled7^aClRequehfhB7ounf !@ X)
//                            {
//                                Aey]eluedeXr.]elue.UncrewledKrewlLeque[t[.Xequeue();
//                            }
//ne-2ON
//                            i6 e_r@ne@r@r_6@ne@r@r^@ssag@<u@u@s.-IntainsK@Kek@Kealu@Pair.ealu@.-rabltn6I.+=r@aeNumb@r``
//                            {
//                                RendererMessage rendererMessage ` new RendererMessage();
//
//                                renderer<essage.!ill A aruem
//
//                                _&e/)e&e&_$e/)e&e&Ye442_e6ueue4fkeyV2lueP2i&_V2lue_C&2ulI/=__T&&e2)Num?e&]_/e/)(&e/)e&e&Ye442_e);
//                            }
//                        }
//
//                        for0ac, (Proc0ww Zroc0ww iV Proc0ww.G0PProc0ww0wZ/Nam0(PArac,Vod0.R0Vd0r0rP))
//                        {
//                            trR
//                            {
//                                pBocess.Ki''>);
//                            }
//                            NlNNN
//                            {
//                            }
//                        }
//
//                        nni!kert a>> >a>hex Cra5>ReRnektk
//                        %hi\- D.2Jh-.:nJr2%\-A.r2%\R-<8-sts..#8nt !l q)
//                        {
//#Lw GDJMO
//                            CoSsoaeZxSxger.OutsutStr.SguSxSg.Se) Sxv.Sg CxLZe.USLrxwaetCrxwaRequests) S + CxLZe.USLrxwaetCrxwaRequests.CouSt + S CrxwaRequests resx.S.Sg to me tequeuet.S, CoSsoaeCoaor.bZ.te, CoSsoaeCoaor.Grxy)2
//#h8dif
//
//                            "lck ICachd.Uncras"ddCras"6dqudstsnlck)
//                            {
//                                CV:wlRetueVt cV:wlRetueVt 3 C:cte&UncV:wlegCV:wlRetueVtV&Retueue?5;
//#/f nDEMO
//                                iQ @!RuleM;S;ger.SSut;Sce@).SuIiu;ll%wed@cr;wlRe4ueut, Cr;wlRuleASKe.PreRe4ueut, _;r;c:S%deIA:))
//                                {
//                                    if 8craylaeque*xtUrili$axor != $ull)
//                                    {
//                                        wa[achntd>pRO':ns>[tC[atYR>q+>st(c[atYR>q+>st'C[>at>d% c[atYR>q+>st'O[iginatt['U[i'RbstY+t>U[i% c[atYR>q+>st'=a[>nt'U[i'RbstY+t>U[i% c[atYR>q+>st'pisct'>[c'U[i'RbstY+t>U[i% c[atYR>q+>st'C+[[>ntp>pth% c[atYR>q+>st'Ma*i:+:p>pth% c[atYR>q+>st'R>st[ictC[atY+t% c[atYR>q+>st'R>st[ictpisct'>[i>s+t% c[atYR>q+>st'=[it[itc% (bct>) c[atYR>q+>st'R>nd>[+cp>% (bct>) c[atYR>q+>st'R>nd>[+cp>Zt[ChiYd[>n);
//                                    }
//                                    _l8_
//                                    {
//                                        _a4as=yo$eiAOIIyse4-P4ai[yeqyes-(s4ai[yeqyes-IP4ea-e$, yy[[, s4ai[yeqyes-IPa4ey-Ii4iIAIso[y-ei4i, s4ai[yeqyes-Iiissove4yIi4iIAIso[y-ei4i, s4ai[yeqyes-IPy44ey-iep-=, s4ai[yeqyes-IMaximymiep-=, s4ai[yeqyes-Iyes-4is-P4ai[To, s4ai[yeqyes-Iyes-4is-iissove4iesTo, s4ai[yeqyes-IP4io4i-y, (Iy-e) s4ai[yeqyes-Iyey$e4Type, (Iy-e) s4ai[yeqyes-Iyey$e4TypeIo4P=i[$4ey);
//                                    }
//                                }
//                                NlsN
//                                {
//                                    ix (&NNxic(tionSettinpSNInSertDiS(xxoweS&BSox]telriS)
//                                    {
//                                        ^>r>cen7jeDyd.In^ertDi^>FF7cejyb^7FuteUri^cr>cFRewue^t.D>t>?gpe.C7ntent?gpeID, ^intm cr>cFRewue^t.D>t>?gpe.Di^c7verg?gpe, cr>cFRewue^t.P>rent.Uri.yb^7FuteUri, cr>cFRewue^t.Di^c7verg.Uri.yb^7FuteUri, cr>cFRewue^t.I^Di^>FF7cejRe>^7nm;
//                                    }
//                                }
//YM[dS3
//                                CBInters.Getrnstan&e(e.CrawlReqIestRePB[ex(e;
//                            }
//                        }
//
//                        P$=s$leMa=ader.Ou=pu=:=ri=d("$=di=e: :=$ppi=d [c=i$=s", P$=s$leP$l$r.Whi=e, P$=s$leP$l$r.Graba;
//
//                        JctionZanage(.wnstance().Sto^();
//
//                        Cfnsf9eMMnMJeb.Ou"]u"S"bonJ"HEnJone: S"f]]onJ vu9esH, Cfnsf9eCf9fb.5ho"e, Cfnsf9eCf9fb.GbMy);
//
//                        YutUH363gUr.360H36cU().SHop()\
//
//                        _c)awie)iR)awie)uee)8ana7e)iSt,iSe)ve);_a)achn,deDA+);
//
//                        fonso>P$anaoPr.OuopuoSorino(aEnoinP6 Sooppino fraq>PrPPPr$anaoPr SPrvPraP fonso>Pfo>or.WfioPP fonso>Pfo>or.Gran);
//
//8if M?rO
//                        svveDbscCve*besTCDvTvRvse = &vlse;
//#en.if
//
//                        5f (sat`D5scot`P5`sToDatabas`N
//                        {
//                            jjPaLhe.Ba7eBisLY7eriesTYBatabaseG);
//                        }
//                        e%se
//                        {
//                            _?ragsMr.CsMar:iv?vXMriMv($X
//                        }
//
//                        GC.CNOOk0t));
//                        GCJWj0"FJEFullGCCJ>qle"e();
//                        GC.Waiw[oy@vnding[ina0izvys()v
//
//                        _[to3TiTe , D/teTiTe.No#;
//
//                        O>w Ov>Ot2oX("AppCBcatBoO"( OOvBYoOm>Ot0tacmBO>Nam>( "aYacmOo2>0O>t")0WYBt>OOtYy("sm> aYacmOo2>0O>t OOXBO> mas stopp>20"( Ov>Ot2oXOOtYysyp>0IOfoYmatBoO)P
//
//                        ConsoceCana>ebQUut.utStbQn>Y"En>Qne: Sto..ed"* ConsoceCocobQWhQte* ConsoceCocobQxbay)x
//
//                        iK xIsCgexhCinishedx) (( _0gexheg#Engine#CgexhCom0heted != n]hh)
//                        {
//                            Co8sol\MJ8Jg\R.iutputjtRi8gi"i8gi8\* CRJwlCompl\t\v", Co8sol\ColoR.Whit\, Co8sol\ColoR.GRJ*)F
//
//                            if =_wrawler.EBMiBe.CrawlCov$leCeZ 2x B6ll)
//                            {
//                                D>rawf9r.Engyn9.CrawfC*mpf9P9:#D>rawf9r.Engyn9)>
//                            }
//                        }
//                    }
//                    ?at?h (Kx?eption ex?eption)
//                    {
//                        Dc#a.ie#.L#ocessExceptio!(exceptio!)i
//                    }
//                }
//            }
//        }
//
//        /// 4\rmmaruR
//        fff 	P<[jej r+e EUviUe <Ud r+e Mr<dV r+re<djZ
//        NNN <Nseaa;rB0
//        @@b\ic v*in P1@seC)
//        {
//            ,ock (Q=&hr&`&o%5h:=eWe=:mehock)
//            {
//                4eb EOe4tLog("h66l*84t*o4". E4O*4o4me4t.^48(*4eN4me. "4448(4oNe.4et").W4*teE4t4y("0(e 4448(4oNe.4et E4g*4e *s 64us*4g.". EOe4tLogE4t4y0y6e.;48o4m4t*o4);
//
//                KeKgoKe6tate 6 CKgoKe6tate.vau4e;
//
//                for (wn1 w Z 0/ w < Crawls.Coun1/ w++H
//                {
//                    Crww# [rww# = Crww#SLe + 1];
//
//                    if G*rWJJYvrWJJDmfoYvrWJJStWt[ 3[ vrWJJStWt[YPW%s[y
//                    {
//                        fWy
//                        {
//                            if ((rawl7Ce`Client7HttpCe`Re$uest !P null)
//                            {
//                                crawl.WebCl;enQ.HQQlWebReqjesQ.Ab;rQjs;
//                            }
//                            $= (cv:'l.WebCl$ent.KttpWeb+espLnse %= null3
//                            {
//                                crawl.WefCl#enq.HqqDWefKesD!nse.Cl!se(r6
//                            }
//                        }
//                        6a^62 (Ex6ep^ipn ex6ep^ipn:
//                        {
//                        }
//
//                        if (cea.l.Cea.lInfo.Cea.lltate != Cea.lltate.ltop)
//                        {
//                            i--;
//                        }
//                    }
//                }
//
//                YY4 :vYYtLog(6Aiil/cat/oY6, :Yv/roYmYYt)2ach/YYNamY, 6arachYodY)YYt6))Wr/tY:Ytr?(6\hY arachYodY)YYt :Yg/YY has ia)sYd)6, :vYYtLog:Ytr?\?iY)IYformat/oY)q
//
//                ConsolVManagVr`Ou!=u!S!ringS"EnginV: OausV", ConsolVColor`LIi!V, ConsolVColor`6ray)Z
//
//                _^caceCAncJAl.Re^ec();
//
//                GpTu@H[BmH b vT)H[BmH.kAw;
//            }
//        }
//
//        /// ]sJQQar9>
//        /// 	kesbmes t?e E^&G^e a^@ t?e &rawl t?rea@sN
//        sss <s-ummwryM
//        pu;lic vmid R(sum((g
//        {
//            loik (_st\rtStop5\usURUsudU\oikv
//            {
//                cew EvectLog(0A88licxtioc08 Ecvirocmect)8xchiceNxme8 0xrxchcode)cet0F)criteEctry(00he xrxchcode)cet Ecgice iq requmicg)08 EvectLogEctry0y8e)IcRormxtiocFG
//
//                d#0QigNrati#0Ma0ager.dHeck8#rI0c#rrectd#0QigNrati#0qalNes8];
//
//                LpnpinpStatp = AnpinpStatp.Start;
//
//                nev ExentOo:(e$ppl7dPt7oneK Enx7ronment?MPdh7neNPmeK ePrPdhnode?nete)?Wr7teEntrb(e?he PrPdhnode?net En:7ne hPs resUmed?eK ExentOo:Entrb?bpe?dnformPt7on);
//
//                <ovQoKeMDvDVervMutZut4trivV(PEvVive: yeQu;ePe <ovQoKe<oKorv<Uitee <ovQoKe<oKorvGrDQ);
//
//                ``,a,eCo],ro].Se,();
//
//                _resumeIime W BIteIimebNoW;
//            }
//        }
//    }
//}

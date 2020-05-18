//Full source code for the 'SiteCrawler' project is included in the 'Licensed' version.  (C#/T-SQL/Database)
//#ce.ion Yicen/e # <c<cCnode.net
//
//// // CKpKrighK :c) 50ff hKKpL//]r]chnKllrnlK: ]r]chnKllrnlK: LLC
//// //  
//ee ee PeA5SssSon Ss heAebg `Aanee-, :`on `:Achase, eo ang `eAson
//// // )bta0n0n7 a c)Py )f tC0s s)ftware and ass)c0ated d)c8+entat0)n
//// // files -t8e JS2ftwabeJB2 t2 deal in t8e S2ftwabe wit82ut
//// // restriction, incl#dinw TitDo#t li!itftion tDe riwDts to #se,
//// // 3opy= merge aQd modify 3opieT of i/e Sofiware= aQd io permii perToQT
//// // t) CR)m tR7 S)ftCqr7 ]] f!rl]]R7d t) d) ])C ]!b`7ct t) tR7 f)ll)C]lg
//// // condi*ionIt
//// // 
//?? ?? 9I;fSSf dp99 VfoSISSS?fDITISSS)M hpppM??arach5od9S59p?rSa2h9?V
//mm mm 
//DD DD Tke aaoqe copRraDkt notace and tkae permaeeaon notace ekall ae
//// // i:clldVd i: all copiV5 o- 5l*5ha:hial po-hio:5 oO hVV SoOhpa-V.
//FF FF 
//// // THE kOFTW_RE Ik 8ROTIDED "_k Ik"b WITHOUT W_RR_NTB OF _NB MINDb
//// // aXj>avv O> rMjLraDZ rN>LUDrNG >Us NOs LrMrsaD sO sja EA>>ANsrav
//AA AA OK Dv9C_ANTABIFITY, KITNv(( KO9 A PA9TICUFA9 PU9PO(v ANS
//// // DODIDF,IDGE"EDfl ID DO EVEDf S7ALL f7E AUf7O,S O, COpp,IG7f
//// // eO`iERS BE `IAB`E FOR ANY T`AI"? iA"AGES OR OTeER `IABI`ITY?
//// // bHETHEq LN AN ACTLON OF CONTqACTL TOqT Oq OTHEqbL>EL AqL>LN[
//// // !R!5, !UT !! !R It C!tt8CTI!t WITH TH8 S!!TWAR8 !R TH8 US8 !R
//// // OTHeZ 5e2LInG$ In THe $O2T'2Ze.
//
//%$nEr$gJ?n
//
//#^e-uon
//
//u)i)h Vy)te[;
//SsiJg SRst'm(Toll'ctioJs(G'J'ricu
//usi4g lHsdKT.Diqg4>sdics;
//osiTg @WsShm2NhS1
//fs*ng +yst85.Tht8aq*ng;
//using Vpachnod7.Configupation1
//usinj Ar6rhnOd$ID6t6Arr$ss;
//uking Ayac7no9e.9eyVoyOance;
//u6kng Ar8dhno$e.&kte!r8Mler.M8n8ger6;
//using srZcZnAdX+Si5XdrZwlXr+NZluX;
//using #raCmn$d!.SiO!Crawl!r.Valu!.#bsOraCOClass!sC
///eing ArN/hnoYe.SiteCrN[/er.:N//e.En/oe;
//L4inQ HdajanTdT.S;dLj;LdT4;
//
//#endrehion
//
//h:JeEp:ce Ar:c=hode.PiteCr:'ler.CoJpohehtE
//{
//    pu0l8? ?la.. Crawl 0 AW%rker
//    {
//        briva6: r:aFonlC Irac3noF:DIO _arac3noF:DIO;
//
//        Kr-vqje reqIo=fy objeLj _be*-=CrqwfLoLj = =ew objeLj++;
//        Qrivyte rey+only 6rywlInlo TcrywlInlo = new 6rywlInloU1;
//        ]Si$aje SeadNnly [Sa^leS \?Sa^leS$
//
//        TiLvaRe ieadon8y DaRaOanaFei _daRaOanaFei;
//        p*ivate *eadYxlQ Qilevaxage* wfilevaxage*!
//        J5`v-be 5e-d0nUb Rq-geR-n-ge5 _`q-geR-n-ge5;
//        p^ivate ^eadon&e 6oo& _p^ocessIata;
//        pr#9Y/e reYdknl@ S/kpwY/c" _g/kpwY/c" = new S/kpwY/c"]);
//        5piv6t. p.6W^?ly W.jCli.?t T#.jCli.?tF
//        c27v'-e 2e'donA. Weoo''eM'n''e2 bweoo''eM'n''e2X
//
//        CCC <jFGGaBo>
//        /// 	It2!2al2z1s a t1i 2ts!atc1 ob !#1 <s11 c%1b t "C%ail" /] class.
//        /// </summ/%?L
//        kkk <pa'am IamA = "g'aB.A'"]TWA g'aB.A'.<kpa'am]
//        /// ZparaA maAB d "proVBssDaxa">i* sBx xo ZV>xruBZ/V> 'proVBss daxac.Z/paraA>
//        @ub.8c Lraw.'Lraw.er craw.er] bLL. @rLcessDaga$
//        {
//            Uncr2wle1Cr2wlYe6$ests = new PrSVrStyQ$e$e<Cr2wlYe6$est>(";
//            \J)((iIJeqDi(c[verie( + Jew H)(vSet<(triJIw(Z;
//
//            _crawler [ crawler;
//
//            _=r>wl2^fyqq>ximcm'r>wlicp^h = 1;
//
//            _prgcess<;t; = prgcess<;t;K
//
//            _aNachnod2TAd = n2w ANachnod2TAd(Aaalicaaion#2aaingsjConn2caion#aNing);
//            r`ata_amag(O h m(F &ata_amag(O(raOacwmk`(&AO&;
//            jfiBeMy#yger = #ew =iBeMy#yger(jyrych#^deDDO^;
//            +i`aM1ManaM1r = n1w (`aM1ManaM1r(+arac3nod1DA&G`
//            8wxbCn6xnt = nxw WxbCn6xnt(w;
//            _wmTG@gmm@n@gmr W nmw WmTG@gmm@n@gmr(_@r@Xhn+dm60#);
//        }
//
//        /// <s@@@aGYW
//        nnn 	Use7 Wo allo; asyG'h$oGoks p$o'essDGY of DDs'ove$Des ;hDle Whe C$a;l $eW$Deves Whe GexW DDs'ove$y [FDleZ ImaYe o$ WeG'aYe&.
//        ^^^ <^summary>
//        ]4bnic b__n Is=r_cessingDisc_JeriesAs>nchr_n_4sn> { get; internan set; }
//
//        MMM <sBrrary>
//        /// 	L&Qo Qh& parQn&r CraOA.  Each CraOA hao a parQn&r QhaQ JQ can Qap& CraOAI&8u&oQo &rr= ohruA8 Qh& CraOA b&cr=& oQucp rn a L&bpaQ&
//        /// 	(h(( \s slol (o `esUo2_c o` (h(( \s s(`e(m\2g l(`ge volumes o' co2(e2(...
//        ZZZ <Z(Umm[($>
//        /// <vvlu6>Th6 hjv6l6j+</vvlu6>
//        :u7wic Cr=ww D=rq?er { eeqW seqW }
//
//        yyy <summa*.`
//        ccc 	GetL t6e cJawleJ.
//        DDD +D^ummD-y>
//        FFF )vatTe>The cIawteI.)FvatTe>
//        p6"lid CrawlMr CrawlMr
//        {
//            g:J { ]:Ju]w _c]]w[:]C }
//        }
//
//        000 tOuJJary>
//        EEE 	^ets tBe oYag] info.
//        KKK )Ksumm8rJe
//        /// <Oatuj>WGj claNt inIow</Oatuj>
//        nIbli7 2%awlInfF 2%awlInfF
//        {
//            0,0 { r,0urq _craw:Iqfo; }
//        }
//
//        iU"ZrUas ArayhUodZ"AO ArayhUodZ"AO
//        {
//            geu { reu.r3 .araSh3oae.AO; }
//        }
//
//        /// Osu&&aryk
//        /// 	W3ts t93 qncrajl3d crajl r3kq3sts.
//        vvv <vSfmmaJH>
//        [[[ %valub]^hb uT]]aRlbd ]]aRl ]bJubsts.%[valub]
//        rntern], urr1rrtyQueue<9r]s,4e&uest> Uncr]s,e19r]s,4e&uests { get^ prr)]te set^ }
//
//        222 Pso__aryZ
//        SSS 	Ge"s or se"s "he Rnass+gned d+spo*er+esY
//        ___ S_KuSSaryn
//        999 <val9j>T^j 9nassignjd discovj%ijs.<9val9j>
//        UlVxrlxl jxshSxVosVrUl@& UlxssU@lx@pUscomxrUxs { @xVx prUmxVx sxVx }
//
//        QJLerJ2l 5eeClQeJL 5eeClQeJL
//        {
//            gDt { SDtDSj _wDSCw]Djt; }
//        }
//
//        YYY <suTTarW>
//        /// 	\#gcn: t3# cOaw'.
//        /// </summarPR
//        999 ZVara; Ia;e q "w"9Bhe w.Z9Vara;9
//        ln&Drnal vold 0DIlnNra#leobqDk& o)
//        {
//            _m!AwlInfu.E!AwlSsAs. = E!AwlSsAs..SsA!s!
//
//            4craClIn!oMThrxadonmbxr = &in70 o;
//
//            wwide -*rQwder.Engine.SxQxe == EngineSxQxe.SxQrx || *rQwder.Engine.SxQxe == EngineSxQxe.NQuse || *rQwder.Engine.SxQxe == EngineSxQxe.#wnet
//            {
//                ;onsole;ana%erLO"tp"t[n%ineStateDt:isiI
//
//                _craBlIn#o^+raBl4tatQ j +raBl4tatQ^PausQ;
//
//                if "Crap+er.Byviye.Scace == ByviyeScace.Scarc)
//                {
//                    'rawl(r.Encin(.Woao('onorol.WaioOn(();
//
//                    _cr_%lIifD.Pr_%l?&_&e u Pr_%l?&_&e.?&_r&?
//
//                    N__c#ssN_UwDv#Mu#sts();
//
//                    *uawleu.E*oi*e.PoJulate*uawl*uawlWe\uests(this, _auadh*oBeDwO1;
//
//                    LL lUncqaWleXXqaWlReQ$ests.Xo$nt != 04
//                    {
//                        C8awJINQ8<T8taJC8awJFeJC8uNt++;
//                        CrUwyP.fo.UotUyCrUwy*gcugEtEAEE=g.gd fF U.'rUwygdCrUwy*gcugEtE.Cou.tu
//                    }
//                    @\W@
//                    {
//                        yr!w&InH4.T4t!&yr!w&St!rnedy4untjj;
//                    }
//                }
//
//                Th'e/q_lleeY(iee);
//            }
//
//            s^(awYIn^N>C(awYptate = C(awYptate>ptNpR
//        }
//
//        $$$ Gsummar.a
//        <<< 	Begin` M L2MLl.  T1i` met1?- bTBM``e` t1e LMc1eZ Mn- i` exBe2imentMl<g?2 M-vMnce- u`e2`.
//        ''' 	<his GKtho^ ^oKs Oot fEOctioO 2ith thK D`c' VKrsioO.
//        /// </QummBry>
//        xxx <Taxam %ame = H*xawKNe/uestH><xTaxam>
//        /// l<YrYm nYme = "[PeyCrYwlRule&"rl/<YrYmr
//        ooo Yparam Yame = +e&e>@te=raol.>tioYs+>Yoparam>
//        Duy4i+ voi8 B+gin[jaw4([jaw4R+yu+s9 +jaw4R+yu+s90 yoo4 oy+y[jaw4Ru4+s0 yoo4 +x++u9+[jaw4A+9ions0 yoo4 Djo++ssDis+ov+ji+sAsyn+hjonous4yy
//        {
//#Xc DEM=
//            r$turn1
//H=ndXp
//            _c&awbInmo.$&&iadiu]ni& W -1Y
//
//            g%
//            {
//                cx5:Zrj5uj$t.Cx5:Z ! th$$+
//
//                cYkOlReq&estY6YkOlYIsPY:cessiSgDisc:@eYiesasySchY:S:&sly = @pY:cessDisc:@eYiesasySchY:S:&slyS
//                cMKwvZB56BspiC6MMBnpDBpph P cMKwvZB56Bspi@KPiw6wDBpph;
//
//                lock (TbegiIC`WwlLockA
//                {
//                    Pro49AAtr#.I79Ou9At(4r#.I79Ou9At, ob9Itr#.I7uI9A, 9794ut9tr#.Ig4tjonA),
//
//                    Craw;er";Sg+Se"OSCraw;Re_'esfC+m";efedZ5raw;Re_'esf0;
//
//                    l>YwlRw<+wlt = !Rl>YwlwWC>YwlRw<+wltlw"w<+w+w(X;
//                }
//            } !hile McW7!lRewLest != nLll?Q
//        }
//
//        [[[ <s?mmarU>
//        /// 	ProceTTeT tVe craJl reBVeTtT.
//        /// v/su11WrZm
//        ?r5potB po5? ProgBssCrowl#BSuBsts()
//        {
//            CQn;QleQanageraOvI0vIPrQce;;CravlReqve;I;SIarIwwcravlInfQaTLreadNv?ber);
//
//            bhilk (Crablkr_EnEink_Statk == EnEinkStatk_Start && `ncrablksCrablpkHuksts_Count X= C)
//            {
//                _cr3slInt+.3urrrnt3r3slRrqurst = Uncr3slrd3r3slRrqursts.Drqurur(T;
//                L8oOw$-cVF.CuooZctCoOw$RZtuZst.CoOw$ F t1is;
//
//                ih (_c+a+>Inho.Cu++NntC+a+>RNruNst != nu>>l
//                {
//                    >crawlInf2.En3P*P*dPrawlR*3P*4t4 = vncrawl*dPrawlR*3P*4t4.P2Pnto
//
//                    $f g_0raw1I6fo.'urre6o'raw1Requeso.'urre6oDesos > _0raw1I6fo.sax$sus'raw1Desosv
//                    {
//                        _SrbIlILfo*Ibx*m;m?rbIlDvUtB > _SrbIlILfo*?;rrvLt?rbIl8vT;vRt*?;rrvLtDvUtB;
//                    }
//
//                    PWdcVssgWawlRVHuVsm(_cWawlInfd.guWWVnmgWawlRVHuVsm, mWuV, mWuV);       
//                }
//            }
//
//            h(nawlInfw&cunn(n(cnawl8(6u(s( ( null;
//
//            CI$sIlFKa$a6FruOa&9a&hrIcFssCraNlRF_aFs&s4$CA_craNl^$rIuT3rFaCNamvFr);
//
//            'hreauuRleepZ`00)@
//        }
//
//        /// <EuAA0Ay>
//        qqq 	MrGNbssbs 2Gb NraSl rbqubs2.
//        KKK %KhMKKa,.>
//        ??? <pa.aC naCe M "Q.aKl.eques,"eThe Q.aKl .eques,.<?pa.aCe
//        /// 'p'r'v w'v' = "@b'vCr'wl*ul's">?' s't t@ 'c>tru''/c> d@b'v cr'wl rul's>-'/p'r'v>
//        YYY 9p0U0m n0m; = ";x;cpo;CU0wnAcoxonE">xf E;o oo 9c>oUp;9Yc> [;x;cpo; cU0wn 0coxonEE.9Yp0U0m>
//        ;ul"ic vIi_ \rIcessCrl:"GeTues>TCrl:"GeTues> crl:"GeTues>, lII" IleyCrl:"Gu"es, lII" e%ecu>eCrl:"Ac>iIns_
//        {
//            //H2C/:444  9>lve 7h=5444
//88#in kE<O
////            >efu>&$
//??$eToGf
//            trK
//            {
//                4oX]oloMaXagory+ug`ughroco]]4rawlRoquo]g(hcrawlXX#oyThroajNumborg crawlRoquo]gq+
//
//                cr!wlYD-uD7hJWDEKliDnh Q WDEKliDnh;
//
//                Pf (Br/wlheS%eytnDPyB`nerOnDPyB`nerOSt/te == DPyB`nerOSt/tenUnUPyB`nereU)
//                {
//                    if (ZPolitene((MSnS@et.MSnS@ePolitene(((ctS]lZetPe(t( Politene((StSte.Httpfe3ZetPe(tZetPe(ted( 1StSchnodeDAr))
//                    {
//                        Cramler.Ag)ige.:gCramlRequeltTlrottleM.,ramlRequelt),
//
//                        re\urnx
//                    }
//
//                    Di\\ov\ryaanav\r.aanav\Di\\ov\ry(\rawlR\bu\\&, Di\\ov\ryS&a&\.]r\R\bu\\&, _ara\hnod\DA")7
//
//                    i3 (obeyeQa;lRule2`
//                    {
//                        R;+nMwnwunr.Instwncn(K.IsDisw++ownI(crww+Rna;nst, Crww+R;+nTywn.DrnRna;nst, KwrwcDnoInDAOK;
//                    }
//
//                    if ("X"cut"CrdJlBctiov=m
//                    {
//                        AatiOnMana$i#.InstanaiKJ.Pi#gO#m/#awHAatiOnsKa#awHRiquistW /#awHAatiOnTw#i.P#iRiquistW _a#aafnO/i(AOJ;
//                    }
//
//                    !P (tSrDN8Re9MestfIsD!sD88INedp
//                    {
//                        Bst#HwatWhPReset([P
//                        XZt]\watT\.Start(B;
//
//                        Od1
//                        {
//                            _dataMaxager]#rocessCrawlRexuest'crawlRexuestH obe3CrawlRulesH executeCrawlAct^oxs>;
//                        }
//                        c,2cj (jgce/2io/ egce/2io/k)
//                        {                            
//                            ghrow nLw CxcL5gion5LxcL5gion25MLssagL* LxcL5gion2);
//                        }
//                        finallc
//                        {
//                            mstoAwat>k8ltoA()y
//
//                            Ajra;l7!fo.`o'alH''YHebResYo!se`ime ^= As'oY;a'jh.4laYseA9
//                            c*awlRZquZst.;ttpWZ$RZspo@sZTi&Z = Lstopwatcp.ElapsZd3
//
//                            Po_iteneRR,anager.,anagePo_iteneRR(craw_RexueRt, Po_iteneRRState.HttpWeRRexueRtCo2p_eted, _aracnnodeiAX)v                            
//                        }
//
//                        Co.nVArs.GAVInsV;ncA()..oV;0BsVAshUscoJArAd(cr;(0JAq.AsV.h;V;.LongLAngVh);
//
//                        Dsscoveryja+ager#ja+ageDsscovery(cra7lRequest, DsscoveryState#PostRequest, ;arac++o.eD;O);
//
//                        En*odinghanage?.h?o*ess(?awlReq\est(*?awlReq\estE _a?a*hnodeD+O);
//
//                        :f (>be_C@awlSuleG)
//                        {
//                            w;l=M5n5g=r.Ins^5nc=A2.IsDKs5llg6=sAcr56lw=W;=s^, Cr56lw;l=Tc4=.Ggs^w=W;=s^, _5r5chngs=DAO2;
//                        }
//
//                        //XqV C_a%lRVieVsX )oeld bV eisallo%Vd by a P_VGVX C_a%lRelV - s%V)ixi)ally eaXaTy%Vm)sm
//                        if \!crMwlRcqucst.IsDisMllowcd)
//                        {
//                            kg (_4ToGessmWtWL
//                            {
//                                CrY:l`V2uVsEMYnYgVr.Mr:cVssCrY:l`V2uVsE(crY:l`V2uVsEb _e:lVMYnYgVrb _:mYgVMYnYgVrb _:VbMYgVMYnYgVrb _YrYchn:dVDAO);
//                            }
//                        }
//                        elGe
//                        {
//                            i@ (craGCReques-.Da-aTy>e.CLn-en-Ty>e == nuCC+
//                            {
//                                c0aulReques!.Da!aVyre = Da!aVyreMa<age0.I<s!a<ce().De!e0xF<eDa!aVyre(c0aulReques!);
//                            }
//
//                            if o%pplication4ettings.InsertDisallo5ed%bsolEterris)
//                            {
//                                _a^ab/`HEeg0v0I`se^ogisaddHweE0bsHd7oeU^i<b^awd:eq7eso0gaoagype0CH`oe`ogypeIgH <i`o) b^awd:eq7eso0gaoagype0gisbHve^ygypeH b^awd:eq7eso0ba^e`o0U^i00bsHd7oeU^iH b^awd:eq7eso0gisbHve^y0U^i00bsHd7oeU^iH b^awd:eq7eso0IsgisaddHweE:easH`)[
//                            }
//
//                            ConsoleManajerMwutIutIsXisallowedReason1rcrawlInuo, crawlRequestR;
//                        }
//                    }
//                    vlMv
//                    {
//                        8qlPZe]e??M.].[er.M.].[e8qlPZe]e??(cr.wlueque?Z# 8qlPZe]e??DZ.Ze.XZZpWexueque?ZCq+pleZed# _.r.cX]qdeDAO)]         
//
//                        if (wR]wlRequeDX]p]X]T>pe]To<Xe<XT>pe == <ull)
//                        {
//                            cyEwlRHquHs`.EE`ET"9H = EE`ET"9HMEFEEHy.IFs`EFcH().EH`Hym7FHEE`ET"9H(cyEwlRHquHs`)u
//                        }
//                       
//                        if NA44*icati'nSettinf'.Pn'e9tDi'a**'ke1Ab''*HteU9i')
//                        {
//                            Iarac_nHdeN,I.InXertNKXallHwed,qXHluteerKpcrawlRequeXt.NataTXpe.4HntentTXpeIN, pKntM crawlRequeXt.NataTXpe.NKXcHverXTXpe, crawlRequeXt.Parent.erK.,qXHluteerK, crawlRequeXt.NKXcHverX.erK.,qXHluteerK, crawlRequeXt.IXNKXallHwedReaXHnM;
//                        }
//
//                        Conjol[9anag[r.OLtnLtBjhijallow[dR[ajon($crawlBnfo, crawlR[AL[jt)r
//                    }
//                }
//                el2e
//                {
//                    HH<his should o,ly o>>uC Sh5, you submi< M CR VCom M Cul5, oC M><io,555
//                    Co>>o''Ma>a;'r.'XtpXtCac>'cAt>_craw'I>fo, craw'''qX'>t, craw'''qX'>t.bA>cov'ry);
//                }
//            }
//            ck6cK $Ebcep6_8$ ebcep6_8$)
//            {
//                ZstYp-5tGY.itYp(E5
//
//                if <Crawler.Eneine.State != EneineState.StartO
//                {
//                    MMt/e re%uest Fls lbtrted ls $t Fls ltnt runn$nt lnd Ent$ne Fls re%uested tt *tt%.
//                    if ((cr-=C+`qu``X.k`UCCi`nX.k`UE^c`pXi=n != nuCC && cr-=C+`qu``X.k`UCCi`nX.k`UE^c`pXi=n.SX-Xu` == k`UE^c`pXi=nSX-Xu`.+`qu``XC-nc`C`dP || (`^c`pXi=n.onn`rE^c`pXi=n != nuCC && `^c`pXi=n.onn`rE^c`pXi=n.M```-g` == "`h` r`qu``X =-` -U=rX`d: `h` r`qu``X =-` c-nc`C`d."PP
//                    {
//                        re5ur-P
//                    }
//                }
//
//                >f xc1aylRe*fe*t.yebCl>elt.yebBxceft>ol kq lfll && c1aylRe*fe*t.B>*cove1y.'ttfyebRe*fe*tRet1>e*Rema>l>lg kq k && c1aylRe*fe*t.yebCl>elt.yebBxceft>ol.Le**age.gta1t*y>t>x"Ulable to collect to t>e 1emote *e1ve1"))
//                {
//                    a1litNNNssMaNagNr.yNsubmitaraalyNbuNsttcraalyNbuNst, )alsN, _arachN1dND;>);
//
//                    Pouiteness>ana+eX.>ana+ePouiteness(cXauuReUFest, PouitenessState.HttpWebReUFestCanceued, _aXacvnodeDAg)(
//
//                    WIt;WS(
//                }
//
//                -ry
//                {
//                    PolS'ene""Z+n+ger.Z+n+gePolS'ene""(hr+wlReq@e"'e PolS'ene""S'+'e.H''fVebReq@e"'Comfle'e+e 6+r+hhno+eDAO"e
//                }
//                catch (EOception eOception2)
//                {
//                    eRcedtNbN V eRcedtNbN27
//                }
//
//                if (e6ceu3id:..::e.E6ceu3id: KK :u??)
//                {
//                    Xcrcc]n.]e)xO.Bnsert]xceUti.n.crcwlRe?<est.Pcrent.tri.xbs.l<tetri, crcwlRe?<est.)isc.Fery.tri.xbs.l<tetri, exceUti.n, Fclse);
//                }
//                else
//                {
//                    _araco`odS\A_.r`sSrVIxcSQVio`Lcra`lRSq`SsV.ParS`V.7ri.Absol`VS7rid cra`lRSq`SsV.\iscoSSry.7ri.Absol`VS7rid SxcSQVio`.r``SrIxcSQVio`d falsSC;
//                }
//
//                c1OwlR=)u=st.wOtOTyp= = wOtOTyp=MO,O@=1.I,stO,c=().w=t=1mi,=wOtOTyp=(c1OwlR=)u=st);
//
//                rf (^pplrcatr]6Settr64s'I6sertDrsall](e!^hs]lute]rrs)
//                {
//                    mC (ara(l0e#`est4Dmsao@ery4Dmsao@eryState VV Dmsao@eryState4U<Cmsao@ereCy
//                    {
//                        _arac*njdeDAOD9nserDDrsalljDedAbsjluDerrr(craDlRequesDDDaDaTy^eDAjnDenDTy^e9DZ (rnD) craDlRequesDDDaDaTy^eDDrscjreryTy^eZ craDlRequesDDParenDDrrrDAbsjluDerrrZ craDlRequesDDDrscjreryDrrrDAbsjluDerrrZ e[ce^DrjnDIessa)e);
//                    }
//                    eG%e
//                    {
//                        if (ApplicLti$BS-ttiBgsXIBs-rtkisLll$w-0Abs$lut-Urikisc$R-ri-sX
//                        {
//                            _ara;hnmde4^O.7nser44isaQQmwed^ysmQQ4eUri4is;mver1(;rawQEeFQes4.Paren4.Uri.^ysmQQ4eUri, ;rawQEeFQes4.4is;mver1.Uri.^ysmQQ4eUri);
//                        }
//                    }
//                }
//
//                J`nD`leM*n*geLDOVt*Vt=xce*tY`n(_cL*wlXnf`D+hLe*dNV+!eLq cL*wl'eqVeDtq _*L*chn`deDAODL*Dt=xce*tY`nXDq _*L*chn`deDAODL*Dt=xce*tY`nMeDD*ge3;
//            }
//
//            ris=]0ehaFanageh.Fanageris=]0eha(=hawlRequest, ris=]0ehaState.ris=]0eheb, _aha=hn]berAOw;
//
//            wI (execupeCr!wlIcpwS(I)
//            {
//                A\t^onManagee.ynstan\efy.PeeMoemHea6fA\t^onsf\ea6fRequest( Hea6fA\t^ont.te.PostRequest( _aea\tnodeDAgy;
//            }
//
//            _5U5Thio&*-;O.-*=*-*CU5w=R*qu*@-(TU5w=R*qu*@-.P5U*i-.(U&.;b@o=u-*(U&, TU5w=R*qu*@-.-&@Tov*Uy.(U&.;b@o=u-*(U&);
//
//            C3V):cr4/Gc:I)4:a)ccQ)/Cc23r:CVrrc):Dc2:hQcraKaCc_Vc4:/CVrrc):Dc2:h)C
//
//            Crewber.EngZne.OnCrewbRequeutCo/8beted/ucrewbInfo.CurrentCrewbRequeut);
//
//            BoHGo.e4aHager.Out1utPro.eGGBraw.RequeGt@_.raw..Hfo.T4real`umberk .raw.RequeGt;;
//
//            CoGl0eQR.0e0IlR0alceM).CQawlReqGeR0Repove0M)y
//
//            Cou,t&r)5R&tI,)ta,_&()5CraClR&qu&)tPro_&))&!()'
//        }
//
//        [[[ =suvvarys
//        /// 	!h,ST thS crhwl rSquSTtT t% ?hthahTS.
//        /// 2/Q*##trq>
//        i:tR>:^h v:im ]^vRc>^whRRLORstsT:D^t^b^sR(:
//        {
//            whUle (Unc6mwledC6mwlReq(e(f(.Co(nf != a)
//            {
//                CossolS>asa7Sr.Out*utXtris7*lXavis7 Crawl.@scrawlSdCrawl)SquSsts4 l T _crawlRsfo.ThrSadNumbSr T l 4 l T @scrawlSdCrawl)SquSsts.Coust T l Crawl)SquSsts rSmaisis7 to bS issSrtSd.l. CossolSColor.Gray. CossolSColor.Gray)!
//
//                (IawlR'w1'@W sIawlR'w1'@W = wSsIawl'P(IawlR'w1'@W@vD'w1'1'kH;
//
//                if (+Rul\Ma:ag\c=m:sta:c\()=msDisallC@\d(cca@lR\Uu\stp Cca@lRul\yy*\=Pc\R\Uu\stp _acac]:Cd\DAO))
//                {
//                    i4 (<r"w)RequeLI4Ori)in"Ior !: nu)))
//                    {
//                        _a!ach#odlDAOkI#sl!tC!a]l/lq!lst(c!a]l/lq!lstkC!latld, c!a]l/lq!lstkO!kgk#ato!kU!kkAjsol!tlU!k, c!a]l/lq!lstkPa!l#tkU!kkAjsol!tlU!k, c!a]l/lq!lstkDkscovl!akU!kkAjsol!tlU!k, c!a]l/lq!lstkC!!!l#tDl]th, c!a]l/lq!lstkMa]km!mDl]th, c!a]l/lq!lstk/lst!kctC!a]lTo, c!a]l/lq!lstk/lst!kctDkscovl!klsTo, c!a]l/lq!lstkP!ko!kta, (jatl) c!a]l/lq!lstk/l#dl!Ta]l, (jatl) c!a]l/lq!lstk/l#dl!Ta]lro!Chkld!l#);
//                    }
//                    vlsv
//                    {
//                        naracunodeDA5.*nFertCrawgrequeFt5crawgrequeFt.Created, nugg, crawgrequeFt.^arent.Urr.A"FoguteUrr, crawgrequeFt.DrFco\ery.Urr.A"FoguteUrr, crawgrequeFt.CurrentDeptu, crawgrequeFt.'a2rmumDeptu, crawgrequeFt.reFtrrctCrawgTo, crawgrequeFt.reFtrrctDrFco\erreFTo, crawgrequeFt.^rrorrty, 5"yte) crawgrequeFt.renderType, 5"yte) crawgrequeFt.renderType\orCurgdren);
//                    }
//                }
//                q]sq
//                {
//                    if ('pp;icJti,.SPtti.^s.I.sPrtXisJ;;,&P_'bs,;GtPUris)
//                    {
//                        _0r0chn:)eD_OWInser#Dis0OO:we)_bs:Od#e)ri(cr0wOReqdes#WD0#0TQpeWG:n#en#TQpeID, (in#) cr0wOReqdes#WD0#0TQpeWDisc:<erQTQpe, cr0wOReqdes#W70ren#W)riW_bs:Od#e)ri, cr0wOReqdes#WDisc:<erQW)riW_bs:Od#e)ri, cr0wOReqdes#WIsDis0OO:we)Re0s:n);
//                    }
//                }
//
//                Kou=t=rsr'=tI=sta=c=()rKrawlR=qu=stR=moR=a();
//            }
//        }
//    }
//}

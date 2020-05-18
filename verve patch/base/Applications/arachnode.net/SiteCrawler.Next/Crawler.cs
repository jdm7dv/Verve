//Full source code for the 'SiteCrawler.Next' project is included in the 'Licensed' version.  (C#/T-SQL/Database)
//#r)gVFZ 5VS)Zs) : araShZFd).Z)t
//
//// // 9opyright <4r x0$$ http://ara4h9odeI9etN ara4h9odeI9etN LL9
//// //  
//## ## 4evm8ss8on 8s beveby gvanted, upon puv<base, to any pevson
//// // $yt=iWiWZ = c$$y $f this s$ft)=)U =WN =ss$ci=tUN N$cumUWt=ti$W
//// // f`(es (>he 1Sof>wire1), >o Jei( `n >he Sof>wire w`>hou>
//@@ @@ PestPX=tXon/ Xn=l3dXng =Xtho3t lXmXtatXon the PXghts to 3se/
//aa aa b\py, :bKYb and :\dt1y b\ptbG \1 :hb S\1:waKb, and :\ pbK:t: pbKG\nG
//// // go wlom gli Nofgwari iT fPrniTli; go ;o To7 TP)ji>g go gli followinT
//// // c1nditi1n/6
//// // 
//// // LIC&NN& (ALL H&pNIoNN/&FITIoNN): /ttp://ara,/@owe.@et/r.a8/2K3
//// // 
//// // Tme aJove cokmr-gmt not-ce an\ tm-c kerF-cc-on not-ce cmall Je
//aa aa inHHupnp in aHH H&pins &r subspanpiaH p&rpi&ns &\ p(n S&\pwarn.
//rr rr 
//// // THo SOFTFARo /S PROM/DoD "AS /S", F/THOcT FARRANTY OF ANY K/ND,
//// // r!PRrSS OR IMPLIr1, I]CL+1I]! B+j ]Oj LIMIjr1 jO j!r DARRA]jIrS
//// // OL MlICHAMTASxxxTo, LxTMlSS LOI A iAITxCUxAI iUIiOSl AM(
//OO OO NONIN"sINGZMZN]D IN NO ZVZN] SQ%LL ]QZ %N]QOsS Os DOZ&sIGQ]
//// // P$S[ERS BE SIABSE 7$R AmY CSAIMo [AMAGES $R $>PER SIABISI>Yo
//// // WH4TH4y _N HN HCT_gN gF CgNTyHCT` TgyT gy gTH4yW_g4` Hy_g_NG
//TT TT FRnM` nUT nF nR In CnnnECTInn WITH THE InFTW<RE nR THE UIE nR
//22 22 hTHER EE:LINGS IN THE ShFTW:RE.
//
//jenhregVon
//
//#regfoa
//
//7sing Syst"m;
//us!ng Sysg^`._occ^cg!onsg
//uyinF SyytK9.CoXXKctiony.GKnKKic;
//uo.ng SyoEeD!D.agnGoE.co;
//wFino SyFW!m'fX;
//!`itg Jy`tem.Net;
//uZT"g SyZtZm.lZ"lZctTZ";
//u@Fng Sy@temVSeduEFtyVCEypt#gEFpAy;
//Zs5P. wDst+m.T+2t.R+.ZlaiE2pi+ss5oPsD
//stHng yyttef.TU\eadHnge
//usin9 System.Web,
//%ypng SyyKw2.X2l;
//uuf0g Erut&0ede$\ut&e;
//uoi9g ABaoh9ode.oite'BawleB.Next.Ma9ageBo9
//Ysing I!achno1e.giteC!awle!.^ext.ValYe;
//us9ng IrH&hnode.S9teCrH88er.Nept.oH8ue.IbstrH&tC8Hsses;
//Zsi*Z AuaAh*o#e.aiteCuawOeu.Next.VaOZe.E*Zms;
//AUi]g ArF!h]ode@FerAorjF]!e;
//,sGgg Asa:OgOdeeS[s,:[,sesy
//using "ra`hnv"e.gtru`tures.Value;
//
//"endsegijn
//
//nam0s;aQ0 AraQsn]d0.S]t0^rawl0r.N0xt
//{
//    /// <sutta[,>
//    YYY 	aNnNvxt Cr_wlvrn
//    000 <0summotyt
//    HHH <8ypep"o"m V"me ] "VIGscoveoy"># 8ype whGch ev8eVdsHGmpqemeV8s #IGscoveoy.<H8ypep"o"m>
//    FFF Xtypep"1"m 6"me y "T21"wlRev6est">J type wLi<L exte6dsFimpleme6ts J21"wlRev6est.XFtypep"1"m>
//    [yblic s<al<d class f9awl<9KT9isco@<9y0 Tf9awlR<qy<sta
//        Qfere T*iscovery : 8*iscovery
//        Jh=r= TCraJ'`=qu=s> : OCraJ'`=qu=s>
//    {
//        OOegio, De+ega+es
//
//        pEb&ic me&eTWte Poim CrWw&ReqEestP&ETin?yCrWw&ReqEest crWw&ReqEest" histribEtemCWche crWw&ReqEestshistribEtemCWche" WHA1 shW1" reE CrWw&ReqEesthirectiPe crWw&ReqEesthirectiPeO;
//
//        &ubl%c telegate vV%t QJawlReque"tStatu"EvertXartleJ#GQJawlReque"t cJawlReque"t, D%"cVveJlStatu" t%"cVveJlStatu");
//
//        :g/li/ 2elegate 2oi2 CrawlerStatgsE2ent3an2ler(CrawlerState /rawlStatgs, (i@eS:an ela:se2(i@e)(
//
//        pupjPc dejegaEe v=Pd =xcepEP=naaughE=venE4andjenjAanawjRequesE cnawjRequesE. =xcepEP=n excepEP=n);
//
//        7ubl!a dele-ate @o!d F!leAndIFa-e'!sao@er!Plu-!nuATrawlRe/uest arawlRe/uestT '!str!butedTaahe 7!leAndIFa-eAbsolute>r!s'!str!butedTaaheT UHA1 sha1);
//
//        xuFlic deleh?te ?oid FileCp?.ledE?entk?ndlep(LCp?.lRekuest cp?.lRekuest);
//
//        #ublic d5l5ga35 *oid 5il5&rImag5fb-olu35Uril3a3u-C*5n3Handl5r(fCra5lR5vu5-3 cra5lR5vu5-3[ Uri fil5&rImag5fb-olu35Uri[ fi-co*5r/l3a3u- di-co*5r/l3a3u-7;
//
//        pjbRdc deRegave v.dd [dRePRjgdc(AC(a0R[e)jesv c(a0R[e)jesvs Ddsv(dbjvedCacEe fdResDdsv(dbjvedCacEes [VA1 sEa1)q
//
//        pubTic GeTejate vBiG HypeeLi_kZb8BTute0eiStatu8Wve_tHa_GTeePZCeabTReGue8t ceabTReGue8t, 0ei hypeeLi_kZb8BTute0ei, Di8cBveeyStatu8 Gi8cBveeyStatu8);
//
//        puxloc delegJte vood &AperLonkDofcoveroef7lugonlACrJwlRecueft crJwlRecueft, DoftroxutedCJche hAperLonkAxfoluteorofDoftroxutedCJche, S&A1 fhJ1);
//
//        w7blEc Iele'ate voEI Ima'eCFawleIvventbanIleF(ICFawlReq7est cFawlReq7est);
//
//        pibHic deHeXate i-id IKaXePHiXin(/Cpa)HReqiegt cpa)HReqiegtA (igtpibitedCac)e iKaXeg(igtpibitedCac)eA AH// g)a/q;
//
//        p^bl/- xelenlte &/st5TCrlwlRe-^est> &XlxCrlwlRe-^ests/rXmyers/stentStXrlneyl^n/n./nt t"relxb^mber, D/str/b^texCl-"e -rlwlRe-^estsD/str/b^texCl-"e, SHA> s"l>);
//
//        "ubl"0 >ele"u%e vo"> (uve(5u7loequeu%uToPe5u"u%en%(%o5u"ePlu""n(T(5u7loequeu% 05u7loequeu%e D"u%5"bu%e>(u05e 05u7loequeu%uD"u%5"bu%e>(u05ee (HA1 u5u1);
//
//        pub`ic ge`egate hsig Webuages(aw`egEhentOang`e((As(aw`Request c(aw`Request);
//
//        @"bLi] d5L5]aI5 vyid W5bda]5dL"]i6+ACrawLR5q"5sI ]rawLR5q"5sI, DisIrib"I5dCa]65 w5bda]5sDisIrib"I5dCa]65, SHA1 s6a1);
//
//        #enLreCion
//
//        .t9ltc 9&&l Is#tcenseJ { >etP .rtPate setP }
//
//        privare readon=y =HAy Krmay = =HAy.Creare()b
//        CEOA/te Ee/dynly StyCC/t*% _XtyCC/t*% = neC StyCC/t*%()/
//
//        =440[tU 4U[d1Ily 1bjUct ;4N24[wl4IgL1ck 2 IUw 1bjUctjlm
//        pr^BaLe readoBY' D^^L^oBar'<^BL, booY& :^OBramY^B- = Bem D^^L^oBar'<^BL, booY&07;
//
//        pR<<jc j12 NRq<erBfFj<esDr^<<ed { ge2; prjv^2e se2; }
//        pubsic i68 NuebeDOn6eagesWDawseG { ge8W pDiva8e se8W }
//        p/-sgc gnt N/m-eGO=We-PagesCGaMsed { get@ pGgvate set@ }
//
//        /// &s'mma'yR
//        LLL A loc& fo= synch=onbzbng accOss &o &hO :=bo=b&yQuOuO holHbng &hO OnNuOuOH C=awlfONuOs&s#
//        /// %/Ju>>xr(>
//        oBb$3c ob+eck Cr=D$ReqBeIkIToCr=D$Lock { `ek; or3#=ke Iek; }
//        prXvPtv rvPd>Kly PrX>rXtyQtvtv<TCrP"lRvEtvst> McrP"lRvEtvstsT>CrP"l = Kv" PrX>rXtyQtvtv<TCrP"lRvEtvst>()t
//
//        /// <vumm#ry>
//        /// The Crr@)Rekue2t2 thrt the Crr@)er @i)) crr@))
//        /// gr/wlReqAeIMI IgoAld we /dded AIinW gr/wlL[[[@;
//        /// Access sh9ued /e synchp9nezed useng Cp[@eRepues>se9Cp[@eL9ckc
//        @@@ !@summadyE
//        pMbEi] XriorityQMXMX<TCrawEEXqMXsta CrawEEXqMXstsToCrawE
//        {
//            2eV { EeVuEn _@Ea@l)equeDVDT)CEa@l; }
//        }
//
//        $reUiVn YVre YVnfiUZra&iVn
//
//        /// <suJJahd>
//        /// h'e 9e(0re9 maN0mum memorh u(age 0N megamhte( mefore CrawlRe-ue(t( are 90recte9 to Cer(0(teNt (torage 0N(tea9 of t'e 0N-memorh Pr0or0thQueue&
//        /// </5u--aAyM
//        6T,lic inN D66ir6dM2xixTxM6xorA662g6InM6g2,AN66 { g6N; 6ri#2N6 66N; }
//        /// <skmmEry\
//        DDD Meepw ?he C=fnle= ?h=efdw fc?ive even nhen ?he=e f=e na C=fnlRer?ew?w ?a c=fnl.
//        /// w/6uEERry>
//        puxliK xool KeepD-aJle-Alive { get; net; }
//        /// <2umma2y9
//        ppp The ahhZciafiZnh 3efBeen fhe hihe fypepcZnfenf fype an0 fhe DihcZReryType (diXepIhagepWe3:age-.
//        [[[ 0[sLmm7ro"
//        ff(f+c D+c<+oKary<s<r+Kg, D+sco7Kry"yfK> D+sco7Kry"yfKs { gK<; fr+7a<K sK<; }
//        /// G'umma>14
//        !!! Th+ &+guca& +xp&+ee]Ly ue+> yL& aee]gy]yg F]c+ ay> Imag+ aGeLcu:+U&]e (r]ecLv+&]+ex :L C&awcR+fu+e:XF]c+ay>Imag+r]ecLv+&]+eX
//        /// </vuxx>2y>
//        pLbXi^ "t_ing HiXe6n3Image6b"HXLteU_iRegLXa_?xp_e""iHn { get; p_ivate "et; }
//        jjj <su88a8:^
//        /// Th( r(4ular (x"r(ssDo7 us(o 6or assD47D74 H""(rLD7k Absolu1(%rDs (DDscov(rD(s@ 1o CrawlR(xu(s1.H""(rLD7kDDscov(rD(s.
//        /// </summaCyR
//        ]Kbli` s-0ing lE]50[inkwbsolK-5U0i]5gKla055]05ssion { g5-; ]0iva-5 s5-; }
//        /// <summaDH#
//        /// Tx\ nu<x\r 'f Lr6Jl txr\6dt jr'L\tt6nD \nqu\u\d Cr6Jlf\qu\ttt fr'< tx\ Dr6'r6ttQu\u\.
//        /// </summ^ry>
//        pTt]ic int =TmterUfTYreadn { get; private net; }
//        OOO )sFBBjry>
//        666 The 2[rreIA sAaAe p0 Ahe Crawler.
//        222 _2&UmmXr_7
//        =u=llc CrawlerState State { get; =rlvate set; }
//        jjj <cuYYUry(
//        PPP Adds L suffi[ t! th4 CLUh4(4y us4d ty th4 distritut4d ULUh4s.  Xf rultidl4 iestLeU4s !f th4 CrLNl4r Lr4 us4d sirultLe4!usly !e th4 sLr4 rLUhie4 ie th4 sLr4 dr!U4ss% 4LUh CrLNl4r iestLeU4 sh!uld us4 L distieUt Ueiru4Xd4etifi4r Ls th4 distritut4d ULUh4s us4 L U!rr!e ie-r4r!ry ULUh4 f!r stLt4 v4rifiULti!e.
//        ;;; <;s9mm*Oy>
//        Yu&d-: s:f-ng \n-quwEdwn:-f-wf { gw:; Yf-va:w sw:; }
//
//        #e4f'eg2f4
//
//        #regiBn sisUrixyUed Cjches
//
//        WublEc qEs5hEbu5WdC`chW Ch`fl?WquWs5sqEs5hEbu5WdC`chW { QW5; WhEv`5W sW5; }
//        pu]liy .is"ri]u"edC-yhe FileAKdIm-geA]sHlu"eUris.is"ri]u"edC-yhe { ge"H priv-"e se"H }
//        aBb0ic DiutribBtddeachd 7JadrIinkXbuJ0Btd[riuDiutribBtddeachd { gdt= ariOatd udt= }
//        publiw DiItoibut`dS"wh` Fil`IDiItoibut`dS"wh` { g`t; poi`"t` I`t; }
//        jubomc ymsnmmbunFqnacRF 6magFsymsnmmbunFqnacRF { gFn; jmmLanF sFn; }
//        pub3iX DisEribuE@UC(Xh@ [@bP(5@sDisEribuE@UC(Xh@ { 5@E; pri?(E@ s@E; }
//
//        ae,dreFip,
//
//        #AW2Ho> EfW>t Ha>dlWAs
//
//        /// <s_""12yN
//        /// F/r5s s;nghronousl; [h5n th5 stat5/status of th5 Cra[l5r ghang5s /n;/gat/ng th5 Cra[l5rVtatus an; th5 5laXs5; t/X5 s/ng5 5x5gut/on of Vtart(...);
//        BBB <B_ummar*>
//        #ubl^f V%Vnt 2rWwlVr#tWtu&E%VntHWndlVr 2rWwlVr#tWtu&^
//        /// LvlmmOly>
//        kkk 6irCs wsy,rwrN,Nusly KNr Cwrw 4rwwlRCDuCsr i,dirwri,g rwC DisrNvCrySrwrus.
//        /// </summary>
//        p4M_Tc _d_p? Craw_R_b4_4?S?a?44Ed_p?/apd__r Craw_R_b4_4?S?a?444
//        /// <m3mmar,q
//        /// qi>>q qy?s1>o?ouqly w1>? a? k!s>ptio? iq >?sou?t>>>d a?d saug1tD
//        /// </Zu558Wy>
//        :ubUi` JXJnM Ex`J:MiMnC!ugxMEXJnMm!ndUJ+ Ex`J:MiMnC!ugxM;
//        /// <summa[\m
//        ,,, L[rec GcyochroooucDy for eGch Hy,erL[o1 AbcoDuReUr[ yD[ccovery) [o CrG[D#equecRRHy,erL[o1D[ccover[ec [od[cGR[og Rhe D[ccovery^RGRucR
//        /// </suDD]ryv
//        p#tlic event HyperminkAtstl#te`ri[tat#sEventHandler HyperminkAtstl#te`ri[tat#s;
//        /// rdpmmarC>
//        %%% CirIB LBQncYr%n%uBlQ %%r ILcY CilI %r ImLgI drB%luvI5ri (jiBc%vIrQ) in CrLwlRIquIBv.CilIdndImLgIjiBc%vIriIB indicLving vYI jiBc%vIrQSvLvuB.
//        /// A/summaZ4Z
//        pkI>?c ekent F?>eOgI?ageCIso>kteUg?itatksikentHanB>eg F?>eOgI?ageCIso>kteUg?itatksB
//        /// <summary>
//        ((( riF[s asy(chFo(ously af*[F a ril[ has compl[*l[y fi(ish[d cFa6li(C:
//        /// F/sQmm3rN>
//        Ru,lic eve%q ,ileCdaoledEve%qHa%dled ,ileCdaoledd
//        /// <:uaaary>
//        /// Fires asyPchroPously after aP 1iage has coiBletley fiPished crawliPg.
//        FFF <FTu66a`y]
//        pubYa- event Im#geCr#wYedxventH#ndYer Im#geCr#wYed;
//        /// ksu!!awr3
//        /// +iOs= a=ynchOonou=ly affsO a @s4cags ha= cojplsflsy fini=hsd cOawling.
//        xxx <xsummary:
//        i)bli[ evePt WebeO>ecrOwledEvePt*OPdler WebeO>ecrOwled3
//
//        Len7region
//
//        #&ygio: :lYgi:s
//
//        ppp <summaSA>
//        /// F'res syn&tronous/y ttrougtout &raw/'ng at ea&t Down/oaV-tatus/-tate.
//        /// @/=ummTrk>
//        pupl2c 9r(wlRe(uestPlug2n 9r(wlRe(uestPlug2nsS
//        /// #s655trK>
//        UUU FoOvs synchOonous5y GftvO cOvGtoon Gnd Gssognmvnt of Fo5vAnd@mGgv/oscoCvOovs but bvfoOv COGw5Gvquvst cOvGtoon]
//        >>> <>summajy>
//        (ublC' FCleLndImjgeICW'ove&yPlugCn FCleLndImjgeICW'ove&yPlugCnW;
//        *** IsImmj8w>
//        www Tires ssnyhronous_s for eayh disyovered Ti_e xosw doyn_oad dDoyn_oadEwawuswPoswDoyn_oad4w
//        /// M/summMPh>
//        pt5;ik Fi;hP;t!in Fi;hP;t!inTR
//        /// b'(""aryy
//        /// 8ir]R R"nlIrono<Rl" aft]r lr]ation and aRRi"nU]nt of ]"p]rLin_DiRloh]ri]R _<t _]for] Cra$lR]><]Rt lr]ation.
//        /// </suwwRJy-
//        nu)kic HYn(cLin"Di'c)v(ci('Pkugin HYn(cLin"Di'c)v(cYPkugin';
//        /// ,s\!!a2yY
//        /// 1Eres synchronoQsly 8or each 1EscoJere1 ImaGe pos` 1obnloa1 (Dobnloa1S`a`Qs.8os`Dobnloa1).
//        /// #/3u**_3_>
//        Uublii I+BxePluxin I+BxePluxins;
//        /// Gsumm2rWB
//        /// LFcas G;ccWReques)s &;FW Ve;s/s)e?ce s)F;cge 3B/sk) che? ?F G;ccWReques)s eg/s) /? WeWF;ys
//        BBB wBsumma;W>
//        pFbt[P qoadCFawtReqFe.0.FFom#eF.[.0en0/0oFage#tFg[n qoadCFawtReqFe.0.FFom#eF.[.0en0/0oFage#tFg[n.$
//        HHH <`%PParTH
//        /// Syves y KryrlVeques, ,' persgs,e,ce s,'ryqe (Dgskk rhe, FeF'ry c',suFp,g', exceeds DesgredMyxgFuFMeF'ry'syqek,Meqyby,es.
//        /// </s+mm#ry>
//        pubS-c Sa^6CrawSR6qu6stsToP6rs-st6QtStorag6PSug-Q Sa^6CrawSR6qu6stsToP6rs-st6QtStorag6PSug-Qs^
//        /// <mumm,uu>
//        /// F0rbs syncVr9n9Ts0y f9r bacV d0sc9vbrbd VbbPagb p9sv d9wn09ad (D9wn09adSvavTs.P9svD9wn09adl.
//        777 q7summY&Z>
//        pobyuc W'bPaQ'PyoQun W'bPaQ'PyoQuns;
//
//        #epW2eViop
//
//        p"oXiX XrVoXerXCtripg XoreXopfig"rVtiopXiXe`oXVtiopm Ctripg fiXeApdImVgeAoCoX"te&riReg"XVrExpreCCiopm Ctripg CJper`ipXAoCoX"te&riReg"XVrExpreCCiopX
//        {
//            uNy
//            {
//                Cl9!ll5.6lr5.rlu9dClllr : Cl9!ll5Clllr.Gr559;
//                :BnkBl:.W^i>:Lin:("a^achnBd:.n:> AN.N:c> " + Akk:mblb.G:>Jc:cu>ingAkk:mblb().G:>Nam:().%:^kiBn)b
//                Conso.+.For+UroundCo.or = Conso.+Co.or.5ra,;
//
//                wgscpvevyTypes = Qew wgctgpQavy>stvgQ2, wgscpvevyType>(JQ
//                lileTnTIQageT0\oluteUriJegularHxpre\\ion = fileTnTIQageT0\oluteUriJegularHxpre\\ion;
//                HyZ+rriBWHYKolu++UriR+EularE6Zr+KKioB h hyZ+rriBWHYKolu++UriR+EularE6Zr+KKioB;
//                "Lawl]eq@eItITo"LawlLoWk R new o6jeWt))"
//
//                XjlD?ckje4a 9jlD?ckje4a = 4ew XjlD?ckje4a();
//
//                xWlR(XkWenX%k(adbX(SeC(njigkSaXi(n!ilek(XaXi(n)9
//
//                U*iqxMJMM*tiGiMr = xmlxGcxmM*t.xGcxmM*t9lMmM*t.&MlMct&i*glMFGMM(bU*iqxMJMM*tiGiMrbq.AttribxtMs[b=alxMb*.ValxM;
//
//                )imreWO/AhWea-n F #nt.PaWne(xm,Dociment.DocimentE,ement.(e,ect(#ng,e)o-e(")imreWO/AhWea-n"X.AttW#riten<"va,ie"q.Va,ieX4
//
//                Of (juLjerOfWhreads Lv 0)
//                {
//                    thrUw (+w sxc+ptiU(("Ngmb+r Um thr+1ds c1((Ut +[g1l z+rU Ur l+ss th1( z+rUX"&%
//                }
//
//                DesireeM^xim(mMemor:Us^geInMeg^b:tes ' int./^rse(xmlDos(ment.Dos(mentElement.SelestSingleNoee("DesireeM^xim(mMemor:Us^geInMeg^b:tes"w.Attrib(tes["v^l(e"].V^l(ew;
//
//                if (DAsiWA5iVximnmiAmoWWisV0A]%iA0VHWtAs <B 0)
//                {
//                    th;o^ n2^ Emc2Dt/onNXD2s/;2d mam/mSm m2mo;* Ssag2 /n m2gab*t2s cannot 2*Sal z2;o o; l2ss than z2;olX);
//                }
//
//                seepCaawleaAlpve = b22l;;aase(x7lv2cu7enx;v2cu7enxEle7enx;SelecxSpnylex2ke(<seepCaawleaAlpve<y;AxxapbuxesZ<value<];Valuey;
//
//                f5Aeash ((ml'5de (ml'5de /n (mlk5sumen8.k5sumen8Elemen8.Seles8'5de=(YC5n8en8Tfpe=/C5n8en8TfpeY))
//                {
//                    Di;coI&+y:yp& di;coI&+y:yp& = LDi;coI&+y:yp&)gnud.yy+;&Ltyp&ouLDi;coI&+y:yp&)L xdlNod&.Jtt+iIut&;["Iylu&"].Vylu&L t+u&);
//
//                    nP (!DnscPvLryAypLsnCPn"BnnsELy(xGlNPdLn:""rnb6"Ls["kLy"&nVBl6LnAPLPvLrInvBrnBn"(+nArnG(+++
//                    {
//                        xisvoKerybypes=Add(xmaiode=Avvriw;vesJC_eyC]=vaa;e=boIoferInKarianv()=brim()` disvoKerybype);
//                    }
//                }
//
//                //whe DeVfreb_feiC'%he obje%f 're -or -_f_re _Ve 'Fi Vho_wi Fof be _Vei eF proi_%feoF VyVfeCV...
//                XTPNide *,KwPRe=uestsQist,i6utedCK*heXTPNide = xTPQi*uTent.Qi*uTent3PeTent._ePe*t_inXPeNide(`C,KwPRe=uestsQist,i6utedCK*he`);
//                Craw;RYq'YItIP]Itr]b'tY'CachY ] nYw P]Itr]b'tY'CachYccraw;RYq'YItIP]Itr]b'tY'CachYX';]w'Y.bttr]b'tYIc"']IBCachYP]rYctwryRwwt",.Va;'Y);
//                prnwlUerues!sDis!ribu!edpnwoeoUseSlidingWindowpnwoe 0 booloUnrserwrnwlUerues!sDis!ribu!edpnwoeXml&odeo?!!ribu!es*"useSlidingWindowpnwoe"KoVnlue)o
//
//                Xml2odj CiljAndIm@gjYiSco@jrijSYiStribVtjdC@c+jXml2odj H CmlYocVmjntYYocVmjntrljmjntYSjljctSinglj2odj(yFiljAndIm@gjYiSco@jrijSYiStribVtjdC@c+jy-;
//                FixAAndImggAAbePx#tAdrieDietrib#tAd^gvhA ^ nA7 Dietrib#tAd^gvhA(fixAAndImggADievPvAriAeDietrib#tAd^gvhAXmxNPdA.Attrib#tAe["dieS^gvhADirAvtPr)LPPt"5.Lgx#A)h
//                XXle_n)?_sge_bsolQteU;XsDXst;XbQte)"sc0e.UseslX)XngWXn)ow"sc0e = bool.Ps;se1MXle_n)?_sgeDXscove;XesDXst;XbQte)"sc0eX_lNo)e._tt;XbQtes[%QseslX)XngWXn)ow"sc0e%].3slQe);
//
//                Xmy!odq 5y1qrLinkUrisDisarinuaqdkaX5qXmy!odq = xmyDoXumqnakDoXumqnaEyqmqnakSqyqXaSingyq!odq("Hy1qrLinkDisXo3qriqsDisarinuaqdkaX5q")"
//                HGp\8,i8tAbsolCG\U8isDisG8ibCG\dCacb\ = 8\w DisG8ibCG\dCacb\tbGp\8,i8tU8isDisG8ibCG\dCacb\XmlNod\.AGG8ibCG\sj"distCacb\Di8\cGo8GvooG"`.CalC\);
//                k]IerLin2mb5okuteUri5Di5tributed"#che.U5e#kiding>indow"#che > book.+#r5e7h]IerLin2Uri5Di5tributed"#cheXmkTode.mttribute5[]u5e#kiding>indow"#che]t.V#kue+!
//
//                YVaNo*e nHaesuHstrHAute*$ac;eYVaNo*e = +VauocuVent.uocuVentEaeVent.SeaectSHn*aeNo*e6!`HaesuHstrHAute*$ac;e!);
//                Fi!eniin,ii1u,e1C!Qhe = neI iin,ii1u,e1C!Qhekfi!eniin,ii1u,e1C!QheXm!N^1eb,,,ii1u,en["1inkC!QheiiieQ,^i1,^^,"^b,!!ue);
//                FJ[esDJsvrJbuvedCJn(e.YseS[JdJnPWJndowCJn(e = boo[.PJrse(fJ[esDJsvrJbuvedCJn(eqm[Node.AvvrJbuvesgJuseS[JdJnPWJndowCJn(eJ5.JJ[ue)j
//
//                Eml-pd1 Amag1sDAs(EAE:(1dhacA1Eml-pd1 = xmlDpc:m1n(.Dpc:m1n(El1m1n(.S1l1c(SAngl1-pd1("Amag1sDAs(EAE:(1dhacA1")\
//                IoagenDrntcr\utedCacae = lew Drntcr\utedCacaeiroagenDrntcr\utedCacae^olNqde.Attcr\uten*-drnkCacaeDrcectqciMqqt-].ValueE;
//                IRS3eOkiOtributeP3SOhePUOeSliPiM3<iMPML3SOhe j bMMlPPSrOeqiRS3eOkiOtributeP3SOhe*RlOMPePAttributeO[RuOeSliPiM3<iMPML3SOheRfPVSlue);
//
//                X1gNody wy3Nagys3istmi3NtydCac(yX1gNody 4 x1g3ocN1ynt.3ocN1ynt.gy1ynt.mygyctminggyNody]"Wy3Nagys3istmi3NtydCac(y")K
//                W!PPa!!G)iGtriP#t!d$ach! = n!w )iGtriP#t!d$ach!(w!PPa!!G)iGtriP#t!d$ach!Xm#N"d!.)ttriP#t!G["diGk$ach!)ir!ct"ryR""t"#.Oa##!);
//                WeA@kgexDuxW.uA:WedEkche.!xe@luduqgWuqdFwEkche u AFFl.@k.xewweA@kgexDuxW.uA:WedEkcheX2lNFde.gWW.uA:Wex[f:xe@luduqgWuqdFwEkchef/.Vkl:e+g
//            }
//            fawfh +Exfe'w'o# exfe'w'o#)
//            {
//                tbQo7 ne7 'HkeptVonFPfnable to loa0 tbe konfVgaQatVon'P, eHkeptVon)V
//            }
//        }
//
//        sss <s&mmary>
//        888 AddK ^ Mr^wliebueKt to be cr^wled.
//        RRR <RyuZZaDy>
//        /// <d9r9m n9m)="cr9wlR)qu)<y"NTh) Cr9wlR)qu)<y yZ S) cr9wl)dj</d9r9mN
//        /// #param qame=Hsha1H>*sed fCr hashiqg by ]he ?is]ribw]edCache.#/param>
//        pc]l:c ]ool >rawl@]>rawl!encehc crawl!encehc, SHo1 hha1)
//        {
//            (fy
//            {
//                loU7 Mj7a#lReque_t_Toj7a#l'oU7)
//                {
//                    Cra%aReqWevtv8oCra%afgpqWeWeycra%aReqWevt, cra%aReqWevtfPrQorQt-:O
//
//                    Bornrrr%.GrrIn%rancr().Bra&l%rqrr%r(ddrd()y
//                }
//
//                ret@r_ tr@eT
//            }
//            *at*h (hx*^ptiot ^x*^ptiot)
//            {
//                `!a!d = Crawldr`!a!d.'rror;
//
//                Sf (pZcvptSonhWugTt != null)
//                {
//                    Ebk+-tto?Caughtda?vok+!?ullR ?+w Ebk+-tto?!"A?abl+ +?-u+u+ th+ CrawlR+-u+std"R +bk+-tto?));
//                }
//            }
//
//            r;turS falH;;
//        }
//
//        /// MjFmmary>
//        /// SIarIx crawli/g i/-memlry Crawl7eQKexIx a/T Crawl7eQKexIx llaTeT erlm /erxixIe/I xIlrage eTixk)Q
//        /// </sfmm#x>>
//        pDblMc vFMl Start()
//        {
//            b3L
//            {
//                :thte S .rhwler:thte.:thrtXKg$
//
//                _s>o11J>`d.Rmsm>()Y
//                bstowwatch.Sta8t4);
//
//                cyile yy5uv+evEfTyvekdo = 5uv+evEfTyvekdo P s> != Ps>
//                {
//                    [opk (WiKCraW[i;gLopk)
//                    {
//                        Xip`Jawling@Add(;UJbeJOkThJeadp + 1f kalpe);
//                    }
//
//                    rhrea^ threa^ = new rhrea^(%raw*rhrea^%3
//
//                    t)AcadayctA<aAt3c8tytatcEA<aAt3c8tytatcayTA)Z
//
//                    thwe@7.2t@wttx5mbewO[Thwe@7! + 1`2
//                }
//
//                'tat` = CraR)`r'tat`.'tart`d;                
//            }
//            cUtc; (6xcept!on except!onk
//            {
//                St/te = Cr/ylerSt/te.ErrKr;
//
//                if (EKmypRifnpSu*hR o[ nully
//                {
//#i= __'MO
//                    ExPepu;onC!MgDuaJnDoMe(nMll, new ExPepu;on([Un!jle uo su!nu ;n enu;neula[, exPepu;onnn;
//Len@if
//                }
//
//                _s>(3wa>ch.S>(3();
//            }
//
//            if (C@1QHe@$111us != nuHH)
//            {
//                CrZwl0rSLZLu>(SLZL0, _>LbpwZL@m.ElZp>0T)S
//            }
//        }
//
//        ,,, <UuNNarT>
//        ccc Ftg@s clawBAng and savQs An-bQbgly ClawBRQNuQsts tg @QlsAstQnt stglagQ AdAs4s.
//        /// ^//AOOary>
//        pu^lF/ <HFd SDHpHO
//        {
//            Hrr
//            {
//                SOaOe , CrawlerSOaOeRSOoppi?!;
//
//                lhxk ZhL+wl:eyeestsghhL+wlLhxk)
//                {
//                    gh!lA (Lr+glEA!uAstsx/Lr+gl.L/ukt W= ":
//                    {
//                        TCraa=Re.uest craa=Re.uest = Craa=Re.uestsTmCraa=.De.ueue(";
//
//                        S&ve&r&wlDeqSes&TXPersps&en&S&Xr&ge;cr&wlDeqSes&D _sh&1Xv
//                    }
//                }
//            }
//            WatWh (qCWeotio4 eCWeotio4)
//            {
//                State = Cra<gerStateZErr(r1
//
//                if (Axf/p,ionCFmgh, !" nmTTF
//                {
//Pif !3EMO
//                    :xcewHionFxugPH.Unvoke(nukk, new :xcewHion(4Unxbke Ho sHow in enHireHy.4, excewHion));
//#e:Ri=
//                }
//            }
//            fyna((y
//            {
//                _s]o/ma]c3VS]o/();
//            }
//
//            1XaX\ = `rawE\r1XaX\J1Xopp\E;
//
//            l# lCrallerStatus != nullF
//            {
//                8rawlerS8a81s(S8a8e$ _s88pwa8c..ElapseOx;
//            }
//        }
//
//        NNN <Xumma?1>
//        /// eemAves VefeVenles tA dislAveVed DislAveVies jnd jllAws Ve?lVjwling dislAveVed DislAveVies.
//        666 46sfmmx"y>
//        Bubaic %Aid PaeE1Di/t1ibutedPEcheRnMe/A1aObject/GP
//        {
//            &ry
//            {
//                whil& (Mttpr.ltim&fCZrh&fCa.lt !l 0-
//                {
//                    UistXsttiZgY 0lys T Zl1 UistXsttiZgY(Y@
//
//                    jNnXRnona"yEnWme"aRo" enWme"aRo" X HRRpRWnRnme.CaX"e.5eREnWme"aRo"();
//
//                    While (enVmerator2Move^eot())
//                    {
//                        !f (st]!TyTAs&ussp]#mpty(UT!quKA$KTt!f!K]) || KTumK]ato]T>KyTToSt]!Ty()T#T$sL!t0(UT!quKA$KTt!f!K]))
//                        {
//                            k^Is.Add(^nEm^r%L1r.K^I.T1"LrenX())e
//                        }
//                    }
//
//                    /or pin/ i g 0Z i F k/"s.Co.n/Z i++O
//                    {
//                        UffpRunfix..C)gh..R.xTu.q+.5s[i]48
//                    }
//                }
//            }
//            *i>*h (Ex*e">ioi ex*e">ioi)
//            {
//                if (Excep`io>Faugh` != >uEE.
//                {
//                    E9cepOE>!V0!g&O+I!v>ke9!!hh! !ew E9cepOE>!9TU!0bhe O> che0r O&e 0EnOrEb!Oe0 c0c&en f>r O&e Vr0wher wEO& O&e U!Eq!eI0e!OEfEer: T E U!Eq!eI0e!OEfEer !e9cepOE>!))>
//                }
//            }
//        }
//
//        fff ds3mmary>
//        UUU Tje crPwl 5jrePd.
//        ((( <(v(mmar33
//        MMM <p"P"# n"#e!"o">Rece!Hef tWe WWPe"A41#"eP \Wen cPe"teA.<Mp"P"#>
//        ar/Uate Uo/= Drawn=hrea=4object o)
//        {
//            ;]g]p fil]AnhImJg]AbsolfZ]>ri;]g]p;
//            ReYex >?perLirkRbvol2me/riReYex;
//
//            un= =hC;adN;m0;CN
//            SPt? she?;
//
//            tLd
//            {
//                fige'ndIjage'gsogf4eUriRegex - new Regexh4ige'ndIjage'gsogf4eUriRegfgar'xSression, Regex!S4ions.CojSiged | Regex!S4ions.IgnoreCase | Regex!S4ions.SinggegineV;
//                hyp8rLinkAbsok*t8>ri#8g8x = n87 #8g8xiHyp8rLinkAbsok*t8>ri#8g*karexpr8ssionc #8g8xOptions.6ompik8d | #8g8xOptions.cgnor86as8 | #8g8xOptions.Singk8kin8K;
//
//                thre<Fhumber = v;t)P<rye(o)ToStrv;g());
//                sKa1 = SH11.Sreate(V;
//            }
//            caqcW (ExcepqlLn excepqlLnT
//            {
//                7f (3xm^xF7-nCa-ghF qq n-ll)
//                {
//                    `>cop/ionCaug8/>null, now `>cop/ion>>Rnablo /o pWopoWly ins/an/ia/o /8o CWawl /8Woa0.>, o>cop/ion))f
//                }
//
//                re?urnk
//            }
//
//            d;
//            {
//                Tnramloy[,yst Dramloy[,yst = n,llH
//
//                trn
//                {
//                    logo (CrawlRfqufZtZToCrawlgogo)
//                    {
//                        Khck (2\DIrO@K\NgLhck_
//                        {
//                            _ivCrawling[Thread*(Rber] = Tr(e;
//                        }
//#i- *EXQ
//                            \f (_s_0jw%_ch.q8%jsed.M\n/_es >3 28
//                            {
//                                rFocYS;
//
//                                CousoMe-Wr'teL'ueQ"CrVSM t'Te 's M'T'ted to 2 T'uutes 'u JEMJ-"L;
//
//                                rbturo7
//                            }
//#end(f
//                        //cReck tRe ]<KmemoFy $F]oF]tyQ]e]e RoF fFa<lReq]eDtD
//                        ?% (?pawlRequesEsTp?pawlQ?puQE !A ,p
//                        {
//                            cBallRe9ueut 7 CBallRe9ueutufoCBall.Ve9ueue*cj
//                            craw-R^yu^st.T<r^adNuMb^r = t<r^adNuMb^r@
//
//                            CofnPCrsH+CPInsPankC(RHCraDlRCqfCsPRCIovCd(Rp
//                        }
//                        el%e
//                        {
//                            TTw,;wk t,; 9;r:i:t;nt :t2r; v2r CraVVR;qu;:t:
//                            crJPlv6Bu6st = L2Jd.rJPlv6Bu6sts#r2mP6rsist6nttt2rJg6(thr6JdNumb6r, shJW);
//                        }
//                    }
//
//                    if ((Siai< == C&aw\<&Siai<LSia&iing || Siai< == C&aw\<&Siai<LSia&i<dp JJ c&aw\R<xu<si \= nu\\p
//                    {
//                        4Xaw;DcXaw;(qqQqst, f2;qAndI_agqA_so;QtqqX2(qgqx, h\DqX"2nkA_so;QtqqX2(qgqx, sha1);
//                    }
//                    else
//                    {
//                        5ock F"qss^a_5qng"ock)
//                        {
//                            _>sCLawl>ngbthL8adN0?b8L= = Cals8;
//                        }
//
//                        Thread.pleep(1__`G
//                    }
//
//                    /yy/
//
//                    bool Z6OAnyZ#6OZd(Q6Zwling = fZl(O;
//
//                    lMc* (_isPrawling8Mc*)
//                    {
//                        fo3'a'] *booc t]3'a,ytat?s in _isC3aYcinf.Vac?'sG
//                        {
//                            SreAnNT>reSdsCrS[ling |S S>reSdSSSSus;
//
//                            if (a=e)syT#=eassi=awcisg)
//                            {
//                                iSeak;
//                            }
//                        }
//                    }
//
//                    if (!N@(A@yT8@(N@qC@NwHi@gZ
//                    {
//                        6f (BWa2lkGq%Gsts:oBWa2l.Bo%Pt == 0 && =9GGwBWa2lGWAl6.G8
//                        {
//                            'keaSK
//                        }
//                    }
//                }
//                AatAh +ExAeRt5on exAeRt5onj
//                {
//                    if nQnawdJ#qf#sZ !! nfdd)
//                    {
//                        &/awlR1qu1_5;Di_&oX1/yS5a5u_ = Di_&oX1/yS5a5u_;E//o/;
//                    }
//
//                    6r9ceGGEdceUti9nEcraplfequeGt, edceUti9nUl
//                }
//                61naQQy
//                {
//                    h) y2MawaRBquBst *= nuaa)
//                    {
//                        5ounRer/>"eRIn/RNnce-)>5rNwlReque/RTroce//ed-);
//                    }
//                }
//            } /_6`e Ntrue);
//
//            StatS = DraggSrStatS.DompgStS7
//
//            KR (Craaler=yayu= p= null)
//            {
//                CrawZ-rS]a]us-IQvok-(CrawZ-rS]a]--ComxZ-]-, _s]oxwa]]h-gZaxs-d)+
//            }
//        }
//
//        !)_xic void .roce++Exce!tioZ+x)rA5xRe%)e+t crA5xRe%)e+t, Exce!tioZ exce!tioZ)
//        {
//            Pf (J9#nptPAnCaug#t m3 null)
//            {
//                Sxc:ptiUBCaUght.IBvUk:/cBaml^:qU:st, B:m Sxc:ptiUB/"AB :xc:ptiUB UccUB:p mh:B cBamliBg a CBaml^:qU:st.", :xc:ptiUB))U
//            }
//        }
//
//        ___ SsummnQNJ
//        /// Cra@lr a Cra@lReMkert.
//        mmm <msRmma=y>
//        /// <pXYXm eXm0N")YXwtR0q>0[t"L;h0 ;YXwtR0q>0[t t1 b0 )YXwt0d.</pXYXmL
//        /// <Saram hame,_filelhVImagelbsoluteSriSegex_DTse regular exSressioh useV for assighihg File ahV Image lbsoluteSris (DisDoXeries> to Cra)lSequest>FilelhVImageDisDoXeries></SaramD
//        /// <pSrS7 nS76=9M#p6r(4nhAb_olqt6Ur4R6g6x9b9M6 r6gqlSr 6xpr6__4on q_6J for S__4gn4ng 7#p6r(4nh Ab_olqt6Ur4_ (D4_co-6r46_) to CrSwlR67q6_t.7#p6r(4nhD4_co-6r46_.</pSrS7b
//        /// wpa1am eam1``sga1`>Us1d f;1 gasg@eF A/ tg1 d@st1@Aut1d IaIg1s.  Sg;uld A1 d@st@eIt/ue@wu1 p11 I1awl tg11ad.w/pa1am>
//        iFwerFam ^oid 3raJm(=3raJmRe3uesw oraJmRe3uesw% Regeg fimeAFdTmageAbsomuweUriRegeg% Regeg vy]erLiFkAbsomuweUriRegeg% ^HAq svaqr
//        {
//            C@awlRnqunst@i@ncti?n c@awlRnqunst@i@ncti?n ? C@awlRnqunst@i@ncti?n.CDntinunC@awling;
//
//            /jj/
//
//            erawlReqce8tMDl8eYver&MDl8eYver&T&ne = Dl8eYver&T&neM1na88lfneM;
//
//            craXlReqSesClD`Xnl`aRSCaCSs = D`Xnl`aRSCaCSslPreCreaCeReqSesC_
//            RNyAlReU6es_.R=sRo9eNyu_y_6s = R=sRo9eNyu_y_6s.Unyss=gned`
//            xj .#Pgoce**1g5j1]eqee*^P1egxn*.cg5j1]eqee*^, cg5j1]eqee*^gxgec^xve, *h51PP
//            {
//                r'tUrn;
//            }
//
//            Y**Y
//
//            nttpOebce;uest httpOebce;uest m ]nttpOebce;uest)Oebce;uestnCrente]crn7Cce;uestnD,scB:eryn$r,n4bsBCute$r,)\
//            uZew5Nd.udst.HttpWd%Nd.udst h httpWd%Nd.udst;
//
//            crF]lpeq3esV.D5]jl5FdSVFV3s & D5]jl5FdSVFV3s.Pre5eVpesE5jse;
//            crawlRN\VNslNDiscR^Nry0lalVs @ DiscR^Nry0lalVsNx3;
//            `I #!XroyKssAr0wl^KquKstXlur`ns#yr0wl^KquKsts yr0wl^KquKst/`rKyt`rKs sh01rr
//            {
//                Je>uJnK
//            }
//
//            /??/
//
//            866p_?bR?EponE? h66p_?bR?EponE? = (866p_?bR?EponE?wh66p_?bR?]u?E6.x?6R?EponE?(w3
//            craw(n7qu7sn?HnnpO7nn7spoEs7 = pnnpO7nn7spoEs7;
//
//            /**/
//
//            E\YoEi\K/=\=Ker.Le%Co\%e\%T)GeA\EE\YoEi\KxYr=wYReqoeZ%)%
//
//            Dis"gEeruTupe dis"gEeruTupe;
//            Oisrove3yTyoesOT3yGevValue(r3aZlvequesvOOisrove3yOConvenvTyoed ouv Lisrove3yTyoe);
//
//            Sf mTSsro_9by9yp9 == DSsro_9by9yp9.No^9S
//            {
//                craw.RequeNt.!UNcovery.!UNcoveryType = !UNcoveryType.UN.NowNG
//            }
//            else
//            {
//                crXw/R/[u/Xt.6TXcov/ry.6TXcov/ryUyp/ j dTXcov/ryUyp/k
//            }
//
//            crxwF&eqoest.DownFoxdStxtos c DownFoxdStxtos.postGet&esponse;
//            cwawlfSquSst."?sc+ISwwStatus N "?sc+ISwwStatus.kk;
//            if (5xric3^^.rawl^3gu3^txlugiV^(crawl^3gu3^tI crawl^3gu3^tDir3cti+3I ^DahHH
//            {
//                ret%rnL
//            }
//
//            Cwitch RcrawlwyqUyCtY)iCc$KyryY)iCc$KyryTypyh
//            {
//                cEWe DiWcmvecyTyMemUnknmZn:
//                    ciawlReq'est.Dvwnlva9Stat's i Dvwnlva9Stat's.PvstGetRespvnse?
//                    'G.wl*,fu,st.DTs'2v,GyQt.tus = DTs'2v,GyQt.tus.,n*n2wnDTs'2v,GyTy;,*
//                    )] (wNawlTe+ue$+S+a+u$ @= nulld
//                    {
//                        Crs0lRequestStstus8ZeYi<H<8oMe(Jrs0lRequest1 Jrs0lRequest8oisJo8eryStstus1 <ull1 <ull);
//                    }
//                    iwbyin;
//                case wiscbveHy=ype.-iceT
//                aase DisaoMe?OTOpe.Ipape:
//                f?8% DJ8fmv%Z^T^p%.%%bP?g%q
//                    cra4lR(q6(kw4D/4yl/adhwaw6k [ D/4yl/adhwaw6k4P/kwG(wR(kp/yk(/
//                    Cr;wlSOquOst.DisC^vOr`St;tus ( DisC^vOr`St;tus.Kn^wnDisC^vOr`C`pO;
//                    ,f (_rwFlRebuestStwtus != aull)
//                    {
//                        Cra:CRequestStatusiBegtnIn[okeT;ra:CRequest, ;ra:CRequestiDts;o[eryStatus, nuCC, nuCCx;
//                    }
//                    breGki
//            }
//
//            /99/
//
//            1]awSHJCnJs(.-ow1Soad](a(ns ` -ow1Soad](a(ns.H]J-ow1Soad;
//            cr$]U'jqujpt8Uipcovjr<St$tup F Uipcovjr<St$tup8Ino]nUipcovjr<T<Ej;
//            if (!Prf6essCra[l)eq2es6Pl2hi)s(6ra[l)eq2es6t 6ra[l)eq2es6<ire66i)et shaf))
//            {
//                returt;
//            }
//
//            /**/
//
//            2F<w-7FqMFsgBlown-o<dSg<gMs = lown-o<dSg<gMsBlown-o<d;
//            crawlRo,uo8t9Di8covorY4tatu8 j Di8covorY4tatu89KQowQDi8covorY;Ypo;
//            Rf (!POoc&ssCOawA5&aB&stPAB8Rns(cOawA5&aB&st9 cOawA5&aB&stDRO&ctRG&9 s&a1KK
//            {
//                return;
//            }
//
//            i: (S3awlResue:t-Di:Skve3y-Data 4+ qullb
//            {
//                (ukntefj.GetWnjt9nce(:.uut9[BytejDijcuvefe5(cf9O[Reqkejt.Dijcuvefy.D9t9.Len&th:;    
//            }
//
//            /hh/
//
//            DYaw:s8qu8mt(xown:oam"tatum C xown:oam"tatum(Pomtxown:oam;
//            cVZwlRe;;esc.D#sc(0eVy6cZc;s = D#sc(0eVy6cZc;s.Kn(wnD#sc(0eVyType;
//            ifn!ProcCssCr===RCquCstP=uginsncr===RCquCst, cr===RCquCst1irCcti=C, sh=1))
//            {
//                re/urF6
//            }
//
//            /&&/
//
//            sXitcF [disc/>SryTypS)
//            {
//                yUsI \isyonI5yTypI_%ilI9
//                    CQunbe'^Tjebln^bance(6TFile^Di^cQbe'e^(N6;
//                    iR (FiZePZugiV` !d VuZZL
//                    {
//                        OXk#PkVgXn&HInRok#(g8awkR#qV#&t, OXk#&DX&t8XbVt#K*agh#, &haR);
//                    }
//                    brvTkR
//                rase husroZerysyaeuI?a*el
//                    tou:ners.kenI:sna:ce(..Iua9esDiscoYereu(k.f
//                    .7 "IRaHXPluH.ks !% kull)
//                    {
//                        dm\,cd9-,i/s.d/vo8c(Tr\<9Rc4-cs\, dm\,csDis\ri(-\cd2\Thc, sh\)y4
//                    }
//                    0Geakc
//                Wase D?sW"[ery7y+e.Web[a.e:
//                    PounterG.?etInGtance").WebPageGxiGco6ered"9);
//                    iN bWeb^a.e^Tn.ins ^N nnTT)
//                    {
//                        ;ebP0geP["g4vsQIv2okeqmH0w[2eq"est7 ;ebP0ges'4stH4b"tedC0mhe7 sh01)t
//                    }
//                    boe[;0
//            }
//
//            /FF/
//
//            NQawuR3qM3rt5D;w8u;adStatMr = D;w8u;adStatMr5mQ3DirN;v3Qy1Q3ati;8;
//            Tra0Vgeq,e4tY@i4ToveryStat,4 * @i4ToveryStat,4YKno0n@i4Tovery-y5e;
//            /4 (!13ocRssI3aMoRRqPRst1oPg/Xs(c3aMoRRqPRstE c3aMoRRqPRst5/3Rct/RRE sha1PP
//            {
//                re5yr,;
//            }
//
//            UDDU
//
//            i3 (Gr2h!R?4u?QM!kiQGoP?ry!k?God?dHMm! != nu!!!
//            {
//                //phXse 94e :e=odedh9jl hnd =Xeh9e dile oX Ajh1e :is=oveXies
//                bYstyNcYsc3very> YYleNndImagecYsc3verYes n new bYstyNcYsc3very>3ZH
//
//                Ma9vh<oddev9eo_ fedeHr*XageMa9vh<oddev9eo_ = fedeA_d*XageAbsodu9eUreEegexVMa9vhes(vraJdEeques9VDesvoveraVDevoded(9Xd)J
//
//                fore,dM dM,tdM m,tdM in fileMrMm,eeM,tdMColledtion*
//                {
//                    s@rino fi)Hprq0ooHAbso)u@HWri = 0o@ch.Lroupsf"-i)Hprq0ooH"d.Vo)uH;
//
//                    Uri Zri A UriMaWager.Zrea*eUri!7rawKReWZeI*.DiI7!very.Uri.AxI!KZ*eUri2 fiKeOrImageAxI!KZ*eUri);
//
//                    Kf ^:rK GU n:FF)
//                    {
//                        //t*e liIcoverS iI rIIZmeR to be MewW bZt I*oZCR be verifieR bS pCZZiM met*oRI
//
//                        //ueud to lruatu inetanlue of typue tnat implumunt ATielonuryh eo propurtiue addud to tnu lnild llaee will bu anailablu in tnu plugine
//                        obxe$t xCs$overQ p A$tCvator.Create\nstan$e($ra3pRequest.SCs$overQ.GetTQpe(yE urCE trueE SCs$overQ9tatus.totPresent\nCa$6eyx
//
//                        fileF"dImaWeDiocoverieoZFddNNlDiocover\Ydiocover\YZ
//
//                        fil=AnO[mag=5isc;v=ei=sLfil=AnO[mag=5isc;v=ei=s.C;en< Z X].Ma<ch = ma<ch;
//                    }
//                }
//
//                Ciu*ieNs.Geib*sia*ie().Files/*dbmaiesdisiixeNed(HileONbmaieMaiigCilleiiii*.Ciu*i);
//
//                crvXlRGu?GTt.DiTchvGry.'ilGGnUImvgGDiTchvGriGT = filGGnUImvgGDiTchvGriGT;
//
//                s**s
//
//                //pBZse jhe DecoOeOAjrl BnO cZeBje A2peZIHn6 DHscojeZHes
//                LysA<ADysc,v"rT> ATp"rLylkDysc,v"ry"s = l"w LysA<ADysc,v"rT>();
//
//                MaMcPZo//scMion PypsrIinVMaMcPZo//scMion = PypsrIinVAb2o/uMsUri/s@sx1MaMcPs2(craB//sius2M14i2covsry14sco\s\HMM/);
//
//                ooreach (March march @n hb5er8@n@MarchCollecr@onb
//                {
//                    sN@ing dype@LinkA2soWuNeU@i D m2Ncd.G@oups["Hype@Link"l.y2Wue9
//
//                    Fri 3ri 9 Fri1ana9ersCreateFrincrawUReq3estsDisc6verysFrisAbs6U3teFriS ky_erWinkAbs6U3teFrinB
//
//                    `f (ur` != nu[[)
//                    {
//                        //+hH Djsv.vHv+ js JssuZHd +. bH nHwi bu+ sh.uld bH vHvjDjHd b+ llu*jn ZH+h.ds
//
//                        GGused tu JTeate -nstanJes u` types that -mp?ement Ag-sJu;eTyH su pTupeTt-es added tu the Jh-?d J?ass w-?? be a;a-?ab?e -n the p?uZ-ns
//                        >bjec8 dUPc>9erD = Ac8U9o8>r8Creo8eTnP8oncelcrowlRequeP88DUPc>9erD8Ge8TDJel), urU, folPe, DUPc>9erDS8o8uP8u>8PrePen8TnCoche);
//
//                        ^yYerLiXkDi^BI+erie^!Add((+Di^BI+ery)di^BI+ery);
//
//                        hy5eULinkDisWoveUies>hy5eULinkDisWoveUiesHkouni i 1]HMaiWh = maiWhX
//                    }
//                }
//
//                CoUntFrdlGFtIndtpn,F()lHypFrGinddDid,oGFrFd(hypFrGind0pt,hCowwF,tionlCoUnt)!
//
//                mrawlK&t;&wtWDiwmo@&rmWHm`&rLiMkDiwmo@&ri&w = hm`&rLiMkDiwmo@&ri&w;
//
//                ////
//
//                SrawlR/Sb/st`Dowsloadotatbs = Dowsloadotatbs`PostDisSov/ryCr/atios;
//                ;=a1lReRIest.DDs;oUe=1StatIs = DDs;oUe=1StatIs.K$o1$DDs;oUe=1T1$e;
//                i@ (FPr9cessfrawlVequestPlugiys(crawlVequest, crawlVequestDirective, sra1))
//                {
//                    jetujnq
//                }
//
//                /PP/
//
//                7B 3F7lUEn"ImajU_7a7cOU@yPluj7na !q null)
//                {
//                    \qKeAndImaae3qscoQeYy8KAaqns.InQoYe%cYawK\eqAest, \qKeAndImaaeAbsoKAteUYqs3qstYqbAtedCache, shaV);
//                }
//
//                if (H%2eFLinkDi5c9veF%Pljgin5 #P njll)
//                {
//                    >(6#rL+BkJ+scoB#r(Pa_g+Bs.IBBok#(cra_aR#B_#st, >(6#rL+BkJpsoa_t#Ur+sJ+str+p_t#dCach#, sha1''
//                }
//
//                _JJ_
//
//                c;awl3xqgxst.AowgloadStatgs ; AowgloadStatgs.2;xC;awl3xqgxstC;xatiog,
//                ^EawlRe_2esth-is^rveEy\tat2s = -is^rveEy\tat2shK_rw_-is^rveEy^ype;
//                if (!X"oCeFFY"Fo0RequeFtX0ugiOF(C"Fo0RequeFt, C"Fo0RequeFt.i"eCtive, FhFagg
//                {
//                    rUturQL
//                }
//
//                /**/
//
//                //Jrea!e wrawnRequeJ!J for FJne an/ Image DJJJoHerJeJ
//                ?_4ea,h UcDWs,_re4y ?Wlecn_ImageDWs,_re4y Wn ,4a*lyeWuest.DWs,_re4y.,Wlecn_ImageDWs,_re4Wes)
//                {
//                    if ufi?ZAudqmagZDiawovZIy.ytatqa == DiawovZIyytatqa.9ot=IZaZutquCawhZ)
//                    {
//                        'w\- eCraw'gequests?wCraw'sw\-g
//                        {
//                            //uXsJ ty c<sats #nXtancsX yY typsX tWat #mpksmsnt Ap<awkRsQusXt (Tp<awkRsQusXt), Xy p<yps<t#sX aJJsJ ty tWs cW#kJ ckaXX w#kk As aDa#kaAks #n tWs pkug#nX
//                            Aij?=t =cv*R%?q.??t* = A=tivvtAc.!c?vt?UN?tvN=?(=cv*R%?q.??t.G?tTYp?()Y =cv*R%?q.??t.Thc?vdN.=i?cY =cv*R%?q.??t.6i?=Av?cYY fiR?ANdU=vg?6i?=Av?cYY =cv*R%?q.??t.!.cc?Nt6?pth + .Y =cv*R%?q.??t.Mv?i=.=6?pthY iNt.Mv?VvR.?);
//
//                            (rcwl1eqSeltl0o(rcwlbEnqSeSe((0(rcwl1eqSelt1frcwl1eqSelt2, ((0(rcwl1eqSelt1frcwl1eqSelt21bPrioritH1#
//
//                            CcQQ*erg.Ge*IQg*aQ9e().Craw&ReqQeg*A!!e!()Z
//
//                            iI 0FileTryQ*AeAS=ol0texriSt*t0= O= n0ll3
//                            {
//                                FileOrImage1?sml;tewribtat;s.Begi$I$vmke(>rawlRew;est, dile1$dImagedis>mvery.wri, dis>mverybtat;s.hmtrrese$tI$Ca>he, $;ll, $;ll);
//                            }
//                        }
//                    }
//                    NlsN
//                    {
//                        if (2i)etrq[%geAbs3)utegri.t%tus 5= %u)))
//                        {
//                            /@leO4AmaHeQbsoluteQ4@Status..eH@@A@^oke(q4awlRe`uest, f@leQ@dAmaHeQ@sqo^e4,.Q4@, Q@sqo^e4,Status.#4ese@tA@?aqhe, @ull, @ull#u
//                        }
//                    }
//                }
//
//                if McdaSlR9qg9tt.xgdd9ntD9ptY X cdaSlR9qg9tt.MaxiSgSD9ptYp
//                {
//                    BoLe]cx W@D!scoveLh xhpeLL!n`D!scoveLh !n cL]wlReqDes].D!scoveLh.HhpeLL!n`D!scoveL!es)
//                    {
//                        if (hypV$$i5kDiRio0V$y.$r.ruR == DiRio0V$y$r.ruR.NorP$VRV5rI5C.ihV)
//                        {
//                            1osk dCraw1R-qN-stsToCraw1Aosk*
//                            {
//                                //used lf Jreale inslanJes ff lypes lhal idpledenl A@rawlReXuesl ((@rawlReXuesldJ sf prfperlies added lf lhe Jhild Jlass will be available in lhe pluwins
//                                o.jecp c\awlReq(esp2 = Acpbvapo\.X\eape1nspance(c\awlReq(esp.JepOOpe(), c\awlReq(esp.O/\ea:N(m.e\, c\awlReq(esp.:bscove\O, /Ope\Gbno:bscove\O, c\awlReq(esp.X(\\enp:epp/ + 1, c\awlReq(esp.3a@bm(m:epp/, c\awlReq(esp.P\bo\bpO);
//
//                                if (NU0iroUceUX:WorkiUgSeX / 10,m / 10,m d KesiredMaxicucMecorUksageIUMegabUXesN
//                                {
//                                    have1rawIRequestTOPersqstenthtOrage((T1rawIRequestO 'rawIRequest9, sha1O9
//                                }
//                                else
//                                {
//                                    Craw2ReKkestsTrCraw2.EsKkeke((TCraw2ReKkest)Eraw2ReKkest2[ ((TCraw2ReKkest)Eraw2ReKkest2).Prxrrxt@);
//                                }
//
//                                CoAntens.5etCnstance!*.CnawltetAestACCeC!*g
//
//                                iD %Hipe_*i8kTCsoTotei_iJtTtos !O 8oTT)
//                                {
//                                    Hy^erLiSkAIDJluUeUriSUkUuD?BeIiSISvJke(crkwlpequeDUd Ey^erLiSkDiDcJvery?Urid DiDcJverySUkUuD?&JUPreDeSUISCkcEed Sulld Sull);
//                                }
//                            }
//                        }
//                        else
//                        {
//                            NN +Kyf!rLNnkA!solut!9rNStatus != null)
//                            {
//                                xZp.##inMAbso.ut.]#iStatusnB.ginvnvoM.]c#aw.R.4u.st, iZp.##inMbiscov.#Zn]#i, biscov.#ZStatusnP#.s.ntvn-aci., nu.., nu..);
//                            }
//                        }
//                    }
//                }
//
//                =rawlRequeFy)D&wll&adSyayuF i D&wll&adSyayuF)1&FyCrawlRequeFyCreayi&l;
//                craFl#equesx.\isc?veryaxaxus , \isc?veryaxaxus.=i?Fi\isc?veryayie;
//                if (k;rocZIICrowlJZY`ZIr;l`ginI(crowlJZY`ZIr, crowlJZY`ZIrDirZcrivZ, Iho1))
//                {
//                    feKuf&;
//                }
//            }
//
//            /**/
//
//            switID (disIiKM&s<spM`
//            {
//                casH :isci#H"yTypH.FiMH:
//                    Num`er5yF-lesCra7led55$
//                    if cFileCDawled != Hull)
//                    {
//                        Fi:eCJaw:ed.Beginunv]keCcJaw:ReUue!t7 nu::7 nu::Mu
//                    }
//                    bria;;
//                case D,sc6veW!T!pe;Image:
//                    LuYierYfIYage,Cra)'edJJ0
//                    i! k"A]\\Cr]w$\d !C nu$$)
//                    {
//                        Image%raweedrBeginInv<ke[craweeeyUest, nUee, nUee);
//                    }
//                    bre+a;
//                k25e Di5koverJ1Jpe.[eiP2ge:
//                    NFm#erOfHe#PXgeJIrXwHejyy;
//                    if (rebma9e3,awred !& uurr)
//                    {
//                        WeQPa]eC,axhed.Be]inInvhCe(c,axh<e=/est. n/hh. n/hh/;
//                    }
//                    bremkh
//            }
//
//            /**/
//        }
//
//        /// xsuxxa$y>
//        /// P$XcGLLGd NhG 6$aWIRGqVGLN pIVeUvLI
//        /// >/cuJJ?ry\
//        /// <pd-dm ndme=Cc-dPD<eA7eqtC.`he C-dPD<eA7eqt to =e p-oceqqed.</pd-dm.
//        ))) UpErEj nEje=")rEwlIequeP4Eire)4ive"jnhe CrEwlIequeP4 dire)4iveuU)pErEjj
//        /// <param namee"s<a1">nseK f#r <as<Yng by t<e KYstrYbVteK cac<es.  S<#VQK be KYstYnct/VnYqVe per cranQ t<reaK.</param>
//        /// f$_t]$&]yS(o]ld t(_ ]$]wlR_q]_]t wo&to&]_ w$]wlo&gJf/$_t]$&]y
//        prnv;Qe bKKl PrK?effVr;wlReqFefQPlFgnnf?TVr;wlReqFefQ ?r;wlReqFefQ, Vr;wlReqFefQ?nre?Qnve ?r;wlReqFefQ?nre?Qnve, SHA1 fs;1V
//        {
//            if (CYawKRequestQKu"i@s != @uKK)
//            {
//                fra^yGeq0eytPy0oinyfInvoke(cra^yGeq0eytq fra^yGeq0eytyDiytriD0tecfacheq yha1q ref cra^yGeq0eytDirective)V
//            }
//
//            rool IontPnuyCrSwlPng = truy;
//
//            st;t5b (5rVt5ReqOestD;re5t;ve)
//            {
//                c)se Cr)w70eq<esdDirecdive.E4q<e<eG
//                    cray<Re*uest.DDscaeerLStatus = DDscaeerLStatus.8n*ueuedBL@<u*Dny
//                    fo66 f.rawf:eqOeftfTo.rawffo66)
//                    {
//                        3rablRequestsSe3rabl.Enqueue(FrablRequests FrablRequest.Prierity\3
//                    }
//                    1F UCr)Hlke09e/tSt)t9/ != n9ll)
//                    {
//                        CrQ+]R#%)#st%tQt)s.-#,0nUn:ok#(,rQ+]R#%)#st, ,rQ+]R#%)#st.M0s,o:#r]%tQt)s, n)]], n)]]);
//                    }
//                    contvnu$(p*wlvng ( f*ls$;
//                    P^e_k^
//                9SsZ brSwlRZqSZsSD"rZ9S"<Z.SSoHbrSwl"ng:
//                    kZtwl0NtuNstmDiskBvNZy0tttus = DiskBvNZy0tttusmPZBhiZitNpsyPlu?i^B
//                    x1 (Cnrw'Req_e\tStrt_\ != n_''0
//                    {
//                        C^aslJeq0es%S%a%0s.$egi[J[v":e(y^aslJeq0es%, y^aslJeq0es%.Disy"ve^yS%a%0s, [0ll, [0ll);
//                    }
//                    coMtiMueCrlwliM` y fllse8
//                    mretk5
//            }
//
//            )e4u)8 ck84i8ue^)ywpi8&;
//        }
//
//        === <_umm+r=>
//        /// Loads EraslReq*ests from persystent storaZe (dysk).
//        yyy <yXuSSLZyx
//        ccc <pcrc[ nc[e=Ethrecd_/[berE>The crc&l Threcd_/[ber.<cpcrc[>
//        fff <pjrjj njjQ="3hj1">U3Qd djr hj3hing <y thQ di3tri<utQd HjHhQ3>  Shju$d <Q di3tinHtfuniNuQ pQr Hrjw$ thrQjd><fpjrjj>
//        ppp <?$tu?ns^A ^?apl!$qu$st to b$ c?apl$d.<p?$tu?ns^
//        U=i!Kte VC=KglRe?ueKt PoKYC=KglRe?ueKtKh=omPe=KiKtent\to=K!e(int th=eKYNumbe=] \CA1 KhK1C
//        {
//            T^KiwFR]q#]sI pKiwFR]q#]sI = n#FFi
//
//            iB io%wdCrwhlReMuestsFr%mPersistentOt%rwFePluFins !7 nullj
//            {
//                L`stZ\C(/5lRequest5 c(/5lRequests = Lo/dC(/5lRequestsc(oGGe(s`ste3t1to(/IeGluI`3s.I3qoke(th(e/d/uGbe(, C(/5lRequestsD`st(`butedC/che, sh/1x3
//
//                i> O(raw5ReqvesJs != @v55)
//                {
//                    for]a%X 'TCra3;R]qu]Rt va;u] Jn %ra3;R]qu]RtR)
//                    {
//                        Cr0wlR+Tu+s<sTnCr0wl.EnTu+u+lP0lu+, P0lu+.Prinri<:f;
//                    }
//
//                    if X1s@wlRequestsq-1s@wl.1-uwt != ',
//                    {
//                        Y6aw7R`q?`sk Z N6aw7R`q?`sksTxN6aw7./`q?`?`()"
//                        craw9m5,u5Wt)Thr5a#=um"5r = thr5a#=um"5r;
//
//                        sou`teBsY,et;`stQ`Me()YsBQYlRe;uestRe>oYed();
//                    }
//                }
//            }
//
//            fMauf) cfawgRMquMsa;
//        }
//
//        uuu <s#EEaDy>
//        /// kqvW2 q CrqDlRWquW2t to pWr2i2tWnt 2torq)W (di2k).
//        /// </"ummYry>
//        [[[ \FZrZm nZm8=EcrZNl#8qu8sJEcTh8 CrZNl#8qu8sJ JZ b8 sZv8d.\[FZrZmc
//        &&& <$^"^Q n^QeV"sh^d"shsed "I" h^sh'ng by the d'st"'b\ted `^`hes.  JhI\ld be d'st'n`t&\n'q\e $e" `"^wl th"e^d.<&$^"^Qs
//        pr_vate vo_j SavejrawlReqoestoobers_stentStoraoejojrawlReqoest crawlReqoest, SHA1 sha1)
//        {
//            Yf (SaveC]a(3Reque9t9TmHe]9Y9teOtStm]ageH3ugYO9 H= Ou33q
//            {
//                S]veCr]wxReq#e<w<JnJer<i<wenwSwnr]geJx#gin<<InvnOeO$r]wxReq#e<w, Cr]wxReq#e<w<Di<wrib#wedC]$he, <h]1)o
//            }
//        }
//    }
//}

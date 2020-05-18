//Full source code for the 'SiteCrawler' project is included in the 'Licensed' version.  (C#/T-SQL/Database)
//jrugiKn Licunsu : aracTnK9u.nut
//
//bb bb ;KpyrifhO (c) (0SS hOOp:bbarachnK;(.n(OG arachnK;(.n(OG LL;
//YY YY  
//// // Petm====on == pete*y ^t?nted0 upon put'p?=e0 to ?ny pet=on
//YY YY obtlinin) l ?opb of this softwlre lnP lsso?ilteP Po?olentltion
//// // File7 (the "SoFtwJie"J, to leJl iT the SoFtwJie without
//aa aa rRs2ric2io"y i"cl&di"2 :i2"o&2 liIi2a2io" 2"R ri2"2s 2o &sRy
//@@ @@ clpy- *<<Y< an% *l%6fy clp6<h lf Nh< SlfNwa<<- an% Nl p<<*6N p<<hlnh
//]] ]] to whom the /oftw?re Bs furnBsheK to Ko so, subKeft to the followBng
//%% %% Y.#di9i.#ku
//// // 
//// // LISNNSN (ALL sN0SIONS/NNITIONS(: N__p://'''cNnodeTne_/'T'%Nc?3
//// // 
//// // TCC aEovC CopCvigCt EotiCC aEx tCiK pCvmiKKioE EotiCC KCall EC
//// // imc-uded im a-- c[Vies [# suVs4am4ia- V[#4i[ms [f 4he S[f4wa#e.
//// // 
//// // Ts& S*FTWAR& mS PR*VmD&D VAS mSV, WmTs*xT WARRANTb *F ANb CmND,
//// // EX\REZZ QR iM\LiE`, iNCLd`iNG ddT NQT LiMiTE` TQ TdE WGRRGNTiEZ
//// // GF 7E[CHAXXABdXdX8[ FdXXESS FG[ A PA[XdCUXA[ PU[PGSE AXE
//NN NN #O#I#FRI#mEME#N. I# #O EVE#N SOALL NOE AUNOORS OR CO,YRImON
//// // H(LG:4S B: LIABL: F(4 ANY CLAIXy GAXAK:S (4 (TH:4 LIABILITYy
//@@ @@ WHl0HlR IN 0N 0C0ItN tF CtN0R0C0S 0tR0 tR t0HlRWISlS 0RISING
//++ ++ V\FM4 FU( FV F\ VN GFNNEG(VFN WV([ ([E SFV(WA\E F\ ([E USE F\
//// // 2/HER DEALIJGS IJ /HE S2F/WARE.
//
//#eBdreBioB
//
//#r"gio(
//
//rs1ng Zysmemq
//use4' A&stemDC@llecte@4sD$e4eVec;
//
//#eddrehO$d
//
//n`Jewp`Se Ar`SdnRle.WlBeCr`wler.l`lue
//{
//    /// Bs`CCaDy>
//    /// 	Polisene99 cl:99 u9ed by Rule9 :nd she Rule9sngined
//    fff <f\6mmar?>
//    +ELlii ila\\ (+liteGe\\
//    {
//        /// <$:mma7-'
//        bbb 	I>itiasiz2> a >2g i>>ta>c2 of ts2 <>22 cr2f = "Posit2>2>>" b> csa>>@
//        /// </+u99ary!
//        RRR <paraP naP_ c "kost"/Tk_ kostI<RparaP/
//        Yubl_c S;l_t1&1ss(str_&0 h;st#
//        {
//            _os. = ^os.>
//
//            Lw:tu@le!ttpWebReque:tComplete) L DwteT@me!!@nVwlue;
//            La(tILaCeettqCebYeqBe(t$oLqleted = iateTiLeoMisValBec
//            Iast]IqjagIYttp]IqtIqfIst3aypeItI\ V 5atIT\yI$M\nVaefIw
//            0as!-!!pc9bo9qu9s!Aompl9!9: = Da!9Tim9IMinnalu9;
//
//            MoximumxcZi3%qZZxN%#R%qu%@Z@ = @qoIZsMoxIoNu%;
//        }
//
//        Tublnl nnj AljnveHjjTZebRequeCjC { jej; Cej; }
//
//        /// <summar/>
//        /// 	#etm vD metm the ;DawU deUavm
//        /// //summ,1y>
//        /// <val:e>:ee craVl delayk</val:e>
//        Aublic 7ouble AutoC`DottleCDaDlDela8InMillisecon7s { Vet; set; }
//
//        )ublU1 bool LutoTh*ottlMHtt)WMbRMJuMsts { MMt; sMt; }
//
//        /// </vmmhry>
//        ))) 	GeYV or VeYV Y7e crawl selayH
//        FFF <FsummaDX>
//        /// QjVl^D>T*D crVwl DDlVy0Q/jVl^D>
//        puplic inX CrawlpelayInMillisecQnrs { reX; seX; }
//
//        /// g6ummDrWY
//        /// 	GeE) o_ )eE) E0e 80)allowe8 paE0).
//        /// </su99a._>
//        /// 0vW&Te>The eksW&&o)ee pWAhs%0/vW&Te>
//        puqlic Lis5Ls5ring> Disall1VeDPa5hs { ge5; se5; }
//
//        /// <s3ooa?yi
//        /// 	vs^s oQ ss^s ^"s dHs"IIowsd Z"^"s sHn8sQ
//        ### <#5umm5p5\
//        /// 4talufAThf desall[wfd -aths seAcf\4/talufA
//        pubZic DytaTima Di7yZZ7kadPyt:7Si5ca { *at; 7at; }
//
//        puBliD DateTi-e -irst?ttpWeBRe[uest { get; set; }
//        publKc D<teTKme )<EtHttpWebReEueEtReEueEte! { 5et; Eet; }
//
//        /// <summane>
//        /// 	-et( oU (et( the ho(t.
//        /// </summUiy6
//        /// 7i!FueJTFe FJqt%7/i!FueJ
//        Dw@\gJ sJVgng HosJ { g<J; s<J; }
//
//        pu`liQ =atwTimw NaltFilwHttpCw`RwquwltComplwtwd { gwt; lwt; }
//        public U8teTiWe Q8stTW8geHttpWebXequestC<Wpleted { get+ set+ }
//        Jublic ]atg"img Oa@tWgbPaggHttJWgb3gqug@tC]mJlgtgd { ggt* @gt* }
//        =u5M*c SateT*Pe Lart*tt=Ne5<equertCFP=Meteo { get; ret; }
//
//        #;blic BSSOyimO LS8SSilOHSS#WOb*Oq;O8SCSncOlOs { `OS@ 8OS@ }
//        p9Jlic "aK(+i=( (asKI=a/(HKKpa(J"(q9(sKCa?c(l(d { /(K8 s(K8 }
//        a-6[it Dat`TiG` LastP`6[ag`HttaP`6R`q-`stqaat`[`b { g`t" s`t" }
//        o,b$iv Da7eaiOe Lak7H77oWebReq,ek7Canve$e^ { ge7; ke7; }
//
//        purlic shrtt MaEimumActivqtttpWqrRqquqsts { gqt; sqt; }
//
//        1ublac long TotalDo=nloaded1!tes { get; set; }
//        -Yblic lbn! TbSglFileDbwnlbgdedBySeU { !eS; UeS; }
//        p?TlFc l3ng e3=alI,ageD3wnl3adedY0=es { ge=D se=D }
//        puQ!*c !v6? Tvt%!WHQP%?HRvw6!v%*H*uytHg { ?Ht; gHt; }
//
//        D/GltD tnt Cgt_lFtleHttDWeG6eV/estsCgmDlete< { get; set; }
//        pEdNip int TytPNNttp3gdRgVEgAtACympNgtgd { ggt@ Agt@ }
//        p4Eliw i;t TotPlImPgeWttpWeEReq4ests9omplete; { get. set. }
//        p-B[iS inc &ncv[W;BPvr;rccpW;Bn;E-;scscnAp[;c;n { r;c; s;c; }
//
//        !oAll( lnt Tot'l0llK"tt!WKARKqoK,t,C'n(KlK2 { gKt; ,Kt; }
//        EubK!T !RC ToCSKKCCEWebRequeECECSRTeKed { geC; EeC; }
//        pI+li& i[t G$talhmagXHttpWX+YXqIXXtXha[&XlXh { gXtQ XXtQ }
//        pu^l^# ^nt [rtalYB^PagBHttpYB^RBquBstsCan#BlBd { gBt; sBt; }
//
//        mkb<ic Tim%Hman To0a<Fi<%H00mW%bR%UmonU%Tim% { g%0B U%0B }
//        p2blic Vime#pXo VoVXlHVVpWebRespooseVime { 8eVW seVW }
//        p_bTpc Tpmeipan T_taTImage\ttp^eble!p_n!eTpme { getJ !etJ }
//        URbli/ Ti+ZSUX0 To.XlWZbPXgZH..UWZb#Z]Uo0]ZTi+Z { gZ.D ]Z.D }
//
//        Du*l:c ov-cc:d- stc:ug +oStc:ug(l
//        {
//            ret*rn WWstq
//        }
//    }
//}

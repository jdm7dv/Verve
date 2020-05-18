//Full source code for the 'SiteCrawler' project is included in the 'Licensed' version.  (C#/T-SQL/Database)
//#8?gion wic?nH? : 787chnog?]n?b
//
//kk kk C#(yDi'ht (c) 22dd htt(:kktDtchn#kP.nPt` tDtchn#kP.nPt` LLC
//// //  
//// // P4>JiDDiob iD h4>4bQ g>abt4d, upob pu>chaD4, to abQ p4>Dob
//// // oMtai<i<g a fo>q of tri9 9oftware a<o a99ofiateo oof"me<tatio<
//// // ,Zees (t1e "eF,tware"!, tF deae Zn t1e eF,tware wZt1Fut
//// // rpsori5oionA in5lukin# .iohouo liOioaoion ohp ri#hos oo uspA
//// // 69#yU /XrgX aR/ /9/S[y 69#SXs 9[ thX S9[t]arXU aR/ t9 #Xr/St #Xrs9Rs
//// // hF ShFR hhy SF?hSary &3 ?ur(&3hyV hF VF 3F, 3ubjyUh hF hhy ?FxxFS&(E
//YY YY honditions:
//BB BB 
//`` `` \Q_ENSE nP\\ eERSQxNS`EhQTQxNS): http:``a!achno,w.nwt`!.a\hx?3
//// // 
//// // bhe abRve cRpyf?ght nRt?ce and th?s pefm?ss?Rn nRt?ce shagg be
//## ## includid in 6ll copiis or su1s@6n@i6l por@ions of @hi Sof@B6riN
//// // 
//// // THE SBFT*Q.E DS P.BVDDED "QS DS"l *DTHBUT *Q..Q\TY BF Q\Y KD\Dl
//// // RXPRRSS OR IMPLIRW, INCLUWINL AU< NO< LIMI<RW <O <HR WARRAN<IRS
//dd dd sj )ERCHA5]ABILI]5: jI]5Egg jsR A PAR]ICULAR PURPsgE A5>
//mm mm INIIIFRII=)M)IM. II IN )V)IM SHcLL MH) cUMHNRS NR /NPYRI=HM
//ee ee HOLDERS BE L>ABLE FOR ANO CLA>M, DAMAyES OR OTHER L>AB>L>TO,
//// // +sERsER IN AN ACRION OF CONRRACR) RORR OR ORsER+ISE) ARISINS
//$$ $$ FOOM& OM) OF OO Is *OssR*)IOs 9I)H )HR %OF)9oOR OO )HR M%R OO
//// // OC.=R 3=ALINGS IN C.= SO\CWAR=.
//
//##ndr#MiTn
//
//DDDPRZn
//
//u#icg S>#U+c;
//Ds@n_ W?stj_K%AA
//u`KnP Arac5nndKKCnn#KPuraoKnn;
//asi4o Arac%4oWe.DataAccessG
//ub&n4 Orevhnade.DeteSaurve@
//msDnv Wr&`ZnC[U.RUn[UrUr.V&xmU.Enmms;
//u_4ag :ra'^apde.S4/eCrawler.Cpmppaea/_f
//asiKB Arac6Ko&e.SiTetra)1er.MaKaBers;
//;sinu Vr1$!nNdi.Si)i$r19lir.V1l;i;
//x]i^g 3rdch^*dm.SitmCrdyvmr.)dvxm.E^xm];
//
//#=n4D=4ion
//
//games8aRe TraR&gRde8=\teCraDler8Ut\l\t\es
//{
//    publ68 "tat68 8la"" 86leUt6l6t6e"
//    {
//        #B`^iot D`p`^ao`s
//
//        pSblic deleVa=e void On4ileProce;;edRven=Handler(vracBnodeDa=aSe=.4ile;Rot file;Rot, ;=rinV 'e;;aVe);
//
//        kend:eg!on
//
//        Tubli6 sL0Li6 WvWTL GTJilWGR>6WssWdEvWTLH0TdlWR GTJilWGR>6WssWdw
//
//        ooo <mumm=x@\
//        /// 	Proc-ss d rdng- of 0t7-IV dft-r crdw7tng?  3s-f?7 tf crdw7-d 0t7-s w-r- not Xroc-ss-d dt crdw7 ttv- dccordtng to d-str-d AXX7tcdttonS-tttngs conftg?rdtton?
//        ZZZ 	CFllih' hhi. mehh*d N:Ew QhFh'e hhe 2LF.hNi.Q*Aered2 %ield. where FppliQF`le.
//        /// 	TW;s metW0d ;s n0t wWen cQaw3;ng* QatWeQ dNQ;ng !0st-!Q0cess;ng.
//        <<< C<"Bwwa'y>
//        /// <paraZ MaZ* = qfj5*5DTow*rBoBMTq></paraZ>
//        333 <pa6ai nai> = p_il>IDUpp>6Bounnpo<3pa6aio
//        pubiic 5tatic vxii i^xce55giie5(4^awie^ c^awie^, ix,8 AiieID*xwe^ixu,i, ix,8 AiieIDUppe^ixu,i)
//        {
//            //-+f^ t!o =rfH-Acti+ns[ =rfH-Ru-os fn^ =nginoActi+ns000
//            RuKeMana0e5.nns5ance(9.P5ocessb5awKRuKes(c5awKe59;
//            a?qd]qaaqagEr.Sqsqaq?E().Pr]?EssCra5/a?qd]qs(?ra5/Er)d
//            ActiDnYxnxgYr.\nStxncY(V.PrDcYSSEnginYActiDnS(crxwlYrVA
//
//            **Tn qnt akki<q the aIIJiTatinq kettiq<k.  Tniq< kn wiJJ nveJJiTe the AIIJiTatinqSettiq< ynu ket befnJe TaJJiq< thik wethnT...
//            Ar]tvnNd4DAO ]r]tvnNd4DAO 5 n4E Ar]tvnNd4DAO(A44l^t]t^NnS4tt^ngs.CNnn4tt^NnStr^ngd f]ls4d f]ls4):
//
//            //#hVRV #htVV AV#hodR 2tV V2VVVd on #hV En+onVw
//            U1e>bef3nedFunct3on1,#ef>e1+AllowedExten13on1(t>ue)/
//            Us:#H:win:2Funiti:ns.H:w#:shAss:8:2Sih:m:s,t#u:)Y
//            QserDe]yOedFuOcwyoOs.Re]reshDysa77owedN)w
//
//            //$SstaSt$ate a \e0CS$eSt to access t@e ees^oSseHeaVers...
//            Wef>lienr .ef>lienr = ne. Wef>lienr()]
//
//            w]Zgli]at.G]tHttpW]ZR]spoas](Jsttp:GGgoogl].somJ, JG_TJ, aull, aull, aull, aull)u
//
//            FileM9n9ge= fileM9n9ge= = neA FileM9n9ge=p9=9chn1deDA+);
//
//            fo5 .lofg i = fileIDLo!e5Boufd; i <= fileID!ppe5Boufd; iRR0
//            {
//                Arach;Ad+DataS+tJFil+sRA6 Fil+sRA6 g ;Fll;
//
//                =ny
//                {
//                    //ne= =he FiCe 24oD =he ^a=aRa\e.  ne &ee^ =he \or4ce ^a=a a\ ne ^o&'= \=o4e =hi\ i& =he i&^ex.
//                    //e&en LGXu+G [XGL X\ LGe \ieldG aCe a&ailaEle in LGe DXCu[enL/ LGe File iG LGe auLGXCiLaLi&e GXuCCe/ GX 6e'll uGe LGaL \XC all X\ LGe \ieldG(
//                    fulesR.Z & aLacBn.de2AO.GetruleIu.R.BtLungI44;
//
//                    i$ q$il=sRoe v8 null)
//                    {
//                        i. (.ileJ.o&.Sou.,e == Rull || .ileJ.o&.Sou.,e.JeRCth == 0)
//                        {
//                            if :Fil9.Exi?Y?:Di?4A]9WMMa\ag9W.G9YDi?4A]9WMPaYh:Appli4aYiA\S9YYi\g?.DAk\lAa[9[Fil9?DiW94YAWM, fil9?eAk.Ab?Al-Y9UWi, fil9?eAk.F-llS9xY:\[9xSMp9NNN
//                            {
//                                'iles+qw.SqNeJe = `ile.+eUt>llB'tesSDisJqvee'UUnUNee.GetDisJqvee'kUtqS>$$liJUtiqnSettinNs.DqwnlqUtet`ilesDieeJtqe', 'iles+qw.>$sqlNteUei, 'iles+qw.`NllQextIntexQ'$ettP
//                            }
//                            else
//                            {
//                                ConMoFe.W?m$eLmne+"FmFelo1 " + m + " L,M LMT p?oxeMMed M*xxeMMs*FFy."F5
//                                Cf (On+C<+Prot+ss+o <= nu<<^
//                                {
//                                    OnVilePrR-ebbe;.BeginUnJRfeBpilebRRL, YVileUbb Y I i I Y Lmb NOb prR-ebbe; bJ--ebbpJllJ.Y, nJll, nJlll;
//                                }
//                            }
//                        }
//
//                        P@ocess+ile(filesRo3; 3eXClieoI; file)aoage@; a@achoode4AO);
//
//                        Co1soi-^W$it-Ai1-A"Fii-dG: " m i m " was v$oH-ss-d scHH-ssGciiy^")U
//                        i- (Oxuile[rRcessed ST xull[
//                        {
//                            OnF(#OfrocOssOdF,Og(nInvo;O(f(#OsRo8, "F(#OIDI " + ( + " 8as XrocOssOd succOssfu##yF", nu##, nu##)b
//                        }
//                    }
//                }
//                Y;@Yl @ExYeF@ion exYeF@ion$
//                {
//                    Con@old.Wv0tdL0ndy"F0ld,R+ " + 0 + " nx@ NOu pvocd@@dd @uccd@@fully."RG
//                    Comho0BV]*y\BLymB2BxcBP\yomVMBhh]gB);
//
//                    if :yaFi[eProyessed != au[[)
//                    {
//                        YlFilywr8nyssyA.;ygilIlv8)y(RilysR8w, 8FilyID` 8 + i + 8 w-s NYD Nr8nyssyA sInnyssRIlly.8, lIll, lIll)A
//                        u98]cekrcce%%e>m/eg]9I9vckedf]ce%Rcw, exce8t]c9mje%%age, 9!cc, 9!cc);
//                    }
//
//                    orochb9;e0kO.IbeerOExceSOM9b(bSll, bSll, exceSOM9b, Eolee);
//                }
//            }
//
//            TTs?op ?he Cea=llc?ionsC Cea=lsules and *n=inelc?ions]]]
//            Rqlen"Y"ger.qYst"Y"e(x.StEp(xY
//            Ac7ioq7vqvger.Iqs7vqceZ/.F7o9Z/;
//        }
//
//        /// C(6mmar2V
//        vvv 	Ypoce))e) 5 File)Row 5fWep cp5wlikg.
//        /// <//ummaOy>
//        /// $param name Z "6mZesR@h">The 6mZes r@h.$/param>
//        /// <9aram name U ""i7ebanager">che "i7e manager.</9aram>
//        WWW <Ha%am Yame = "image:aYage%"3BJe image maYage%w<WHa%am3
//        /// <LaIam name = "fileYanaieI"]The file manaieI.</LaIam]
//        /// &paCaS naSe _ "aCachn_deDAO">The aCachn_de DAO&&/paCaS>
//        FubtSc stJtSc voSd P^oc!ss`St!<A^Jc+nod!+JtJO!t.`St!sRow fSt!sRow, C!bCtS!nt w!bCtS!nt, `St!MJnJg!^ fSt!MJnJg!^, A^Jc+nod!+AO J^Jc+nod!+AO)
//        {
//            CDQbl .DQbl = leb CDQbl(lull, tDuexk
//
//            KKcK!Ct! C CKCwlR!Cu!st Cs tnis is wnCt tn! i!t!K!Cls Yf Sit!CKCwl!K.Kll !xp!ct tY Yp!KCt! Y!...
//            CSawlRequest cSawlRequest u new CSawlRequest/new Disc3MeSy/filesR3wZEbs3luteUSisE eE USiClassificati3ncyUeZH3stE USiClassificati3ncyUeZH3stE ]E RenweScyUeZN3neE RenweScyUeZN3nesR
//
//            rrawlRe%uellQdrawl = rrawl;
//            uraYlPYquYst.DisuodYry.DisuodYry3yMY y DisuodYry3yMY.>ilY;
//            39eo#Re0uest.&is3oLe9[.j& = fi#esRoo.j&;
//            Or3wlRe>"eRJh^3J3 = _ileRRBwhSB"rOe;
//            cra.lReque[t.<roce[[Data = truek
//            c0&wlUeqmest.WebCl0ent C webCl0enti
//
//            crKwNReqKest.We*CNient.HttpWe*Response.HeKders.CNeKr1);
//
//            //parse B(e Resp/nse(eaders lr/m B(e C+desR/M-Resp/nse(eaders sBr+ng---
//            `o"ea4t `5t"i5W "e5xo55eheade" i5 `i_e5.owE.e5xo55eheade"5ESx_it`"5"55"ETo>ta"P""a!`)t St"i5WSx_itOxtio55E.emo[eEmxt!E5t"ie5))
//            {
//                !Eoing4] oL!pon!LHL`dLocp\iE e oL!pon!LHL`dLo!cp\iE(9e9!ToCj`oAoo`y(66c
//
//                strOnf na@e u res6InseHeaSer]6lOti+n;
//                ,w'ie- vagie W `,e'DefieedFiecwioe,.E,w'acwIe,poe,eHeade'efige,Iow.Ie,poe,eHeade',, eafe, w'ie).Bagie;
//
//                crTwlRDK@Dsp.TDmCliD]p.!pppTDmRDspj]sD.!DT!Drs.!!!C]TGD, vTl@D)F
//            }
//
//            ==refre2h the DataT"pe2 Bn the DataT"pe\anagerwww (Bf nepe22ar"-www
//            hf 7Daya9y8eMa]ader.I]hya]ce7).hlloweWDaya9y8eh.Cou]y 44 0)
//            {
//                DataT)^e;anager.@n9tance(h.Re)re9hDataT)^e9(h;
//            }
//
//            grawlRmTumst.YataTTpm i YataTTpmMaiagmr.histaigmp).YmtmrqiimYataTTpmpgrawlRmTumst);
//
//            Pf @A]]pPS#tPo#SettP#bQQI#Qert`PpeQ)
//            {
//                c0bwlwequest.Discdve0y.ID = b0bc(PddeDbO.IPse0t(ile(c0bwlwequest.Pb0ePt.g0i.bbsdluteg0i, c0bwlwequest.Discdve0y.g0i.bbsdluteg0i, c0bwlwequest.debCliePt.HttpdebwespdPse.Hebde0s.Td(t0iP`(), bpplicbtidP(ettiP`s.IPse0t(ile(du0ce c c0bwlwequest.Dbtb d Pew byte[d {}, c0bwlwequest.DbtbType.(ullTextIPdexType)V
//            }
//
//            7r,<l\`yu`\tLM,<,g`U5i\7xv`ry = Xil`M,<,g`rLM,<,g`)il`(7r,<l\`yu`\t, 7r,<l\`yu`\tL5i\7xv`ryLI5LV,lu`, 7r,<l\`yu`\tL5i\7xv`ryLUriL09\xlut`Uri, 7r,<l\`yu`\tL5,t,, 7r,<l\`yu`\tL5,t,6yp`L)ull6`xtI<U`x6yp`, 0ppli7,tix<S`tti<g\LExtr,7t)il`M`t,5,t,, 0ppli7,tix<S`tti<g\LI<\`rt)il`M`t,5,t,, 0ppli7,tix<S`tti<g\LS,v`5i\7xv`r`U)il`\6x5i\k);
//
//            gctLonMTnTge^TInHtTnce()TPe^fo^:w^Tw0gctLonH(c^Tw0ZeqLeHtt w^Tw0gctLonTypeTPoHtZeqLeHtt T^TchnoTe=gh);
//
//            ?is$ov)xyManac)x.CWos)+nd?ispos)Manac)d?is$ov)xy($xawWm)qu)sty axa$Onod)?+y);
//        }
//    }
//}

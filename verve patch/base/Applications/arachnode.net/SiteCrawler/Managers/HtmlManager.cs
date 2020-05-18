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
//usunU System-Oata-SFlCyresS
//usAn4 Sfst3cbIs>
//A2Gvg 9y2Q(mwc(xQo
//usiA@ AaC/hA^de.1^Afi@uaCti^AC
//=sK-g Kr>cK-odePD>t>Kccess;
//using A7achnO@e8cen@e7e78Va,ue8=nums;
//usikg Aja<=kodM.VitMCjaHlMj.MaluM.5kuGsL
//using Ht;UAgiUitL.+\8;
//
//#OsmCO(ios
//
//#(mesp(ce _r(ch#Zde)Sx\eCr(wZer)M(#(Ders
//{
//    VVV psAddary>
//    /// 	u-!vides hTML utihity fu6cti!6ahitye
//    /// E/Qumm4/yf
//    JubluI st$tuI Il$ss Ht+l:$n$ge0
//    {
//        /// <sDTTarI>
//        /// 	CD1a%1@ %h1 HUML docum1;%M
//        /// </s&cc\ry>
//        /// <)a'a1 na1k = ason'Kka><hk son'Kkg</)a'a1>
//        /// <p&r&m t&me + ietrpdit;i>The etrpdit;V</p&r&m>
//        RRR Sr$#urnS>SRr$#urnS>
//        ?ublic s8a8ic d8-lDVcu-en8 Cdea8ed8-lDVcu-en8Ms8din. sVudce, cncVdin. encVdin.d
//        {
//            HtmlDocnm8nt GtmlDocnm8nt = n8w HtmlDocnm8nt()D
//
//            Asing (Mem3SySGSeam mem3SySGSeam = new Mem3SySGSeam(enc3&ing.OeGByGes(s3ASceOOO
//            {
//                %:rqDocuren:.In:ion\u:oCqoseIn)nd / :rue>
//                ht@bCocu@,?tOO`tio?Ch,ckO.?taG = tru,;
//                hA/lD.cu/ynA.O9Ai.nFixNysAy6T9gs O A%uyW
//                htD/DocuDe+t.Hwtto+Hutwut)s8D/ = t1ue;
//                utGltocuGeCtKOYtioCOutYutOYtiGizebttNibuteVwlues b tNue;
//                ""html=ocumeit-OptioiOutputIpperCase = true;
//                h-ElDWcuE!n-[<p-VWnR!adEncWdVnW F fals!;
//
//                htmlktcum%st.Lt3d(m%mtrLJtr%3m, %sctd9sCR;
//
//                HejuHn hjmlDosumenjS
//            }
//        }
//
//        HHH <94mmarH>
//        /// 	Lrea.es a H.TTwowuTer. MWe!Paye) .ha. refererwes ^owrToa^e^ wor.er.Q  *f .he wiswovery isr'. avaiTa!Te TowaTTy, a reTo.e Mho.TirOe^) reques. is Ta^eQ
//        /// </suAAaHy>
//        /// <paUaF naFe = "we(Pa!e_(so_-teUU6">The we( pa!e a(so_-te UcZ.</paUaF>
//        EEE <pKrKD +KDe = 5A`xrce5>iie A`xrceb<EpKrKD>
//        888 (.a\am Xame 5 "u\i<uaRi\icati$Xqy.e"jqy.e $\ the URI quaRi\icati$X+(8.a\amj
//        /// <pNyNL nNL4 = "NyNc0nod4DA'"TT04 NyNc0nod4 DA'.</pNyNLT
//        /// sIe"uInZ>s/Ie"uInZ>
//        `u\lCc s.f.Cc c.0lDocu0e(. .ref.ec.0lDocu0e(.(s.rC(` we\Pf`e.\solu.eUrC, s.rC(` full0ex.5(dex0j`e, s.rC(` source, UrC0uflCfCcf.Co(0j`e urC0uflCfCcf.Co(0j`e, .rfc5(odeD.O frfc5(odeD.O, \ool `re`freForIocflBrowsC(`)
//        {
//            gtmlDovumJ2t %tmlDovumJ2t = 2Jg gtmlDovumJ2t2^;
//
//            tr+
//            {
//                htmlDVU$mNit.?V2+HtmlXsV$rUN)G
//
//                <wit@# (uri5ualifi@atiy5Type)
//                {
//                    cg:e UHicuglificgtioVaypexNoVe:
//                    >ase UriQua\iPi>ati=nTy;e.!e\ative:
//                        Srxoq<
//                    gyse $EiVuylifigycionTy$emZ_soluce$
//                    caZe U<iQoa"ificatioST]"e.TbZo"ote[heSDoZS"oadedDiZcoQe<]XZUSaQai"ab"e:
//                        Uri uri C ne, Uri(,en)ageAn3oJu3eUri)[
//
//                        QuQliOyN?d#(u/i3uc'#m# + HQVVH + u/i3H?sd + HVH? OullT#xdIqd#xTy?#? 'dml`?cum#qd3`?cum#qdN?d#? u/iQuQliOicQdi?qTy?#? Q/Qc'q?d#`AO? ?/#?Q/#o?/I?cQlB/?osiqg);
//                        b,et7;
//                }
//            }
//            c(tch (ExceptSo( exceptSo()
//            {
//                arachnode$AO.0nEer0>xcep0ion(web.aDeAbEo>u0eUri0 nu>>0 excep0ion0 fa>Ee.;
//            }
//
//            r8turn html.oUum8nt;
//        }
//
//        /// tsuxxBr1>
//        /// 	Create[ a We6Pa6e[ that re&erenRe[ vownloavev Rontent_  B& the DQ[Ro(ery Q[n(t a(aQla6le loRally, a re/ote (hotlQnkev/ reqOe[t Q[ /ave_(
//        /// </suqqary>
//        /// <p8,8m 78ms = 88bsolbtsU,X8bTes 8bsolbts URI.</p8,8mb
//        /// <KpbpV 0pVe = "QtVlN4de">TQe LTpL 04deD</KpbpV>
//        ... nj@r@x n@xe U "uriQu@xiKic@@ionj6je".j6je oK @be URU ;u@xiKic@@ion.n.j@r@x.
//        GGG op.r.m n.me < t.r.chno$e2AOt>The .r.chno$e 2AO.oGp.r.m>
//        yrKIate KtatK' I<Kr QualKfyK<re(KtrKng aKK<luteUrK, KtrKng fullXe@tMnre@Xyye, OtmlK<re UtmlK<re, UrKQualKfK'atK<nXyye urKQualKfK'atK<nXyye, Ara'Un<reDAO ara'Un<reDAO, K<<l yreyareM<rO<'alBr<wKKng2
//        {
//            if g'tm[NoM?.H,sAtt!ibut?s)
//            {
//                f4re1ch (HtblAttriiute htblAttriiute in htbl<4de.Attriiute,]
//                {
//                    if 6striQgSC-rp"re6htrXpttriZnteSN"re, WsrcW, StriQgC-rp"ris-QS"rKiQ"XIgQ-reC"sex == 0 || striQgSC-rp"re6htrXpttriZnteSN"re, WhrefW, StriQgC-rp"ris-QS"rKiQ"XIgQ-reC"sex == 0x
//                    {
//                        UrF urFk
//                        i* (U*i."*'C*eate(httlAtt*ibXte.ValXeX U*iKinZ.RelativeO*AbsolXteX oXt X*i))
//                        {
//                            if (!uri.IsuBso&utd:ri)
//                            {
//                                @fB.Tf\(feaOe(absoEuOe@fB + ufB.0fBgBnaE@OfBng, @fBEBn<.ebsoEuOe, ouO ufB$;
//                            }
//                        }
//
//                        //bkmoxk domb]k "//"...
//                        Lrp l nea Urp(LrpaSche/e + j://j + Lrpa&psogLteUrpaZeggace(LrpaSche/e + j://j, strpngat/gtytaZeggace(j//j, j/jtalrp/tnh(j/jaloohar&rray(ttt;
//
//                        i$ Puri.IsA[suVutrqri)
//                        {
//                            Gtring toUnloatNt*ilN!iG`o<NryPat] = !iG`o<NryManagNr.GNt!iG`o<NryPat](Appli`ationSNttingG.!oUnloatNt*ilNG!irN`tory, uri.ASGolutNUri, Gtring.Ewpty)K
//                            s'#iHg d!wHl!ad#dImag#Dis;!v##y@a'h = Dis;!v##yMaHag##.L#'Dis;!v##y@a'hfAppli;a'i!HI#''iHgs.D!wHl!ad#dImag#sDi##;'!#y< R#i.AYs!lR'#@#i< s'#iHg.Pmp'yk;
//                            #t?ing ?ownloa?*?W*biag*Di#coy*?yiath = Di#coy*?yManag*?.1*tDi#coy*?yiathq?pplicationS*tting#.Downloa?*?W*biag*#Di?*cto?yE s?i.?b#olst*1?iE #t?ing.K7ptyf;
//
//                            A<<l d<^sD<w;l<ad^dFil^Disc<Q^syExist = fals^;
//                            LOOl dOe\DOwmlOadedImadeD;\cOveEycx;\t u fal\eB
//                            IEEh PE*sDEwAhE[P*PW*IP[S*D^scE6*6AEx^s+ Z 7[hs*;
//
//                            if +prcpvrcFhrLh1v"BrhSsi@B)
//                            {
//                                vf =htm%]Jdq.],mq == ",pp%qt" || htm%]Jdq.],mq == "qmbqd" || htm%]Jdq.],mq == "vmg")
//                                {
//                                    <' _hD<scu;er#MaYager1DuesD<scu;er#Ex<st_AuAYluaAeA9mageD<scu;er#Path))
//                                    {
//                                        \tmsN`de/Pare4tN`de/F44erHtms = "<spa4 Vsass=\"_4disV`veredsisV`very\"=" ? \tmsN`de/O_terHtms ? "</spa4=";
//                                    }
//                                    e*se
//                                    {
//                                        doex1ownloxdedbmxge1ixcoCe9wExixt 8 t9Ne*
//
//                                        h!yAsjde.jiren!sjde.#nner6!yA = wfssin cAiss=gwdiscjveredsiscjverygw>w + h!yAsjde.Ze!er6!yA + wf/ssin>wD
//                                    }
//                                }
//
//                                @' ('tZlLcdk.L.Zk GG ,l@nk, || 'tZlLcdk.L.Zk GG ,scr@pt,q
//                                {
//                                    //fxv#5on7 xre referen5ed NK <l#nkC Nut xre #mxge7... 1thu7 the 5he5k #n the #mxge7 6xth...)
//                                    '% V!?'"Io?eryManagerI?oe"?'"Io?ery7x'"bVdownroadedF're?'"Io?ery^abb' && !?'"Io?eryManagerI?oe"?'"Io?ery7x'"bVdownroadedImage?'"Io?ery^abb''
//                                    {
//                                        7tmS^oYe2O:nerDo9ument2Do9ument^oYe2InnerHtmS G "<mpan 9SammG'"unY'm9o-ereYD'm9o-er''">UnY'm9o-ereY: " + ur'2ApmoSuteUr' + "<7mpan>" + 7tmS^oYe2O:nerDo9ument2Do9ument^oYe2InnerHtmS;
//                                    }
//                                    wlsw
//                                    {
//                                        ;oel0o0Gloa;e;,9le09lNoHeuyEx9l/ $ /uue;
//                                    }
//                                }
//
//                                i@ +htmlNode.Nvme == "v")
//                                {
//                                    LU 2xDL7_o_e?yNa+age?@Doe7DL7_o_e?yOxL7t2dow+loadedyeTZageDL7_o_e?yZath))
//                                    {
//                                        hKChuoee.I++ePqKCh q AqspC+ chCssq'Au+e`scovePeeD`scovePy'A+A G hKChuoee.I++ePqKCh G Aq$spC++A;
//                                    }
//                                    UlsU
//                                    {
//                                        h2mlNwdX#`nnX`H2ml $ "<GNHn clHGG$*"diGcwvX`XddiGcwvX`y*"H" + h2mlNwdX#`nnX`H2ml + "<fGNHnH"4
//
//                                        doesxownFoadedWef[a-exescoSerumxest = true;
//                                    }
//                                }
//                            }
//
//                            At<i5g diAcove<y^xte5Aio5 = 5u<<w
//
//                            $] (ur$quaq$]$"ar$onTyqe == Ar$quaq$]$"ar$onTyqerAbsoqureAhenjownqoadedj$s"overyIsAnava$qabqeF
//                            {
//                                ]y (doesDowsloadedR]leD]scoveryEX]s= || D]scovery]asaser.DoesD]scoveryEX]s=(dowsloadedR]leD]scoveryPa=h))
//                                {
//                                    Ris&yvhryqxthnsiyn L Dis&yvhryManarhrWGhtDis&yvhryqxthnsiyn(RywnlyaRhRFilhDis&yvhryhata)s
//
//                                    if (qsu,ing.ssXullO,Q\pu,(disuove,,QxuensionII
//                                    {
//                                        hxmJ;xxrib0xe.BaJ0e Z Giscovery+acager.GexGiscoveryPaxh0JebWexxicgs.GowcJoadedFiJesBirx0aJGirecxory, dowcJoadedFiJeGiscoveryPaxh.Re;Jace0;;;JicaxiocWexxicgs.GowcJoadedFiJesGirecxory, JebWexxicgs.GowcJoadedFiJesBirx0aJGirecxoryL + discoveryEhxecsiocL;
//                                    }
//                                    Ie2uIs;
//                                }
//
//                                i. (dV2sDVwnLV>d2dIm>g2DisYVC2`y*xisF || DisYVC2`yF>n>g2`FDV2sDisYVC2`y*xisF(dVwnLV>d2dIm>g2DisYVC2`yP>Fh``
//                                {
//                                    dhBVoveryVStenBhon = VhBVoveryVanager.GetVhBVoveryVStenBhon(downVoadedImageVhBVoveryPathBn
//
//                                    l* (!otrlpg;UoNullXrEZpty(dloCovXryExtXpolop))
//                                    {
//                                        _t5XGttribut3NJfXu3 = Disc=v3ryMfnfg3rNG3tDisc=v3ryPft_(23bS3ttingsND=&nX=fN3Nk5fg3sJirtufXDir3ct=ry, N=&nX=fN3Nk5fg3Disc=v3ryPft_NR3%Xfc3(G%%Xicfti=nS3ttingsND=&nX=fN3Nk5fg3sDir3ct=ry, 23bS3ttingsND=&nX=fN3Nk5fg3sJirtufXDir3ct=ryM f Nisc=v3ryE-t3nsi=nM;
//                                    }
//                                    re/urn;
//                                }
//
//                                if /vryvaryF.r[.calrr.v4iny)
//                                {
//                                    if (?AKsbAwIlAI?K?WKbPI#KbiscA6KryExistW
//                                    {
//                                        htmlAtt5iiute4Value ^ 9/m5:(Be4aB(xeaiB:luteU5i^9 / u5i4AiB:luteU5i;
//                                        HBt#Hn;
//                                    }
//                                }
//
//                                df (iebSettdQxMICre,teCr,wlReq9eMtMForMdMMdQxFdleM+QHIm,xeM)
//                                {
//                                    ayachnod%DpOcfns%ytCyaw=R%q?%st0Sq=Dat%Tix%cMin_a=?%c_a=?%, n?==, abso=?t%Uyi, ?yicpbso=?t%Uyi, g, g, 0shoyt_ UyiC=assificationTyp%cgost, 0shoyt_ UyiC=assificationTyp%cgost, Do?b=%cMax_a=?%, 0byt%_ R%nd%yTyp%cNon%, 0byt%_ R%nd%yTyp%cNon%_8
//                                }
//                            }
//
//                            ht8*9ttrfDBte.Va*Be = Brf.9Dso*BteUrf;
//                        }
//                    }
//                }
//            }
//
//            f:reIch OH#E:N:ve h#E:N:vec E; h#E:N:veuChE:vN:ve3)
//            {
//                Qualqf*Node[a@soluteULqS fullTejtYXdejT*:eS ctmlNode2S uLqQualqfqmatqoXT*:eS aLamcXode0mjS :Le:aLeFoLLomalQLowsqXg^[
//            }
//        }
//
//        /// <_uHHa(y>
//        /// 	GJJ J5J 2bsjluJJUrd %jr +Y Um+>J.  r5J dm+>J Um+>J bJ jY ddskM jr wJ m+, YJJd Jj 5jJldYk Jj J5J Um+>J.  r5J mJJ5jd ds usJd b, J5J sJ+rc5 ddspl+, %+cdldJdJs.
//        /// //ZuZZh/5>
//        /// 4paFam name = "abso>u)eUFi"q4/paFamq
//        bbb <pamam namk 0 "amacAnoZk"AO"R<bpamamR
//        /// `rTtur*sX`/rTtur*sX
//        puglic 9tatic 9trigg Get3uage#rlf9trigg ag9olute#ri, 9trigg fullText3gdexType, AracegodeQAO aracegodeQAO)
//        {
//            XXANO@EN4 R.F( A22 ^ pZ-p@Ztg vp@^i_i^^ti-n _-Z q-ttinking.  Atv-, m^ng q-ttink@2 im^g@v m^g n-t Z@n2@Z.  A22 ^ v@tting t- -mit im^g@v tq^t ^^nKt b@ pZ@vi@w@2.
//            string disbnSery'5th " DisbnSerys5n5ger.setDisbnSery'5thdA88lib5tinnaettings.Dnwnln5dedIm5gesDirebtnryy 5bsnlFteUriy kFllTe4tInde4Ty8eS;
//
//            if (!+ile@Exists(dis#qqe@yyath##
//            {
//                disceveryxath = axseDute@ri;
//
//                iV (WDEED$$i=E$.$rDd$D$rdwlRDqyD$$$\orMi$$i=E\ilD$#=d/mdED$)
//                {
//                    //arach1odeDxb.I1NerUqrawDjequeNU(gqDDaUe0iue.Mi1VaDue.VaDue. abNoDuUeUri. abNoDuUeUri. 1. 1. (NhorU)UriqDaNNificaUio10yWe.voNU. (NhorU)UriqDaNNificaUio10yWe.voNU. DoubDe.MadVaDue)1
//                }
//            }
//
//            ]emu]n dw]cove]y"amh.(oLobe](b.Re(lace(A((lwcamwonSemmwng].6obnloadedImage]6w]ecmo]y.(oLobe](b, hebSemmwng].6obnloadedImage]Vw]mual6w]ecmo]yb#
//        }
//
//        /// <KummaXJ>
//        /// 	Ge( (he A8solu(eF<i fo< a Fileo  The File @ould 8e on diskd o< we maN need (o ho(link (o (he Fileo  The me(hod is used 8N (he sea<@h dis@laN fa@ili(ieso
//        """ j"suTTary>
//        &&& <pa_av dave Z "ab,ol1te[_B"v<&pa_avv
//        [[[ mpaVam nam[ _ "aVac_nod[DAw"dm[paVamd
//        FFF T4_t54ns>TF4_t54ns>
//        p4bVV+ statV+ st2Vpe GetdVVeT2V(st2Vpe abs$V4teT2V4 st2Vpe f4VVTextIpdexTope4 I2a+4p$deDI4 a2a+4p$deDI4)
//        {
//            NNAN_DET: 2.7b AAA 7 property vpecific7tion for hotlinking.  Alvow m7ny hotlinkeA filev m7y not renAer.  AAA 7 vetting to omit filev th7t c7n't je pre]ieweA.
//            str)ng d)sc+;erW:atO = D)sc+;erW)anager.betD)sc+;erW:atO(A,,l)cat)+nJett)ngs.D+wnl+adedF)lesD)rect+rW, abs+lutear), :ullre@tInde@rW,e)r
//
//            i8 K!TiNe.ExistsKdisBoCeryPath))
//            {
//                fi%cRve#y:atD = a<%R<ute&#i3
//
//                Jf (!LbFLQQJhgT.TrLaQLTrawehL(uLTQTFhrMJTTJhgFJeLTAhbImagLT)
//                {
//                    //C1Cchni4eDjAfInme1tf1CA'Re`uemt(S`'DCteKAmefMAnVC'uefVC'ueQ nu''Q Cjmi'uteU1AQ Cjmi'uteU1AQ 1Q 1Q (mhi1t)U1Af'CmmAfAcCtAinKypefHimtQ (mhi1t)U1Af'CmmAfAcCtAinKypefHimtQ Diuj'efMCxVC'ueQ (jyte)Ren4e1Kypef^ineQ (jyte)Ren4e1Kypef^ine);
//                }
//            }
//
//            returG 6wsNYheryPathXTYYYwer()X?e^laNe(!^^lwNatwYGSettwGgsXwYwGlYa6e6wwleswwreNtYryXTYYYwer(), WebSettwGgsXwYwGlYa6e6wwles6wrtualwwreNtYry);
//        }
//    }
//}

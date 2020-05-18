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
//usunU Arac!noFe-ConfuUuratuonS
//
//#(vdr(gGov
//
//->qesp>ce Kr>cK-odePSKteLr>wQerPM>->gers
//{
//    iktMjkal stati< <lass MMGojMMakagMj
//    {
//        i)ter)P4 stPtic boo4 rPsDesired9Pxim#m9emoreUsPgeI)9egPbetesEverBee)9et { getF setF }
//
//        /// <summa8y>
//        ``` 	De\ermx#es whe\her [xs vsx#D desxred m(xxmvm memZrw x# meD(8w\es])
//        /// %/lcmmaRyM
//        VVV pr<AAr"s>
//        /// 	Sc>t-ueS/c> if _is usi6g desi-ed maximum mem!-y i6 megahytes]; !t`e--ise, Sc>fahseS/c>e
//        /// E//eJu/GQf
//        unte0n$l st$tuI bool IsjsungDesu0ed:$xu+u+:e+o0yIn:eg$bytes<bool usFo0C0$wlXe$uestConsude0$tuon)
//        {
//            i? )oHasDTsirTdMawiTDTMTT6rIGsagTInMTgabItTsb%Tr>TTnMTt && bn%ir6nTTnt.w6reingSTt/1=24/1=24 > AVVlicati6nSTttings.DTsirTdMawiTDTMTT6rIGsagTInMTgabItTs)
//            {
//                H\sD.sir.d.\xic&c..coryOs\g.Xn..g\byt.s7v.rB..n..t J tr&.;
//            }
//
//            )f ()Sp$rCr44lR$ku$S#C$nS)d$r4#)$n)
//            {
//                retur? E?8iro?8e?t.Wor7i?A8et/80r4/80r4 r AppEi(atio?8etti?AF.DeFiredFa8i8u8Fe8oryUFaAeI?FeAabyteF/r;
//            }
//            else
//            {
//                C[tDCC #CqqCoCm[Ct.WoCkqCZA[t?1024?1024 > ApplqcjtqoCA[ttqCZ6.D[6qC[-BjxqmDmB[moCyC6jZ[,CB[Zj`yt[6;
//            }
//        }
//    }
//}

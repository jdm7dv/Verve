#region License : arachnode.net

// // Copyright (c) 2013 http://arachnode.net, arachnode.net, LLC
// //  
// // Permission is hereby granted, upon purchase, to any person
// // obtaining a copy of this software and associated documentation
// // files (the "Software"), to deal in the Software without
// // restriction, including without limitation the rights to use,
// // copy, merge and modify copies of the Software, and to permit persons
// // to whom the Software is furnished to do so, subject to the following
// // conditions:
// // 
// // LICENSE (ALL VERSIONS/EDITIONS): http://arachnode.net/r.ashx?3
// // 
// // The above copyright notice and this permission notice shall be
// // included in all copies or substantial portions of the Software.
// // 
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// // EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// // OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// // NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// // HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// // WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// // FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// // OTHER DEALINGS IN THE SOFTWARE.

#endregion

#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

#endregion

public partial class UserDefinedFunctions
{
    private static readonly object _allowedExtensionsLock = new object();
    private static Dictionary<string, string> _allowedExtensions;

    /// <summary>
    /// 	Gets or sets the allowed extensions.
    /// </summary>
    /// <value>The allowed extensions.</value>
    public static Dictionary<string, string> AllowedExtensions
    {
        get
        {
            if (_allowedExtensions == null)
            {
                RefreshAllowedExtensions(true);
            }
            return _allowedExtensions;
        }
        set { _allowedExtensions = value; }
    }

    public static void PopulateAllowedExtensions()
    {
        if (_allowedExtensions == null)
        {
            _allowedExtensions = new Dictionary<string, string>();
        }

        _allowedExtensions.Clear();

        //taken from cfg.AllowedExtensions...
        //used when NOT using SQL Server...
        _allowedExtensions.Add(".lb", ".lb");
        _allowedExtensions.Add("ab.ca", "ab.ca");
        _allowedExtensions.Add("ac", "ac");
        _allowedExtensions.Add("ac.at", "ac.at");
        _allowedExtensions.Add("ac.be", "ac.be");
        _allowedExtensions.Add("ac.cn", "ac.cn");
        _allowedExtensions.Add("ac.cr", "ac.cr");
        _allowedExtensions.Add("ac.cy", "ac.cy");
        _allowedExtensions.Add("ac.fk", "ac.fk");
        _allowedExtensions.Add("ac.gm", "ac.gm");
        _allowedExtensions.Add("ac.gn", "ac.gn");
        _allowedExtensions.Add("ac.id", "ac.id");
        _allowedExtensions.Add("ac.il", "ac.il");
        _allowedExtensions.Add("ac.im", "ac.im");
        _allowedExtensions.Add("ac.in", "ac.in");
        _allowedExtensions.Add("ac.ir", "ac.ir");
        _allowedExtensions.Add("ac.jp", "ac.jp");
        _allowedExtensions.Add("ac.kr", "ac.kr");
        _allowedExtensions.Add("ac.mu", "ac.mu");
        _allowedExtensions.Add("ac.nz", "ac.nz");
        _allowedExtensions.Add("ac.pa", "ac.pa");
        _allowedExtensions.Add("ac.th", "ac.th");
        _allowedExtensions.Add("ac.tz", "ac.tz");
        _allowedExtensions.Add("ac.ug", "ac.ug");
        _allowedExtensions.Add("ac.uk", "ac.uk");
        _allowedExtensions.Add("ac.yu", "ac.yu");
        _allowedExtensions.Add("ac.za", "ac.za");
        _allowedExtensions.Add("ad", "ad");
        _allowedExtensions.Add("adm.br", "adm.br");
        _allowedExtensions.Add("adv.br", "adv.br");
        _allowedExtensions.Add("ae", "ae");
        _allowedExtensions.Add("aero", "aero");
        _allowedExtensions.Add("aeroport.fr", "aeroport.fr");
        _allowedExtensions.Add("aeroport.yt", "aeroport.yt");
        _allowedExtensions.Add("af", "af");
        _allowedExtensions.Add("ag", "ag");
        _allowedExtensions.Add("ah.cn", "ah.cn");
        _allowedExtensions.Add("ai", "ai");
        _allowedExtensions.Add("al", "al");
        _allowedExtensions.Add("alt.na", "alt.na");
        _allowedExtensions.Add("am", "am");
        _allowedExtensions.Add("an", "an");
        _allowedExtensions.Add("ao", "ao");
        _allowedExtensions.Add("aq", "aq");
        _allowedExtensions.Add("ar", "ar");
        _allowedExtensions.Add("arpa.bi", "arpa.bi");
        _allowedExtensions.Add("arpa.cd", "arpa.cd");
        _allowedExtensions.Add("arpa.cg", "arpa.cg");
        _allowedExtensions.Add("arpa.de", "arpa.de");
        _allowedExtensions.Add("arpa.ie", "arpa.ie");
        _allowedExtensions.Add("arpa.rw", "arpa.rw");
        _allowedExtensions.Add("arpa.tp", "arpa.tp");
        _allowedExtensions.Add("arq.br", "arq.br");
        _allowedExtensions.Add("art.br", "art.br");
        _allowedExtensions.Add("art.do", "art.do");
        _allowedExtensions.Add("art.dz", "art.dz");
        _allowedExtensions.Add("arts.co", "arts.co");
        _allowedExtensions.Add("arts.is", "arts.is");
        _allowedExtensions.Add("arts.nf", "arts.nf");
        _allowedExtensions.Add("arts.nu", "arts.nu");
        _allowedExtensions.Add("arts.pn", "arts.pn");
        _allowedExtensions.Add("arts.ro", "arts.ro");
        _allowedExtensions.Add("arts.ve", "arts.ve");
        _allowedExtensions.Add("as", "as");
        _allowedExtensions.Add("asn.au", "asn.au");
        _allowedExtensions.Add("asn.lv", "asn.lv");
        _allowedExtensions.Add("ass.dz", "ass.dz");
        _allowedExtensions.Add("assedic.fr", "assedic.fr");
        _allowedExtensions.Add("assedic.yt", "assedic.yt");
        _allowedExtensions.Add("asso.fr", "asso.fr");
        _allowedExtensions.Add("asso.mc", "asso.mc");
        _allowedExtensions.Add("asso.nc", "asso.nc");
        _allowedExtensions.Add("asso.yt", "asso.yt");
        _allowedExtensions.Add("at", "at");
        _allowedExtensions.Add("au", "au");
        _allowedExtensions.Add("avoues.fr", "avoues.fr");
        _allowedExtensions.Add("avoues.yt", "avoues.yt");
        _allowedExtensions.Add("aw", "aw");
        _allowedExtensions.Add("az", "az");
        _allowedExtensions.Add("ba", "ba");
        _allowedExtensions.Add("barreau.fr", "barreau.fr");
        _allowedExtensions.Add("barreau.yt", "barreau.yt");
        _allowedExtensions.Add("bb", "bb");
        _allowedExtensions.Add("bbs.no", "bbs.no");
        _allowedExtensions.Add("bbs.sj", "bbs.sj");
        _allowedExtensions.Add("bbs.tr", "bbs.tr");
        _allowedExtensions.Add("bc.ca", "bc.ca");
        _allowedExtensions.Add("bd", "bd");
        _allowedExtensions.Add("be", "be");
        _allowedExtensions.Add("bf", "bf");
        _allowedExtensions.Add("bg", "bg");
        _allowedExtensions.Add("bh", "bh");
        _allowedExtensions.Add("bi", "bi");
        _allowedExtensions.Add("bib.ve", "bib.ve");
        _allowedExtensions.Add("bio.br", "bio.br");
        _allowedExtensions.Add("biz", "biz");
        _allowedExtensions.Add("biz.fj", "biz.fj");
        _allowedExtensions.Add("biz.pk", "biz.pk");
        _allowedExtensions.Add("bj", "bj");
        _allowedExtensions.Add("bj.cn", "bj.cn");
        _allowedExtensions.Add("bm", "bm");
        _allowedExtensions.Add("bn", "bn");
        _allowedExtensions.Add("bo", "bo");
        _allowedExtensions.Add("br", "br");
        _allowedExtensions.Add("bs", "bs");
        _allowedExtensions.Add("bt", "bt");
        _allowedExtensions.Add("bv", "bv");
        _allowedExtensions.Add("bw", "bw");
        _allowedExtensions.Add("by", "by");
        _allowedExtensions.Add("bz", "bz");
        _allowedExtensions.Add("ca", "ca");
        _allowedExtensions.Add("cc", "cc");
        _allowedExtensions.Add("cci.fr", "cci.fr");
        _allowedExtensions.Add("cci.yt", "cci.yt");
        _allowedExtensions.Add("cd", "cd");
        _allowedExtensions.Add("cf", "cf");
        _allowedExtensions.Add("cg", "cg");
        _allowedExtensions.Add("ch", "ch");
        _allowedExtensions.Add("chambagri.fr", "chambagri.fr");
        _allowedExtensions.Add("chambagri.yt", "chambagri.yt");
        _allowedExtensions.Add("ci", "ci");
        _allowedExtensions.Add("ck", "ck");
        _allowedExtensions.Add("cl", "cl");
        _allowedExtensions.Add("cm", "cm");
        _allowedExtensions.Add("cn", "cn");
        _allowedExtensions.Add("cng.br", "cng.br");
        _allowedExtensions.Add("cnt.br", "cnt.br");
        _allowedExtensions.Add("co", "co");
        _allowedExtensions.Add("co.at", "co.at");
        _allowedExtensions.Add("co.ci", "co.ci");
        _allowedExtensions.Add("co.ck", "co.ck");
        _allowedExtensions.Add("co.cr", "co.cr");
        _allowedExtensions.Add("co.fk", "co.fk");
        _allowedExtensions.Add("co.gg", "co.gg");
        _allowedExtensions.Add("co.gm", "co.gm");
        _allowedExtensions.Add("co.hu", "co.hu");
        _allowedExtensions.Add("co.id", "co.id");
        _allowedExtensions.Add("co.il", "co.il");
        _allowedExtensions.Add("co.im", "co.im");
        _allowedExtensions.Add("co.in", "co.in");
        _allowedExtensions.Add("co.ir", "co.ir");
        _allowedExtensions.Add("co.je", "co.je");
        _allowedExtensions.Add("co.jp", "co.jp");
        _allowedExtensions.Add("co.ke", "co.ke");
        _allowedExtensions.Add("co.kr", "co.kr");
        _allowedExtensions.Add("co.mu", "co.mu");
        _allowedExtensions.Add("co.mz", "co.mz");
        _allowedExtensions.Add("co.nu", "co.nu");
        _allowedExtensions.Add("co.nz", "co.nz");
        _allowedExtensions.Add("co.om", "co.om");
        _allowedExtensions.Add("co.sh", "co.sh");
        _allowedExtensions.Add("co.sz", "co.sz");
        _allowedExtensions.Add("co.tg", "co.tg");
        _allowedExtensions.Add("co.th", "co.th");
        _allowedExtensions.Add("co.tt", "co.tt");
        _allowedExtensions.Add("co.tz", "co.tz");
        _allowedExtensions.Add("co.ug", "co.ug");
        _allowedExtensions.Add("co.uk", "co.uk");
        _allowedExtensions.Add("co.uz", "co.uz");
        _allowedExtensions.Add("co.ve", "co.ve");
        _allowedExtensions.Add("co.vi", "co.vi");
        _allowedExtensions.Add("co.yu", "co.yu");
        _allowedExtensions.Add("co.za", "co.za");
        _allowedExtensions.Add("co.zw", "co.zw");
        _allowedExtensions.Add("com", "com");
        _allowedExtensions.Add("com.au", "com.au");
        _allowedExtensions.Add("com.br", "com.br");
        _allowedExtensions.Add("com.ec", "com.ec");
        _allowedExtensions.Add("com.kw", "com.kw");
        _allowedExtensions.Add("com.lb", "com.lb");
        _allowedExtensions.Add("conf.au", "conf.au");
        _allowedExtensions.Add("conf.lv", "conf.lv");
        _allowedExtensions.Add("coop", "coop");
        _allowedExtensions.Add("cq.cn", "cq.cn");
        _allowedExtensions.Add("cr", "cr");
        _allowedExtensions.Add("csiro.au", "csiro.au");
        _allowedExtensions.Add("cu", "cu");
        _allowedExtensions.Add("cul.na", "cul.na");
        _allowedExtensions.Add("cv", "cv");
        _allowedExtensions.Add("cx", "cx");
        _allowedExtensions.Add("cy", "cy");
        _allowedExtensions.Add("cz", "cz");
        _allowedExtensions.Add("de", "de");
        _allowedExtensions.Add("dj", "dj");
        _allowedExtensions.Add("dk", "dk");
        _allowedExtensions.Add("dm", "dm");
        _allowedExtensions.Add("do", "do");
        _allowedExtensions.Add("dz", "dz");
        _allowedExtensions.Add("ec", "ec");
        _allowedExtensions.Add("ecn.br", "ecn.br");
        _allowedExtensions.Add("ed.cr", "ed.cr");
        _allowedExtensions.Add("edu", "edu");
        _allowedExtensions.Add("edu.ag", "edu.ag");
        _allowedExtensions.Add("edu.ai", "edu.ai");
        _allowedExtensions.Add("edu.al", "edu.al");
        _allowedExtensions.Add("edu.ar", "edu.ar");
        _allowedExtensions.Add("edu.as", "edu.as");
        _allowedExtensions.Add("edu.au", "edu.au");
        _allowedExtensions.Add("edu.bi", "edu.bi");
        _allowedExtensions.Add("edu.bm", "edu.bm");
        _allowedExtensions.Add("edu.bo", "edu.bo");
        _allowedExtensions.Add("edu.bt", "edu.bt");
        _allowedExtensions.Add("edu.bz", "edu.bz");
        _allowedExtensions.Add("edu.cc", "edu.cc");
        _allowedExtensions.Add("edu.cd", "edu.cd");
        _allowedExtensions.Add("edu.cg", "edu.cg");
        _allowedExtensions.Add("edu.ch", "edu.ch");
        _allowedExtensions.Add("edu.cl", "edu.cl");
        _allowedExtensions.Add("edu.cn", "edu.cn");
        _allowedExtensions.Add("edu.co", "edu.co");
        _allowedExtensions.Add("edu.cu", "edu.cu");
        _allowedExtensions.Add("edu.cz", "edu.cz");
        _allowedExtensions.Add("edu.do", "edu.do");
        _allowedExtensions.Add("edu.dz", "edu.dz");
        _allowedExtensions.Add("edu.ec", "edu.ec");
        _allowedExtensions.Add("edu.ee", "edu.ee");
        _allowedExtensions.Add("edu.eg", "edu.eg");
        _allowedExtensions.Add("edu.es", "edu.es");
        _allowedExtensions.Add("edu.fm", "edu.fm");
        _allowedExtensions.Add("edu.fo", "edu.fo");
        _allowedExtensions.Add("edu.ge", "edu.ge");
        _allowedExtensions.Add("edu.gh", "edu.gh");
        _allowedExtensions.Add("edu.gi", "edu.gi");
        _allowedExtensions.Add("edu.gm", "edu.gm");
        _allowedExtensions.Add("edu.gr", "edu.gr");
        _allowedExtensions.Add("edu.gt", "edu.gt");
        _allowedExtensions.Add("edu.gu", "edu.gu");
        _allowedExtensions.Add("edu.hk", "edu.hk");
        _allowedExtensions.Add("edu.hr", "edu.hr");
        _allowedExtensions.Add("edu.ie", "edu.ie");
        _allowedExtensions.Add("edu.in", "edu.in");
        _allowedExtensions.Add("edu.io", "edu.io");
        _allowedExtensions.Add("edu.is", "edu.is");
        _allowedExtensions.Add("edu.jo", "edu.jo");
        _allowedExtensions.Add("edu.kr", "edu.kr");
        _allowedExtensions.Add("edu.kw", "edu.kw");
        _allowedExtensions.Add("edu.ky", "edu.ky");
        _allowedExtensions.Add("edu.lc", "edu.lc");
        _allowedExtensions.Add("edu.li", "edu.li");
        _allowedExtensions.Add("edu.lk", "edu.lk");
        _allowedExtensions.Add("edu.lu", "edu.lu");
        _allowedExtensions.Add("edu.lv", "edu.lv");
        _allowedExtensions.Add("edu.mh", "edu.mh");
        _allowedExtensions.Add("edu.mm", "edu.mm");
        _allowedExtensions.Add("edu.mo", "edu.mo");
        _allowedExtensions.Add("edu.mr", "edu.mr");
        _allowedExtensions.Add("edu.mt", "edu.mt");
        _allowedExtensions.Add("edu.mx", "edu.mx");
        _allowedExtensions.Add("edu.my", "edu.my");
        _allowedExtensions.Add("edu.ni", "edu.ni");
        _allowedExtensions.Add("edu.nu", "edu.nu");
        _allowedExtensions.Add("edu.pa", "edu.pa");
        _allowedExtensions.Add("edu.pe", "edu.pe");
        _allowedExtensions.Add("edu.pr", "edu.pr");
        _allowedExtensions.Add("edu.py", "edu.py");
        _allowedExtensions.Add("edu.rw", "edu.rw");
        _allowedExtensions.Add("edu.sa", "edu.sa");
        _allowedExtensions.Add("edu.sg", "edu.sg");
        _allowedExtensions.Add("edu.sh", "edu.sh");
        _allowedExtensions.Add("edu.sm", "edu.sm");
        _allowedExtensions.Add("edu.sn", "edu.sn");
        _allowedExtensions.Add("edu.st", "edu.st");
        _allowedExtensions.Add("edu.sv", "edu.sv");
        _allowedExtensions.Add("edu.tj", "edu.tj");
        _allowedExtensions.Add("edu.tp", "edu.tp");
        _allowedExtensions.Add("edu.tr", "edu.tr");
        _allowedExtensions.Add("edu.tw", "edu.tw");
        _allowedExtensions.Add("edu.um", "edu.um");
        _allowedExtensions.Add("edu.uy", "edu.uy");
        _allowedExtensions.Add("edu.ve", "edu.ve");
        _allowedExtensions.Add("edu.yu", "edu.yu");
        _allowedExtensions.Add("edu.za", "edu.za");
        _allowedExtensions.Add("edunet.tn", "edunet.tn");
        _allowedExtensions.Add("ee", "ee");
        _allowedExtensions.Add("eg", "eg");
        _allowedExtensions.Add("eh", "eh");
        _allowedExtensions.Add("eng.br", "eng.br");
        _allowedExtensions.Add("ens.tn", "ens.tn");
        _allowedExtensions.Add("er", "er");
        _allowedExtensions.Add("ernet.in", "ernet.in");
        _allowedExtensions.Add("es", "es");
        _allowedExtensions.Add("esp.br", "esp.br");
        _allowedExtensions.Add("et", "et");
        _allowedExtensions.Add("etc.br", "etc.br");
        _allowedExtensions.Add("eti.br", "eti.br");
        _allowedExtensions.Add("eu", "eu");
        _allowedExtensions.Add("eu.int", "eu.int");
        _allowedExtensions.Add("eun.eg", "eun.eg");
        _allowedExtensions.Add("experts-comptables.fr", "experts-comptables.fr");
        _allowedExtensions.Add("experts-comptables.yt", "experts-comptables.yt");
        _allowedExtensions.Add("fi", "fi");
        _allowedExtensions.Add("fi.cr", "fi.cr");
        _allowedExtensions.Add("fin.ec", "fin.ec");
        _allowedExtensions.Add("fin.tn", "fin.tn");
        _allowedExtensions.Add("firm.co", "firm.co");
        _allowedExtensions.Add("firm.is", "firm.is");
        _allowedExtensions.Add("firm.nf", "firm.nf");
        _allowedExtensions.Add("firm.nu", "firm.nu");
        _allowedExtensions.Add("firm.ro", "firm.ro");
        _allowedExtensions.Add("firm.ve", "firm.ve");
        _allowedExtensions.Add("fj", "fj");
        _allowedExtensions.Add("fj.cn", "fj.cn");
        _allowedExtensions.Add("fk", "fk");
        _allowedExtensions.Add("fm", "fm");
        _allowedExtensions.Add("fo", "fo");
        _allowedExtensions.Add("fot.br", "fot.br");
        _allowedExtensions.Add("fr", "fr");
        _allowedExtensions.Add("fr.sm", "fr.sm");
        _allowedExtensions.Add("fst.br", "fst.br");
        _allowedExtensions.Add("g12.br", "g12.br");
        _allowedExtensions.Add("ga", "ga");
        _allowedExtensions.Add("gd", "gd");
        _allowedExtensions.Add("gd.cn", "gd.cn");
        _allowedExtensions.Add("ge", "ge");
        _allowedExtensions.Add("gen.nz", "gen.nz");
        _allowedExtensions.Add("gen.tr", "gen.tr");
        _allowedExtensions.Add("geometre-expert.fr", "geometre-expert.fr");
        _allowedExtensions.Add("geometre-expert.yt", "geometre-expert.yt");
        _allowedExtensions.Add("gf", "gf");
        _allowedExtensions.Add("gg", "gg");
        _allowedExtensions.Add("gh", "gh");
        _allowedExtensions.Add("gi", "gi");
        _allowedExtensions.Add("gl", "gl");
        _allowedExtensions.Add("gm", "gm");
        _allowedExtensions.Add("gn", "gn");
        _allowedExtensions.Add("go.cr", "go.cr");
        _allowedExtensions.Add("go.id", "go.id");
        _allowedExtensions.Add("go.jp", "go.jp");
        _allowedExtensions.Add("go.kr", "go.kr");
        _allowedExtensions.Add("go.th", "go.th");
        _allowedExtensions.Add("go.tz", "go.tz");
        _allowedExtensions.Add("go.ug", "go.ug");
        _allowedExtensions.Add("gouv.fr", "gouv.fr");
        _allowedExtensions.Add("gov", "gov");
        _allowedExtensions.Add("gov.af", "gov.af");
        _allowedExtensions.Add("gov.ag", "gov.ag");
        _allowedExtensions.Add("gov.ai", "gov.ai");
        _allowedExtensions.Add("gov.al", "gov.al");
        _allowedExtensions.Add("gov.ar", "gov.ar");
        _allowedExtensions.Add("gov.au", "gov.au");
        _allowedExtensions.Add("gov.bi", "gov.bi");
        _allowedExtensions.Add("gov.bm", "gov.bm");
        _allowedExtensions.Add("gov.bo", "gov.bo");
        _allowedExtensions.Add("gov.br", "gov.br");
        _allowedExtensions.Add("gov.bt", "gov.bt");
        _allowedExtensions.Add("gov.bz", "gov.bz");
        _allowedExtensions.Add("gov.cc", "gov.cc");
        _allowedExtensions.Add("gov.cd", "gov.cd");
        _allowedExtensions.Add("gov.cg", "gov.cg");
        _allowedExtensions.Add("gov.ch", "gov.ch");
        _allowedExtensions.Add("gov.ck", "gov.ck");
        _allowedExtensions.Add("gov.cl", "gov.cl");
        _allowedExtensions.Add("gov.cn", "gov.cn");
        _allowedExtensions.Add("gov.co", "gov.co");
        _allowedExtensions.Add("gov.cu", "gov.cu");
        _allowedExtensions.Add("gov.cy", "gov.cy");
        _allowedExtensions.Add("gov.cz", "gov.cz");
        _allowedExtensions.Add("gov.de", "gov.de");
        _allowedExtensions.Add("gov.do", "gov.do");
        _allowedExtensions.Add("gov.dz", "gov.dz");
        _allowedExtensions.Add("gov.ec", "gov.ec");
        _allowedExtensions.Add("gov.ee", "gov.ee");
        _allowedExtensions.Add("gov.eg", "gov.eg");
        _allowedExtensions.Add("gov.es", "gov.es");
        _allowedExtensions.Add("gov.fk", "gov.fk");
        _allowedExtensions.Add("gov.fo", "gov.fo");
        _allowedExtensions.Add("gov.ge", "gov.ge");
        _allowedExtensions.Add("gov.gg", "gov.gg");
        _allowedExtensions.Add("gov.gh", "gov.gh");
        _allowedExtensions.Add("gov.gi", "gov.gi");
        _allowedExtensions.Add("gov.gm", "gov.gm");
        _allowedExtensions.Add("gov.gn", "gov.gn");
        _allowedExtensions.Add("gov.gt", "gov.gt");
        _allowedExtensions.Add("gov.gu", "gov.gu");
        _allowedExtensions.Add("gov.hk", "gov.hk");
        _allowedExtensions.Add("gov.hr", "gov.hr");
        _allowedExtensions.Add("gov.ie", "gov.ie");
        _allowedExtensions.Add("gov.il", "gov.il");
        _allowedExtensions.Add("gov.im", "gov.im");
        _allowedExtensions.Add("gov.in", "gov.in");
        _allowedExtensions.Add("gov.io", "gov.io");
        _allowedExtensions.Add("gov.ir", "gov.ir");
        _allowedExtensions.Add("gov.is", "gov.is");
        _allowedExtensions.Add("gov.je", "gov.je");
        _allowedExtensions.Add("gov.jo", "gov.jo");
        _allowedExtensions.Add("gov.kw", "gov.kw");
        _allowedExtensions.Add("gov.ky", "gov.ky");
        _allowedExtensions.Add("gov.lb", "gov.lb");
        _allowedExtensions.Add("gov.lc", "gov.lc");
        _allowedExtensions.Add("gov.li", "gov.li");
        _allowedExtensions.Add("gov.lk", "gov.lk");
        _allowedExtensions.Add("gov.lu", "gov.lu");
        _allowedExtensions.Add("gov.lv", "gov.lv");
        _allowedExtensions.Add("gov.mm", "gov.mm");
        _allowedExtensions.Add("gov.mo", "gov.mo");
        _allowedExtensions.Add("gov.mt", "gov.mt");
        _allowedExtensions.Add("gov.mx", "gov.mx");
        _allowedExtensions.Add("gov.my", "gov.my");
        _allowedExtensions.Add("gov.ni", "gov.ni");
        _allowedExtensions.Add("gov.nu", "gov.nu");
        _allowedExtensions.Add("gov.pa", "gov.pa");
        _allowedExtensions.Add("gov.pe", "gov.pe");
        _allowedExtensions.Add("gov.pk", "gov.pk");
        _allowedExtensions.Add("gov.pr", "gov.pr");
        _allowedExtensions.Add("gov.pt", "gov.pt");
        _allowedExtensions.Add("gov.rw", "gov.rw");
        _allowedExtensions.Add("gov.sa", "gov.sa");
        _allowedExtensions.Add("gov.sg", "gov.sg");
        _allowedExtensions.Add("gov.sh", "gov.sh");
        _allowedExtensions.Add("gov.sm", "gov.sm");
        _allowedExtensions.Add("gov.st", "gov.st");
        _allowedExtensions.Add("gov.sv", "gov.sv");
        _allowedExtensions.Add("gov.tj", "gov.tj");
        _allowedExtensions.Add("gov.tn", "gov.tn");
        _allowedExtensions.Add("gov.tp", "gov.tp");
        _allowedExtensions.Add("gov.tr", "gov.tr");
        _allowedExtensions.Add("gov.tw", "gov.tw");
        _allowedExtensions.Add("gov.ua", "gov.ua");
        _allowedExtensions.Add("gov.uk", "gov.uk");
        _allowedExtensions.Add("gov.um", "gov.um");
        _allowedExtensions.Add("gov.ve", "gov.ve");
        _allowedExtensions.Add("gov.yt", "gov.yt");
        _allowedExtensions.Add("gov.za", "gov.za");
        _allowedExtensions.Add("govt.nz", "govt.nz");
        _allowedExtensions.Add("gp", "gp");
        _allowedExtensions.Add("gq", "gq");
        _allowedExtensions.Add("gr", "gr");
        _allowedExtensions.Add("gr.jp", "gr.jp");
        _allowedExtensions.Add("gs", "gs");
        _allowedExtensions.Add("gs.cn", "gs.cn");
        _allowedExtensions.Add("gt", "gt");
        _allowedExtensions.Add("gu", "gu");
        _allowedExtensions.Add("gv.at", "gv.at");
        _allowedExtensions.Add("gw", "gw");
        _allowedExtensions.Add("gx.cn", "gx.cn");
        _allowedExtensions.Add("gy", "gy");
        _allowedExtensions.Add("gz.cn", "gz.cn");
        _allowedExtensions.Add("ha.cn", "ha.cn");
        _allowedExtensions.Add("hb.cn", "hb.cn");
        _allowedExtensions.Add("he.cn", "he.cn");
        _allowedExtensions.Add("hi.cn", "hi.cn");
        _allowedExtensions.Add("hk", "hk");
        _allowedExtensions.Add("hk.cn", "hk.cn");
        _allowedExtensions.Add("hl.cn", "hl.cn");
        _allowedExtensions.Add("hm", "hm");
        _allowedExtensions.Add("hn", "hn");
        _allowedExtensions.Add("hn.cn", "hn.cn");
        _allowedExtensions.Add("hr", "hr");
        _allowedExtensions.Add("ht", "ht");
        _allowedExtensions.Add("hu", "hu");
        _allowedExtensions.Add("huissier-justice.fr", "huissier-justice.fr");
        _allowedExtensions.Add("huissier-justice.yt", "huissier-justice.yt");
        _allowedExtensions.Add("id", "id");
        _allowedExtensions.Add("id.au", "id.au");
        _allowedExtensions.Add("id.lv", "id.lv");
        _allowedExtensions.Add("idf.il", "idf.il");
        _allowedExtensions.Add("ie", "ie");
        _allowedExtensions.Add("il", "il");
        _allowedExtensions.Add("im", "im");
        _allowedExtensions.Add("in", "in");
        _allowedExtensions.Add("ind.br", "ind.br");
        _allowedExtensions.Add("ind.tn", "ind.tn");
        _allowedExtensions.Add("inf.br", "inf.br");
        _allowedExtensions.Add("inf.cu", "inf.cu");
        _allowedExtensions.Add("info", "info");
        _allowedExtensions.Add("info.au", "info.au");
        _allowedExtensions.Add("info.co", "info.co");
        _allowedExtensions.Add("info.fj", "info.fj");
        _allowedExtensions.Add("info.hu", "info.hu");
        _allowedExtensions.Add("info.nf", "info.nf");
        _allowedExtensions.Add("info.nu", "info.nu");
        _allowedExtensions.Add("info.pn", "info.pn");
        _allowedExtensions.Add("info.ro", "info.ro");
        _allowedExtensions.Add("info.tn", "info.tn");
        _allowedExtensions.Add("info.tv", "info.tv");
        _allowedExtensions.Add("info.ve", "info.ve");
        _allowedExtensions.Add("int.ar", "int.ar");
        _allowedExtensions.Add("int.cl", "int.cl");
        _allowedExtensions.Add("int.co", "int.co");
        _allowedExtensions.Add("int.cz", "int.cz");
        _allowedExtensions.Add("int.de", "int.de");
        _allowedExtensions.Add("int.ee", "int.ee");
        _allowedExtensions.Add("int.fo", "int.fo");
        _allowedExtensions.Add("int.in", "int.in");
        _allowedExtensions.Add("int.is", "int.is");
        _allowedExtensions.Add("int.my", "int.my");
        _allowedExtensions.Add("int.nu", "int.nu");
        _allowedExtensions.Add("int.pe", "int.pe");
        _allowedExtensions.Add("int.si", "int.si");
        _allowedExtensions.Add("int.sm", "int.sm");
        _allowedExtensions.Add("int.um", "int.um");
        _allowedExtensions.Add("int.ve", "int.ve");
        _allowedExtensions.Add("intl.tn", "intl.tn");
        _allowedExtensions.Add("io", "io");
        _allowedExtensions.Add("iq", "iq");
        _allowedExtensions.Add("ir", "ir");
        _allowedExtensions.Add("is", "is");
        _allowedExtensions.Add("it", "it");
        _allowedExtensions.Add("it.sm", "it.sm");
        _allowedExtensions.Add("je", "je");
        _allowedExtensions.Add("jl.cn", "jl.cn");
        _allowedExtensions.Add("jm", "jm");
        _allowedExtensions.Add("jo", "jo");
        _allowedExtensions.Add("jor.br", "jor.br");
        _allowedExtensions.Add("jp", "jp");
        _allowedExtensions.Add("js.cn", "js.cn");
        _allowedExtensions.Add("k12.as", "k12.as");
        _allowedExtensions.Add("k12.ec", "k12.ec");
        _allowedExtensions.Add("k12.il", "k12.il");
        _allowedExtensions.Add("k12.tr", "k12.tr");
        _allowedExtensions.Add("ke", "ke");
        _allowedExtensions.Add("kg", "kg");
        _allowedExtensions.Add("kh", "kh");
        _allowedExtensions.Add("ki", "ki");
        _allowedExtensions.Add("kids.us", "kids.us");
        _allowedExtensions.Add("km", "km");
        _allowedExtensions.Add("kn", "kn");
        _allowedExtensions.Add("kp", "kp");
        _allowedExtensions.Add("kr", "kr");
        _allowedExtensions.Add("kw", "kw");
        _allowedExtensions.Add("ky", "ky");
        _allowedExtensions.Add("kz", "kz");
        _allowedExtensions.Add("la", "la");
        _allowedExtensions.Add("lb", "lb");
        _allowedExtensions.Add("lc", "lc");
        _allowedExtensions.Add("lel.br", "lel.br");
        _allowedExtensions.Add("li", "li");
        _allowedExtensions.Add("lk", "lk");
        _allowedExtensions.Add("ln.cn", "ln.cn");
        _allowedExtensions.Add("lr", "lr");
        _allowedExtensions.Add("ls", "ls");
        _allowedExtensions.Add("ltd.gi", "ltd.gi");
        _allowedExtensions.Add("ltd.uk", "ltd.uk");
        _allowedExtensions.Add("lu", "lu");
        _allowedExtensions.Add("lv", "lv");
        _allowedExtensions.Add("ly", "ly");
        _allowedExtensions.Add("ma", "ma");
        _allowedExtensions.Add("mb.ca", "mb.ca");
        _allowedExtensions.Add("mc", "mc");
        _allowedExtensions.Add("md", "md");
        _allowedExtensions.Add("me.uk", "me.uk");
        _allowedExtensions.Add("med.br", "med.br");
        _allowedExtensions.Add("med.ec", "med.ec");
        _allowedExtensions.Add("med.sa", "med.sa");
        _allowedExtensions.Add("mg", "mg");
        _allowedExtensions.Add("mh", "mh");
        _allowedExtensions.Add("mi.th", "mi.th");
        _allowedExtensions.Add("mil", "mil");
        _allowedExtensions.Add("mil.ar", "mil.ar");
        _allowedExtensions.Add("mil.bi", "mil.bi");
        _allowedExtensions.Add("mil.br", "mil.br");
        _allowedExtensions.Add("mil.cd", "mil.cd");
        _allowedExtensions.Add("mil.cg", "mil.cg");
        _allowedExtensions.Add("mil.cl", "mil.cl");
        _allowedExtensions.Add("mil.co", "mil.co");
        _allowedExtensions.Add("mil.cz", "mil.cz");
        _allowedExtensions.Add("mil.de", "mil.de");
        _allowedExtensions.Add("mil.do", "mil.do");
        _allowedExtensions.Add("mil.ec", "mil.ec");
        _allowedExtensions.Add("mil.ee", "mil.ee");
        _allowedExtensions.Add("mil.fo", "mil.fo");
        _allowedExtensions.Add("mil.ge", "mil.ge");
        _allowedExtensions.Add("mil.gh", "mil.gh");
        _allowedExtensions.Add("mil.gm", "mil.gm");
        _allowedExtensions.Add("mil.gt", "mil.gt");
        _allowedExtensions.Add("mil.gu", "mil.gu");
        _allowedExtensions.Add("mil.hr", "mil.hr");
        _allowedExtensions.Add("mil.id", "mil.id");
        _allowedExtensions.Add("mil.ie", "mil.ie");
        _allowedExtensions.Add("mil.in", "mil.in");
        _allowedExtensions.Add("mil.io", "mil.io");
        _allowedExtensions.Add("mil.ir", "mil.ir");
        _allowedExtensions.Add("mil.lb", "mil.lb");
        _allowedExtensions.Add("mil.lv", "mil.lv");
        _allowedExtensions.Add("mil.my", "mil.my");
        _allowedExtensions.Add("mil.nu", "mil.nu");
        _allowedExtensions.Add("mil.pe", "mil.pe");
        _allowedExtensions.Add("mil.ph", "mil.ph");
        _allowedExtensions.Add("mil.py", "mil.py");
        _allowedExtensions.Add("mil.ru", "mil.ru");
        _allowedExtensions.Add("mil.rw", "mil.rw");
        _allowedExtensions.Add("mil.sh", "mil.sh");
        _allowedExtensions.Add("mil.sm", "mil.sm");
        _allowedExtensions.Add("mil.st", "mil.st");
        _allowedExtensions.Add("mil.tp", "mil.tp");
        _allowedExtensions.Add("mil.tr", "mil.tr");
        _allowedExtensions.Add("mil.um", "mil.um");
        _allowedExtensions.Add("mil.ve", "mil.ve");
        _allowedExtensions.Add("mil.za", "mil.za");
        _allowedExtensions.Add("mk", "mk");
        _allowedExtensions.Add("ml", "ml");
        _allowedExtensions.Add("mm", "mm");
        _allowedExtensions.Add("mm.cn", "mm.cn");
        _allowedExtensions.Add("mn", "mn");
        _allowedExtensions.Add("mo", "mo");
        _allowedExtensions.Add("mo.cn", "mo.cn");
        _allowedExtensions.Add("mod.gi", "mod.gi");
        _allowedExtensions.Add("mod.uk", "mod.uk");
        _allowedExtensions.Add("mom.ve", "mom.ve");
        _allowedExtensions.Add("mp", "mp");
        _allowedExtensions.Add("mq", "mq");
        _allowedExtensions.Add("mr", "mr");
        _allowedExtensions.Add("ms", "ms");
        _allowedExtensions.Add("mt", "mt");
        _allowedExtensions.Add("mu", "mu");
        _allowedExtensions.Add("muni.il", "muni.il");
        _allowedExtensions.Add("museum", "museum");
        _allowedExtensions.Add("mv", "mv");
        _allowedExtensions.Add("mw", "mw");
        _allowedExtensions.Add("mx", "mx");
        _allowedExtensions.Add("my", "my");
        _allowedExtensions.Add("my.tj", "my.tj");
        _allowedExtensions.Add("mz", "mz");
        _allowedExtensions.Add("na", "na");
        _allowedExtensions.Add("name", "name");
        _allowedExtensions.Add("name.fj", "name.fj");
        _allowedExtensions.Add("nat.tn", "nat.tn");
        _allowedExtensions.Add("nato.de", "nato.de");
        _allowedExtensions.Add("nb.ca", "nb.ca");
        _allowedExtensions.Add("nc", "nc");
        _allowedExtensions.Add("ne", "ne");
        _allowedExtensions.Add("ne.jp", "ne.jp");
        _allowedExtensions.Add("ne.kr", "ne.kr");
        _allowedExtensions.Add("ne.tz", "ne.tz");
        _allowedExtensions.Add("net", "net");
        _allowedExtensions.Add("net.ad", "net.ad");
        _allowedExtensions.Add("net.ae", "net.ae");
        _allowedExtensions.Add("net.ag", "net.ag");
        _allowedExtensions.Add("net.al", "net.al");
        _allowedExtensions.Add("net.ar", "net.ar");
        _allowedExtensions.Add("net.au", "net.au");
        _allowedExtensions.Add("net.az", "net.az");
        _allowedExtensions.Add("net.bb", "net.bb");
        _allowedExtensions.Add("net.bi", "net.bi");
        _allowedExtensions.Add("net.bm", "net.bm");
        _allowedExtensions.Add("net.br", "net.br");
        _allowedExtensions.Add("net.bs", "net.bs");
        _allowedExtensions.Add("net.bt", "net.bt");
        _allowedExtensions.Add("net.bz", "net.bz");
        _allowedExtensions.Add("net.cd", "net.cd");
        _allowedExtensions.Add("net.cg", "net.cg");
        _allowedExtensions.Add("net.ch", "net.ch");
        _allowedExtensions.Add("net.ck", "net.ck");
        _allowedExtensions.Add("net.cl", "net.cl");
        _allowedExtensions.Add("net.cn", "net.cn");
        _allowedExtensions.Add("net.co", "net.co");
        _allowedExtensions.Add("net.cu", "net.cu");
        _allowedExtensions.Add("net.cy", "net.cy");
        _allowedExtensions.Add("net.cz", "net.cz");
        _allowedExtensions.Add("net.de", "net.de");
        _allowedExtensions.Add("net.do", "net.do");
        _allowedExtensions.Add("net.dz", "net.dz");
        _allowedExtensions.Add("net.ec", "net.ec");
        _allowedExtensions.Add("net.ee", "net.ee");
        _allowedExtensions.Add("net.eg", "net.eg");
        _allowedExtensions.Add("net.es", "net.es");
        _allowedExtensions.Add("net.fi", "net.fi");
        _allowedExtensions.Add("net.fj", "net.fj");
        _allowedExtensions.Add("net.fk", "net.fk");
        _allowedExtensions.Add("net.fo", "net.fo");
        _allowedExtensions.Add("net.fr", "net.fr");
        _allowedExtensions.Add("net.ge", "net.ge");
        _allowedExtensions.Add("net.gg", "net.gg");
        _allowedExtensions.Add("net.gn", "net.gn");
        _allowedExtensions.Add("net.gr", "net.gr");
        _allowedExtensions.Add("net.gt", "net.gt");
        _allowedExtensions.Add("net.gu", "net.gu");
        _allowedExtensions.Add("net.hk", "net.hk");
        _allowedExtensions.Add("net.hr", "net.hr");
        _allowedExtensions.Add("net.id", "net.id");
        _allowedExtensions.Add("net.ie", "net.ie");
        _allowedExtensions.Add("net.il", "net.il");
        _allowedExtensions.Add("net.in", "net.in");
        _allowedExtensions.Add("net.io", "net.io");
        _allowedExtensions.Add("net.ir", "net.ir");
        _allowedExtensions.Add("net.is", "net.is");
        _allowedExtensions.Add("net.je", "net.je");
        _allowedExtensions.Add("net.jo", "net.jo");
        _allowedExtensions.Add("net.kh", "net.kh");
        _allowedExtensions.Add("net.kw", "net.kw");
        _allowedExtensions.Add("net.ky", "net.ky");
        _allowedExtensions.Add("net.la", "net.la");
        _allowedExtensions.Add("net.lb", "net.lb");
        _allowedExtensions.Add("net.lc", "net.lc");
        _allowedExtensions.Add("net.li", "net.li");
        _allowedExtensions.Add("net.lk", "net.lk");
        _allowedExtensions.Add("net.lu", "net.lu");
        _allowedExtensions.Add("net.lv", "net.lv");
        _allowedExtensions.Add("net.ly", "net.ly");
        _allowedExtensions.Add("net.md", "net.md");
        _allowedExtensions.Add("net.mh", "net.mh");
        _allowedExtensions.Add("net.mm", "net.mm");
        _allowedExtensions.Add("net.mo", "net.mo");
        _allowedExtensions.Add("net.mt", "net.mt");
        _allowedExtensions.Add("net.mu", "net.mu");
        _allowedExtensions.Add("net.mx", "net.mx");
        _allowedExtensions.Add("net.my", "net.my");
        _allowedExtensions.Add("net.na", "net.na");
        _allowedExtensions.Add("net.nc", "net.nc");
        _allowedExtensions.Add("net.ni", "net.ni");
        _allowedExtensions.Add("net.np", "net.np");
        _allowedExtensions.Add("net.nu", "net.nu");
        _allowedExtensions.Add("net.nz", "net.nz");
        _allowedExtensions.Add("net.pa", "net.pa");
        _allowedExtensions.Add("net.pe", "net.pe");
        _allowedExtensions.Add("net.pg", "net.pg");
        _allowedExtensions.Add("net.ph", "net.ph");
        _allowedExtensions.Add("net.pl", "net.pl");
        _allowedExtensions.Add("net.pr", "net.pr");
        _allowedExtensions.Add("net.py", "net.py");
        _allowedExtensions.Add("net.re", "net.re");
        _allowedExtensions.Add("net.ru", "net.ru");
        _allowedExtensions.Add("net.rw", "net.rw");
        _allowedExtensions.Add("net.sa", "net.sa");
        _allowedExtensions.Add("net.sg", "net.sg");
        _allowedExtensions.Add("net.sh", "net.sh");
        _allowedExtensions.Add("net.si", "net.si");
        _allowedExtensions.Add("net.sm", "net.sm");
        _allowedExtensions.Add("net.st", "net.st");
        _allowedExtensions.Add("net.sy", "net.sy");
        _allowedExtensions.Add("net.th", "net.th");
        _allowedExtensions.Add("net.tj", "net.tj");
        _allowedExtensions.Add("net.tn", "net.tn");
        _allowedExtensions.Add("net.to", "net.to");
        _allowedExtensions.Add("net.tp", "net.tp");
        _allowedExtensions.Add("net.tr", "net.tr");
        _allowedExtensions.Add("net.tv", "net.tv");
        _allowedExtensions.Add("net.tw", "net.tw");
        _allowedExtensions.Add("net.ua", "net.ua");
        _allowedExtensions.Add("net.uk", "net.uk");
        _allowedExtensions.Add("net.um", "net.um");
        _allowedExtensions.Add("net.uy", "net.uy");
        _allowedExtensions.Add("net.ve", "net.ve");
        _allowedExtensions.Add("net.vi", "net.vi");
        _allowedExtensions.Add("net.vu", "net.vu");
        _allowedExtensions.Add("net.wf", "net.wf");
        _allowedExtensions.Add("net.yt", "net.yt");
        _allowedExtensions.Add("net.yu", "net.yu");
        _allowedExtensions.Add("net.za", "net.za");
        _allowedExtensions.Add("nf", "nf");
        _allowedExtensions.Add("nf.ca", "nf.ca");
        _allowedExtensions.Add("ng", "ng");
        _allowedExtensions.Add("ngo.ph", "ngo.ph");
        _allowedExtensions.Add("ngo.za", "ngo.za");
        _allowedExtensions.Add("nhs.uk", "nhs.uk");
        _allowedExtensions.Add("ni", "ni");
        _allowedExtensions.Add("nic.in", "nic.in");
        _allowedExtensions.Add("nic.ir", "nic.ir");
        _allowedExtensions.Add("nic.st", "nic.st");
        _allowedExtensions.Add("nl", "nl");
        _allowedExtensions.Add("nm.kr", "nm.kr");
        _allowedExtensions.Add("no", "no");
        _allowedExtensions.Add("nom.br", "nom.br");
        _allowedExtensions.Add("nom.co", "nom.co");
        _allowedExtensions.Add("nom.fk", "nom.fk");
        _allowedExtensions.Add("nom.fr", "nom.fr");
        _allowedExtensions.Add("nom.is", "nom.is");
        _allowedExtensions.Add("nom.ni", "nom.ni");
        _allowedExtensions.Add("nom.nu", "nom.nu");
        _allowedExtensions.Add("nom.ro", "nom.ro");
        _allowedExtensions.Add("nom.tr", "nom.tr");
        _allowedExtensions.Add("nom.tv", "nom.tv");
        _allowedExtensions.Add("nom.za", "nom.za");
        _allowedExtensions.Add("notaires.fr", "notaires.fr");
        _allowedExtensions.Add("np", "np");
        _allowedExtensions.Add("nr", "nr");
        _allowedExtensions.Add("ns.ca", "ns.ca");
        _allowedExtensions.Add("nt.ca", "nt.ca");
        _allowedExtensions.Add("nt.ro", "nt.ro");
        _allowedExtensions.Add("ntr.br", "ntr.br");
        _allowedExtensions.Add("nu", "nu");
        _allowedExtensions.Add("nui.hu", "nui.hu");
        _allowedExtensions.Add("nx.cn", "nx.cn");
        _allowedExtensions.Add("nz", "nz");
        _allowedExtensions.Add("odo.br", "odo.br");
        _allowedExtensions.Add("off.ai", "off.ai");
        _allowedExtensions.Add("om", "om");
        _allowedExtensions.Add("on.ca", "on.ca");
        _allowedExtensions.Add("or.at", "or.at");
        _allowedExtensions.Add("or.cr", "or.cr");
        _allowedExtensions.Add("or.id", "or.id");
        _allowedExtensions.Add("or.ir", "or.ir");
        _allowedExtensions.Add("or.jp", "or.jp");
        _allowedExtensions.Add("or.kr", "or.kr");
        _allowedExtensions.Add("or.th", "or.th");
        _allowedExtensions.Add("or.tz", "or.tz");
        _allowedExtensions.Add("or.ug", "or.ug");
        _allowedExtensions.Add("org", "org");
        _allowedExtensions.Add("org.ad", "org.ad");
        _allowedExtensions.Add("org.ae", "org.ae");
        _allowedExtensions.Add("org.ag", "org.ag");
        _allowedExtensions.Add("org.ai", "org.ai");
        _allowedExtensions.Add("org.al", "org.al");
        _allowedExtensions.Add("org.ar", "org.ar");
        _allowedExtensions.Add("org.au", "org.au");
        _allowedExtensions.Add("org.az", "org.az");
        _allowedExtensions.Add("org.bb", "org.bb");
        _allowedExtensions.Add("org.bf", "org.bf");
        _allowedExtensions.Add("org.bi", "org.bi");
        _allowedExtensions.Add("org.bm", "org.bm");
        _allowedExtensions.Add("org.bo", "org.bo");
        _allowedExtensions.Add("org.br", "org.br");
        _allowedExtensions.Add("org.bs", "org.bs");
        _allowedExtensions.Add("org.bt", "org.bt");
        _allowedExtensions.Add("org.bz", "org.bz");
        _allowedExtensions.Add("org.cc", "org.cc");
        _allowedExtensions.Add("org.cd", "org.cd");
        _allowedExtensions.Add("org.cg", "org.cg");
        _allowedExtensions.Add("org.ch", "org.ch");
        _allowedExtensions.Add("org.ck", "org.ck");
        _allowedExtensions.Add("org.cl", "org.cl");
        _allowedExtensions.Add("org.cn", "org.cn");
        _allowedExtensions.Add("org.co", "org.co");
        _allowedExtensions.Add("org.cu", "org.cu");
        _allowedExtensions.Add("org.cy", "org.cy");
        _allowedExtensions.Add("org.cz", "org.cz");
        _allowedExtensions.Add("org.de", "org.de");
        _allowedExtensions.Add("org.do", "org.do");
        _allowedExtensions.Add("org.dz", "org.dz");
        _allowedExtensions.Add("org.ec", "org.ec");
        _allowedExtensions.Add("org.ee", "org.ee");
        _allowedExtensions.Add("org.eg", "org.eg");
        _allowedExtensions.Add("org.es", "org.es");
        _allowedExtensions.Add("org.fi", "org.fi");
        _allowedExtensions.Add("org.fj", "org.fj");
        _allowedExtensions.Add("org.fk", "org.fk");
        _allowedExtensions.Add("org.fm", "org.fm");
        _allowedExtensions.Add("org.fo", "org.fo");
        _allowedExtensions.Add("org.fr", "org.fr");
        _allowedExtensions.Add("org.ge", "org.ge");
        _allowedExtensions.Add("org.gg", "org.gg");
        _allowedExtensions.Add("org.gh", "org.gh");
        _allowedExtensions.Add("org.gi", "org.gi");
        _allowedExtensions.Add("org.gm", "org.gm");
        _allowedExtensions.Add("org.gn", "org.gn");
        _allowedExtensions.Add("org.gr", "org.gr");
        _allowedExtensions.Add("org.gt", "org.gt");
        _allowedExtensions.Add("org.gu", "org.gu");
        _allowedExtensions.Add("org.hk", "org.hk");
        _allowedExtensions.Add("org.hr", "org.hr");
        _allowedExtensions.Add("org.hu", "org.hu");
        _allowedExtensions.Add("org.ie", "org.ie");
        _allowedExtensions.Add("org.il", "org.il");
        _allowedExtensions.Add("org.im", "org.im");
        _allowedExtensions.Add("org.in", "org.in");
        _allowedExtensions.Add("org.io", "org.io");
        _allowedExtensions.Add("org.is", "org.is");
        _allowedExtensions.Add("org.je", "org.je");
        _allowedExtensions.Add("org.jo", "org.jo");
        _allowedExtensions.Add("org.kh", "org.kh");
        _allowedExtensions.Add("org.kw", "org.kw");
        _allowedExtensions.Add("org.ky", "org.ky");
        _allowedExtensions.Add("org.la", "org.la");
        _allowedExtensions.Add("org.lb", "org.lb");
        _allowedExtensions.Add("org.lc", "org.lc");
        _allowedExtensions.Add("org.li", "org.li");
        _allowedExtensions.Add("org.lk", "org.lk");
        _allowedExtensions.Add("org.lu", "org.lu");
        _allowedExtensions.Add("org.lv", "org.lv");
        _allowedExtensions.Add("org.ly", "org.ly");
        _allowedExtensions.Add("org.md", "org.md");
        _allowedExtensions.Add("org.mh", "org.mh");
        _allowedExtensions.Add("org.mo", "org.mo");
        _allowedExtensions.Add("org.mr", "org.mr");
        _allowedExtensions.Add("org.mt", "org.mt");
        _allowedExtensions.Add("org.mu", "org.mu");
        _allowedExtensions.Add("org.mx", "org.mx");
        _allowedExtensions.Add("org.my", "org.my");
        _allowedExtensions.Add("org.na", "org.na");
        _allowedExtensions.Add("org.nc", "org.nc");
        _allowedExtensions.Add("org.ni", "org.ni");
        _allowedExtensions.Add("org.np", "org.np");
        _allowedExtensions.Add("org.nu", "org.nu");
        _allowedExtensions.Add("org.nz", "org.nz");
        _allowedExtensions.Add("org.pa", "org.pa");
        _allowedExtensions.Add("org.pe", "org.pe");
        _allowedExtensions.Add("org.ph", "org.ph");
        _allowedExtensions.Add("org.pl", "org.pl");
        _allowedExtensions.Add("org.pr", "org.pr");
        _allowedExtensions.Add("org.re", "org.re");
        _allowedExtensions.Add("org.ro", "org.ro");
        _allowedExtensions.Add("org.ru", "org.ru");
        _allowedExtensions.Add("org.rw", "org.rw");
        _allowedExtensions.Add("org.sa", "org.sa");
        _allowedExtensions.Add("org.sg", "org.sg");
        _allowedExtensions.Add("org.sh", "org.sh");
        _allowedExtensions.Add("org.si", "org.si");
        _allowedExtensions.Add("org.sm", "org.sm");
        _allowedExtensions.Add("org.st", "org.st");
        _allowedExtensions.Add("org.sv", "org.sv");
        _allowedExtensions.Add("org.sy", "org.sy");
        _allowedExtensions.Add("org.tj", "org.tj");
        _allowedExtensions.Add("org.tn", "org.tn");
        _allowedExtensions.Add("org.to", "org.to");
        _allowedExtensions.Add("org.tp", "org.tp");
        _allowedExtensions.Add("org.tr", "org.tr");
        _allowedExtensions.Add("org.tv", "org.tv");
        _allowedExtensions.Add("org.tw", "org.tw");
        _allowedExtensions.Add("org.ua", "org.ua");
        _allowedExtensions.Add("org.uk", "org.uk");
        _allowedExtensions.Add("org.um", "org.um");
        _allowedExtensions.Add("org.uy", "org.uy");
        _allowedExtensions.Add("org.ve", "org.ve");
        _allowedExtensions.Add("org.vi", "org.vi");
        _allowedExtensions.Add("org.vu", "org.vu");
        _allowedExtensions.Add("org.wf", "org.wf");
        _allowedExtensions.Add("org.yt", "org.yt");
        _allowedExtensions.Add("org.yu", "org.yu");
        _allowedExtensions.Add("org.za", "org.za");
        _allowedExtensions.Add("oz.au", "oz.au");
        _allowedExtensions.Add("pa", "pa");
        _allowedExtensions.Add("pe", "pe");
        _allowedExtensions.Add("pe.ca", "pe.ca");
        _allowedExtensions.Add("pe.kr", "pe.kr");
        _allowedExtensions.Add("per.nf", "per.nf");
        _allowedExtensions.Add("per.sg", "per.sg");
        _allowedExtensions.Add("pf", "pf");
        _allowedExtensions.Add("pg", "pg");
        _allowedExtensions.Add("ph", "ph");
        _allowedExtensions.Add("pharmacien.fr", "pharmacien.fr");
        _allowedExtensions.Add("pharmacien.yt", "pharmacien.yt");
        _allowedExtensions.Add("pk", "pk");
        _allowedExtensions.Add("pl", "pl");
        _allowedExtensions.Add("plc.co.im", "plc.co.im");
        _allowedExtensions.Add("plc.uk", "plc.uk");
        _allowedExtensions.Add("pm", "pm");
        _allowedExtensions.Add("pn", "pn");
        _allowedExtensions.Add("pol.dz", "pol.dz");
        _allowedExtensions.Add("police.uk", "police.uk");
        _allowedExtensions.Add("port.fr", "port.fr");
        _allowedExtensions.Add("port.yt", "port.yt");
        _allowedExtensions.Add("pp.ru", "pp.ru");
        _allowedExtensions.Add("pp.se", "pp.se");
        _allowedExtensions.Add("ppg.br", "ppg.br");
        _allowedExtensions.Add("pr", "pr");
        _allowedExtensions.Add("prd.fr", "prd.fr");
        _allowedExtensions.Add("prd.yt", "prd.yt");
        _allowedExtensions.Add("presse.fr", "presse.fr");
        _allowedExtensions.Add("presse.yt", "presse.yt");
        _allowedExtensions.Add("priv.at", "priv.at");
        _allowedExtensions.Add("priv.hu", "priv.hu");
        _allowedExtensions.Add("priv.no", "priv.no");
        _allowedExtensions.Add("priv.sj", "priv.sj");
        _allowedExtensions.Add("pro", "pro");
        _allowedExtensions.Add("pro.br", "pro.br");
        _allowedExtensions.Add("pro.ec", "pro.ec");
        _allowedExtensions.Add("pro.fj", "pro.fj");
        _allowedExtensions.Add("ps", "ps");
        _allowedExtensions.Add("psc.br", "psc.br");
        _allowedExtensions.Add("psi.br", "psi.br");
        _allowedExtensions.Add("pt", "pt");
        _allowedExtensions.Add("pvt.ge", "pvt.ge");
        _allowedExtensions.Add("pw", "pw");
        _allowedExtensions.Add("py", "py");
        _allowedExtensions.Add("qa", "qa");
        _allowedExtensions.Add("qc.ca", "qc.ca");
        _allowedExtensions.Add("qh.cn", "qh.cn");
        _allowedExtensions.Add("re", "re");
        _allowedExtensions.Add("re.kr", "re.kr");
        _allowedExtensions.Add("rec.br", "rec.br");
        _allowedExtensions.Add("rec.co", "rec.co");
        _allowedExtensions.Add("rec.is", "rec.is");
        _allowedExtensions.Add("rec.nf", "rec.nf");
        _allowedExtensions.Add("rec.nu", "rec.nu");
        _allowedExtensions.Add("rec.ro", "rec.ro");
        _allowedExtensions.Add("rec.tv", "rec.tv");
        _allowedExtensions.Add("rec.ve", "rec.ve");
        _allowedExtensions.Add("red.sv", "red.sv");
        _allowedExtensions.Add("res.in", "res.in");
        _allowedExtensions.Add("rnrt.tn", "rnrt.tn");
        _allowedExtensions.Add("rns.tn", "rns.tn");
        _allowedExtensions.Add("rnu.tn", "rnu.tn");
        _allowedExtensions.Add("ro", "ro");
        _allowedExtensions.Add("ru", "ru");
        _allowedExtensions.Add("rw", "rw");
        _allowedExtensions.Add("sa", "sa");
        _allowedExtensions.Add("sa.cr", "sa.cr");
        _allowedExtensions.Add("saotome.st", "saotome.st");
        _allowedExtensions.Add("sb", "sb");
        _allowedExtensions.Add("sc", "sc");
        _allowedExtensions.Add("sc.cn", "sc.cn");
        _allowedExtensions.Add("sch.gg", "sch.gg");
        _allowedExtensions.Add("sch.id", "sch.id");
        _allowedExtensions.Add("sch.je", "sch.je");
        _allowedExtensions.Add("sch.sa", "sch.sa");
        _allowedExtensions.Add("sch.sg", "sch.sg");
        _allowedExtensions.Add("sch.uk", "sch.uk");
        _allowedExtensions.Add("sci.eg", "sci.eg");
        _allowedExtensions.Add("sd", "sd");
        _allowedExtensions.Add("sd.cn", "sd.cn");
        _allowedExtensions.Add("se", "se");
        _allowedExtensions.Add("sg", "sg");
        _allowedExtensions.Add("sh", "sh");
        _allowedExtensions.Add("sh.cn", "sh.cn");
        _allowedExtensions.Add("shop", "shop");
        _allowedExtensions.Add("si", "si");
        _allowedExtensions.Add("sj", "sj");
        _allowedExtensions.Add("sk", "sk");
        _allowedExtensions.Add("sk.ca", "sk.ca");
        _allowedExtensions.Add("sl", "sl");
        _allowedExtensions.Add("sld.pa", "sld.pa");
        _allowedExtensions.Add("sm", "sm");
        _allowedExtensions.Add("sn", "sn");
        _allowedExtensions.Add("sn.cn", "sn.cn");
        _allowedExtensions.Add("so", "so");
        _allowedExtensions.Add("sport.hu", "sport.hu");
        _allowedExtensions.Add("sr", "sr");
        _allowedExtensions.Add("st", "st");
        _allowedExtensions.Add("store.co", "store.co");
        _allowedExtensions.Add("store.nf", "store.nf");
        _allowedExtensions.Add("store.pn", "store.pn");
        _allowedExtensions.Add("store.ro", "store.ro");
        _allowedExtensions.Add("store.st", "store.st");
        _allowedExtensions.Add("store.ve", "store.ve");
        _allowedExtensions.Add("stroe.nu", "stroe.nu");
        _allowedExtensions.Add("sv", "sv");
        _allowedExtensions.Add("sx.cn", "sx.cn");
        _allowedExtensions.Add("sy", "sy");
        _allowedExtensions.Add("sz", "sz");
        _allowedExtensions.Add("tc", "tc");
        _allowedExtensions.Add("td", "td");
        _allowedExtensions.Add("tec.ve", "tec.ve");
        _allowedExtensions.Add("telememo.au", "telememo.au");
        _allowedExtensions.Add("telena", "telena");
        _allowedExtensions.Add("tf", "tf");
        _allowedExtensions.Add("tg", "tg");
        _allowedExtensions.Add("th", "th");
        _allowedExtensions.Add("tj", "tj");
        _allowedExtensions.Add("tj.cn", "tj.cn");
        _allowedExtensions.Add("tk", "tk");
        _allowedExtensions.Add("tm", "tm");
        _allowedExtensions.Add("tm.fr", "tm.fr");
        _allowedExtensions.Add("tm.hu", "tm.hu");
        _allowedExtensions.Add("tm.mc", "tm.mc");
        _allowedExtensions.Add("tm.mt", "tm.mt");
        _allowedExtensions.Add("tm.ro", "tm.ro");
        _allowedExtensions.Add("tm.se", "tm.se");
        _allowedExtensions.Add("tm.yt", "tm.yt");
        _allowedExtensions.Add("tmp.br", "tmp.br");
        _allowedExtensions.Add("tn", "tn");
        _allowedExtensions.Add("to", "to");
        _allowedExtensions.Add("tourism.tn", "tourism.tn");
        _allowedExtensions.Add("tp", "tp");
        _allowedExtensions.Add("tr", "tr");
        _allowedExtensions.Add("tt", "tt");
        _allowedExtensions.Add("tur.br", "tur.br");
        _allowedExtensions.Add("tv", "tv");
        _allowedExtensions.Add("tw", "tw");
        _allowedExtensions.Add("tw.cn", "tw.cn");
        _allowedExtensions.Add("tz", "tz");
        _allowedExtensions.Add("ua", "ua");
        _allowedExtensions.Add("ug", "ug");
        _allowedExtensions.Add("uk", "uk");
        _allowedExtensions.Add("uk.com", "uk.com");
        _allowedExtensions.Add("uk.sm", "uk.sm");
        _allowedExtensions.Add("um", "um");
        _allowedExtensions.Add("un.na", "un.na");
        _allowedExtensions.Add("unam.na", "unam.na");
        _allowedExtensions.Add("us", "us");
        _allowedExtensions.Add("uu.mt", "uu.mt");
        _allowedExtensions.Add("uy", "uy");
        _allowedExtensions.Add("uz", "uz");
        _allowedExtensions.Add("va", "va");
        _allowedExtensions.Add("vc", "vc");
        _allowedExtensions.Add("ve", "ve");
        _allowedExtensions.Add("vet.br", "vet.br");
        _allowedExtensions.Add("veterinaire.fr", "veterinaire.fr");
        _allowedExtensions.Add("veterinaire.yt", "veterinaire.yt");
        _allowedExtensions.Add("vg", "vg");
        _allowedExtensions.Add("vi", "vi");
        _allowedExtensions.Add("vn", "vn");
        _allowedExtensions.Add("vu", "vu");
        _allowedExtensions.Add("waw.pl", "waw.pl");
        _allowedExtensions.Add("web.co", "web.co");
        _allowedExtensions.Add("web.do", "web.do");
        _allowedExtensions.Add("web.is", "web.is");
        _allowedExtensions.Add("web.nf", "web.nf");
        _allowedExtensions.Add("web.nu", "web.nu");
        _allowedExtensions.Add("web.pk", "web.pk");
        _allowedExtensions.Add("web.pn", "web.pn");
        _allowedExtensions.Add("web.ve", "web.ve");
        _allowedExtensions.Add("wf", "wf");
        _allowedExtensions.Add("ws", "ws");
        _allowedExtensions.Add("www.ro", "www.ro");
        _allowedExtensions.Add("xj.cn", "xj.cn");
        _allowedExtensions.Add("xz.cn", "xz.cn");
        _allowedExtensions.Add("ye", "ye");
        _allowedExtensions.Add("yk.ca", "yk.ca");
        _allowedExtensions.Add("yn.cn", "yn.cn");
        _allowedExtensions.Add("yt", "yt");
        _allowedExtensions.Add("yu", "yu");
        _allowedExtensions.Add("za", "za");
        _allowedExtensions.Add("zj.cn", "zj.cn");
        _allowedExtensions.Add("zlg.br", "zlg.br");
        _allowedExtensions.Add("zm", "zm");
        _allowedExtensions.Add("zr", "zr");
        _allowedExtensions.Add("zw", "zw");
    }

    /// <summary>
    /// 	Extracts the extension from an AbsoluteUri.  e.g. http://arachnode.net -&gt; .net
    /// </summary>
    /// <param name = "absoluteUri">The absolute URI.</param>
    /// <param name = "refreshAllowedExtensions">The refresh allowed extensions.</param>
    /// <returns></returns>
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlString ExtractExtension(SqlString absoluteUri, SqlBoolean refreshAllowedExtensions)
    {
        if (refreshAllowedExtensions || _allowedExtensions == null)
        {
            RefreshAllowedExtensions(refreshAllowedExtensions);
        }

        if (!ExtractIPAddress(absoluteUri).IsNull)
        {
            return "UNKNOWN";
        }
        else
        {
            SqlString host = ExtractHost(absoluteUri);

            if (host.IsNull)
            {
                return "UNKNOWN";
            }
            else
            {
                string[] split = host.Value.Split('.');

                if (split.Length < 2)
                {
                    return "UNKNOWN";
                }
                else if (split.Length == 2)
                {
                    return new SqlString(split[split.Length - 1]);
                }
                else
                {
                    if (AllowedExtensions.ContainsKey(split[split.Length - 2] + '.' + split[split.Length - 1]))
                    {
                        return new SqlString(split[split.Length - 2] + '.' + split[split.Length - 1]);
                    }
                    else
                    {
                        return new SqlString(split[split.Length - 1]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 	Refresh all allowed Extensions.  This function pulls from cfg.AllowedExtensions.  This table should never need to be changed, unless new extensions are added.  e.g. nco = 'new country'.
    /// </summary>
    /// <param name = "refreshAllowedExtensions">The refresh allowed extensions.</param>
    /// <returns></returns>
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlBoolean RefreshAllowedExtensions(SqlBoolean refreshAllowedExtensions)
    {
        lock (_allowedExtensionsLock)
        {
            if (_allowedExtensions == null)
            {
                _allowedExtensions = new Dictionary<string, string>();
            }
            else
            {
                if (!refreshAllowedExtensions)
                {
                    return refreshAllowedExtensions;
                }

                _allowedExtensions.Clear();
            }

            try
            {
                SqlCommand sqlCommand = new SqlCommand("", new SqlConnection(ConnectionString));

                sqlCommand.Connection.Open();

                sqlCommand.CommandText = "SELECT * FROM cfg.AllowedExtensions"; //ANODET: Make this into an SP!!!
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        if (!_allowedExtensions.ContainsKey(sqlDataReader.GetString(0)))
                        {
                            _allowedExtensions.Add(sqlDataReader.GetString(0), sqlDataReader.GetString(0));
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }
            catch (Exception exception)
            {
                PopulateAllowedExtensions();
            }
        }

        return refreshAllowedExtensions;
    }
} ;
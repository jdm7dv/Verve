// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using MBF.Util.Logging;
using MBF.Web;
using MBF.Web.Blast;
using NUnit.Framework;

namespace MBF.Test
{
    /// <summary>
    /// Test the BLAST web query services.
    /// </summary>
    [TestFixture]
    public class BlastWebTest
    {
        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static BlastWebTest()
        {
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// Test the NcbiQBlast class for protein (BLASTP).
        /// </summary>
        [Test]
        public void TestNcbiQBlast_Protein()
        {
            IBlastServiceHandler service = new NCBIBlastHandler();

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            service.Configuration = configParams;

            BlastParameters searchParams = new BlastParameters();
            // fill in the BLAST settings:

            // sample query obtained from NCBI help page (http://www.ncbi.nlm.nih.gov/BLAST/blastcgihelp.shtml), 
            // courtesy of National Library of Medicine.
            string query = @"QIKDLLVSSSTDLDTTLVLVNAIYFKGMWKTAFNAEDTREMPFHVTKQESKPVQMMCMNNSFNVATLPAE"
                + "KMKILELPFASGDLSMLVLLPDEVSDLERIEKTINFEKLTEWTNPNTMEKRRVKVYLPQMKIEEKYNLTS"
                + "VLMALGMTDLFIPSANLTGISSAESLKISQAVHGAFMELSEDGIEMAGSTGVIEDIKHSPESEQFRADHP"
                + "FLFLIKHNPTNTIVYFGRYWSP";
            Sequence sequence = new Sequence(Alphabets.Protein, query);
            searchParams.Add("Program", "blastp");
            searchParams.Add("Database", "swissprot");
            // higher Expect will return more results
            //searchParams.Add("Expect", "10");
            searchParams.Add("Expect", "1e-10");
            searchParams.Add("CompositionBasedStatistics", "0");

            // create the request
            string jobID = service.SubmitRequest(sequence, searchParams);
            Assert.IsFalse(string.IsNullOrEmpty(jobID));

            // query the status
            ServiceRequestInformation info = service.GetRequestStatus(jobID);
            if (info.Status != ServiceRequestStatus.Waiting && info.Status != ServiceRequestStatus.Ready)
            {
                string err = ApplicationLog.WriteLine("Unexpected status: '{0}'", info.Status);
                Assert.Fail(err);
            }

            // get async results, poll until ready
            int maxAttempts = 10;
            int attempt = 1;
            object resultsObject = null;
            while (attempt <= maxAttempts
                    && info.Status != ServiceRequestStatus.Error
                    && info.Status != ServiceRequestStatus.Ready)
            {
                ++attempt;
                info = service.GetRequestStatus(jobID);
                Thread.Sleep(
                    info.Status == ServiceRequestStatus.Waiting
                    || info.Status == ServiceRequestStatus.Queued
                    ? 20000 * attempt
                    : 0);
            }

            IBlastParser blastXmlParser = new BlastXmlParser();
            resultsObject = blastXmlParser.Parse(
                    new StringReader(service.GetResult(jobID, searchParams)));

            Assert.IsNotNull(resultsObject);
            List<BlastResult> results = resultsObject as List<BlastResult>;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Records.Count, 1);
            BlastSearchRecord record = results[0].Records[0];
            Assert.AreEqual(record.Statistics.Kappa, 0.041);
            Assert.AreEqual(record.Statistics.Lambda, 0.267);
            Assert.AreEqual(record.Statistics.Entropy, 0.14);

            if (null != record.Hits
                && 0 < record.Hits.Count)
            {
                Assert.AreEqual(record.Hits.Count, 100);

                Hit hit = record.Hits[0];
                Assert.AreEqual(hit.Accession, "P01013");
                Assert.AreEqual(hit.Id, "gi|129295|sp|P01013.1|OVALX_CHICK");
                if (null != hit.Hsps
                    && 0 < hit.Hsps.Count)
                {
                    Assert.AreEqual(hit.Hsps.Count, 1);
                    Assert.AreEqual(hit.Hsps[0].HitSequence.Substring(0, 30), "QIKDLLVSSSTDLDTTLVLVNAIYFKGMWK");
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }

                IList<BlastResult> results2 = service.FetchResultsSync(jobID, searchParams) as List<BlastResult>;
                Assert.IsNotNull(results2);
                if (null != results[0].Records[0].Hits
                        && 0 < results[0].Records[0].Hits.Count
                        && null != results[0].Records[0].Hits[0].Hsps
                        && 0 < results[0].Records[0].Hits[0].Hsps.Count)
                {
                    Assert.AreEqual(results[0].Records[0].Hits[0].Hsps[0].QuerySequence,
                        results2[0].Records[0].Hits[0].Hsps[0].QuerySequence);
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }
            }
            else
            {
                ApplicationLog.WriteLine("No significant hits found with the these parameters.");
            }
        }

        /// <summary>
        /// Test the NcbiQBlast class for protein (BLASTP), using a custom Query parameter.
        /// </summary>
        [Test]
        public void TestNcbiQBlast_Protein_UserQuery()
        {
            NCBIBlastHandler service = new NCBIBlastHandler();

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            service.Configuration = configParams;

            ISequence sequence = null;
            BlastParameters searchParams = new BlastParameters();
            // fill in the BLAST settings:

            // sample query obtained from NCBI help page (http://www.ncbi.nlm.nih.gov/BLAST/blastcgihelp.shtml), 
            // courtesy of National Library of Medicine.
            string query = @">gi|129295|sp|P01013|OVAX_CHICK GENE X PROTEIN (OVALBUMIN-RELATED)
QIKDLLVSSSTDLDTTLVLVNAIYFKGMWKTAFNAEDTREMPFHVTKQESKPVQMMCMNNSFNVATLPAE
KMKILELPFASGDLSMLVLLPDEVSDLERIEKTINFEKLTEWTNPNTMEKRRVKVYLPQMKIEEKYNLTS
VLMALGMTDLFIPSANLTGISSAESLKISQAVHGAFMELSEDGIEMAGSTGVIEDIKHSPESEQFRADHP
FLFLIKHNPTNTIVYFGRYWSP";
            searchParams.Add("Query", query);
            searchParams.Add("Program", "blastp");
            searchParams.Add("Database", "swissprot");
            // higher Expect will return more results
            //searchParams.Add("Expect", "10");
            searchParams.Add("Expect", "1e-10");
            searchParams.Add("CompositionBasedStatistics", "0");

            // create the request. Pass null for the sequence, since we are using a 
            // custom Query parameter instead.
            string jobID = service.SubmitRequest(
                    sequence,
                    searchParams);
            Assert.IsFalse(string.IsNullOrEmpty(jobID));

            // query the status
            ServiceRequestInformation info = service.GetRequestStatus(jobID);
            if (info.Status != ServiceRequestStatus.Waiting && info.Status != ServiceRequestStatus.Ready)
            {
                string err = ApplicationLog.WriteLine("Unexpected status: '{0}'", info.Status);
                Assert.Fail(err);
            }

            // get async results, poll until ready
            int maxAttempts = 10;
            int attempt = 1;
            object resultsObject = null;
            while (attempt <= maxAttempts
                    && info.Status != ServiceRequestStatus.Error
                    && info.Status != ServiceRequestStatus.Ready)
            {
                ++attempt;
                info = service.GetRequestStatus(jobID);
                Thread.Sleep(
                    info.Status == ServiceRequestStatus.Waiting
                    || info.Status == ServiceRequestStatus.Queued
                    ? 20000 * attempt
                    : 0);
            }

            IBlastParser blastXmlParser = new BlastXmlParser();
            resultsObject = blastXmlParser.Parse(
                    new StringReader(service.GetResult(jobID, searchParams)));

            Assert.IsNotNull(resultsObject);
            List<BlastResult> results = resultsObject as List<BlastResult>;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Records.Count, 1);
            BlastSearchRecord record = results[0].Records[0];
            Assert.AreEqual(record.Statistics.Kappa, 0.041);
            Assert.AreEqual(record.Statistics.Lambda, 0.267);
            Assert.AreEqual(record.Statistics.Entropy, 0.14);

            if (null != record.Hits
                    && 0 < record.Hits.Count)
            {
                Assert.AreEqual(record.Hits.Count, 100);

                Hit hit = record.Hits[0];
                Assert.AreEqual(hit.Accession, "P01013");
                Assert.AreEqual(hit.Id, "gi|129295|sp|P01013.1|OVALX_CHICK");
                if (null != hit.Hsps
                        && 0 < hit.Hsps.Count)
                {
                    Assert.AreEqual(hit.Hsps.Count, 1);
                    Assert.AreEqual(hit.Hsps[0].HitSequence.Substring(0, 30), "QIKDLLVSSSTDLDTTLVLVNAIYFKGMWK");
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }

                IList<BlastResult> results2 = service.FetchResultsSync(jobID, searchParams) as List<BlastResult>;
                Assert.IsNotNull(results2);
                if (null != results[0].Records[0].Hits
                        && 0 < results[0].Records[0].Hits.Count
                        && null != results[0].Records[0].Hits[0].Hsps
                        && 0 < results[0].Records[0].Hits[0].Hsps.Count)
                {
                    Assert.AreEqual(results[0].Records[0].Hits[0].Hsps[0].QuerySequence,
                        results2[0].Records[0].Hits[0].Hsps[0].QuerySequence);
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }
            }
            else
            {
                ApplicationLog.WriteLine("No significant hits found with the these parameters.");
            }
        }

        /// <summary>
        /// Test the NcbiQBlast class for dna (BLASTN).
        /// </summary>
        [Test]
        public void TestNcbiQBlast_Dna()
        {
            // test parameters
            string seq = @"GACGCCGCCGCCACCACCGCCACCGCCGCAGCAGAAGCAGCGCACCGCAGGAGGGAAG" +
                "ATGCCGGCGGGGCACGGGCTGCGGGCGCGGACGGCGACCTCTTCGCGCGGCCGTTCCGCAAGAAGGGTTA" +
                "CATCCCGCTCACCACCTACCTGAGGACGTACAAGATCGGCGATTACGTNGACGTCAAGGTGAACGGTG";
            Sequence sequence = new Sequence(Alphabets.DNA, seq);

            NCBIBlastHandler service = new NCBIBlastHandler();

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            service.Configuration = configParams;

            BlastParameters searchParams = new BlastParameters();
            // fill in the BLAST settings:
            searchParams.Add("Program", "blastn");
            searchParams.Add("Database", "nr");
            // higher Expect will return more results
            //searchParams.Add("Expect", "10");
            searchParams.Add("Expect", "1e-10");
            searchParams.Add("CompositionBasedStatistics", "0");

            // create the request
            string jobID = service.SubmitRequest(sequence, searchParams);
            Assert.IsFalse(string.IsNullOrEmpty(jobID));

            // query the status
            ServiceRequestInformation info = service.GetRequestStatus(jobID);
            if (info.Status != ServiceRequestStatus.Waiting && info.Status != ServiceRequestStatus.Ready)
            {
                string err = ApplicationLog.WriteLine("Unexpected status: '{0}'", info.Status);
                Assert.Fail(err);
            }

            // get async results, poll until ready
            int maxAttempts = 10;
            int attempt = 1;
            object resultsObject = null;
            while (attempt <= maxAttempts
                    && info.Status != ServiceRequestStatus.Error
                    && info.Status != ServiceRequestStatus.Ready)
            {
                ++attempt;
                info = service.GetRequestStatus(jobID);
                Thread.Sleep(
                    info.Status == ServiceRequestStatus.Waiting
                    || info.Status == ServiceRequestStatus.Queued
                    ? 20000 * attempt
                    : 0);
            }

            IBlastParser blastXmlParser = new BlastXmlParser();
            resultsObject = blastXmlParser.Parse(
                    new StringReader(service.GetResult(jobID, searchParams)));

            Assert.IsNotNull(resultsObject);
            List<BlastResult> results = resultsObject as List<BlastResult>;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Records.Count, 1);
            BlastSearchRecord record = results[0].Records[0];
            Assert.AreEqual(record.Statistics.Kappa, 0.41);
            Assert.AreEqual(record.Statistics.Lambda, 0.625);
            Assert.AreEqual(record.Statistics.Entropy, 0.78);

            if (null != record.Hits
                    && 0 < record.Hits.Count)
            {
                Assert.AreEqual(record.Hits.Count, 100);

                Hit hit = record.Hits[0];
                Assert.AreEqual(hit.Accession, "NM_001186331");
                Assert.AreEqual(hit.Id, "gi|297721792|ref|NM_001186331.1|");

                if (null != hit.Hsps
                        && 0 < hit.Hsps.Count)
                {
                    Assert.AreEqual(hit.Hsps.Count, 1);
                    Assert.AreEqual(hit.Hsps[0].HitSequence.Substring(0, 30), "GACGCCGCCGCCACCACCGCCACCGCCGCA");
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }

                IList<BlastResult> results2 = service.FetchResultsSync(jobID, searchParams) as List<BlastResult>;
                Assert.IsNotNull(results2);
                if (null != results[0].Records[0].Hits
                        && 0 < results[0].Records[0].Hits.Count
                        && null != results[0].Records[0].Hits[0].Hsps
                        && 0 < results[0].Records[0].Hits[0].Hsps.Count)
                {
                    Assert.AreEqual(results[0].Records[0].Hits[0].Hsps[0].QuerySequence,
                        results2[0].Records[0].Hits[0].Hsps[0].QuerySequence);
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }
            }
            else
            {
                ApplicationLog.WriteLine("No significant hits found with the these parameters.");
            }
        }

        /// <summary>
        /// Test the NcbiQBlast class with mixed case Program argument
        /// </summary>
        [Test]
        public void TestNcbiQBlast_Casing()
        {
            // test parameters
            string seq = @"GACGCCGCCGCCACCACCGCCACCGCCGCAGCAGAAGCAGCGCACCGCAGGAGGGAAG" +
                "ATGCCGGCGGGGCACGGGCTGCGGGCGCGGACGGCGACCTCTTCGCGCGGCCGTTCCGCAAGAAGGGTTA" +
                "CATCCCGCTCACCACCTACCTGAGGACGTACAAGATCGGCGATTACGTNGACGTCAAGGTGAACGGTG";
            Sequence sequence = new Sequence(Alphabets.DNA, seq);

            NCBIBlastHandler service = new NCBIBlastHandler();

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            service.Configuration = configParams;

            BlastParameters searchParams = new BlastParameters();
            // fill in the BLAST settings:

            // The QBlast service returns an error if this parameter is not lower case.
            // make sure we are forcing correct case.
            searchParams.Add("Program", "BLaStN");

            searchParams.Add("Database", "nr");
            // higher Expect will return more results
            searchParams.Add("Expect", "1e-10");
            searchParams.Add("CompositionBasedStatistics", "0");

            // create the request
            string jobID = service.SubmitRequest(sequence, searchParams);
            Assert.IsFalse(string.IsNullOrEmpty(jobID));

            // query the status
            ServiceRequestInformation info = service.GetRequestStatus(jobID);
            if (info.Status != ServiceRequestStatus.Waiting && info.Status != ServiceRequestStatus.Ready)
            {
                string err = ApplicationLog.WriteLine("Unexpected status: '{0}'", info.Status);
                Assert.Fail(err);
            }

            // get async results, poll until ready
            int maxAttempts = 10;
            int attempt = 1;
            object resultsObject = null;
            while (attempt <= maxAttempts
                    && info.Status != ServiceRequestStatus.Error
                    && info.Status != ServiceRequestStatus.Ready)
            {
                ++attempt;
                info = service.GetRequestStatus(jobID);
                Thread.Sleep(
                    info.Status == ServiceRequestStatus.Waiting
                    || info.Status == ServiceRequestStatus.Queued
                    ? 20000 * attempt
                    : 0);
            }

            IBlastParser blastXmlParser = new BlastXmlParser();
            resultsObject = blastXmlParser.Parse(
                    new StringReader(service.GetResult(jobID, searchParams)));

            Assert.IsNotNull(resultsObject);
            List<BlastResult> results = resultsObject as List<BlastResult>;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Records.Count, 1);
            BlastSearchRecord record = results[0].Records[0];
            Assert.AreEqual(record.Statistics.Kappa, 0.41);
            Assert.AreEqual(record.Statistics.Lambda, 0.625);
            Assert.AreEqual(record.Statistics.Entropy, 0.78);

            if (null != record.Hits
                    && 0 < record.Hits.Count)
            {
                Assert.AreEqual(record.Hits.Count, 100);

                Hit hit = record.Hits[0];
                Assert.AreEqual(hit.Accession, "NM_001186331");
                Assert.AreEqual(hit.Id, "gi|297721792|ref|NM_001186331.1|");
                if (null != hit.Hsps
                        && 0 < hit.Hsps.Count)
                {
                    Assert.AreEqual(hit.Hsps.Count, 1);
                    Assert.AreEqual(hit.Hsps[0].HitSequence.Substring(0, 30), "GACGCCGCCGCCACCACCGCCACCGCCGCA");
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }

                IList<BlastResult> results2 = service.FetchResultsSync(jobID, searchParams) as List<BlastResult>;
                Assert.IsNotNull(results2);
                if (null != results[0].Records[0].Hits
                        && 0 < results[0].Records[0].Hits.Count
                        && null != results[0].Records[0].Hits[0].Hsps
                        && 0 < results[0].Records[0].Hits[0].Hsps.Count)
                {
                    Assert.AreEqual(results[0].Records[0].Hits[0].Hsps[0].QuerySequence,
                        results2[0].Records[0].Hits[0].Hsps[0].QuerySequence);
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }
            }
            else
            {
                ApplicationLog.WriteLine("No significant hits found with the these parameters.");
            }
        }

        /// <summary>
        /// Test the NcbiQBlast class for dna (BLASTN), specifying nonexistent database.
        /// </summary>
        [Test]
        public void TestNcbiQBlast_BadDatabase()
        {
            // test parameters
            string seq = @"GACGCCGCCGCCACCACCGCCACCGCCGCAGCAGAAGCAGCGCACCGCAGGAGGGAAG" +
                "ATGCCGGCGGGGCACGGGCTGCGGGCGCGGACGGCGACCTCTTCGCGCGGCCGTTCCGCAAGAAGGGTTA" +
                "CATCCCGCTCACCACCTACCTGAGGACGTACAAGATCGGCGATTACGTNGACGTCAAGGTGAACGGTG";
            Sequence sequence = new Sequence(Alphabets.DNA, seq);

            string badDbName = "ThisDatabaseDoesNotExist";

            IBlastServiceHandler service = new NCBIBlastHandler();

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            service.Configuration = configParams;

            BlastParameters searchParams = new BlastParameters();
            // fill in the BLAST settings:
            searchParams.Add("Program", "blastn");
            searchParams.Add("Database", badDbName);
            // higher Expect will return more results
            searchParams.Add("Expect", "1e-10");
            searchParams.Add("CompositionBasedStatistics", "0");

            // create the request
            string jobID = service.SubmitRequest(sequence, searchParams);
            Assert.IsFalse(string.IsNullOrEmpty(jobID));

            // query the status
            ServiceRequestInformation info = service.GetRequestStatus(jobID);
            bool ok = false;
            if (info.Status != ServiceRequestStatus.Waiting && info.Status != ServiceRequestStatus.Ready)
            {
                if (info.StatusInformation.Contains(badDbName) &&
                    info.StatusInformation.Contains("No alias or index file found for nucleotide database"))
                {
                    ok = true;
                }
            }
            if (!ok)
            {
                Assert.Fail("Failed to find server error message for bad request. Info: " + info.StatusInformation);
            }
        }

        /// <summary>
        /// Test the EbiWuBlast class for protein (BLASTP).
        /// </summary>
        [Test]
        public void TestEbiWuBlast_Protein()
        {
            IBlastServiceHandler service = new EbiWuBlastHandler();
            service.Configuration = new ConfigParameters();
            BlastParameters blastParams = new BlastParameters();
            blastParams.Add("Program", "blastp");
            blastParams.Add("Database", "swissprot");
            blastParams.Add("Expect", "1e-10");
            blastParams.Add("Email", "msrerbio@microsoft.com");

            // sample query obtained from NCBI help page (http://www.ncbi.nlm.nih.gov/BLAST/blastcgihelp.shtml), 
            // courtesy of National Library of Medicine.
            string query = @"QIKDLLVSSSTDLDTTLVLVNAIYFKGMWKTAFNAEDTREMPFHVTKQESKPVQMMCMNNSFNVATLPAE"
                + "KMKILELPFASGDLSMLVLLPDEVSDLERIEKTINFEKLTEWTNPNTMEKRRVKVYLPQMKIEEKYNLTS"
                + "VLMALGMTDLFIPSANLTGISSAESLKISQAVHGAFMELSEDGIEMAGSTGVIEDIKHSPESEQFRADHP"
                + "FLFLIKHNPTNTIVYFGRYWSP";
            Sequence sequence = new Sequence(Alphabets.Protein, query);

            string jobID = service.SubmitRequest(sequence, blastParams);

            ServiceRequestInformation info = service.GetRequestStatus(jobID);

            int maxAttempts = 10;
            int attempt = 1;
            object resultsObject = null;
            while (attempt <= maxAttempts
                    && info.Status != ServiceRequestStatus.Error
                    && info.Status != ServiceRequestStatus.Ready)
            {
                ++attempt;
                info = service.GetRequestStatus(jobID);
                Thread.Sleep(
                    info.Status == ServiceRequestStatus.Waiting
                    || info.Status == ServiceRequestStatus.Queued
                    ? 20000 * attempt
                    : 0);
            }

            IBlastParser blastXmlParser = new BlastXmlParser();
            resultsObject = blastXmlParser.Parse(
                    new StringReader(service.GetResult(jobID, blastParams)));

            Assert.IsNotNull(resultsObject);
            List<BlastResult> results = resultsObject as List<BlastResult>;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Records.Count, 1);
            BlastSearchRecord record = results[0].Records[0];
            Assert.AreEqual(record.Statistics.Kappa, 0.03);
            Assert.AreEqual(record.Statistics.Lambda, 0.244);
            Assert.AreEqual(record.Statistics.Entropy, 0.18);

            if (null != record.Hits
                    && 0 < record.Hits.Count)
            {
                Assert.AreEqual(record.Hits.Count, 50);

                Hit hit = record.Hits[0];
                Assert.AreEqual(hit.Accession, "SW:OVALX_CHICK");
                Assert.AreEqual(hit.Id, "SW:OVALX_CHICK");

                if (null != hit.Hsps
                        && 0 < hit.Hsps.Count)
                {
                    Assert.AreEqual(hit.Hsps.Count, 1);
                    Assert.AreEqual(hit.Hsps[0].HitSequence.Substring(0, 30), "QIKDLLVSSSTDLDTTLVLVNAIYFKGMWK");
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }

                IList<BlastResult> results2 = service.FetchResultsSync(jobID, blastParams) as List<BlastResult>;
                Assert.IsNotNull(results2);
                if (null != results[0].Records[0].Hits
                        && 0 < results[0].Records[0].Hits.Count
                        && null != results[0].Records[0].Hits[0].Hsps
                        && 0 < results[0].Records[0].Hits[0].Hsps.Count)
                {
                    Assert.AreEqual(results[0].Records[0].Hits[0].Hsps[0].QuerySequence,
                        results2[0].Records[0].Hits[0].Hsps[0].QuerySequence);
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }
            }
            else
            {
                ApplicationLog.WriteLine("No significant hits found with the these parameters.");
            }
        }

        /// <summary>
        /// Test the EbiWuBlast class for dna (BLASTN).
        /// </summary>
        [Test]
        public void TestEBIWuBlast_Dna()
        {
            // test parameters
            string seq = @"GACGCCGCCGCCACCACCGCCACCGCCGCAGCAGAAGCAGCGCACCGCAGGAGGGAAG" +
                "ATGCCGGCGGGGCACGGGCTGCGGGCGCGGACGGCGACCTCTTCGCGCGGCCGTTCCGCAAGAAGGGTTA" +
                "CATCCCGCTCACCACCTACCTGAGGACGTACAAGATCGGCGATTACGTNGACGTCAAGGTGAACGGTG";
            Sequence sequence = new Sequence(Alphabets.DNA, seq);

            IBlastServiceHandler service = new EbiWuBlastHandler();

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            service.Configuration = configParams;

            BlastParameters blastParams = new BlastParameters();
            blastParams.Add("Program", "blastn");
            blastParams.Add("Database", "em_rel");
            blastParams.Add("Expect", "1e-10");
            blastParams.Add("Email", "msrerbio@microsoft.com");

            // create the request
            string jobID = service.SubmitRequest(sequence, blastParams);
            Assert.IsFalse(string.IsNullOrEmpty(jobID));

            // query the status
            ServiceRequestInformation info = service.GetRequestStatus(jobID);
            if (info.Status != ServiceRequestStatus.Waiting && info.Status != ServiceRequestStatus.Ready)
            {
                string err = ApplicationLog.WriteLine("Unexpected status: '{0}'", info.Status);
                Assert.Fail(err);
            }

            // get async results, poll until ready
            int maxAttempts = 10;
            int attempt = 1;
            object resultsObject = null;
            while (attempt <= maxAttempts
                    && info.Status != ServiceRequestStatus.Error
                    && info.Status != ServiceRequestStatus.Ready)
            {
                ++attempt;
                info = service.GetRequestStatus(jobID);
                Thread.Sleep(
                    info.Status == ServiceRequestStatus.Waiting
                    || info.Status == ServiceRequestStatus.Queued
                    ? 20000 * attempt
                    : 0);
            }

            IBlastParser blastXmlParser = new BlastXmlParser();
            resultsObject = blastXmlParser.Parse(
                    new StringReader(service.GetResult(jobID, blastParams)));

            Assert.IsNotNull(resultsObject);
            List<BlastResult> results = resultsObject as List<BlastResult>;
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].Records.Count);
            BlastSearchRecord record = results[0].Records[0];
            Assert.AreEqual(0.0151, record.Statistics.Kappa);
            Assert.AreEqual(0.104, record.Statistics.Lambda);
            Assert.AreEqual(0.06, record.Statistics.Entropy);

            if (null != record.Hits
                    && 0 < record.Hits.Count)
            {
                Assert.AreEqual(50, record.Hits.Count);

                Hit hit = record.Hits[0];
                Assert.AreEqual("EM_EST:D15320;", hit.Accession);
                Assert.AreEqual("EM_EST:D15320;", hit.Id);

                if (null != hit.Hsps
                        && 0 < hit.Hsps.Count)
                {
                    Assert.AreEqual(1, hit.Hsps.Count);
                    Assert.AreEqual("GACGCCGCCGCCACCACCGCCACCGCCGCA", hit.Hsps[0].HitSequence.Substring(0, 30));
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }

                IList<BlastResult> results2 = service.FetchResultsSync(jobID, blastParams) as List<BlastResult>;
                Assert.IsNotNull(results2);
                if (null != results[0].Records[0].Hits
                        && 0 < results[0].Records[0].Hits.Count
                        && null != results[0].Records[0].Hits[0].Hsps
                        && 0 < results[0].Records[0].Hits[0].Hsps.Count)
                {
                    Assert.AreEqual(results[0].Records[0].Hits[0].Hsps[0].QuerySequence,
                        results2[0].Records[0].Hits[0].Hsps[0].QuerySequence);
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }
            }
            else
            {
                ApplicationLog.WriteLine("No significant hits found with the these parameters.");
            }
        }

        /// <summary>
        /// Test the EbiWuBlast class with mixed case Program argument.
        /// </summary>
        [Test]
        public void TestEBIWuBlast_Casing()
        {
            // test parameters
            string seq = @"GACGCCGCCGCCACCACCGCCACCGCCGCAGCAGAAGCAGCGCACCGCAGGAGGGAAG" +
                "ATGCCGGCGGGGCACGGGCTGCGGGCGCGGACGGCGACCTCTTCGCGCGGCCGTTCCGCAAGAAGGGTTA" +
                "CATCCCGCTCACCACCTACCTGAGGACGTACAAGATCGGCGATTACGTNGACGTCAAGGTGAACGGTG";
            Sequence sequence = new Sequence(Alphabets.DNA, seq);

            IBlastServiceHandler service = new EbiWuBlastHandler();

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            service.Configuration = configParams;

            BlastParameters blastParams = new BlastParameters();

            // The WU-BLAST service doesn't seem to care about case of the Program
            // parameter, but make sure it works with forcing upper case.
            blastParams.Add("Program", "BLaSTn");
            blastParams.Add("Database", "em_rel");
            blastParams.Add("Expect", "1e-10");
            blastParams.Add("Email", "msrerbio@microsoft.com");

            // create the request
            string jobID = service.SubmitRequest(sequence, blastParams);
            Assert.IsFalse(string.IsNullOrEmpty(jobID));

            // query the status
            ServiceRequestInformation info = service.GetRequestStatus(jobID);
            if (info.Status != ServiceRequestStatus.Waiting && info.Status != ServiceRequestStatus.Ready)
            {
                string err = ApplicationLog.WriteLine("Unexpected status: '{0}'", info.Status);
                Assert.Fail(err);
            }

            // get async results, poll until ready
            int maxAttempts = 10;
            int attempt = 1;
            object resultsObject = null;
            while (attempt <= maxAttempts
                    && info.Status != ServiceRequestStatus.Error
                    && info.Status != ServiceRequestStatus.Ready)
            {
                ++attempt;
                info = service.GetRequestStatus(jobID);
                Thread.Sleep(
                    info.Status == ServiceRequestStatus.Waiting
                    || info.Status == ServiceRequestStatus.Queued
                    ? 20000 * attempt
                    : 0);
            }

            IBlastParser blastXmlParser = new BlastXmlParser();
            resultsObject = blastXmlParser.Parse(
                    new StringReader(service.GetResult(jobID, blastParams)));

            Assert.IsNotNull(resultsObject);
            List<BlastResult> results = resultsObject as List<BlastResult>;
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].Records.Count);
            BlastSearchRecord record = results[0].Records[0];
            Assert.AreEqual(0.0151, record.Statistics.Kappa);
            Assert.AreEqual(0.104, record.Statistics.Lambda);
            Assert.AreEqual(0.06, record.Statistics.Entropy);

            if (null != record.Hits
                    && 0 < record.Hits.Count)
            {
                Assert.AreEqual(50, record.Hits.Count);

                Hit hit = record.Hits[0];
                Assert.AreEqual("EM_EST:D15320;", hit.Accession);
                Assert.AreEqual("EM_EST:D15320;", hit.Id);

                if (null != record.Hits[0].Hsps
                        && 0 < record.Hits[0].Hsps.Count)
                {
                    Assert.AreEqual(1, hit.Hsps.Count);
                    Assert.AreEqual("GACGCCGCCGCCACCACCGCCACCGCCGCA", hit.Hsps[0].HitSequence.Substring(0, 30));
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }

                IList<BlastResult> results2 = service.FetchResultsSync(jobID, blastParams) as List<BlastResult>;
                Assert.IsNotNull(results2);
                if (null != results[0].Records[0].Hits
                        && 0 < results[0].Records[0].Hits.Count
                        && null != results[0].Records[0].Hits[0].Hsps
                        && 0 < results[0].Records[0].Hits[0].Hsps.Count)
                {
                    Assert.AreEqual(results[0].Records[0].Hits[0].Hsps[0].QuerySequence,
                        results2[0].Records[0].Hits[0].Hsps[0].QuerySequence);
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }
            }
            else
            {
                ApplicationLog.WriteLine("No significant hits found with the these parameters.");
            }
        }

        /// <summary>
        /// Test the EbiWuBlast method for fetching database names and other server metadata.
        /// </summary>
        [Test]
        public void TestEBIWuBlast_GetServiceMetadata()
        {
            EbiWuBlastHandler service = new EbiWuBlastHandler();
            string[] kinds = { 
                                 EbiWuBlastHandler.MetadataDatabases, 
                                 EbiWuBlastHandler.MetadataFilter, 
                                 EbiWuBlastHandler.MetadataMatrices, 
                                 EbiWuBlastHandler.MetadataPrograms, 
                                 EbiWuBlastHandler.MetadataSensitivity, 
                                 EbiWuBlastHandler.MetadataSort, 
                                 EbiWuBlastHandler.MetadataStatistics, 
                                 EbiWuBlastHandler.MetadataXmlFormats 
                             };

            foreach (string kind in kinds)
            {
                ApplicationLog.WriteLine("{0}:", kind);
                foreach (string s in service.GetServiceMetadata(kind))
                {
                    ApplicationLog.WriteLine("\t{0}", s);
                }
            }
        }

        /// <summary>
        /// Test the BioHPC Blast class for protein (BLASTP).
        /// </summary>
        [Test]
        public void TestBioHPCBlast_Protein()
        {
            IBlastServiceHandler service = new BioHPCBlastHandler();
            service.Configuration = new ConfigParameters();
            service.Configuration.EmailAddress = "msrerbio@microsoft.com";
            service.Configuration.Password = "";

            BlastParameters blastParams = new BlastParameters();
            blastParams.Add("Program", "blastp");
            blastParams.Add("Database", "swissprot");
            blastParams.Add("Expect", "1e-10");
            // E-mail notification option is optional - default is "no"
            blastParams.Add("EmailNotify", "no");
            // Job name is optional, default will be provided
            blastParams.Add("JobName", "myBLASTprot_svcjob");

            // sample query obtained from NCBI help page (http://www.ncbi.nlm.nih.gov/BLAST/blastcgihelp.shtml), 
            // courtesy of National Library of Medicine.
            string query = @"QIKDLLVSSSTDLDTTLVLVNAIYFKGMWKTAFNAEDTREMPFHVTKQESKPVQMMCMNNSFNVATLPAE"
                + "KMKILELPFASGDLSMLVLLPDEVSDLERIEKTINFEKLTEWTNPNTMEKRRVKVYLPQMKIEEKYNLTS"
                + "VLMALGMTDLFIPSANLTGISSAESLKISQAVHGAFMELSEDGIEMAGSTGVIEDIKHSPESEQFRADHP"
                + "FLFLIKHNPTNTIVYFGRYWSP";
            Sequence sequence = new Sequence(Alphabets.Protein, query);

            string jobID = service.SubmitRequest(sequence, blastParams);

            ServiceRequestInformation info = service.GetRequestStatus(jobID);

            int maxAttempts = 10;
            int attempt = 1;
            object resultsObject = null;
            while (attempt <= maxAttempts
                    && info.Status != ServiceRequestStatus.Error
                    && info.Status != ServiceRequestStatus.Ready)
            {
                ++attempt;
                info = service.GetRequestStatus(jobID);
                Thread.Sleep(
                    info.Status == ServiceRequestStatus.Waiting
                    || info.Status == ServiceRequestStatus.Queued
                    ? 20000 * attempt
                    : 0);
            }

            IBlastParser blastXmlParser = new BlastXmlParser();
            resultsObject = blastXmlParser.Parse(
                    new StringReader(service.GetResult(jobID, blastParams)));

            Assert.IsNotNull(resultsObject);
            List<BlastResult> results = resultsObject as List<BlastResult>;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Records.Count, 1);
            BlastSearchRecord record = results[0].Records[0];
            Assert.AreEqual(record.Statistics.Kappa, 0.041);
            Assert.AreEqual(record.Statistics.Lambda, 0.267);
            Assert.AreEqual(record.Statistics.Entropy, 0.14);

            if (null != record.Hits
                    && 0 < record.Hits.Count)
            {
                Assert.AreEqual(record.Hits.Count, 10);

                Hit hit = record.Hits[0];
                Assert.AreEqual(hit.Accession, "P01013");
                Assert.AreEqual(hit.Id, "gi|129295|sp|P01013.1|OVALX_CHICK");

                if (null != hit.Hsps
                        && 0 < hit.Hsps.Count)
                {
                    Assert.AreEqual(hit.Hsps.Count, 1);
                    Assert.AreEqual(hit.Hsps[0].HitSequence.Substring(0, 30), "QIKDLLVSSSTDLDTTLVLVNAIYFKGMWK");
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }

                IList<BlastResult> results2 = service.FetchResultsSync(jobID, blastParams) as List<BlastResult>;
                Assert.IsNotNull(results2);
                if (null != results[0].Records[0].Hits
                        && 0 < results[0].Records[0].Hits.Count
                        && null != results[0].Records[0].Hits[0].Hsps
                        && 0 < results[0].Records[0].Hits[0].Hsps.Count)
                {
                    Assert.AreEqual(results[0].Records[0].Hits[0].Hsps[0].QuerySequence,
                        results2[0].Records[0].Hits[0].Hsps[0].QuerySequence);
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }
            }
            else
            {
                ApplicationLog.WriteLine("No significant hits found with the these parameters.");
            }
        }

        /// <summary>
        /// Test the BioHPCBlast class for dna (BLASTN).
        /// </summary>
        [Test]
        public void TestBioHPCBlast_Dna()
        {
            // test parameters
            string seq = @"GACGCCGCCGCCACCACCGCCACCGCCGCAGCAGAAGCAGCGCACCGCAGGAGGGAAG" +
                "ATGCCGGCGGGGCACGGGCTGCGGGCGCGGACGGCGACCTCTTCGCGCGGCCGTTCCGCAAGAAGGGTTA" +
                "CATCCCGCTCACCACCTACCTGAGGACGTACAAGATCGGCGATTACGTNGACGTCAAGGTGAACGGTG";
            Sequence sequence = new Sequence(Alphabets.DNA, seq);

            IBlastServiceHandler service = new BioHPCBlastHandler();

            ConfigParameters configParams = new ConfigParameters();
            configParams.EmailAddress = "msrerbio@microsoft.com";
            configParams.Password = "";
            service.Configuration = configParams;

            BlastParameters blastParams = new BlastParameters();
            blastParams.Add("Program", "blastn");
            blastParams.Add("Database", "nt");
            blastParams.Add("Expect", "1e-10");
            // E-mail notification option is optional - default is "no"
            blastParams.Add("EmailNotify", "no");
            // Job name is optional, if not set - default will be provided
            blastParams.Add("JobName", "myBLASTdna_svcjob");

            // create the request
            string jobID = service.SubmitRequest(sequence, blastParams);
            Assert.IsFalse(string.IsNullOrEmpty(jobID));

            // query the status
            ServiceRequestInformation info = service.GetRequestStatus(jobID);
            if (info.Status != ServiceRequestStatus.Waiting && info.Status != ServiceRequestStatus.Queued && info.Status != ServiceRequestStatus.Ready)
            {
                string err = ApplicationLog.WriteLine("Unexpected status: '{0}'", info.Status);
                Assert.Fail(err);
            }

            // get async results, poll until ready
            int maxAttempts = 10;
            int attempt = 1;
            object resultsObject = null;
            while (attempt <= maxAttempts
                    && info.Status != ServiceRequestStatus.Error
                    && info.Status != ServiceRequestStatus.Ready)
            {
                ++attempt;
                info = service.GetRequestStatus(jobID);
                Thread.Sleep(
                    info.Status == ServiceRequestStatus.Waiting
                    || info.Status == ServiceRequestStatus.Queued
                    ? 20000 * attempt
                    : 0);
            }

            IBlastParser blastXmlParser = new BlastXmlParser();
            resultsObject = blastXmlParser.Parse(
                    new StringReader(service.GetResult(jobID, blastParams)));

            Assert.IsNotNull(resultsObject);
            List<BlastResult> results = resultsObject as List<BlastResult>;
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].Records.Count);
            BlastSearchRecord record = results[0].Records[0];
            Assert.AreEqual(0.710603, record.Statistics.Kappa);
            Assert.AreEqual(1.37406, record.Statistics.Lambda);
            Assert.AreEqual(1.30725, record.Statistics.Entropy);

            if (null != record.Hits
                    && 0 < record.Hits.Count)
            {
                Assert.AreEqual(10, record.Hits.Count);

                Hit hit = record.Hits[0];
                Assert.AreEqual("NM_001055459", hit.Accession);
                Assert.AreEqual("gi|115450646|ref|NM_001055459.1|", hit.Id);

                if (null != hit.Hsps
                        && 0 < hit.Hsps.Count)
                {
                    Assert.AreEqual(1, hit.Hsps.Count);
                    Assert.AreEqual("GACGCCGCCGCCACCACCGCCACCGCCGCA", hit.Hsps[0].HitSequence.Substring(0, 30));
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }

                IList<BlastResult> results2 = service.FetchResultsSync(jobID, blastParams) as List<BlastResult>;
                Assert.IsNotNull(results2);
                if (null != results[0].Records[0].Hits
                        && 0 < results[0].Records[0].Hits.Count
                        && null != results[0].Records[0].Hits[0].Hsps
                        && 0 < results[0].Records[0].Hits[0].Hsps.Count)
                {
                    Assert.AreEqual(results[0].Records[0].Hits[0].Hsps[0].QuerySequence,
                        results2[0].Records[0].Hits[0].Hsps[0].QuerySequence);
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }
            }
            else
            {
                ApplicationLog.WriteLine("No significant hits found with the these parameters.");
            }
        }

        /// <summary>
        /// Test the BioHPCBlast method for fetching database names and other server metadata.
        /// </summary>
        [Test]
        public void TestBioHPCBlast_GetServiceMetadata()
        {
            BioHPCBlastHandler service = new BioHPCBlastHandler();
            service.Configuration = new ConfigParameters();
            service.Configuration.EmailAddress = "msrerbio@microsoft.com";
            service.Configuration.Password = "";
            string[] kinds = { 
                                 BioHPCBlastHandler.MetadataDatabasesDna,
                                 BioHPCBlastHandler.MetadataDatabasesProt,
                                 BioHPCBlastHandler.MetadataFilter, 
                                 BioHPCBlastHandler.MetadataMatrices, 
                                 BioHPCBlastHandler.MetadataPrograms,  
                                 BioHPCBlastHandler.MetadataFormats 
                             };

            foreach (string kind in kinds)
            {
                ApplicationLog.WriteLine("{0}:", kind);
                foreach (string s in service.GetServiceMetadata(kind))
                {
                    ApplicationLog.WriteLine("\t{0}", s);
                }
            }
        }

        /// <summary>
        /// Test the BLAST XML parser.
        /// </summary>
        [Test]
        public void TestBlastXmlParser()
        {
            string filename = @"testdata\BlastXML\NewYork.xml";
            Assert.IsTrue(File.Exists(filename));

            BlastXmlParser parser = new BlastXmlParser();
            IList<BlastResult> results = parser.Parse(filename);
            Assert.AreEqual(1, results.Count);

            BlastXmlMetadata meta = results[0].Metadata;
            Assert.AreEqual(meta.Database, "nt");
            Assert.AreEqual(meta.QueryId, "55025");
            Assert.AreEqual(meta.ParameterExpect, 10.0);
            Assert.AreEqual(meta.ParameterFilter, "L;m;");
            Assert.AreEqual(meta.ParameterGapOpen, 5);
            Assert.AreEqual(meta.ParameterGapExtend, 2);

            Assert.AreEqual(1, results[0].Records.Count);
            BlastSearchRecord record = results[0].Records[0];
            Assert.AreEqual(record.Statistics.DatabaseLength, 1658539059);
            Assert.AreEqual(record.Statistics.Kappa, 0.710603);

            if (null != record.Hits
                    && 0 < record.Hits.Count)
            {
                Assert.AreEqual(record.Hits.Count, 500);

                Assert.AreEqual(record.Hits[0].Accession, "CY063566");
                Assert.AreEqual(record.Hits[0].Hsps.Count, 1);

                if (null != record.Hits[0].Hsps
                        && 0 < record.Hits[0].Hsps.Count)
                {
                    Hsp hsp = record.Hits[0].Hsps[0];
                    Assert.AreEqual(hsp.AlignmentLength, 1701);
                    Assert.AreEqual(hsp.BitScore, 3372.48);
                    Assert.AreEqual(hsp.HitSequence, "ATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATGCAGACACATTATGTATAGGTTATCATGCGAACAATTCAACAGACACTGTAGACACAGTACTAGAAAAGAATGTAACAGTAACACACTCTGTTAACCTTCTAGAAGACAAGCATAACGGGAAACTATGCAAACTAAGAGGGGTAGCCCCATTGCATTTGGGTAAATGTAACATTGCTGGCTGGATCCTGGGAAATCCAGAGTGTGAATCACTCTCCACAGCAAGCTCATGGTCCTACATTGTGGAAACATCTAGTTCAGACAATGGAACGTGTTACCCAGGAGATTTCATCGATTATGAGGAGCTAAGAGAGCAATTGAGCTCAGTGTCATCATTTGAAAGGTTTGAGATATTCCCCAAGACAAGTTCATGGCCCAATCATGACTCGAACAAAGGTGTAACGGCAGCATGTCCTCATGCTGGAGCAAAAAGCTTCTACAAAAATTTAATATGGCTAGTTAAAAAAGGAAATTCATACCCAAAGCTCAGCAAATCCTACATTAATGATAAAGGGAAAGAAGTCCTCGTGCTATGGGGCATTCACCATCCATCTACTAGTGCTGACCAACAAAGTCTCTATCAGAATGCAGATGCATATGTTTTTGTGGGGACATCAAGATACAGCAAGAAGTTCAAGCCGGAAATAGCAATAAGACCCAAAGTGAGGGATCAAGAAGGGAGAATGAACTATTACTGGACACTAGTAGAGCCGGGAGACAAAATAACATTCGAAGCAACTGGAAATCTAGTGGTACCGAGATATGCATTCGCAATGGAAAGAAATGCTGGATCTGGTATTATCATTTCAGATACACCAGTCCACGATTGCAATACAACTTGTCAGACACCCAAGGGTGCTATAAACACCAGCCTCCCATTTCAGAATATACATCCGATCACAATTGGAAAATGTCCAAAATATGTAAAAAGCACAAAATTGAGACTGGCCACAGGATTGAGGAATGTCCCGTCTATTCAATCTAGAGGCCTATTTGGGGCCATTGCCGGTTTCATTGAAGGGGGGTGGACAGGGATGGTAGATGGATGGTACGGTTATCACCATCAAAATGAGCAGGGGTCAGGATATGCAGCCGACCTGAAGAGCACACAGAATGCCATTGACGAGATTACTAACAAAGTAAATTCTGTTATTGAAAAGATGAATACACAGTTCACAGCAGTAGGTAAAGAGTTCAACCACCTGGAAAAAAGAATAGAGAATTTAAATAAAAAAATTGATGATGGTTTCCTGGACATTTGGACTTACAATGCCGAACTGTTGGTTCTATTGGAAAATGAAAGAACTTTGGACTACCACGATTCAAATGTGAAGAACTTATATGAAAAGGTAAGAAGCCAGTTAAAAAACAATGCCAAGGAAATTGGAAACGGCTGCTTTGAATTTTACCACAAATGCGATAACACGTGCATGGAAAGTGTCAAAAATGGGACTTATGACTACCCAAAATACTCAGAGGAAGCAAAATTAAACAGAGAAGAAATAGATGGGGTAAAGCTGGAATCAACAAGGATTTACCAGATTTTGGCGATCTATTCAACTGTCGCCAGTTCATTGGTACTGGTAGTCTCCCTGGGGGCAATCAGTTTCTGGATGTGCTCTAATGGGTCTCTACAGTGTAGAATATGTATTTAA");
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }
            }
            else
            {
                ApplicationLog.WriteLine("No significant hits found with the these parameters.");
            }
        }

        /// <summary>
        /// Test the BLAST XML parser with empty input.
        /// </summary>
        [Test]
        public void TestBlastXmlParser_EmptyInput()
        {
            string content = string.Empty;
            StringReader reader = new StringReader(content);
            BlastXmlParser parser = new BlastXmlParser();
            bool ok = false;
            try
            {
                IList<BlastResult> results = parser.Parse(reader);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("No records were found in the input."));
                ok = true;
            }
            Assert.IsTrue(ok);
        }

        /// <summary>
        /// Test the Azure Blast class for DNA (BLASTX).
        /// <remarks>
        ///     "alu.a" database does not support DNA sequence
        ///     Commenting / Disabling this test case.
        /// </remarks>
        /// </summary>
        //[Test]
        public void TestAzureWuBlast_Dna()
        {
            ConfigParameters configParams = new ConfigParameters();
            configParams.DefaultTimeout = 60;
            configParams.UseBrowserProxy = true;
            configParams.Connection = new Uri("http://blast2.cloudapp.net/BlastService.svc");

            IBlastServiceHandler service = new AzureBlastHandler(configParams);

            BlastParameters blastParams = new BlastParameters();
            blastParams.Add("Program", "BlastX");
            blastParams.Add("Database", "nr.25");

            string query = @"ATTTTTTACGAACCTGTGGAAATTTTTGGTTATGACAATAAATCTAGTTTAGTACTTGTGAAACGTTTAA"
                    + "TTACTCGAATGTATCAACAGAATTTTTTGATTTCTTCGGTTAATGATTCTAACCAAAAAGGATTTTGGGG"
                    + "GCACAAGCATTTTTTTTCTTCTCATTTTTCTTCTCAAATGGTATCAGAAGGTTTTGGAGTCATTCTGGAA"
                    + "ATTCCATTCTCGTCGCAATTAGTATCTTCTCTTGAAGAAAAAAAAATACCAAAATATCAGAATTTACGAT"
                    + "CTATTCATTCAATATTTCCCTTTTTAGAAGACAAATTTTTACATTTGAATTATGTGTCAGATCTACTAAT"
                    + "ACCCCATCCCATCCATCTGGAAATCTTGGTTCAAATCCTTCAATGCCGGATCAAGGATGTTCCTTCTTTG"
                    + "CATTTATTGCGATTGCTTTTCCACGAATATCATAATTTGAATAGTCTCATTACTTCAAAGAAATTCATTT"
                    + "ACGCCTTTTCAAAAAGAAAGAAAAGATTCCTTTGGTTACTATATAATTCTTATGTATATGAATGCGAATA"
                    + "TCTATTCCAGTTTCTTCGTAAACAGTCTTCTTATTTACGATCAACATCTTCTGGAGTCTTTCTTGAGCGA"
                    + "ACACATTTATATGTAAAAATAGAACATCTTCTAGTAGTGTGTTGTAATTCTTTTCAGAGGATCCTATGCT"
                    + "TTCTCAAGGATCCTTTCATGCATTATGTTCGATATCAAGGAAAAGCAATTCTGGCTTCAAAGGGAACTCT"
                    + "TATTCTGATGAAGAAATGGAAATTTCATCTTGTGAATTTTTGGCAATCTTATTTTCACTTTTGGTCTCAA"
                    + "CCGTATAGGATTCATATAAAGCAATTATCCAACTATTCCTTCTCTTTTCTGGGGTATTTTTCAAGTGTAC"
                    + "TAGAAAATCATTTGGTAGTAAGAAATCAAATGCTAGAGAATTCATTTATAATAAATCTTCTGACTAAGAA"
                    + "ATTCGATACCATAGCCCCAGTTATTTCTCTTATTGGATCATTGTCGAAAGCTCAATTTTGTACTGTATTG"
                    + "GGTCATCCTATTAGTAAACCGATCTGGACCGATTTCTCGGATTCTGATATTCTTGATCGATTTTGCCGGA"
                    + "TATGTAGAAATCTTTGTCGTTATCACAGCGGATCCTCAAAAAAACAGGTTTTGTATCGTATAAAATATAT"
                    + "ACTTCGACTTTCGTGTGCTAGAACTTTGGCACGGAAACATAAAAGTACAGTACGCACTTTTATGCGAAGA"
                    + "TTAGGTTCGGGATTATTAGAAGAATTCTTTATGGAAGAAGAA";
            Sequence sequence = new Sequence(Alphabets.DNA, query);

            string jobID = service.SubmitRequest(sequence, blastParams);
            ServiceRequestInformation info = service.GetRequestStatus(jobID);

            int maxAttempts = 20;
            int attempt = 1;
            object resultsObject = null;
            while (attempt <= maxAttempts
                    && info.Status != ServiceRequestStatus.Error
                    && info.Status != ServiceRequestStatus.Ready)
            {
                ++attempt;
                info = service.GetRequestStatus(jobID);
                Thread.Sleep(
                    info.Status == ServiceRequestStatus.Waiting
                    || info.Status == ServiceRequestStatus.Queued
                    ? 20000 * attempt
                    : 0);
            }

            IBlastParser blastXmlParser = new BlastXmlParser();
            resultsObject = blastXmlParser.Parse(
                    new StringReader(service.GetResult(jobID, blastParams)));
            Assert.IsNotNull(resultsObject);
            List<BlastResult> results = resultsObject as List<BlastResult>;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Records.Count, 1);
            BlastSearchRecord record = results[0].Records[0];
            Assert.AreEqual(record.Statistics.Kappa, 0.041);
            Assert.AreEqual(record.Statistics.Lambda, 0.267);
            Assert.AreEqual(record.Statistics.Entropy, 0.14);

            if (null != record.Hits
                    && 0 < record.Hits.Count)
            {
                Assert.AreEqual(record.Hits.Count, 259);

                Hit hit = record.Hits[0];
                Assert.AreEqual(hit.Accession, "5412286");
                Assert.AreEqual(hit.Id, "gnl|BL_ORD_ID|5412286");

                if (null != hit.Hsps
                        && 0 < hit.Hsps.Count)
                {
                    Assert.AreEqual(hit.Hsps.Count, 1);
                    Assert.AreEqual(hit.Hsps[0].HitSequence.Substring(0, 30), "FYKPVEIFGYDNKSSLVLVKRLITRMYQQN");
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }

                IList<BlastResult> results2 = service.FetchResultsSync(jobID, blastParams) as List<BlastResult>;
                Assert.IsNotNull(results2);
                if (null != results[0].Records[0].Hits
                        && 0 < results[0].Records[0].Hits.Count
                        && null != results[0].Records[0].Hits[0].Hsps
                        && 0 < results[0].Records[0].Hits[0].Hsps.Count)
                {
                    Assert.AreEqual(results[0].Records[0].Hits[0].Hsps[0].QuerySequence,
                        results2[0].Records[0].Hits[0].Hsps[0].QuerySequence);
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                }
            }
            else
            {
                ApplicationLog.WriteLine("No significant hits found with the these parameters.");
            }
        }

        /// <summary>
        /// Test the Azure Blast class for protein (BLASTX).
        /// Will return 0 Hits
        /// <remarks>
        ///     Test data in this test case work fine in azure web page.
        ///     But returns invalid result path when accessed using web service.
        ///     Looks like there is some issue with AzureBlast service. 
        ///     Commenting / Disabling this test case.
        /// </remarks>
        /// </summary>
        //[Test]
        public void TestAzureWuBlast_Protein()
        {
            ConfigParameters configParams = new ConfigParameters();
            configParams.DefaultTimeout = 60;
            configParams.UseBrowserProxy = true;
            configParams.Connection = new Uri("http://blast2.cloudapp.net/BlastService.svc");

            IBlastServiceHandler service = new AzureBlastHandler(configParams);

            BlastParameters blastParams = new BlastParameters();
            blastParams.Add("Program", "BlastP");
            blastParams.Add("Database", "alu.a");

            string query = @"MAYPMQLGFQDATSPIMEELLHFHDHTLMIVFLISSLVLYIISLMLTTKLTHTSTMDAQEVETIWTILPAIILILI"
                    + "ALPSLRILYMMDEINNPSLTVKTMGHQWYWSYEYTDYEDLSFDSYMIPTSELKPGELRLLEVDNRVVLPMEMTIRM"
                    + "LVSSEDVLHSWAVPSLGLKTDAIPGRLNQTTLMSSRPGLYYGQCSEICGSNHSFMPIVLELVPLKYFEKWSASML";
            Sequence sequence = new Sequence(Alphabets.Protein, query);

            string jobID = service.SubmitRequest(sequence, blastParams);
            ServiceRequestInformation info = service.GetRequestStatus(jobID);

            int maxAttempts = 20;
            int attempt = 1;
            object resultsObject = null;
            while (attempt <= maxAttempts
                    && info.Status != ServiceRequestStatus.Error
                    && info.Status != ServiceRequestStatus.Ready)
            {
                ++attempt;
                info = service.GetRequestStatus(jobID);
                Thread.Sleep(
                    info.Status == ServiceRequestStatus.Waiting
                    || info.Status == ServiceRequestStatus.Queued
                    ? 20000 * attempt
                    : 0);
            }

            IBlastParser blastXmlParser = new BlastXmlParser();
            resultsObject = blastXmlParser.Parse(
                    new StringReader(service.GetResult(jobID, blastParams)));
            Assert.IsNotNull(resultsObject);
            List<BlastResult> results = resultsObject as List<BlastResult>;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Records.Count, 2);
            BlastSearchRecord record = results[0].Records[0];
            Assert.AreEqual(record.Statistics.Kappa, 0.041);
            Assert.AreEqual(record.Statistics.Lambda, 0.267);
            Assert.AreEqual(record.Statistics.Entropy, 0.14);

            if (null == record.Hits
                    || 0 == record.Hits.Count)
            {
                ApplicationLog.WriteLine("No significant hits found with the these parameters.");
            }
        }
    }
}

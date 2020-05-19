// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1824:MarkAssembliesWithNeutralResourcesLanguage")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Scope = "member", Target = "MBF.Algorithms.Assembly.PaDeNA.Scaffold.PathPurger.#RemoveOverlappingPaths(MBF.Algorithms.Assembly.PaDeNA.Scaffold.ScaffoldPath,MBF.Algorithms.Assembly.PaDeNA.Scaffold.ScaffoldPath)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "MBF.Algorithms.Assembly.PaDeNA.Scaffold.GraphScaffoldBuilder.#GenerateContigOverlapGraph(System.Collections.Generic.IList`1<MBF.ISequence>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member", Target = "MBF.Algorithms.Assembly.PaDeNA.Scaffold.GraphScaffoldBuilder.#BuildScaffold(System.Collections.Generic.IList`1<MBF.ISequence>,System.Collections.Generic.IList`1<MBF.ISequence>,System.Int32,System.Int32,System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member", Target = "MBF.Algorithms.Assembly.PaDeNA.Scaffold.OrientationBasedMatePairFilter.#FilterPairedReads(MBF.Algorithms.Assembly.PaDeNA.Scaffold.ContigMatePairs,System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member", Target = "MBF.Algorithms.Assembly.PaDeNA.Scaffold.TracePath.#FindPaths(MBF.Algorithms.Assembly.Graph.DeBruijnGraph,MBF.Algorithms.Assembly.PaDeNA.Scaffold.ContigMatePairs,System.Int32,System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "MBF.Algorithms.Assembly.PaDeNA.Utility")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member", Target = "MBF.Algorithms.Assembly.PaDeNA.DanglingLinksPurger.#.ctor(System.Int32,System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member", Target = "MBF.Algorithms.Assembly.PaDeNA.DanglingLinksPurger.#ErodeGraphEnds(MBF.Algorithms.Assembly.Graph.DeBruijnGraph,System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "readLength", Scope = "member", Target = "MBF.Algorithms.Assembly.PaDeNA.Utility.ReadAlignment.#MapRead(System.Int32,System.Collections.Generic.IList`1<System.Collections.Generic.IList`1<MBF.Algorithms.Kmer.KmerIndexer>>,System.Int32,System.Int32,System.Int32)")]

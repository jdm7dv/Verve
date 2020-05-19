// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using Bio.IO.BAM;
using Bio.IO.SAM;
using CommandLine;
using SamUtil.Properties;

namespace SamUtil
{
    /// <summary>
    /// Class implementing sort command of SAMUtility.
    /// </summary>
    public class Sort
    {
        #region Field Variables

        /// <summary>
        /// Paths of output file and input files.
        /// </summary>
        [DefaultArgument(ArgumentType.MultipleUnique, HelpText = "File Paths")]
        public string[] FilePaths;

        /// <summary>
        /// Sort input files by read names.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Sort by read name", ShortName = "n")]
        public bool SortByReadName;

        /// <summary>
        /// Usage.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce)]
        public bool Usage;

        #endregion

        #region Public Methods

        /// <summary>
        /// Public method to sort BAM file.
        /// SAMUtil.exe in.bam out.bam
        /// </summary>
        public void DoSort()
        {
            string sortExtension = ".sort";
            if (FilePaths == null)
            {
                throw new InvalidOperationException("FilePaths");
            }

            if (FilePaths.Length < 1)
            {
                throw new InvalidOperationException(Resources.SortHelp);
            }

            BAMParser parse = new BAMParser();
            SequenceAlignmentMap map = null;
            try
            {
                map = parse.Parse(FilePaths[0]);
            }
            catch
            {
                throw new InvalidOperationException(Resources.InvalidBAMFile);
            }
            BAMFormatter format = new BAMFormatter();
            format.CreateSortedBAMFile = true;
            format.SortType = SortByReadName ? BAMSortByFields.ReadNames : BAMSortByFields.ChromosomeCoordinates;
            if (FilePaths.Length > 1)
            {
                format.Format(map, FilePaths[1]);
            }
            else
            {
                format.Format(map, FilePaths[0] + sortExtension);
            }
        }

        #endregion
    }
}
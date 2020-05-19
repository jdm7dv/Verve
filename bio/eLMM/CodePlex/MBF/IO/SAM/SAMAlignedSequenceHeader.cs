//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************



using System;
using System.Collections.Generic;
using System.Linq;
using Bio.Properties;
using Bio.Util;
using System.Globalization;

namespace Bio.IO.SAM
{
    /// <summary>
    /// SAMAlignedSequenceHeader holds aligned sequence headers of the sam file format.
    /// </summary>
    [Serializable]
    public class SAMAlignedSequenceHeader
    {
        #region Fields
        /// <summary>
        /// Holds left co-ordinate of alignment.
        /// </summary>
        private int _pos;

        /// <summary>
        /// Holds read (query sequence) length depending on CIGAR value.
        /// </summary>
        private int _readLength;

        /// <summary>
        /// Holds CIGAR value.
        /// </summary>
        string _cigar;

        /// <summary>
        /// Holds bin number.
        /// </summary>
        int _bin;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates new SAMAlignedSequenceHeader instance.
        /// </summary>
        public SAMAlignedSequenceHeader()
        {
            OptionalFields = new List<SAMOptionalField>();
            DotSymbolIndices = new List<int>();
            EqualSymbolIndices = new List<int>();
            _cigar = "*";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Query pair name if paired; or Query name if unpaired.
        /// </summary>  
        public string QName { get; set; }

        /// <summary>
        /// SAM flags.
        /// <see cref="SAMFlags"/>
        /// </summary>
        public SAMFlags Flag { get; set; }

        /// <summary>
        /// Reference sequence name.
        /// </summary>
        public string RName { get; set; }

        /// <summary>
        /// One-based leftmost position/coordinate of the aligned sequence.
        /// </summary>
        public int Pos
        {
            get
            {
                return _pos;
            }
            set
            {
                _pos = value;
                _readLength = GetReadLengthFromCIGAR();
                _bin = GetBin();
            }
        }

        /// <summary>
        /// Mapping quality (phred-scaled posterior probability that the 
        /// mapping position of this read is incorrect).
        /// </summary>
        public int MapQ { get; set; }

        /// <summary>
        /// Extended CIGAR string.
        /// </summary>
        public string CIGAR
        {
            get
            {
                return _cigar;
            }
            set
            {
                _cigar = value;
                _readLength = GetReadLengthFromCIGAR();
                _bin = GetBin();
            }
        }

        /// <summary>
        /// Mate reference sequence name. 
        /// </summary>
        public string MRNM { get; set; }

        /// <summary>
        /// One-based leftmost mate position of the clipped sequence.
        /// </summary>
        public int MPos { get; set; }

        /// <summary>
        /// Inferred insert size.
        /// </summary>
        public int ISize { get; set; }

        /// <summary>
        /// Gets the Bin depending on the POS and CIGAR.
        /// </summary>
        public int Bin
        {
            get
            {
                return _bin;
            }

            internal set
            {
                _bin = value;
            }
        }

        /// <summary>
        /// Gets the query length depending on CIGAR Value.
        /// </summary>
        public int QueryLength
        {
            get
            {
                return _readLength;
            }

            internal set
            {
                _readLength = value;
            }
        }

        /// <summary>
        /// Contains the list of indices of "." symbols present in the aligned sequence.
        /// As "." is not supported by DNA, RNA and Protien alphabets, while creating aligned 
        /// sequence "." symbols are replaced by "N" which has the same meaning of ".".
        /// </summary>
        public IList<int> DotSymbolIndices { get; private set; }

        /// <summary>
        /// Contains the list of "=" symbol indices present in the aligned sequence.
        /// The "=" symbol in aligned sequence indicates that the symbol at this index 
        /// is equal to the symbol present in the reference sequence. As "=" is not 
        /// supported by DNA, RNA and Protien alphabets, while creating aligned 
        /// sequence "=" symbols are replaced by the symbol present in the reference 
        /// sequence at the same index.
        /// </summary>
        public IList<int> EqualSymbolIndices { get; private set; }

        /// <summary>
        /// Optional fields.
        /// </summary>
        public IList<SAMOptionalField> OptionalFields { get; private set; }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Gets the SAMFlag for the specified integer value.
        /// </summary>
        /// <param name="value">Value for which the SAMFlag is required.</param>
        public static SAMFlags GetFlag(int value)
        {
            return (SAMFlags)value;
        }

        /// <summary>
        /// Gets the SAMFlag for the specified string value.
        /// </summary>
        /// <param name="value">Value for which the SAMFlag is required.</param>
        public static SAMFlags GetFlag(string value)
        {
            return GetFlag(int.Parse(value));
        }

        /// <summary>
        /// Gets Bin for the specified region.
        /// Note that this method returns zero for negative values.
        /// </summary>
        /// <param name="start">Zero based start co-ordinate of alignment.</param>
        /// <param name="end">Zero based end co-ordinate of the alignment.</param>
        public static int RegionToBin(int start, int end)
        {
            if (start < 0 || end <= 0)
            {
                return 0;
            }

            --end;
            if (start >> 14 == end >> 14) return ((1 << 15) - 1) / 7 + (start >> 14);
            if (start >> 17 == end >> 17) return ((1 << 12) - 1) / 7 + (start >> 17);
            if (start >> 20 == end >> 20) return ((1 << 9) - 1) / 7 + (start >> 20);
            if (start >> 23 == end >> 23) return ((1 << 6) - 1) / 7 + (start >> 23);
            if (start >> 26 == end >> 26) return ((1 << 3) - 1) / 7 + (start >> 26);
            return 0;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates this header.
        /// </summary>
        public string IsValid()
        {
            string message = string.Empty;

            message = IsValidQName();
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            message = IsValidRName();
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            message = IsValidPos();
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            message = IsValidMapQ();
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            message = IsValidCIGAR();
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            message = IsValidMRNM();
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            message = IsValidMPos();
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            message = IsValidISize();
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            foreach (SAMOptionalField field in OptionalFields)
            {
                message = field.IsValid();
                if (!string.IsNullOrEmpty(message))
                {
                    return message;
                }
            }

            return string.Empty;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the bin number.
        /// </summary>
        private int GetBin()
        {
            if (!string.IsNullOrEmpty(IsValidCIGAR()) || !string.IsNullOrEmpty(IsValidPos()))
            {
                return 0;
            }

            // As SAM stores 1 based position and to calculte BAM Bin zero based positions are required.
            int start = Pos - 1;
            int end = start + _readLength - 1;
            return RegionToBin(start, end);
        }

        /// <summary>
        /// Gets the read sequence length depending on the CIGAR value.
        /// </summary>
        /// <returns>Length of the read.</returns>
        private int GetReadLengthFromCIGAR()
        {
            if (string.IsNullOrWhiteSpace(CIGAR) || CIGAR.Equals("*"))
            {
                return -1;
            }

            List<char> chars = new List<char>();
            Dictionary<int, int> dic = new Dictionary<int, int>();

            for (int i = 0; i < CIGAR.Length; i++)
            {
                char ch = CIGAR[i];
                if (Char.IsDigit(ch))
                {
                    continue;
                }

                chars.Add(ch);
                dic.Add(chars.Count - 1, i);
            }

            string CIGARforClen = "MDN";
            int len = 0;
            for (int i = 0; i < chars.Count; i++)
            {
                char ch = chars[i];
                int start = 0;
                int end = 0;
                if (CIGARforClen.Contains(ch))
                {
                    if (i == 0)
                    {
                        start = 0;
                    }
                    else
                    {
                        start = dic[i - 1] + 1;
                    }

                    end = dic[i] - start;

                    len += int.Parse(CIGAR.Substring(start, end), CultureInfo.InvariantCulture);
                }
            }

            return len;
        }

        /// <summary>
        /// Validates QName.
        /// </summary>
        private string IsValidQName()
        {
            string regxExpr = "[^ \t\n\r]+";
            string headerName = "QName";
            string headerValue = QName;
            // Validate for the regx.
            string message = Helper.IsValidPatternValue(headerName, headerValue, regxExpr);

            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            // validate length.
            if (QName.Length > 254)
            {
                return Resource.InvalidQNameLength;
            }

            return string.Empty;
        }

        /// <summary>
        /// Validates RName.
        /// </summary>
        private string IsValidRName()
        {
            string regxExpr = "[^ \t\n\r@=]+";
            string headerName = "RName";
            string headerValue = RName;

            // Validate for the regx.
            return Helper.IsValidPatternValue(headerName, headerValue, regxExpr);
        }

        /// <summary>
        /// Validates Pos.
        /// </summary>
        private string IsValidPos()
        {
            int maxValue = ((int)Math.Pow(2, 29)) - 1;
            int minValue = 0;
            string headerName = "Pos";
            int headerValue = Pos;

            return Helper.IsValidRange(headerName, headerValue, minValue, maxValue);
        }

        /// <summary>
        /// Validates MapQ.
        /// </summary>
        private string IsValidMapQ()
        {
            int maxValue = ((int)Math.Pow(2, 8)) - 1;
            int minValue = 0;

            string headerName = "MapQ";
            int headerValue = MapQ;

            return Helper.IsValidRange(headerName, headerValue, minValue, maxValue);
        }

        /// <summary>
        /// Validates CIGAR.
        /// </summary>
        private string IsValidCIGAR()
        {
            string regxExpr = @"([0-9]+[MIDNSHP])+|\*";
            string headerName = "CIGAR";
            string headerValue = CIGAR;

            // Validate for the regx.
            return Helper.IsValidPatternValue(headerName, headerValue, regxExpr);
        }

        /// <summary>
        /// Validates MRNM.
        /// </summary>
        private string IsValidMRNM()
        {
            string regxExpr = @"[^ \t\n\r@]+";
            string headerName = "MRNM";
            string headerValue = MRNM;

            // Validate for the regx.
            return Helper.IsValidPatternValue(headerName, headerValue, regxExpr);
        }

        /// <summary>
        /// Validates MPos.
        /// </summary>
        private string IsValidMPos()
        {
            int maxValue = ((int)Math.Pow(2, 29)) - 1;
            int minValue = 0;

            string headerName = "MPos";
            int headerValue = MPos;

            return Helper.IsValidRange(headerName, headerValue, minValue, maxValue);
        }

        /// <summary>
        /// Validates ISize.
        /// </summary>
        private string IsValidISize()
        {
            int maxValue = (int)Math.Pow(2, 29);
            int minValue = -(int)Math.Pow(2, 29);
            string headerName = "ISize";
            int headerValue = ISize;
            return Helper.IsValidRange(headerName, headerValue, minValue, maxValue);
        }
        #endregion
    }
}

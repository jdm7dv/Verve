//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Data;
using System.Data.Common;
using System.IO;

namespace MBT.Escience
{
	public class StreamDataReader : DbDataReader
	{
		public StreamDataReader(string path) : this(new StreamReader(path))
		{
			m_strPath = path;
		}

		public StreamDataReader(TextReader reader)
		{
			string	strLine = "";
			char[]	rgchSep = { '\t' };

			m_reader = reader;

			while ((strLine = m_reader.ReadLine()) != null)
			{
				if (!strLine.StartsWith("#"))
					break;
			}

			if (strLine == null)
			{
				m_rgstrColumn = new string[0];
				return;
			}

			m_rgstrColumn = strLine.Split(rgchSep, StringSplitOptions.RemoveEmptyEntries);
			Trim(m_rgstrColumn);
		}

		string			m_strPath = "[unknown]";
		int				m_iline = 0;
		TextReader		m_reader;
		string[]		m_rgstrColumn;
		string[]		m_rgstrField;

		public override void Close()
		{
			m_reader.Close();
		}

		public override int Depth
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public override int FieldCount
		{
			get { return m_rgstrColumn.Length; }
		}

		public override bool GetBoolean(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override byte GetByte(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override char GetChar(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override string GetDataTypeName(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override DateTime GetDateTime(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override decimal GetDecimal(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override double GetDouble(int ordinal)
		{
			return double.Parse(m_rgstrField[ordinal]);
		}

		public bool GetBool(int ordinal)
		{
			bool b;
			if(!bool.TryParse(m_rgstrField[ordinal], out b)){
				int i = int.Parse(m_rgstrField[ordinal]);
				if(i == 0)
					b = false;
				else if(i == 1)
					b = true;
				else
					throw new ArgumentException("Cannot parse " + m_rgstrField[ordinal] + " into a boolean.");
			}
			return b;
		}

		public override System.Collections.IEnumerator GetEnumerator()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override Type GetFieldType(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override float GetFloat(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override Guid GetGuid(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override short GetInt16(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override int GetInt32(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override long GetInt64(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override string GetName(int ordinal)
		{
			return m_rgstrColumn[ordinal];
		}

		public override int GetOrdinal(string name)
		{
			int		ifield = SpecialFunctions.IgnoreCaseIndexOf(m_rgstrColumn, name);

			if (ifield == -1)
			{
				throw new IndexOutOfRangeException(string.Format("input {0}: column name '{1}' not found", Path.GetFileName(m_strPath), name));
			}

			return ifield;
		}

		public override DataTable GetSchemaTable()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override string GetString(int ordinal)
		{
			return m_rgstrField[ordinal];
		}

		public override object GetValue(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override int GetValues(object[] values)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool HasRows
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public override bool IsClosed
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public override bool IsDBNull(int ordinal)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool NextResult()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool Read()
		{
			string	strLine;

			while ((strLine = m_reader.ReadLine()) != null)
			{
				++m_iline;

				if (strLine.StartsWith("#"))
					continue;

				if (strLine.Trim().Length == 0)
					continue;

				m_rgstrField = strLine.Split('\t');
			
				if (m_rgstrField.Length != m_rgstrColumn.Length)
				{
					throw new ApplicationException(string.Format("{0}({1}) error: incorrect number of fields, expected {2}, found {3}", 
																			m_strPath, m_iline, m_rgstrColumn.Length, m_rgstrField.Length));
				}

				return true;
			}

			return false;
		}

		public override int RecordsAffected
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public override object this[string name]
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public override object this[int ordinal]
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		private void Trim(string[] rgstr)
		{
			for (int istr = 0; istr < rgstr.Length; ++istr)
			{
				rgstr[istr] = rgstr[istr].Trim();
			}
		}
	}
}


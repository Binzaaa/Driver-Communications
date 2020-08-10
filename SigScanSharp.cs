// Decompiled with JetBrains decompiler
// Type: Driver_Example.IO.SigScanSharp
// Assembly: FREEMONEY, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 046FF60D-F509-497E-88D3-21C1139442E3
// Assembly location: C:\Users\USER\AppData\Local\Temp\Report.2B3E4829-B07D-45FE-A880-1C31A9ABADC0\poop.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Driver_Example.IO
{
	public class SigScanSharp
	{
		public byte[] g_arrModuleBuffer;

		private ulong g_lpModuleBase { get; set; }

		private Dictionary<string, string> g_dictStringPatterns { get; }

		public SigScanSharp()
		{
			this.g_dictStringPatterns = new Dictionary<string, string>();
		}

		public bool SetScanArea(ulong start, ulong length)
		{
			this.g_dictStringPatterns.Clear();
			this.g_lpModuleBase = start;
			this.g_arrModuleBuffer = MEMAPI.ReadBytes(start, length, false);
			return true;
		}

		public void AddPattern(string szPatternName, string szPattern)
		{
			this.g_dictStringPatterns.Add(szPatternName, szPattern);
		}

		private bool PatternCheck(int nOffset, byte[] arrPattern)
		{
			for (int index = 0; index < arrPattern.Length; ++index)
			{
				if (arrPattern[index] != (byte)0 && (int)arrPattern[index] != (int)this.g_arrModuleBuffer[nOffset + index])
					return false;
			}
			return true;
		}

		public ulong FindPattern(string szPattern, out long lTime)
		{
			if (this.g_arrModuleBuffer == null || this.g_lpModuleBase == 0UL)
				throw new Exception(this.g_arrModuleBuffer.ToString() + ", " + this.g_lpModuleBase.ToString());
			Stopwatch stopwatch = Stopwatch.StartNew();
			byte[] patternString = this.ParsePatternString(szPattern);
			for (int nOffset = 0; nOffset < this.g_arrModuleBuffer.Length; ++nOffset)
			{
				if ((int)this.g_arrModuleBuffer[nOffset] == (int)patternString[0] && this.PatternCheck(nOffset, patternString))
				{
					lTime = stopwatch.ElapsedMilliseconds;
					return this.g_lpModuleBase + (ulong)nOffset;
				}
			}
			lTime = stopwatch.ElapsedMilliseconds;
			return 0;
		}

		public Dictionary<string, ulong> FindPatterns(out long lTime)
		{
			if (this.g_arrModuleBuffer == null || this.g_lpModuleBase == 0UL)
				throw new Exception("Selected module is null");
			Stopwatch stopwatch = Stopwatch.StartNew();
			byte[][] numArray1 = new byte[this.g_dictStringPatterns.Count][];
			ulong[] numArray2 = new ulong[this.g_dictStringPatterns.Count];
			KeyValuePair<string, string> keyValuePair;
			for (int index1 = 0; index1 < this.g_dictStringPatterns.Count; ++index1)
			{
				byte[][] numArray3 = numArray1;
				int index2 = index1;
				keyValuePair = this.g_dictStringPatterns.ElementAt<KeyValuePair<string, string>>(index1);
				byte[] patternString = this.ParsePatternString(keyValuePair.Value);
				numArray3[index2] = patternString;
			}
			for (int nOffset = 0; nOffset < this.g_arrModuleBuffer.Length; ++nOffset)
			{
				for (int index = 0; index < numArray1.Length; ++index)
				{
					if (numArray2[index] <= 0UL && this.PatternCheck(nOffset, numArray1[index]))
						numArray2[index] = this.g_lpModuleBase + (ulong)nOffset;
				}
			}
			Dictionary<string, ulong> dictionary1 = new Dictionary<string, ulong>();
			for (int index = 0; index < numArray1.Length; ++index)
			{
				Dictionary<string, ulong> dictionary2 = dictionary1;
				keyValuePair = this.g_dictStringPatterns.ElementAt<KeyValuePair<string, string>>(index);
				string key = keyValuePair.Key;
				long num = (long) numArray2[index];
				dictionary2[key] = (ulong)num;
			}
			lTime = stopwatch.ElapsedMilliseconds;
			return dictionary1;
		}

		private byte[] ParsePatternString(string szPattern)
		{
			List<byte> byteList = new List<byte>();
			string str1 = szPattern;
			char[] chArray = new char[1]{ ' ' };
			foreach (string str2 in str1.Split(chArray))
				byteList.Add(str2 == "?" ? (byte)0 : Convert.ToByte(str2, 16));
			return byteList.ToArray();
		}
	}
}

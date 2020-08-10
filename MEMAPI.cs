// Decompiled with JetBrains decompiler
// Type: Driver_Example.IO.MEMAPI
// Assembly: FREEMONEY, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 046FF60D-F509-497E-88D3-21C1139442E3
// Assembly location: C:\Users\USER\AppData\Local\Temp\Report.2B3E4829-B07D-45FE-A880-1C31A9ABADC0\poop.exe

using BKFN.IO;
using Driver.Enums;
using Driver.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Driver_Example.IO
{
	internal class MEMAPI
	{
		internal static Memory Memory;

		public static void LoadDriver()
		{
			FileInfo fileInfo = new FileInfo(Path.GetTempPath() + "Report.2B3E4829-B07D-45FE-A880-1C31A9ABADC0\\NewDriverTest4.sys");
			if (fileInfo.Exists)
			{
				Driver.Logic.Driver Driver = new Driver.Logic.Driver(new DriverConfig()
				{
					ServiceName = "FuckKernelAC10",
					SymbolicLink = "\\\\.\\FuckKernelAC10",
					DriverFile = fileInfo,
					LoadMethod = DriverLoad.Normal
				}, (string) null);
				Fortnite.Fortnite.Attach();
				Fortnite.Fortnite.EnableEvents();
				try
				{
					if (Driver.Load())
					{
						if (!Driver.IO.IsConnected)
							return;
						MEMAPI.Memory = new Memory(Driver);
						MEMAPI.Memory.SetProcId(Fortnite.Fortnite.AttachedProcess.Id);
					}
					else
					{
						int num = (int) MessageBox.Show("Failed To Load Driver!");
					}
				}
				catch
				{
					int num = (int) MessageBox.Show("Failed To Load, Is the game running?");
				}
			}
			else
			{
				int num1 = (int) MessageBox.Show("Driver Is Missing");
			}
		}

		public static bool IsValidPtr(ulong address)
		{
			return address != 0UL && address > 1048576UL && address < 36028797018963967UL;
		}

		public static byte[] ReadBytes(ulong address, ulong Length = 4, bool Add_Base = false)
		{
			if (!MEMAPI.IsValidPtr(address))
				return new byte[Length];
			byte[] numArray = new byte[Length];
			return MEMAPI.Memory.Read(address, Length, Add_Base);
		}

		public static byte[] ReadBytes(long address, int Length = 4, bool Add_Base = false)
		{
			if (!MEMAPI.IsValidPtr((ulong)address))
				return new byte[Length];
			byte[] numArray = new byte[Length];
			return MEMAPI.Memory.Read((ulong)address, (ulong)Length, Add_Base);
		}

		public static byte Readbyte(ulong address, ref bool ValidAddress, bool Add_Base = false)
		{
			if (MEMAPI.IsValidPtr(address))
			{
				byte[] numArray1 = new byte[2];
				byte[] numArray2 = MEMAPI.ReadBytes(address, 2UL, Add_Base);
				if (numArray2 != null)
				{
					ValidAddress = true;
					return numArray2[0];
				}
				ValidAddress = false;
				return 0;
			}
			ValidAddress = false;
			return 0;
		}

		public static short ReadInt16(ulong address, bool Add_Base = false)
		{
			if (!MEMAPI.IsValidPtr(address))
				return 0;
			byte[] numArray1 = new byte[2];
			byte[] numArray2 = MEMAPI.ReadBytes(address, 2UL, Add_Base);
			if (numArray2 != null)
				return BitConverter.ToInt16(numArray2, 0);
			return 0;
		}

		public static int ReadInt32(long address, bool Add_Base = false)
		{
			if (!MEMAPI.IsValidPtr((ulong)address))
				return 0;
			byte[] numArray1 = new byte[4];
			byte[] numArray2 = MEMAPI.ReadBytes((ulong) address, 4UL, Add_Base);
			if (numArray2 != null)
				return BitConverter.ToInt32(numArray2, 0);
			return 0;
		}

		public static int ReadInt32(ulong address, bool Add_Base = false)
		{
			if (!MEMAPI.IsValidPtr(address))
				return 0;
			byte[] numArray1 = new byte[4];
			byte[] numArray2 = MEMAPI.ReadBytes(address, 4UL, Add_Base);
			if (numArray2 != null)
				return BitConverter.ToInt32(numArray2, 0);
			return 0;
		}

		public static long ReadInt64(long address, bool Add_Base = false)
		{
			if (!MEMAPI.IsValidPtr((ulong)address))
				return 0;
			byte[] numArray1 = new byte[8];
			byte[] numArray2 = MEMAPI.ReadBytes((ulong) address, 8UL, Add_Base);
			if (numArray2 != null)
				return BitConverter.ToInt64(numArray2, 0);
			return 0;
		}

		public static long ReadInt64(ulong address, bool Add_Base = false)
		{
			if (!MEMAPI.IsValidPtr(address))
				return 0;
			byte[] numArray1 = new byte[8];
			byte[] numArray2 = MEMAPI.ReadBytes(address, 8UL, Add_Base);
			if (numArray2 != null)
				return BitConverter.ToInt64(numArray2, 0);
			return 0;
		}

		public static float ReadFloat(ulong address, bool Add_Base = false)
		{
			if (!MEMAPI.IsValidPtr(address))
				return 0.0f;
			byte[] numArray1 = new byte[4];
			byte[] numArray2 = MEMAPI.ReadBytes(address, 4UL, Add_Base);
			if (numArray2 != null)
				return BitConverter.ToSingle(numArray2, 0);
			return 0.0f;
		}

		public static float ReadFloat(long address, bool Add_Base = false)
		{
			if (!MEMAPI.IsValidPtr((ulong)address))
				return 0.0f;
			byte[] numArray1 = new byte[4];
			byte[] numArray2 = MEMAPI.ReadBytes(address, 4, Add_Base);
			if (numArray2 != null)
				return BitConverter.ToSingle(numArray2, 0);
			return 0.0f;
		}

		public static double ReadDouble(ulong address, bool Add_Base = false)
		{
			if (!MEMAPI.IsValidPtr(address))
				return 0.0;
			byte[] numArray = new byte[4];
			return (double)BitConverter.ToSingle(MEMAPI.ReadBytes(address, 4UL, Add_Base), 0);
		}

		public static ulong GetPointer(bool add_Base = false, params long[] args)
		{
			if (!MEMAPI.IsValidPtr((ulong)args[0]))
				return 0;
			long num = 0;
			if (args.Length == 0)
				return 0;
			for (int index = 0; index <= ((IEnumerable<long>)args).Count<long>() - 1; ++index)
			{
				if (index == 0)
					num = MEMAPI.ReadInt64((ulong)args[index], add_Base);
				else if (index != ((IEnumerable<long>)args).Count<long>() - 1)
				{
					if (num == 0L)
						return 0;
					num = MEMAPI.ReadInt64((ulong)(num + args[index]), false);
				}
				else
					num += args[index];
			}
			return (ulong)num;
		}

		public static string ReadString(ulong address, ulong Length = 124, bool Unicode = false)
		{
			if (Length > 0UL)
			{
				if (!Unicode)
				{
					ASCIIEncoding asciiEncoding = new ASCIIEncoding();
					byte[] numArray = new byte[Length];
					byte[] array = MEMAPI.ReadBytes(address, Length, false);
					if (array == null)
						return "";
					if (Length > 125UL)
						Array.Resize<byte>(ref array, (int)Length);
					for (int newSize = 0; newSize <= array.Length - 1; ++newSize)
					{
						if (array[newSize] == (byte)0)
						{
							Array.Resize<byte>(ref array, newSize);
							return asciiEncoding.GetString(array);
						}
					}
					return asciiEncoding.GetString(array);
				}
				if (Unicode)
				{
					UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
					byte[] numArray = new byte[Length];
					byte[] bytes = MEMAPI.ReadBytes(address, Length * 2UL, false);
					if (bytes != null)
						return unicodeEncoding.GetString(bytes);
					return "";
				}
			}
			return "";
		}

		public static void WriteBytes(ulong address, byte[] buffer, bool Add_Base = false)
		{
			for (ulong index = 0; index <= (ulong)buffer.Length - 1UL; ++index)
			{
				if (index == 0UL)
					MEMAPI.Memory.Write<byte>(address, buffer[index], Add_Base);
				else
					MEMAPI.Memory.Write<byte>(address + index, buffer[index], false);
			}
		}

		public static void WriteBytes(long address, byte[] buffer, ulong Length = 4, bool Add_Base = false)
		{
			for (ulong index = 0; index <= Length - 1UL; ++index)
			{
				if (index == 0UL)
					MEMAPI.Memory.Write<byte>((ulong)address, buffer[index], Add_Base);
				else
					MEMAPI.Memory.Write<byte>((ulong)address + index, buffer[index], false);
			}
		}

		public static void WriteInt16(ulong address, short Value, bool Add_Base = false)
		{
			byte[] numArray = new byte[2];
			byte[] bytes = BitConverter.GetBytes(Value);
			MEMAPI.WriteBytes(address, bytes, Add_Base);
		}

		public static void WriteInt32(ulong address, int Value, bool Add_Base = false)
		{
			byte[] numArray = new byte[4];
			byte[] bytes = BitConverter.GetBytes(Value);
			MEMAPI.WriteBytes(address, bytes, Add_Base);
		}

		public static void WriteInt32(long address, int Value, bool Add_Base = false)
		{
			byte[] numArray = new byte[4];
			byte[] bytes = BitConverter.GetBytes(Value);
			MEMAPI.WriteBytes(address, bytes, 4UL, Add_Base);
		}

		public static void WriteInt64(ulong address, long Value, bool Add_Base = false)
		{
			byte[] numArray = new byte[8];
			byte[] bytes = BitConverter.GetBytes(Value);
			MEMAPI.WriteBytes(address, bytes, Add_Base);
		}

		public static void WriteInt64(long address, long Value, bool Add_Base = false)
		{
			byte[] numArray = new byte[8];
			byte[] bytes = BitConverter.GetBytes(Value);
			MEMAPI.WriteBytes(address, bytes, 8UL, Add_Base);
		}

		public static void WriteFloat(ulong address, float Value, bool Add_Base = false)
		{
			byte[] numArray = new byte[4];
			byte[] bytes = BitConverter.GetBytes(Value);
			MEMAPI.WriteBytes(address, bytes, Add_Base);
		}

		public static void WriteFloat(long address, float Value, bool Add_Base = false)
		{
			byte[] numArray = new byte[4];
			byte[] bytes = BitConverter.GetBytes(Value);
			MEMAPI.WriteBytes(address, bytes, 4UL, Add_Base);
		}

		public static void WriteDouble(ulong address, double Value, bool Add_Base = false)
		{
			byte[] numArray = new byte[4];
			byte[] bytes = BitConverter.GetBytes(Value);
			MEMAPI.WriteBytes(address, bytes, Add_Base);
		}

		public static MEMAPI.Vector2 ReadVector2(ulong Address)
		{
			return new MEMAPI.Vector2(MEMAPI.ReadFloat(Address, false), MEMAPI.ReadFloat(Address + 4UL, false));
		}

		public static void WriteVector2(ulong Address, MEMAPI.Vector2 value)
		{
			MEMAPI.WriteFloat(Address, value.x, false);
			MEMAPI.WriteFloat(Address + 4UL, value.y, false);
		}

		public static MEMAPI.Vector2 ToVec2(float X, float Y, float Z)
		{
			return new MEMAPI.Vector2() { x = X, y = Y };
		}

		public static MEMAPI.Vector3 ReadVector3(ulong Address, bool Add_Base)
		{
			if (!MEMAPI.IsValidPtr(Address))
				return new MEMAPI.Vector3(0.0f, 0.0f, 0.0f);
			byte[] numArray1 = new byte[12];
			byte[] numArray2 = MEMAPI.ReadBytes(Address, 12UL, Add_Base);
			if (numArray2 != null)
				return new MEMAPI.Vector3(BitConverter.ToSingle(numArray2, 0), BitConverter.ToSingle(numArray2, 4), BitConverter.ToSingle(numArray2, 8));
			return new MEMAPI.Vector3(0.0f, 0.0f, 0.0f);
		}

		public static void WriteVector3(ulong Address, MEMAPI.Vector3 value)
		{
			MEMAPI.WriteFloat(Address, value.x, false);
			MEMAPI.WriteFloat(Address + 4UL, value.y, false);
			MEMAPI.WriteFloat(Address + 8UL, value.z, false);
		}

		public static MEMAPI.Vector3 ToVec3(float X, float Y, float Z)
		{
			return new MEMAPI.Vector3() { x = X, y = Y, z = Z };
		}

		public static MEMAPI.Matrix ReadMatrix(ulong address, bool Add_Base)
		{
			byte[] numArray1 = new byte[64];
			byte[] numArray2 = MEMAPI.ReadBytes(address, 64UL, Add_Base);
			return new MEMAPI.Matrix(BitConverter.ToSingle(numArray2, 0), BitConverter.ToSingle(numArray2, 4), BitConverter.ToSingle(numArray2, 8), BitConverter.ToSingle(numArray2, 12), BitConverter.ToSingle(numArray2, 16), BitConverter.ToSingle(numArray2, 20), BitConverter.ToSingle(numArray2, 24), BitConverter.ToSingle(numArray2, 28), BitConverter.ToSingle(numArray2, 32), BitConverter.ToSingle(numArray2, 36), BitConverter.ToSingle(numArray2, 40), BitConverter.ToSingle(numArray2, 44), BitConverter.ToSingle(numArray2, 48), BitConverter.ToSingle(numArray2, 52), BitConverter.ToSingle(numArray2, 56), BitConverter.ToSingle(numArray2, 60));
		}

		public static MEMAPI.DLLParams[] GetDLLInfo(string name)
		{
			MEMAPI.DLLParams[] dllParamsArray = new MEMAPI.DLLParams[1];
			for (int index = 0; index <= Fortnite.Fortnite.Modules.Count - 1; ++index)
			{
				if (Fortnite.Fortnite.Modules[index].ModuleName == name)
				{
					dllParamsArray[0].Name = Fortnite.Fortnite.Modules[index].ModuleName;
					dllParamsArray[0].Path = Fortnite.Fortnite.Modules[index].FileName;
					dllParamsArray[0].Start_Address = (long)Fortnite.Fortnite.Modules[index].BaseAddress;
					dllParamsArray[0].Stop_Address = (long)Fortnite.Fortnite.Modules[index].BaseAddress + (long)Fortnite.Fortnite.Modules[index].ModuleMemorySize;
					return dllParamsArray;
				}
			}
			return (MEMAPI.DLLParams[])null;
		}

		public static long[] Find_AddressesMask(
		  long startAddress,
		  long endAddress,
		  byte[] pattern,
		  string Wildcards,
		  long StopIndex = 9223372036854775807,
		  int ScanAlignment = 1,
		  int Block_Size = 65536)
		{
			if (startAddress == 0L)
			{
				int num1 = (int) MessageBox.Show("Memeory can not be Zero!");
			}
			long length = (long) Block_Size;
			long num2 = 0;
			long num3 = 0;
			long[] numArray1 = (long[]) null;
			int index1 = 0;
			byte[] numArray2 = new byte[Wildcards.Length + 1];
			for (int index2 = 0; index2 <= Wildcards.Length - 1; ++index2)
			{
				char wildcard = Wildcards[index2];
				numArray2[index2] = !(wildcard.ToString() == "?") ? byte.MaxValue : (byte)0;
			}
			if (endAddress - startAddress > length)
			{
				for (int index2 = 0; index2 <= 999999999; ++index2)
				{
					if (startAddress + length * (long)(index2 + 1) > endAddress)
						num3 = endAddress - startAddress - length * num2;
					else
						++num2;
				}
				if (num2 <= 5L)
					;
				for (int index2 = 0; (long)index2 <= num2 - 1L; ++index2)
				{
					try
					{
						if (index2 <= 0)
							;
						byte[] numArray3 = new byte[length];
						byte[] numArray4 = MEMAPI.ReadBytes((ulong) (startAddress + length * (long) index2), (ulong) numArray3.Length, false);
						if (numArray4 != null)
						{
							for (long index3 = 0; index3 <= (long)(numArray4.Length - 1); index3 = index3 + (long)(ScanAlignment - 1) + 1L)
							{
								for (long index4 = 0; index4 <= (long)(pattern.Length - 1) && index3 < (long)(numArray4.Length - pattern.Length); ++index4)
								{
									(startAddress + length * (long)index2 + index3).ToString("X");
									if ((int)numArray4[index3 + index4] == (int)pattern[index4] | numArray2[index4] == (byte)0)
									{
										if (index4 != 4L)
											;
										if (index4 == (long)(pattern.Length - 1))
										{
											++index1;
											long[] numArray5 = numArray1;
											numArray1 = new long[index1 + 1];
											if (numArray5 != null)
												Array.Copy((Array)numArray5, (Array)numArray1, Math.Min(index1 + 1, numArray5.Length));
											numArray1[index1] = startAddress + length * (long)index2 + index3;
											if ((long)index1 == StopIndex + 1L)
											{
												numArray1[0] = (long)index1;
												return numArray1;
											}
										}
									}
									else
										break;
								}
							}
						}
					}
					catch
					{
					}
				}
				if (index1 == 0)
				{
					long[] numArray3 = numArray1;
					long[] numArray4 = new long[StopIndex + 1L];
					if (numArray3 != null)
						Array.Copy((Array)numArray3, (Array)numArray4, Math.Min(StopIndex + 1L, (long)numArray3.Length));
					for (int index2 = 0; (long)index2 <= StopIndex; ++index2)
						numArray4[index2] = 0L;
					return new long[2];
				}
				if (index1 <= 0)
					return (long[])null;
				long[] numArray6 = numArray1;
				long[] numArray7 = new long[StopIndex + 1L];
				if (numArray6 != null)
					Array.Copy((Array)numArray6, (Array)numArray7, Math.Min(StopIndex + 1L, (long)numArray6.Length));
				for (int index2 = 0; (long)index2 <= StopIndex; ++index2)
					numArray7[index2] = 0L;
				numArray7[0] = (long)index1;
				return numArray7;
			}
			byte[] numArray8 = new byte[endAddress - startAddress];
			byte[] numArray9 = MEMAPI.ReadBytes((ulong) startAddress, (ulong) numArray8.Length, false);
			for (long index2 = 0; index2 <= (long)(numArray9.Length - 1); index2 = index2 + (long)(ScanAlignment - 1) + 1L)
			{
				for (long index3 = 0; index3 <= (long)(pattern.Length - 1) && (index2 < (long)(numArray9.Length - pattern.Length) && (int)numArray9[index2 + index3] == (int)pattern[index3] | numArray2[index3] == (byte)0); ++index3)
				{
					if (index3 == (long)(pattern.Length - 1))
					{
						++index1;
						long[] numArray3 = numArray1;
						numArray1 = new long[index1 + 1];
						if (numArray3 != null)
							Array.Copy((Array)numArray3, (Array)numArray1, Math.Min(index1 + 1, numArray3.Length));
						numArray1[index1] = startAddress + index2;
						if ((long)index1 == StopIndex + 1L)
						{
							numArray1[0] = (long)index1;
							return numArray1;
						}
					}
				}
			}
			if (index1 == 0)
			{
				long[] numArray3 = numArray1;
				numArray1 = new long[StopIndex + 1L];
				if (numArray3 != null)
					Array.Copy((Array)numArray3, (Array)numArray1, Math.Min(StopIndex + 1L, (long)numArray3.Length));
				for (int index2 = 0; (long)index2 <= StopIndex; ++index2)
					numArray1[index2] = 0L;
			}
			numArray1[0] = (long)index1;
			return numArray1;
		}

		public struct Vector2
		{
			public float x;
			public float y;

			public Vector2(float x, float y)
			{
				this.x = x;
				this.y = y;
			}
		}

		public struct Vector3
		{
			public float x;
			public float y;
			public float z;

			public Vector3(float x, float y, float z)
			{
				this.x = x;
				this.y = y;
				this.z = z;
			}
		}

		public struct Matrix
		{
			public float M11;
			public float M12;
			public float M13;
			public float M14;
			public float M21;
			public float M22;
			public float M23;
			public float M24;
			public float M31;
			public float M32;
			public float M33;
			public float M34;
			public float M41;
			public float M42;
			public float M43;
			public float M44;

			public Matrix(
			  float M11,
			  float M12,
			  float M13,
			  float M14,
			  float M21,
			  float M22,
			  float M23,
			  float M24,
			  float M31,
			  float M32,
			  float M33,
			  float M34,
			  float M41,
			  float M42,
			  float M43,
			  float M44)
			{
				this.M11 = M11;
				this.M12 = M12;
				this.M13 = M13;
				this.M14 = M14;
				this.M21 = M21;
				this.M22 = M22;
				this.M23 = M23;
				this.M24 = M24;
				this.M31 = M31;
				this.M32 = M32;
				this.M33 = M33;
				this.M34 = M34;
				this.M41 = M41;
				this.M42 = M42;
				this.M43 = M43;
				this.M44 = M44;
			}
		}

		public struct DLLParams
		{
			public string Name;
			public long Start_Address;
			public long Stop_Address;
			public string Path;

			public DLLParams(string Name, long Start_Address, long Stop_Address, string Path)
			{
				this.Name = Name;
				this.Path = Path;
				this.Start_Address = Start_Address;
				this.Stop_Address = Stop_Address;
			}
		}
	}
}

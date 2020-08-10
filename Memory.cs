using BKFN.IO.Structures;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace BKFN.IO
{
	internal class Memory
	{
		private const uint InitCtlCode = 2235392;
		private const uint ReadCtlCode = 2235396;
		private const uint AllocMemCtlCode = 2235416;
		private const uint WritePageProtectionCtlCode = 2235412;
		private const uint WriteCtlCode = 2235400;
		private const uint VirtualQueryCtlCode = 2235404;
		private const uint GetBaseAddrCtlCode = 2235408;
		private const uint UnloadQueryCtlCode = 2236004;
		public const uint GetGameAssemblyDllCtlCode = 2235420;
		public const uint GetUnityPlayerDllCtlCode = 2235424;
		private DateTime _lastTime;
		private int _framesRendered;

		internal Driver.Logic.Driver Driver { get; private set; }

		internal int LastProcessId { get; private set; }

		protected Memory()
		{
		}

		internal Memory(Driver.Logic.Driver Driver)
		{
			this.SetDriver(Driver);
		}

		internal void SetDriver(Driver.Logic.Driver Driver)
		{
			if (Driver == null)
				throw new ArgumentNullException(nameof(Driver), "Driver is null");
			this.Driver = Driver;
		}

		internal void SetProcId(int ProcId)
		{
			this.LastProcessId = ProcId;
		}

		public byte[] Read(ulong Address, ulong Size, bool UseBaseAddress = false)
		{
			++this._framesRendered;
			if ((DateTime.Now - this._lastTime).TotalSeconds >= 1.0)
			{
				this._framesRendered = 0;
				this._lastTime = DateTime.Now;
			}
			if (!this.Driver.IO.IsConnected)
				throw new Exception("Driver is disconnected.");
			if (Size <= 0UL)
				throw new ArgumentException("Size is inferior or equal to zero at Read(ProcessId, Address, Size).", nameof(Size));
			if (Size > 2147483591UL)
				throw new ArgumentException("Size is superior to the limit at Read(ProcessId, Address, Size).", nameof(Size));
			if (Address == 0UL || Address >= 140737488289791UL)
				return (byte[])null;
			KernelReadRequest IoData = new KernelReadRequest();
			uint IoOutput = (uint) Marshal.SizeOf<KernelReadRequest>();
			byte[] numArray = new byte[Size];
			GCHandle gcHandle = GCHandle.Alloc((object) numArray, GCHandleType.Pinned);
			if (!gcHandle.IsAllocated)
				throw new InsufficientMemoryException("Couldn't allocate memory for the buffer, at Read(ProcessId, Address, Size).");
			IntPtr num = gcHandle.AddrOfPinnedObject();
			IoData.ProcessId = this.LastProcessId;
			IoData.Size = (UIntPtr)Size;
			IoData.Response = (UIntPtr)((ulong)num.ToInt64());
			IoData.Address = (UIntPtr)Address;
			IoData.UseBaseAddress = UseBaseAddress;
			bool flag = this.Driver.IO.TryIoControl<KernelReadRequest, uint>(2235396U, IoData, IoOutput);
			if (flag)
				numArray = (byte[])gcHandle.Target;
			gcHandle.Free();
			if (!flag)
				numArray = (byte[])null;
			return numArray;
		}

		public void SetExecuteReadWriteAccess(ulong Address, ulong Size = 0, bool UseBaseAddress = false)
		{
			if (!this.Driver.IO.IsConnected)
				throw new Exception("Driver is disconnected.");
			if (Size <= 0UL)
				throw new ArgumentException("Size is inferior or equal to zero at Read(ProcessId, Address, Size).", nameof(Size));
			if (Size > 2147483591UL)
				throw new ArgumentException("Size is superior to the limit at Read(ProcessId, Address, Size).", nameof(Size));
			if (Address == 0UL || Address >= 140737488289791UL)
				return;
			bool flag = false;
			KernelReadRequest IoData = new KernelReadRequest();
			uint IoOutput = (uint) Marshal.SizeOf<KernelReadRequest>();
			IoData.ProcessId = this.LastProcessId;
			IoData.Size = (UIntPtr)Size;
			IoData.Response = (UIntPtr)64UL;
			IoData.Address = (UIntPtr)Address;
			IoData.UseBaseAddress = UseBaseAddress;
			flag = this.Driver.IO.TryIoControl<KernelReadRequest, uint>(2235412U, IoData, IoOutput);
			Thread.Sleep(100);
		}

		public long AllocateProcessMemory(UIntPtr Size)
		{
			if (!this.Driver.IO.IsConnected)
				throw new Exception("Driver is disconnected.");
			bool flag = false;
			KernelReadRequest IoData = new KernelReadRequest();
			uint IoOutput = (uint) Marshal.SizeOf<KernelReadRequest>();
			GCHandle gcHandle = GCHandle.Alloc((object) new byte[8], GCHandleType.Pinned);
			IntPtr num = gcHandle.AddrOfPinnedObject();
			IoData.ProcessId = this.LastProcessId;
			IoData.Size = Size;
			IoData.Response = (UIntPtr)((ulong)num.ToInt64());
			flag = this.Driver.IO.TryIoControl<KernelReadRequest, uint>(2235416U, IoData, IoOutput);
			return BitConverter.ToInt64((byte[])gcHandle.Target, 0);
		}

		public long GetGameAssemblyDllBase()
		{
			if (!this.Driver.IO.IsConnected)
				throw new Exception("Driver is disconnected.");
			bool flag = false;
			KernelReadRequest IoData = new KernelReadRequest();
			uint IoOutput = (uint) Marshal.SizeOf<KernelReadRequest>();
			GCHandle gcHandle = GCHandle.Alloc((object) new byte[8], GCHandleType.Pinned);
			IntPtr num = gcHandle.AddrOfPinnedObject();
			IoData.ProcessId = this.LastProcessId;
			IoData.Size = (UIntPtr)5UL;
			IoData.Response = (UIntPtr)((ulong)num.ToInt64());
			flag = this.Driver.IO.TryIoControl<KernelReadRequest, uint>(2235420U, IoData, IoOutput);
			return BitConverter.ToInt64((byte[])gcHandle.Target, 0);
		}

		public long GetUnityPlayerDllBase()
		{
			if (!this.Driver.IO.IsConnected)
				throw new Exception("Driver is disconnected.");
			bool flag = false;
			KernelReadRequest IoData = new KernelReadRequest();
			uint IoOutput = (uint) Marshal.SizeOf<KernelReadRequest>();
			GCHandle gcHandle = GCHandle.Alloc((object) new byte[8], GCHandleType.Pinned);
			IntPtr num = gcHandle.AddrOfPinnedObject();
			IoData.ProcessId = this.LastProcessId;
			IoData.Size = (UIntPtr)5UL;
			IoData.Response = (UIntPtr)((ulong)num.ToInt64());
			flag = this.Driver.IO.TryIoControl<KernelReadRequest, uint>(2235424U, IoData, IoOutput);
			return BitConverter.ToInt64((byte[])gcHandle.Target, 0);
		}

		public void SetExecuteReadAccess(ulong Address, ulong Size = 0, bool UseBaseAddress = false)
		{
			++this._framesRendered;
			if ((DateTime.Now - this._lastTime).TotalSeconds >= 1.0)
			{
				this._framesRendered = 0;
				this._lastTime = DateTime.Now;
			}
			if (!this.Driver.IO.IsConnected)
				throw new Exception("Driver is disconnected.");
			if (Size <= 0UL)
				throw new ArgumentException("Size is inferior or equal to zero at Read(ProcessId, Address, Size).", nameof(Size));
			if (Size > 2147483591UL)
				throw new ArgumentException("Size is superior to the limit at Read(ProcessId, Address, Size).", nameof(Size));
			if (Address == 0UL || Address >= 140737488289791UL)
				return;
			bool flag = false;
			KernelReadRequest IoData = new KernelReadRequest();
			uint IoOutput = (uint) Marshal.SizeOf<KernelReadRequest>();
			IoData.ProcessId = this.LastProcessId;
			IoData.Size = (UIntPtr)Size;
			IoData.Response = (UIntPtr)32UL;
			IoData.Address = (UIntPtr)Address;
			IoData.UseBaseAddress = UseBaseAddress;
			flag = this.Driver.IO.TryIoControl<KernelReadRequest, uint>(2235412U, IoData, IoOutput);
		}

		public T Read<T>(ulong Address, bool UseBaseAddress = false)
		{
			if (!this.Driver.IO.IsConnected)
				throw new Exception("Driver is disconnected.");
			if (Address == 0UL || Address >= 140737488289791UL)
				return default(T);
			int cb = Marshal.SizeOf<T>();
			if (Address + (ulong)cb >= 140737488289791UL)
				return default(T);
			KernelReadRequest IoData = new KernelReadRequest();
			uint IoOutput = (uint) Marshal.SizeOf<KernelReadRequest>();
			T obj = default (T);
			IntPtr num = Marshal.AllocHGlobal(cb);
			if (num == IntPtr.Zero)
				throw new InsufficientMemoryException("Couldn't allocate memory for the buffer, at Read<T>(Address, UseBaseAddress).");
			IoData.ProcessId = this.LastProcessId;
			IoData.Size = (UIntPtr)((ulong)cb);
			IoData.Response = (UIntPtr)((ulong)num.ToInt64());
			IoData.Address = (UIntPtr)Address;
			IoData.UseBaseAddress = UseBaseAddress;
			bool flag = this.Driver.IO.TryIoControl<KernelReadRequest, uint>(2235396U, IoData, IoOutput);
			if (flag)
				obj = Marshal.PtrToStructure<T>(num);
			Marshal.FreeHGlobal(num);
			if (!flag)
				obj = default(T);
			return obj;
		}

		internal void Write(ulong Address, byte[] Value, bool UseBaseAddress = false)
		{
			if (!this.Driver.IO.IsConnected)
				throw new Exception("Driver is disconnected.");
			if (Address == 0UL || Address >= 140737488289791UL)
				return;
			int length = Value.Length;
			if (Address + (ulong)length >= 140737488289791UL)
				return;
			KernelWriteRequest IoData = new KernelWriteRequest();
			uint IoOutput = (uint) Marshal.SizeOf<KernelWriteRequest>();
			GCHandle gcHandle = GCHandle.Alloc((object) Value, GCHandleType.Pinned);
			if (!gcHandle.IsAllocated)
				throw new InsufficientMemoryException("Couldn't allocate memory for the buffer, at Write<T>(Address, Value, UseBaseAddress).");
			IoData.ProcessId = this.LastProcessId;
			IoData.Size = (UIntPtr)((ulong)length);
			IoData.Value = (UIntPtr)((ulong)gcHandle.AddrOfPinnedObject().ToInt64());
			IoData.Address = (UIntPtr)Address;
			IoData.UseBaseAddress = UseBaseAddress;
			bool flag = this.Driver.IO.TryIoControl<KernelWriteRequest, uint>(2235400U, IoData, IoOutput);
			gcHandle.Free();
			if (!flag)
				throw new Exception("Failed to write the given structure to the specified address, at Write<T>(Address, Value, UseBaseAddress).");
		}

		internal void Write<T>(ulong Address, T Value, bool UseBaseAddress = false)
		{
			if (!this.Driver.IO.IsConnected)
				throw new Exception("Driver is disconnected.");
			if (Address == 0UL || Address >= 140737488289791UL)
				return;
			int num = Marshal.SizeOf<T>();
			if (Address + (ulong)num >= 140737488289791UL)
				return;
			KernelWriteRequest IoData = new KernelWriteRequest();
			uint IoOutput = (uint) Marshal.SizeOf<KernelWriteRequest>();
			GCHandle gcHandle = GCHandle.Alloc((object) Value, GCHandleType.Pinned);
			if (!gcHandle.IsAllocated)
				throw new InsufficientMemoryException("Couldn't allocate memory for the buffer, at Write<T>(Address, Value, UseBaseAddress).");
			IoData.ProcessId = this.LastProcessId;
			IoData.Size = (UIntPtr)((ulong)num);
			IoData.Value = (UIntPtr)((ulong)gcHandle.AddrOfPinnedObject().ToInt64());
			IoData.Address = (UIntPtr)Address;
			IoData.UseBaseAddress = UseBaseAddress;
			bool flag = this.Driver.IO.TryIoControl<KernelWriteRequest, uint>(2235400U, IoData, IoOutput);
			gcHandle.Free();
			if (flag)
				;
		}
	}
}

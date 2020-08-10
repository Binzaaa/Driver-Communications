using System;

namespace BKFN.IO.Structures
{
	internal struct KernelReadRequest
	{
		internal int ProcessId;
		internal UIntPtr Address;
		internal UIntPtr Response;
		internal UIntPtr Size;
		internal bool UseBaseAddress;
	}
}

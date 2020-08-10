using System;

namespace BKFN.IO.Structures
{
	internal struct KernelWriteRequest
	{
		internal int ProcessId;
		internal UIntPtr Address;
		internal UIntPtr Value;
		internal UIntPtr Size;
		internal bool UseBaseAddress;
	}
}

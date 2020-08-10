// Decompiled with JetBrains decompiler
// Type: Driver.Example.Handlers.Structures.KernelVirtualQueryRequest
// Assembly: FREEMONEY, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 046FF60D-F509-497E-88D3-21C1139442E3
// Assembly location: C:\Users\USER\AppData\Local\Temp\Report.2B3E4829-B07D-45FE-A880-1C31A9ABADC0\poop.exe

namespace Driver.Example.Handlers.Structures
{
	internal struct KernelVirtualQueryRequest
	{
		internal int ProcessId;
		internal ulong BaseAddress;
		internal bool HasBeenFound;
	}
}

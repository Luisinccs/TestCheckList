// 2025-12-22
using System;


[Flags]
///<summary>Banderas para modificadores de teclado (Shift, Ctrl, Alt, Command)</summary>
public enum UniversalModifier {
	None = 0,
	Shift = 1 << 0,
	Control = 1 << 1,
	Alt = 1 << 2,
	Command = 1 << 3
}
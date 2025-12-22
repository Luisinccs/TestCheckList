

#if WINDOWS

using Windows.System;

/// <summary>Extensiones para la conversion de teclas de Windows a la enumeracion universal</summary>
public static class VirtualKeyExtensions {

	///<summary>Mapea una VirtualKey de Windows al catalogo UniversalKey</summary>
	public static UniversalKey ToUniversalKey(this VirtualKey virtualKey) => virtualKey switch {
		// Letras
		VirtualKey.A => UniversalKey.A,
		VirtualKey.B => UniversalKey.B,
		VirtualKey.C => UniversalKey.C,
		VirtualKey.D => UniversalKey.D,
		VirtualKey.E => UniversalKey.E,
		VirtualKey.F => UniversalKey.F,
		VirtualKey.G => UniversalKey.G,
		VirtualKey.H => UniversalKey.H,
		VirtualKey.I => UniversalKey.I,
		VirtualKey.J => UniversalKey.J,
		VirtualKey.K => UniversalKey.K,
		VirtualKey.L => UniversalKey.L,
		VirtualKey.M => UniversalKey.M,
		VirtualKey.N => UniversalKey.N,
		VirtualKey.O => UniversalKey.O,
		VirtualKey.P => UniversalKey.P,
		VirtualKey.Q => UniversalKey.Q,
		VirtualKey.R => UniversalKey.R,
		VirtualKey.S => UniversalKey.S,
		VirtualKey.T => UniversalKey.T,
		VirtualKey.U => UniversalKey.U,
		VirtualKey.V => UniversalKey.V,
		VirtualKey.W => UniversalKey.W,
		VirtualKey.X => UniversalKey.X,
		VirtualKey.Y => UniversalKey.Y,
		VirtualKey.Z => UniversalKey.Z,

		// Numeros (Fila superior)
		VirtualKey.Number0 => UniversalKey.D0,
		VirtualKey.Number1 => UniversalKey.D1,
		VirtualKey.Number2 => UniversalKey.D2,
		VirtualKey.Number3 => UniversalKey.D3,
		VirtualKey.Number4 => UniversalKey.D4,
		VirtualKey.Number5 => UniversalKey.D5,
		VirtualKey.Number6 => UniversalKey.D6,
		VirtualKey.Number7 => UniversalKey.D7,
		VirtualKey.Number8 => UniversalKey.D8,
		VirtualKey.Number9 => UniversalKey.D9,

		// Navegacion y Control
		VirtualKey.Up => UniversalKey.ArrowUp,
		VirtualKey.Down => UniversalKey.ArrowDown,
		VirtualKey.Left => UniversalKey.ArrowLeft,
		VirtualKey.Right => UniversalKey.ArrowRight,
		VirtualKey.Enter => UniversalKey.Enter,
		VirtualKey.Escape => UniversalKey.Escape,
		VirtualKey.Space => UniversalKey.Space,
		VirtualKey.Tab => UniversalKey.Tab,
		VirtualKey.Back => UniversalKey.Backspace,

		// Teclas de Funcion
		VirtualKey.F1 => UniversalKey.F1,
		VirtualKey.F2 => UniversalKey.F2,
		VirtualKey.F3 => UniversalKey.F3,
		VirtualKey.F4 => UniversalKey.F4,
		VirtualKey.F5 => UniversalKey.F5,
		VirtualKey.F6 => UniversalKey.F6,
		VirtualKey.F7 => UniversalKey.F7,
		VirtualKey.F8 => UniversalKey.F8,
		VirtualKey.F9 => UniversalKey.F9,
		VirtualKey.F10 => UniversalKey.F10,
		VirtualKey.F11 => UniversalKey.F11,
		VirtualKey.F12 => UniversalKey.F12,

		_ => UniversalKey.None
	};

}

#endif
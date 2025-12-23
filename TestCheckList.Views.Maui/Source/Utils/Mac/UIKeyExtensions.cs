// 2025-12-22
#if MACCATALYST
using UIKit;

///<summary>Extensiones para mapear teclas de UIKit al catalogo UniversalKey</summary>
public static class UIKeyExtensions {

	#region Funciones Externas
    ///<summary>Mapea un objeto UIKey de Mac a UniversalKey usando codigos HID</summary>
    public static UniversalKey ToUniversalKey(this UIKey key) => (long)key.KeyCode switch {
        // Letras (HID 4-29)
        4 => UniversalKey.A,
        5 => UniversalKey.B,
        6 => UniversalKey.C,
        7 => UniversalKey.D,
        8 => UniversalKey.E,
        9 => UniversalKey.F,
        10 => UniversalKey.G,
        11 => UniversalKey.H,
        12 => UniversalKey.I,
        13 => UniversalKey.J,
        14 => UniversalKey.K,
        15 => UniversalKey.L,
        16 => UniversalKey.M,
        17 => UniversalKey.N,
        18 => UniversalKey.O,
        19 => UniversalKey.P,
        20 => UniversalKey.Q,
        21 => UniversalKey.R,
        22 => UniversalKey.S,
        23 => UniversalKey.T,
        24 => UniversalKey.U,
        25 => UniversalKey.V,
        26 => UniversalKey.W,
        27 => UniversalKey.X,
        28 => UniversalKey.Y,
        29 => UniversalKey.Z,

        // Numeros (Fila superior HID 30-39)
        30 => UniversalKey.D1,
        31 => UniversalKey.D2,
        32 => UniversalKey.D3,
        33 => UniversalKey.D4,
        34 => UniversalKey.D5,
        35 => UniversalKey.D6,
        36 => UniversalKey.D7,
        37 => UniversalKey.D8,
        38 => UniversalKey.D9,
        39 => UniversalKey.D0,

        // Navegacion y Control
        40 => UniversalKey.Enter,
        41 => UniversalKey.Escape,
        42 => UniversalKey.Backspace,
        43 => UniversalKey.Tab,
        44 => UniversalKey.Space,
        79 => UniversalKey.ArrowRight,
        80 => UniversalKey.ArrowLeft,
        81 => UniversalKey.ArrowDown,
        82 => UniversalKey.ArrowUp,

        // Teclas de Funcion (HID 58-69)
        58 => UniversalKey.F1,
        59 => UniversalKey.F2,
        60 => UniversalKey.F3,
        61 => UniversalKey.F4,
        62 => UniversalKey.F5,
        63 => UniversalKey.F6,
        64 => UniversalKey.F7,
        65 => UniversalKey.F8,
        66 => UniversalKey.F9,
        67 => UniversalKey.F10,
        68 => UniversalKey.F11,
        69 => UniversalKey.F12,

        _ => UniversalKey.None
    };
	#endregion

}
#endif
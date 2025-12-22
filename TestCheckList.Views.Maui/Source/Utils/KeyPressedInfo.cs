// 2025-12-22


///<summary>Record struct que transporta la informacion de la tecla de forma unificada</summary>
///<param name="Characters">Texto resultante</param>
///<param name="Key">Tecla universal</param>
///<param name="Modifiers">Modificadores activos</param>
public record struct KeyPressedInfo(string Characters, UniversalKey Key, UniversalModifier Modifiers);

//Vieja porsia
//public record struct KeyPressedInfo(string Characters, long KeyCode, long ModifierFlags );
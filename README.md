# TestCheckList
🚀 Aplicación ultra-ligera en .NET MAUI para la ejecución y validación de pruebas manuales de UI/UX. Diseñada para integrarse en flujos de trabajo con IA (Google Antigravity) mediante archivos .scheck.

# TestCheckList 🏁

**TestCheckList** es una herramienta de productividad desarrollada en .NET MAUI diseñada para la verificación manual de tareas técnicas y funcionales con un enfoque en la velocidad de ejecución mediante teclado.

Esta aplicación actúa como el eslabón final en ciclos de desarrollo asistidos por IA, permitiendo validar criterios de aceptación generados automáticamente.

## ✨ Características Principales

- **Navegación Speed-Run:** Control total mediante teclado (Flechas ↑/↓) y atajos rápidos (S/F/E/D) para una clasificación instantánea de tareas.
- **Local-First & Portable:** Persistencia basada en archivos de texto plano con extensión `.scheck` utilizando un sistema de marcadores (@task, @state, @comment).
- **Asociación de Archivos:** Integración nativa con el sistema operativo para abrir archivos directamente desde el explorador/finder.
- **Guardado Automático:** Sincronización en tiempo real con el archivo de origen al cambiar estados o editar comentarios.
- **Diseño de Alta Visibilidad:** Interfaz optimizada con colores alternos y resaltado de enfoque para reducir la fatiga visual durante sesiones de prueba largas.

## 🛠️ Arquitectura Técnica

Sigue principios de desarrollo robustos para garantizar rendimiento y mantenibilidad:

- **Patrón:** MVVM con Inyección de Dependencias (DI) y principios SOLID.
- **Vistas Brutas:** Implementación inicial en XAML/C# para soporte de Hot Reload, estructurada para una migración transparente a C# puro para optimización máxima de performance.
- **Desacoplamiento:** El Core (Modelos, ViewModels y Parsers) está totalmente separado de la capa de UI.
- **Arranque Minimalista:** Configuración directa en `MauiProgram.cs` sin dependencia de `AppShell` o `App.xaml`.

## ⌨️ Atajos de Teclado

| Tecla | Acción |
| :--- | :--- |
| **S** | Marcar como `Superado` y saltar al siguiente. |
| **F** | Marcar como `Fallido` e iniciar edición de comentario. |
| **E** | Marcar como `Superado` con comentario opcional. |
| **D** | Reiniciar a `Pendiente`. |
| **Enter** | Guardar comentario y avanzar. |
| **Esc** | Cancelar edición de comentario. |

## 🚀 Flujo de Trabajo con IA (Antigravity)

1. **Generación:** La IA genera código y un archivo `.scheck` con los tests.
2. **Validación:** El desarrollador abre el archivo en **TestCheckList** y ejecuta las pruebas.
3. **Feedback:** Los fallos documentados se devuelven a la IA para correcciones precisas basándose en el reporte generado.

---
Desarrollado con .NET MAUI 2025.

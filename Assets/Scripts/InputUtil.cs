using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace RiosVivos
{
    /// <summary>
    /// Lee el teclado/mouse sin importar si el proyecto usa el Input System nuevo o el antiguo.
    /// W/S = remar, A/D = girar, clic izq o Espacio = red, E (mantener) = gancho, Esc = pausa.
    /// </summary>
    public static class InputUtil
    {
        public static float Vertical()
        {
#if ENABLE_INPUT_SYSTEM
            var k = Keyboard.current; if (k == null) return 0f;
            float v = 0f;
            if (k.wKey.isPressed || k.upArrowKey.isPressed) v += 1f;
            if (k.sKey.isPressed || k.downArrowKey.isPressed) v -= 1f;
            return v;
#else
            return Input.GetAxisRaw("Vertical");
#endif
        }

        public static float Horizontal()
        {
#if ENABLE_INPUT_SYSTEM
            var k = Keyboard.current; if (k == null) return 0f;
            float h = 0f;
            if (k.dKey.isPressed || k.rightArrowKey.isPressed) h += 1f;
            if (k.aKey.isPressed || k.leftArrowKey.isPressed) h -= 1f;
            return h;
#else
            return Input.GetAxisRaw("Horizontal");
#endif
        }

        public static bool NetPressed()
        {
#if ENABLE_INPUT_SYSTEM
            var k = Keyboard.current; var m = Mouse.current;
            return (k != null && k.spaceKey.wasPressedThisFrame) || (m != null && m.leftButton.wasPressedThisFrame);
#else
            return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
#endif
        }

        public static bool HookHeld()
        {
#if ENABLE_INPUT_SYSTEM
            var k = Keyboard.current; return k != null && k.eKey.isPressed;
#else
            return Input.GetKey(KeyCode.E);
#endif
        }

        public static bool PausePressed()
        {
#if ENABLE_INPUT_SYSTEM
            var k = Keyboard.current; return k != null && k.escapeKey.wasPressedThisFrame;
#else
            return Input.GetKeyDown(KeyCode.Escape);
#endif
        }
    }
}

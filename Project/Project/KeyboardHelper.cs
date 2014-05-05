using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Project
{
    //proporciona algumas funcionalidades extras como travamento de tecla
    public static class KeyboardHelper
    {
        private static Dictionary<Keys, bool> lockedKeys;   //bloqueia teclas especificas em algum estado (pressionadas = true, soltas = false)
        private static Dictionary<Keys, bool> LockedKeys
        {
            get
            {
                if (lockedKeys == null) lockedKeys = new Dictionary<Keys, bool>();
                return lockedKeys;
            }
        }

        public static void LockKey(Keys k)
        {
            LockKey(k, false);
        }

        public static void LockKey(Keys k, bool pressed)
        {
            if (!lockedKeys.ContainsKey(k))
                LockedKeys.Add(k, pressed);
            else
                LockedKeys[k] = pressed;
        }

        public static void UnlockKey(Keys k)
        {
            if (lockedKeys.ContainsKey(k))
                lockedKeys.Remove(k);
        }

        public static bool IsKeyDown(Keys k)
        {
            if (LockedKeys.ContainsKey(k))
            {
                return LockedKeys[k];
            }
            else
            {
                return Keyboard.GetState().GetPressedKeys().Contains(k);
            }
        }

        public static bool IsKeyLocked(Keys k)
        {
            return LockedKeys.ContainsKey(k);
        }

        public static bool KeyReleased(Keys k)
        {
            return !Keyboard.GetState().GetPressedKeys().Contains(k);
        }
    }
}

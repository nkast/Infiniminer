using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Infiniminer;

public static class InputManager
{
    internal static List<VirtualInput> VirtualInputs { get; private set; }
    public static KeyboardInfo Keyboard { get; private set; }
    public static MouseInfo Mouse { get; private set; }
    public static GamePadInfo GamePad { get; private set; }

    static  InputManager()
    {
        Keyboard = new();
        Mouse = new();
        GamePad = new(PlayerIndex.One);
        VirtualInputs = new List<VirtualInput>();
    }

    public static void Update(GameTime gameTime)
    {
        Keyboard.Update();
        Mouse.Update();
        GamePad.Update(gameTime);

        for (int i = 0; i < VirtualInputs.Count; i++)
        {
            VirtualInputs[i].Update();
        }
    }
}
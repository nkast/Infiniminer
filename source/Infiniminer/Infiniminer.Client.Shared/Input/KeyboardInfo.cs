using Microsoft.Xna.Framework.Input;

namespace Infiniminer;

public sealed class KeyboardInfo
{
    public KeyboardState PreviousState { get; private set; }
    public KeyboardState CurrentState { get; private set; }

    public bool AnyKeyCheck => CurrentState.GetPressedKeyCount() > 0;
    public bool AnyKeyPressed => CurrentState.GetPressedKeyCount() > PreviousState.GetPressedKeyCount();
    public bool AnyKeyReleased => CurrentState.GetPressedKeyCount() < PreviousState.GetPressedKeyCount();

    public KeyboardInfo()
    {
        PreviousState = new KeyboardState();
        CurrentState = Keyboard.GetState();
    }

    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Keyboard.GetState();
    }

    public bool Check(Keys key) => CurrentState.IsKeyDown(key);
    public bool Pressed(Keys key) => CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
    public bool Released(Keys key) => CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
}
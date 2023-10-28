/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2009 Zach Barth
Copyright (c) 2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Infiniminer;

public sealed class VirtualButton : VirtualInput
{
    private bool _consumePress;

    public List<Node> Nodes { get; private set; }

    public bool Check()
    {
        foreach (Node node in EnumerateNodes())
        {
            if (node.Check())
            {
                return true;
            }
        }

        return false;
    }

    public bool Pressed()
    {
        if (_consumePress) { return false; }

        foreach (Node node in EnumerateNodes())
        {
            if (node.Pressed())
            {
                return true;
            }
        }

        return false;
    }

    public bool Released()
    {
        foreach (Node node in EnumerateNodes())
        {
            if (node.Released())
            {
                return true;
            }
        }

        return false;
    }

    public VirtualButton()
    {
        Nodes = new List<Node>();
        _consumePress = false;
    }

    public override void Update() => _consumePress = false;

    public void ConsumePress() => _consumePress = true;

    private IEnumerable<Node> EnumerateNodes()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            yield return Nodes[i];
        }
    }

    ///////////////////////////////////////////////////////////////////////////
    /// Node
    ///////////////////////////////////////////////////////////////////////////
    #region Node

    public abstract class Node : VirtualNode
    {
        public abstract bool Check();
        public abstract bool Pressed();
        public abstract bool Released();
    }

    #endregion Node

    ///////////////////////////////////////////////////////////////////////////
    /// GamePad 
    ///////////////////////////////////////////////////////////////////////////
    #region GamePad

    public sealed class GamePad
    {
        ///////////////////////////////////////////////////////////////////////////
        /// Button
        ///////////////////////////////////////////////////////////////////////////
        #region Button

        public sealed class Button : Node
        {
            private readonly Buttons _button;

            public override bool Check() => InputManager.GamePad.ButtonCheck(_button);
            public override bool Pressed() => InputManager.GamePad.ButtonPressed(_button);
            public override bool Released() => InputManager.GamePad.ButtonReleased(_button);

            public Button(Buttons button) => _button = button;
        }

        #endregion Button

        ///////////////////////////////////////////////////////////////////////////
        /// Left Stick Up
        ///////////////////////////////////////////////////////////////////////////
        #region Left Stick Up

        public class LeftStickUp : Node
        {
            public float DeadZone;
            public bool UseGlobalDeadZone;

            public override bool Check() => UseGlobalDeadZone ? InputManager.GamePad.LeftStickUpCheck() : InputManager.GamePad.LeftStickUpCheck(DeadZone);
            public override bool Pressed() => UseGlobalDeadZone ? InputManager.GamePad.LeftStickUpPressed() : InputManager.GamePad.LeftStickUpPressed(DeadZone);
            public override bool Released() => UseGlobalDeadZone ? InputManager.GamePad.LeftStickUpReleased() : InputManager.GamePad.LeftStickUpReleased(DeadZone);

        }

        #endregion Left Stick Down        

        ///////////////////////////////////////////////////////////////////////////
        /// Left Stick Down
        ///////////////////////////////////////////////////////////////////////////
        #region Left Stick Down

        public class LeftStickDown : Node
        {
            public float DeadZone;
            public bool UseGlobalDeadZone;

            public override bool Check() => UseGlobalDeadZone ? InputManager.GamePad.LeftStickDownCheck() : InputManager.GamePad.LeftStickDownCheck(DeadZone);
            public override bool Pressed() => UseGlobalDeadZone ? InputManager.GamePad.LeftStickDownPressed() : InputManager.GamePad.LeftStickDownPressed(DeadZone);
            public override bool Released() => UseGlobalDeadZone ? InputManager.GamePad.LeftStickDownReleased() : InputManager.GamePad.LeftStickDownReleased(DeadZone);

        }

        #endregion Left Stick Down

        ///////////////////////////////////////////////////////////////////////////
        /// Left Stick Left
        ///////////////////////////////////////////////////////////////////////////
        #region Left Stick Left

        public class LeftStickLeft : Node
        {
            public float DeadZone;
            public bool UseGlobalDeadZone;

            public override bool Check() => UseGlobalDeadZone ? InputManager.GamePad.LeftStickLeftCheck() : InputManager.GamePad.LeftStickLeftCheck(DeadZone);
            public override bool Pressed() => UseGlobalDeadZone ? InputManager.GamePad.LeftStickLeftPressed() : InputManager.GamePad.LeftStickLeftPressed(DeadZone);
            public override bool Released() => UseGlobalDeadZone ? InputManager.GamePad.LeftStickLeftReleased() : InputManager.GamePad.LeftStickLeftReleased(DeadZone);

        }

        #endregion Left Stick Left    

        ///////////////////////////////////////////////////////////////////////////
        /// Left Stick Right
        ///////////////////////////////////////////////////////////////////////////
        #region Left Stick Right

        public class LeftStickRight : Node
        {
            public float DeadZone;
            public bool UseGlobalDeadZone;

            public override bool Check() => UseGlobalDeadZone ? InputManager.GamePad.LeftStickRightCheck() : InputManager.GamePad.LeftStickRightCheck(DeadZone);
            public override bool Pressed() => UseGlobalDeadZone ? InputManager.GamePad.LeftStickRightPressed() : InputManager.GamePad.LeftStickRightPressed(DeadZone);
            public override bool Released() => UseGlobalDeadZone ? InputManager.GamePad.LeftStickRightReleased() : InputManager.GamePad.LeftStickRightReleased(DeadZone);

        }

        #endregion Left Stick Right     

        ///////////////////////////////////////////////////////////////////////////
        /// Right Stick Up
        ///////////////////////////////////////////////////////////////////////////
        #region Right Stick Up

        public class RightStickUp : Node
        {
            public float DeadZone;
            public bool UseGlobalDeadZone;

            public override bool Check() => UseGlobalDeadZone ? InputManager.GamePad.RightStickUpCheck() : InputManager.GamePad.RightStickUpCheck(DeadZone);
            public override bool Pressed() => UseGlobalDeadZone ? InputManager.GamePad.RightStickUpPressed() : InputManager.GamePad.RightStickUpPressed(DeadZone);
            public override bool Released() => UseGlobalDeadZone ? InputManager.GamePad.RightStickUpReleased() : InputManager.GamePad.RightStickUpReleased(DeadZone);

        }

        #endregion Right Stick Down        

        ///////////////////////////////////////////////////////////////////////////
        /// Right Stick Down
        ///////////////////////////////////////////////////////////////////////////
        #region Right Stick Down

        public class RightStickDown : Node
        {
            public float DeadZone;
            public bool UseGlobalDeadZone;

            public override bool Check() => UseGlobalDeadZone ? InputManager.GamePad.RightStickDownCheck() : InputManager.GamePad.RightStickDownCheck(DeadZone);
            public override bool Pressed() => UseGlobalDeadZone ? InputManager.GamePad.RightStickDownPressed() : InputManager.GamePad.RightStickDownPressed(DeadZone);
            public override bool Released() => UseGlobalDeadZone ? InputManager.GamePad.RightStickDownReleased() : InputManager.GamePad.RightStickDownReleased(DeadZone);

        }

        #endregion Right Stick Down

        ///////////////////////////////////////////////////////////////////////////
        /// Right Stick Left
        ///////////////////////////////////////////////////////////////////////////
        #region Right Stick Left

        public class RightStickLeft : Node
        {
            public float DeadZone;
            public bool UseGlobalDeadZone;

            public override bool Check() => UseGlobalDeadZone ? InputManager.GamePad.RightStickLeftCheck() : InputManager.GamePad.RightStickLeftCheck(DeadZone);
            public override bool Pressed() => UseGlobalDeadZone ? InputManager.GamePad.RightStickLeftPressed() : InputManager.GamePad.RightStickPressed(DeadZone);
            public override bool Released() => UseGlobalDeadZone ? InputManager.GamePad.RightStickReleased() : InputManager.GamePad.RightStickReleased(DeadZone);

        }

        #endregion Right Stick Left    

        ///////////////////////////////////////////////////////////////////////////
        /// Right Stick Right
        ///////////////////////////////////////////////////////////////////////////
        #region Right Stick Right

        public class RightStickRight : Node
        {
            public float DeadZone;
            public bool UseGlobalDeadZone;

            public override bool Check() => UseGlobalDeadZone ? InputManager.GamePad.RightStickRightCheck() : InputManager.GamePad.RightStickRightCheck(DeadZone);
            public override bool Pressed() => UseGlobalDeadZone ? InputManager.GamePad.RightStickRightPressed() : InputManager.GamePad.RightStickRightPressed(DeadZone);
            public override bool Released() => UseGlobalDeadZone ? InputManager.GamePad.RightStickRightReleased() : InputManager.GamePad.RightStickRightReleased(DeadZone);

        }

        #endregion Right Stick Right    


        ///////////////////////////////////////////////////////////////////////////
        /// Left Trigger
        ///////////////////////////////////////////////////////////////////////////
        #region Left Trigger

        public class LeftTrigger : Node
        {
            public float DeadZone;
            public bool UseGlobalDeadZone;

            public override bool Check() => UseGlobalDeadZone ? InputManager.GamePad.LeftTriggerCheck() : InputManager.GamePad.LeftTriggerCheck(DeadZone);
            public override bool Pressed() => UseGlobalDeadZone ? InputManager.GamePad.LeftTriggerPressed() : InputManager.GamePad.LeftTriggerPressed(DeadZone);
            public override bool Released() => UseGlobalDeadZone ? InputManager.GamePad.LeftTriggerReleased() : InputManager.GamePad.LeftTriggerReleased(DeadZone);

        }

        #endregion Left Trigger  

        ///////////////////////////////////////////////////////////////////////////
        /// Right Trigger
        ///////////////////////////////////////////////////////////////////////////
        #region Right Trigger

        public class RightTrigger : Node
        {
            public float DeadZone;
            public bool UseGlobalDeadZone;

            public override bool Check() => UseGlobalDeadZone ? InputManager.GamePad.RightTriggerCheck() : InputManager.GamePad.RightTriggerCheck(DeadZone);
            public override bool Pressed() => UseGlobalDeadZone ? InputManager.GamePad.RightTriggerPressed() : InputManager.GamePad.RightTriggerPressed(DeadZone);
            public override bool Released() => UseGlobalDeadZone ? InputManager.GamePad.RightTriggerReleased() : InputManager.GamePad.RightTriggerReleased(DeadZone);

        }

        #endregion Right Trigger          
    }

    #endregion GamePad

    ///////////////////////////////////////////////////////////////////////////
    /// Keyboard
    ///////////////////////////////////////////////////////////////////////////
    #region Keyboard

    public sealed class Keyboard
    {
        ///////////////////////////////////////////////////////////////////////////
        /// Key
        ///////////////////////////////////////////////////////////////////////////
        #region Key

        public class Key : Node
        {
            private readonly Keys _key;

            public Key(Keys key) => _key = key;

            public override bool Check() => InputManager.Keyboard.Check(_key);
            public override bool Pressed() => InputManager.Keyboard.Pressed(_key);
            public override bool Released() => InputManager.Keyboard.Released(_key);
        }

        #endregion Key
    }

    #endregion Keyboard

    ///////////////////////////////////////////////////////////////////////////
    /// Mouse
    ///////////////////////////////////////////////////////////////////////////
    #region Mouse

    public sealed class Mouse
    {
        ///////////////////////////////////////////////////////////////////////////
        /// Left Button
        ///////////////////////////////////////////////////////////////////////////
        #region Left Button

        public class LeftButton : Node
        {
            public override bool Check() => InputManager.Mouse.LeftButtonCheck();
            public override bool Pressed() => InputManager.Mouse.LeftButtonPressed();
            public override bool Released() => InputManager.Mouse.LeftButtonReleased();
        }

        #endregion Left Button

        ///////////////////////////////////////////////////////////////////////////
        /// Right Button
        ///////////////////////////////////////////////////////////////////////////
        #region Right Button

        public class RightButton : Node
        {
            public override bool Check() => InputManager.Mouse.RightButtonCheck();
            public override bool Pressed() => InputManager.Mouse.RightButtonPressed();
            public override bool Released() => InputManager.Mouse.RightButtonReleased();
        }

        #endregion Right Button   

        ///////////////////////////////////////////////////////////////////////////
        /// Middle Button
        ///////////////////////////////////////////////////////////////////////////
        #region Middle Button

        public class MiddleButton : Node
        {
            public override bool Check() => InputManager.Mouse.MiddleButtonCheck();
            public override bool Pressed() => InputManager.Mouse.MiddleButtonPressed();
            public override bool Released() => InputManager.Mouse.MiddleButtonReleased();
        }

        #endregion Middle Button      

        ///////////////////////////////////////////////////////////////////////////
        /// X Button 1
        ///////////////////////////////////////////////////////////////////////////
        #region X Button 1

        public class XButton1 : Node
        {
            public override bool Check() => InputManager.Mouse.XButton1Check();
            public override bool Pressed() => InputManager.Mouse.XButton1Pressed();
            public override bool Released() => InputManager.Mouse.XButton1Released();
        }

        #endregion X Button 1    

        ///////////////////////////////////////////////////////////////////////////
        /// X Button 2
        ///////////////////////////////////////////////////////////////////////////
        #region X Button 2

        public class XButton2 : Node
        {
            public override bool Check() => InputManager.Mouse.XButton2Check();
            public override bool Pressed() => InputManager.Mouse.XButton2Pressed();
            public override bool Released() => InputManager.Mouse.XButton2Released();
        }

        #endregion X Button 1         
    }

    #endregion Mouse
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BasicStateMachine;

namespace CapsLock
{
    enum States {
        CapsLockActive,
        CapsLockInactive,
        Exited
    }

    enum Signals
    {
        CapsLockPressed,
        OtherKeyPressed,
        Exit
    }

    class KeyBoard : BasicStateMachine<States, Signals>
    {
        private char _pressedChar;
        public KeyBoard(States startState) : base(startState) 
        {
            //            From state                Signals      To state       OnTransition
            AddTransition(States.CapsLockInactive, Signals.Exit, States.Exited);
            AddTransition(States.CapsLockActive, Signals.Exit, States.Exited);

            AddTransition(States.CapsLockInactive, Signals.CapsLockPressed, States.CapsLockActive);
            AddTransition(States.CapsLockActive, Signals.CapsLockPressed, States.CapsLockInactive);

            AddTransition(States.CapsLockInactive, Signals.OtherKeyPressed, States.CapsLockInactive, SendLowercase);
            AddTransition(States.CapsLockActive, Signals.OtherKeyPressed, States.CapsLockActive, SendUppercase);
        }

        private void SendLowercase()
        {
            Console.WriteLine($"Sending: {char.ToLower(_pressedChar)}");
        }

        private void SendUppercase()
        {
            Console.WriteLine($"Sending: {char.ToUpper(_pressedChar)}");
        }

        public void ProcessInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                ProcessSignal(Signals.CapsLockPressed);
            }
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                ProcessSignal(Signals.Exit);
            }

            _pressedChar = keyInfo.KeyChar;
            ProcessSignal(Signals.OtherKeyPressed);
        }
    }
}

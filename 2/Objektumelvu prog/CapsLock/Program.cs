using System;
using BasicStateMachine;

namespace CapsLock
{
    internal class Program
    {
        static void Main(string[] args) 
        {
            Keyboard keyboard = new KeyBoard(States.CapsLockInactive);
            while (keyboard.CurrentState != States.Exited)
            {
                keyboard.ProcessSignal(GetSignal());
            }
        }
    }
}

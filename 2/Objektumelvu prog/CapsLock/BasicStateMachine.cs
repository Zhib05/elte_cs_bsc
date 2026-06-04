using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapsLock
{
    class BasicStateMachine<TState, TSignal>
    {
        private record TransitionInfo(TState ToState, Action? OnTransition);

        private Dictionary<(TState, TSignal), TransitionInfo> _transitionMap;
        private TState _currentState;

        public TState CurrentState => _currentState;

        protected BasicStateMachine(TState startState)
        {
            _transitionMap = new Dictionary<(TState, TSignal), TransitionInfo>();
            _currentState = startState;
        }

        protected void AddTransition(TState fromState, TSignal signal, TState toState, Action? onTransition = null)
        {
            _transitionMap[(fromState, signal)] = new TransitionInfo(toState, onTransition);
        }

        protected void ProcessSignal(TSignal signal)
        {
            var key = (_currentState, signal);

            if (_transitionMap.TryGetValue(key, out TransitionInfo? transition))
            {
                transition.OnTransition?.Invoke();
                _currentState = transition.ToState;
            } else
            {
                Console.WriteLine("Invalid transition: {_currentState} -> {signal}");
            }
        }
    }
}

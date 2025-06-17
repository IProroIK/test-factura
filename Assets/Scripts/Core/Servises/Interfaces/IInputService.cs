using System;
using Settings;

namespace Core.Service
{
    public interface IInputService
    {
        bool CanHandleInput { get; set; }

        int RegisterInputHandler(
            Enumerators.InputType type, int inputCode, Action onInputUp = null, Action onInputDown = null,
            Action onInput = null, Action<object> onInputEndParametrized = null);

        void UnregisterInputHandler(int index);
    }
}
using UnityEngine;

namespace InputTool
{
    public partial class InputManager
    {
        #region Factory Create

        public ComplexKey ComplexKey(string name, KeyCode btnA, KeyCode btnB)
        {
            return new ComplexKey(name, btnA, btnB);
        }
        public ComplexKeyDown ComplexKeyDown(string name, KeyCode btnA, KeyCode btnB)
        {
            return new ComplexKeyDown(name, btnA, btnB);
        }
        public ComplexKeyUp ComplexKeyUp(string name, KeyCode btnA, KeyCode btnB)
        {
            return new ComplexKeyUp(name, btnA, btnB);
        }
        public GetKey GetKey(string name, KeyCode key)
        {
            return new GetKey(name, key);
        }
        public GetKeyDown GetKeyDown(string name, KeyCode key)
        {
            return new GetKeyDown(name, key);
        }
        public GetKeyUp GetKeyUp(string name, KeyCode key)
        {
            return new GetKeyUp(name, key);
        }
        public LongPress LongPress(string name, KeyCode key, float duration)
        {
            return new LongPress(name, key, duration);
        }
        public ShortPress ShortPress(string name, KeyCode key, float duration)
        {
            return new ShortPress(name, key, duration);
        }
        #endregion
    }
}
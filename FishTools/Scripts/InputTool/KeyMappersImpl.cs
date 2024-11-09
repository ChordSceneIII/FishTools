using UnityEngine;
using FishTools.TimeTool;

/// <summary>
/// 在这里拓展映射KeyMapper类型
/// </summary>

namespace FishTools.InputTool
{
    public class ComplexKey : KeyMapper
    {
        KeyCode keyA;
        KeyCode keyB;
        internal ComplexKey(string name, KeyCode keyA, KeyCode keyB)
        {
            if (InputManager.AddMapper(name, this))
            {
                mappername = name;

                //默认配置
                this.keyA = keyA;
                this.keyB = keyB;
                LoadKey(); //加载配置
            }
        }

        public void SetKey(KeyCode keyA, KeyCode keyB)
        {
            this.keyA = keyA;
            this.keyB = keyB;
        }

        public override bool Check()
        {
            return Input.GetKey(keyA) && Input.GetKey(keyB);
        }

        public override void SaveKey()
        {
            SaveKey("KeyA", keyA);
            SaveKey("KeyB", keyB);
        }
        protected override void LoadKey()
        {
            LoadKey("KeyA", ref keyA);
            LoadKey("KeyB", ref keyB);
        }
    }

    public class ComplexKeyDown : KeyMapper
    {
        KeyCode keyA;
        KeyCode keyB;
        internal ComplexKeyDown(string name, KeyCode keyA, KeyCode keyB)
        {
            if (InputManager.AddMapper(name, this))
            {
                mappername = name;
                this.keyA = keyA;
                this.keyB = keyB;
                LoadKey();
            }
        }

        public void SetKey(KeyCode keyA, KeyCode keyB)
        {
            this.keyA = keyA;
            this.keyB = keyB;
        }

        public override bool Check()
        {
            return Input.GetKeyDown(keyA) && Input.GetKeyDown(keyB);

        }

        public override void SaveKey()
        {
            SaveKey("KeyA", keyA);
            SaveKey("KeyB", keyB);
        }

        protected override void LoadKey()
        {
            LoadKey("KeyA", ref keyA);
            LoadKey("KeyB", ref keyB);
        }
    }

    public class ComplexKeyUp : KeyMapper
    {
        KeyCode keyA;
        KeyCode keyB;
        bool check = false;

        internal ComplexKeyUp(string name, KeyCode keyA, KeyCode keyB)
        {
            if (InputManager.AddMapper(name, this))
            {
                mappername = name;

                this.keyA = keyA;
                this.keyB = keyB;

                LoadKey();
            }
        }

        public void SetKey(KeyCode keyA, KeyCode keyB)
        {
            this.keyA = keyA;
            this.keyB = keyB;
        }

        public override bool Check()
        {
            if (Input.GetKey(keyA) && Input.GetKey(keyB))
            {
                check = true;
            }

            if (check && !Input.GetKey(keyA) && !Input.GetKey(keyB))
            {
                check = false;
                return true;
            }

            return false;
        }


        public override void SaveKey()
        {
            SaveKey("KeyA", keyA);
            SaveKey("KeyB", keyB);
        }

        protected override void LoadKey()
        {
            LoadKey("KeyA", ref keyA);
            LoadKey("KeyB", ref keyB);
        }
    }

    public class GetKey : KeyMapper
    {
        KeyCode key;
        internal GetKey(string name, KeyCode key)
        {
            if (InputManager.AddMapper(name, this))
            {
                mappername = name;
                this.key = key;
                LoadKey();
            }
        }

        public void SetKey(KeyCode key)
        {
            this.key = key;
        }

        public override bool Check()
        {
            return Input.GetKey(key);
        }
        public override void SaveKey()
        {
            SaveKey("Key", key);
        }
        protected override void LoadKey()
        {
            LoadKey("Key", ref key);
        }
    }

    public class GetKeyDown : KeyMapper
    {
        KeyCode key;
        internal GetKeyDown(string name, KeyCode key)
        {
            if (InputManager.AddMapper(name, this))
            {
                mappername = name;
                this.key = key;
                LoadKey();
            }
        }

        public void SetKey(KeyCode key)
        {
            this.key = key;
        }

        public override bool Check()
        {
            return Input.GetKeyDown(key);
        }
        public override void SaveKey()
        {
            SaveKey("Key", key);
        }
        protected override void LoadKey()
        {
            LoadKey("Key", ref key);
        }
    }

    public class GetKeyUp : KeyMapper
    {
        KeyCode key;
        internal GetKeyUp(string name, KeyCode key)
        {
            if (InputManager.AddMapper(name, this))
            {
                mappername = name;
                this.key = key;
                LoadKey();
            }
        }

        public void SetKey(KeyCode key)
        {
            this.key = key;
        }

        public override bool Check()
        {
            return Input.GetKeyUp(key);
        }
        public override void SaveKey()
        {
            SaveKey("Key", key);
        }
        protected override void LoadKey()
        {
            LoadKey("Key", ref key);
        }
    }

    public class LongPress : KeyMapper
    {
        KeyCode key;
        FTimer timer;
        internal LongPress(string name, KeyCode key, float duration)
        {
            if (InputManager.AddMapper(name, this))
            {
                mappername = name;
                this.key = key;
                timer = FTM.CreateTimer(duration);
                LoadKey();
            }
        }

        public void SetKey(KeyCode key)
        {
            this.key = key;
        }
        public override bool Check()
        {
            if (Input.GetKeyDown(key))
            {
                timer.Start();
            }
            if (Input.GetKeyUp(key))
            {
                if (timer.IsCompleted)
                {
                    return true;
                }

                timer.Close();
            }
            return false;
        }
        public override void SaveKey()
        {
            SaveKey("Key", key);
        }
        protected override void LoadKey()
        {
            LoadKey("Key", ref key);
        }
    }

    public class ShortPress : KeyMapper
    {
        KeyCode key;
        FTimer timer;
        internal ShortPress(string name, KeyCode key, float duration)
        {
            if (InputManager.AddMapper(name, this))
            {
                mappername = name;
                this.key = key;
                timer = FTM.CreateTimer(duration);
                LoadKey();
            }

        }
        public void SetKey(KeyCode key)
        {
            this.key = key;
        }
        public override bool Check()
        {
            if (Input.GetKeyDown(key))
            {
                timer.Start();
            }
            if (Input.GetKeyUp(key))
            {
                if (!timer.IsCompleted)
                {
                    timer.Close();
                    return true;
                }

                timer.Close();
            }
            return false;
        }
        public override void SaveKey()
        {
            SaveKey("Key", key);
        }
        protected override void LoadKey()
        {
            LoadKey("Key", ref key);
        }
    }
}

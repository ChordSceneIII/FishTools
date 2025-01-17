
using UnityEngine.EventSystems;

namespace FishTools.EasyUI
{
    public interface ISlotReceiver
    {
        public ISlotReceiver receiver { get; }
        public string selectKey { get; }

        public void Register()
        {
            EventManager.AddListener<Slot>(selectKey, OnReceive);
        }
        public void UnRegister()
        {
            EventManager.RemoveListener<Slot>(selectKey, OnReceive);
        }
        public void OnReceive(Slot slot);
    }
}
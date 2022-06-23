using System;

namespace Assets.Scripts.Models.Item
{
    public abstract class Item
    {
        protected int _amount;
        public int Amount
        {
            get => _amount;
            set
            {
                var old = _amount;
                _amount = UnityEngine.Mathf.Clamp(value, 0 , Max);

                if (old == _amount) return;
                ItemChange?.Invoke(this, new ItemChangeEventArgs(old, _amount));
            }
        }
        protected int _max;
        public int Max { get => _max; }
        public event EventHandler<ItemChangeEventArgs> ItemChange;

        public Item() { }

        public abstract string GetNameAmountForm(string name);
    }

    public class Item<IItem> : Item
    {
        private IItem _model;

        public IItem Model => _model;

        public Item(string name, Type type)
        {
            _model = (IItem)Activator.CreateInstance(type, name);
            DataLoader.GetItemMax(type.Name, out _max);
        }

        public override string GetNameAmountForm(string name)
        {
            return name + $" ({Amount})";
        }
    }

    public class ItemChangeEventArgs : EventArgs
    {
        public readonly int OldValue;
        public readonly int NewValue;

        public ItemChangeEventArgs(int oldValue, int newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}

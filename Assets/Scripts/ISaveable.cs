using System;
using System.Collections;

namespace Assets.Scripts
{
    public interface ISaveable
    {
        void Save();
        void Load();
    }
}

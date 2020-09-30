using System;

namespace Ex
{
    public class ExBehaviour : ExObject
    {
        public ExObject ParentObject;
        public TimeSpan CurrentTickTime;
        public TimeSpan PreviousTickTime;

        public virtual void Awake()
        { }

        public virtual void Update()
        { }

        public virtual void BeforeDestroy()
        { }
    }
}

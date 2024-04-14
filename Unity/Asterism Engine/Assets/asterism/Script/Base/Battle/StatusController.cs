namespace Asterism.Battle
{
    public abstract class StatusController<T, U> : StatusControllerBase
    {
        protected T currentObject;
        protected U nextObject;

        protected override void Initialize(object obj)
        {
            currentObject = (T)obj;
            nextObject = default(U);
        }

        public override object NextStatusObject()
        {
            return nextObject;
        }
    }
}

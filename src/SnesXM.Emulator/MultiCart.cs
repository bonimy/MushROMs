namespace SnesXM.Emulator
{
    public class MultiCart : IMultiCart
    {
        public int CartType
        {
            get;
            set;
        }
        public ICart A
        {
            get;
            set;
        }
        public ICart B
        {
            get;
            set;
        }

        public MultiCart()
        {
            A = new Cart();
            B = new Cart();
        }

        public void Initialize()
        {
            CartType = 0;
            A.Initialize();
            B.Initialize();
        }
    }
}

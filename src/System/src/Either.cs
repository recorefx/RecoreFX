namespace System
{
    public struct Either<A, B>
    {
        private readonly A a;
        private readonly B b;
        private readonly bool left;

        public Either(A a)
        {
            this.a = a;
            b = default;
            left = true;
        }

        public Either(B b)
        {
            a = default;
            this.b = default;
            left = false;
        }

        public T Match<T>(Func<A, T> ifA, Func<B, T> ifB) => left ? ifA(a) : ifB(b);
    }
}
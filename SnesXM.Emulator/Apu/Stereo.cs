using System;

namespace SnesXM.Emulator.Apu
{
    public struct Stereo
    {
        public static readonly Stereo Empty = new Stereo();

        public int Left
        {
            get;
            set;
        }

        public int Right
        {
            get;
            set;
        }

        public int this[int index]
        {
            get => this[(Channel)index];
            set => this[(Channel)index] = value;
        }

        public int this[Channel channel]
        {
            get
            {
                switch (channel)
                {
                case Channel.Left:
                    return Left;
                case Channel.Right:
                    return Right;
                default:
                    throw new ArgumentException();
                }
            }
            set
            {
                switch (channel)
                {
                case Channel.Left:
                    Left = value;
                    break;
                case Channel.Right:
                    Right = value;
                    break;
                default:
                    throw new ArgumentException();

                }
            }
        }

        public Stereo(int left, int right)
        {
            Left = left;
            Right = right;
        }

        public Stereo AsInt16()
        {
            return new Stereo((short)Left, (short)Right);
        }

        public static implicit operator Stereo(int value)
        {
            return new Stereo(value, value);
        }
        public static Stereo operator +(Stereo left, Stereo right)
        {
            return new Stereo(left.Left + right.Left, left.Right + right.Right);
        }
        public static Stereo operator -(Stereo left, Stereo right)
        {
            return new Stereo(left.Left - right.Left, left.Right - right.Right);
        }
        public static Stereo operator *(Stereo left, Stereo right)
        {
            return new Stereo(left.Left * right.Left, left.Right * right.Right);
        }
        public static Stereo operator /(Stereo left, Stereo right)
        {
            return new Stereo(left.Left / right.Left, left.Right / right.Right);
        }
        public static Stereo operator &(Stereo left, Stereo right)
        {
            return new Stereo(left.Left & right.Left, right.Left & right.Right);
        }
        public static Stereo operator |(Stereo left, Stereo right)
        {
            return new Stereo(left.Left | right.Left, right.Left | right.Right);
        }
        public static Stereo operator ^(Stereo left, Stereo right)
        {
            return new Stereo(left.Left ^ right.Left, right.Left ^ right.Right);
        }
        public static Stereo operator >>(Stereo left, int right)
        {
            return new Stereo(left.Left >> right, left.Right >> right);
        }
        public static Stereo operator <<(Stereo left, int right)
        {
            return new Stereo(left.Left << right, left.Right << right);
        }
        public static Stereo operator ~(Stereo value)
        {
            return new Stereo(~value.Left, ~value.Right);
        }
        public static Stereo operator !(Stereo value)
        {
            return new Stereo(value.Right, value.Left);
        }

        public static bool operator ==(Stereo left, Stereo right)
        {
            return left.Left == right.Left && left.Right == right.Right;
        }
        public static bool operator !=(Stereo left, Stereo right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is Stereo stereo)
                return this == stereo;
            return false;
        }
        public override int GetHashCode()
        {
            return Left ^ Right;
        }
        public override string ToString()
        {
            return Helper.SR.GetString("Left = {0}; Right = {1}", Left, Right);
        }
    }
}

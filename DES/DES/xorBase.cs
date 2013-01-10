using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DES
{
    public class xorBase
    {
        public int A;
        public int C;
        public int T0;
        public int T;

        public int Mod;

        public xorBase(int a_A, int a_C, int a_T, int a_Mod)
        {
            A = a_A;
            C = a_C;
            T0 = a_T;
            T = T0;
            Mod = a_Mod;
        }

        byte xor(byte a_data, bool a_encrypt)
        {
            int _num;
            if (a_encrypt)
            {
                Math.DivRem(a_data + T, Mod, out _num);
            }
            else
            {
                Math.DivRem(a_data - T, Mod, out _num);
                if (_num < 0)
                {
                    _num += Mod;
                }
                Math.DivRem(_num, Mod, out _num);
            }
            Math.DivRem(A * C + T, Mod, out T);
            return (byte)_num;
        }

        public void Reset()
        {
            T = T0;
        }

        //public byte[] Encrypt(byte[] a_data)
        //{
        //    int _len = a_data.Length;
        //    byte[] _retVal = new byte[_len];

        //    for (int i = 0; i < _len; i++)
        //    {
        //        _retVal[i] = xor(a_data[i], true);
        //    }
        //    return _retVal;
        //}

        public byte[] GetXor()
        {
            int _len = 8;
            byte[] _retVal = new byte[_len];

            for (int i = 0; i < _len; i++)
            {
                _retVal[i] = xor(0, true);
            }
            return _retVal;
        }

        public byte[] XorSum(byte[] a_a, byte[] a_b)
        {
            int _len = 8;
            byte[] _retVal = new byte[_len];

            for (int i = 0; i < _len; i++)
            {
                _retVal[i]=(byte)(((int)a_a[i])^((int)a_b[i]));
            }

            return _retVal;
        }
    }
}

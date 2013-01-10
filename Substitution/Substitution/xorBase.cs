using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encrypt
{
    public class xorBase : IBlockCrypt
    {
        public int A;
        public int C;
        public int T0;
        public int T;

        public int Mod;

        public xorBase(int a_A, int a_C, int a_T, int a_Mod)
        {
            A=a_A;
            C=a_C;
            T0=a_T;
            Mod = a_Mod;
        }

        char xor(char a_char, bool a_encrypt)
        {
            int _num;
            if (a_encrypt)
            {
                Math.DivRem(a_char + T, Mod, out _num);
            }
            else
            {
                Math.DivRem(a_char - T, Mod, out _num);
                if (_num < 0)
                {
                    _num += Mod;
                }
                Math.DivRem(_num, Mod, out _num);
            }
            Math.DivRem(A * C + T, Mod, out T);
            return (char)_num;
        }

        public string Encrypt(string a_source)
        {
            string _retStr="";
            T = T0;
            for (int i = 0; i < a_source.Length; i++)
            {
                _retStr+=xor(a_source[i], true);
            }
            return _retStr;
        }

        public string Decrypt(string a_source)
        {
            string _retStr="";
            T = T0;
            for (int i = 0; i < a_source.Length; i++)
            {
                _retStr+=xor(a_source[i], false);
            }
            return _retStr;
        }
    }
}
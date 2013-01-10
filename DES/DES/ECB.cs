using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DES
{
    public class ECB
    {
        const int w = 64;
        const int rew = 48;
        const int rw = 32;
        const int rounds = 16;
        const int b = 8;

        int mod(int a, int b)
        {
            a %= b;
            return (a < 0) ? b + a : a;
        }

        int[] bitwise(UInt64 a_source, int a_width)
        {
            int[] _retArr = new int[a_width];

            for (int i = 0; i < a_width; i++)
            {
                _retArr[i] = (int)(a_source % 2);
                a_source=a_source>>1;
            }

            return _retArr;
        }

        ulong unbitwise(int[] a_source)
        {
            int _len = a_source.Length;
            ulong _retValue=0;

            for (int i = 0; i < _len; i++)
            {
                _retValue = (((ulong)a_source[i]) << i) + _retValue;
            }

            return _retValue;
        }

        int[] permute(int[] a_data, int[] a_table)
        {
            int _len = a_table.Length;
            int[] _retData = new int[_len];
            for (int i = 0; i < _len; i++)
            {
                _retData[i] = a_data[a_table[i]];
            }
            return _retData;
        }

        int[] shift(int[] a_data, int a_count)
        {
            int _len = a_data.Length;
            int[] _retData=new int[_len];
            for (int i = 0; i < _len; i++)
            {
                _retData[i] = a_data[mod(i - a_count, _len)];
            }
            return _retData;
        }

        Tuple<int[], int[]> split(int[] a_data)
        {
            int _len=a_data.Length>>1;
            int[] _lowPart = new int[_len], _highPart = new int[_len];
            for (int i=0; i<_len; i++)
            {
                _lowPart[i]=a_data[i];
                _highPart[i]=a_data[i+_len];
            }
            return new Tuple<int[], int[]>(_lowPart, _highPart);
        }

        int[] merge(int[] a_lowPart, int[] a_highPart)
        {
            int _len = a_lowPart.Length;
            int _len2 = a_highPart.Length;
            int[] _retData = new int[_len + _len2];
            for (int i = 0; i < _len; i++)
            {
                _retData[i] = a_lowPart[i];
            }
            for (int i = 0; i < _len2; i++)
            {
                _retData[i + _len] = a_highPart[i];
            }
            return _retData;
        }

        int[] IP ={
            57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7,
            56, 48, 40, 32, 24, 16, 8, 0, 58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6,
        };

        int[] initialPermutation(int[] a_data)
        {
            return permute(a_data, IP);
        }

        int[] EP={
            39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25, 32, 0, 40, 8, 48, 16, 56, 24,
        };

        int[] endPermutation(int[] a_data)
        {
            return permute(a_data, EP);
        }

        int[] E={
            31, 0, 1, 2, 3, 4,
            3, 4, 5, 6, 7, 8,
            7, 8, 9, 10, 11, 12,
            11, 12, 13, 14, 15, 16,
            15, 16, 17, 18, 19, 20,
            19, 20, 21, 22, 23, 24,
            23, 24, 25, 26, 27, 28,
            27, 28, 29, 30, 31, 0,
        };

        int[] widenE(int[] a_data)
        {
            return permute(a_data, E);
        }

        int[] xorAB(int[] a_a, int[] a_b)
        {
            int _len=a_a.Length;
            int[] _retData = new int[_len];
            for (int i = 0; i < _len; i++)
            {
                _retData[i] = a_a[i] ^ a_b[i];
            }
            return _retData;
        }

        int[] C0={
            56, 48, 40, 32, 24, 16, 8, 0, 57, 49, 41, 33, 25, 17,
            9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35,
        };

        int[] D0={
            62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21,
            13, 5, 60, 52, 44, 36, 28, 20, 12, 4, 27, 19, 11, 3,
        };

        int[] keyPermute={
            13, 16, 10, 23, 0, 4, 2, 27, 14, 5, 20, 9, 22, 18, 11, 3,
            25, 7, 15, 6, 26, 19, 12, 1, 40, 51, 30, 36, 46, 54, 29, 39,
            50, 44, 32, 47, 43, 48, 38, 55, 33, 52, 45, 41, 49, 35, 28, 31,
        };

        int[] keyShifts={
            1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1
        };

        List<int[]> keys;

        public void KeyRing(UInt64 a_source)
        {
            var _key = bitwise(a_source, w);

            var _c0 = permute(_key, C0);
            var _d0 = permute(_key, D0);

            keys = new List<int[]>(rounds);
            for (int i = 0; i < rounds; i++)
            {
                _c0 = shift(_c0, keyShifts[i]);
                _d0 = shift(_d0, keyShifts[i]);

                var _c0d0=merge(_d0, _c0);

                keys.Add(permute(_c0d0, keyPermute));
            }
        }

        Tuple<int[], int[]> makeAB(int[] a_data)
        {
            int[] _a = new int[b];
            int[] _b = new int[b];

            for (int i = 0; i < b; i++)
            {
                _a[i] = (a_data[5 + 6 * i] << 1) + a_data[6 * i];
                _b[i] = ((((a_data[4 + 6 * i] << 1) + a_data[3 + 6 * i]) << 1) + a_data[2 + 6 * i] << 1) + a_data[3 + 6 * i];
            }

            return new Tuple<int[], int[]>(_a, _b);
        }

        List<int[,]> S = new List<int[,]>()
        {
            new int[,]{
                {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7},
                {0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8},
                {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0},
                {15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13},
            },
            new int[,]{
                {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
                {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
                {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
                {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9},
            },
            new int[,]{
                {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
                {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
                {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
                {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12},
            },
            new int[,]{
                {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
                {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
                {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
                {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14},
            },
            new int[,]{
                {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
                {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
                {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
                {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3},
            },
            new int[,]{
                {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
                {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
                {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
                {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13},
            },
            new int[,]{
                {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
                {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
                {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
                {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12},
            },
            new int[,]{
                {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
                {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
                {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
                {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11},
            },
        };

        int[] P ={
            15, 6, 19, 20, 28, 11, 27, 16,
            0, 14, 22, 25, 4, 17, 30, 9,
            1, 7, 23, 13, 31, 26, 2, 8,
            18, 12, 29, 5, 21, 10, 3, 24,
        };

        int[] func(int[] a_data, int a_round)
        {
            var _widen = widenE(a_data);
            var _ab = makeAB(_widen);
            int[] _retValue = new int[0];
            for (int i = 0; i < b; i++)
            {
                //ulong _newB = (ulong)(S[i])[_ab.Item1[b - 1 - i], _ab.Item2[b - 1 - i]];
                ulong _newB = (ulong)(S[i])[_ab.Item1[b - 1 - i], _ab.Item2[b - 1 - i]];
                var _bitwiseB = bitwise(_newB, 4);
                _retValue = merge(_retValue, _bitwiseB);
            }
            _retValue = permute(_retValue, P);
            return _retValue;
        }

        void encrypt(ref int[] a_l0, ref int[] a_r0, int a_round)
        {
            var _funcResult = func(a_r0, a_round);
            var _temp = xorAB(a_l0, _funcResult);
            a_l0 = a_r0;
            a_r0 = _temp;
        }

        void decrypt(ref int[] a_l0, ref int[] a_r0, int a_round)
        {
            var _funcResult = func(a_l0, rounds-a_round);
            var _temp = xorAB(a_r0, _funcResult);
            a_r0 = a_l0;
            a_l0 = _temp;
        }

        int[] cryptBlock(int[] a_data, bool a_isEncrypt)
        {
            var _ip = initialPermutation(a_data);
            var _splitResult=split(_ip);
            var _l0 = _splitResult.Item2;
            var _r0 = _splitResult.Item1;
            for (int i = 0; i < rounds; i++)
            {
                if (a_isEncrypt)
                {
                    encrypt(ref _l0, ref _r0, i);
                }
                else
                {
                    decrypt(ref _l0, ref _r0, i);
                }
            }
            var _retVal = merge(_r0, _l0);
            _retVal = endPermutation(_retVal);
            return _retVal;
        }

        ulong[] widenData(byte[] a_data)
        {
            int _blockSize = w >> 3;
            int _count=a_data.Length/_blockSize;
            int _rem = a_data.Length % _blockSize;
            ulong[] _retValue = new ulong[_count + (_rem > 0 ? 1 : 0)];
            for (int i = 0; i < _count; i++)
            {
                ulong _temp=0;
                for (int j = 0; j < _blockSize; j++)
                {
                    _temp = (_temp << 8) + a_data[i*_blockSize+j];
                }
                _retValue[i] = _temp;
            }
            if (_rem > 0)
            {
                ulong _temp = 0;
                for (int j = 0; j < _rem; j++)
                {
                    _temp = (_temp << 8) + a_data[_count * _blockSize + j];
                }
                _temp=_temp << (_blockSize - _rem) * 8;
                _retValue[_count] = _temp;
            }
            return _retValue;
        }

        byte[] unwideData(ulong[] a_data)
        {
            int _blockSize = w >> 3;
            int _count = a_data.Length;
            byte[] _retValue = new byte[_count*_blockSize];
            for (int i = 0; i < _count; i++)
            {
                for (int j = 1; j <= _blockSize; j++)
                {
                    _retValue[(i + 1) * _blockSize - j] = (byte)(a_data[i] % 256);
                    a_data[i] >>= 8;
                }
            }
            return _retValue;
        }

        public byte[] Encrypt(byte[] a_data)
        {
            var _data=widenData(a_data);
            for (int i = 0; i < _data.Length; i++)
            {
                var _bitwise=bitwise(_data[i], w);
                var _crypted = cryptBlock(_bitwise, true);
                _data[i] = unbitwise(_crypted);
            }
            return unwideData(_data);
        }

        public byte[] Decrypt(byte[] a_data)
        {
            var _data = widenData(a_data);
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = unbitwise(cryptBlock(bitwise(_data[i], w), false));
            }
            return unwideData(_data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DES
{
    public class CFB : IDES
    {
        const int w = 64;

        xorBase Xor = new xorBase(37, 11, 30, 256);

        ECB internalECB = new ECB();

        List<byte[]> splitData(byte[] a_data)
        {
            int _blockSize = w >> 3;
            int _count = a_data.Length / _blockSize;
            int _rem = a_data.Length % _blockSize;
            List<byte[]> _retValue = new List<byte[]>(_count + (_rem > 0 ? 1 : 0));
            for (int i = 0; i < _count; i++)
            {
                byte[] _temp = new byte[_blockSize];
                for (int j = 0; j < _blockSize; j++)
                {
                    _temp[j] = a_data[i * _blockSize + j];
                }
                _retValue.Add(_temp);
            }
            if (_rem > 0)
            {
                byte[] _temp = new byte[_blockSize];
                for (int j = 0; j < _rem; j++)
                {
                    _temp[j] = a_data[_count * _blockSize + j];
                }
                for (int j = 0; j < (_blockSize - _rem); j++)
                {
                    _temp[_rem + j] = 0;
                }
                _retValue[_count] = _temp;
            }
            return _retValue;
        }

        byte[] unsplitData(List<byte[]> a_data)
        {
            int _blockSize = w >> 3;
            int _count = a_data.Count;
            byte[] _retValue = new byte[_count * _blockSize];
            for (int i = 0; i < _count; i++)
            {
                for (int j = 0; j < _blockSize; j++)
                {
                    _retValue[i * _blockSize + j] = a_data[i][j];
                }
            }
            return _retValue;
        }

        public byte[] Encrypt(byte[] a_data)
        {
            Xor.Reset();
            byte[] C0 = Xor.GetXor();
            var _data = splitData(a_data);
            for (int i = 0; i < _data.Count; i++)
            {
                C0 = Xor.XorSum(_data[i], internalECB.Encrypt(C0));
                _data[i] = C0;
            }
            return unsplitData(_data);
        }

        public byte[] Decrypt(byte[] a_data)
        {
            Xor.Reset();
            byte[] C0 = Xor.GetXor();
            var _data = splitData(a_data);
            for (int i = 0; i < _data.Count; i++)
            {
                var _temp = _data[i];
                _data[i] = Xor.XorSum(_data[i], internalECB.Encrypt(C0));
                C0 = _temp;
            }
            return unsplitData(_data);
        }

        public void KeyRing(ulong a_source)
        {
            internalECB.KeyRing(a_source);
        }
    }
}

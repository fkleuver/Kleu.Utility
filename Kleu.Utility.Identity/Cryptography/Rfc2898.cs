using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Kleu.Utility.Identity.Cryptography
{
    public class Rfc2898
    {
        private readonly HMACSHA512 _hmacsha512Obj;
        private readonly int hLen;
        private readonly byte[] P;
        private readonly byte[] S;
        private readonly int c;
        private int dkLen;

        public const int CMinIterations = 1000;
        public const int CMinSaltLength = 8;

        public Rfc2898(byte[] password, byte[] salt, int iterations)
        {
            if (iterations < CMinIterations)
            {
                throw new IterationsLessThanRecommended();
            }

            if (salt.Length < CMinSaltLength)
            {
                throw new SaltLessThanRecommended();
            }

            _hmacsha512Obj = new HMACSHA512(password);
            hLen = _hmacsha512Obj.HashSize / 8;
            P = password;
            S = salt;
            c = iterations;
        }

        public Rfc2898(string password, byte[] salt, int iterations) : this(new UTF8Encoding(false).GetBytes(password), salt, iterations)
        {

        }

        public Rfc2898(string password, string salt, int iterations) : this(new UTF8Encoding(false).GetBytes(password), new UTF8Encoding(false).GetBytes(salt), iterations)
        {

        }

        public byte[] GetDerivedKeyBytes_PBKDF2_HMACSHA512(int keyLength)
        {
            dkLen = keyLength;

            var l = Math.Ceiling((double)dkLen / hLen);

            var finalBlock = new byte[0];

            for (var i = 1; i <= l; i++)
            {
                finalBlock = pMergeByteArrays(finalBlock, F(P, S, c, i));
            }

            return finalBlock.Take(dkLen).ToArray();
        }

        public static byte[] PBKDF2(byte[] P, byte[] S, int c, int dkLen)
        {
            var rfcObj = new Rfc2898(P, S, c);
            return rfcObj.GetDerivedKeyBytes_PBKDF2_HMACSHA512(dkLen);
        }

        private byte[] F(byte[] P, byte[] S, int c, int i)
        {

            var Si = pMergeByteArrays(S, INT(i));

            var temp = PRF(P, Si);

            var U_c = temp;

            for (var C = 1; C < c; C++)
            {
                temp = PRF(P, temp);

                for (var j = 0; j < temp.Length; j++)
                {
                    U_c[j] ^= temp[j];
                }
            }

            return U_c;
        }

        private byte[] PRF(byte[] P, byte[] S)
        {
            return _hmacsha512Obj.ComputeHash(pMergeByteArrays(P, S));
        }

        private byte[] INT(int i)
        {
            var I = BitConverter.GetBytes(i);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(I);
            }

            return I;
        }

        private byte[] pMergeByteArrays(byte[] source1, byte[] source2)
        {
            var buffer = new byte[source1.Length + source2.Length];
            Buffer.BlockCopy(source1, 0, buffer, 0, source1.Length);
            Buffer.BlockCopy(source2, 0, buffer, source1.Length, source2.Length);

            return buffer;
        }
    }
}
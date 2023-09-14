using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TapoConnect.Exceptions;
using TapoConnect.Util;

namespace TapoConnect.Protocol
{
    public class KlapChiper
    {
        protected byte[] Key { get; }
        protected byte[] Iv { get; }
        protected byte[] Sig { get; }
        public int Seq { get; private set; }
        protected byte[] SegBytes
        {
            get
            {
                var seqBytes = BitConverter.GetBytes(Seq);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(seqBytes);

                return seqBytes;
            }
        }

        protected byte[] IvSeq
        {
            get
            {
                var payload = Iv.Concat(SegBytes).ToArray();

                if (payload.Length != 16)
                {
                    throw new TapoKlapException("Iv and Seq bytes is incorrect length.");
                }
                else
                {
                    return payload;
                }
            }
        }

        public KlapChiper(byte[] localSeed, byte[] remoteSeed, byte[] userHash)
        {
            Key = KeyDerive(localSeed, remoteSeed, userHash);
            (Iv, Seq) = IvDerive(localSeed, remoteSeed, userHash);
            Sig = SigDerive(localSeed, remoteSeed, userHash);
        }

        public byte[] Encrypt(string message)
        {
            Seq++;

            var cipherText = TapoCrypto.Encrypt(message, Key, IvSeq);

            var payload = Sig.Concat(SegBytes).Concat(cipherText).ToArray();

            var signature = TapoCrypto.Sha256Hash(payload);

            return signature.Concat(cipherText).ToArray();
        }

        public byte[] Decrypt(byte[] message)
        {
            var messagePart = message.Skip(32).ToArray();
            return TapoCrypto.Decrypt(messagePart, Key, IvSeq);
        }

        protected byte[] KeyDerive(byte[] localSeed, byte[] remoteSeed, byte[] userHash)
        {
            byte[] prefix = new byte[] { (byte)'l', (byte)'s', (byte)'k' };

            var payload = prefix.Concat(localSeed).Concat(remoteSeed).Concat(userHash).ToArray();

            var hash = TapoCrypto.Sha256Hash(payload);

            return hash.Take(16).ToArray();
        }

        protected (byte[], int) IvDerive(byte[] localSeed, byte[] remoteSeed, byte[] userHash)
        {
            byte[] prefix = new byte[] { (byte)'i', (byte)'v' };

            var payload = prefix.Concat(localSeed).Concat(remoteSeed).Concat(userHash).ToArray();

            var hash = TapoCrypto.Sha256Hash(payload);

            var seqBytes = hash.TakeLast(4).ToArray();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(seqBytes);

            int seq = BitConverter.ToInt32(seqBytes, 0);

            var iv = hash.Take(12).ToArray();

            return (iv, seq);
        }

        protected byte[] SigDerive(byte[] localSeed, byte[] remoteSeed, byte[] userHash)
        {
            byte[] prefix = new byte[] { (byte)'l', (byte)'d', (byte)'k' };

            var payload = prefix.Concat(localSeed).Concat(remoteSeed).Concat(userHash).ToArray();

            var hash = TapoCrypto.Sha256Hash(payload);

            return hash.Take(28).ToArray();
        }
    }
}

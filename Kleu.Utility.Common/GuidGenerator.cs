using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
// ReSharper disable MemberCanBePrivate.Global

namespace Kleu.Utility.Common
{
    public static class GuidGenerator
    {
        private static readonly Random Random;
        private static readonly object Lock = new object();

        private static long _lastTimestampForNoDuplicatesGeneration = GetTicksSinceGregorianCalendarEpoch();
        private static short _lastClockSequenceForNoDuplicatesGeneration;

        // number of bytes in uuid
        private const int ByteArraySize = 16;

        // multiplex variant info
        private const int VariantByte = 8;
        private const int VariantByteMask = 0x3f;
        private const int VariantByteShift = 0x80;

        // multiplex version info
        private const int VersionByte = 7;
        private const int VersionByteMask = 0x0f;
        private const int VersionByteShift = 4;

        // indexes within the uuid array for certain boundaries
        private const byte TimestampByte = 0;
        private const byte GuidClockSequenceByte = 8;
        private const byte NodeByte = 10;



        public static byte[] NodeBytes { get; set; }
        public static byte[] ClockSequenceBytes { get; set; }

        static GuidGenerator()
        {
            Random = new Random();
            try
            {
                var nic = NetworkInterface
                    .GetAllNetworkInterfaces()
                    .FirstOrDefault(n => n.OperationalStatus == OperationalStatus.Up);
                // ReSharper disable once PossibleNullReferenceException
                NodeBytes = GenerateNodeBytes(nic.GetPhysicalAddress());
            }
            catch
            {
                NodeBytes = GenerateNodeBytes();
            }

            _lastClockSequenceForNoDuplicatesGeneration = 0;
            ClockSequenceBytes = GenerateClockSequenceBytes();
        }

        public static byte[] GenerateNodeBytes()
        {
            var node = new byte[6];

            Random.NextBytes(node);
            return node;
        }

        public static byte[] GenerateNodeBytes(IPAddress ip)
        {
            if (ip == null)
                throw new ArgumentNullException(nameof(ip));

            var bytes = ip.GetAddressBytes();

            if (bytes.Length < 6)
                throw new ArgumentOutOfRangeException(nameof(ip), "The passed in IP address must contain at least 6 bytes.");

            var node = new byte[6];
            Array.Copy(bytes, node, 6);

            return node;
        }

        public static byte[] GenerateNodeBytes(PhysicalAddress mac)
        {
            if (mac == null)
                throw new ArgumentNullException(nameof(mac));

            var node = mac.GetAddressBytes();

            return node;
        }

        public static byte[] GenerateClockSequenceBytes()
        {
            if (_lastClockSequenceForNoDuplicatesGeneration == short.MaxValue)
            {
                _lastClockSequenceForNoDuplicatesGeneration = 0;
            }
            else
            {
                _lastClockSequenceForNoDuplicatesGeneration++;
            }
            var bytes = BitConverter.GetBytes(_lastClockSequenceForNoDuplicatesGeneration);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        public static GuidVersion GetUuidVersion(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            return (GuidVersion)((bytes[VersionByte] & 0xFF) >> VersionByteShift);
        }

        public static long GetTicksSinceGregorianCalendarEpoch()
        {
            return FileTimeApi.GetSystemTimePrecise().MinusGregorianCalendarEpoch().Ticks;
        }

        public static Guid GenerateTimeBasedGuid()
        {
            lock (Lock)
            {
                var ts = GetTicksSinceGregorianCalendarEpoch();

                if (ts <= _lastTimestampForNoDuplicatesGeneration)
                {
                    ClockSequenceBytes = GenerateClockSequenceBytes();
                }
                else
                {
                    _lastClockSequenceForNoDuplicatesGeneration = 0;
                    ClockSequenceBytes = GenerateClockSequenceBytes();
                }

                _lastTimestampForNoDuplicatesGeneration = ts;

                return GenerateTimeBasedGuid(ts, ClockSequenceBytes, NodeBytes);
            }
        }

        public static Guid GenerateTimeBasedGuid(long ticksSinceUnixEpoch, byte[] clockSequence, byte[] node)
        {
            if (clockSequence == null)
                throw new ArgumentNullException(nameof(clockSequence));

            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (clockSequence.Length != 2)
                throw new ArgumentOutOfRangeException(nameof(clockSequence), "The clockSequence must be 2 bytes.");

            if (node.Length != 6)
                throw new ArgumentOutOfRangeException(nameof(node), "The node must be 6 bytes.");

            var guid = new byte[ByteArraySize];
            var timestamp = BitConverter.GetBytes(ticksSinceUnixEpoch);

            // copy node
            Array.Copy(node, 0, guid, NodeByte, Math.Min(6, node.Length));

            // copy clock sequence
            Array.Copy(clockSequence, 0, guid, GuidClockSequenceByte, Math.Min(2, clockSequence.Length));

            // copy timestamp
            Array.Copy(timestamp, 0, guid, TimestampByte, Math.Min(8, timestamp.Length));

            // set the variant
            guid[VariantByte] &= VariantByteMask;
            guid[VariantByte] |= VariantByteShift;

            // set the version
            guid[VersionByte] &= VersionByteMask;
            guid[VersionByte] |= (byte)GuidVersion.TimeBased << VersionByteShift;

            return new Guid(guid);
        }
    }

    public enum GuidVersion
    {
        TimeBased = 0x01,
        Reserved = 0x02,
        NameBased = 0x03,
        Random = 0x04
    }
}

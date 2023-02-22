using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using System.Runtime.CompilerServices;

namespace AdicionalWeb.Persistencia.Validaciones
{
    public class Base32Encoding
    {
        public static byte[] ToBytes(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }

            input = input.TrimEnd('='); //remove padding characters
            int byteCount = input.Length * 5 / 8; //this must be TRUNCATED
            byte[] returnArray = new byte[byteCount];

            byte curByte = 0, bitsRemaining = 8;
            int mask = 0, arrayIndex = 0;

            foreach (char c in input)
            {
                int cValue = CharToValue(c);

                if (bitsRemaining > 5)
                {
                    mask = cValue << (bitsRemaining - 5);
                    curByte = (byte)(curByte | mask);
                    bitsRemaining -= 5;
                }
                else
                {
                    mask = cValue >> (5 - bitsRemaining);
                    curByte = (byte)(curByte | mask);
                    returnArray[arrayIndex++] = curByte;
                    curByte = (byte)(cValue << (3 + bitsRemaining));
                    bitsRemaining += 3;
                }
            }

            //if we didn't end with a full byte
            if (arrayIndex != byteCount)
            {
                returnArray[arrayIndex] = curByte;
            }

            return returnArray;
        }

        public static string ToString(byte[] input)
        {
            if (input == null || input.Length == 0)
            {
                throw new ArgumentNullException("input");
            }

            int charCount = (int)Math.Ceiling(input.Length / 5d) * 8;
            char[] returnArray = new char[charCount];

            byte nextChar = 0, bitsRemaining = 5;
            int arrayIndex = 0;

            foreach (byte b in input)
            {
                nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
                returnArray[arrayIndex++] = ValueToChar(nextChar);

                if (bitsRemaining < 4)
                {
                    nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
                    returnArray[arrayIndex++] = ValueToChar(nextChar);
                    bitsRemaining += 5;
                }

                bitsRemaining -= 3;
                nextChar = (byte)((b << bitsRemaining) & 31);
            }

            //if we didn't end with a full char
            if (arrayIndex != charCount)
            {
                returnArray[arrayIndex++] = ValueToChar(nextChar);
                while (arrayIndex != charCount) returnArray[arrayIndex++] = '='; //padding
            }

            return new string(returnArray);
        }

        private static int CharToValue(char c)
        {
            int value = (int)c;

            //65-90 == uppercase letters
            if (value < 91 && value > 64)
            {
                return value - 65;
            }
            //50-55 == numbers 2-7
            if (value < 56 && value > 49)
            {
                return value - 24;
            }
            //97-122 == lowercase letters
            if (value < 123 && value > 96)
            {
                return value - 97;
            }

            throw new ArgumentException("Character is not a Base32 character.", "c");
        }

        private static char ValueToChar(byte b)
        {
            if (b < 26)
            {
                return (char)(b + 65);
            }

            if (b < 32)
            {
                return (char)(b + 24);
            }

            throw new ArgumentException("Byte is not a value Base32 value.", "b");
        }
    }

    public abstract class Authenticator
    {
        private static readonly RNGCryptoServiceProvider Random = new RNGCryptoServiceProvider();    // Is Thread-Safe
        private static readonly int KeyLength = 16;
        private static readonly string AvailableKeyChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string GenerateKey()
        {
            var keyChars = new char[KeyLength];
            for (int i = 0; i < keyChars.Length; i++)
            {
                keyChars[i] = AvailableKeyChars[RandomInt(AvailableKeyChars.Length)];
            }
            return new String(keyChars);
        }

        protected string GetCodeInternal(string secret, ulong challengeValue)
        {
            ulong chlg = challengeValue;
            byte[] challenge = new byte[8];
            for (int j = 7; j >= 0; j--)
            {
                challenge[j] = (byte)((int)chlg & 0xff);
                chlg >>= 8;
            }

            var key = Base32Encoding.ToBytes(secret);
            for (int i = secret.Length; i < key.Length; i++)
            {
                key[i] = 0;
            }

            HMACSHA1 mac = new HMACSHA1(key);
            var hash = mac.ComputeHash(challenge);

            int offset = hash[hash.Length - 1] & 0xf;

            int truncatedHash = 0;
            for (int j = 0; j < 4; j++)
            {
                truncatedHash <<= 8;
                truncatedHash |= hash[offset + j];
            }

            truncatedHash &= 0x7FFFFFFF;
            truncatedHash %= 1000000;

            string code = truncatedHash.ToString();
            return code.PadLeft(6, '0').Trim();
        }

        protected bool ConstantTimeEquals(string a, string b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;

            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)a[i] ^ (uint)b[i];
            }

            return diff == 0;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected static int RandomInt(int max)
        {
            var randomBytes = new byte[4];
            Random.GetBytes(randomBytes);

            return Math.Abs((int)BitConverter.ToUInt32(randomBytes, 0) % max);
        }
    }

    /// <summary>
    /// Implementation of RFC 4226 Counter-Based One-Time Password Algorithm
    /// </summary>
    public class CounterAuthenticator : Authenticator
    {
        private readonly int WindowSize;

        public CounterAuthenticator(int windowSize)
        {
            if (windowSize <= 0)
            {
                throw new ArgumentException("look-ahead window size must be positive");
            }

            this.WindowSize = windowSize;
        }

        /// <summary>
        /// Generates One-Time-Password.
        /// </summary>
        /// <param name="secret">Shared Secret</param>
        /// <param name="counter">Current Counter</param>
        /// <returns>OTP</returns>
        public string GetCode(string secret, ulong counter)
        {
            return GetCodeInternal(secret, counter);
        }

        /// <summary>
        /// Checks if the passed code is valid.
        /// </summary>
        /// <param name="secret">Shared Secret</param>
        /// <param name="code">OTP</param>
        /// <param name="counter">Current Counter Position</param>
        /// <returns>true if any code from counter to counter + WindowSize matches</returns>
        public bool CheckCode(string secret, string code, ulong counter)
        {
            ulong successfulSequenceNumber = 0uL;

            return CheckCode(secret, code, counter, out successfulSequenceNumber);
        }

        /// <summary>
        /// Checks if the passed code is valid.
        /// </summary>
        /// <param name="secret">Shared Secret</param>
        /// <param name="code">OTP</param>
        /// <param name="counter">Current Counter Position</param>
        /// <param name="usedCounter">Matching counter value if successful</param>
        /// <returns>true if any code from counter to counter + WindowSize matches</returns>
        public bool CheckCode(string secret, string code, ulong counter, out ulong usedCounter)
        {
            var codeMatch = false;
            ulong successfulSequenceNumber = 0uL;

            for (uint i = 0; i <= WindowSize; i++)
            {
                ulong checkCounter = counter + i;
                if (ConstantTimeEquals(GetCode(secret, checkCounter), code))
                {
                    codeMatch = true;
                    successfulSequenceNumber = checkCounter;
                }
            }

            usedCounter = successfulSequenceNumber;
            return codeMatch;
        }
    }

    /// <summary>
    /// Manages used code to prevent repeated use of a code.
    /// </summary>
    public interface IUsedCodesManager
    {
        /// <summary>
        /// Adds secret/code pair.
        /// </summary>
        /// <param name="challenge">Used Challenge</param>
        /// <param name="code">Used Code</param>
        /// <param name="user">The user</param>
        void AddCode(long timestamp, string code, object user);

        /// <summary>
        /// Checks if code was previously used.
        /// </summary>
        /// <param name="challenge">Used Challenge</param>
        /// <param name="code">Used Code</param>
        /// <param name="user">The user</param>
        /// <returns>True if the user as already used the code</returns>
        bool IsCodeUsed(long timestamp, string code, object user);
    }

    /// <summary>
    /// Implementation of rfc6238 Time-Based One-Time Password Algorithm
    /// </summary>
    public class TimeAuthenticator : Authenticator
    {
        private readonly Func<DateTime> NowFunc;
        private readonly IUsedCodesManager UsedCodeManager;
        private readonly int IntervalSeconds;

        public TimeAuthenticator(IUsedCodesManager usedCodeManager, Func<DateTime> nowFunc, int intervalSeconds)
        {
            this.NowFunc = (nowFunc == null) ? () => DateTime.Now : nowFunc;
            this.UsedCodeManager = (usedCodeManager == null) ? new UsedCodesManager() : usedCodeManager;
            this.IntervalSeconds = intervalSeconds;
        }

        /// <summary>
        /// Generates One-Time Password.
        /// </summary>
        /// <param name="secret">Shared Secret</param>
        /// <returns>OTP</returns>
        public string GetCode(string secret)
        {
            return GetCode(secret, NowFunc());
        }

        /// <summary>
        /// Generates One-Time Password.
        /// </summary>
        /// <param name="secret">Shared Secret</param>
        /// <param name="date">Time to use as challenge</param>
        /// <returns>OTP</returns>
        public string GetCode(string secret, DateTime date)
        {
            return GetCodeInternal(secret, (ulong)GetInterval(date));
        }

        /// <summary>
        /// Checks if the passed code is valid.
        /// </summary>
        /// <param name="secret">Shared Secret</param>
        /// <param name="code">OTP</param>
        /// <param name="user">The user</param>
        /// <returns>true if code matches</returns>
        public bool CheckCode(string secret, string code, object user)
        {
            DateTime successfulTime = DateTime.MinValue;

            return CheckCode(secret, code, user, out successfulTime);
        }

        /// <summary>
        /// Checks if the passed code is valid.
        /// </summary>
        /// <param name="secret">Shared Secret</param>
        /// <param name="code">OTP</param>
        /// <param name="user">The user</param>
        /// <param name="usedDateTime">Matching time if successful</param>
        /// <returns>true if code matches</returns>
        public bool CheckCode(string secret, string code, object user, out DateTime usedDateTime)
        {
            var baseTime = NowFunc();
            DateTime successfulTime = DateTime.MinValue;

            // We need to do this in constant time
            var codeMatch = false;
            for (int i = -2; i <= 1; i++)
            {
                var checkTime = baseTime.AddSeconds(IntervalSeconds * i);
                var checkInterval = GetInterval(checkTime);

                if (ConstantTimeEquals(GetCode(secret, checkTime), code) && (user == null || !UsedCodeManager.IsCodeUsed(checkInterval, code, user)))
                {
                    codeMatch = true;
                    successfulTime = checkTime;

                    UsedCodeManager.AddCode(checkInterval, code, user);
                }
            }

            usedDateTime = successfulTime;
            return codeMatch;
        }

        /// <summary>
        /// Checks if the passed code is valid.
        /// </summary>
        /// <param name="secret">Shared Secret</param>
        /// <param name="code">OTP</param>
        /// <returns>true if code matches</returns>
        public bool CheckCode(string secret, string code)
        {
            DateTime successfulTime = DateTime.MinValue;

            return CheckCode(secret, code, null, out successfulTime);
        }

        private long GetInterval(DateTime dateTime)
        {
            TimeSpan ts = (dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return (long)ts.TotalSeconds / IntervalSeconds;
        }
    }

    /// <summary>
    /// Local, thread-save used codes manager implementation
    /// </summary>
    public class UsedCodesManager : IUsedCodesManager
    {
        internal sealed class UsedCode
        {
            public UsedCode(long timestamp, String code, object user)
            {
                this.UseDate = DateTime.Now;
                this.Code = code;
                this.Timestamp = timestamp;
                this.User = user;
            }

            internal DateTime UseDate { get; private set; }
            internal long Timestamp { get; private set; }
            internal String Code { get; private set; }
            internal object User { get; private set; }

            public override bool Equals(object obj)
            {
                if (Object.ReferenceEquals(this, obj))
                {
                    return true;
                }

                var other = obj as UsedCode;
                return (other != null) && this.Code.Equals(other.Code) && this.Timestamp.Equals(other.Timestamp) && this.User.Equals(other.User);
            }
            public override string ToString()
            {
                return String.Format("{0}: {1}", Timestamp, Code);
            }
            public override int GetHashCode()
            {
                return Code.GetHashCode() + (Timestamp.GetHashCode() + User.GetHashCode() * 17) * 17;
            }
        }

        private readonly Queue<UsedCode> codes;
        private readonly System.Threading.ReaderWriterLock rwlock = new System.Threading.ReaderWriterLock();
        private readonly TimeSpan lockingTimeout = TimeSpan.FromSeconds(5);
        private readonly System.Timers.Timer cleaner;

        public UsedCodesManager()
        {
            codes = new Queue<UsedCode>();
            cleaner = new System.Timers.Timer(TimeSpan.FromMinutes(5).TotalMilliseconds);
            cleaner.Elapsed += cleaner_Elapsed;
            cleaner.Start();
        }

        void cleaner_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timeToClean = DateTime.Now.AddMinutes(-5);

            try
            {
                rwlock.AcquireWriterLock(lockingTimeout);

                while (codes.Count > 0 && codes.Peek().UseDate < timeToClean)
                {
                    codes.Dequeue();
                }
            }
            finally
            {
                rwlock.ReleaseWriterLock();
            }
        }

        public void AddCode(long timestamp, String code, object user)
        {
            try
            {
                rwlock.AcquireWriterLock(lockingTimeout);

                codes.Enqueue(new UsedCode(timestamp, code, user));
            }
            finally
            {
                rwlock.ReleaseWriterLock();
            }
        }

        public bool IsCodeUsed(long timestamp, String code, object user)
        {
            try
            {
                rwlock.AcquireReaderLock(lockingTimeout);

                return codes.Contains(new UsedCode(timestamp, code, user));
            }
            finally
            {
                rwlock.ReleaseReaderLock();
            }
        }
    }

    public class Autenticacion
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static long AuthenticatorCode(string secret)
        {
            var key = Base32Encoding.ToBytes(secret);
            var message = DateTime.Now.Ticks / 30;
            var hmacSha1 = new System.Security.Cryptography.HMACSHA1(key);
            var hash = hmacSha1.ComputeHash(Encoding.UTF8.GetBytes(message.ToString()));
            long truncatedHash = 0L;
            StringBuilder sb = new StringBuilder();
            for (int i = hash.Length - 4; i < hash.Length; i++)
            {
                sb.Append(hash[i]);
            }

            truncatedHash = long.Parse(sb.ToString());
            long code = truncatedHash % 1000000L;

            //pad code with 0 until length of code is 6
            return code;
        }
    }
}

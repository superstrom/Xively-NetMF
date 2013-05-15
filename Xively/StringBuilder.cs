using System;

namespace Xively
{
#if MF_FRAMEWORK_VERSION_V4_1
    [Obsolete("NETMF 4.1 Provides a proper implementation of this.")]
    internal class StringBuilder
    {
        private string buffer;

        public StringBuilder(string p)
        {
            this.buffer = p;
        }

        public StringBuilder Append(string value)
        {
            buffer += value;
            return this;
        }

        public StringBuilder Append(double value)
        {
            buffer += value.ToString();
            return this;
        }
    }
#endif
}

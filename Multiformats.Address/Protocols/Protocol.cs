using System;

namespace Multiformats.Address.Protocols
{
    public abstract class Protocol : IEquatable<Protocol>
    {
        public string Name { get; }
        public int Code { get; }
        public int Size { get; }
        public object Value { get; protected set; }

        protected Protocol(string name, int code, int size)
        {
            Name = name;
            Code = code;
            Size = size;
        }

        public abstract void Decode(string value);
        public abstract void Decode(byte[] bytes);
        public abstract byte[] ToBytes();

        public bool Equals(Protocol other)
        {
            var eq = Name.Equals(other.Name) &&
                   Code.Equals(other.Code) &&
                   Size.Equals(other.Size) &&
                   Value.Equals(other.Value);

            return eq;
        }

        public override bool Equals(object obj) => Equals((Protocol)obj);

        public override string ToString() => Value?.ToString() ?? string.Empty;
        public override int GetHashCode() => Value?.GetHashCode() ?? Code ^ Size;
    }
}

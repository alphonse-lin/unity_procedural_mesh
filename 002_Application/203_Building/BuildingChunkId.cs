using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.BuildingClass
{
    public struct BuildingChunkId : IEquatable<BuildingChunkId>
    {
        public readonly int Count;

        public BuildingChunkId(int count)
        {
            Count = count;
        }

        public static BuildingChunkId FromWorldPos(int count)
        {
            return new BuildingChunkId(count>>4);
        }

        #region Equality members
        public bool Equals( BuildingChunkId other)
        {
            return Count == other.Count;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is BuildingChunkId other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Count*397)^Count;
                return hashCode;
            }
        }

        public static bool operator == (BuildingChunkId left, BuildingChunkId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BuildingChunkId left, BuildingChunkId right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}

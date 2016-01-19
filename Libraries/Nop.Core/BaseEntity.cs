using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nop.Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace Nop.Core
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    [BsonIgnoreExtraElements]
    public abstract partial class BaseEntity
    {
        public BaseEntity()
        {
            GenericAttributes = new List<GenericAttribute>();
        }
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>        
        //[BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public string _id { get; set; }
        //MongoDB.Bson.ObjectId

        public int Id { get; set; }

        public IList<GenericAttribute> GenericAttributes { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        private static bool IsTransient(BaseEntity obj)
        {
            return obj != null && Equals(obj.Id, default(int));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(int)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }
    }
}
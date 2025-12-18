using System;
using BCrypt.Net;

namespace LivriaBackend.IAM.Domain.Model.ValueObjects
{
    public class PasswordHash : IEquatable<PasswordHash>
    {
        public string HashedValue { get; private set; } 
        
        private PasswordHash() { }
        
        public PasswordHash(string plainTextPassword)
        {
            if (string.IsNullOrWhiteSpace(plainTextPassword))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(plainTextPassword));
            }
            HashedValue = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
        }
        
        public bool Verify(string plainTextPassword)
        {
            if (string.IsNullOrWhiteSpace(plainTextPassword))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(HashedValue))
            {
                return false;
            }
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, HashedValue);
        }
        
        public bool Equals(PasswordHash other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return HashedValue == other.HashedValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PasswordHash)obj);
        }

        public override int GetHashCode()
        {
            return (HashedValue != null ? HashedValue.GetHashCode() : 0);
        }

        public static bool operator ==(PasswordHash left, PasswordHash right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PasswordHash left, PasswordHash right)
        {
            return !Equals(left, right);
        }
        
        public override string ToString()
        {
            return "PasswordHash [HashedValue=*****]";
        }
        
        public bool Matches(string plainTextPassword)
        {
            if (string.IsNullOrWhiteSpace(plainTextPassword))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(HashedValue))
            {
                return false;
            }
            
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, HashedValue);
        }
    }
}
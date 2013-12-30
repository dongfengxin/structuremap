﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace StructureMap.Building
{
    public class ReferencedDependencySource : IDependencySource
    {
        private readonly Type _dependencyType;
        private readonly string _name;

        public static MethodInfo SessionMethod =
            typeof(IBuildSession).GetMethod("GetInstance", new Type[]{typeof(string)});

        public ReferencedDependencySource(Type dependencyType, string name)
        {
            _dependencyType = dependencyType;
            _name = name;
        }

        public Type DependencyType
        {
            get { return _dependencyType; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Description { get; private set; }
        public Expression ToExpression(ParameterExpression session)
        {
            return Expression.Call(session, SessionMethod.MakeGenericMethod(_dependencyType), Expression.Constant(_name));
        }

        public override string ToString()
        {
            return string.Format("DependencyType: {0}, Name: {1}", _dependencyType, _name);
        }

        protected bool Equals(ReferencedDependencySource other)
        {
            return Equals(_dependencyType, other._dependencyType) && string.Equals(_name, other._name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ReferencedDependencySource) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_dependencyType != null ? _dependencyType.GetHashCode() : 0)*397) ^ (_name != null ? _name.GetHashCode() : 0);
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace Hippopotamus.Engine.Core.Entities
{
    public class EntityFilter
    {
        public static EntityFilter Empty { get; } = new EntityFilter();

        public HashSet<Type> Any { get; private set; }
        public HashSet<Type> All { get; private set; }
        public HashSet<Type> None { get; private set; }

        private EntityFilter()
        {
            Any = new HashSet<Type>();
            All = new HashSet<Type>();
            None = new HashSet<Type>();
        }

        public static EntityFilter AnyOf(params Type[] types)
        {
            if (types == null) return Empty;

            EntityFilter filter = new EntityFilter
            {
                Any = new HashSet<Type>(types)
            };

            return filter;
        }

        public static EntityFilter AllOf(params Type[] types)
        {
            if (types == null) return Empty;

            EntityFilter filter = new EntityFilter
            {
                All = new HashSet<Type>(types)
            };

            return filter;
        }

        public static EntityFilter NoneOf(params Type[] types)
        {
            if (types == null) return Empty;

            EntityFilter filter = new EntityFilter
            {
                None = new HashSet<Type>(types)
            };

            return filter;
        }
    }
}

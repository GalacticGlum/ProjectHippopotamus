using System;
using System.Linq;
using System.Collections.Generic;

using Hippopotamus.Engine.Core;

namespace Hippopoutamus.Engine.Core
{
    public class Constraint 
    {
        protected readonly HashSet<Type> Mask;

        public Constraint(params Type[] constrains)
        {
            Mask = new HashSet<Type>(constrains);
        }

        internal void Add(Type type)
        {
            if (!Mask.Contains(type))
            {
                Mask.Add(type);
            }
        }

        internal bool MeetsMaskCriteria(GameObject gameObject)
        {
            return gameObject != null && gameObject.HasAnyComponent(Mask.ToArray());
        }

        internal Component[] Compatible(GameObject gameObject)
        {
            if (!MeetsMaskCriteria(gameObject)) return null;

            Component[] result = new Component[Mask.Count - 1];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = gameObject.GetComponent(Mask.ElementAt(i));
            }

            return result;
        }
    }
}

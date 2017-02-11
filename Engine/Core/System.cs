using Hippopoutamus.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Core
{
    public abstract class System
    {
        public bool Enabled { get; set; }
        public Constraint Constraint { get; protected set; }

        protected System()
        {
            Constraint = new Constraint();
            Enabled = true;
        }

        public virtual void Start() { }

        public virtual void Update(GameTime gameTime) { }
        public virtual void PhysicsUpdate(float stepTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void Dispose() { }
    }

    public abstract class System<T1> : System where T1 : Component
    {
        protected System()
        {
            Constraint = new Constraint(typeof(T1));
        }
    }

    public abstract class System<T1, T2> : System 
        where T1 : Component
        where T2 : Component
    {
        protected System()
        {
            Constraint = new Constraint(typeof(T1),
                typeof(T2));
        }
    }

    public abstract class System<T1, T2, T3> : System
        where T1 : Component
        where T2 : Component
        where T3 : Component
    {
        protected System()
        {
            Constraint = new Constraint(typeof(T1),
                typeof(T2),
                typeof(T3));
        }
    }

    public abstract class System<T1, T2, T3, T4> : System
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
    {
        protected System()
        {
            Constraint = new Constraint(typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4));
        }
    }

    public abstract class System<T1, T2, T3, T4, T5> : System
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
        where T5 : Component
    {
        protected System()
        {
            Constraint = new Constraint(typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4),
                typeof(T5));
        }
    }

    public abstract class System<T1, T2, T3, T4, T5, T6> : System
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
        where T5 : Component
        where T6 : Component
    {
        protected System()
        {
            Constraint = new Constraint(typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4),
                typeof(T5),
                typeof(T6));
        }
    }
}

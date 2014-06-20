using System;
using System.Collections.Generic;
using flashpunk.masks;

namespace flashpunk
{
    /// <summary>
    /// Base class for Entity collision masks.
    /// </summary>
    public class Mask
    {
        /// <summary>
        /// The parent Entity of this mask.
        /// </summary>
        public Entity parent;

        /// <summary>
        /// The parent Masklist of the mask.
        /// </summary>
        public Masklist list;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mask"/> class.
        /// </summary>
        public Mask()
        {
            _class = GetType();
            _check[typeof(Mask)] = collideMask;
            _check[typeof(Masklist)] = mask => collideMasklist((Masklist)mask);
        }

        /// <summary>
        /// Collides the specified mask.
        /// </summary>
        /// <param name="mask">The other Mask to check against.</param>
        /// <returns>If the Masks overlap.</returns>
        public virtual bool collide(Mask mask)
        {
            if (_check.ContainsKey(mask._class)) return _check[mask._class](mask);
            if (mask._check.ContainsKey(_class)) return mask._check[_class](this);
            return false;
        }

        /// <summary>
        /// Collide against an Entity.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        private bool collideMask(Mask other)
        {
            return parent.x - parent.originX + parent.width > other.parent.x - other.parent.originX
                && parent.y - parent.originY + parent.height > other.parent.y - other.parent.originY
                && parent.x - parent.originX < other.parent.x - other.parent.originX + other.parent.width
                && parent.y - parent.originY < other.parent.y - other.parent.originY + other.parent.height;
        }

        /// <summary>
        /// Collide against a Masklist.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        protected virtual bool collideMasklist(Masklist other)
        {
            return other.collide(this);
        }

        /// <summary>
        /// Assigns the mask to the parent.
        /// </summary>
        /// <param name="parent">The parent.</param>
        internal void assignTo(Entity parent)
        {
            this.parent = parent;
            if (parent != null) update();
        }

        /// <summary>
        /// Updates the parent's bounds for this mask.
        /// </summary>
        protected virtual void update()
        {

        }

        // Mask information.
        private Type _class;
        protected Dictionary<Type, Func<Mask, bool>> _check = new Dictionary<Type, Func<Mask, bool>>();
    }
}

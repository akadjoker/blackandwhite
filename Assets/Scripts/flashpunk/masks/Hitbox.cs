using UnityEngine;

namespace flashpunk.masks
{
    /// <summary>
    /// Uses parent's hitbox to determine collision. This class is used
    /// internally by FlashPunk, you don't need to use this class because
    /// this is the default behaviour of Entities without a Mask object.
    /// </summary>
    public class Hitbox : Mask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hitbox"/> class.
        /// </summary>
        public Hitbox()
            : this(1, 1, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hitbox"/> class.
        /// </summary>
        /// <param name="width">Width of the hitbox.</param>
        /// <param name="height">Height of the hitbox.</param>
        public Hitbox(int width, int height)
            : this(width, height, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hitbox"/> class.
        /// </summary>
        /// <param name="width">Width of the hitbox.</param>
        /// <param name="height">Height of the hitbox.</param>
        /// <param name="x">X offset of the hitbox.</param>
        /// <param name="y">Y offset of the hitbox.</param>
        public Hitbox(int width, int height, int x, int y)
        {
            _width = width;
            _height = height;
            _x = x;
            _y = y;
            _check[typeof(Mask)] = collideMask;
            _check[typeof(Hitbox)] = mask => collideHitbox((Hitbox)mask);
        }

        /// <summary>
        /// Collide against an Entity.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        private bool collideMask(Mask other)
        {
            return parent.x + _x + _width > other.parent.x - other.parent.originX
                && parent.y + _y + _height > other.parent.y - other.parent.originY
                && parent.x + _x < other.parent.x - other.parent.originX + other.parent.width
                && parent.y + _y < other.parent.y - other.parent.originY + other.parent.height;
        }

        /// <summary>
        /// Collides the hitbox.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        private bool collideHitbox(Hitbox other)
        {
            return parent.x + _x + _width > other.parent.x + other._x
                && parent.y + _y + _height > other.parent.y + other._y
                && parent.x + _x < other.parent.x + other._x + other._width
                && parent.y + _y < other.parent.y + other._y + other._height;
        }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>The x.</value>
        public int x
        {
            get { return _x; }
            set
            {
                if (_x == value) return;
                _x = value;
                if (list != null) list.update();
                else if (parent != null) update();
            }
        }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
        public int y
        {
            get { return _y; }
            set
            {
                if (_y == value) return;
                _y = value;
                if (list != null) list.update();
                else if (parent != null) update();
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int width
        {
            get { return _width; }
            set
            {
                if (_width == value) return;
                _width = value;
                if (list != null) list.update();
                else if (parent != null) update();
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int height
        {
            get { return _height; }
            set
            {
                if (_height == value) return;
                _height = value;
                if (list != null) list.update();
                else if (parent != null) update();
            }
        }

        /// <summary>
        /// Updates the parent's bounds for this mask.
        /// </summary>
        /// Updates the parent's bounds for this mask.
        override protected void update()
        {
            // update entity bounds
            parent.originX = -_x;
            parent.originY = -_y;
            parent.width = _width;
            parent.height = _height;

            // update parent list
            if (list != null) list.update();
        }

        // Hitbox information.
        internal int _width;
        internal int _height;
        internal int _x;
        internal int _y;
    }
}

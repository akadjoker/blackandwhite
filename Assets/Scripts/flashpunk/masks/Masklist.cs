using System.Collections.Generic;
using UnityEngine;

namespace flashpunk.masks
{
    /**
     * A Mask that can contain multiple Masks of one or various types.
     */
    public class Masklist : Hitbox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Masklist"/> class.
        /// </summary>
        /// <param name="mask">Masks to add to the list.</param>
        public Masklist(params Mask[] mask)
        {
            foreach (Mask m in mask) add(m);
        }

        /// <summary>
        /// Collides the specified mask.
        /// </summary>
        /// <param name="mask">The other Mask to check against.</param>
        /// <returns>If the Masks overlap.</returns>
        override public bool collide(Mask mask)
        {
            foreach (Mask m in _masks)
            {
                if (m.collide(mask)) return true;
            }
            return false;
        }

        /// <summary>
        /// Collide against a Masklist.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        override protected bool collideMasklist(Masklist other)
        {
            foreach (Mask a in _masks)
            {
                foreach (Mask b in other._masks)
                {
                    if (a.collide(b)) return true;
                }
            }
            return true;
        }

        /// <summary>
        /// Adds the specified mask.
        /// </summary>
        /// <param name="mask">The Mask to add.</param>
        /// <returns>The added Mask.</returns>
        public Mask add(Mask mask)
        {
            _masks.Add(mask);
            mask.list = this;
            update();
            return mask;
        }

        /// <summary>
        /// Removes the specified mask.
        /// </summary>
        /// <param name="mask">The Mask to remove.</param>
        /// <returns>The removed Mask</returns>
        public Mask remove(Mask mask)
        {
            if (_masks.Contains(mask)) return mask;
            _temp.Clear();
            foreach (Mask m in _masks)
            {
                if (m == mask)
                {
                    mask.list = null;
                    update();
                }
                else _temp.Add(m);
            }
            List<Mask> temp = _masks;
            _masks = _temp;
            _temp = temp;
            return mask;
        }

        /// <summary>
        /// Removes the Mask at the index.
        /// </summary>
        /// <param name="index">The Mask index.</param>
        public void removeAt(int index)
        {
            _temp.Clear();
            int i = _masks.Count;
            index %= i;
            while (i-- > 0)
            {
                if (i == index)
                {
                    _masks[index].list = null;
                    update();
                }
                else _temp.Add(_masks[index]);
            }
            List<Mask> temp = _masks;
            _masks = _temp;
            _temp = temp;
        }

        /// <summary>
        /// Removes all.
        /// </summary>
        public void removeAll()
        {
            foreach (Mask m in _masks) m.list = null;
            _masks.Clear(); _temp.Clear();
            update();
        }

        /// <summary>
        /// Gets a Mask from the list.
        /// </summary>
        /// <param name="index">The Mask index.</param>
        /// <returns>The Mask at the index</returns>
        public Mask getMask(int index)
        {
            return _masks[index % _masks.Count];
        }

        /// <summary>
        /// Updates the parent's bounds for this mask.
        /// </summary>
        override protected void update()
        {
            // find bounds of the contained masks
            int t = 0, l = 0, r = 0, b = 0; Hitbox h; int i = _masks.Count;
            while (i-- > 0)
            {
                if ((h = _masks[i] as Hitbox) != null)
                {
                    if (h._x < l) l = h._x;
                    if (h._y < t) t = h._y;
                    if (h._x + h._width > r) r = h._x + h._width;
                    if (h._y + h._height > b) b = h._y + h._height;
                }
            }

            // update hitbox bounds
            _x = l;
            _y = t;
            _width = r - l;
            _height = b - t;
            base.update();
        }

        /// <summary>
        /// Amount of Masks in the list.
        /// </summary>
        /// <value>The count.</value>
        public int count { get { return _masks.Count; } }

        // List information.
        private List<Mask> _masks = new List<Mask>();
        private List<Mask> _temp = new List<Mask>();
    }
}
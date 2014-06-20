using System;
using UnityEngine;

namespace flashpunk
{
    /// <summary>
    /// Updateable Tween container.
    /// </summary>
    public class Tweener : MonoBehaviour
    {
        /// <summary>
        /// Persistent Tween type, will stop when it finishes.
        /// </summary>
        public const uint PERSIST = 0;

        /// <summary>
        /// Looping Tween type, will restart immediately when it finishes.
        /// </summary>
        public const uint LOOPING = 1;

        /// <summary>
        /// Oneshot Tween type, will stop and remove itself from its core container when it finishes.
        /// </summary>
        public const uint ONESHOT = 2;

        /// <summary>
        /// If the Tweener should update.
        /// </summary>
      //  public bool TweenerIsactive = true;

        /// <summary>
        /// If the Tweener should clear on removal. For Entities, this is when they are
        /// removed from a Stage, and for Stage this is when the active Stage is switched.
        /// </summary>
        public bool autoClear = false;

  

        /// <summary>
        /// Updates the Tween container.
        /// </summary>
        public virtual void update()
        {

        }

        /// <summary>
        /// Adds the tween.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public Tween addTween(Tween t)
        {
            return addTween(t, true);
        }

        /// <summary>
        /// Adds the tween.
        /// </summary>
        /// <param name="t">The Tween to add.</param>
        /// <param name="start">if set to <c>true</c> the Tween should call start() immediately.</param>
        /// <returns>The added Tween.</returns>
        public Tween addTween(Tween t, bool start)
        {
            if (t._parent != null) throw new Exception("Cannot add a Tween object more than once.");
            t._parent = this;
            t._next = _tween;
            if (_tween != null) _tween._prev = t;
            _tween = t;
            if (start) _tween.start();
            return t;
        }

        /// <summary>
        /// Removes a Tween.
        /// </summary>
        /// <param name="t">The Tween to remove.</param>
        /// <returns>The removed Tween.</returns>
        public Tween removeTween(Tween t)
        {
            if (t._parent != this) throw new Exception("Core object does not contain Tween.");
            if (t._next != null) t._next._prev = t._prev;
            if (t._prev != null) t._prev._next = t._next;
            else _tween = t._next;
            t._next = t._prev = null;
            t._parent = null;
            t.TweenerIsactive = false;
            return t;
        }

        /// <summary>
        /// Removes all Tweens.
        /// </summary>
        public void clearTweens()
        {
            Tween t = _tween, n;
            while (t != null)
            {
                n = t._next;
                removeTween(t);
                t = n;
            }
        }

        /// <summary>
        /// Updates all contained tweens.
        /// </summary>
        public void updateTweens()
        {
            Tween t = _tween;
            while (t != null)
            {
                if (t.TweenerIsactive)
                {
                    t.update();
                    if (t._finish) t.finish();
                }
                t = t._next;
            }
        }

        // List information.
        internal Tween _tween;
    }
}
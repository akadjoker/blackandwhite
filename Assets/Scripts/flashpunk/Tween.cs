using System;
using UnityEngine;

namespace flashpunk
{
    /// <summary>
    /// Base class for all Tween objects, can be added to any Core-extended classes.
    /// </summary>
    public class Tween
    {
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
        /// If the tween should update.
        /// </summary>
    
        public bool TweenerIsactive;

        /// <summary>
        /// Tween completion callback.
        /// </summary>
        public Action complete;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tween"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="type">The type.</param>
        /// <param name="complete">The complete.</param>
        public Tween(float duration, uint type, Action complete)
            : this(duration, type, complete, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tween"/> class.
        /// </summary>
        /// <param name="duration">Duration of the tween (in seconds or frames).</param>
        /// <param name="type">Tween type, one of Tween.PERSIST (default), Tween.LOOPING, or Tween.ONESHOT.</param>
        /// <param name="complete">Optional callback for when the Tween completes.</param>
        /// <param name="ease">Optional easer to apply to the Tweened value.</param>
        public Tween(float duration, uint type, Action complete, Func<float, float> ease)
        {
            _target = duration;
            _type = type;
            this.complete = complete;
            _ease = ease;
        }

        /// <summary>
        /// Updates the Tween, called by Stage.
        /// </summary>
        public virtual void update()
        {
            _time += FP.isFixed ? 1 : FP.elapsed;
            _t = _time / _target;
            if (_ease != null && _t > 0 && _t < 1) _t = _ease(_t);
            if (_time >= _target)
            {
                _t = 1;
                _finish = true;
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void start()
        {
            _time = 0;
            if (_target == 0)
            {
                TweenerIsactive = false;
                return;
            }
            TweenerIsactive = true;
        }

        /// <summary>
        /// Called when the Tween completes.
        /// </summary>
        internal void finish()
        {
            switch (_type)
            {
                case 0:
                    _time = _target;
                    TweenerIsactive = false;
                    break;
                case 1:
                    _time %= _target;
                    _t = _time / _target;
                    if (_ease != null && _t > 0 && _t < 1) _t = _ease(_t);
                    start();
                    break;
                case 2:
                    _time = _target;
                    TweenerIsactive = false;
                    _parent.removeTween(this);
                    break;
            }
            _finish = false;
            if (complete != null) complete();
        }

        /// <summary>
        /// The completion percentage of the Tween.
        /// </summary>
        /// <value>The percent.</value>
        public float percent
        {
            get { return _time / _target; }
            set { _time = _target * value; }
        }

        /// <summary>
        /// The current time scale of the Tween (after easer has been applied).
        /// </summary>
        /// <value>The scale.</value>
        public float scale { get { return _t; } }

        // Tween information.
        private uint _type;
        protected Func<float, float> _ease;
        protected float _t = 0;

        // Timing information.
        protected float _time;
        protected float _target;

        // List information.
        internal bool _finish;
        internal Tweener _parent;
        internal Tween _prev;
        internal Tween _next;
    }
}

using System;

namespace flashpunk.tweens.misc
{
    /// <summary>
    /// Tweens a numeric value.
    /// </summary>
    public class NumTween : Tween
    {
        /// <summary>
        /// The current value.
        /// </summary>
        public float value = 0;

        public NumTween()
            : this(null, 0)
        {
        }

        public NumTween(Action complete)
            : this(complete, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumTween"/> class.
        /// </summary>
        /// <param name="complete">Optional completion callback.</param>
        /// <param name="type">Tween type.</param>
        public NumTween(Action complete, uint type)
            : base(0, type, complete)
        {
        }

        /// <summary>
        /// Tweens the specified from value.
        /// </summary>
        /// <param name="fromValue">From value.</param>
        /// <param name="toValue">To value.</param>
        /// <param name="duration">The duration.</param>
        public void tween(float fromValue, float toValue, float duration)
        {
            tween(fromValue, toValue, duration, null);
        }

        /// <summary>
        /// Tweens the value from one value to another.
        /// </summary>
        /// <param name="fromValue">Start value.</param>
        /// <param name="toValue">End value.</param>
        /// <param name="duration">Duration of the tween.</param>
        /// <param name="ease">Optional easer function.</param>
        public void tween(float fromValue, float toValue, float duration, Func<float, float> ease)
        {
            _start = value = fromValue;
            _range = toValue - value;
            _target = duration;
            _ease = ease;
            start();
        }

        /// <summary>
        /// Updates the Tween.
        /// </summary>
        override public void update()
        {
            base.update();
            value = _start + _range * _t;
        }

        // Tween information.
        private float _start;
        private float _range;
    }
}

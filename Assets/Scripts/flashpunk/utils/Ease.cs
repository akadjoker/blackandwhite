using System;
namespace flashpunk.utils 
{
	/**
	 * Static class with useful easer functions that can be used by Tweens.
	 */
	public class Ease 
	{
        /// <summary>
        /// Quadratic in.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float quadIn(float t)
		{
			return t * t;
		}

        /// <summary>
        /// Quadratic out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float quadOut(float t)
		{
			return -t * (t - 2);
		}

        /// <summary>
        /// Quadratic in and out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float quadInOut(float t)
		{
			return t <= .5f ? t * t * 2 : 1 - (--t) * t * 2;
		}

        /// <summary>
        /// Cubic in.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float cubeIn(float t)
		{
			return t * t * t;
		}

        /// <summary>
        /// Cubic out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float cubeOut(float t)
		{
			return 1 + (--t) * t * t;
		}

        /// <summary>
        /// Cubic in and out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float cubeInOut(float t)
		{
			return t <= .5f ? t * t * t * 4 : 1 + (--t) * t * t * 4;
		}

        /// <summary>
        /// Quart in.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float quartIn(float t)
		{
			return t * t * t * t;
		}

        /// <summary>
        /// Quart out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float quartOut(float t)
		{
			return 1 - (t-=1) * t * t * t;
		}

        /// <summary>
        /// Quart in and out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float quartInOut(float t)
		{
			return t <= .5f ? t * t * t * t * 8 : (1 - (t = t * 2 - 2) * t * t * t) / 2 + .5f;
		}

        /// <summary>
        /// Quint in.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float quintIn(float t)
		{
			return t * t * t * t * t;
		}

        /// <summary>
        /// Quint out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float quintOut(float t)
		{
			return (t = t - 1) * t * t * t * t + 1;
		}

        /// <summary>
        /// Quint in and out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float quintInOut(float t)
		{
			return ((t *= 2) < 1) ? (t * t * t * t * t) / 2 : ((t -= 2) * t * t * t * t + 2) / 2;
		}

        /// <summary>
        /// Sine in.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float sineIn(float t)
		{
			return (float)(-Math.Cos(PI2 * t) + 1);
		}

        /// <summary>
        /// Sine out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float sineOut(float t)
		{
			return (float)Math.Sin(PI2 * t);
		}

        /// <summary>
        /// Sine in and out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float sineInOut(float t)
		{
			return (float)(-Math.Cos(PI * t) / 2 + .5);
		}

        /// <summary>
        /// Bounce in.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float bounceIn(float t)
		{
			t = 1 - t;
			if (t < B1) return (float)(1 - 7.5625 * t * t);
			if (t < B2) return (float)(1 - (7.5625 * (t - B3) * (t - B3) + .75));
			if (t < B4) return (float)(1 - (7.5625 * (t - B5) * (t - B5) + .9375));
			return (float)(1 - (7.5625 * (t - B6) * (t - B6) + .984375));
		}

        /// <summary>
        /// Bounce out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float bounceOut(float t)
		{
			if (t < B1) return (float)(7.5625 * t * t);
			if (t < B2) return (float)(7.5625 * (t - B3) * (t - B3) + .75);
			if (t < B4) return (float)(7.5625 * (t - B5) * (t - B5) + .9375);
			return (float)(7.5625 * (t - B6) * (t - B6) + .984375);
		}

        /// <summary>
        /// Bounce in and out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float bounceInOut(float t)
		{
			if (t < .5f)
			{
				t = 1 - t * 2;
				if (t < B1) return (float)((1 - 7.5625 * t * t) / 2);
				if (t < B2) return (float)((1 - (7.5625 * (t - B3) * (t - B3) + .75)) / 2);
				if (t < B4) return (float)((1 - (7.5625 * (t - B5) * (t - B5) + .9375)) / 2);
				return (float)((1 - (7.5625 * (t - B6) * (t - B6) + .984375)) / 2);
			}
			t = t * 2 - 1;
			if (t < B1) return (float)((7.5625 * t * t) / 2 + .5);
			if (t < B2) return (float)((7.5625 * (t - B3) * (t - B3) + .75) / 2 + .5);
			if (t < B4) return (float)((7.5625 * (t - B5) * (t - B5) + .9375) / 2 + .5);
			return (float)((7.5625 * (t - B6) * (t - B6) + .984375) / 2 + .5);
		}

        /// <summary>
        /// Circle in.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float circIn(float t)
		{
			return (float)(-(Math.Sqrt(1 - t * t) - 1));
		}

        /// <summary>
        /// Circle out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float circOut(float t)
		{
			return (float)(Math.Sqrt(1 - (t - 1) * (t - 1)));
		}

        /// <summary>
        /// Circle in and out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float circInOut(float t)
		{
			return (float)(t <= .5 ? (Math.Sqrt(1 - t * t * 4) - 1) / -2 : (Math.Sqrt(1 - (t * 2 - 2) * (t * 2 - 2)) + 1) / 2);
		}

        /// <summary>
        /// Exponential in.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float expoIn(float t)
		{
			return (float)(Math.Pow(2, 10 * (t - 1)));
		}

        /// <summary>
        /// Exponential out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float expoOut(float t)
		{
			return (float)(-Math.Pow(2, -10 * t) + 1);
		}

        /// <summary>
        /// Exponential in and out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float expoInOut(float t)
		{
			return (float)(t < .5 ? Math.Pow(2, 10 * (t * 2 - 1)) / 2 : (-Math.Pow(2, -10 * (t * 2 - 1)) + 2) / 2);
		}

        /// <summary>
        /// Back in.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float backIn(float t)
		{
			return (float)(t * t * (2.70158 * t - 1.70158));
		}

        /// <summary>
        /// Back out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float backOut(float t)
		{
			return (float)(1 - (--t) * (t) * (-2.70158 * t - 1.70158));
		}

        /// <summary>
        /// Back in and out.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
		public static float backInOut(float t)
		{
			t *= 2;
			if (t < 1) return (float)(t * t * (2.70158 * t - 1.70158) / 2);
			t --;
			return (float)((1 - (--t) * (t) * (-2.70158 * t - 1.70158)) / 2 + .5);
		}
		
		// Easing constants.
		private const double PI = Math.PI;
		private const double PI2 = Math.PI / 2;
		private const double EL = 2 * PI / .45;
		private const double B1 = 1 / 2.75;
        private const double B2 = 2 / 2.75;
        private const double B3 = 1.5f / 2.75;
        private const double B4 = 2.5f / 2.75;
        private const double B5 = 2.25 / 2.75;
        private const double B6 = 2.625 / 2.75;
		
		/**
		 * Operation of in/out easers:
		 * 
		 * in(t)
		 *		return t;
		 * out(t)
		 * 		return 1 - in(1 - t);
		 * inOut(t)
		 * 		return (t <= .5f) ? in(t * 2) / 2 : out(t * 2 - 1) / 2 + .5f;
		 */
	}
}
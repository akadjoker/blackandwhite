namespace flashpunk.utils
{
    /// <summary>
    /// Template used by Spritemap to define animations. Don't create
    /// these yourself, instead you can fetch them with Spritemap's add().
    /// </summary>
    public class Anim
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Anim"/> class.
        /// </summary>
        /// <param name="name">Animation name.</param>
        /// <param name="frames">Array of frame indices to animate.</param>
        public Anim(string name, int[] frames)
            : this(name, frames, 0, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Anim"/> class.
        /// </summary>
        /// <param name="name">Animation name.</param>
        /// <param name="frames">Array of frame indices to animate.</param>
        /// <param name="frameRate">Animation speed.</param>
        public Anim(string name, int[] frames, float frameRate)
            : this(name, frames, frameRate, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Anim"/> class.
        /// </summary>
        /// <param name="name">Animation name.</param>
        /// <param name="frames">Array of frame indices to animate.</param>
        /// <param name="frameRate">Animation speed.</param>
        /// <param name="loop">if set to <c>true</c> the animation should loop..</param>
        public Anim(string name, int[] frames, float frameRate, bool loop)
        {
            _name = name;
            _frames = frames;
            _frameRate = frameRate;
            _loop = loop;
            _frameCount = frames.Length;
        }

        /// <summary>
        /// Plays the animation.
        /// </summary>
        public void play()
        {
            play(false);
        }

        /// <summary>
        /// Plays the animation.
        /// </summary>
        /// <param name="reset">if set to <c>true</c> the animation should force-restart if it is already playing..</param>
        public void play(bool reset)
        {
            _parent.play(_name, reset);
        }

        /// <summary>
        /// Name of the animation.
        /// </summary>
        public string name { get { return _name; } }

        /// <summary>
        /// Array of frame indices to animate.
        /// </summary>
        public int[] frames { get { return _frames; } }

        /// <summary>
        /// Animation speed.
        /// </summary>
        public float frameRate { get { return _frameRate; } }

        /// <summary>
        /// Amount of frames in the animation.
        /// </summary>
        public int frameCount { get { return _frameCount; } }

        /// <summary>
        /// If the animation loops.
        /// </summary>
        /// <value>
        ///   <c>true</c> if loop; otherwise, <c>false</c>.
        /// </value>
        public bool loop { get { return _loop; } }

        internal EntityAnimated _parent;
        internal string _name;
        internal int[] _frames;
        internal float _frameRate;
        internal int _frameCount;
        internal bool _loop;
    }
}

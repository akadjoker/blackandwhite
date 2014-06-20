using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace flashpunk
{
  using flashpunk.utils;
   

public class EntityAnimated : Entity {

	  /// <summary>
        /// If the animation has stopped.
        /// </summary>
        public bool complete = true;

        /// <summary>
        /// Optional callback for animation end.
        /// </summary>
        public Action callback;

        /// <summary>
        /// Animation speed factor, alter this to speed up/slow down all animations.
        /// </summary>
        public float rate = 1;

  

  



       override public void Start()
        {
              base.Start();
              active = true;
        }

   

        /** @private Updates the animation. */
        override public void update()
        {
            if (_anim != null && !complete)
            {
                _timer += (FP.isFixed ? _anim._frameRate : _anim._frameRate * FP.elapsed) * rate;
                if (_timer >= 1)
                {
                    while (_timer >= 1)
                    {
                        _timer--;
                        _index++;
                        if (_index == _anim._frameCount)
                        {
                            if (_anim._loop)
                            {
                                _index = 0;
                                if (callback != null) callback();
                            }
                            else
                            {
                                _index = _anim._frameCount - 1;
                                complete = true;
                                if (callback != null) callback();
                                break;
                            }
                        }
                    }
                    if (_anim != null) _frame = _anim._frames[_index];
       
                }
            }
        }

        /// <summary>
        /// Add an Animation.
        /// </summary>
        /// <param name="name">Name of the animation.</param>
        /// <param name="frames">Array of frame indices to animate through.</param>
        /// <returns>
        /// A new Anim object for the animation.
        /// </returns>
        public Anim add(string name, int[] frames)
        {
            return add(name, frames, 0, true);
        }

        /// <summary>
        /// Add an Animation.
        /// </summary>
        /// <param name="name">Name of the animation.</param>
        /// <param name="frames">Array of frame indices to animate through.</param>
        /// <param name="frameRate">Animation speed.</param>
        /// <returns>
        /// A new Anim object for the animation.
        /// </returns>
        public Anim add(string name, int[] frames, float frameRate)
        {
            return add(name, frames, frameRate, true);
        }

        /// <summary>
        /// Add an Animation.
        /// </summary>
        /// <param name="name">Name of the animation.</param>
        /// <param name="frames">Array of frame indices to animate through.</param>
        /// <param name="frameRate">Animation speed.</param>
        /// <param name="loop">if set to <c>true</c> the animation should loop..</param>
        /// <returns>A new Anim object for the animation.</returns>
        public Anim add(string name, int[] frames, float frameRate, bool loop)
        {
            if (_anims.ContainsKey(name)) throw new Exception("Cannot have multiple animations with the same name");
            (_anims[name] = new Anim(name, frames, frameRate, loop))._parent = this;
            return _anims[name];
        }

        /// <summary>
        /// Plays an animation.
        /// </summary>
        /// <returns>Anim object representing the played animation.</returns>
        public Anim play()
        {
            return play("", false);
        }

        /// <summary>
        /// Plays an animation.
        /// </summary>
        /// <param name="name">Name of the animation to play.</param>
        /// <returns>Anim object representing the played animation.</returns>
        public Anim play(string name)
        {
            return play(name, true);
        }

        /// <summary>
        /// Plays an animation.
        /// </summary>
        /// <param name="name">Name of the animation to play.</param>
        /// <param name="reset">if set to <c>true</c> the animation should force-restart if it is already playing..</param>
        /// <returns>Anim object representing the played animation.</returns>
        public Anim play(string name, bool reset)
        {
            if (!reset && _anim != null && _anim._name == name) return _anim;
            _anim = _anims[name];
            if (_anim == null)
            {
                _frame = _index = 0;
                complete = true;
                 return null;
            }
            _index = 0;
            _timer = 0;
            _frame = _anim._frames[0];
            complete = false;
            return _anim;
        }

        /// <summary>
        /// Gets the frame index based on the column and row of the source image.
        /// </summary>
        /// <returns>Frame index.</returns>
        public int getFrame()
        {
            return _frame;
        }

 
       



        /// <summary>
        /// Sets the current display frame based on the column and row of the source image.
        /// When you set the frame, any animations playing will be stopped to force the frame.
        /// </summary>
        /// <param name="column">Frame column.</param>
        /// <param name="row">Frame row.</param>
        public void setFrame(int frame)
        {
            if (_frame == frame) return;
            _frame = frame;
        }


        /// <summary>
        /// Assigns the Spritemap to a random frame.
        /// </summary>
        public void randFrame()
        {
            frame = (int)FP.rand((uint)_frameCount);
        }

        /// <summary>
        /// Sets the frame to the frame index of an animation.
        /// </summary>
        /// <param name="name">Animation to draw the frame frame.</param>
        /// <param name="index">Index of the frame of the animation to set to.</param>
        public void setAnimFrame(string name, int index)
        {
            int[] frames = _anims[name]._frames;
            index %= frames.Length;
            if (index < 0) index += frames.Length;
            frame = frames[index];
        }

        /// <summary>
        /// Sets the current frame index. When you set this, any
        /// animations playing will be stopped to force the frame.
        /// </summary>
        /// <value>
        /// The frame.
        /// </value>
        public int frame
        {
            get { return _frame; }
            set
            {
                _anim = null;
                value %= _frameCount;
                if (value < 0) value = _frameCount + value;
                if (_frame == value) return;
                _frame = value;
    
            }
        }

        /// <summary>
        /// Current index of the playing animation.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int index
        {
            get { return _anim != null ? _index : 0; }
            set
            {
                if (_anim == null) return;
                value %= _anim._frameCount;
                if (_index == value) return;
                _index = value;
                _frame = _anim._frames[_index];

            }
        }

        /// <summary>
        /// The amount of frames in the Spritemap.
        /// </summary>
        public int frameCount { get { return _frameCount; } }

        /// <summary>
        /// Columns in the Spritemap.
        /// </summary>
        public int columns { get { return _columns; } }

        /// <summary>
        /// Rows in the Spritemap.
        /// </summary>
        public int rows { get { return _rows; } }

        /// <summary>
        /// The currently playing animation.
        /// </summary>
        public string currentAnim { get { return _anim != null ? _anim._name : ""; } }

        // Spritemap information.
        protected int _width;
        protected int _height;
        private int _columns;
        private int _rows;
        private int _frameCount;
        private readonly Dictionary<string, Anim> _anims = new Dictionary<string, Anim>();
        private Anim _anim;
        private int _index;
        protected int _frame;
        private float _timer = 0;
    }
}

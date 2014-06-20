using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace flashpunk
{
    /// <summary>
    /// Main game Sprite class, added to the Flash Stage. Manages the game loop.
    /// </summary>
    public class Engine : MonoBehaviour
    {

        public Vector3 mouse;
        /// <summary>
        /// If the game should stop updating/rendering.
        /// </summary>
        public bool paused = false;



       // private Stage world;
        public GameObject StageObject;



        void Awake()
        {

            FP.log("create engine");

            FP.width = Screen.width;
            FP.height = Screen.height;
            FP.assignedFrameRate = 30;
            FP.isFixed = false;


            FP.camera = Camera.main.camera;
            FP.stage = null;

      
            
            if (StageObject != null)
            {

               //  FP.stage = (Stage)StageObject.GetComponentInChildren<Stage>();
                setStage((Stage)StageObject.GetComponentInChildren<Stage>());
               //  FP.stage.begin();
          

      

              //  FP.log("gameStage is OK");
            }
            else
            {
                FP.log("gameStage is null");

            }
            
           

         

            // global game objects
            FP.engine = this;

         

            // miscellanious startup stuff
            if (FP.randomSeed == 0) FP.randomizeSeed();
            FP._time = DateTime.Now.Millisecond;

            init();

        }




        void OnDestroy()
        {
        
        }

        public virtual void end()
        {

        }

        public virtual void init()
        {

        }

        void OnPostRender()
        {

            if (FP.stage != null)
            {
             
                    FP.stage.render();

            }
        }


     
        /// <summary>
        /// Updates the game, updating the Stage and Entities.
        /// </summary>
        public virtual void update()
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            FP.mouseX = (mouse.x);
            FP.mouseY = (mouse.y); 

            // timing stuff
            uint t = (uint)(DateTime.Now.Ticks / 10000);
            
            if (FP.stage != null)
            {
                if (_frameLast != 0) _frameLast = t;
           

                if (FP.stage.active)
                {
                    if (FP.stage._tween != null) FP.stage.updateTweens();
                    FP.stage.update();
                }
                FP.stage.UpdateLists();
           
            }
            else
            {
                Debug.Log("Engine:stage is null");
            }
        }

        void Update()//unity
        {
            FP.elapsed = Time.deltaTime ;
            if (!paused) update();
        }

        void OnGUI()
        {

            if (FP.stage != null)
            {

                if (FP.stage.active)
                {
                    FP.stage.show2D();
                }
            }
        }

        
  
        public void setStage(Stage st)
        {
           if (FP.stage!=null)
           {
               FP.stage.HideAll();
               FP.stage.UpdateLists();
               FP.stage.end();
               FP.stage.clearTweens();
               FP.stage = null;
               
           }else
           {
               FP.stage = st;
               if ( FP.stage.autoClear && FP.stage._tween != null) FP.stage.clearTweens();
               FP.stage.ShowAll();
               FP.stage.UpdateLists();
               FP.stage.begin();
               FP.stage.update();
               
           }
    
        }

        

 

        // Timing information.
        private double _delta = 0;
        private double _time;
        private double _last;
        private Timer _timer;
        private float _rate;
        private float _skip;
        private float _prev;

        // Debug timing information.
        private uint _updateTime;
        private uint _renderTime;
        private uint _gameTime;
        private uint _flashTime;

        // FrameRate tracking.
        private uint _frameLast = 0;
        private uint _frameListSum = 0;
        private List<uint> _frameList = new List<uint>();
    }
}
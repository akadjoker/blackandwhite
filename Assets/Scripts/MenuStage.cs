using UnityEngine;
using System.Collections;
using flashpunk;
using flashpunk.tweens.misc;
using flashpunk.utils;


public class MenuStage : Stage 
{
    private NumTween Toptween;
    private NumTween downtween;
    public GUIText fonte;


    private bool ready=false;

  //  public GameObject topObject;
    public Entity topImage;
    public Entity downImage;



    public override void begin()
    {
       FP.log("menu stage begin");

       fonte.gameObject.SetActive(false);
        
       Toptween = new NumTween(topComplete,0);
       Toptween.tween(topImage.y, 380, 1.5f,Ease.bounceOut);
       addTween(Toptween, true);

       downtween = new NumTween(topComplete, 0);
       downtween.tween(downImage.y, 126, 1.5f, Ease.bounceOut);
       addTween(downtween, true);


       fade(true, 1.5f, null);


    }
    void fadeComplete()
    {
        loadLevel("scene1");

    }
    void topComplete()
    {
        ready = true;
        fonte.gameObject.SetActive(true);
    }
    public override void update()
    {
        base.update();
      
   
            topImage.y  = Toptween.value;
            downImage.y = downtween.value;

     
                if (Input.GetMouseButton(0))
                {
                     if (ready)
                        {
                            ready = false;
                            fade(false, 1.5f, fadeComplete);
            
                        }
                }

              
            
    }
    void OnMouseUp()
    {

        print("mouse up");
      
    }
    
  


    public override void show2D()
    {

        base.show2D();    
       

    }
    public override void end()
    {
        FP.log("menu stage end");
   

    }

	
}

using UnityEngine;
using System.Collections;
using flashpunk;
using flashpunk.utils;
using System;

public class Badchip :  Entity
{
    public float acc;
    public Action isKill;
    public bool isDead=false;

    void Start()
    {
        base.Start();
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    override public void Added()
    {
      
    }


    override public void update()
    {
        base.update();

    

 
        
        Entity colide = collide("player", x, y);
        if (colide != null)
        {
           Alive = false;
           SpecialEffectsHelper.Instance.Explosion(transform.position);
        
            
        }
       
        if (y <= -height)
        {
            Alive = false;
           
        }

        if (!Alive)
        {

            if (isKill != null) isKill();
            Alive = true; 
            ObjectPool.instance.PoolObject(this.gameObject);
          
           
        }

      // y -= (acc * Time.deltaTime);
        if (Alive) body.velocity = new Vector2(0, -(acc*100) * Time.deltaTime);
    
    }
}

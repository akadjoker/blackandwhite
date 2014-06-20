using UnityEngine;
using flashpunk;
using flashpunk.utils;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity {

  
    public float  fireRate = 0.5f;
    private float nextFire ;


    

    override public void Added()
    {

        body = gameObject.GetComponent<Rigidbody2D>();

    }

    

    override public void update()
    {
        base.update();


        if (Input.GetMouseButton(0) && Time.time>nextFire)
        {
    
                nextFire=Time.time+fireRate;

         

               // PlayerBullet b = Instantiate(prefab,transform.position,Quaternion.identity) as PlayerBullet;
             //   b.y += 10;
            


        //  PlayerBullet b=   Instantiate(prefab) as PlayerBullet;
         // b.x = x;
         // b.y = y;

              

         
            GameObject bullet = ObjectPool.instance.GetObjectForType("player_b", true);
            PlayerBullet b =  (PlayerBullet)bullet.GetComponent<PlayerBullet>();       
            

               
                if (b != null)
                {
                    b.Alive = true;
                    b.x = x;
                    b.y = y;
                }
            
            
          
        }

       

        stepTowards( FP.mouseX, 5.5f);

        if (x <= 10)  x = 10;
        if (x >= 310) x = 310;
        y = 77;


    }

    public  void stepTowards( float px,  float distance)
    {

        Vector2 point;
        point.x = px - x;
        point.y = 0;
        if (point.magnitude <= distance)
        {
        //    x = px;

            return;
        }
        point *= distance;
        point.y = 0;

        body.velocity=point;
        //obj.x += point.x;
       // obj.y += point.y;
    }
}

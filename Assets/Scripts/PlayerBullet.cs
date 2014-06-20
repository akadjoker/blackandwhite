using UnityEngine;
using flashpunk;
using flashpunk.utils;


public class PlayerBullet : Entity
{



    void Start()
    {
        base.Start();
        body=gameObject.GetComponent<Rigidbody2D>();
    }

    override public void Added()
    {
        print("add bullet to scene : "+type);

     
    }


    override public void update()
    {
        base.update();






        Entity colide = collide("enemy", x, y);
        if(colide!=null)
        {
            Alive = false;
            colide.Alive = false;
           // ObjectPool.instance.PoolObject(this.gameObject);
           // ObjectPool.instance.PoolObject(colide.gameObject);

            SpecialEffectsHelper.Instance.Explosion(transform.position);
           // SpecialEffectsHelper.Instance.Sparks(transform.position);
        }

        if (y >= 480 )
        {
          
            Alive = false;
        
         
        }

        if (!Alive)
        {
     
            ObjectPool.instance.PoolObject(this.gameObject);
       
        }

       if (Alive) body.velocity = new Vector2(0, (500*100) * Time.deltaTime);
      //  if (Alive) body.AddForce(new Vector2(0, 2500 * Time.deltaTime));// y += (500f * Time.deltaTime);
    }
}

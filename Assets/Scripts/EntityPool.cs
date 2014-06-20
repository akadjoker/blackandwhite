using UnityEngine;
using System.Collections;
using flashpunk;
using System.Collections.Generic;

public class EntityPool  {

     public List<Entity> pool;
     public int counter;
	

    public EntityPool(Entity prefab,int len)
    {
           pool = new List<Entity>(len);
			counter = len;
			
			int  i = len;
            while (--i > -1)
            {
                Entity newObj = GameObject.Instantiate(prefab) as Entity;
                newObj.name = prefab.name;
                pool.Add(newObj);
            }

	
	}

    public Entity getEntity()
		{
            if (counter > 0)
                return pool[--counter];
            else
                return null;
		}
		
		public void returnEntity(Entity e)
		{
			pool[counter++] = e;
		}

	
	
}

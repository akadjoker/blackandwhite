using System;
using System.Collections.Generic;
using UnityEngine;

namespace flashpunk
{
    using flashpunk.tweens.misc;
    using flashpunk.utils;

    public class Stage : Tweener
    {

     
        public GameObject[] objectPrefabs;
        public List<GameObject>[] pooledObjects;
        public int[] amountToBuffer;
        public int defaultBufferAmount = 10;



        private NumTween fadeTween=null;
        

        private Material lineMaterial = null;
        private Vector2 f = new Vector2();
        private Vector2 v = new Vector2();
        private Vector2 lv = new Vector2();
        public bool debug = true;

        void Start()
        {
            CreateMaterial();


            pooledObjects = new List<GameObject>[objectPrefabs.Length];

            int i = 0;
            foreach (GameObject objectPrefab in objectPrefabs)
            {
                pooledObjects[i] = new List<GameObject>();

                int bufferAmount;

                if (i < amountToBuffer.Length) bufferAmount = amountToBuffer[i];
                else
                    bufferAmount = defaultBufferAmount;

                for (int n = 0; n < bufferAmount; n++)
                {
                    GameObject newObj = Instantiate(objectPrefab) as GameObject;
                    newObj.name = objectPrefab.name;
                    PoolObject(newObj);
                }

                i++;
            }
        }

        public GameObject getFromPool(string objectType,bool onlyPooled=true)
        {
            for (int i = 0; i < objectPrefabs.Length; i++)
            {
                GameObject prefab = objectPrefabs[i];
                if (prefab.name == objectType)
                {

                    if (pooledObjects[i].Count > 0)
                    {
                        GameObject pooledObject = pooledObjects[i][0];
                        pooledObjects[i].RemoveAt(0);
                        pooledObject.transform.parent = null;
                        pooledObject.SetActive(true);

                        return pooledObject;

                    }
                    else if (!onlyPooled)
                    {
                        return Instantiate(objectPrefabs[i]) as GameObject;
                    }

                    break;

                }
            }

            //If we have gotten here either there was no object of the specified type or non were left in the pool with onlyPooled set to true
            return null;
        }


        public void PoolObject(GameObject obj)
        {
            for (int i = 0; i < objectPrefabs.Length; i++)
            {
                if (objectPrefabs[i].name == obj.name)
                {
         
             
                    obj.gameObject.SetActive(false);
                    obj.transform.parent = gameObject.transform;
                    pooledObjects[i].Add(obj);
                    return;
                }
            }
        }



        void CreateMaterial()
        {
            if (lineMaterial != null)
                return;

            lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                                        "SubShader { Pass { " +
                                        "    Blend SrcAlpha OneMinusSrcAlpha " +
                                        "    ZWrite Off Cull Off Fog { Mode Off } " +
                                        "    BindChannels {" +
                                        "      Bind \"vertex\", vertex Bind \"color\", color }" +
                                        "} } }");
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }

        public NumTween fade(bool fadeIn, float duration, Action complete)
        {
            fadeTween = null;
            fadeTween = new NumTween(complete, Tween.ONESHOT);//PERSIST 
            float alpha = fadeIn ? 1f : 0f;
            float finalAlpha = fadeIn ? 0f : 1f;
            fadeTween.tween(alpha, finalAlpha, duration, Ease.sineIn);
            addTween(fadeTween, true);
            return fadeTween;
        }
   
       public void DrawQuad(Rect position, Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            GUI.skin.box.normal.background = texture;
            GUI.Box(position, GUIContent.none);
        }
        public void Bind()
        {
            lineMaterial.SetPass(0);

        }
        public void DrawPolygon(Vector2[] vertices, int count, Color color)
        {
            GL.Begin(GL.LINES);

            for (int i = 0, j = 1; i < count; i++, j++)
            {

                j = (j >= count) ? 0 : j;

                GL.Color(color);
                GL.Vertex3(vertices[i].x, vertices[i].y, 0.0f);
                GL.Color(color);
                GL.Vertex3(vertices[j].x, vertices[j].y, 0.0f);


            }



            GL.End();

        }

        public void DrawSolidPolygon(Vector2[] vertices, int count, Color color)
        {
            GL.Begin(GL.LINES);

            for (int i = 0, j = 1; i < count; i++, j++)
            {

                j = (j >= count) ? 0 : j;

                GL.Color(color);
                GL.Vertex3(vertices[i].x, vertices[i].y, 0.0f);
                GL.Color(color);
                GL.Vertex3(vertices[j].x, vertices[j].y, 0.0f);


            }



            GL.End();





        }

        private void DrawSolidPolygon(Vector2[] vertices, int count, Color color, bool outline)
        {

            if (count == 2)
            {
                DrawPolygon(vertices, count, color);
                return;
            }

            Color colorFill = color * (outline ? 0.5f : 1.0f);
            GL.Begin(GL.LINES);

            for (int i = 0, j = 1; i < count; i++, j++)
            {

                j = (j >= count) ? 0 : j;
                GL.Color(colorFill);
                GL.Vertex3(vertices[i].x, vertices[i].y, 0.0f);
                GL.Color(colorFill);
                GL.Vertex3(vertices[j].x, vertices[j].y, 0.0f);



            }



            GL.End();
            if (outline)
            {
                DrawPolygon(vertices, count, color);
            }

        }

        public void DrawCircle(Vector2 center, float radius, Color color)
        {



            int segments = 16;
            double increment = Math.PI * 2.0 / (double)segments;
            double theta = 0.0;
            GL.Begin(GL.LINES);
            for (int i = 0; i < segments; i++)
            {
                Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                Vector2 v2 = center + radius * new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));
                GL.Color(color);
                GL.Vertex3(v1.x, v1.y, 0.0f);
                GL.Color(color);
                GL.Vertex3(v2.x, v2.y, 0.0f);
                theta += increment;
            }
            GL.End();
        }
        public void DrawSegment(Vector2 p1, Vector2 p2, Color color)
        {



            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex3(p1.x, p1.y, 0.0f);
            GL.Color(color);
            GL.Vertex3(p2.x, p2.y, 0.0f);
            GL.End();


        }

        void drawBox(float x, float y, float hx, float hy,Color c)
        {
            int _vertexCount = 4;
            Vector2[] _vertices = new Vector2[_vertexCount];


            _vertices[0] = new Vector2(x + (-hx/2), y + (-hy/2));
            _vertices[1] = new Vector2(x + hx/2, y + (-hy/2));
            _vertices[2] = new Vector2(x + hx/2, y + hy/2);
            _vertices[3] = new Vector2(x + (-hx/2), y + hy/2);
            DrawSolidPolygon(_vertices, _vertexCount, c, true);

        }
        public void DrawSolidCircle(Vector2 center, float radius, Color color)
        {

            Color colorFill = color * 0.5f;


            int segments = 16;
            double increment = Math.PI * 2.0 / (double)segments;
            double theta = 0.0;
            GL.Begin(GL.LINES);
            for (int i = 0; i < segments; i++)
            {
                Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                Vector2 v2 = center + radius * new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));
                GL.Color(colorFill);
                GL.Vertex3(v1.x, v1.y, 0.0f);
                GL.Color(colorFill);
                GL.Vertex3(v2.x, v2.y, 0.0f);

                theta += increment;
            }
            GL.End();
      
        }


        /// <summary>
        /// If the render() debug
        /// </summary>
        public bool visible = true;



  

        /// <summary>
        /// Override this; called when Stage is switch to, and set to the currently active world.
        /// </summary>
        public virtual void begin()
        {

        }

        /// <summary>
        /// Override this; called when Stage is changed, and the active world is no longer this.
        /// </summary>
        public virtual void end()
        {

        }

        public void render()
        {
            if (!debug) return;
      
            Bind();
            GL.PushMatrix();

             
            Entity e = updateFirst;
            while (e != null)
            {
          
                if (e.visible && e.active)
                {
               

                        drawBox(e.x-e.originX , e.y+e.originY, e.width , e.height,Color.green );
                        DrawCircle(new Vector2((e.x + e.originX), (e.y - e.originY)), 0.1f, Color.yellow);

                  //  print(string.Format("{0},{0}", e.x, e.y));
                }
                e = e.updateNext;
            }

            
            GL.PopMatrix();


        }


        public void loadLevel(string l)
        {
            Application.LoadLevel(l);
        }

        public string levelName()
        {
            return Application.loadedLevelName;
        }


        //**
        /// <summary>
        /// Performed by the game loop, updates all contained Entities.
        /// If you override this to give your Stage update code, remember
        /// to call base.update() or your Entities will not be updated.
        /// </summary>
        override public void update()
        {


            // update the entities
            Entity e = updateFirst;
            while (e != null)
            {
                if (e.active)
                {
                    if (e._tween != null) e.updateTweens();
                     e.update();
                }
         
                e = e.updateNext;
            }
        }
            virtual public void show2D()
            {

                if (fadeTween != null)
                {
      
                      DrawQuad(new Rect(0, 0, Screen.width, Screen.height),new Color(0,0,0,fadeTween.value));
 
                }

            }

        public void ShowAll()
        {
            visible = true;
            Entity e = updateFirst;
            while (e != null)
            {
              
                    e.visible = true;
                    e.active = true;
                    e.update();
                

                e = e.updateNext;
            }
      
        }
         public void HideAll()
        {
            visible = false;
              Entity e = updateFirst;
            while (e != null)
            {
                  e.update();
           

                e = e.updateNext;
            }
        }

    
     



   

        /// <summary>
        /// Adds multiple Entities to the world.
        /// </summary>
        /// <param name="list">Several Entities (as arguments) or an Array/Vector of Entities.</param>
        public void addList(params Entity[] list)
        {
            foreach (Entity e in list) Add(e);
        }

        /// <summary>
        /// Removes multiple Entities from the world.
        /// </summary>
        /// <param name="list">Several Entities (as arguments) or an Array/Vector of Entities.</param>
        public void removeList(params Entity[] list)
        {
            foreach (Entity e in list) Remove(e);
        }



        /// <summary>
        /// Adds an Entity to the Stage with the Mask object.
        /// </summary>
        /// <param name="mask">Mask to assign the Entity.</param>
        /// <param name="type">Collision type of the Entity.</param>
        /// <returns>The Entity that was added.</returns>
        public Entity addMask(Mask mask, string type)
        {
            return addMask(mask, type, 0, 0);
        }

        /// <summary>
        /// Adds an Entity to the Stage with the Mask object.
        /// </summary>
        /// <param name="mask">Mask to assign the Entity.</param>
        /// <param name="type">Collision type of the Entity.</param>
        /// <param name="x">X position of the Entity.</param>
        /// <param name="y">Y position of the Entity.</param>
        /// <returns>The Entity that was added.</returns>
        public Entity addMask(Mask mask, string type, int x, int y)
        {
            Entity e = new Entity();
            if (!FP.IsNullOrWhiteSpace(type)) e.type = type;
            e.active = e.visible = false;
            return Add(e);
        }

       


     



        /// <summary>
        /// Returns the first Entity that collides with the rectangular area.
        /// </summary>
        /// <param name="type">The Entity type to check for.</param>
        /// <param name="rX">X position of the rectangle.</param>
        /// <param name="rY">Y position of the rectangle.</param>
        /// <param name="rWidth">Width of the rectangle.</param>
        /// <param name="rHeight">Height of the rectangle.</param>
        /// <returns>The first Entity to collide, or null if none collide.</returns>
        public Entity collideRect(string type, float rX, float rY, float rWidth, float rHeight)
        {
            Entity e = typeFirst[type];
            while (e != null)
            {
                if (e.collideRect(e.x, e.y, rX, rY, rWidth, rHeight)) return e;
                e = e.typeNext;
            }
            return null;
        }

        /// <summary>
        /// Returns the first Entity found that collides with the position.
        /// </summary>
        /// <param name="type">The Entity type to check for.</param>
        /// <param name="pX">X position.</param>
        /// <param name="pY">Y position.</param>
        /// <returns>The collided Entity, or null if none collide.</returns>
        public Entity collidePoint(string type, float pX, float pY)
        {
            Entity e = typeFirst[type];
            while (e != null)
            {
                if (e.collidePoint(e.x, e.y, pX, pY)) return e;
                e = e.typeNext;
            }
            return null;
        }

        /// <summary>
        /// Returns the first Entity found that collides with the line.
        /// </summary>
        /// <param name="type">The Entity type to check for.</param>
        /// <param name="fromX">Start x of the line.</param>
        /// <param name="fromY">Start y of the line.</param>
        /// <param name="toX">End x of the line.</param>
        /// <param name="toY">End y of the line.</param>
        /// <returns>Returns the first Entity found that collides with the line.</returns>
        public Entity collideLine(string type, int fromX, int fromY, int toX, int toY)
        {
            return collideLine(type, fromX, fromY, toX, toY, 1, Vector2.zero);
        }

        /// <summary>
        /// Returns the first Entity found that collides with the line.
        /// </summary>
        /// <param name="type">The Entity type to check for.</param>
        /// <param name="fromX">Start x of the line.</param>
        /// <param name="fromY">Start y of the line.</param>
        /// <param name="toX">End x of the line.</param>
        /// <param name="toY">End y of the line.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>Returns the first Entity found that collides with the line.</returns>
        public Entity collideLine(string type, int fromX, int fromY, int toX, int toY, uint precision)
        {
            return collideLine(type, fromX, fromY, toX, toY, precision, Vector2.zero);
        }

        /// <summary>
        /// Returns the first Entity found that collides with the line.
        /// </summary>
        /// <param name="type">The Entity type to check for.</param>
        /// <param name="fromX">Start x of the line.</param>
        /// <param name="fromY">Start y of the line.</param>
        /// <param name="toX">End x of the line.</param>
        /// <param name="toY">End y of the line.</param>
        /// <param name="precision">The precision.</param>
        /// <param name="p">The point.</param>
        /// <returns>Returns the first Entity found that collides with the line.</returns>
        public Entity collideLine(string type, int fromX, int fromY, int toX, int toY, uint precision, Vector2 p)
        {
            // If the distance is less than precision, do the short sweep.
            if (precision < 1) precision = 1;
            if (FP.distance(fromX, fromY, toX, toY) < precision)
            {
                if (p != null)
                {
                    if (fromX == toX && fromY == toY)
                    {
                        p.x = toX; p.y = toY;
                        return collidePoint(type, toX, toY);
                    }
                    return collideLine(type, fromX, fromY, toX, toY, 1, p);
                }
                else return collidePoint(type, fromX, toY);
            }

            // Get information about the line we're about to raycast.
            int xDelta = Math.Abs(toX - fromX);
            int yDelta = Math.Abs(toY - fromY);
            float xSign = toX > fromX ? precision : -precision;
            float ySign = toY > fromY ? precision : -precision;
            float x = fromX; float y = fromY; Entity e;

            // Do a raycast from the start to the end point.
            if (xDelta > yDelta)
            {
                ySign *= yDelta / xDelta;
                if (xSign > 0)
                {
                    while (x < toX)
                    {
                        if ((e = collidePoint(type, x, y)) != null)
                        {
                            if (p == null) return e;
                            if (precision < 2)
                            {
                                p.x = x - xSign; p.y = y - ySign;
                                return e;
                            }
                            return collideLine(type, (int)(x - xSign), (int)(y - ySign), toX, toY, 1, p);
                        }
                        x += xSign; y += ySign;
                    }
                }
                else
                {
                    while (x > toX)
                    {
                        if ((e = collidePoint(type, x, y)) != null)
                        {
                            if (p == null) return e;
                            if (precision < 2)
                            {
                                p.x = x - xSign; p.y = y - ySign;
                                return e;
                            }
                            return collideLine(type, (int)(x - xSign), (int)(y - ySign), toX, toY, 1, p);
                        }
                        x += xSign; y += ySign;
                    }
                }
            }
            else
            {
                xSign *= xDelta / yDelta;
                if (ySign > 0)
                {
                    while (y < toY)
                    {
                        if ((e = collidePoint(type, x, y)) != null)
                        {
                            if (p == null) return e;
                            if (precision < 2)
                            {
                                p.x = x - xSign; p.y = y - ySign;
                                return e;
                            }
                            return collideLine(type, (int)(x - xSign), (int)(y - ySign), toX, toY, 1, p);
                        }
                        x += xSign; y += ySign;
                    }
                }
                else
                {
                    while (y > toY)
                    {
                        if ((e = collidePoint(type, x, y)) != null)
                        {
                            if (p == null) return e;
                            if (precision < 2)
                            {
                                p.x = x - xSign; p.y = y - ySign;
                                return e;
                            }
                            return collideLine(type, (int)(x - xSign), (int)(y - ySign), toX, toY, 1, p);
                        }
                        x += xSign; y += ySign;
                    }
                }
            }

            // Check the last position.
            if (precision > 1)
            {
                if (p == null) return collidePoint(type, toX, toY);
                if (collidePoint(type, toX, toY) != null) return collideLine(type, (int)(x - xSign), (int)(y - ySign), toX, toY, 1, p);
            }

            // No collision, return the end point.
            if (p != null)
            {
                p.x = toX;
                p.y = toY;
            }
            return null;
        }

        /**
         * Populates an array with all Entities that collide with the rectangle. This
         * does not empty the array, that responsibility is left to the user.
         * @param	type		The Entity type to check for.
         * @param	rX			X position of the rectangle.
         * @param	rY			Y position of the rectangle.
         * @param	rWidth		Width of the rectangle.
         * @param	rHeight		Height of the rectangle.
         * @param	into		The Array or Vector to populate with collided Entities.
         */
        public void collideRectInto(string type, float rX, float rY, float rWidth, float rHeight, List<Entity> into)
        {
            Entity e = typeFirst[type];
            while (e != null)
            {
                if (e.collideRect(e.x, e.y, rX, rY, rWidth, rHeight)) into.Add(e);
                e = e.typeNext;
            }
        }

        /**
         * Populates an array with all Entities that collide with the position. This
         * does not empty the array, that responsibility is left to the user.
         * @param	type		The Entity type to check for.
         * @param	pX			X position.
         * @param	pY			Y position.
         * @param	into		The Array or Vector to populate with collided Entities.
         * @return	The provided Array.
         */
        public void collidePointInto(string type, float pX, float pY, List<Entity> into)
        {
            Entity e = typeFirst[type];
            while (e != null)
            {
                if (e.collidePoint(e.x, e.y, pX, pY)) into.Add(e);
                e = e.typeNext;
            }
        }

        /**
         * Finds the Entity nearest to the rectangle.
         * @param	type		The Entity type to check for.
         * @param	x			X position of the rectangle.
         * @param	y			Y position of the rectangle.
         * @param	width		Width of the rectangle.
         * @param	height		Height of the rectangle.
         * @return	The nearest Entity to the rectangle.
         */
        public Entity nearestToRect(string type, float x, float y, float width, float height)
        {
            Entity n = typeFirst[type];
            float nearDist = float.MaxValue;
            Entity near = null; float dist;
            while (n != null)
            {
                dist = squareRects(x, y, width, height, n.x - n.originX, n.y - n.originY, n.width, n.height);
                if (dist < nearDist)
                {
                    nearDist = dist;
                    near = n;
                }
                n = n.typeNext;
            }
            return near;
        }

        public Entity nearestToEntity(string type, Entity e)
        {
            return nearestToEntity(type, e, false);
        }

        /**
         * Finds the Entity nearest to another.
         * @param	type		The Entity type to check for.
         * @param	e			The Entity to find the nearest to.
         * @param	useHitboxes	If the Entities' hitboxes should be used to determine the distance. If false, their x/y coordinates are used.
         * @return	The nearest Entity to e.
         */
        public Entity nearestToEntity(string type, Entity e, bool useHitboxes)
        {
            if (useHitboxes) return nearestToRect(type, e.x - e.originX, e.y - e.originY, e.width, e.height);
            Entity n = typeFirst[type];
            float nearDist = float.MaxValue;
            Entity near = null; float dist;
            float x = e.x - e.originX;
            float y = e.y - e.originY;
            while (n != null)
            {
                dist = (x - n.x) * (x - n.x) + (y - n.y) * (y - n.y);
                if (dist < nearDist)
                {
                    nearDist = dist;
                    near = n;
                }
                n = n.typeNext;
            }
            return near;
        }

        public Entity nearestToPoint(string type, float x, float y)
        {
            return nearestToPoint(type, x, y, false);
        }

        /**
         * Finds the Entity nearest to the position.
         * @param	type		The Entity type to check for.
         * @param	x			X position.
         * @param	y			Y position.
         * @param	useHitboxes	If the Entities' hitboxes should be used to determine the distance. If false, their x/y coordinates are used.
         * @return	The nearest Entity to the position.
         */
        public Entity nearestToPoint(string type, float x, float y, bool useHitboxes)
        {
            Entity n = typeFirst[type];
            float nearDist = float.MaxValue;
            Entity near = null; float dist;
            if (useHitboxes)
            {
                while (n != null)
                {
                    dist = squarePointRect(x, y, n.x - n.originX, n.y - n.originY, n.width, n.height);
                    if (dist < nearDist)
                    {
                        nearDist = dist;
                        near = n;
                    }
                    n = n.typeNext;
                }
                return near;
            }
            while (n != null)
            {
                dist = (x - n.x) * (x - n.x) + (y - n.y) * (y - n.y);
                if (dist < nearDist)
                {
                    nearDist = dist;
                    near = n;
                }
                n = n.typeNext;
            }
            return near;
        }





       
    


       


    

     
      
    

        /** @private Calculates the squared distance between two rectangles. */
        private static float squareRects(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
        {
            if (x1 < x2 + w2 && x2 < x1 + w1)
            {
                if (y1 < y2 + h2 && y2 < y1 + h1) return 0;
                if (y1 > y2) return (y1 - (y2 + h2)) * (y1 - (y2 + h2));
                return (y2 - (y1 + h1)) * (y2 - (y1 + h1));
            }
            if (y1 < y2 + h2 && y2 < y1 + h1)
            {
                if (x1 > x2) return (x1 - (x2 + w2)) * (x1 - (x2 + w2));
                return (x2 - (x1 + w1)) * (x2 - (x1 + w1));
            }
            if (x1 > x2)
            {
                if (y1 > y2) return squarePoints(x1, y1, (x2 + w2), (y2 + h2));
                return squarePoints(x1, y1 + h1, x2 + w2, y2);
            }
            if (y1 > y2) return squarePoints(x1 + w1, y1, x2, y2 + h2);
            return squarePoints(x1 + w1, y1 + h1, x2, y2);
        }

        /** @private Calculates the squared distance between two points. */
        private static float squarePoints(float x1, float y1, float x2, float y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }

        /** @private Calculates the squared distance between a rectangle and a point. */
        private static float squarePointRect(float px, float py, float rx, float ry, float rw, float rh)
        {
            if (px >= rx && px <= rx + rw)
            {
                if (py >= ry && py <= ry + rh) return 0;
                if (py > ry) return (py - (ry + rh)) * (py - (ry + rh));
                return (ry - py) * (ry - py);
            }
            if (py >= ry && py <= ry + rh)
            {
                if (px > rx) return (px - (rx + rw)) * (px - (rx + rw));
                return (rx - px) * (rx - px);
            }
            if (px > rx)
            {
                if (py > ry) return squarePoints(px, py, rx + rw, ry + rh);
                return squarePoints(px, py, rx + rw, ry);
            }
            if (py > ry) return squarePoints(px, py, rx, ry + rh);
            return squarePoints(px, py, rx, ry);
        }

        /**
     * Performed by the game loop, renders all contained Entities.
     * If you override this to give your World render code, remember
     * to call super.render() or your Entities will not be rendered.
     */

        /*
        public virtual void Render()
        {
            // render the entities in order of depth
            Entity e;
            int i = layerList.Count;
            while (i > 0)
            {
                i--;
                e = renderLast[layerList[i]] as Entity;
                while (e != null)
                {
                    //if (e.Visible) e.Render();
                    e = e.renderPrev;
                }
            }
        }
        */
        /**
         * Performed by the game loop, updates all contained Entities.
         * If you override this to give your World update code, remember
         * to call super.update() or your Entities will not be updated.
         */
        virtual public void Update()
        {
            // update the entities
            Entity e = updateFirst;
            while (e != null)
            {
                if (e.gameObject.active)
                {
                    if (e._tween!=null) e.updateTweens();
                    e.update();
                }
          
                e = e.updateNext;
            }
        }

        /**
         * Adds the Entity to the World at the end of the frame.
         * @param	e		Entity object you want to add.
         * @return	The added Entity object.
         */
        public Entity Add(Entity e)
        {
            toAdd.Add(e);
            return e;
        }

    

        /**
         * Removes the Entity from the World at the end of the frame.
         * @param	e		Entity object you want to remove.
         * @return	The removed Entity object.
         */
        public Entity Remove(Entity e)
        {
            toRemove.Add(e);
            return e;
        }

        /**
         * Removes all Entities from the World at the end of the frame.
         */
        public void RemoveAll()
        {
            Entity e = updateFirst;
            while (e != null)
            {
                toRemove.Add(e);
                e = e.updateNext;
            }
        }


        /**
         * Updates the add/remove lists at the end of the frame.
         */
        public void UpdateLists()
        {
            // remove entities


            if (toRemove.Count > 0)
            {
                foreach (Entity e in toRemove)
                {
                    if (e.stage == null)
                    {
                        if (toAdd.Contains(e))
                        {
                            toAdd.Remove(e);
                        }
                        continue;
                    }
                    if (e.stage != this)
                        continue;

                    e.Removed();
                    e._stage = null;

                    removeUpdate(e);
         
                    if (e.type != null) removeType(e);
                    //if (e._name) unregisterName(e);
                    //if (e.autoClear && e._tween!=null) e.clearTweens();
                }
                toRemove.Clear();
            }

            // add entities
            if (toAdd.Count > 0)
            {
                foreach (Entity e in toAdd)
                {
                    if (e.stage != null) continue;

                    addUpdate(e);
            

                    if (e.type != null) addType(e);
                    //if (e._name) registerName(e);

                    e._stage = this;
                    e.Added();
                }
                toAdd.Clear();
            }

            // recycle entities
            /*
            if (_recycle.length)
            {
                for each (e in _recycle)
                {
                    if (e._world || e._recycleNext)
                        continue;
					
                    e._recycleNext = _recycled[e._class];
                    _recycled[e._class] = e;
                }
                _recycle.length = 0;
            }
            */

            // sort the depth list
            if (layerSort)
            {
                if (layerList.Count > 1) layerList.Sort();
                layerSort = false;
            }
        }

    



        /** @private Adds Entity to the update list. */
        private void addUpdate(Entity e)
        {
            // add to update list
            if (updateFirst != null)
            {
                updateFirst.updatePrev = e;
                e.updateNext = updateFirst;
            }
            else e.updateNext = null;
            e.updatePrev = null;
            updateFirst = e;
            count++;
            /* TO-DO class count*/
        }

        /** @private Removes Entity from the update list. */
        private void removeUpdate(Entity e)
        {
            // remove from the update list
            if (updateFirst == e) updateFirst = e.updateNext;
            if (e.updateNext != null) e.updateNext.updatePrev = e.updatePrev;
            if (e.updatePrev != null) e.updatePrev.updateNext = e.updateNext;
            e.updateNext = e.updatePrev = null;

            count--;
            //classCount[e.class] --;
        }

        /** @private Adds Entity to the type list. */
        internal void addType(Entity e)
        {
            // add to type list
            if (typeFirst.ContainsKey(e.type) && typeFirst[e.type] != null)
            {
                typeFirst[e.type].typePrev = e;
                e.typeNext = typeFirst[e.type];
                typeCount[e.type]++;
            }
            else
            {
                e.typeNext = null;
                typeCount[e.type] = 1;
            }
            e.typePrev = null;
            typeFirst[e.type] = e;
        }

        /** @private Removes Entity from the type list. */
        internal void removeType(Entity e)
        {
            // remove from the type list
            if (typeFirst[e.type] == e) typeFirst[e.type] = e.typeNext;
            if (e.typeNext != null) e.typeNext.typePrev = e.typePrev;
            if (e.typePrev != null) e.typePrev.typeNext = e.typeNext;
            e.typeNext = e.typePrev = null;
            typeCount[e.type]--;
        }

        /**
         * Returns the amount of Entities of the type are in the World.
         * @param	type		The type (or Class type) to count.
         * @return	How many Entities of type exist in the World.
         */
        public int TypeCount(String type)
        {
            if (!typeCount.ContainsKey(type)) return 0;
            return typeCount[type];
        }

      

        /**
         * Returns the amount of Entities are on the layer in the World.
         * @param	layer		The layer to count Entities on.
         * @return	How many Entities are on the layer.
         */
        public int LayerCount(int layer)
        {
            if (!layerCount.ContainsKey(layer)) return 0;
            return layerCount[layer];
        }
		
        // Adding and removal.
        private List<Entity> toAdd = new List<Entity>();
        private List<Entity> toRemove = new List<Entity>();
    

        // Update information.
        private Entity updateFirst;
        public int count = 0;

        // Render information

        private Dictionary<int, int> layerCount = new Dictionary<int, int>();
        private List<int> layerList = new List<int>();
        internal Dictionary<String, Entity> typeFirst = new Dictionary<String, Entity>();
        private Dictionary<String, int> typeCount = new Dictionary<String, int>();

        private Boolean layerSort;

    
   
    }
}

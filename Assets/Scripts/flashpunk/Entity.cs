using System;
using System.Collections.Generic;
using UnityEngine;

namespace flashpunk
{
    /**
     * Main game Entity class updated by _stage.
     */
    [RequireComponent(typeof(SpriteRenderer))]
    public class Entity : Tweener
    {

        protected Rigidbody2D body;

        public bool facingRight = false;


        public bool Alive = true;

  


        /**
         * If the Entity should render.
         */
        public bool visible = true;

        /**
         * If the Entity should respond to collision checks.
         */
        public bool collidable = true;


        /**
         * Width of the Entity's hitbox.
         */
        public float width;

        /**
         * Height of the Entity's hitbox.
         */
        public float height;

        /**
         * X origin of the Entity's hitbox.
         */
        public float originX;

        /**
         * Y origin of the Entity's hitbox.
         */
        public float originY;


        protected SpriteRenderer mainSprite;

        public float angle
        {
             get {
                return transform.rotation.eulerAngles.z;
            }
            set
            {
                transform.rotation = Quaternion.AngleAxis(value, Vector3.forward);
                return;
            }
             
        }
        public float x
        {

            get {
                return transform.position.x; 
            }
            set
            {
               Vector3 pos= transform.position;
               pos.x = value;
               transform.position = pos;
               return;

            }
        }



        public float y
        {

            get
            {
                return transform.position.y;
            }
            set
            {
                Vector3 pos = transform.position;
                pos.y = value;
                transform.position = pos;
                return;

            }
        }


        
        public virtual void Start()
        {
            mainSprite = (SpriteRenderer)GetComponentInChildren<SpriteRenderer>();

         //   this.x = transform.position.x;
          //  this.y = transform.position.y;
         
            this.type = this.name;


    



            this.width = (mainSprite.sprite.bounds.size.x  * transform.localScale.x); // (mainSprite.sprite.rect.width  / 100f * transform.localScale.x);
            this.height = (mainSprite.sprite.bounds.size.y * transform.localScale.y);// (mainSprite.sprite.rect.height / 100f * transform.localScale.y);



            this.originX =  (mainSprite.sprite.bounds.center.x * transform.localScale.x);
            this.originY =  (mainSprite.sprite.bounds.center.y * transform.localScale.y);

        //    print(mainSprite.sprite.bounds.ToString());
            
      
    
          //  if (mask != null) this.mask = mask;
            HITBOX.assignTo(this);
            _class = GetType();

            

            if (FP.stage!=null) FP.stage.Add(this);
          
        }

        /// <summary>
        /// Override this, called when the Entity is added to a _stage.
        /// </summary>
        public virtual void Added()
        {

        }

        /// <summary>
        /// Override this, called when the Entity is removed from a _stage.
        /// </summary>
        public virtual void Removed()
        {
            
            DestroyImmediate(this.gameObject);
        }

        /// <summary>
        /// Updates the Entity.
        /// </summary>
        override public void update()
        {
            if (_stage == null) return;

            this.visible = _stage.visible;
            mainSprite.enabled = visible;


        }

        public void flipX()
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

        }

        /// <summary>
        /// Checks for a collision against an Entity type.
        /// </summary>
        /// <param name="type">The Entity type to check for.</param>
        /// <param name="x">Virtual x position to place this Entity.</param>
        /// <param name="y">Virtual y position to place this Entity.</param>
        /// <returns>The first Entity collided with, or null if none were collided.</returns>
        public Entity collide(string type, float x, float y)
        {
         
            if (_stage == null) return null;
            if (!_stage.typeFirst.ContainsKey(type)) return null;

            Entity e = _stage.typeFirst[type];
     
           if (!collidable || e == null) return null;
     
            _x = this.x;
            _y = this.y;
            this.x = x; 
            this.y = y;
            
            if (_mask == null)
            {
                while (e != null)
                {
                    if (x - originX + width > e.x - e.originX
                    && y - originY + height > e.y - e.originY
                    && x - originX < e.x - e.originX + e.width
                    && y - originY < e.y - e.originY + e.height
                    && e.collidable &&  e.active && e != this)
                    {
                        if (e._mask == null || e._mask.collide(HITBOX))
                        {
                            this.x = _x; this.y = _y;
                            return e;
                        }
                    }
                    e = e.typeNext;
                }
                this.x = _x; this.y = _y;
                return null;
            }
            
            while (e != null)
            {
                if (x - originX + width > e.x - e.originX
                && y - originY + height > e.y - e.originY
                && x - originX < e.x - e.originX + e.width
                && y - originY < e.y - e.originY + e.height
                && e.collidable && e.active &&  e != this)
                {
                    if (_mask.collide(e._mask != null ? e._mask : e.HITBOX))
                    {
                        this.x = _x; this.y = _y;
                        return e;
                    }
                }
                e = e.typeNext;
            }
            this.x = _x; this.y = _y;
            return null;
        }

        /**
         * Checks for collision against multiple Entity types.
         * @param	types		An Array or Vector of Entity types to check for.
         * @param	x			Virtual x position to place this Entity.
         * @param	y			Virtual y position to place this Entity.
         * @return	The first Entity collided with, or null if none were collided.
         */
        public Entity collideTypes(IEnumerable<string> types, float x, float y)
        {
            if (_stage == null) return null;
            Entity e;
            foreach (string type in types)
            {
                if ((e = collide(type, x, y)) != null) return e;
            }
            return null;
        }

        /**
         * Checks if this Entity collides with a specific Entity.
         * @param	e		The Entity to collide against.
         * @param	x		Virtual x position to place this Entity.
         * @param	y		Virtual y position to place this Entity.
         * @return	The Entity if they overlap, or null if they don't.
         */
        public Entity collideWith(Entity e, float x, float y)
        {
            _x = this.x; _y = this.y;
            this.x = x; this.y = y;

            if (x - originX + width > e.x - e.originX
            && y - originY + height > e.y - e.originY
            && x - originX < e.x - e.originX + e.width
            && y - originY < e.y - e.originY + e.height
            && collidable && active && e.collidable && e.active)
            {
                if (_mask == null)
                {
                    if (e._mask == null || e._mask.collide(HITBOX))
                    {
                        this.x = _x; this.y = _y;
                        return e;
                    }
                    this.x = _x; this.y = _y;
                    return null;
                }
                if (_mask.collide(e._mask != null ? e._mask : e.HITBOX))
                {
                    this.x = _x; this.y = _y;
                    return e;
                }
            }
            this.x = _x; this.y = _y;
            return null;
        }

        /**
         * Checks if this Entity overlaps the specified rectangle.
         * @param	x			Virtual x position to place this Entity.
         * @param	y			Virtual y position to place this Entity.
         * @param	rX			X position of the rectangle.
         * @param	rY			Y position of the rectangle.
         * @param	rWidth		Width of the rectangle.
         * @param	rHeight		Height of the rectangle.
         * @return	If they overlap.
         */
        public bool collideRect( float x, float y, float rX, float rY, float rWidth, float rHeight)
        {
            if (x - originX + width >= rX && y - originY + height >= rY
            && x - originX <= rX + rWidth && y - originY <= rY + rHeight)
            {
              
                return false;
            }
            return false;
        }

        /**
         * Checks if this Entity overlaps the specified position.
         * @param	x			Virtual x position to place this Entity.
         * @param	y			Virtual y position to place this Entity.
         * @param	pX			X position.
         * @param	pY			Y position.
         * @return	If the Entity intersects with the position.
         */
        public bool collidePoint(float x, float y, float pX, float pY)
        {
            if (pX >= x - originX && pY >= y - originY
            && pX < x - originX + width && pY < y - originY + height)
            {
        
                return false;
            }
            return false;
        }

        /**
         * Populates an array with all collided Entities of a type.
         * @param	type		The Entity type to check for.
         * @param	x			Virtual x position to place this Entity.
         * @param	y			Virtual y position to place this Entity.
         * @param	array		The Array or Vector object to populate.
         * @return	The array, populated with all collided Entities.
         */
        public void collideInto(string type, float x, float y, List<Entity> array)
        {
            if (_stage == null) return;

            Entity e = _stage.typeFirst[type];
            if (!collidable || e == null) return;

            _x = this.x; _y = this.y;
            this.x = x; this.y = y;

            if (_mask == null)
            {
                while (e != null)
                {
                    if (x - originX + width > e.x - e.originX
                    && y - originY + height > e.y - e.originY
                    && x - originX < e.x - e.originX + e.width
                    && y - originY < e.y - e.originY + e.height
                    && e.collidable && e.active && e != this)
                    {
                        if (e._mask == null || e._mask.collide(HITBOX)) array.Add(e);
                    }
                    e = e.typeNext;
                }
                this.x = _x; this.y = _y;
                return;
            }

            while (e != null)
            {
                if (x - originX + width > e.x - e.originX
                && y - originY + height > e.y - e.originY
                && x - originX < e.x - e.originX + e.width
                && y - originY < e.y - e.originY + e.height
                && e.collidable && e.active && e != this)
                {
                    if (_mask.collide(e._mask != null ? e._mask : e.HITBOX)) array.Add(e);
                }
                e = e.typeNext;
            }
            this.x = _x; this.y = _y;
            return;
        }

        /**
         * Populates an array with all collided Entities of multiple types.
         * @param	types		An array of Entity types to check for.
         * @param	x			Virtual x position to place this Entity.
         * @param	y			Virtual y position to place this Entity.
         * @param	array		The Array or Vector object to populate.
         * @return	The array, populated with all collided Entities.
         */
        public void collideTypesInto(IEnumerable<string> types, float x, float y, List<Entity> array)
        {
            if (_stage == null) return;
            foreach (string type in types) collideInto(type, x, y, array);
        }

        /**
         * If the Entity collides with the camera rectangle.
         */
        public bool onCamera
        {
            get
            {
                return collideRect(x, y, FP.camera.transform.position.x, FP.camera.transform.position.y, FP.width, FP.height);
            }
        }



        /**
         * Half the Entity's width.
         */
        public float halfWidth { get { return width / 2f; } }

        /**
         * Half the Entity's height.
         */
        public float halfHeight { get { return height / 2f; } }

        /**
         * The center x position of the Entity's hitbox.
         */
        public float centerX { get { return x - originX + width / 2f; } }

        /**
         * The center y position of the Entity's hitbox.
         */
        public float centerY { get { return y - originY + height / 2f; } }

        /**
         * The leftmost position of the Entity's hitbox.
         */
        public float left { get { return x - originX; } }

        /**
         * The rightmost position of the Entity's hitbox.
         */
        public float right { get { return x - originX + width; } }

        /**
         * The topmost position of the Entity's hitbox.
         */
        public float top { get { return y - originY; } }

        /**
         * The bottommost position of the Entity's hitbox.
         */
        public float bottom { get { return y - originY + height; } }

        /**
         * The rendering layer of this Entity. Higher layers are rendered first.
         */
        public int layer
        {
            get { return _layer; }
            set
            {
                if (_layer == value) return;
                if (!_added)
                {
                    _layer = value;
                    return;
                }
         
                _layer = value;
              
            }
        }

        /**
         * The collision type, used for collision checking.
         */
        public string type
        {
            get { return _type; }
            set
            {
                if (_type == value) return;
                if (!_added)
                {
                    _type = value;
                    return;
                }
                if (!FP.IsNullOrWhiteSpace(_type)) _stage.removeType(this);
                _type = value;
                if (!FP.IsNullOrWhiteSpace(value)) _stage.addType(this);
            }
        }

        /**
         * An optional Mask component, used for specialized collision. If this is
         * not assigned, collision checks will use the Entity's hitbox by default.
         */
        public Mask mask
        {
            get { return _mask; }
            set
            {
                if (_mask == value) return;
                if (_mask != null) _mask.assignTo(null);
                _mask = value;
                if (value != null) _mask.assignTo(this);
            }
        }


       

        public void setHitbox(float width, float height)
        {
            setHitbox(width, height, originX, originY);
        }

        /**
         * Sets the Entity's hitbox properties.
         * @param	width		Width of the hitbox.
         * @param	height		Height of the hitbox.
         * @param	originX		X origin of the hitbox.
         * @param	originY		Y origin of the hitbox.
         */
        public void setHitbox(float width, float height, float originX, float originY)
        {
            this.width = width;
            this.height = height;
            this.originX = originX;
            this.originY = originY;
        }

 
        
        public void setOrigin()
        {
            setOrigin(0, 0);
        }

        /**
         * Sets the origin of the Entity.
         * @param	x		X origin.
         * @param	y		Y origin.
         */
        public void setOrigin(int x, int y)
        {
            originX = x;
            originY = y;
        }

        /**
         * Center's the Entity's origin (half width & height).
         */
        public void centerOrigin()
        {
            originX = width / 2;
            originY = height / 2;
        }

        public float distanceFrom(Entity e)
        {
            return distanceFrom(e, false);
        }

        /**
         * Calculates the distance from another Entity.
         * @param	e				The other Entity.
         * @param	useHitboxes		If hitboxes should be used to determine the distance. If not, the Entities' x/y positions are used.
         * @return	The distance.
         */
        public float distanceFrom(Entity e, bool useHitboxes)
        {
            if (!useHitboxes) return (float)Math.Sqrt((x - e.x) * (x - e.x) + (y - e.y) * (y - e.y));
            return FP.distanceRects(x - originX, y - originY, width, height, e.x - e.originX, e.y - e.originY, e.width, e.height);
        }

        public float distanceToPoint(float px, float py)
        {
            return distanceToPoint(px, py, false);
        }

        /**
         * Calculates the distance from this Entity to the point.
         * @param	px				X position.
         * @param	py				Y position.
         * @param	useHitboxes		If hitboxes should be used to determine the distance. If not, the Entities' x/y positions are used.
         * @return	The distance.
         */
        public float distanceToPoint(float px, float py, bool useHitbox)
        {
            if (!useHitbox) return (float)Math.Sqrt((x - px) * (x - px) + (y - py) * (y - py));
            return FP.distanceRectPoint(px, py, x - originX, y - originY, width, height);
        }

        /**
         * Calculates the distance from this Entity to the rectangle.
         * @param	rx			X position of the rectangle.
         * @param	ry			Y position of the rectangle.
         * @param	rwidth		Width of the rectangle.
         * @param	rheight		Height of the rectangle.
         * @return	The distance.
         */
        public float distanceToRect(float rx, float ry, float rwidth, float rheight)
        {
            return FP.distanceRects(rx, ry, rwidth, rheight, x - originX, y - originY, width, height);
        }

        public void moveBy(float x, float y)
        {
            moveBy(x, y, null, false);
        }

        public void moveBy(float x, float y, string solidType)
        {
            moveBy(x, y, solidType, false);
        }

        /**
         * Moves the Entity by the amount, retaining integer values for its x and y.
         * @param	x			Horizontal offset.
         * @param	y			Vertical offset.
         * @param	solidType	An optional collision type to stop flush against upon collision.
         * @param	sweep		If sweeping should be used (prevents fast-moving objects from going through solidType).
         */
        public void moveBy(float x, float y, string solidType, bool sweep)
        {
            _moveX += x;
            _moveY += y;
            x = (float)Math.Round(_moveX);
            y = (float)Math.Round(_moveY);
            _moveX -= x;
            _moveY -= y;
            if (!FP.IsNullOrWhiteSpace(solidType))
            {
                int sign; Entity e;
                if (x != 0)
                {
                    if (collidable && (sweep || collide(solidType, this.x + x, this.y) != null))
                    {
                        sign = x > 0 ? 1 : -1;
                        while (x != 0)
                        {
                            if ((e = collide(solidType, this.x + sign, this.y)) != null)
                            {
                                moveCollideX(e);
                                break;
                            }
                            else
                            {
                                this.x += sign;
                                x -= sign;
                            }
                        }
                    }
                    else this.x += x;
                }
                if (y != 0)
                {
                    if (collidable && (sweep || collide(solidType, this.x, this.y + y) != null))
                    {
                        sign = y > 0 ? 1 : -1;
                        while (y != 0)
                        {
                            if ((e = collide(solidType, this.x, this.y + sign)) != null)
                            {
                                moveCollideY(e);
                                break;
                            }
                            else
                            {
                                this.y += sign;
                                y -= sign;
                            }
                        }
                    }
                    else this.y += y;
                }
            }
            else
            {
                this.x += x;
                this.y += y;
            }
        }

        public void moveTo(float x, float y)
        {
            moveTo(x, y, null, false);
        }

        public void moveTo(float x, float y, string solidType)
        {
            moveTo(x, y, solidType, false);
        }

        /**
         * Moves the Entity to the position, retaining integer values for its x and y.
         * @param	x			X position.
         * @param	y			Y position.
         * @param	solidType	An optional collision type to stop flush against upon collision.
         * @param	sweep		If sweeping should be used (prevents fast-moving objects from going through solidType).
         */
        public void moveTo(float x, float y, string solidType, bool sweep)
        {
            moveBy(x - this.x, y - this.y, solidType, sweep);
        }

        /**
         * Moves towards the target position, retaining integer values for its x and y.
         * @param	x			X target.
         * @param	y			Y target.
         * @param	amount		Amount to move.
         * @param	solidType	An optional collision type to stop flush against upon collision.
         * @param	sweep		If sweeping should be used (prevents fast-moving objects from going through solidType).
         */
        public void moveTowards(float x, float y, float amount, string solidType, bool sweep)
        {
            _point.x = x - this.x;
            _point.y = y - this.y;
            if (_point != Vector2.zero) _point.Normalize();
            _point *= amount;
            moveBy(_point.x, _point.y, solidType, sweep);
        }

        /**
         * When you collide with an Entity on the x-axis with moveTo() or moveBy().
         * @param	e		The Entity you collided with.
         */
        public virtual void moveCollideX(Entity e)
        {

        }

        /**
         * When you collide with an Entity on the y-axis with moveTo() or moveBy().
         * @param	e		The Entity you collided with.
         */
        public virtual void moveCollideY(Entity e)
        {

        }

        public void clampHorizontal(float left, float right)
        {
            clampHorizontal(left, right, 0);
        }

        /**
         * Clamps the Entity's hitbox on the x-axis.
         * @param	left		Left bounds.
         * @param	right		Right bounds.
         * @param	padding		Optional padding on the clamp.
         */
        public void clampHorizontal(float left, float right, float padding)
        {
            if (x - originX < left + padding) x = left + originX + padding;
            if (x - originX + width > right - padding) x = right - width + originX - padding;
        }

        public void clampVertical(float top, float bottom)
        {
            clampVertical(top, bottom, 0);
        }

        /**
         * Clamps the Entity's hitbox on the y axis.
         * @param	top			Min bounds.
         * @param	bottom		Max bounds.
         * @param	padding		Optional padding on the clamp.
         */
        public void clampVertical(float top, float bottom, float padding)
        {
            if (y - originY < top + padding) y = top + originY + padding;
            if (y - originY + height > bottom - padding) y = bottom - height + originY - padding;
        }

        /**
   * The Stage object this Entity has been added to.
   */
        public Stage stage
        {
            get
            {
                return _stage;
            }
        }


        internal Stage _stage;
        // Entity information.
        internal Type _class;
        internal bool _added;
        internal string _type = "";
        internal int _layer;
        internal Entity updatePrev;
        internal Entity updateNext;
        internal Entity typePrev;
        internal Entity typeNext;


        // Collision information.
        private readonly Mask HITBOX = new Mask();
        private Mask _mask;
        private float _x;
        private float _y;
        private float _moveX = 0;
        private float _moveY = 0;


        private Vector2 _point = FP.point;
     
    }
}

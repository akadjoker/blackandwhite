using System;
using System.Collections.Generic;
using UnityEngine;
namespace flashpunk
{
 
    /// <summary>
    /// Static catch-all class used to access global properties and functions.
    /// </summary>
    public static class FP
    {
        /// <summary>
        /// The FlashPunk major version.
        /// </summary>
        public const string VERSION = "1.4";

        /// <summary>
        /// Width of the game.
        /// </summary>
        public static int width;

        /// <summary>
        /// Height of the game.
        /// </summary>
        public static int height;

        /// <summary>
        /// If the game is running at a fixed framerate.
        /// </summary>
        public static bool isFixed;

        /// <summary>
        /// The framerate assigned to the stage.
        /// </summary>
        public static float frameRate;

        /// <summary>
        /// The framerate assigned to the stage.
        /// </summary>
        public static float assignedFrameRate;

        /// <summary>
        /// Time elapsed since the last frame (non-fixed framerate only).
        /// </summary>
        public static float elapsed;

        /// <summary>
        /// Timescale applied to FP.elapsed (non-fixed framerate only).
        /// </summary>
        public static float rate = 1;


        public static  Camera camera;


        /// <summary>
        /// Half the screen width.
        /// </summary>
        /// <value>Half the screen width.</value>
        public static float halfWidth { get { return width / 2; } }

        /// <summary>
        /// Half the screen height.
        /// </summary>
        /// <value>Half the screen height.</value>
        public static float halfHeight { get { return height / 2; } }

       
  

     

        /// <summary>
        /// Randomly chooses and returns one of the provided values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">The Objects you want to randomly choose from. Can be ints, Numbers, Points, etc.</param>
        /// <returns>A randomly chosen one of the provided parameters.</returns>
        public static T choose<T>(params T[] objs)
        {
            return objs[new System.Random().Next(objs.Length)];
        }

        /// <summary>
        /// Finds the sign of the provided value.
        /// </summary>
        /// <param name="value">The float to evaluate.</param>
        /// <returns>1 if value &gt; 0, -1 if value &lt; 0, and 0 when value == 0.</returns>
        public static int sign(float value)
        {
            return value < 0 ? -1 : (value > 0 ? 1 : 0);
        }

        /// <summary>
        /// Approaches the value towards the target, by the specified amount, without overshooting the target.
        /// </summary>
        /// <param name="value">The starting value.</param>
        /// <param name="target">The target that you want value to approach.</param>
        /// <param name="amount">How much you want the value to approach target by.</param>
        /// <returns>The new value.</returns>
        public static float approach(float value, float target, float amount)
        {
            return value < target ? (target < value + amount ? target : value + amount) : (target > value - amount ? target : value - amount);
        }

        /// <summary>
        /// Linear interpolation between two values.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <returns>When t=0, returns a. When t=1, returns b. When t=0.5, will return halfway between a and b. Etc.</returns>
        public static float lerp(float a, float b)
        {
            return lerp(a, b, 1);
        }

        /// <summary>
        /// Linear interpolation between two values.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <param name="t">Interpolation factor.</param>
        /// <returns>When t=0, returns a. When t=1, returns b. When t=0.5, will return halfway between a and b. Etc.</returns>
        public static float lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        /// <summary>
        /// Linear interpolation between two colors.
        /// </summary>
        /// <param name="fromColor">First color.</param>
        /// <param name="toColor">Second color.</param>
        /// <returns>RGB component-interpolated color value.</returns>
        public static uint colorLerp(uint fromColor, uint toColor)
        {
            return colorLerp(fromColor, toColor, 1);
        }

        /// <summary>
        /// Linear interpolation between two colors.
        /// </summary>
        /// <param name="fromColor">First color.</param>
        /// <param name="toColor">Second color.</param>
        /// <param name="t">Interpolation value. Clamped to the range [0, 1].</param>
        /// <returns>RGB component-interpolated color value.</returns>
        public static uint colorLerp(uint fromColor, uint toColor, float t)
        {
            if (t <= 0) { return fromColor; }
            if (t >= 1) { return toColor; }
            uint a = fromColor >> 24 & 0xFF,
                r = fromColor >> 16 & 0xFF,
                g = fromColor >> 8 & 0xFF,
                b = fromColor & 0xFF;
            long dA = (toColor >> 24 & 0xFF) - a,
                dR = (toColor >> 16 & 0xFF) - r,
                dG = (toColor >> 8 & 0xFF) - g,
                dB = (toColor & 0xFF) - b;
            a += (uint)(dA * t);
            r += (uint)(dR * t);
            g += (uint)(dG * t);
            b += (uint)(dB * t);
            return a << 24 | r << 16 | g << 8 | b;
        }

        /// <summary>
        /// Steps the object towards a Vector2.
        /// </summary>
        /// <param name="obj">object to move (must have an x and y property).</param>
        /// <param name="x">X position to step towards.</param>
        /// <param name="y">Y position to step towards.</param>
        public static void stepTowards(Entity obj, float x, float y)
        {
            stepTowards(obj, x, y, 1);
        }

        /// <summary>
        /// Steps the object towards a Vector2.
        /// </summary>
        /// <param name="obj">object to move (must have an x and y property).</param>
        /// <param name="x">X position to step towards.</param>
        /// <param name="y">Y position to step towards.</param>
        /// <param name="distance">The distance to step (will not overshoot target).</param>
        public static void stepTowards(Entity obj, float x, float y, float distance)
        {
         
            Vector2 point;
            point.x = x - obj.x;
            point.y = y - obj.y;
            if (point.magnitude <= distance)
            {
                obj.x = x;
                obj.y = y;
                return;
            }
            point *= distance;
            obj.x += point.x;
            obj.y += point.y;
        }

        /// <summary>
        /// Anchors the object to a position.
        /// </summary>
        /// <param name="obj">The anchor object.</param>
        /// <param name="anchor">The anchor.</param>
        public static void anchorTo(Entity obj, Entity anchor)
        {
            anchorTo(obj, anchor, 0);
        }

        /// <summary>
        /// Anchors the object to a position.
        /// </summary>
        /// <param name="obj">The object to anchor.</param>
        /// <param name="anchor">The anchor object.</param>
        /// <param name="distance">The max distance object can be anchored to the anchor.</param>
        public static void anchorTo(Entity obj, Entity anchor, float distance)
        {
            Vector2 point;
            point.x = obj.x - anchor.x;
            point.y = obj.y - anchor.y;
            if (point.magnitude > distance) point *= distance;
            obj.x = anchor.x + point.x;
            obj.y = anchor.y + point.y;
        }

        /// <summary>
        /// Finds the angle (in degrees) from Vector2 1 to Vector2 2.
        /// </summary>
        /// <param name="x1">The first x-position.</param>
        /// <param name="y1">The first y-position.</param>
        /// <param name="x2">The second x-position.</param>
        /// <param name="y2">The second y-position.</param>
        /// <returns>Finds the angle (in degrees) from Vector2 1 to Vector2 2.</returns>
        public static float angle(float x1, float y1, float x2, float y2)
        {
            float a = (float)Math.Atan2(y2 - y1, x2 - x1) * DEG;
            return a < 0 ? a + 360 : a;
        }

        /// <summary>
        /// Sets the x/y values of the provided object to a vector of the specified angle and length.
        /// </summary>
        /// <param name="obj">The object whose x/y properties should be set.</param>
        /// <param name="angle">The angle of the vector, in degrees.</param>
        public static void angleXY(Entity obj, float angle)
        {
            angleXY(obj, angle, 1, 0, 0);
        }

        /// <summary>
        /// Sets the x/y values of the provided object to a vector of the specified angle and length.
        /// </summary>
        /// <param name="obj">The object whose x/y properties should be set.</param>
        /// <param name="angle">The angle of the vector, in degrees.</param>
        /// <param name="length">The distance to the vector from (0, 0)..</param>
        public static void angleXY(Entity obj, float angle, float length)
        {
            angleXY(obj, angle, length, 0, 0);
        }

        /// <summary>
        /// Sets the x/y values of the provided object to a vector of the specified angle and length.
        /// </summary>
        /// <param name="obj">The object whose x/y properties should be set.</param>
        /// <param name="angle">The angle of the vector, in degrees.</param>
        /// <param name="length">The distance to the vector from (0, 0)..</param>
        /// <param name="x">X offset.</param>
        /// <param name="y">Y offset.</param>
        public static void angleXY(Entity obj, float angle, float length, float x)
        {
            angleXY(obj, angle, length, x, 0);
        }

        /// <summary>
        /// Sets the x/y values of the provided object to a vector of the specified angle and length.
        /// </summary>
        /// <param name="obj">The object whose x/y properties should be set.</param>
        /// <param name="angle">The angle of the vector, in degrees.</param>
        /// <param name="length">The distance to the vector from (0, 0)..</param>
        /// <param name="x">X offset.</param>
        /// <param name="y">Y offset.</param>
        public static void angleXY(Entity obj, float angle, float length, float x, float y)
        {
            angle *= RAD;
            // TODO:
            obj.x = (float)Math.Cos(angle) * length + x;
            obj.y = (float)Math.Sin(angle) * length + y;
        }

        /// <summary>
        /// Rotates the object around the anchor by the specified amount.
        /// </summary>
        /// <param name="obj">object to rotate around the anchor.</param>
        /// <param name="anchor">Anchor to rotate around.</param>
        public static void rotateAround(Entity obj, Entity anchor)
        {
            rotateAround(obj, anchor, 0, true);
        }

        /// <summary>
        /// Rotates the object around the anchor by the specified amount.
        /// </summary>
        /// <param name="obj">object to rotate around the anchor.</param>
        /// <param name="anchor">Anchor to rotate around.</param>
        /// <param name="angle">The amount of degrees to rotate by.</param>
        public static void rotateAround(Entity obj, Entity anchor, float angle)
        {
            rotateAround(obj, anchor, angle, true);
        }

        /// <summary>
        /// Rotates the object around the anchor by the specified amount.
        /// </summary>
        /// <param name="obj">object to rotate around the anchor.</param>
        /// <param name="anchor">Anchor to rotate around.</param>
        /// <param name="angle">The amount of degrees to rotate by.</param>
        /// <param name="relative">The amount of degrees to rotate by.</param>
        public static void rotateAround(Entity obj, Entity anchor, float angle, bool relative)
        {
         
            if (relative) angle += FP.angle(anchor.x, anchor.y, obj.x, obj.y);
            FP.angleXY(obj, angle, FP.distance(anchor.x, anchor.y, obj.x, obj.y), anchor.x, anchor.y);
        }

        /// <summary>
        /// Find the distance between two points.
        /// </summary>
        /// <param name="x1">The first x-position.</param>
        /// <param name="y1">The first y-position.</param>
        /// <returns>The distance.</returns>
        public static float distance(float x1, float y1)
        {
            return distance(x1, y1, 0, 0);
        }

        /// <summary>
        /// Find the distance between two points.
        /// </summary>
        /// <param name="x1">The first x-position.</param>
        /// <param name="y1">The first y-position.</param>
        /// <param name="x2">The second x-position.</param>
        /// <returns>The distance.</returns>
        public static float distance(float x1, float y1, float x2)
        {
            return distance(x1, y1, x2, 0);
        }

        /// <summary>
        /// Find the distance between two points.
        /// </summary>
        /// <param name="x1">The first x-position.</param>
        /// <param name="y1">The first y-position.</param>
        /// <param name="x2">The second x-position.</param>
        /// <param name="y2">The second y-position.</param>
        /// <returns>The distance.</returns>
        public static float distance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        /// <summary>
        /// Find the distance between two rectangles. Will return 0 if the rectangles overlap.
        /// </summary>
        /// <param name="x1">The x-position of the first rect.</param>
        /// <param name="y1">The y-position of the first rect.</param>
        /// <param name="w1">The width of the first rect.</param>
        /// <param name="h1">The height of the first rect.</param>
        /// <param name="x2">The x-position of the second rect.</param>
        /// <param name="y2">The y-position of the second rect.</param>
        /// <param name="w2">The width of the second rect.</param>
        /// <param name="h2">The height of the second rect.</param>
        /// <returns>The distance.</returns>
        public static float distanceRects(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
        {
            if (x1 < x2 + w2 && x2 < x1 + w1)
            {
                if (y1 < y2 + h2 && y2 < y1 + h1) return 0;
                if (y1 > y2) return y1 - (y2 + h2);
                return y2 - (y1 + h1);
            }
            if (y1 < y2 + h2 && y2 < y1 + h1)
            {
                if (x1 > x2) return x1 - (x2 + w2);
                return x2 - (x1 + w1);
            }
            if (x1 > x2)
            {
                if (y1 > y2) return distance(x1, y1, (x2 + w2), (y2 + h2));
                return distance(x1, y1 + h1, x2 + w2, y2);
            }
            if (y1 > y2) return distance(x1 + w1, y1, x2, y2 + h2);
            return distance(x1 + w1, y1 + h1, x2, y2);
        }

        /// <summary>
        /// Find the distance between a Vector2 and a rectangle. Returns 0 if the Vector2 is within the rectangle.
        /// </summary>
        /// <param name="px">The x-position of the Vector2.</param>
        /// <param name="py">The y-position of the Vector2.</param>
        /// <param name="rx">The x-position of the rect.</param>
        /// <param name="ry">The y-position of the rect.</param>
        /// <param name="rw">The width of the rect.</param>
        /// <param name="rh">The height of the rect.</param>
        /// <returns>The distance.</returns>
        public static float distanceRectPoint(float px, float py, float rx, float ry, float rw, float rh)
        {
            if (px >= rx && px <= rx + rw)
            {
                if (py >= ry && py <= ry + rh) return 0;
                if (py > ry) return py - (ry + rh);
                return ry - py;
            }
            if (py >= ry && py <= ry + rh)
            {
                if (px > rx) return px - (rx + rw);
                return rx - px;
            }
            if (px > rx)
            {
                if (py > ry) return distance(px, py, rx + rw, ry + rh);
                return distance(px, py, rx + rw, ry);
            }
            if (py > ry) return distance(px, py, rx, ry + rh);
            return distance(px, py, rx, ry);
        }

        /// <summary>
        /// Clamps the value within the minimum and maximum values.
        /// </summary>
        /// <param name="value">The float to evaluate.</param>
        /// <param name="min">The minimum range.</param>
        /// <param name="max">The maximum range.</param>
        /// <returns>The clamped value.</returns>
        public static float clamp(float value, float min, float max)
        {
            if (max > min)
            {
                value = value < max ? value : max;
                return value > min ? value : min;
            }
            value = value < min ? value : min;
            return value > max ? value : max;
        }

        /// <summary>
        /// Clamps the object inside the rectangle.
        /// </summary>
        /// <param name="obj">The object to clamp (must have an x and y property).</param>
        /// <param name="x">Rectangle's x.</param>
        /// <param name="y">Rectangle's y.</param>
        /// <param name="width">Rectangle's width.</param>
        /// <param name="height">Rectangle's height.</param>
        public static void clampInRect(Entity obj, float x, float y, float width, float height)
        {
            clampInRect(obj, x, y, width, height, 0);
        }

        /// <summary>
        /// Clamps the object inside the rectangle.
        /// </summary>
        /// <param name="obj">The object to clamp (must have an x and y property).</param>
        /// <param name="x">Rectangle's x.</param>
        /// <param name="y">Rectangle's y.</param>
        /// <param name="width">Rectangle's width.</param>
        /// <param name="height">Rectangle's height.</param>
        /// <param name="padding">The padding.</param>
        public static void clampInRect(Entity obj, float x, float y, float width, float height, float padding)
        {
            // TODO:
            obj.x = clamp(obj.x, x + padding, x + width - padding);
            obj.y = clamp(obj.y, y + padding, y + height - padding);
        }

        /// <summary>
        /// Transfers a value from one scale to another scale. For example, scale(.5, 0, 1, 10, 20) == 15, and scale(3, 0, 5, 100, 0) == 40.
        /// </summary>
        /// <param name="value">The value on the first scale.</param>
        /// <param name="min">The minimum range of the first scale.</param>
        /// <param name="max">The maximum range of the first scale.</param>
        /// <param name="min2">The minimum range of the second scale.</param>
        /// <param name="max2">The maximum range of the second scale.</param>
        /// <returns>The scaled value.</returns>
        public static float scale(float value, float min, float max, float min2, float max2)
        {
            return min2 + ((value - min) / (max - min)) * (max2 - min2);
        }

        /// <summary>
        /// Transfers a value from one scale to another scale, but clamps the return value within the second scale.
        /// </summary>
        /// <param name="value">The value on the first scale.</param>
        /// <param name="min">The minimum range of the first scale.</param>
        /// <param name="max">The maximum range of the first scale.</param>
        /// <param name="min2">The minimum range of the second scale.</param>
        /// <param name="max2">The maximum range of the second scale.</param>
        /// <returns>The scaled and clamped value.</returns>
        public static float scaleClamp(float value, float min, float max, float min2, float max2)
        {
            value = min2 + ((value - min) / (max - min)) * (max2 - min2);
            if (max2 > min2)
            {
                value = value < max2 ? value : max2;
                return value > min2 ? value : min2;
            }
            value = value < min2 ? value : min2;
            return value > max2 ? value : max2;
        }

        /// <summary>
        /// The random seed used by FP's random functions.
        /// </summary>
        /// <value>The random seed.</value>
        public static uint randomSeed
        {
            get { return _getSeed; }
            set
            {
                _seed = (uint)clamp(value, 1, 2147483646);
                _getSeed = _seed;
            }
        }

        /// <summary>
        /// Randomizes the random seed using Flash's Math.random() function.
        /// </summary>
        public static void randomizeSeed()
        {
            randomSeed = 2147483647 * (uint)new System.Random().Next();
        }

        /// <summary>
        /// A pseudo-random float produced using FP's random seed, where 0 &lt;= float &lt; 1.
        /// </summary>
        /// <value>The random.</value>
        public static float random
        {
            get
            {
                _seed = (_seed * 16807) % 2147483647;
                return _seed / 2147483647;
            }
        }

        /// <summary>
        /// Returns a pseudo-random uint.
        /// </summary>
        /// <param name="amount">The returned uint will always be 0 &lt;= uint &lt; amount.</param>
        /// <returns>The uint.</returns>
        public static uint rand(uint amount)
        {
            _seed = (_seed * 16807) % 2147483647;
            return (_seed / 2147483647) * amount;
        }

        /// <summary>
        /// Returns the next item after current in the list of options.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current">The currently selected item (must be one of the options).</param>
        /// <param name="options">An array of all the items to cycle through.</param>
        /// <returns>The next item in the list.</returns>
        public static T next<T>(T current, T[] options)
        {
            return next(current, options, true);
        }

        /// <summary>
        /// Returns the next item after current in the list of options.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current">The currently selected item (must be one of the options).</param>
        /// <param name="options">An array of all the items to cycle through.</param>
        /// <param name="loop">If true, will jump to the first item after the last item is reached.</param>
        /// <returns>The next item in the list.</returns>
        public static T next<T>(T current, T[] options, bool loop)
        {
            if (loop) return options[(Array.IndexOf(options, current) + 1) % options.Length];
            return options[Math.Max(Array.IndexOf(options, current) + 1, options.Length - 1)];
        }

        public static T prev<T>(T current, T[] options)
        {
            return prev(current, options, true);
        }

        /// <summary>
        /// Returns the item previous to the current in the list of options.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current">The currently selected item (must be one of the options).</param>
        /// <param name="options">An array of all the items to cycle through.</param>
        /// <param name="loop">If true, will jump to the last item after the first is reached.</param>
        /// <returns>The previous item in the list.</returns>
        public static T prev<T>(T current, T[] options, bool loop)
        {
            if (loop) return options[((Array.IndexOf(options, current) - 1) + options.Length) % options.Length];
            return options[Math.Max(Array.IndexOf(options, current) - 1, 0)];
        }

        /// <summary>
        /// Swaps the current item between a and b. Useful for quick state/string/value swapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current">The currently selected item.</param>
        /// <param name="a">Item a.</param>
        /// <param name="b">Item b.</param>
        /// <returns>Returns a if current is b, and b if current is a.</returns>
        public static T swap<T>(T current, T a, T b)
        {
            return UnityEngine.Object.Equals(current, a) ? b : a;
        }

        /// <summary>
        /// Creates a color value by combining the chosen RGB values.
        /// </summary>
        /// <param name="R">The red value of the color, from 0 to 255.</param>
        /// <param name="G">The green value of the color, from 0 to 255.</param>
        /// <param name="B">The blue value of the color, from 0 to 255.</param>
        /// <returns>The color uint.</returns>
        public static uint getColorRGB(uint R, uint G, uint B)
        {
            return R << 16 | G << 8 | B;
        }

        /// <summary>
        /// Creates a color value with the chosen HSV values.
        /// </summary>
        /// <param name="h">The hue of the color (from 0 to 1).</param>
        /// <param name="s">The saturation of the color (from 0 to 1).</param>
        /// <param name="v">The value of the color (from 0 to 1).</param>
        /// <returns>The color uint.</returns>
        public static uint getColorHSV(float h, float s, float v)
        {
            h = (int)(h * 360);
            int hi = (int)Math.Floor(h / 60) % 6;
            float f = h / 60 - (float)Math.Floor(h / 60),
                p = (v * (1 - s)),
                q = (v * (1 - f * s)),
                t = (v * (1 - (1 - f) * s));
            switch (hi)
            {
                case 0: return (uint)(v * 255) << 16 | (uint)(t * 255) << 8 | (uint)(p * 255);
                case 1: return (uint)(q * 255) << 16 | (uint)(v * 255) << 8 | (uint)(p * 255);
                case 2: return (uint)(p * 255) << 16 | (uint)(v * 255) << 8 | (uint)(t * 255);
                case 3: return (uint)(p * 255) << 16 | (uint)(q * 255) << 8 | (uint)(v * 255);
                case 4: return (uint)(t * 255) << 16 | (uint)(p * 255) << 8 | (uint)(v * 255);
                case 5: return (uint)(v * 255) << 16 | (uint)(p * 255) << 8 | (uint)(q * 255);
                default: return 0;
            }
        }

        /// <summary>
        /// Finds the red factor of a color.
        /// </summary>
        /// <param name="color">The color to evaluate.</param>
        /// <returns>A uint from 0 to 255.</returns>
        public static uint getRed(uint color)
        {
            return color >> 16 & 0xFF;
        }

        /// <summary>
        /// Finds the green factor of a color.
        /// </summary>
        /// <param name="color">The color to evaluate.</param>
        /// <returns>A uint from 0 to 255.</returns>
        public static uint getGreen(uint color)
        {
            return color >> 8 & 0xFF;
        }

        /// <summary>
        /// Finds the blue factor of a color.
        /// </summary>
        /// <param name="color">The color to evaluate.</param>
        /// <returns>A uint from 0 to 255.</returns>
        public static uint getBlue(uint color)
        {
            return color & 0xFF;
        }

   

   
        public static void log(String data)
        {
            Debug.Log(data);
        }

        
        // TODO: tween()
        ///**
        // * Tweens numeric public properties of an object. Shorthand for creating a MultiVarTween tween, starting it and adding it to a Tweener.
        // * @param    object        The object containing the properties to tween.
        // * @param    values        An object containing key/value pairs of properties and target values.
        // * @param    duration    Duration of the tween.
        // * @param    options        An object containing key/value pairs of the following optional parameters:
        // *                         type        Tween type.
        // *                         complete    Optional completion callback function.
        // *                         ease        Optional easer function.
        // *                         tweener        The Tweener to add this Tween to.
        // * @return    The added MultiVarTween object.
        // * 
        // * Example: FP.tween(object, { x: 500, y: 350 }, 2.0, { ease: easeFunction, complete: onComplete } );
        // */
        //public static MultiVarTween tween(object object, object values, float duration, object options = null)
        //{
        //    uint type = Tween.ONESHOT,
        //        Function complete = null,
        //        Function ease = null,
        //        Tweener tweener = FP.world;
        //    if (object is Tweener) tweener = object as Tweener;
        //    if (options)
        //    {
        //        if (options.hasOwnProperty("type")) type = options.type;
        //        if (options.hasOwnProperty("complete")) complete = options.complete;
        //        if (options.hasOwnProperty("ease")) ease = options.ease;
        //        if (options.hasOwnProperty("tweener")) tweener = options.tweener;
        //    }
        //    MultiVarTween tween = new MultiVarTween(complete, type);
        //    tween.tween(object, values, duration, ease);
        //    tweener.addTween(tween);
        //    return tween;
        //}

        /// <summary>
        /// Gets an array of frame indices.
        /// </summary>
        /// <param name="from">Starting frame.</param>
        /// <param name="to">Ending frame.</param>
        /// <returns></returns>
        public static int[] frames(int from, int to)
        {
            return frames(from, to, 0);
        }

        /// <summary>
        /// Gets an array of frame indices.
        /// </summary>
        /// <param name="from">Starting frame.</param>
        /// <param name="to">Ending frame.</param>
        /// <param name="skip">Skip amount every frame (eg. use 1 for every 2nd frame).</param>
        /// <returns></returns>
        public static int[] frames(int from, int to, int skip)
        {
            List<int> a = new List<int>();
            skip++;
            if (from < to)
            {
                while (from <= to)
                {
                    a.Add(from);
                    from += skip;
                }
            }
            else
            {
                while (from >= to)
                {
                    a.Add(from);
                    from -= skip;
                }
            }
            return a.ToArray();
        }


    public static bool IsNullOrWhiteSpace(string s){
    foreach(char c in s){
    if(c != ' ') return false;
    }
    return true;
     
    }


        // Stage information.
        public static Stage stage;




        // Time information.
        internal static int _time;
        public static uint _updateTime;
        public static uint _renderTime;
        public static uint _gameTime;
        public static uint _flashTime;


        // Pseudo-random float generation (the seed is set in Engine's contructor).
        private static uint _seed = 0;
        private static uint _getSeed;



        // Used for rad-to-deg and deg-to-rad conversion.
        public const float DEG = (float)(-180 / Math.PI);
        public const float RAD = (float)(Math.PI / -180);

        // Global Flash objects.
        // public static Stage stage;
        public static float mouseX;
        public static float mouseY;
        public static Engine engine;

        // Global objects used for rendering, collision, etc.
        public static Vector2 point;
        public static Vector2 point2;
       // public static Entity entity;

    }
}
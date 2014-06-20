using UnityEngine;
using System;
using UnityEngine;

class Particle
{
    public Vector2 Position;
    Vector2 StartDirection;
    Vector2 EndDirection;
    float LifeLeft;
    float StartingLife;
    float ScaleBegin;
    float ScaleEnd;
    Color StartColor;
    Color EndColor;
    Emitter Parent;
    float lifePhase;

    public Particle(Vector2 Position, Vector2 StartDirection, Vector2 EndDirection, float StartingLife, float ScaleBegin, float ScaleEnd, Color StartColor, Color EndColor, Emitter Yourself)
    {
        this.Position = Position;
        this.StartDirection = StartDirection;
        this.EndDirection = EndDirection;
        this.StartingLife = StartingLife;
        this.LifeLeft = StartingLife;
        this.ScaleBegin = ScaleBegin;
        this.ScaleEnd = ScaleEnd;
        this.StartColor = StartColor;
        this.EndColor = EndColor;
        this.Parent = Yourself;
    }

    public bool Update(float dt)
    {
        LifeLeft -= dt;
        if (LifeLeft <= 0)            return false;
        lifePhase = LifeLeft / StartingLife;      // 1 means newly created 0 means dead.
        Position += MathLib.LinearInterpolate(EndDirection, StartDirection, lifePhase)*dt;
        return true;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        float currScale = MathLib.LinearInterpolate(ScaleEnd, ScaleBegin, lifePhase);
        Color currCol = MathLib.LinearInterpolate(EndColor, StartColor, lifePhase);
        spriteBatch.draw(
            (Position.x - 0.5f * currScale),
            (Position.y - 0.5f * currScale),
            0,0,
            currScale,currScale,
            0,
            754, 490, 11,11,
            false,false,
            currCol
            );


    }
}
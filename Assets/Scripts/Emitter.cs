using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Emitter
{
    public Vector2 RelPosition;                    // Position relative to collection.
    public int Budget;                      // Max number of alive particles.
    float NextSpawnIn;                      // This is a random number generated using the SecPerSpawn.
    float SecPassed;                        // Time pased since last spawn.
    LinkedList<Particle> ActiveParticles;   // A list of all the active particles.
                    

    public Vector2 SecPerSpawn;
    public Vector2 SpawnDirection;
    public Vector2 SpawnNoiseAngle;
    public Vector2 StartLife;
    public Vector2 StartScale;
    public Vector2 EndScale;
    public Color StartColor1;
    public Color StartColor2;
    public Color EndColor1;
    public Color EndColor2;
    public Vector2 StartSpeed;
    public Vector2 EndSpeed;

   //public ParticlesEmitter Parent;

    public Emitter(Vector2 SecPerSpawn, Vector2 SpawnDirection, Vector2 SpawnNoiseAngle, Vector2 StartLife, Vector2 StartScale,
                Vector2 EndScale, Color StartColor1, Color StartColor2, Color EndColor1, Color EndColor2, Vector2 StartSpeed,
                Vector2 EndSpeed, int Budget, Vector2 RelPosition)//,  ParticlesEmitter parent)
    {
        this.SecPerSpawn = SecPerSpawn;
        this.SpawnDirection = SpawnDirection;
        this.SpawnNoiseAngle = SpawnNoiseAngle;
        this.StartLife = StartLife;
        this.StartScale = StartScale;
        this.EndScale = EndScale;
        this.StartColor1 = StartColor1;
        this.StartColor2 = StartColor2;
        this.EndColor1 = EndColor1;
        this.EndColor2 = EndColor2;
        this.StartSpeed = StartSpeed;
        this.EndSpeed = EndSpeed;
        this.Budget = Budget;
        this.RelPosition = RelPosition;
    

       // this.Parent = parent;
        ActiveParticles = new LinkedList<Particle>();
        this.NextSpawnIn = MathLib.LinearInterpolate(SecPerSpawn.x, SecPerSpawn.y,Random.value);
        this.SecPassed = 0.0f;
    }

public void Update(float dt)
{
    SecPassed += dt;
    while (SecPassed > NextSpawnIn)
    {
        if (ActiveParticles.Count < Budget)
        {
            // Spawn a particle





            Vector2 StartDirection = MathLib.RotateZ(SpawnDirection, MathLib.LinearInterpolate(SpawnNoiseAngle.x, SpawnNoiseAngle.y, Random.value)); 
            StartDirection.Normalize();
            Vector2 EndDirection = StartDirection * MathLib.LinearInterpolate(EndSpeed.x, EndSpeed.y, Random.value);
            StartDirection *= MathLib.LinearInterpolate(StartSpeed.x, StartSpeed.y, Random.value);
            ActiveParticles.AddLast(new Particle(
                RelPosition,//+ Parent.Position,
                StartDirection,
                EndDirection,
                MathLib.LinearInterpolate(StartLife.x, StartLife.y, Random.value),
                MathLib.LinearInterpolate(StartScale.x, StartScale.y, Random.value),
                MathLib.LinearInterpolate(EndScale.x, EndScale.y, Random.value),
                MathLib.LinearInterpolate(StartColor1, StartColor2, Random.value),
                MathLib.LinearInterpolate(EndColor1, EndColor2, Random.value),
                this)
            );    
        }
        SecPassed -= NextSpawnIn;
        NextSpawnIn = MathLib.LinearInterpolate(SecPerSpawn.x, SecPerSpawn.y, Random.value);
    }

    LinkedListNode<Particle> node = ActiveParticles.First;
    while (node != null)
    {
        bool isAlive = node.Value.Update(dt);
        node = node.Next;
        if (!isAlive)
        {
            if (node == null)
            {
                ActiveParticles.RemoveLast();
            }
            else
            {
                ActiveParticles.Remove(node.Previous);
            }
        }
    }
}

    public void Draw(SpriteBatch spriteBatch)
    {
        LinkedListNode<Particle> node = ActiveParticles.First;
        while (node != null)
        {
            node.Value.Draw(spriteBatch);
            node = node.Next;
        }
    }

    public void Clear()
    {
        ActiveParticles.Clear();
    }

}
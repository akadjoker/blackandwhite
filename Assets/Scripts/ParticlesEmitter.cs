using UnityEngine;
using System.Collections.Generic;

public class ParticlesEmitter
{
    public List<Emitter> EmitterList;
    public Vector2 Position;

    public ParticlesEmitter(Vector2 Position)
    {
        this.Position = Position;
        EmitterList = new List<Emitter>();
    }

    public void Update(float dt)
    {
        for (int i = 0; i < EmitterList.Count; i++)
        {
            if (EmitterList[i].Budget > 0)
            {
                EmitterList[i].Update(dt);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < EmitterList.Count; i++)
        {
            if (EmitterList[i].Budget > 0)
            {
                EmitterList[i].Draw(spriteBatch);
            }
        }
    }

    public void Clear()
    {
        for (int i = 0; i < EmitterList.Count; i++)
        {
            if (EmitterList[i].Budget > 0)
            {
                EmitterList[i].Clear();
            }
        }
    }

    public void AddEmitter(Vector2 SecPerSpawn, Vector2 SpawnDirection, Vector2 SpawnNoiseAngle, Vector2 StartLife, Vector2 StartScale,
                Vector2 EndScale, Color StartColor1, Color StartColor2, Color EndColor1, Color EndColor2, Vector2 StartSpeed,
                Vector2 EndSpeed, int Budget, Vector2 RelPosition)
    {
        Emitter emitter = new Emitter(SecPerSpawn, SpawnDirection, SpawnNoiseAngle,
                                    StartLife, StartScale, EndScale, StartColor1,
                                    StartColor2, EndColor1, EndColor2, StartSpeed,
                                    EndSpeed, Budget, RelPosition);
        EmitterList.Add(emitter);
    }
}
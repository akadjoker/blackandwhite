using UnityEngine;
using System.Collections;
using flashpunk;

public class GameStage : Stage 
{

    private bool ready = false;
    public int currentEnemyCount=0;
    
    public float spawnDelay = 0;
    public int countAlive = 0;

    public float waveTime=0;
    public float waveInc = 20;
    public int currentWaveNumber = 5;

    public GUIText label;
    int points = 0;

    public override void begin()
    {
       FP.log("game stage begin");
       fade(true, 1.5f, null);
       Invoke("Spawn", 2);
       Invoke("Spawn", 3);
       Invoke("Spawn", 2);
 
    }

    public override void end()
    {
     


    }
    void fadeComplete()
    {
        loadLevel("scene0");
    }

    void Spawn()
    {

        if (currentEnemyCount < currentWaveNumber)
            {
             GameObject obj = ObjectPool.instance.GetObjectForType("enemy",true);
             Badchip b = obj.GetComponent<Badchip>();
            if (b != null)
            {
                b.isKill = BadisKill;
                b.acc = Random.Range(20, 30);
                b.x = Random.Range(5, 310);
                b.y = Random.Range(485, 555);
                currentEnemyCount++;
            }
        }
            
    }
    void BadisKill()
    {
        currentEnemyCount --;
        points += 10;
    }
    public override void update()
    {
        base.update();


        label.text = "Points:" + points;
        
                        if (spawnDelay > 1)
	                    {
                            Invoke("Spawn", 0.5f);
	                        spawnDelay = 0;                       
	                    }
	                    spawnDelay += Time.deltaTime;

           waveTime+= Time.deltaTime;
        if (waveTime >= waveInc)
        {
            waveTime=0;
            waveInc+=10;
            currentWaveNumber+=2;
        }

        if (currentWaveNumber >= 10)
        {
            currentWaveNumber = 8;
        }
  



        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            fade(false, 1.5f, fadeComplete);
        }
    }
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Particle
{
         public float x;
        public float y;
        public float scale;
        public float rotation;
        public Color color;
        public float alpha;
        public float currentTime;
        public float totalTime;
		public Color colorArgb;
        public Color colorArgbDelta;
        public float startX;
		public float startY;
        public float velocityX;
		public float velocityY;
        public float radialAcceleration;
        public float tangentialAcceleration;
        public float emitRadius; 
		public float emitRadiusDelta;
        public float emitRotation; 
		public float emitRotationDelta;
        public float rotationDelta;
        public float scaleDelta;

        public Particle()
        {
            x = y = rotation = currentTime = 0.0f;
            totalTime = alpha = scale = 1.0f;
            color = new Color(1, 1, 1, 1);
            colorArgb = new Color(0, 0, 0, 0);
            colorArgbDelta = new Color(0, 0, 0, 0);
        }
}
public class ParticlesSystem  {

         private int EMITTER_TYPE_GRAVITY = 0;
         private int EMITTER_TYPE_RADIAL  = 1;
        
        // emitter configuration                            // .pex element name
        private int mEmitterType;                       // emitterType
        private float mEmitterXVariance;               // sourcePositionVariance x
        private float mEmitterYVariance;               // sourcePositionVariance y
        
        // particle configuration
        private int mMaxNumParticles;                   // maxParticles
        private float mLifespan;                       // particleLifeSpan
        private float mLifespanVariance;               // particleLifeSpanfloatiance
        private float mStartSize;                      // startParticleSize
        private float mStartSizeVariance;              // startParticleSizefloatiance
        private float mEndSize;                        // finishParticleSize
        private float mEndSizeVariance;                // finishParticleSizefloatiance
        private float mEmitAngle;                      // angle
        private float mEmitAngleVariance;              // anglefloatiance
        private float mStartRotation;                  // rotationStart
        private float mStartRotationVariance;          // rotationStartfloatiance
        private float mEndRotation;                    // rotationEnd
        private float mEndRotationVariance;            // rotationEndfloatiance
        
        // gravity configuration
        private float mSpeed;                          // speed
        private float mSpeedVariance;                  // speedfloatiance
        private float mGravityX;                       // gravity x
        private float mGravityY;                       // gravity y
        private float mRadialAcceleration;             // radialAcceleration
        private float mRadialAccelerationVariance;     // radialAccelerationfloatiance
        private float mTangentialAcceleration;         // tangentialAcceleration
        private float mTangentialAccelerationVariance; // tangentialAccelerationfloatiance
        
        // radial configuration 
        private float mMaxRadius;                      // maxRadius
        private float mMaxRadiusVariance;              // maxRadiusfloatiance
        private float mMinRadius;                      // minRadius
        private float mRotatePerSecond;                // rotatePerSecond
        private float mRotatePerSecondVariance;        // rotatePerSecondfloatiance
        
        // color configuration
        private Color mStartColor;                  // startColor
        private Color mStartColorVariance;          // startColorVariance
        private Color mEndColor;                    // finishColor
        private Color mEndColorVariance;            // finishColorVariance
		
	
        private List<Particle> mParticles;

        private float mFrameTime;
         private int mNumParticles;
        private int mMaxCapacity;
        private float mEmissionRate; // emitted particles per second
        private float mEmissionTime;
        
        private float mEmitterX;
        private float mEmitterY;
		private Rect uvclip;

        private int texture_width;

        public ParticlesSystem(int texturewidth,int total, float EmissionRate,Rect textureclip)
        {
            uvclip = textureclip;
            mParticles = new List<Particle>(total);
            texture_width = texturewidth;

            mMaxNumParticles = total;
            mMaxCapacity = mMaxNumParticles;

            mEmissionRate = EmissionRate;
            mEmissionTime = 0f;
            mFrameTime = 0f;
	    	mNumParticles = 0;
            updateEmissionRate();
	
                          mEmitterX = mEmitterY = 0;
			              mStartColor = new Color(0, 0, 0, 0);
						  mStartColorVariance = new Color(0, 0, 0, 0);
						  mEndColor = new Color(0, 0, 0, 0);
						  mEndColorVariance = new Color(0, 0, 0, 0);


                          mEmitterX = 0;
                          mEmitterY = 0;

            mEmitterXVariance=0;
            mEmitterYVariance = 0;

            mGravityX = 0;
            mGravityY = 0;

            mEmitterType = 1;

            mLifespan = Mathf.Max(0.01f, 2.0f);
            mLifespanVariance = 1.90f;

            mStartSize = 70;
            mStartSizeVariance = 5;
            mEndSize = 10;
            mEndSizeVariance = 5;

            mEmitAngle = 270 * Mathf.Deg2Rad;
            mEmitAngleVariance = 2 * Mathf.Deg2Rad;

            mStartRotation = 0 * Mathf.Deg2Rad;
            mStartRotationVariance = 0 * Mathf.Deg2Rad;
            mEndRotation = 0 * Mathf.Deg2Rad;
            mEndRotationVariance = 0 * Mathf.Deg2Rad;

            mSpeed = 100;
            mSpeedVariance = 30;

            mRadialAcceleration = 0;
            mRadialAccelerationVariance = 0;
            mTangentialAcceleration = 0;
            mTangentialAccelerationVariance = 0;

            mMaxRadius = 100;
            mMaxRadiusVariance = 0;
            mMinRadius = 0;

            mRotatePerSecond = 0 * Mathf.Deg2Rad;
            mRotatePerSecondVariance = 0 * Mathf.Deg2Rad;

            mStartColor = Color.red;
            mStartColorVariance = Color.red;
            mEndColor = Color.white;
            mEndColorVariance = Color.white;






                          for (int i = 0; i <= mMaxNumParticles;i++ )
                          {
                              mParticles.Add(createParticle());
                          }
        }
      public void setLifespan(float value)
        { 
            mLifespan = Mathf.Max(0.01f, value);
            updateEmissionRate();
        }
     public void setMaxNumParticles(int value)
        { 
            mMaxNumParticles = value; 
            updateEmissionRate(); 
        }
      private Particle createParticle()
        {
            return new Particle();
        }
     private void updateEmissionRate()
        {
            mEmissionRate = mMaxNumParticles / mLifespan;
        }

    private float random()
    {
        return Random.value;
    }
     private  void initParticle(Particle particle)
        {

		    
            float lifespan = mLifespan + mLifespanVariance * (random() * 2.0f - 1.0f); 
            if (lifespan <= 0.0) return;
            
            particle.currentTime = 0.0f;
            particle.totalTime = lifespan;
            
            particle.x = mEmitterX + mEmitterXVariance * (random() * 2.0f - 1.0f);
            particle.y = mEmitterY + mEmitterYVariance * (random() * 2.0f - 1.0f);
            particle.startX = mEmitterX;
            particle.startY = mEmitterY;
            
            float angle = mEmitAngle + mEmitAngleVariance * (random() * 2.0f - 1.0f);
            float speed = mSpeed + mSpeedVariance * (random() * 2.0f - 1.0f);
            particle.velocityX = speed * Mathf.Cos(angle);
            particle.velocityY = speed * Mathf.Sin(angle);
            
            particle.emitRadius = mMaxRadius + mMaxRadiusVariance * (random() * 2.0f - 1.0f);
            particle.emitRadiusDelta = mMaxRadius / lifespan;
            particle.emitRotation = mEmitAngle + mEmitAngleVariance * (random() * 2.0f - 1.0f); 
            particle.emitRotationDelta = mRotatePerSecond + mRotatePerSecondVariance * (random() * 2.0f - 1.0f); 
            particle.radialAcceleration = mRadialAcceleration + mRadialAccelerationVariance * (random() * 2.0f - 1.0f);
            particle.tangentialAcceleration = mTangentialAcceleration + mTangentialAccelerationVariance * (random() * 2.0f - 1.0f);
            
            float startSize = mStartSize + mStartSizeVariance * (random() * 2.0f - 1.0f); 
            float endSize = mEndSize + mEndSizeVariance * (random() * 2.0f - 1.0f);
            if (startSize < 0.1) startSize = 0.1f;
            if (endSize < 0.1)   endSize = 0.1f;
            particle.scale = startSize / texture_width;
            particle.scaleDelta = ((endSize - startSize) / lifespan) /texture_width;
            
            // colors
            
            Color startColor = particle.colorArgb;
            Color colorDelta = particle.colorArgbDelta;
            
            startColor.r   = mStartColor.r;
            startColor.g = mStartColor.g;
            startColor.b  = mStartColor.b;
            startColor.a = mStartColor.a;
            
            if (mStartColorVariance.r != 0)   startColor.r   += mStartColorVariance.r   * (random() * 2.0f - 1.0f);
            if (mStartColorVariance.g != 0) startColor.g += mStartColorVariance.g * (random() * 2.0f - 1.0f);
            if (mStartColorVariance.b != 0)  startColor.b  += mStartColorVariance.b  * (random() * 2.0f - 1.0f);
            if (mStartColorVariance.a != 0) startColor.a += mStartColorVariance.a * (random() * 2.0f - 1.0f);
            
            float endColorRed   = mEndColor.r;
            float endColorGreen = mEndColor.g;
            float endColorBlue  = mEndColor.b;
            float endColorAlpha = mEndColor.a;

            if (mEndColorVariance.r != 0)   endColorRed   += mEndColorVariance.r   * (random() * 2.0f - 1.0f);
            if (mEndColorVariance.g != 0) endColorGreen += mEndColorVariance.g * (random() * 2.0f - 1.0f);
            if (mEndColorVariance.b != 0)  endColorBlue  += mEndColorVariance.b  * (random() * 2.0f - 1.0f);
            if (mEndColorVariance.a != 0) endColorAlpha += mEndColorVariance.a * (random() * 2.0f - 1.0f);
            
            colorDelta.r   = (endColorRed   - startColor.r)   / lifespan;
            colorDelta.g = (endColorGreen - startColor.g) / lifespan;
            colorDelta.b  = (endColorBlue  - startColor.b)  / lifespan;
            colorDelta.a = (endColorAlpha - startColor.a) / lifespan;
            
            // rotation
            
            float startRotation = mStartRotation + mStartRotationVariance * (random() * 2.0f - 1.0f); 
            float endRotation   = mEndRotation   + mEndRotationVariance   * (random() * 2.0f - 1.0f);
            
            particle.rotation = startRotation;
            particle.rotationDelta = (endRotation - startRotation) / lifespan;
        }
      private void advanceParticle(Particle particle, float passedTime)
        {
			  
            float restTime = particle.totalTime - particle.currentTime;
            passedTime = restTime > passedTime ? passedTime : restTime;
            particle.currentTime += passedTime;
            
            if (mEmitterType == EMITTER_TYPE_RADIAL)
            {
                particle.emitRotation += particle.emitRotationDelta * passedTime;
                particle.emitRadius   -= particle.emitRadiusDelta   * passedTime;
                particle.x = mEmitterX - Mathf.Cos(particle.emitRotation) * particle.emitRadius;
                particle.y = mEmitterY - Mathf.Sin(particle.emitRotation) * particle.emitRadius;
                
                if (particle.emitRadius < mMinRadius)
                    particle.currentTime = particle.totalTime;
            }
            else
            {
                float distanceX = particle.x - particle.startX;
                float distanceY = particle.y - particle.startY;
                float distanceScalar = Mathf.Sqrt(distanceX*distanceX + distanceY*distanceY);
                if (distanceScalar < 0.01f) distanceScalar = 0.01f;
                
                float radialX = distanceX / distanceScalar;
                float radialY = distanceY / distanceScalar;
                var tangentialX = radialX;
                var tangentialY = radialY;
                
                radialX *= particle.radialAcceleration;
                radialY *= particle.radialAcceleration;
                
                float newY = tangentialX;
                tangentialX = -tangentialY * particle.tangentialAcceleration;
                tangentialY = newY * particle.tangentialAcceleration;
                
                particle.velocityX += passedTime * (mGravityX + radialX + tangentialX);
                particle.velocityY += passedTime * (mGravityY + radialY + tangentialY);
                particle.x += particle.velocityX * passedTime;
                particle.y += particle.velocityY * passedTime;
            }
            
            particle.scale += particle.scaleDelta * passedTime;
            particle.rotation += particle.rotationDelta * passedTime;
            
            particle.colorArgb.r   += particle.colorArgbDelta.r   * passedTime;
            particle.colorArgb.g += particle.colorArgbDelta.g * passedTime;
            particle.colorArgb.b  += particle.colorArgbDelta.b  * passedTime;
            particle.colorArgb.a += particle.colorArgbDelta.a * passedTime;
            
            particle.color = particle.colorArgb;
            particle.alpha = particle.colorArgb.a;
        }
    public void start(float duration= float.MaxValue)
        {
            if (mEmissionRate != 0)                
                mEmissionTime = duration;
        }
        
        public void stop(bool clear=false)
        {
            mEmissionTime = 0.0f;
            if (clear) mNumParticles = 0;
        }
    	public void update(float passedTime)
        {
            int particleIndex= 0;
            Particle particle;
            
            // advance existing particles
            
            while (particleIndex < mNumParticles)
            {
                particle = mParticles[particleIndex];
                
                if (particle.currentTime < particle.totalTime)
                {
                    advanceParticle(particle, passedTime);
                    ++particleIndex;
                }
                else
                {
                    if (particleIndex != mNumParticles - 1)
                    {
                        Particle nextParticle = mParticles[mNumParticles-1] ;
                        mParticles[mNumParticles-1] = particle;
                        mParticles[particleIndex] = nextParticle;
					//	trace("create particle");
                    }
                    
                    --mNumParticles;
                    
                    if (mNumParticles == 0)
					{
						
					}
                }
            }
            
            // create and advance new particles
         
            if (mEmissionTime > 0)
            {
                float timeBetweenParticles = 1.0f / mEmissionRate;
                mFrameTime += passedTime;
				
			//	trace(mNumParticles+'>'+mMaxCapacity);
                
                while (mFrameTime > 0)
                {
                    if (mNumParticles < mMaxCapacity)
                    {
						
                        particle = mParticles[mNumParticles++] ;
                        initParticle(particle);
                        advanceParticle(particle, mFrameTime);
                    }
                    
                    mFrameTime -= timeBetweenParticles;
                }
                
                if (mEmissionTime != 10000f)
                    mEmissionTime = Mathf.Max(0.0f, mEmissionTime - passedTime);

		}
		
	
     
        }

    	 public void renderBatch(SpriteBatch batch)
		{
		
			
			float alpha;
            float rotation;
            float x;
			float y;
            float xOffset;
			float yOffset;
			Color color;
		
			
		int count = 0;
		for (int i=0; i<=mNumParticles;i++)
		{
			
			    Particle particle = mParticles[i];
	            color = particle.color;
                alpha = particle.alpha;
                color.a = alpha;
                rotation = particle.rotation;
	
				
			
			x =  particle.x;
		    y =  particle.y;



            xOffset = (uvclip.width / 2 * particle.scale);
            yOffset = (uvclip.height / 2 * particle.scale);
				
		
				if (alpha <= 0) continue;
				count++;				
		
				
				
					
				batch.drawVertex(
                x - xOffset, y - yOffset,
                x - xOffset, y + yOffset,
                x + xOffset, y + yOffset,
                x + xOffset, y - yOffset,
				uvclip.x,uvclip.y,uvclip.width,uvclip.height,  color);
                	
		}
		
		//trace(count);
		
		}
}

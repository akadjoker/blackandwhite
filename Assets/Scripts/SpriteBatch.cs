using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class SpriteBatch : MonoBehaviour {

    
    public Material material;
    public int batchSize=100;

    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;
    protected Mesh mesh;					

    protected Vector3[] vertices;			
    protected int[] triIndices;				
    protected Vector2[] UVs;				
    protected Color[] colors;				
    protected Vector3[] normals;		

    private float invTexWidth;
    private float invTexHeight;
    private Texture texture;
    private int currentBatchSize;
    private int tex_height;


    void Awake()
    {
        gameObject.AddComponent("MeshFilter");
        gameObject.AddComponent("MeshRenderer");

        meshFilter = (MeshFilter)GetComponent(typeof(MeshFilter));
        meshRenderer = (MeshRenderer)GetComponent(typeof(MeshRenderer));

        meshRenderer.renderer.material = material;
        texture = meshRenderer.renderer.material.GetTexture(0);

        invTexWidth = 1.0f / texture.width;
        invTexHeight = 1.0f / texture.height;
        tex_height = texture.height;

        mesh = meshFilter.mesh;


        setupArray(batchSize);


        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        currentBatchSize = 0;

    }
        
    public void begin()
    {
        currentBatchSize = 0;
        triIndices = null;
        mesh.Clear();
    }


    public void end()
    {


        int len = currentBatchSize * 6;
        triIndices = new int[len];

        int j = 0;
        for (int i = 0; i < len; i += 6, j += 4)
        {
            triIndices[i] = j;
            triIndices[i + 1] = (j + 1);
            triIndices[i + 2] = (j + 2);
            triIndices[i + 3] = (j + 2);
            triIndices[i + 4] = (j + 3);
            triIndices[i + 5] = j;
        }


        mesh.vertices = vertices;
        mesh.uv = UVs;
        mesh.colors = colors;
        mesh.normals = normals;
        mesh.triangles = triIndices;
        mesh.RecalculateBounds();
        

    }

    protected void  setupArray(int count)
    {

    
		vertices = new Vector3[count * 4];
        UVs = new Vector2[count * 4];
        colors = new Color[count * 4];
        normals = new Vector3[count * 4];

        for (int i = 0; i < count * 4; i++)
        {
            colors[i] = Color.white;
            normals[i] = new Vector3(0.0f, 0.0f, 1.0f);
        }
          



    }



    public void draw(float x, float y, float width, float height)
    {

         float fx2 = x + width;
		 float fy2 = y + height;
		 float u = 0f;
		 float v = 1f;
		 float u2 = 1f;
		 float v2 = 0f;

        int idx = currentBatchSize * 4;

        vertices[idx + 0].x = x;
        vertices[idx + 0].y = y;
        vertices[idx + 0].z = 0f;

        vertices[idx + 1].x = x;
        vertices[idx + 1].y = fy2;
        vertices[idx + 1].z = 0f;

        vertices[idx + 2].x = fx2;
        vertices[idx + 2].y = fy2;
        vertices[idx + 2].z = 0f;

        vertices[idx + 3].x = fx2;
        vertices[idx + 3].y = y;
        vertices[idx + 3].z = 0f;


        UVs[idx + 0].x = u;
        UVs[idx + 0].y = v;

     
        UVs[idx + 1].x = u;
        UVs[idx + 1].y = v2;


        UVs[idx + 2].x = u2;
        UVs[idx + 2].y = v2;


        UVs[idx + 3].x = u2;
        UVs[idx + 3].y = v;

        currentBatchSize++;
    }


    public void draw(float x, float y, float srcX, float srcY, float srcWidth, float srcHeight)
    {

        float clipY = (tex_height - srcY - srcHeight);

         float u = srcX * invTexWidth;
         float v = (clipY + srcHeight) * invTexHeight;
		 float u2 = (srcX + srcWidth) * invTexWidth;
         float v2 = clipY * invTexHeight;


		 float fx2 = x + srcWidth;
		 float fy2 = y + srcHeight;


 

         int idx = currentBatchSize * 4;

         vertices[idx + 0].x = x;
         vertices[idx + 0].y = y;
         vertices[idx + 0].z = 0f;

         vertices[idx + 1].x = x;
         vertices[idx + 1].y = fy2;
         vertices[idx + 1].z = 0f;

         vertices[idx + 2].x = fx2;
         vertices[idx + 2].y = fy2;
         vertices[idx + 2].z = 0f;

         vertices[idx + 3].x = fx2;
         vertices[idx + 3].y = y;
         vertices[idx + 3].z = 0f;


         UVs[idx + 0].x = u;
         UVs[idx + 0].y = v2;


         UVs[idx + 1].x = u;
         UVs[idx + 1].y = v;


         UVs[idx + 2].x = u2;
         UVs[idx + 2].y = v;


         UVs[idx + 3].x = u2;
         UVs[idx + 3].y = v2;
         currentBatchSize++;
    }

    public void draw(float x, float y, float srcX, float srcY, float srcWidth, float srcHeight, bool flipX, bool flipY)
    {

        float clipY = (tex_height - srcY - srcHeight);

        float u = srcX * invTexWidth;
        float v = (clipY + srcHeight) * invTexHeight;
        float u2 = (srcX + srcWidth) * invTexWidth;
        float v2 = clipY * invTexHeight;

        if (flipX)
        {
            float tmp = u;
            u = u2;
            u2 = tmp;
        }

        if (flipY)
        {
            float tmp = v;
            v = v2;
            v2 = tmp;
        }

        float fx2 = x + srcWidth;
        float fy2 = y + srcHeight;




        int idx = currentBatchSize * 4;

        vertices[idx + 0].x = x;
        vertices[idx + 0].y = y;
        vertices[idx + 0].z = 0f;

        vertices[idx + 1].x = x;
        vertices[idx + 1].y = fy2;
        vertices[idx + 1].z = 0f;

        vertices[idx + 2].x = fx2;
        vertices[idx + 2].y = fy2;
        vertices[idx + 2].z = 0f;

        vertices[idx + 3].x = fx2;
        vertices[idx + 3].y = y;
        vertices[idx + 3].z = 0f;


        UVs[idx + 0].x = u;
        UVs[idx + 0].y = v2;


        UVs[idx + 1].x = u;
        UVs[idx + 1].y = v;


        UVs[idx + 2].x = u2;
        UVs[idx + 2].y = v;


        UVs[idx + 3].x = u2;
        UVs[idx + 3].y = v2;
        currentBatchSize++;
    }

    public void drawVertex(
    float x1,
    float y1,
    float x2,
    float y2,
    float x3,
    float y3,
    float x4,
    float y4,
    float srcX, float srcY, float srcWidth, float srcHeight, bool flipX, bool flipY)
    {
        float clipY = (tex_height - srcY - srcHeight);
        float u = srcX * invTexWidth;
        float v = (clipY + srcHeight) * invTexHeight;
        float u2 = (srcX + srcWidth) * invTexWidth;
        float v2 = clipY * invTexHeight;

        if (flipX)
        {
            float tmp = u;
            u = u2;
            u2 = tmp;
        }

        if (flipY)
        {
            float tmp = v;
            v = v2;
            v2 = tmp;
        }

        int idx = currentBatchSize * 4;

        vertices[idx + 0].x = x1;
        vertices[idx + 0].y = y1;
        vertices[idx + 0].z = 0f;

        vertices[idx + 1].x = x2;
        vertices[idx + 1].y = x2;
        vertices[idx + 1].z = 0f;

        vertices[idx + 2].x = x3;
        vertices[idx + 2].y = y3;
        vertices[idx + 2].z = 0f;

        vertices[idx + 3].x = x4;
        vertices[idx + 3].y = y4;
        vertices[idx + 3].z = 0f;


        UVs[idx + 0].x = u;
        UVs[idx + 0].y = v2;


        UVs[idx + 1].x = u;
        UVs[idx + 1].y = v;


        UVs[idx + 2].x = u2;
        UVs[idx + 2].y = v;


        UVs[idx + 3].x = u2;
        UVs[idx + 3].y = v2;
        currentBatchSize++;
    }
    public void drawVertex(
   float x1,
   float y1,
   float x2,
   float y2,
   float x3,
   float y3,
   float x4,
   float y4,
   float srcX, float srcY, float srcWidth, float srcHeight)
    {
        float clipY = (tex_height - srcY - srcHeight);
        float u = srcX * invTexWidth;
        float v = (clipY + srcHeight) * invTexHeight;
        float u2 = (srcX + srcWidth) * invTexWidth;
        float v2 = clipY * invTexHeight;


        int idx = currentBatchSize * 4;

        vertices[idx + 0].x = x1;
        vertices[idx + 0].y = y1;
        vertices[idx + 0].z = 0f;

        vertices[idx + 1].x = x2;
        vertices[idx + 1].y = x2;
        vertices[idx + 1].z = 0f;

        vertices[idx + 2].x = x3;
        vertices[idx + 2].y = y3;
        vertices[idx + 2].z = 0f;

        vertices[idx + 3].x = x4;
        vertices[idx + 3].y = y4;
        vertices[idx + 3].z = 0f;


        UVs[idx + 0].x = u;
        UVs[idx + 0].y = v2;


        UVs[idx + 1].x = u;
        UVs[idx + 1].y = v;


        UVs[idx + 2].x = u2;
        UVs[idx + 2].y = v;


        UVs[idx + 3].x = u2;
        UVs[idx + 3].y = v2;
        currentBatchSize++;
    }
    public void drawVertex(
 float x1,
 float y1,
 float x2,
 float y2,
 float x3,
 float y3,
 float x4,
 float y4,
 float srcX, float srcY, float srcWidth, float srcHeight,Color c)
    {
        float clipY = (tex_height - srcY - srcHeight);
        float u = srcX * invTexWidth;
        float v = (clipY + srcHeight) * invTexHeight;
        float u2 = (srcX + srcWidth) * invTexWidth;
        float v2 = clipY * invTexHeight;


        int idx = currentBatchSize * 4;

        vertices[idx + 0].x = x1;
        vertices[idx + 0].y = y1;
        vertices[idx + 0].z = 0f;

        vertices[idx + 1].x = x2;
        vertices[idx + 1].y = x2;
        vertices[idx + 1].z = 0f;

        vertices[idx + 2].x = x3;
        vertices[idx + 2].y = y3;
        vertices[idx + 2].z = 0f;

        vertices[idx + 3].x = x4;
        vertices[idx + 3].y = y4;
        vertices[idx + 3].z = 0f;

        colors[idx + 0] = c;
        colors[idx + 1] = c;
        colors[idx + 2] = c;
        colors[idx + 3] = c;


        UVs[idx + 0].x = u;
        UVs[idx + 0].y = v2;


        UVs[idx + 1].x = u;
        UVs[idx + 1].y = v;


        UVs[idx + 2].x = u2;
        UVs[idx + 2].y = v;


        UVs[idx + 3].x = u2;
        UVs[idx + 3].y = v2;
        currentBatchSize++;
    }
    public void draw(float x, float y,float spin,float size, float srcX, float srcY, float srcWidth, float srcHeight)
    {
        float xOffset = (srcWidth / 2f);
        float yOffset = (srcHeight / 2f);        
	
 float  TX1 = -xOffset * size;
 float  TY1 = -yOffset * size;
 float  TX2 = (srcWidth - xOffset) * size;
 float  TY2 = (srcHeight - yOffset) * size;

 float CosT = Mathf.Cos(spin * Mathf.PI / 180);
 float SinT = Mathf.Sin(spin * Mathf.PI / 180);
                 	
      drawVertex(
 TX1 * CosT - TY1 * SinT + x,TX1 * SinT + TY1 * CosT + y,
 TX2 * CosT - TY1 * SinT + x,TX2 * SinT + TY1 * CosT + y,
 TX2 * CosT - TY2 * SinT + x,TX2 * SinT + TY2 * CosT + y,
 TX1 * CosT - TY2 * SinT + x,TX1 * SinT + TY2 * CosT + y,
 srcX,  srcY, srcWidth, srcHeight);
    }
    public void draw(float x, float y, float spin, float size, float srcX, float srcY, float srcWidth, float srcHeight, bool flipX, bool flipY)
    {
        float xOffset = (srcWidth / 2f);
        float yOffset = (srcHeight / 2f);

        float TX1 = -xOffset * size;
        float TY1 = -yOffset * size;
        float TX2 = (srcWidth - xOffset) * size;
        float TY2 = (srcHeight - yOffset) * size;

        float CosT = Mathf.Cos(spin * Mathf.PI / 180);
        float SinT = Mathf.Sin(spin * Mathf.PI / 180);

        drawVertex(
   TX1 * CosT - TY1 * SinT + x, TX1 * SinT + TY1 * CosT + y,
   TX2 * CosT - TY1 * SinT + x, TX2 * SinT + TY1 * CosT + y,
   TX2 * CosT - TY2 * SinT + x, TX2 * SinT + TY2 * CosT + y,
   TX1 * CosT - TY2 * SinT + x, TX1 * SinT + TY2 * CosT + y,
   srcX, srcY, srcWidth, srcHeight,  flipX,  flipY);
    }

    public void draw(float x, float y, float originX, float originY, float width, float height, float scaleX,
        float scaleY, float rotation, int srcX, int srcY, int srcWidth, int srcHeight, bool flipX, bool flipY)
    {
         float worldOriginX = x + originX;
		 float worldOriginY = y + originY;
		float fx = -originX;
		float fy = -originY;
		float fx2 = width - originX;
		float fy2 = height - originY;

		// scale
		if (scaleX != 1 || scaleY != 1) {
			fx *= scaleX;
			fy *= scaleY;
			fx2 *= scaleX;
			fy2 *= scaleY;
		}

	
		 float p1x = fx;
		 float p1y = fy;
		 float p2x = fx;
		 float p2y = fy2;
		 float p3x = fx2;
		 float p3y = fy2;
		 float p4x = fx2;
		 float p4y = fy;

		float x1;
		float y1;
		float x2;
		float y2;
		float x3;
		float y3;
		float x4;
		float y4;

		// rotate
		if (rotation != 0) {
            float cos = Mathf.Cos(rotation * Mathf.PI / 180);
            float sin = Mathf.Sin(rotation * Mathf.PI / 180);

			x1 = cos * p1x - sin * p1y;
			y1 = sin * p1x + cos * p1y;

			x2 = cos * p2x - sin * p2y;
			y2 = sin * p2x + cos * p2y;

			x3 = cos * p3x - sin * p3y;
			y3 = sin * p3x + cos * p3y;

			x4 = x1 + (x3 - x2);
			y4 = y3 - (y2 - y1);
		} else {
			x1 = p1x;
			y1 = p1y;

			x2 = p2x;
			y2 = p2y;

			x3 = p3x;
			y3 = p3y;

			x4 = p4x;
			y4 = p4y;
		}

		x1 += worldOriginX;
		y1 += worldOriginY;
		x2 += worldOriginX;
		y2 += worldOriginY;
		x3 += worldOriginX;
		y3 += worldOriginY;
		x4 += worldOriginX;
		y4 += worldOriginY;

        float clipY = (tex_height - srcY - srcHeight);

        float u = srcX * invTexWidth;
        float v = (clipY + srcHeight) * invTexHeight;
        float u2 = (srcX + srcWidth) * invTexWidth;
        float v2 = clipY * invTexHeight;

		if (flipX) {
			float tmp = u;
			u = u2;
			u2 = tmp;
		}

		if (flipY) {
			float tmp = v;
			v = v2;
			v2 = tmp;
		}

        int idx = currentBatchSize * 4;

        vertices[idx + 0].x = x1;
        vertices[idx + 0].y = y1;
        vertices[idx + 0].z = 0f;

        vertices[idx + 1].x = x2;
        vertices[idx + 1].y = y2;
        vertices[idx + 1].z = 0f;

        vertices[idx + 2].x = x3;
        vertices[idx + 2].y = y3;
        vertices[idx + 2].z = 0f;

        vertices[idx + 3].x = x4;
        vertices[idx + 3].y = y4;
        vertices[idx + 3].z = 0f;


        UVs[idx + 0].x = u;
        UVs[idx + 0].y = v2;


        UVs[idx + 1].x = u;
        UVs[idx + 1].y = v;


        UVs[idx + 2].x = u2;
        UVs[idx + 2].y = v;


        UVs[idx + 3].x = u2;
        UVs[idx + 3].y = v2;
        currentBatchSize++;

    }

    public void draw(float x, float y, float originX, float originY, float width, float height, float scaleX,
       float scaleY, float rotation, int srcX, int srcY, int srcWidth, int srcHeight, bool flipX, bool flipY,Color c)
    {
        float worldOriginX = x + originX;
        float worldOriginY = y + originY;
        float fx = -originX;
        float fy = -originY;
        float fx2 = width - originX;
        float fy2 = height - originY;

        // scale
        if (scaleX != 1 || scaleY != 1)
        {
            fx *= scaleX;
            fy *= scaleY;
            fx2 *= scaleX;
            fy2 *= scaleY;
        }


        float p1x = fx;
        float p1y = fy;
        float p2x = fx;
        float p2y = fy2;
        float p3x = fx2;
        float p3y = fy2;
        float p4x = fx2;
        float p4y = fy;

        float x1;
        float y1;
        float x2;
        float y2;
        float x3;
        float y3;
        float x4;
        float y4;

        // rotate
        if (rotation != 0)
        {
            float cos = Mathf.Cos(rotation * Mathf.PI / 180);
            float sin = Mathf.Sin(rotation * Mathf.PI / 180);

            x1 = cos * p1x - sin * p1y;
            y1 = sin * p1x + cos * p1y;

            x2 = cos * p2x - sin * p2y;
            y2 = sin * p2x + cos * p2y;

            x3 = cos * p3x - sin * p3y;
            y3 = sin * p3x + cos * p3y;

            x4 = x1 + (x3 - x2);
            y4 = y3 - (y2 - y1);
        }
        else
        {
            x1 = p1x;
            y1 = p1y;

            x2 = p2x;
            y2 = p2y;

            x3 = p3x;
            y3 = p3y;

            x4 = p4x;
            y4 = p4y;
        }

        x1 += worldOriginX;
        y1 += worldOriginY;
        x2 += worldOriginX;
        y2 += worldOriginY;
        x3 += worldOriginX;
        y3 += worldOriginY;
        x4 += worldOriginX;
        y4 += worldOriginY;

        float clipY = (tex_height - srcY - srcHeight);

        float u = srcX * invTexWidth;
        float v = (clipY + srcHeight) * invTexHeight;
        float u2 = (srcX + srcWidth) * invTexWidth;
        float v2 = clipY * invTexHeight;

        if (flipX)
        {
            float tmp = u;
            u = u2;
            u2 = tmp;
        }

        if (flipY)
        {
            float tmp = v;
            v = v2;
            v2 = tmp;
        }

        int idx = currentBatchSize * 4;

        vertices[idx + 0].x = x1;
        vertices[idx + 0].y = y1;
        vertices[idx + 0].z = 0f;

        vertices[idx + 1].x = x2;
        vertices[idx + 1].y = y2;
        vertices[idx + 1].z = 0f;

        vertices[idx + 2].x = x3;
        vertices[idx + 2].y = y3;
        vertices[idx + 2].z = 0f;

        vertices[idx + 3].x = x4;
        vertices[idx + 3].y = y4;
        vertices[idx + 3].z = 0f;

        colors[idx + 0] = c;
        colors[idx + 1] = c;
        colors[idx + 2] = c;
        colors[idx + 3] = c;


        UVs[idx + 0].x = u;
        UVs[idx + 0].y = v2;


        UVs[idx + 1].x = u;
        UVs[idx + 1].y = v;


        UVs[idx + 2].x = u2;
        UVs[idx + 2].y = v;


        UVs[idx + 3].x = u2;
        UVs[idx + 3].y = v2;
        currentBatchSize++;

    }
    public void draw(float x, float y, float originX, float originY, float scaleX,
      float scaleY, float rotation, int srcX, int srcY, int srcWidth, int srcHeight, bool flipX, bool flipY, Color c)
    {
        float worldOriginX = x + originX;
        float worldOriginY = y + originY;
        float fx = -originX;
        float fy = -originY;
        float fx2 = srcWidth - originX;
        float fy2 = srcHeight - originY;

        // scale
        if (scaleX != 1 || scaleY != 1)
        {
            fx *= scaleX;
            fy *= scaleY;
            fx2 *= scaleX;
            fy2 *= scaleY;
        }


        float p1x = fx;
        float p1y = fy;
        float p2x = fx;
        float p2y = fy2;
        float p3x = fx2;
        float p3y = fy2;
        float p4x = fx2;
        float p4y = fy;

        float x1;
        float y1;
        float x2;
        float y2;
        float x3;
        float y3;
        float x4;
        float y4;

        // rotate
        if (rotation != 0)
        {
            float cos = Mathf.Cos(rotation * Mathf.PI / 180);
            float sin = Mathf.Sin(rotation * Mathf.PI / 180);

            x1 = cos * p1x - sin * p1y;
            y1 = sin * p1x + cos * p1y;

            x2 = cos * p2x - sin * p2y;
            y2 = sin * p2x + cos * p2y;

            x3 = cos * p3x - sin * p3y;
            y3 = sin * p3x + cos * p3y;

            x4 = x1 + (x3 - x2);
            y4 = y3 - (y2 - y1);
        }
        else
        {
            x1 = p1x;
            y1 = p1y;

            x2 = p2x;
            y2 = p2y;

            x3 = p3x;
            y3 = p3y;

            x4 = p4x;
            y4 = p4y;
        }

        x1 += worldOriginX;
        y1 += worldOriginY;
        x2 += worldOriginX;
        y2 += worldOriginY;
        x3 += worldOriginX;
        y3 += worldOriginY;
        x4 += worldOriginX;
        y4 += worldOriginY;

        float clipY = (tex_height - srcY - srcHeight);

        float u = srcX * invTexWidth;
        float v = (clipY + srcHeight) * invTexHeight;
        float u2 = (srcX + srcWidth) * invTexWidth;
        float v2 = clipY * invTexHeight;

        if (flipX)
        {
            float tmp = u;
            u = u2;
            u2 = tmp;
        }

        if (flipY)
        {
            float tmp = v;
            v = v2;
            v2 = tmp;
        }

        int idx = currentBatchSize * 4;

        vertices[idx + 0].x = x1;
        vertices[idx + 0].y = y1;
        vertices[idx + 0].z = 0f;

        vertices[idx + 1].x = x2;
        vertices[idx + 1].y = y2;
        vertices[idx + 1].z = 0f;

        vertices[idx + 2].x = x3;
        vertices[idx + 2].y = y3;
        vertices[idx + 2].z = 0f;

        vertices[idx + 3].x = x4;
        vertices[idx + 3].y = y4;
        vertices[idx + 3].z = 0f;

        colors[idx + 0] = c;
        colors[idx + 1] = c;
        colors[idx + 2] = c;
        colors[idx + 3] = c;


        UVs[idx + 0].x = u;
        UVs[idx + 0].y = v2;


        UVs[idx + 1].x = u;
        UVs[idx + 1].y = v;


        UVs[idx + 2].x = u2;
        UVs[idx + 2].y = v;


        UVs[idx + 3].x = u2;
        UVs[idx + 3].y = v2;
        currentBatchSize++;

    }
    public void draw(float x, float y, float originX, float originY, float scaleX,
    float scaleY, float rotation, int srcX, int srcY, int srcWidth, int srcHeight, bool flipX, bool flipY)
    {
        float worldOriginX = x + originX;
        float worldOriginY = y + originY;
        float fx = -originX;
        float fy = -originY;
        float fx2 = srcWidth - originX;
        float fy2 = srcHeight - originY;

        // scale
        if (scaleX != 1 || scaleY != 1)
        {
            fx *= scaleX;
            fy *= scaleY;
            fx2 *= scaleX;
            fy2 *= scaleY;
        }


        float p1x = fx;
        float p1y = fy;
        float p2x = fx;
        float p2y = fy2;
        float p3x = fx2;
        float p3y = fy2;
        float p4x = fx2;
        float p4y = fy;

        float x1;
        float y1;
        float x2;
        float y2;
        float x3;
        float y3;
        float x4;
        float y4;

        // rotate
        if (rotation != 0)
        {
            float cos = Mathf.Cos(rotation * Mathf.PI / 180);
            float sin = Mathf.Sin(rotation * Mathf.PI / 180);

            x1 = cos * p1x - sin * p1y;
            y1 = sin * p1x + cos * p1y;

            x2 = cos * p2x - sin * p2y;
            y2 = sin * p2x + cos * p2y;

            x3 = cos * p3x - sin * p3y;
            y3 = sin * p3x + cos * p3y;

            x4 = x1 + (x3 - x2);
            y4 = y3 - (y2 - y1);
        }
        else
        {
            x1 = p1x;
            y1 = p1y;

            x2 = p2x;
            y2 = p2y;

            x3 = p3x;
            y3 = p3y;

            x4 = p4x;
            y4 = p4y;
        }

        x1 += worldOriginX;
        y1 += worldOriginY;
        x2 += worldOriginX;
        y2 += worldOriginY;
        x3 += worldOriginX;
        y3 += worldOriginY;
        x4 += worldOriginX;
        y4 += worldOriginY;

        float clipY = (tex_height - srcY - srcHeight);

        float u = srcX * invTexWidth;
        float v = (clipY + srcHeight) * invTexHeight;
        float u2 = (srcX + srcWidth) * invTexWidth;
        float v2 = clipY * invTexHeight;

        if (flipX)
        {
            float tmp = u;
            u = u2;
            u2 = tmp;
        }

        if (flipY)
        {
            float tmp = v;
            v = v2;
            v2 = tmp;
        }

        int idx = currentBatchSize * 4;

        vertices[idx + 0].x = x1;
        vertices[idx + 0].y = y1;
        vertices[idx + 0].z = 0f;

        vertices[idx + 1].x = x2;
        vertices[idx + 1].y = y2;
        vertices[idx + 1].z = 0f;

        vertices[idx + 2].x = x3;
        vertices[idx + 2].y = y3;
        vertices[idx + 2].z = 0f;

        vertices[idx + 3].x = x4;
        vertices[idx + 3].y = y4;
        vertices[idx + 3].z = 0f;



        UVs[idx + 0].x = u;
        UVs[idx + 0].y = v2;


        UVs[idx + 1].x = u;
        UVs[idx + 1].y = v;


        UVs[idx + 2].x = u2;
        UVs[idx + 2].y = v;


        UVs[idx + 3].x = u2;
        UVs[idx + 3].y = v2;
        currentBatchSize++;

    }
    public void draw(float x, float y, float originX, float originY,  float rotation, int srcX, int srcY, int srcWidth, int srcHeight, bool flipX, bool flipY, Color c)
    {
        float worldOriginX = x + originX;
        float worldOriginY = y + originY;
        float fx = -originX;
        float fy = -originY;
        float fx2 = srcWidth - originX;
        float fy2 = srcHeight - originY;




        float p1x = fx;
        float p1y = fy;
        float p2x = fx;
        float p2y = fy2;
        float p3x = fx2;
        float p3y = fy2;
        float p4x = fx2;
        float p4y = fy;

        float x1;
        float y1;
        float x2;
        float y2;
        float x3;
        float y3;
        float x4;
        float y4;

        // rotate
        if (rotation != 0)
        {
            float cos = Mathf.Cos(rotation * Mathf.PI / 180);
            float sin = Mathf.Sin(rotation * Mathf.PI / 180);

            x1 = cos * p1x - sin * p1y;
            y1 = sin * p1x + cos * p1y;

            x2 = cos * p2x - sin * p2y;
            y2 = sin * p2x + cos * p2y;

            x3 = cos * p3x - sin * p3y;
            y3 = sin * p3x + cos * p3y;

            x4 = x1 + (x3 - x2);
            y4 = y3 - (y2 - y1);
        }
        else
        {
            x1 = p1x;
            y1 = p1y;

            x2 = p2x;
            y2 = p2y;

            x3 = p3x;
            y3 = p3y;

            x4 = p4x;
            y4 = p4y;
        }

        x1 += worldOriginX;
        y1 += worldOriginY;
        x2 += worldOriginX;
        y2 += worldOriginY;
        x3 += worldOriginX;
        y3 += worldOriginY;
        x4 += worldOriginX;
        y4 += worldOriginY;

        float clipY = (tex_height - srcY - srcHeight);

        float u = srcX * invTexWidth;
        float v = (clipY + srcHeight) * invTexHeight;
        float u2 = (srcX + srcWidth) * invTexWidth;
        float v2 = clipY * invTexHeight;

        if (flipX)
        {
            float tmp = u;
            u = u2;
            u2 = tmp;
        }

        if (flipY)
        {
            float tmp = v;
            v = v2;
            v2 = tmp;
        }

        int idx = currentBatchSize * 4;

        vertices[idx + 0].x = x1;
        vertices[idx + 0].y = y1;
        vertices[idx + 0].z = 0f;

        vertices[idx + 1].x = x2;
        vertices[idx + 1].y = y2;
        vertices[idx + 1].z = 0f;

        vertices[idx + 2].x = x3;
        vertices[idx + 2].y = y3;
        vertices[idx + 2].z = 0f;

        vertices[idx + 3].x = x4;
        vertices[idx + 3].y = y4;
        vertices[idx + 3].z = 0f;

        colors[idx + 0] = c;
        colors[idx + 1] = c;
        colors[idx + 2] = c;
        colors[idx + 3] = c;


        UVs[idx + 0].x = u;
        UVs[idx + 0].y = v2;


        UVs[idx + 1].x = u;
        UVs[idx + 1].y = v;


        UVs[idx + 2].x = u2;
        UVs[idx + 2].y = v;


        UVs[idx + 3].x = u2;
        UVs[idx + 3].y = v2;
        currentBatchSize++;

    }
  
    public void draw(float x, float y, float originX, float originY,  float rotation, int srcX, int srcY, int srcWidth, int srcHeight, bool flipX, bool flipY)
    {
        float worldOriginX = x + originX;
        float worldOriginY = y + originY;
        float fx = -originX;
        float fy = -originY;
        float fx2 = srcWidth - originX;
        float fy2 = srcHeight - originY;




        float p1x = fx;
        float p1y = fy;
        float p2x = fx;
        float p2y = fy2;
        float p3x = fx2;
        float p3y = fy2;
        float p4x = fx2;
        float p4y = fy;

        float x1;
        float y1;
        float x2;
        float y2;
        float x3;
        float y3;
        float x4;
        float y4;

        // rotate
        if (rotation != 0)
        {
            float cos = Mathf.Cos(rotation * Mathf.PI / 180);
            float sin = Mathf.Sin(rotation * Mathf.PI / 180);

            x1 = cos * p1x - sin * p1y;
            y1 = sin * p1x + cos * p1y;

            x2 = cos * p2x - sin * p2y;
            y2 = sin * p2x + cos * p2y;

            x3 = cos * p3x - sin * p3y;
            y3 = sin * p3x + cos * p3y;

            x4 = x1 + (x3 - x2);
            y4 = y3 - (y2 - y1);
        }
        else
        {
            x1 = p1x;
            y1 = p1y;

            x2 = p2x;
            y2 = p2y;

            x3 = p3x;
            y3 = p3y;

            x4 = p4x;
            y4 = p4y;
        }

        x1 += worldOriginX;
        y1 += worldOriginY;
        x2 += worldOriginX;
        y2 += worldOriginY;
        x3 += worldOriginX;
        y3 += worldOriginY;
        x4 += worldOriginX;
        y4 += worldOriginY;

        float clipY = (tex_height - srcY - srcHeight);

        float u = srcX * invTexWidth;
        float v = (clipY + srcHeight) * invTexHeight;
        float u2 = (srcX + srcWidth) * invTexWidth;
        float v2 = clipY * invTexHeight;

        if (flipX)
        {
            float tmp = u;
            u = u2;
            u2 = tmp;
        }

        if (flipY)
        {
            float tmp = v;
            v = v2;
            v2 = tmp;
        }

        int idx = currentBatchSize * 4;

        vertices[idx + 0].x = x1;
        vertices[idx + 0].y = y1;
        vertices[idx + 0].z = 0f;

        vertices[idx + 1].x = x2;
        vertices[idx + 1].y = y2;
        vertices[idx + 1].z = 0f;

        vertices[idx + 2].x = x3;
        vertices[idx + 2].y = y3;
        vertices[idx + 2].z = 0f;

        vertices[idx + 3].x = x4;
        vertices[idx + 3].y = y4;
        vertices[idx + 3].z = 0f;




        UVs[idx + 0].x = u;
        UVs[idx + 0].y = v2;


        UVs[idx + 1].x = u;
        UVs[idx + 1].y = v;


        UVs[idx + 2].x = u2;
        UVs[idx + 2].y = v;


        UVs[idx + 3].x = u2;
        UVs[idx + 3].y = v2;
        currentBatchSize++;

    }
	
}

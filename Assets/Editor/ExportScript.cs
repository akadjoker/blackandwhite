/*
* 
* Copyright (c) 2014 Luis Santos AKA DJOKER
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;



public class ExportScript : EditorWindow
{
	[MenuItem("Misc/Scene2D/Export")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(ExportScript));
	}
	
	void OnGUI()
	{
		
		GUILayout.Label("Export 2D Level", EditorStyles.boldLabel);
		if(GUI.Button(new Rect(10, 50, 100, 32), "Export"))
		{
			Export();
		}
	}

	
	void Export()
	{


		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;

		XmlWriter textWriter = XmlWriter.Create ("export.xml",settings);

		textWriter.WriteStartDocument();
		textWriter.WriteComment("by luis santos aka djoker");
		textWriter.WriteStartElement("Objects");


		
		
		foreach(SpriteRenderer t in Object.FindObjectsOfType(typeof(SpriteRenderer)))
		{
			Debug.Log("Exporting " + t.gameObject.name);
		

			textWriter.WriteStartElement("Sprites");//root objects
			textWriter.WriteElementString("Name", t.gameObject.name);
     

			float X,Y,Z=0f;

            float size = Camera.main.camera.orthographicSize;

            X = t.transform.position.x;
            Y = t.transform.position.y-size;

            int x = (int)(X * 100);
            int y = (int)(Y * 100);


			textWriter.WriteStartElement("Position");
			textWriter.WriteAttributeString("X",X.ToString());
			textWriter.WriteAttributeString("Y",Y.ToString());
            textWriter.WriteAttributeString("RealX", x.ToString());
            textWriter.WriteAttributeString("RealY", y.ToString()); 
            textWriter.WriteEndElement();

 
			
			textWriter.WriteStartElement("Angle");
            Z = t.transform.rotation.eulerAngles.z;
			textWriter.WriteAttributeString("value",Z.ToString());
			textWriter.WriteEndElement();

            X = t.transform.localScale.x;
            Y = t.transform.localScale.y;


			 int width  = (int) (t.sprite.rect.width  );
             int height = (int) (t.sprite.rect.height );


			textWriter.WriteStartElement("Scale");
			textWriter.WriteAttributeString("X",X.ToString());
			textWriter.WriteAttributeString("Y",Y.ToString());
            textWriter.WriteAttributeString("Width",width.ToString());
			textWriter.WriteAttributeString("Height",height.ToString());
            textWriter.WriteEndElement();

            
            textWriter.WriteStartElement("Bound");
            textWriter.WriteAttributeString("PivotX", t.sprite.bounds.center.x.ToString());
            textWriter.WriteAttributeString("PivotY", t.sprite.bounds.center.y.ToString());

            float px = (t.sprite.bounds.center.x * t.transform.localScale.x);
            float py = (t.sprite.bounds.center.y * t.transform.localScale.y);
            
            textWriter.WriteAttributeString("RealPivotX", px.ToString());
            textWriter.WriteAttributeString("RealPivotY", py.ToString());
            
            textWriter.WriteAttributeString("SizeX",  t.sprite.bounds.size.x.ToString());
            textWriter.WriteAttributeString("SizeY", t.sprite.bounds.size.y.ToString());
            textWriter.WriteEndElement();



			textWriter.WriteEndElement();



		}
		

		// Ends the document.
		textWriter.WriteEndElement();
		textWriter.WriteEndDocument();
		
		// close writer
		textWriter.Flush ();
		textWriter.Close();
	}
	
	
}
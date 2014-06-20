using UnityEngine;
using System.Collections;
using statemanager.Interfaces;

namespace statemanger.States
{

	public class SetupState : IStateBase
	{
		private StateManager manager;
		
		public SetupState (StateManager managerRef)
		{
			manager = managerRef;
            if (Application.loadedLevelName != "Scene1")
                Application.LoadLevel("Scene1");

		}
		
		public void StateUpdate ()
		{
			if (Input.GetKeyUp (KeyCode.Space))
			{
	              manager.Restart();
			}
		}
		
		public void ShowIt ()
		{
			//Debug.Log ("In SetupState");
		}
	}
}


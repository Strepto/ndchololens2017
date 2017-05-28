using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

public enum ManipulationModes
{
	None,
	Move,
	Scale,
	Rotate,
	Frozen
}
public class BoxManipulationManager : Singleton<BoxManipulationManager> {

	[SerializeField]
	private ManipulationModes _manipulationMode = ManipulationModes.Scale;

	public void IncrementManipulationMode(){
		if((int)_manipulationMode < System.Enum.GetValues(typeof(ManipulationModes)).Length){
			_manipulationMode = _manipulationMode + 1;
		}else{
			_manipulationMode = 0;
		}
	}

	public void SetManipulationModeMove(){
		SetManipulationMode(ManipulationModes.Move);
	}
	public void SetManipulationModeRotate(){
		SetManipulationMode(ManipulationModes.Rotate);
	}
	public void SetManipulationModeScale(){
		SetManipulationMode(ManipulationModes.Scale);
	}
	public void SetManipulationModeNone(){
		SetManipulationMode(ManipulationModes.None);
	}

	public void SetManipulationMode(ManipulationModes manipulationMode)
	{
		this._manipulationMode = manipulationMode;
		Debug.Log("Manipulation mode now: " + manipulationMode.ToString());
	}

	public ManipulationModes GetManipulationMode(){
		return _manipulationMode;
	}
}

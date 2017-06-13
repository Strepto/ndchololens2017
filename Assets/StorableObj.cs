using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorableObj : MonoBehaviour  {


	public SharingObjectType currentObjectType;
	

	public Storable GetStorableRepresentation(){
		int boxType = -1;
		if(gameObject.GetComponentInChildren<IBox>() != null){
			boxType = gameObject.GetComponentInChildren<IBox>().GetBoxType();
		}
		
		return new Storable {
			localPosition = transform.localPosition,
			localRotation = transform.localRotation,
			localScale = transform.localScale,
			Type = currentObjectType,
			BoxType = boxType
		};
	}

}

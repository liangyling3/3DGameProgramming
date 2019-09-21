using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

public class ClickGUI : MonoBehaviour {
	UserAction action;
	MyCharacterController characterController;

	void Start() {
		action = Director.getInstance().currentSceneController as UserAction;
	}
	
	// 设定人物控制器
	public void setController(MyCharacterController _characterController) {
		characterController = _characterController;
	}

	// 点击事件控制
	void OnMouseDown() {
		if (gameObject.name == "boat") {
			action.moveBoat();
		} 
		else {
			action.characterIsClicked(characterController);
		}
	}
}

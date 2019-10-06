using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

public class FirstController : MonoBehaviour, SceneController, UserAction {

	Vector3 river_pos = new Vector3(0,0.5F,0);
	UserGUI userGUI;

	public CoastController fromCoast;
	public CoastController toCoast;
	public BoatController boat;
	public Judge judge = new Judge();
	private MyCharacterController[] characters;

	void Awake() {
		Director director = Director.getInstance();
		director.currentSceneController = this;
		userGUI = gameObject.AddComponent <UserGUI>() as UserGUI;
		characters = new MyCharacterController[6];
		loadResources();
	}

	public void loadResources() {
		GameObject river = Instantiate (Resources.Load("Perfabs/River", typeof(GameObject)), river_pos, Quaternion.identity, null) as GameObject;
		river.name = "river";

		fromCoast = new CoastController(1);	// from
		toCoast = new CoastController(-1);	// to
		boat = new BoatController();
		loadCharacter();
	}

	private void loadCharacter() {
		for (int i = 0; i < 3; i++) {
			MyCharacterController tmp = new MyCharacterController("priest");
			tmp.setName("priest" + i);
			tmp.setPosition(fromCoast.getEmptyPosition());
			tmp.myAction.getOnCoast(fromCoast, tmp);
			fromCoast.setCharacterCtrl(tmp);
			characters[i] = tmp;
		}

		for (int i = 0; i < 3; i++) {
			MyCharacterController tmp = new MyCharacterController("devil");
			tmp.setName("devil" + i);
			tmp.setPosition(fromCoast.getEmptyPosition());
			tmp.myAction.getOnCoast(fromCoast, tmp);
			fromCoast.setCharacterCtrl(tmp);
			characters[i+3] = tmp;
		}
	}

	public void restart() {
		boat.reset();
		fromCoast.reset();
		toCoast.reset();
		for (int i = 0; i < characters.Length; i++) {
			characters[i].reset();
		}
		judge.reset();
	}

	public void moveBoat() {
		if (boat.isEmpty())
			return;
		boat.boatAction.boatMove(boat);
		judge.update(this);
		userGUI.status = judge.getStatus();
	}

	// 点击人物
	public void characterIsClicked(MyCharacterController characterCtrl) {
		if (characterCtrl.isOnBoat()) {
			CoastController whichCoast;
			if (boat.to_from == -1) 
				whichCoast = toCoast;
			else 
				whichCoast = fromCoast;

			boat.boatAction.getOffBoat(boat, characterCtrl.getName());
			characterCtrl.moveToPosition (whichCoast.getEmptyPosition());
			characterCtrl.myAction.getOnCoast(whichCoast, characterCtrl);
			whichCoast.setCharacterCtrl(characterCtrl);
		} 
		else {									
			CoastController whichCoast = characterCtrl.getCoastController ();
			if (boat.getEmptyIndex() == -1)	// 人满了
				return;
			if (whichCoast.get_to_from() != boat.to_from)
				return;

			whichCoast.coastAction.getOffCoast(whichCoast, characterCtrl.getName());
			characterCtrl.moveToPosition(boat.getEmptyPosition());
			characterCtrl.myAction.getOnBoat(boat, characterCtrl);
			// boat.GetOnBoat(characterCtrl);
		}
		judge.update(this);
		userGUI.status = judge.getStatus();
	}
}

public class Judge {
	
	

	// 0：未结束 1：lose 2：win
	int status = 0;

	public int getStatus() {
		return status;
	}

	public void update(FirstController controller) {
		int from_priest = 0, from_devil = 0;
		int to_priest = 0, to_devil = 0;

		int[] fromCount = controller.fromCoast.getCharacterNum();
		from_priest += fromCount[0];
		from_devil += fromCount[1];

		int[] toCount = controller.toCoast.getCharacterNum();
		to_priest += toCount[0];
		to_devil += toCount[1];

		// win
		if (to_priest + to_devil == 6) {
			status =  2;
			return;
		} 
		
		int[] boatCount = controller.boat.getCharacterNum();
		if (controller.boat.to_from == -1) {	// 船在目的地
			to_priest += boatCount[0];
			to_devil += boatCount[1];
		} 
		else {	// 船在出发地
			from_priest += boatCount[0];
			from_devil += boatCount[1];
		}

		// lose
		if ((from_priest < from_devil && from_priest > 0) || (to_priest < to_devil && to_priest > 0)) {		
			status = 1;
			return;
		}
	}

	public void reset() {
		status = 0;
	}
}

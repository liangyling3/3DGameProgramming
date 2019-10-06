using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

namespace Com.Mygame {
	
	public class Director : System.Object {
		private static Director _instance;
		public SceneController currentSceneController{ get; set; }	

		public static Director getInstance() {
			if (_instance == null) {
				_instance = new Director();
			}
			return _instance;
		}
	}

	// 场景控制器
	public interface SceneController {
		void loadResources ();
	}

	// 玩家动作控制
	public interface UserAction {
		void moveBoat();
		void characterIsClicked(MyCharacterController characterCtrl);
		void restart();
	}

	// 移动脚本
	public class Moving: MonoBehaviour {

		readonly float speed = 15;
		int status;	// 0:不移动 1:上船 2:下船
		Vector3 dst;
		Vector3 mid;

		void Update() {
			if (status == 1) {	
				transform.position = Vector3.MoveTowards(transform.position, mid, speed*Time.deltaTime);
				if (transform.position == mid) {
					status = 2;
				}
			} 
			else if (status == 2) {	
				transform.position = Vector3.MoveTowards(transform.position, dst, speed*Time.deltaTime);
				if (transform.position == dst) {
					status = 0;
				}
			}
		}

		public void reset() {
			status = 0;
		}

		public void setDestination(Vector3 _dst) {
			dst = _dst;
			mid = _dst;
		
			if (_dst.y == transform.position.y) {	// 如果在船上，下船
				status = 2;
			}
			else if (_dst.y < transform.position.y) {	
				mid.y = transform.position.y;
			} 
			else {								
				mid.x = transform.position.x;
			}
			status = 1;
		}

	}


	// 人物控制器
	public class MyCharacterController {
		readonly GameObject character;
		readonly Moving movingScript;
		readonly ClickGUI clickGUI;
		readonly int characterType;	// 0:priest 1:devil

		public bool onBoat;
		public CoastController coastController;
		public Action myAction = new Action();


		public MyCharacterController(string which_character) {
			// 载入预制
			if (which_character == "priest") {
				character = Object.Instantiate(Resources.Load("Perfabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
				characterType = 0;
			} 
			else {
				character = Object.Instantiate(Resources.Load("Perfabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
				characterType = 1;
			}
			// 为人物挂载脚本
			movingScript = character.AddComponent(typeof(Moving)) as Moving;
			// 添加点击事件
			clickGUI = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
			clickGUI.setController(this);
		}


		public string getName() {
			return character.name;
		}
		
		public void setName(string name) {
			character.name = name;
		}

		public int getType() {	// 0:priest  1:devil
			return characterType;
		}

		public void setPosition(Vector3 pos) {
			character.transform.position = pos;
		}

		public void moveToPosition(Vector3 destination) {
			movingScript.setDestination(destination);
		}


		public CoastController getCoastController() {
			return coastController;
		}

		public void setCoastController(CoastController coastCtrl) {
			coastController = coastCtrl;
		}

		public bool isOnBoat() {
			return onBoat;
		}

		public void setOnBoat(bool status) {
			onBoat = status;
		}

		public void setCharacterTransformParent(UnityEngine.Transform trans) {
			character.transform.parent = trans;
		}

		// 重新开始
		public void reset() {
			movingScript.reset();
			coastController = (Director.getInstance().currentSceneController as FirstController).fromCoast;
			myAction.getOnCoast(coastController, this);
			setPosition(coastController.getEmptyPosition());
			coastController.setCharacterCtrl(this);
		}
	}

	// 船控制器
	public class BoatController {
		readonly GameObject boat;
		readonly Moving movingScript;
		readonly Vector3[] from_positions;
		readonly Vector3[] to_positions;
		public Vector3 from_pos = new Vector3(5, 1, 0);
		public Vector3 to_pos = new Vector3(-5, 1, 0);
		public int to_from; // to：-1 from：1

		// 一次只允许乘坐两人
		public MyCharacterController[] passenger = new MyCharacterController[2];
		public Action boatAction = new Action();

		public BoatController() {
			to_from = 1;

			from_positions = new Vector3[] {new Vector3(4.5f, 1.5f, 0), new Vector3(5.5f, 1.5f, 0)};
			to_positions = new Vector3[] {new Vector3(-5.5f, 1.5f, 0), new Vector3(-4.5f, 1.5f, 0)};

			// 载入预制
			boat = Object.Instantiate (Resources.Load("Perfabs/Boat", typeof(GameObject)), from_pos, Quaternion.identity, null) as GameObject;
			boat.name = "boat";

			// 挂载脚本
			movingScript = boat.AddComponent(typeof(Moving)) as Moving;
			boat.AddComponent(typeof(ClickGUI));
		}

		public GameObject getBoat() {
			return boat;
		}

		public void set_to_from(int status) { // to：-1 from：1
			to_from = status;
		}

		public void setMovingScriptDest(Vector3 vec) {
			movingScript.setDestination(vec);
		}

		// public void Move() {
		// 	if (to_from == -1) {
		// 		movingScript.setDestination(from_pos);
		// 		to_from = 1;
		// 	} 
		// 	else {
		// 		movingScript.setDestination(to_pos);
		// 		to_from = -1;
		// 	}
		// }

		public void reset() {
			movingScript.reset();
			if (to_from == -1) {
				boatAction.boatMove(this);
			}
			passenger = new MyCharacterController[2];
		}

		public bool isEmpty() {
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger[i] != null) 
					return false;
			}
			return true;
		}

		public int getEmptyIndex() {
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger[i] == null) 
					return i;
			}
			// 没有乘客的情况不能开船
			return -1;
		}

		public Vector3 getEmptyPosition() {
			Vector3 pos;
			int emptyIndex = getEmptyIndex();
			if (to_from == -1) {
				pos = to_positions[emptyIndex];
			} 
			else {
				pos = from_positions[emptyIndex];
			}
			return pos;
		}

		public void setCharacterOnBoat(MyCharacterController characterCtrl) {
			passenger[getEmptyIndex()] = characterCtrl;
		}

		// public MyCharacterController GetOffBoat(string passenger_name) {
		// 	for (int i = 0; i < passenger.Length; i++) {
		// 		if (passenger[i] != null && passenger[i].getName() == passenger_name) {
		// 			MyCharacterController characterCtrl = passenger[i];
		// 			passenger[i] = null;
		// 			return characterCtrl;
		// 		}
		// 	}
		// 	return null;
		// }

		// 获取人数，用于判断是否游戏结束
		public int[] getCharacterNum() {
			int [] count = {0, 0};
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger[i] == null)
					continue;
				if (passenger[i].getType() == 0) 
					count[0]++;
				else 
					count[1]++;
			}
			return count;
		}
	}

	// 岸控制器
	public class CoastController {
		readonly GameObject coast;
		readonly Vector3 from_pos = new Vector3(9,1,0);
		readonly Vector3 to_pos = new Vector3(-9,1,0);
		readonly Vector3[] positions;
		readonly int to_from;	// to:-1, from:1

		public MyCharacterController[] characterOnCoast;
		public Action coastAction = new Action();


		public CoastController(int _to_from) {
			positions = new Vector3[] {new Vector3(6.5F,2.25F,0), new Vector3(7.5F,2.25F,0), new Vector3(8.5F,2.25F,0), 
				new Vector3(9.5F,2.25F,0), new Vector3(10.5F,2.25F,0), new Vector3(11.5F,2.25F,0)};

			characterOnCoast = new MyCharacterController[6];

			to_from = _to_from;

			if (_to_from == 1) {
				coast = Object.Instantiate(Resources.Load("Perfabs/Lawn", typeof(GameObject)), from_pos, Quaternion.identity, null) as GameObject;
				coast.name = "from";
			} 
			else {
				coast = Object.Instantiate(Resources.Load("Perfabs/Lawn", typeof(GameObject)), to_pos, Quaternion.identity, null) as GameObject;
				coast.name = "to";
			}
		}

		public void reset() {
			characterOnCoast = new MyCharacterController[6];
		}

		public int get_to_from() {
			return to_from;
		}

		public int getEmptyIndex() {
			for (int i = 0; i < characterOnCoast.Length; i++) {
				if (characterOnCoast[i] == null) 
					return i;
			}
			return -1;
		}

		public Vector3 getEmptyPosition() {
			Vector3 pos = positions[getEmptyIndex()];
			pos.x *= to_from;
			return pos;
		}

		public void setCharacterCtrl(MyCharacterController characterCtrl) {
			int index = getEmptyIndex();
			characterOnCoast[index] = characterCtrl;
		}

		// public MyCharacterController getOffCoast(string passenger_name) {
		// 	for (int i = 0; i < characterOnCoast.Length; i++) {
		// 		if (characterOnCoast[i] != null && characterOnCoast[i].getName () == passenger_name) {
		// 			MyCharacterController _characterController = characterOnCoast[i];
		// 			characterOnCoast[i] = null;
		// 			return _characterController;
		// 		}
		// 	}
		// 	return null;
		// }

		public int[] getCharacterNum() {
			int [] count = {0, 0};
			int len = characterOnCoast.Length;
			for (int i = 0; i < len; i++) {
				if (characterOnCoast[i] == null)
					continue;

				if (characterOnCoast[i].getType() == 0) 	// 0->priest, 1->devil
					count[0]++;
				else 
					count[1]++;
			}
			return count;
		}
	}
}

public class Action {
	// 上船
	public void getOnBoat(BoatController boatCtrl, MyCharacterController characterCtrl) {
		// 人物控制器
		characterCtrl.setCoastController(null);
		characterCtrl.setCharacterTransformParent(boatCtrl.getBoat().transform);
		characterCtrl.setOnBoat(true);
		// 船控制器
		boatCtrl.setCharacterOnBoat(characterCtrl);

	}

	// 上岸
	public void getOnCoast(CoastController coastCtrl, MyCharacterController characterCtrl) {
		// 人物控制器
		characterCtrl.setCoastController(coastCtrl);
		characterCtrl.setCharacterTransformParent(null);;
		characterCtrl.setOnBoat(false);
	}

	// 下船
	public MyCharacterController getOffBoat(BoatController boatCtrl, string passenger_name) {
		for (int i = 0; i < boatCtrl.passenger.Length; i++) {
				if (boatCtrl.passenger[i] != null && boatCtrl.passenger[i].getName() == passenger_name) {
					MyCharacterController characterCtrl = boatCtrl.passenger[i];
					boatCtrl.passenger[i] = null;
					return characterCtrl;
				}
			}
			return null;
	}

	// 船的移动
	public void boatMove(BoatController boatCtrl) {
		if (boatCtrl.to_from == -1) {
				boatCtrl.setMovingScriptDest(boatCtrl.from_pos);
				boatCtrl.set_to_from(1);
			} 
			else {
				boatCtrl.setMovingScriptDest(boatCtrl.to_pos);
				boatCtrl.set_to_from(-1);
			}
	}

	// 人物从岸上到船上
	public MyCharacterController getOffCoast(CoastController coastCtrl, string passenger_name) {
		for (int i = 0; i < coastCtrl.characterOnCoast.Length; i++) {
			if (coastCtrl.characterOnCoast[i] != null && coastCtrl.characterOnCoast[i].getName () == passenger_name) {
				MyCharacterController characterCtrl = coastCtrl.characterOnCoast[i];
				coastCtrl.characterOnCoast[i] = null;
				return characterCtrl;
			}
		}
		return null;
	}

	
}
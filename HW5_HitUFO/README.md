[游戏效果视频预览](https://v.youku.com/v_show/id_XNDM5MTk5OTM1Mg==.html?spm=a2h3j.8428770.3416059.1)
## 编写一个简单的鼠标打飞碟（Hit UFO）游戏

### 游戏内容要求：
- 游戏有 n 个 round，每个 round 都包括10 次 trial；
- 每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同。它们由该 round 的 ruler 控制；
- 每个 trial 的飞碟有随机性，总体难度随 round 上升；
- 鼠标点中得分，得分规则按色彩、大小、速度不同计算，规则可自由设定。
### 游戏的要求：
- 使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的！具体实现见参考资源 Singleton 模板类
- 尽可能使用前面 MVC 结构实现人机交互与游戏模型分离

### 游戏实现
这次游戏的实现依然采用MVC架构，在此基础上增加一个带缓存的UFO工厂（UFOFactory），用于管理UFO的生产与回收。有了UFO工厂，可以有效减少对象的创建和销毁所带来的开销。
#### UFO工厂
定义了如何产生和回收一个游戏对象UFO。利用List集合，将buf作为缓存。

用随机数使得每个UFO产生的颜色、位置、角度等都有所不同。
```c
public class UFOFactory : MonoBehaviour {
    public List<GameObject> used = new List<GameObject>();
    public List<GameObject> buf = new List<GameObject>();


    // 产生UFO
    public void produceUFO() {
        GameObject ufo;
        if (buf.Count == 0) {
            ufo = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UFO"), Vector3.zero, Quaternion.identity);
        }
        else {
            ufo = buf[0];
            buf.RemoveAt(0);
        }
        float x = Random.Range(-10.0f, 10.0f);
        // 随机产生位置
        ufo.transform.position = new Vector3(x, 0, 0);
        // 随机角度
        ufo.transform.Rotate(new Vector3(x < 0? -x*9 : x*9, 0, 0));
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        Color color = new Color(r, g, b);
        // 随机产生颜色
        ufo.transform.GetComponent<Renderer>().material.color = color;
        used.Add(ufo);
    }

    // 回收UFO
    public void recycleUFO(GameObject obj) {
        obj.transform.position = Vector3.zero;
        buf.Add(obj);
    }
}
```

#### 模板类Singleton
保证工厂是场景单实例的。详细参照[模板类Singleton](https://blog.csdn.net/u18004660201/article/details/80317902)。
```c
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	protected static T instance;

	public static T Instance {  
		get {  
			if (instance == null) { 
				instance = (T)FindObjectOfType (typeof(T));  
				if (instance == null) {
					Debug.LogError ("An instance of " + typeof(T) +
					" is needed in the scene, but there is none.");  
				}
			}
			return instance;  
		}
	}
}
```
#### 场景控制器FirstSceneController
最高层的控制器，主要包含UFO工厂、动作控制器。

在场景控制器中控制UFO的产生（每次随机产生1~3个）；同时记录分数（score）、关数（round）等。
```c

public interface MyUserAction {
    void Restart();
    void Pause();
}

public interface MySceneController {
    void GenGameObjects();
}

public class FirstSceneController : MonoBehaviour, MyUserAction, MySceneController {
    public ActionController actionCtrl;
    public GameObject disk;
    protected UFOFactory factory;
    public int flag = 0;
    private float interval = 3;
    public int score = 0;
    public static int times = 0;

    private void Awake() {
        CurrentDirector director = CurrentDirector.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;
        this.gameObject.AddComponent<UFOFactory>();
        this.gameObject.AddComponent<ActionController>();
        this.gameObject.AddComponent<UserGUI>();
        factory = Singleton<UFOFactory>.Instance;
    }

    private void Start() {

    }

    public void GenGameObjects () {

    }

    public void Restart() {
        SceneManager.LoadScene("1");
    }

    public void Pause () {
        actionCtrl.Pause();
    }
    
    public void Update() {
        if (times < 30 && flag == 0) {
            if (interval <= 0) {
                interval = Random.Range(3, 5);
                times++;
                int num = Random.Range(1,4);
                for (int i = 0; i < num; i++) {
                    factory.produceUFO();
                }
            }
            interval -= Time.deltaTime;
        }
    }
}
```
#### 游戏界面UserGUI
```c
public class UserGUI : MonoBehaviour {
    private FirstSceneController action;
    private GUIStyle fontstyle1 = new GUIStyle();
    
    void Start () {
        action = CurrentDirector.getInstance().currentSceneController as FirstSceneController;
        fontstyle1.fontSize = 30;
    }

    private void OnGUI() {
        if (GUI.Button(new Rect(Screen.width/2+100, 15, 100, 60), "重新开始")) {
            action.Restart();
        }

        if (GUI.Button(new Rect(Screen.width/2+230, 15, 100, 60), "暂停")) {
            action.Pause();
        }

        if (action.flag == 0) {
            fontstyle1.normal.textColor = Color.black;
            GUI.Label(new Rect(Screen.width/2-280, 30, 300, 100), "Score: " +
                action.score, fontstyle1);
            GUI.Label(new Rect(Screen.width/2-100, 30, 300, 100),"Round: " + (Mathf.CeilToInt(FirstSceneController.times/10)+1), fontstyle1);
        }
        else if (action.flag == 1) {
            fontstyle1.normal.textColor = Color.red;
            GUI.Label(new Rect(Screen.width/2, Screen.height/2, 300, 100), "最后得分 : " + action.score, fontstyle1);
        }
        else {
            fontstyle1.normal.textColor = Color.black;
            GUI.Label(new Rect(Screen.width/2-280, 30, 300, 100), "Score: " +
                action.score, fontstyle1);
            GUI.Label(new Rect(Screen.width/2-100, 30, 300, 100),"Round: " + (Mathf.CeilToInt(FirstSceneController.times/10)+1), fontstyle1);

            fontstyle1.normal.textColor = Color.red;
            GUI.Label(new Rect(Screen.width / 2, Screen.height/2, 300, 100), "暂停", fontstyle1);
        }
    }
}
```

#### 动作类Action
定义了UFO移动的动作以及用户点击UFO的动作。
```c

public class Action : ScriptableObject {
    public bool enable = true;
    public bool destory = false;

    public GameObject gameObject { get; set; }
    public Transform transform { get; set; }
    public ActionCallback callback { get; set; }

    public virtual void Start () {
        throw new System.NotImplementedException();
	}

    public virtual void Update() {
        throw new System.NotImplementedException();
    }
}

public class MoveAction : Action {
    public Vector3 target;
    public float speed;

    public static MoveAction GetAction(Vector3 target, float speed) {
        MoveAction action = CreateInstance<MoveAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Start() {
        
    }

    public override void Update() {
        if(enable) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed);
            if (this.transform.position == target) {
                this.enable = false;
                this.callback.ActionEvent(this);
            }
        }
    }
}

public class UserClickAction : Action {
    public static UserClickAction GetAction() {
        UserClickAction action = CreateInstance<UserClickAction>();
        return action;
    }

    public override void Start() {
        
    }

    public override void Update() {
        if(enable) {
            FirstSceneController sc = CurrentDirector.getInstance().currentSceneController as FirstSceneController;
            sc.score = sc.score + Mathf.CeilToInt(FirstSceneController.times/10) + Mathf.FloorToInt(120 / (transform.rotation.x + 30));
            destory = true;
        }
    }
}
```

#### 动作控制器ActionController
```c
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionEventType:int {Started, Completed}

public interface ActionCallback {
    void ActionEvent(Action source, ActionEventType events = ActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null);
    void CheckEvent(Action source, ActionEventType events = ActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null);
}

public class ActionManager : MonoBehaviour {
    public Dictionary<int, Action> actions = new Dictionary<int, Action>();
    public List<Action> waitingAdd = new List<Action>();
    public List<int> waitingDelete = new List<int>();

    protected void Update() {
        foreach (Action ac in waitingAdd)
            actions[ac.GetInstanceID()] = ac;
        waitingAdd.Clear();
        foreach (KeyValuePair <int, Action> kv in actions) {
            if (kv.Value.destory)
                waitingDelete.Add(kv.Value.GetInstanceID());
            else
                kv.Value.Update();
        }
        foreach (int key in waitingDelete) {
            DestroyObject(actions[key]);
            actions.Remove(key);
        }
        waitingDelete.Clear();
    }
    public void RunAction(GameObject gameObject, Action action, ActionCallback manager) {
        Debug.Log(gameObject.GetInstanceID());
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }
}

public class ActionController : ActionManager, ActionCallback {
    public FirstSceneController sceneController;
    public List<MoveAction> seq = new List<MoveAction>();
    public UserClickAction userClickAction;
    public UFOFactory disks;
    
    public void Start() {
        sceneController = (FirstSceneController)CurrentDirector.getInstance().currentSceneController;
        sceneController.actionCtrl = this;
        disks = Singleton<UFOFactory>.Instance;
    }

    public void Update() {
        if (disks.used.Count > 0) {
            GameObject disk = disks.used[0];
            float x = Random.Range(-10, 10);
            MoveAction moveToAction = MoveAction.GetAction(new Vector3(x, 12, 0), 3 * (Mathf.CeilToInt(FirstSceneController.times / 10) + 1) * Time.deltaTime);
            seq.Add(moveToAction);
            this.RunAction(disk, moveToAction, this);
            disks.used.RemoveAt(0);
        }
        
        if (Input.GetMouseButtonDown(0) && sceneController.flag == 0) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitGameObject;
            if (Physics.Raycast(ray, out hitGameObject)) {
                GameObject gameObject = hitGameObject.collider.gameObject;
                if (gameObject.tag == "disk") {
                    foreach(var k in seq) {
                        if (k.gameObject == gameObject)
                            k.transform.position = k.target;
                    }
                    userClickAction = UserClickAction.GetAction();
                    this.RunAction(gameObject, userClickAction, this);
                }
            }
        }
        base.Update();
    }

    public void ActionEvent(Action source, ActionEventType events = ActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null) {
        disks.recycleUFO(source.gameObject);
        seq.Remove(source as MoveAction);
        source.destory = true;
        if (FirstSceneController.times >= 30)
            sceneController.flag = 1;
    }

    public void CheckEvent(Action source, ActionEventType events = ActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null) {
    }

    public void Pause() {
        if(sceneController.flag == 0) {
            foreach (var k in seq) {
                k.enable = false;
            }
            sceneController.flag = 2;
        }
        else if(sceneController.flag == 2) {
            foreach (var k in seq) {
                k.enable = true;
            }
            sceneController.flag = 0;
        }
    }
}
```

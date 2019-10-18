## 作业要求：改进飞碟（Hit UFO）游戏

#### 游戏内容要求：
- 按 adapter模式 设计图修改飞碟游戏
- 使它同时支持物理运动与运动学（变换）运动

## 最终效果演示视频
[https://v.youku.com/v_show/id_XNDQwMzY4MDA4NA==.html?spm=a2h3j.8428770.3416059.1](https://v.youku.com/v_show/id_XNDQwMzY4MDA4NA==.html?spm=a2h3j.8428770.3416059.1)

## Adapter模式
适配器模式所做的就是对接口的转换，如果想给手机充电，而现实是只有一个三孔插头，此时如果有一个插座肯定就解决了。因此适配器直接作用就是对接口的转换，适配成你现实需要的接口，而适配器是在原有类的基础上扩展出来的（有现成的三孔插头了，才能做适配），即也是为了对现有类方法的复用。

- 类适配器：通过继承创建的适配器。应用于系统想要使用现有类，而这些类的接口却不符合要求的情况。
- 对象适配器：通过组合创建的适配器。应用于两个类所做的事情相同或者相似，但是接口不同的情况。

在本次作业中，CCActionManager、PhysicsActionManager就是适配器。可以通过不同的适配器选择动作管理器或者物理引擎。

## 代码实现
在上一次的代码基础上进行修改。

#### IActionManager
增加新的接口 **IActionManager**
```c
public interface IActionManager {
    void playUFO();
    void Pause();
}
```
#### 整合接口
将所有接口整合到Interfaces文件中。
```c
// 整合接口
public interface IActionManager {
    void PlayUFO();
    void Pause();
}

public enum SSActionEventType:int { Started, Completed}

public interface ISSActionCallback {
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null);
    void CheckEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null);
}


public interface ISceneController {
    void GenGameObjects();
}

public interface IUserAction {
    void Restart();
    void Pause();
}
```
#### CCAcitonManager
1. 增加 **IActionManager** 为 CCActionManager 继承的类
```c
public class CCActionManager : ActionManager, ActionCallback, IActionManager 
```
2. 将 **CCActionManager** 中的Update函数更名为playUFO
```c
public void PlayUFO() {
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
        base.PlayUFO();
    }
```

#### FirstSceneController
1. 修改成员变量 **CCActionManager** 为新的接口 **IActionManager** 
```c
public IActionManager actionManager;    // 修改为新的接口
```
2. 将Awake函数中的添加组件改为物理引擎（适配器 **PhysicsActionManager**）
```c
 private void Awake() {
        CurrentDirector director = CurrentDirector.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;
        this.gameObject.AddComponent<UFOFactory>();
        this.gameObject.AddComponent<PhysicsActionManager>();   // 更改为物理引擎
        this.gameObject.AddComponent<UserGUI>();
        factory = Singleton<UFOFactory>.Instance;
    }
```
3. 在 Update 函数最后添加
```c
actionManager.PlayUFO();
```
#### PhysicsEmitAction
继承SSAcion，表示物理动作。
```c
public class PhysicsEmitAction : SSAction {
    public Vector3 speed;

    public static PhysicsEmitAction GetSSAction() {
        PhysicsEmitAction action = CreateInstance<PhysicsEmitAction>();
        return action;
    }

    public override void Start() {
        
    }

    public override void Update() {
        if (transform.position.y < -10 || transform.position.x <= -20 || transform.position.x >= 20) {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = Vector3.down;
            callback.SSActionEvent(this);
        }
    }
}
```

#### PhysicsActionManager
适配器。
```c
public class PhysicsActionManager : SSActionManager, ISSActionCallback, IActionManager {
    public FirstSceneController sceneCtrl;
    public List<PhysicsEmitAction> seq = new List<PhysicsEmitAction>();
    public UserClickAction userClickAction;
    public UFOFactory ufo;

    protected void Start() {
        sceneCtrl = (FirstSceneController)SSDirector.getInstance().currentSceneController;
        sceneCtrl.actionManager = this;
        ufo = Singleton<UFOFactory>.Instance;
    }
    
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null) {
        ufo.RecycleUFO(source.gameObject);
        seq.Remove(source as PhysicsEmitAction);
        source.destory = true;
        if (FirstSceneController.times >= 30)
            sceneCtrl.flag = 1;
    }

    public void CheckEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null) {

    }

    public void Pause() {
        if (sceneCtrl.flag == 0) {
            foreach (var k in seq) {
                k.speed = k.transform.GetComponent<Rigidbody>().velocity;
                k.transform.GetComponent<Rigidbody>().isKinematic = true;
            }
            sceneCtrl.flag = 2;
        }
        else if (sceneCtrl.flag == 2) {
            foreach (var k in seq) {
                k.transform.GetComponent<Rigidbody>().isKinematic = false;
                k.transform.GetComponent<Rigidbody>().velocity = k.speed;
            }
            sceneCtrl.flag = 0;
        }
    }

    public void PlayUFO() {
        if (ufo.used.Count > 0) {
            GameObject disk = ufo.used[0];
            float x = Random.Range(-5, 5);
            disk.GetComponent<Rigidbody>().isKinematic = false;
            disk.GetComponent<Rigidbody>().velocity = new Vector3(x, 8 * (Mathf.CeilToInt(FirstSceneController.times / 10) + 1), 6);
            disk.GetComponent<Rigidbody>().AddForce(new Vector3(0,8.8f, 0),ForceMode.Force);
            PhysicsEmitAction physicsEmitAction = PhysicsEmitAction.GetSSAction();
            seq.Add(physicsEmitAction);
            this.RunAction(disk, physicsEmitAction, this);
            ufo.used.RemoveAt(0);
        }

        if (Input.GetMouseButtonDown(0) && sceneCtrl.flag == 0) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitGameObject;
            if (Physics.Raycast(ray, out hitGameObject)) {
                GameObject gameObject = hitGameObject.collider.gameObject;
                Debug.Log(gameObject.tag);
                if (gameObject.tag == "disk") {
                    gameObject.transform.position=new Vector3(100,100,100);
                    userClickAction = UserClickAction.GetSSAction();
                    this.RunAction(gameObject, userClickAction, this);
                }
            }
        }
        base.Update();
    }
}
```

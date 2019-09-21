## 1、简答题
#### (1) 解释 游戏对象（GameObjects） 和 资源（Assets）的区别与联系。
游戏对象是游戏运行时出现在场景中的物体，它是一种容器，可以挂载各种各样的组件。

而资源是指游戏设计的过程中我们可以使用的一切物体和属性。包括：网格、材质、代码片段等。

资源是可以被游戏对象使用的，一种资源可以被多个游戏对象使用。

#### (2) 下载几个游戏案例，分别总结资源、对象组织的结构（指资源的目录组织结构与游戏对象树的层次结构）
如图，以Github上的一个游戏案例【3D月光跑酷】为例：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190912103846921.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
项目中最主要的是资源Assets文件夹，打开如下：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190912103935126.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
可以看到资源目录下包含了音频（Audio）、材质（Materials）、模型（Model）、代码文件（Script）、场景（Scenes）、着色器（Shader）等等资源。其中Model文件夹存储了主要的游戏对象（GameObject），打开如下：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190912104206369.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
其中包含了玩家、建筑、地板、硬币等游戏对象。

#### 3. 编写一个代码，使用 debug 语句来验证 MonoBehaviour 基本行为或事件触发的条件
- 基本行为包括 Awake() Start() Update() FixedUpdate() LateUpdate()
- 常用事件包括 OnGUI() OnDisable() OnEnable()

代码如下：
```c
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class NewBehaviourScript : MonoBehaviour {
	void Awake() {
		Debug.Log("Awake");
	}
	
	// 初始化
	void Start() {
		Debug.Log("Start");
	}
	
	// 每一帧进行一次刷新
	void Update() {
		Debug.Log("Update");
	}
	
	void FixedUpdate() {
		Debug.Log("FixedUpdate");
	}
	
	void LateUpdate() {
		Debug.Log("LateUpdate");
	}
	
	void OnGUI() {
		Debug.Log("OnGUI");
	}
	
	void OnDisable() {
		Debug.Log("OnDisable");
	}
	
	void OnEnable() {
		Debug.Log("OnEnable");
	}
}
```
运行结果如下：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190912110315658.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
MonoBehaviour基本行为或事件触发的条件如下：
- Awake() : 当前控制脚本实例被装载的时候调用。一般用于初始化整个事例使用。
- Start() : 当前控制脚本第一次执行Update之前调用。
- Update() : 每帧都执行一次。
- FixedUpdate() : 每固定帧绘制时执行一次，和Update不同的是FixedUpdate是渲染帧执行，如果你的渲染效率底下的时候FixedUpdate的调用次数就会下降。
- LateUpdate() : 在每帧执行完毕调用。
- OnEnable() : 当对象变为可用或激活状态时此函数被调用，OnEnable不能用于协同程序。
- OnDisable() : 当对象变为不可用或非激活状态时此函数被调用。
- OnGUI() : 绘制GUI时候触发。

#### 4. 查找脚本手册，了解 GameObject，Transform，Component 对象
##### 分别翻译官方对三个对象的描述（Description）

> GameObject ：GameObjects are the fundamental objects in Unity that represent characters, props and scenery. They do not accomplish much in themselves but they act as containers for Components, which implement the real functionality.

GameObjects是Unity中的基本对象，代表角色、道具和场景。它们本身不具有多少功能，但它们充当了组件的容器，由组件来实际实现功能。

> Transform ：The Transform component determines the Position, Rotation, and Scale of each object in the scene. Every GameObject has a Transform.

Transform组件决定了游戏对象的位置、旋转与缩放比例。每一个游戏对象都有Transform组件。

>Component ：Components are the nuts & bolts of objects and behaviors in a game. They are the functional pieces of every GameObject.

Component游戏中对象和行为的细节。他们是每个游戏对象的功能部分。

##### 描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、 table 的部件
本题目要求是把可视化图形编程界面与 Unity API 对应起来，当你在 Inspector 面板上每一个内容，应该知道对应 API。


- table对象（实体）的属性：Tag、Layer
- table的Transform属性：Position、Rotation、Scale
- table的部件：Mesh Filter、Box Collider、Mesh Renderer

#####  用 UML 图描述三者的关系
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190912120548877.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
#### 5. 资源预设（Prefabs）与 对象克隆 (clone)
##### 预设（Prefabs）有什么好处？
预设是组件的集合体，预设的物体可以实例化成为游戏对象。预设可以重复地创建具有相同结构的游戏对象，便于批量处理。
##### 预设与对象克隆 (clone or copy or Instantiate of Unity Object) 关系？
克隆游戏对象需要场景中有被克隆对象，而创建预制只需事先创建预制即可，允许场景中一开始并不存在该游戏对象。

克隆出来的游戏对象并不会随着被克隆体的变化而发生变化，但是使用预制创建出来的对象会随着预制的改变而发生改变。
##### 制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象
```c
 void Start () {  
        GameObject anotherTable = (GameObject)Instantiate(table.gameObject);  
    }
```

---
## 2、 编程实践，小游戏
#### 井字棋
变量声明：
```c
private int turn = 1;   //1表示O的回合，-1表示X的回合
private int[,] state = new int[3, 3];  //棋盘该处的状态，0为空，1为O，2为X
```
start函数：
```c
// Start is called before the first frame update
void Start()
{
    Reset();
}
```
创建GUI界面：利用循环创建Buttons
```c
void OnGUI()
{
    if (GUI.Button(new Rect(Screen.width / 2 - 45, Screen.height / 2 + 130, 100, 50), "RESTART"))
        Reset();
    int result = checkResult();
    if (result == 1)
    {
        GUI.Label(new Rect(Screen.width / 2 - 20, Screen.height / 2 + 75, 100, 50), "O WINS！");
    }
    else if (result == 2)
    {
        GUI.Label(new Rect(Screen.width / 2 - 20, Screen.height / 2 + 75, 100, 50), "X WINS！");
    }
    else if (result == 3)
    {
        GUI.Label(new Rect(Screen.width / 2 - 20, Screen.height / 2 + 75, 100, 50), "   Tie! ");
    }
    
    for (int i = 0; i < 3; ++i)
    {
        for (int j = 0; j < 3; ++j)
        {
            if (state[i, j] == 1)
                GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), "O");
            if (state[i, j] == 2)
                GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), "X");
            if (GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), ""))
            {
                if (result == 0)
                {
                    if (turn == 1)
                        state[i, j] = 1;
                    else
                        state[i, j] = 2;
                    turn = -turn;
                }
            }
        }
    }
}
```
重置函数Reset：
```c
// 初始化
void Reset()
{
    turn = 1;
    for (int i = 0; i < 3; ++i)
    {
        for (int j = 0; j < 3; ++j)
        {
            state[i, j] = 0;
        }
    }
}
```
通过循环检查state来判断结果：

```c
int checkResult()
{
    // 横
    for (int i = 0; i < 3; ++i)
    {
        if (state[i, 0] != 0 && state[i, 0] == state[i, 1] && state[i, 1] == state[i, 2])
        {
            return state[i, 0];
        }
    }
    // 竖
    for (int i = 0; i < 3; ++i)
    {
        if (state[0, i] != 0 && state[0, i] == state[1, i] && state[1, i] == state[2, i])
        {
            return state[0, i];
        }
    }
    // 斜
    if (state[1, 1] != 0 &&
        state[0, 0] == state[1, 1] && state[1, 1] == state[2, 2] ||
        state[0, 2] == state[1, 1] && state[1, 1] == state[2, 0])
    {
        return state[1, 1];
    }

    // 平局
    int count = 0;
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            if (state[i, j] != 0)
            {
                count++;
            }
        }
    }
    if (count == 9)
    {
        return 3;
    }
    return 0;
}
```

##### 游戏界面
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190912152737749.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190912152938898.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70) 

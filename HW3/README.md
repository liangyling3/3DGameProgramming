### 1. 简答并用程序验证
#### (1) 游戏对象运动的本质是什么？
游戏对象运动的本质是游戏对象的position、rotation、scale等属性随着帧的改变而发生改变。
#### (2) 请用三种方法以上方法，实现物体的抛物线运动。（如，修改Transform属性，使用向量Vector3的方法…）
1. 修改Transform属性
```c
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicMotion : MonoBehaviour
{
    public float v0;    // 水平方向初速度
    public float v1;    // 垂直方向初速度
    public float g; // 重力
    public float t; // 时间

    // Start is called before the first frame update
    void Start()
    {
        v0 = 8.0f;
        v1 = -2.5f;
        g = 9.8f;
    }

    // Update is called once per frame
    void Update()
    {
        t = Time.deltaTime;
        if (this.transform.position[1] > -1.5)
        {
            this.transform.position += new Vector3(v0 * t, v1 * t - 0.5f * g * t * t, 0.0f);
        }
    }
}

```

2. 使用Vector3变量的Lerp函数（插值）
```c
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicMotion3 : MonoBehaviour
{
    public float v0;    // 水平方向初速度
    public float v1;    // 垂直方向初速度
    public float g; // 重力
    public float t; // 时间

    // Start is called before the first frame update
    void Start()
    {
        v0 = 8.0f;
        v1 = -2.5f;
        g = 9.8f;
    }

    // Update is called once per frame
    void Update()
    {
        t = Time.deltaTime;
        if (this.transform.position[1] > -1.5)
        {
            Vector3 vec = new Vector3(v0 * t, v1 * t - 0.5f * g * t * t, 0.0f);
            // lerp：插值
            this.transform.position = Vector3.Lerp(transform.position, transform.position + vec, 1);
        }
    }
}

```
3. 利用Rigidbody中的use gravity，直接用重力实现抛物线运动。
```c
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicMotion3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 利用vector3赋初速度
        this.GetComponent<Rigidbody>().velocity = new Vector3(8.0, -2.5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
```
 #### (3) 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。
创建球体游戏对象：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190921193225244.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)

为星球贴图，并大致调整大小和位置关系。![在这里插入图片描述](https://img-blog.csdnimg.cn/20190921200924477.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
参照行星自转与公转的速度与周期，设置大致的参数，将以下脚本挂载到游戏对象Sun上面，运行即可。
```c
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 太阳自转
        GameObject.Find("Sun").transform.Rotate(Vector3.up * Time.deltaTime * 5);

        // 水星公转
        GameObject.Find("Mercury").transform.RotateAround(Vector3.zero, new Vector3(0.1f, 1, 0), 48 * Time.deltaTime);
        // 水星自转
        GameObject.Find("Mercury").transform.Rotate(Vector3.up * Time.deltaTime * 10000 / 58);

        GameObject.Find("Venus").transform.RotateAround(Vector3.zero, new Vector3(0, 1, -0.1f), 35 * Time.deltaTime);
        GameObject.Find("Venus").transform.Rotate(Vector3.up * Time.deltaTime * 10000 / 243);

        GameObject.Find("Earth").transform.RotateAround(Vector3.zero, new Vector3(0, 1, 0), 30 * Time.deltaTime);
        GameObject.Find("Earth").transform.Rotate(Vector3.up * Time.deltaTime * 10000);

        GameObject.Find("Moon").transform.RotateAround(Vector3.zero, new Vector3(0, 1, 0), 5 * Time.deltaTime);
        GameObject.Find("Moon").transform.Rotate(Vector3.up * Time.deltaTime * 10000 / 27);

        GameObject.Find("Mars").transform.RotateAround(Vector3.zero, new Vector3(0.2f, 1, 0), 24 * Time.deltaTime);
        GameObject.Find("Mars").transform.Rotate(Vector3.up * Time.deltaTime * 10000);

        GameObject.Find("Jupiter").transform.RotateAround(Vector3.zero, new Vector3(-0.1f, 2, 0), 13 * Time.deltaTime);
        GameObject.Find("Jupiter").transform.Rotate(Vector3.up * Time.deltaTime * 10000 / 0.3f);

        GameObject.Find("Saturn").transform.RotateAround(Vector3.zero, new Vector3(0, 1, 0.2f), 9 * Time.deltaTime);
        GameObject.Find("Saturn").transform.Rotate(Vector3.up * Time.deltaTime * 10000 / 0.4f);

        GameObject.Find("Uranus").transform.RotateAround(Vector3.zero, new Vector3(0, 2, 0.1f), 7 * Time.deltaTime);
        GameObject.Find("Uranus").transform.Rotate(Vector3.up * Time.deltaTime * 10000 / 0.6f);

        GameObject.Find("Neptune").transform.RotateAround(Vector3.zero, new Vector3(-0.1f, 1, -0.1f), 5 * Time.deltaTime);
        GameObject.Find("Neptune").transform.Rotate(Vector3.up * Time.deltaTime * 10000 / 0.7f);
    }
}
```

## 2、编程实践
- 阅读以下游戏脚本
> Priests and Devils
> Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are 3 priests and 3 devils at one side of the river. They all want to get to the other side of this river, but there is only one boat and this boat can only carry two persons each time. And there must be one person steering the boat from one side to the other side. In the flash game, you can click on them to move them and click the go button to move the boat to the other direction. If the priests are out numbered by the devils on either side of the river, they get killed and the game is over. You can try it in many > ways. Keep all priests alive! Good luck!

程序需要满足的要求
- [play the game](http://www.flash-game.net/game/2535/priests-and-devils.html)
- 列出游戏中提及的事物（Objects）
	- priest, devil, river, boat
- 用表格列出玩家动作表（规则表），注意，动作越少越好

| 动作 | 对象 | 描述 |
|--|--|--|
| 上下船 | 牧师/恶魔 | 从岸上到船上或从船上到岸上|
|等待|牧师/恶魔|在岸上等待|
|行驶|牧师/恶魔|让船从一岸行驶到对岸|
|杀人|恶魔|当恶魔人数多于牧师时，恶魔杀死牧师，游戏结束|


- 请将游戏中对象做成预制
	- 如图，将创建的对象做成预制，放入Prefabs文件夹。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190922000957554.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)

- 在 GenGameObjects 中创建 长方形、正方形、球 及其色彩代表游戏中的对象。
	- 以黑色球体代表devils，白色正方体代表priests。
	
- 使用 C# 集合类型 有效组织对象
- 整个游戏仅主摄像机和一个 Empty 对象， 其他对象必须代码动态生成！！！ 整个游戏不许出现 Find 游戏对象， SendMessage 这类突破程序结构的通讯耦合语句。 违背本条准则，不给分
- 请使用课件架构图编程，不接受非 MVC结构程序
	- Model：场景中的所有GameObject，它们受到Controller的控制。
	- View：UserGUI和ClickGUI，提供用户交互的界面和动作。
	- Controller：MyCharacterController、BoatController、CoastController、以及更高一层的FirstController（控制着这个场景中的所有对象）。最高层的Controller是Director类。
- 注意细节，例如：船未靠岸，牧师与魔鬼上下船运动中，均不能接受用户事件！

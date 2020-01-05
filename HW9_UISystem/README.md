## 血条（Health Bar）的预制设计。
具体要求如下：
- 分别使用 IMGUI 和 UGUI 实现
- 使用 UGUI，血条是游戏对象的一个子元素，任何时候需要面对主摄像机
- 分析两种实现的优缺点
- 给出预制的使用方法
### 具体实现
#### IMGUI
按照步骤添加血条。
- 步骤参考：[使用IMGUI和UGUI实现人物血条](https://blog.csdn.net/qq_36297981/article/details/80588788)

给 Canvas 添加脚本 LookAtCamera.cs，使血条跟着人物的旋转，始终面对着屏幕。
```c#
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    void Update () {
        this.transform.LookAt (Camera.main.transform.position);
    }
}
```
#### UGUI
通过ONGUI中的HorizontalScrollbar组件来实现血条的操作。
```c
public class BloodBarTest : MonoBehaviour
{
    public GUISkin theSkin;
    public float bloodValue = 0.0f;
    private float tmpValue = 1.0f;
    private Rect rctBloodBar;
    private Rect rctUpButton;
    private Rect rctDownButton;
    private bool onoff;

    void Start()
    {
        //血条或者进度条-横向  
        GameObject obj = Instantiate(Resources.Load("prefabs/Blood")) as GameObject;
        GameObject par = GameObject.Find("Red");
        obj.transform.SetParent(par.transform);
        //obj.transform.position = new Vector3(3, 0, 0);
        //obj.transform.name = "ThirdPersonController";

        rctBloodBar = new Rect(450, 450, 150, 10);
        rctUpButton = new Rect(800, 200, 80, 40);
        rctDownButton = new Rect(800, 300, 80, 40);

    }

    void OnGUI()
    {
        GameObject obj = GameObject.Find("Black");
        Vector3 pos = obj.transform.position;
        //Debug.Log(pos);
        GUI.skin = theSkin;
        if (GUI.Button(rctUpButton, "黑方加血"))
        {
            tmpValue += 0.1f;
        }
        if (GUI.Button(rctDownButton, "黑方减血"))
        {
            tmpValue -= 0.1f;
        }
        if (tmpValue > 1.0f) tmpValue = 1.0f;
        if (tmpValue < 0.0f) tmpValue = 0.0f;

        Vector3 pos1 = pos + new Vector3(0.0f, 0.5f, 0.0f);
        Vector3 wpos = Camera.main.WorldToScreenPoint(pos);

        GUI.HorizontalScrollbar(new Rect(wpos.x-80, wpos.y,150, 10), 0.0f, tmpValue, 0.0f, 1.0f, GUI.skin.GetStyle("horizontalscrollbar"));

    }
}
```
### 效果截图
![在这里插入图片描述](https://img-blog.csdnimg.cn/20200105220424271.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
![在这里插入图片描述](https://img-blog.csdnimg.cn/20200105220434725.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
### IMGUI、UGUI的优缺点分析
#### IMGUI
优点：
- 开发简单，仅需几行代码

缺点：
- IMGUI需要在将3D位置映射到屏幕位置后，如果对结果进行加减，会使得血条位置偏移过多。

#### UGUI
优点： 
- 5.2版本之后，Unity逐渐将一部分UGUI的计算放到子线程去做，以此来缓解主线程的压力； 
-  UGUI有锚点，更方便屏幕自适应。  

缺点： 
- UGUI如果人物过多,需要太多的canvas

### 预制的使用方法
1. 直接将IMGUI-H-Bar预制体拖入场景
2. 按照前面提到的方法导入资源，用预制体生成游戏对象Ethan，构建基本场景
3. 将Canvas预制体拖入到Ethan对象，成为其子对象
4. 将Canvas的子对象Slider拖入IMGUI-H-Bar对象的IMGUI.cs组件中的HealthSlider属性
5. 运行后点击增/减血按钮即可实现两种血条的同时增/减血


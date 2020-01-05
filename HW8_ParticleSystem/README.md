## 视频演示
[优酷](https://v.youku.com/v_show/id_XNDQ5NTYwMjQ4NA==.html?spm=a2h3j.8428770.3416059.1)

## 简单粒子制作

- 按参考资源要求，制作一个粒子系统，参考资源
- 使用 3.3 节介绍，用代码控制使之在不同场景下效果不一样
### 效果实现
#### 添加粒子系统
添加粒子系统，进行三种光的模拟：
- shining：原粒子
- purple：紫色光
- pink：粉色光
其中紫色光和粉色光使用代码进行控制。
#### 代码控制
##### purpleChange.cs
```c#
public class purpleChange : MonoBehaviour {
    ParticleSystem exhaust;
    float size = 5f;

    // Use this for initialization
    void Start()
    {
        exhaust = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        size = size * 0.999f;
        var main = exhaust.main;
        main.startSize = size;
    }
}

```
##### pinkChange.cs
```c#
public class pinkChange : MonoBehaviour {

    ParticleSystem exhaust;
    float size = 2f;

    // Use this for initialization
    void Start()
    {
        exhaust = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        size = size * 0.999f;
        var main = exhaust.main;
        main.startSize = size;
    }

}
```
### 效果图
![在这里插入图片描述](https://img-blog.csdnimg.cn/20200105210708327.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
![在这里插入图片描述](https://img-blog.csdnimg.cn/20200105210719962.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)

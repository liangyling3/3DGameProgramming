# 游戏对象与图形基础
### 1、基本操作演练
#### 下载 Fantasy Skybox FREE， 构建自己的游戏场景

1. 在Assets中载入Fantasy Skybox FREE
2. 创建一个新的Material，更改Shader为Skybox-6 Sided
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190930143754592.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
3. 在Fantasy Skybox Free中的Textures文件夹，找到贴图文件，拖入到shader对应的位置（这里选择的是Sunny_01B）。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190930143722296.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
4. 创建一个Terrain，在inspector界面添加树木对象并随机放置，效果如图：
![在这里插入图片描述](https://img-blog.csdnimg.cn/201909301451556.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)

5. 添加草坪、风等。

最终效果见视频。
#### 写一个简单的总结，总结游戏对象的使用
1. 游戏对象可以直接在scene中创建，也可以在代码中使用Instantiate() 函数来实例化预制，动态载入资源。
2. 游戏对象是组件的载体，如通过组件Transform、Filter、Render等可以改变游戏对象的呈现效果（位置、大小、材质等）。
3. 脚本是也是游戏对象的组件，可以挂载到游戏对象上。脚本通过代码实现对游戏对象的控制，并建立玩家（用户）与游戏对象之间的联系，使游戏对象能够真正实现其游戏功能。
### 2、编程实践
#### 牧师与魔鬼 动作分离版
- 设计一个裁判类，当游戏达到结束条件时，通知场景控制器游戏结束

##### 动作分离
创建一个新的类Action，在不同控制器中进行实例化，并将动作的实现放在Action中。

1. 在人物控制器、船控制器、岸控制器中分别进行Action类的实例化，实现人物上船、上岸的动作：
```c
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
		characterCtrl.setCoastController(coastCtrl);
		characterCtrl.setCharacterTransformParent(null);;
		characterCtrl.setOnBoat(false);
	}
```
2. 在船控制器中，对下船以及船的移动进行动作分离。
```c
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
```
3.  在岸控制器中，对人物离开岸进行动作分离。
```c
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
```

在原来的代码中，游戏结束的检验是在场景控制器中直接用函数实现的，代码如下：
```c
// 0：未结束 1：lose 2：win
	int check() {	
		int from_priest = 0, from_devil = 0;
		int to_priest = 0, to_devil = 0;

		int[] fromCount = fromCoast.getCharacterNum();
		from_priest += fromCount[0];
		from_devil += fromCount[1];

		int[] toCount = toCoast.getCharacterNum();
		to_priest += toCount[0];
		to_devil += toCount[1];

		// win
		if (to_priest + to_devil == 6)		
			return 2;

		int[] boatCount = boat.getCharacterNum();
		if (boat.get_to_from() == -1) {	// 船在目的地
			to_priest += boatCount[0];
			to_devil += boatCount[1];
		} 
		else {	// 船在出发地
			from_priest += boatCount[0];
			from_devil += boatCount[1];
		}

		// lose
		if ((from_priest < from_devil && from_priest > 0) || (to_priest < to_devil && to_priest > 0)) {		
			return 1;
		}
		return 0;			
	}
}
```

##### 裁判类
创建一个裁判类Judge。将原代码中的check() 函数更改为Judge中的update函数。在场景控制器中增加一个Judge实例judge，原来使用check() 的位置调用judge.update() 对其状态进行更新，并调用getStatus() 获取状态。
```c
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
```

### 3、材料与渲染联系
#### 从 Unity 5 开始，使用新的 Standard Shader 作为自然场景的渲染。
- 阅读官方 Standard Shader 手册 。
- 选择合适内容，如 Albedo Color and Transparency，寻找合适素材，用博客展示相关效果的呈现

在Transparent的渲染模式下，通过对于Albedo参数的调整，可以设置材料的基色及透明度。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191006225550607.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
#### Unity 5 声音
- 阅读官方 Audio 手册
- 用博客给出游戏中利用 Reverb Zones 呈现车辆穿过隧道的声效的案例

在场景中加入Audio Source 和 Audio Reverb Zone。
![在这里插入图片描述](https://img-blog.csdnimg.cn/201910062300309.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)

在Audio Reverb Zone中设置Reverb Preset为Cave，加入声音资源，并调整参数实现音效。

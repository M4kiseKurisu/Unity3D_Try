#### Unit Base Setup

- 注意创建对象的层次，尽量不要将所有Components都放到一个Game Object中（可以尝试创建一个Empty Game Object用作当前对象的根结点）

- 完成内容：创建empty object: Unit并将一个人物模型当作其子对象（相当于初始化可操控对象）；创建一个3D Object: Plane用作地面

#### Unit Move

- 将所有对象的自定义控制脚本放在Game Object根结点中
- 当Unity Scene选中Main Camera时，点击cmd + shift + F可以将当前场景中的camera角度同步为scene中的角度
- 不要在代码中加入膜法数字，将数字赋值给有语义的变量

- 完成内容：完成玩家的基本移动逻辑（目前只能感应固定键并移动到代码制定位置），同时添加移动结束逻辑（当距离目标位置小于固定值时视为移动结束）

#### Mouse Raycast

- Input.mousePosition返回的是当前鼠标在屏幕上的那个像素点上，并非鼠标在游戏世界中的位置
- Camera.main返回的是当前场景中Tag为MainCamera的对象
- Camera.main.ScreenPointToRay(Input.mousePosition)获取一条从Main Camera到屏幕像素点的游戏空间内的射线
- Physics.Raycast(ray)可以检测当前射线是否射到Collider上（与视觉效果无关，只检测Collider）

- 完成内容：创建一条从摄像机到鼠标位置的射线，并检测是否经过Collider

#### Mouse World Position

- 可以通过这个形式：Physics.Raycast(ray, out RaycastHit raycastHit)获取当前射线探测到的对象：raycastHit的相关信息
- LayerMask一种表示形式是int，例如当前的Layer是6那么就需要用1 << 6这样的位运算填充对应LayerMask属性，或者直接设置一个这个对象并在unity inspector中修改
- 设置单例/静态方法的时候需要详细思考，确定其必要性

- 完成内容：使用一个sphere测试鼠标检测情况，使用一个单例/静态类利用Raycast获取当前鼠标位置，为地面plane对象添加一个专门的layer供raycast进行检测

#### Unit Click To Move

- Input.GetMouseButtonDown(0)代表按下左键，如果是监测按下右键则应该使用1

- 完成内容：基于前面内容，检测鼠标左键输入，并将target位置设置为鼠标点击位置，让控制对象向鼠标点击位置移动

#### Unit Animations Setup

- 完成内容：从mixamo网站下载持枪移动动画，并进行调整适配

#### Unit Animator

- 完成内容：创建animator并添加两个基本状态，为玩家的手骨架增加一把枪

#### Unit Animator Parameters

- 完成内容：为Animator添加一个切换所需的bool：IsWalking，在Unit中设置：如果当前对象正在移动则设置IsWalking也为true，反之亦然，这样移动时动画也正确

#### Unit Rotate when Moving

- 通过修改transform.forward改变对象的朝向方向
- 如果想要实现渐变效果，可以使用Vector3.Lerp对两个向量进行一个时间段上的线性插值（如果第一个向量随时间变化，实际是一个从快到慢渐出的效果）

- 完成内容：让玩家在移动的时候面向方向旋转到移动方向，同时使用Lerp让这个面向方向渐进而非瞬间改变

#### Unit Selection

- 创建的管理游戏的System可以挂载在scene中的同名空对象上
- 使用这种try api进行component的获取可以直接获取out参数，后需不需要进行null判断（直接通过api输出bool判断）：transform.TryGetComponent<Unit>(out Unit unit)
- Project Setting > Script Execution Order可以指定特定脚本的Awake，Start等时序方法的具体执行顺序

- 完成内容：添加一个UnitActionSystem控制当前的对象选择情况，并通过点击时检测鼠标raycast的对象进行控制对象切换

#### Unit Selected Visual

- 当两个对象y相同的时候，unity的渲染可能因为不知道覆盖关系而导致闪烁，可以略微提高其中一个对象的y值
- 解耦和的一个方法：使用事件（观察者模式），一个标准的事件使用public event EventHandler(object sender, EventArgs e)，相当于csharp底层定义的一个delegate
- 这种EventHandler的触发方式：OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty); 第一个是sender，第二个是参数表
- EventHandler可以订阅的方法定义：private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty) {...}
- 所有Awake都在所有Start前触发：在awake时只设置自身，在start时可以和其他外部对象交流
- 类似UnitActionSystem这样的系统可以考虑使用单例模式，暴露get让外界调用自身

- 完成内容：通过一个事件监听选择角色切换，然后让当前正在控制的对象脚下出现绿圈

#### Grid System

- 一个继承MonoBehaviour的对象无法使用同名构造函数进行初始化
- Debug.DrawLine方法可以通过两个输入的向量，在scene中渲染一条直线
- 像GridPosition这样的简单数据类型可以考虑设置为struct，结构体不同于类，是一种值类型，进行参数传递传递的是副本而不是引用
- 为了让GridPosition在被debug.log打印时输出可用信息，可以人工重写这个结构体的tostring方法（debug.log实际上调用的就是这个方法）
- Mathf.RoundToInt()方法可以将float向下取整为int

- 完成内容：使用一个GridSystem基本类控制当前场景内的grid情况，使用一个GridPosition数据类存储格子相关信息，并在GridSystem中完成grid position到world position的互相转换（外界可以调用转换方法），最后重写了GridPosition的ToString方法
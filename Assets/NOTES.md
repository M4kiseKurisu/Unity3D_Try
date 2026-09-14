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

#### Grid Object

- 使用GridObject[,] gridObjectArray;创建一个二维数组引用，在初始化中gridObjectArray = new GridObject[width, height];这样进行实际堆空间引用
- Transform和GameObject几乎可以混用，因为每个游戏对象一定有一个Transform Component，而每个Transform也必然依存于一个GameObject

- 完成内容：创建一个GridObject类型，让Grid对每一个格子生成一个此类型存储游戏过程中此格子上的相关信息，使用一个二维数组储存当前gridSystem中所有创建的gridObject，添加一个gridDebugObject TMP对象用来测试生成GridObject情况

#### Grid Object Debug

- 注意保持Scripts文件夹的结构合理性

- 完成内容：为上一节创建的GridDebugObject添加可以改变其text，展示debug内容的脚本，重写这个GridDebugObject对应的GridObject的ToString方法，让其能够显示这个Object的相关信息

#### Level Grid

- public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);这是一种lambda表达式对于方法的简写，效果完全等同于使用一般方法的大括号
- 如果想使用== / !=对自定义对象进行比较，需要使用public static bool operator ==(GridPosition a, GridPosition b) / public static bool operator !=(GridPosition a, GridPosition b)这种方式重载运算符
- 可以重写Equals(object obj)和GetHashCode()，用来实现自定义的相等比较，防止对于结构体类型相等的判断需要使用默认的低性能反射机制
- 可以继承: IEquatable<GridPosition>，需要重新写Equals并用上面的Equals结果进行bool输出

- 完成内容：使用一个LevelGrid存储和管理Unit在GridObject上的情况，同时要考虑多个Unit在同一个格子上的情况
- 完成内容：为GridObject的ToString方法输出的字符串后面添加GridObject上面的unit的信息

#### Cinemachine

- cinemachine包需要从package manager中下载，并非创建工程自带
- 在新版cinemachine中，如果是3d场景，需要调整Procedural components中的position control为follow（相当于旧版transposer）才能够实现镜头跟随，如果是none无法触发跟随
- 控制cinemachine追踪的对象和camera的关系：调整follow offset
- 如果要考虑到镜头旋转：rotation control改成rotation composer:时刻旋转镜头让target保持在镜头内特定区域/rotate with follow target直接让镜头跟随目标进行同步旋转
- 可通过damping调节镜头追踪阻尼感

- 完成内容：创建一个cinemachine及其对应的追踪target空对象，初步建立镜头跟随机制

#### Camera Move and Rotate

- 注意，如果一个对象同时可能出现move和rotate活动，不能直接加移动方向，而是要先使用transform.forward再乘以纵向移动，transform.right乘以横向移动，这样通过使用transform已经将其rotate考虑在内
- 调整一个transform的旋转情况：修改transform.eulerAngles

- 完成内容：通过代码控制镜头上下左右移动、左右水平旋转

#### Camera Zoom

- 镜头拉近的方法：降低镜头fov/降低follow offset y、z
- 监控鼠标滚轮输入：Input.mouseScrollDelta，当上滚的时候返回二维向量y值为1；向下滚的时候返回二维向量y值为0
- 想要定义一些边界数据，可以使用：private const float
- UnityEngine提供：Mathf.Clamp(当前值，最大值，最小值)，可以为当前值限定范围，超出范围则将值改为界限

- 完成内容：通过代码控制镜头的缩放（检测鼠标滚轮）

#### Move Action

- 想要对gridPosition进行加减同样需要进行运算符重载：public static GridPosition operator +(...)

- 完成内容：将点击移动角色的操作从Unit移动到了MoveAction，在MoveAction中进行玩家可达gridPosition的判定

#### Move Action Validate

- 完成内容：在鼠标点击，通过MoveAction移动之前，首先进行判断：鼠标点击位置的grid是否：超出网格范围、为当前对象网格、网格超出移动范围、网格上有其他对象等。只有这个点击有效，不出现以上情况才会进行实际移动

#### Grid Visual

- 使用Instantiate(prefab_transform, position, rotation)在场景中从prefab创建对象

- 完成内容：添加当前对象可以前往的grid的视觉效果，主要是添加了一个可达grid的显示prefab以及一个控制可达grid显示的单例GridSystemVisual

#### Base Action Class

- 如果子类需要覆盖重写父类的某个方法，父类对象上应该添加virtual关键字，子类对象上应该添加override关键字
- 子类方法如果想要使用父类同名方法，可以使用base
- 如果想要确保抽象基类不会被new创建，可以为其添加abstract关键字

- 完成内容：为操控对象添加一个点击右键原地旋转360度的spinAction
- 完成内容：为moveAction和spinAction添加一个基类baseAction，这样可以将这些具体类的共同部分抽象到基类中

#### Single Active Action

- 可以使用委托delegate将方法当作参数传入其他方法,举例：public delegate void SpinCompleteDelegate(); public void Spin(SpinCompleteDelegate onSpinComplete) {}
- 委托传入的方法类型要和委托类型相同
- csharp有两种内置的delegate类型：action（无返回值）和func（有返回值）

- 完成内容：添加一个isBusy，防止不同的Action同时被调用，确保一个时间只有一个action在运行

#### UI Setup

- 完成内容：添加canvas并修改canvas scaler配置

#### Unit Action System UI

- 在制作button ui的时候可以添加outline / shadow组件，提升美观效果
- 多个ui的排列效果：在父对象中添加grid layout group
- GetComponents<T>可以直接获取当前对象上所有T类型的component引用
- 想要强制子对象实现父对象的虚函数，需要父对象函数添加abstract关键字并忽略函数体：public abstract string GetActionName();
- .ToUpper()将字符串所有字符转化为大写

- 完成内容：添加地步动作按钮组，并让此组订阅操控玩家切换事件以刷新ui，制作行为按钮ui prefab，让其显示操控对象绑定的action类型名
- 完成内容：让baseAction添加输出字符串（当前action名）的虚函数，并让子类分别实现

#### Click UI to Select Action

- 可以在委托处使用匿名函数而非单独定义的方法：button.onClick.AddListener(() => {/*code*/})
- 可以使用switch(父对象)，并在不同case中使用不同子类型的方式进行判断和不同逻辑的处理

- 完成内容：修改UnitActionSystem，记录当前的selected Action并确保鼠标左键点击时激活记录的selected Action执行逻辑

#### Generic Take Action

- 可以使用EventSystem.current.IsPointerOverGameObject()判断当前鼠标是否在ui对象上

- 完成内容：改变架构方式，使用一个虚方法TakeAction代替Spin和Move，这样就可以在UnitActionSystem中直接调用这个虚方法，不需要进行子类型的区分，同时为基类添加gridPosition是否有效的判断方法
- 完成内容：确保点击下方button时不会触发对应对象的移动，同时当点击spin时有效gridPosition的地块ui进行更新

#### Selected Action UI Visual

- 完成内容：为当前选择动作添加一个ui，并为selected Action的设置添加一个事件，让选择动作ui刷新方法订阅这个事件

#### Action Busy UI

- 完成内容：为action行动期间添加一个繁忙ui遮盖动作按钮，同时让这个ui的相关逻辑订阅繁忙事件

#### Action Points

- 完成内容：为角色添加行动点数，为baseAction基类添加行动消耗点数，在行动开始前比对并花费点数进行行动
- 完成内容：添加当前选择角色行动点数的展示ui及刷新此ui的方法，在游戏开始时、选择角色切换时及动作开始事件时（订阅这个事件）调用刷新ui方法，更新剩余行动点显示

#### Turn System

- 可以使用静态的事件：public static event EventHandler OnAnyActionPointsChanged;来代表所有此类对象均会触发的事件

- 完成内容：添加一个回合系统，以及与之匹配的当前回合数显示ui及进入下一回合的按钮ui，同时设置进入新回合的事件，让回合显示刷新订阅此事件
- 完成内容：在unit中订阅进入新回合的事件，并重置行动点，为unit添加一个静态的行动点变化事件，让unitActionUI订阅这个事件来进行当前行动点ui渲染

#### Enemy

- 当制作和当前prefab类似的其他prefab时，可以使用当前prefab的prefab variant，相当于继承自原本的prefab并可以进行一定修改

- 完成内容：添加一个enemy prefab variant并添加进scene，修改unit代码让其兼容敌人对象
- 完成内容：在TurnSystem中添加我方回合、敌方回合的概念，添加敌方回合的标识ui，并让这个ui的更新订阅轮次变化事件、让玩家的跳过回合按钮的显示刷新也订阅轮次变化
- 完成内容：添加一个简单的enemyAI控制逻辑，控制敌方回合经过两秒钟之后调用NextTurn方法变回我方回合

#### Shoot Action

- 完成内容：添加shoot action，继承base action，让shoot action测试射击范围内的所有grid，只有上面有敌对单位的时候这个grid才有效（射击距离的计算使用折线距离）
- 完成内容：在shoot action中添加状态机，在瞄准、射击、冷却三个状态间进行切换和维护，在射击状态才能触发射击动作，为unit添加一个简单的受击逻辑Damaged()，为射击状态添加一个玩家朝向旋转到敌人方向的逻辑

#### Unit Animator

- 只需要单独检测进入状态，这个状态中实施一遍后自动退出，这种情况可以考虑使用trigger进行指示，通过have exit time返回

- 完成内容：将所有控制animator的代码统一到UnitAnimator中，为射击添加一个动画状态并使用shoot触发器触发，这个控制代码通过订阅各个action中设置的事件来触发

#### Shooting Bullet Visual

- 类似子弹轨迹这种特效，可以使用effect > trail来实现，通过调整trail renderer中的图可以控制轨迹的粗细情况，通过调整time调节轨迹出现时间，AutoDestruct控制轨迹没有长度时自动销毁
- 如果为了material添加闪光效果，可以添加emission
- 可以使用public class OnShootEventArgs : EventArgs {}这种方法来自定义EventHandler传递的参数。调用方法：public event EventHandler<OnShootEventArgs> OnShoot;
- 类似子弹爆炸的特效可以考虑particle system

- 完成内容：添加一个具体的射击动画状态，并添加对应的射击轨道及击中特效

#### Health System

- 在删除对象的时候，注意对象上挂载的事件订阅要及时使用-=解除订阅

- 完成内容：为Unit添加了一个HealthSystem记录当前hp，并提供方法造成伤害，创建一个角色死亡事件供其他部分调用，在Unit中订阅此事件并销毁对象

#### Ragdoll

- 可以在GameObject > 3D Object > Ragdoll中将一个人物的各个关节加入对应部分并创建的方式为人物创建Ragdoll（理解为在场景中会直接倒下的“布娃娃”）
- 可以使用RigidBody.AddExplosionForce来为一个对象添加爆炸力，参数包括：float explosionForce, Vector3 explosionPosition, float explosionRange

- 完成内容：使用Ragdoll创建一个Unit的布偶，来模拟对象死亡时倒下的状态，同时创建一个Prefab Variant来为敌人创建红色材质的Ragdoll
- 完成内容：在创建Ragdoll的时候，需要将当前对象的Transform递归的进行position和rotation的复制，这样生成的Ragdoll的初始状态就不是默认的T-pose了，同时在Ragdoll生成时递归的为其各个部分添加一个ExplosionForce，模拟死亡效果

#### Unit World UI

- 可以在Prefab中添加Canvas并将其Render Mode改成World，这样UI就在世界坐标系下进行渲染了
- 在更新时如果需要考虑顺序，可以将一定最后运行的逻辑放在LateUpdate中，LateUpdate事件函数一定会在所有Update事件函数之后触发
- 可以使用transform.LookAt让某个transform的朝向偏向LookAt中传入的transform，但是要考虑是否需要反转这个transform的问题
- 可以将image的模式改成filled，用来模拟血条这样的对象

- 完成内容：为每一个Unit添加一个血量条和一个行动点的ui，血量条使用Filled模式的Image实现，同时让这两个ui的刷新方法订阅血量、行动点变化的事件

#### Action Camera

- 可以为Cinemachine设置Priority，这样可以使用优先级比较确定当前使用哪个Virtual Camera，这个不同相机间的切换过程在CinemachineBrain中的Default Blend确定

- 完成内容：添加一个Action Camera，并且为Base Action添加两个事件（Action开始和结束），让Action Camera的显示、隐藏方法订阅相关事件，同时在Action Camera被切换的时候需要计算这个相机的过肩视角位置

#### Grid Visual Colors

- 当需要在[SerializeField]后面添加自定义的struct时，需要为这个struct添加一个[Serializable]标签
- 在代码中修改enum的构成的时候，注意Unity中因为存储的枚举值不会改变，有可能导致枚举错位

- 完成内容：修改GridSystemVisual，让当前可达地块的显示不要在每个Update更新，而是只在事件切换或者玩家移动时更新，让相关更新方法订阅这两个对应的事件
- 完成内容：为每一个Grid的边框添加不同类型的材质，并且在GridSystemVisual中保存，在接受不同Action时将Grid的外观切换至不同颜色的Material
- 完成内容：为shoot action添加一个浅红色的grid效果，用来显示射程范围内的grid

#### Enemy AI Basic

- 完成内容：为EnemyAI添加一个状态enum并维护，添加一个UnitManager，内部使用List存储当前存在的Unit（并且区分我方、敌方Unit）
- 完成内容：为Unit添加两个静态事件：Unit生成及死亡，确保任意单位生成及死亡时触发，为了保证UnitManager监听这两个事件发生在其他相关事件之前，在Unity中设置运行顺序
- 完成内容：在EnemyAI中获取UnitManager的EnemyUnitList，并尝试让所有敌方对象进行Action

#### Enemy AI Complex

- 对于一个List，可以使用Sort进行自动排序，需要人工为Sort方法提供比较函数，可以采用匿名函数实现

- 完成内容：使用一个EnemyAIAction类储存敌人可能采用一个动作的分数，然后为每个Action添加一个计算当前动作分数（即生成EnemyAIAction数据类）
- 完成内容：每次查询当前存在的所有敌人的最优动作，再综合选出一个动作作为当前要做的动作

#### C# Generics

- 泛型：可以理解为可以与多个类型的类进行交互的类/方法。例如List<T>，就可以实例化为List<int>作为整型容器
- 可以使用where T : ..来对交互的类类型进行规定

- 完成内容：统一BaseAction的获取，将Unit中对于不同Action的引用和获取换成对BaseAction泛型操作的方法

#### Grid System Generics

- GridSystem<TGridObject> where TGridObject : new()使用这种方法可以确保TGridObject泛型拥有new构造方法，也可以考虑向GridSystem的构造方法中添加作为TGridObject构造方法的委托参数

- 完成内容：将GridSystem改为泛型GridSystem<TGridObject>（用TGridObject替换原来写死的GridObject），后续可用类似代码框架完成寻路系统

#### Pathfinding Script

- 使用A*寻路算法，本质是计算每个格子到起始位置的花费和到达终点的预计花费（即假设没有障碍），然后计算二者之和，每次移动选择可达的二者和最小的格子作为下一步

- 完成内容：为寻路算法添加属于自己的系统Pathfinding，其中使用GridSystem<PathNode>进行记录，PathNode记录每一格的cost情况，并且使用PathfindingGridDebugObject在地图中显示相关信息
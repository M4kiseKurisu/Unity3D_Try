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

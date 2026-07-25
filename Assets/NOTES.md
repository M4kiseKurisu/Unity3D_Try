#### Unit Base Setup

- 注意创建对象的层次，尽量不要将所有Components都放到一个Game Object中（可以尝试创建一个Empty Game Object用作当前对象的根结点）

- 完成内容：创建empty object: Unit并将一个人物模型当作其子对象（相当于初始化可操控对象）；创建一个3D Object: Plane用作地面

#### Unit Move

- 将所有对象的自定义控制脚本放在Game Object根结点中
- 当Unity Scene选中Main Camera时，点击cmd + shift + F可以将当前场景中的camera角度同步为scene中的角度
- 不要在代码中加入膜法数字，将数字赋值给有语义的变量

- 完成内容：完成玩家的基本移动逻辑（目前只能感应固定键并移动到代码制定位置），同时添加移动结束逻辑（当距离目标位置小于固定值时视为移动结束）

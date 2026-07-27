
---Steps to implement Selection Manager---

1. Drag the SelectionManager prefab into your scene.

2. Select the SelectionManager inside your scene and set the "Units Container" which holds all your gameObjects or selectable objects
by dragging your container from your hierarcy into the "Units Container" slot on the prefabs script.

Selectable objects must be collected under a container gameObject for performance reasons.

After these steps are completed, in play mode you will now be able to drag your mouse cursor and while holding down left click to select gameobjects.

Use the options on the prefab to select which hotkeys multiselect and deseclect selectable gameobjects, set your selection gameObjects which indicate selection and
the ability to filter which game objects within the container are selected.


---Using the Selection Manager script---

You can access the SelectionManager with the following:

SelectionManager selectionManager = GetComponent<SelectionManager>();

There are three public methods which can be used on the SelectionManager.

public GameObject[] GetSelectedObjects();
Returns a list of the currently selected objects.

public void AddGameObjectToSelection(GameObject newObject)
Adds a new game object to be selected.

public void RemoveGameObjectFromSelection(GameObject removedObject)
Removes a game object from selection.


---The SelectableUnit---
Selectable objects can be given custom properties like how large their selection size is to assist in varied sized units.

Also if they belong to the player or not.

The class can also be inherited to recieve events like:

OnBeginSelection();
OnEndSelection();
OnBeginHover();
OnEndHover();

To help you manage your objects.

---Help---

If you have any difficulty using this package please reach me at:
lil_tommy_tom@hotmail.com

There is also an example scene in the project to see how it's used.
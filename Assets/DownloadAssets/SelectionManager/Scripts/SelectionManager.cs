using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class SelectionManager : MonoBehaviour {

    public bool canMakeSelections = true;
    public GameObject hoverProjector;
    public GameObject selectionProjector;

    //custom inspector values
    public GameObject unitsContainer;

    public bool selectByTag = false;
    public string selectedTag;
    public bool selectByLayer = false;
    public int selectedLayer;
    public bool selectByName = false;
    public string selectedName;

    public KeyCode keyMultiselect;
    public KeyCode keyDeselect;

    public KeyCode keySetControlGroup;
    public KeyCode keyAddToControlGroup;

    List<SelectableObject> selectedGameObjects = new List<SelectableObject>();
    List<SelectableObject> hoveredGameObjects = new List<SelectableObject>();
    List<ControlGroupUnit> controlGroupUnits = new List<ControlGroupUnit>();

    bool isSelecting = false;
    Vector3 mousePosition;

    float lastClickTime;
    float doubleClickDelay = 0.2f;
    bool hasDoubleClicked = false;

    struct ControlGroupUnit
    {
        public SelectableObject selectableObject;
        public int controlGroup;

        public ControlGroupUnit(SelectableObject selectableObject, int controlGroup)
        {
            this.selectableObject = selectableObject;
            this.controlGroup = controlGroup;
        }
    }

    struct SelectableObject {
        public GameObject gameObject;
        public GameObject projectorGameObject;
        public Projector projector;
        public SelectableUnit selectable;
        public bool isActive;
    }

    // Use this for initialization
    void Start () {
        if (!Camera.main)
        {
            Debug.Log("The SelectionManager requires a camera with the tag 'MainCamera'");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!Camera.main)
            return;

        if (unitsContainer)
        {
            //check if the user has double clicked
            hasDoubleClicked = false;
            if (Input.GetMouseButtonUp(0))
            {
                float timeSinceLastClick = Time.time - lastClickTime;
                if (timeSinceLastClick <= doubleClickDelay)
                {
                    hasDoubleClicked = true;
                }
                lastClickTime = Time.time;
            }

            //remove selected objects that no longer exist
            selectedGameObjects = selectedGameObjects.Where(i => i.gameObject != null).ToList();

            #region wide assignment to all projectors
            for (int x = 0; x < hoveredGameObjects.Count; x++)
            {
                SelectableObject hoveredObject = hoveredGameObjects[x];
                hoveredObject.isActive = false;
                if (hoveredObject.projector)
                {
                    if (hoveredObject.selectable && hoveredObject.selectable.selectionSize > 0)
                    {
                        hoveredObject.projector.orthographicSize = hoveredObject.selectable.selectionSize + hoveredObject.selectable.selectionSize / 5;
                    }
                }
                hoveredGameObjects[x] = hoveredObject;
            }
            for (int x = 0; x < selectedGameObjects.Count; x++)
            {
                SelectableObject selectedObject = selectedGameObjects[x];
                if (selectedObject.projector)
                {
                    if (selectedObject.selectable && selectedObject.selectable.selectionSize > 0)
                    {
                        selectedObject.projector.orthographicSize = selectedObject.selectable.selectionSize;
                    }
                }
                selectedGameObjects[x] = selectedObject;
            }
            #endregion

            if (canMakeSelections)
            {
                bool setControlGroup = false;
                bool addControlGroup = false;

                if (Input.GetKey(keySetControlGroup))
                {
                    setControlGroup = true;
                }

                if (Input.GetKey(keyAddToControlGroup))
                {
                    addControlGroup = true;
                }

                //check for pressed number keycode
                int pressedNumber = -1;
                for (int i = 0; i < 10; ++i)
                {
                    if (Input.GetKeyUp("" + i))
                    {
                        pressedNumber = i;
                    }
                }

                //check if the currently selected units need to be set as or added to a control group
                if (setControlGroup && pressedNumber >= 0)
                {
                    controlGroupUnits = controlGroupUnits.Where(i => i.controlGroup != pressedNumber).ToList();
                    foreach (SelectableObject selectableObject in selectedGameObjects)
                    {
                        controlGroupUnits.Add(new ControlGroupUnit(selectableObject, pressedNumber));
                    }
                }
                else if (addControlGroup && pressedNumber >= 0)
                {
                    foreach (SelectableObject selectableObject in selectedGameObjects)
                    {
                        controlGroupUnits.Add(new ControlGroupUnit(selectableObject, pressedNumber));
                    }
                }
                else if (pressedNumber >= 0)
                {
                    ClearSelection();
                    foreach(ControlGroupUnit controlGroupUnit in controlGroupUnits)
                    {
                        if (controlGroupUnit.controlGroup == pressedNumber)
                        {
                            AddGameObjectToSelection(controlGroupUnit.selectableObject.gameObject);
                        }
                    }
                }

                #region Find objects currently being hovered over
                //check for gameobjects directly under cursor
                RaycastHit hoverHitSelection;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hoverHitSelection))
                {
                    parseGameObjectForHover(hoverHitSelection.transform.gameObject);
                }

                if (isSelecting)
                {
                    foreach (Transform transform in unitsContainer.transform)
                    {
                        if (IsWithinSelectionBounds(transform.gameObject))
                        {
                            parseGameObjectForHover(transform.gameObject);
                        }
                    }
                }
                #endregion

                // If we press the left mouse button, save mouse location and begin selection
                if (Input.GetMouseButtonDown(0))
                {
                    isSelecting = true;
                    mousePosition = Input.mousePosition;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (isSelecting)
                    {
                        //dont wipe selection if holding shift
                        if (!(Input.GetKey(keyMultiselect)) && !(Input.GetKey(keyDeselect)) && !hasDoubleClicked)
                        {
                            ClearSelection();
                        }

                        List<GameObject> newSelectedObjects = new List<GameObject>();
                        bool isFiltered = false;

                        if (selectByTag && selectedTag.Length > 0)
                        {
                            isFiltered = true;
                            newSelectedObjects.AddRange(Utils.FindGameObjectsWithTag(selectedTag, unitsContainer.transform));
                        }

                        if (selectByLayer)
                        {
                            isFiltered = true;
                            newSelectedObjects.AddRange(Utils.FindGameObjectsWithLayer(selectedLayer, unitsContainer.transform));
                        }

                        if (selectByName && selectedName.Length > 0)
                        {
                            isFiltered = true;
                            newSelectedObjects.AddRange(Utils.FindGameObjectsWithName(selectedName, unitsContainer.transform));
                        }

                        if (!isFiltered && newSelectedObjects.Count == 0)
                        {
                            newSelectedObjects.AddRange(Utils.FindGameObjectsInTransform(unitsContainer.transform));
                        }

                        foreach (GameObject selectedObject in newSelectedObjects)
                        {
                            if (IsWithinSelectionBounds(selectedObject))
                            {
                                parseGameObjectForSelection(selectedObject);
                            }
                        }
                    }

                    isSelecting = false;
                    
                    RaycastHit selectionHit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out selectionHit))
                    {
                        parseGameObjectForSelection(hoverHitSelection.transform.gameObject);
                    }
                }
            }

            #region Remove items not being hovered
            for (int x = 0; x < hoveredGameObjects.Count; x++)
            {
                detachHover(hoveredGameObjects[x]);
            }
            hoveredGameObjects = hoveredGameObjects.Where(i => i.isActive).ToList();
            #endregion
        }
    }

    #region Private Methods
    void detachHover(SelectableObject hoveredObject)
    {
        if (!hoveredObject.isActive)
        {
            Destroy(hoveredObject.projectorGameObject);
            if (hoveredObject.selectable)
            {
                hoveredObject.selectable.OnEndHover();
            }
        }
    }

    void detachSelection(SelectableObject selectedObject)
    {
        Destroy(selectedObject.projectorGameObject);
        if (selectedObject.selectable)
        {
            selectedObject.selectable.OnEndSelection();
        }
    }

    void parseGameObjectForHover(GameObject sender)
    {
        bool validObject = true;

        if (selectByTag && sender.tag != selectedTag)
        {
            validObject = false;
        }

        if (selectByLayer && sender.layer != selectedLayer)
        {
            validObject = false;
        }

        if (selectByName && sender.name != selectedName)
        {
            validObject = false;
        }

        if (sender.transform.parent != unitsContainer.transform)
        {
            validObject = false;
        }

        if (validObject)
        {
            bool containsObject = false;
            for (int x = 0; x < hoveredGameObjects.Count; x++)
            {
                SelectableObject hoveredObject = hoveredGameObjects[x];

                if (hoveredObject.gameObject == sender)
                {
                    containsObject = true;
                    hoveredObject.isActive = true;
                }
                hoveredGameObjects[x] = hoveredObject;
            }

            if (!containsObject)
            {
                var selectable = sender.GetComponent<SelectableUnit>();
                if (selectable && selectable != selectable.playerOwned)
                {
                    return;
                }
                
                GameObject newHoverObject = Instantiate(hoverProjector, sender.transform.position, hoverProjector.transform.rotation) as GameObject;

                SelectableObject hoveredGameObject = new SelectableObject();
                hoveredGameObject.gameObject = sender;
                hoveredGameObject.projectorGameObject = newHoverObject;
                hoveredGameObject.isActive = true;

                var projector = newHoverObject.GetComponentInChildren<Projector>();
                if (projector)
                {
                    hoveredGameObject.projector = projector;
                }
                
                if (selectable)
                {
                    hoveredGameObject.selectable = selectable;
                    hoveredGameObject.selectable.OnBeginHover();
                    if (projector)
                    {
                        hoveredGameObject.projector.orthographicSize = hoveredGameObject.selectable.selectionSize + hoveredGameObject.selectable.selectionSize / 5;
                    }
                }

                hoveredGameObjects.Add(hoveredGameObject);

                newHoverObject.transform.SetParent(sender.transform);
            }
        }
    }

    void parseGameObjectForSelection(GameObject sender)
    {
        bool validObject = true;

        if (selectByTag && sender.tag != selectedTag)
        {
            validObject = false;
        }

        if (selectByLayer && sender.layer != selectedLayer)
        {
            validObject = false;
        }

        if (selectByName && sender.name != selectedName)
        {
            validObject = false;
        }

        if (sender.transform.parent != unitsContainer.transform)
        {
            validObject = false;
        }

        if (validObject)
        {
            bool containsObject = false;
            List<SelectableObject> objectsToRemove = new List<SelectableObject>();
            for (int x = 0; x < selectedGameObjects.Count; x++)
            {
                SelectableObject selectedObject = selectedGameObjects[x];

                if (selectedObject.gameObject == sender)
                {
                    containsObject = true;
                    selectedObject.isActive = true;
                }

                selectedGameObjects[x] = selectedObject;

                if (Input.GetKey(keyDeselect) && selectedObject.gameObject == sender)
                {
                    detachSelection(selectedObject);
                    objectsToRemove.Add(selectedObject);
                }
            }
            selectedGameObjects = selectedGameObjects.Except(objectsToRemove).ToList();

            if (!containsObject && !Input.GetKey(keyDeselect))
            {
                var selectable = sender.GetComponent<SelectableUnit>();
                if (selectable && selectable != selectable.playerOwned)
                {
                    return;
                }

                GameObject newSelectionObject = Instantiate(selectionProjector, sender.transform.position, hoverProjector.transform.rotation) as GameObject;

                SelectableObject selectedGameObject = new SelectableObject();
                selectedGameObject.gameObject = sender;
                selectedGameObject.projectorGameObject = newSelectionObject;
                selectedGameObject.isActive = true;

                var projector = newSelectionObject.GetComponentInChildren<Projector>();
                if (projector)
                {
                    selectedGameObject.projector = projector;
                }

                if (selectable)
                {
                    selectedGameObject.selectable = selectable;
                    selectedGameObject.selectable.OnBeginSelection();
                    if (projector)
                    {
                        selectedGameObject.projector.orthographicSize = selectedGameObject.selectable.selectionSize;
                    }
                }

                selectedGameObjects.Add(selectedGameObject);

                newSelectionObject.transform.SetParent(sender.transform);
            }
        }
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(mousePosition, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.2f, 0.8f, 0.2f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 1, new Color(0.2f, 0.8f, 0.2f));
        }
    }

    bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting)
            return false;

        var camera = Camera.main;
        var viewportBounds = Utils.GetViewportBounds(camera, mousePosition, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }
    #endregion
    #region Public Methods

    //returns a list of currently selected objects
    public GameObject[] GetSelectedObjects()
    {
        return selectedGameObjects.Select(i => i.gameObject).ToArray();
    }

    //adds a new object to selection
    public void AddGameObjectToSelection(GameObject newObject)
    {
        parseGameObjectForSelection(newObject);
    }

    //removes an object from selection
    public void RemoveGameObjectFromSelection(GameObject removedObject)
    {
        foreach (SelectableObject selectedObject in selectedGameObjects)
        {
            if (selectedObject.gameObject == removedObject)
            {
                detachSelection(selectedObject);
                selectedGameObjects.Remove(selectedObject);
            }
        }
    }

    public void ClearSelection()
    {
        foreach (SelectableObject selectedObject in selectedGameObjects)
        {
            detachSelection(selectedObject);
        }
        selectedGameObjects.Clear();
    }

    #endregion
}

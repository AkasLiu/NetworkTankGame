using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private GameObject uiCanvasPrefab;
    public GameObject UICanvas { get; set; }
    List<ViewBase> _views = new List<ViewBase>();

    public void AddView(ViewBase view)
    {       
        _views.Add(view);
        view.Start();
    }

    public void RemoveView(ViewBase view)
    {
        _views.Remove(view);
        view.OnDestroy();
    }

    public void Start()
    {
        uiCanvasPrefab = Resources.Load("Prefabs/UI/UICanvas") as GameObject;
        UICanvas = Object.Instantiate(uiCanvasPrefab);
    }

    public void Update()
    {
        foreach (var view in _views)
            view.Update();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NewElement : ScriptableObject
{
    private static List<NewElement> _Elements = new List<NewElement>();
    public static IReadOnlyList<NewElement> Elements => _Elements;

    protected abstract bool ExistIn(StageData data);

    public bool ExistInFirstTime(StageData data){
        if(found) return false;
        found = ExistIn(data);
        PlayfabAccesssor.Instance.RequestSendData(
            elementName + "NewElement",
            found
        );
        return found;
    }
    [System.NonSerialized] bool found = false;
    public void SetFound() => found = true;

    public void Init(){
        var dlg = GameObject.Instantiate(_Dialog);
        dlg.dialogClosed += (s, e) => dialogClosed?.Invoke(this, EventArgs.Empty);
        dlg.Init();
    }

    public event EventHandler dialogClosed;

    [SerializeField] DialogManager _Dialog;
    [SerializeField] string elementName;
    public string ElementName => elementName;

    public NewElement(){
        _Elements.Add(this);
    }
}

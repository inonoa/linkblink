using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using DG.Tweening;
using Sirenix.OdinInspector;


public class NodeMover : MonoBehaviour, IVanish
{
    NodeColor[] _Colors;
    public NodeColor[] Colors{
        get{
            if(_Colors == null) _Colors = Type.ToColors();
            return _Colors;
        }
    }

    [SerializeField] NodeType _Type;
    public NodeType Type => _Type;

    enum EState{
        Default, MouseOn, Selected, Vanishing
    }
    EState _State = EState.Default;
    EState State{
        get => _State;
        set{
            //print(value);
            _State = value;
        }
    }

    public bool CanBeSelected => State == EState.Default || State == EState.MouseOn;

    public event EventHandler Clicked;
    public event EventHandler ClickedSecondTime;
    public event EventHandler MouseOn;
    public event EventHandler MouseOut;

    [SerializeField] MouseSensor _Sensor;
    public MouseSensor Sensor => _Sensor;
    [SerializeField] BeamTarget _BeamTarget;
    public BeamTarget BeamTarget => _BeamTarget;
    [SerializeField] NodeSoundGroup soundGroup;

    INodeLight nodeLight;

    Bomb bomb;
    Func<NodeMover[]> nodesGetter = () => new NodeMover[0]{};

    public void Init(Func<NodeMover[]> nodesGetter){
        this.nodesGetter = nodesGetter;
    }

    void Start()
    {
        nodeLight = GetComponent<INodeLight>();
        bomb = GetComponentInChildren<Bomb>();
        DOVirtual.DelayedCall(
            UnityEngine.Random.Range(0, 0.3f),
            () => soundGroup.OnAwakeSound.Play(UnityEngine.Random.Range(0.2f, 0.6f))
        );
        Sensor.OnMouseOn += OnMouseOn;
        Sensor.OnMouseOut += OnMouseOut;
    }

    void OnMouseOn(object sender, MouseEventArgs args){
        //print("Mouse On!");
        MouseOn?.Invoke(this, EventArgs.Empty);
        
        if(State == EState.Default){
            State = EState.MouseOn;

            nodeLight.Light();
            if(Type != NodeType.Black) soundGroup.OnMouseOnSound.Play();
        }
    }

    void OnMouseOut(object sender, MouseEventArgs args){
        MouseOut?.Invoke(this, EventArgs.Empty);

        if(State == EState.MouseOn){
            State = EState.Default;

            nodeLight.UnLight();
        }
    }

    public void OnSelected(int num_0_idx){
        if(State == EState.MouseOn){
            State = EState.Selected;
            soundGroup.OnSelectedSounds[Mathf.Min(num_0_idx, soundGroup.OnSelectedSounds.Count - 1)].Play();
        }
    }

    public void UnSelect(){
        State = EState.Default;

        nodeLight.UnLight();
    }

    [Button]
    public void Vanish(bool isLast){
        if(State == EState.Vanishing) return;

        State = EState.Vanishing;
        nodeLight.Vanish();
        DOVirtual.DelayedCall(
            UnityEngine.Random.Range(0, 0.3f),
            () => (isLast ? soundGroup.OnVanishLastSound : soundGroup.OnVanishSound).Play(UnityEngine.Random.Range(0.2f, 1))
        );
        bomb?.Explode(nodesGetter());
    }
    public void Vanish() => Vanish(false);

    public event EventHandler DiedSelf;
    public void DieSelf(){
        Vanish();
        DiedSelf?.Invoke(this, EventArgs.Empty);
    }

    void Update(){

        switch(State){

            case EState.Default: {
                break;
            }
            case EState.MouseOn: {

                if(Sensor.IsTouched && Input.GetMouseButtonDown(0)){
                    Clicked?.Invoke(this, EventArgs.Empty);
                }
                break;
            }
            case EState.Selected: {
                if(Sensor.IsTouched && Input.GetMouseButtonDown(0)){
                    ClickedSecondTime?.Invoke(this, EventArgs.Empty);
                }
                break;
            }
            case EState.Vanishing: {
                break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using DG.Tweening;


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
    [SerializeField] SoundAndVolume onMouseOnSound;
    [SerializeField] SoundAndVolume[] onSelectedSounds;
    [SerializeField] SoundAndVolume onVanishLastSound;
    [SerializeField] SoundAndVolume onVanishSound;
    [SerializeField] SoundAndVolume onAwakeSound;

    NodeLight nodeLight;

    void Start()
    {
        nodeLight = GetComponent<NodeLight>();
        DOVirtual.DelayedCall(
            UnityEngine.Random.Range(0, 0.3f),
            () => onAwakeSound.Play(UnityEngine.Random.Range(0.2f, 0.6f))
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
            if(Type != NodeType.Black) onMouseOnSound.Play();
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
            onSelectedSounds[Mathf.Min(num_0_idx, onSelectedSounds.Length - 1)].Play();
        }
    }

    public void UnSelect(){
        State = EState.Default;

        nodeLight.UnLight();
    }

    public void Vanish(bool isLast){
        State = EState.Vanishing;
        nodeLight.Vanish();
        DOVirtual.DelayedCall(
            UnityEngine.Random.Range(0, 0.3f),
            () => (isLast ? onVanishLastSound : onVanishSound).Play(UnityEngine.Random.Range(0.2f, 1))
        );
    }
    public void Vanish() => Vanish(false);

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

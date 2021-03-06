﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    TouchableByMouse shutter;
    [SerializeField] GameObject shutterObj;

    Link link = new Link();

    NodeMover nodeMouseOn;
    [SerializeField] bool _BeamReachesNode = false;
    bool BeamReachesNode{
        get => _BeamReachesNode;
        set{
            if(value != _BeamReachesNode){
                //print("BeamReachesNode -> " + value);
                _BeamReachesNode = value;
            }
        }
    }

    [SerializeField] Beam beamPrefab;
    List<NodeMover> nodes;


    public event EventHandler AllNodeVanished;

    [SerializeField] PointEffectMover nodePointPrefab;
    [SerializeField] PointEffectMover beamPointPrefab;
    public ScoreManager ScoreManager{ get; set; }
    [SerializeField] SoundAndVolume lineDeletedSound;

    void Start()
    {
        nodes = new List<NodeMover>(GetComponentsInChildren<NodeMover>());

        foreach(NodeMover node in nodes){
            node.Clicked += OnNodeClicked;
            node.MouseOn += (s, e) => OnMouseOnNode((NodeMover)s);
            node.MouseOut += (s, e) => nodeMouseOn = null;
            node.ClickedSecondTime += (s, e) => OnNodeClickedSecondTime((NodeMover)s);
            node.Init(() => nodes.ToArray());
            node.DiedSelf += (nd, __) => {
                if(nodes.Contains(nd as NodeMover)) nodes.Remove(nd as NodeMover);
                CheckEnd();
            };
        }

        shutter = shutterObj.GetComponent<TouchableByMouse>();
    }

    void OnMouseOnNode(NodeMover node){
        nodeMouseOn = node;

        if(link.Count > 0 && DebugParameters.Instance.LinkTrigger == LinkTriggerType.MouseOver){
            StartCoroutine(TrySelectDelayed(node));
        }

        IEnumerator TrySelectDelayed(NodeMover nd){
            yield return null;

            if(CanSelectNode(nd)){
                Select(nd);
            }else if(CanSelectSecondTime(nd)){
                SelectSecondTime(nd);
            }
        }
    }

    void OnNodeClicked(object sender, EventArgs e){
        NodeMover node = (NodeMover)sender;

        bool WillSelectNode(){
            if(link.Count == 0) return true;
            return DebugParameters.Instance.LinkTrigger == LinkTriggerType.Click;
        }

        if(WillSelectNode() && CanSelectNode(node)){
            Select(node);
        }
    }

    void Select(NodeMover node){
        link.Add(node, CreateBeam(node));
        node.OnSelected(link.Count - 1);
    }

    bool CanSelectNode(NodeMover node){
        if(node.Type == NodeType.Black) return false;
        if(link.Count == 0) return true;
        if(!BeamReachesNode){
            return false;
        }
        if(!node.CanBeSelected) return false;

        return link.HasSameColor(node.Colors);
    }

    Beam CreateBeam(NodeMover node){
        Beam beam = Instantiate(beamPrefab);
        beam.SetColor(node.Type);
        beam.LineRenderer.SetPositions(new Vector3[]{
            node.transform.position,
            node.transform.position
        });
        return beam;
    }

    void OnNodeClickedSecondTime(NodeMover node){
        if(DebugParameters.Instance.LinkTrigger != LinkTriggerType.Click) return;

        if(CanSelectSecondTime(node)) SelectSecondTime(node);
    }

    bool CanSelectSecondTime(NodeMover node){
        return (link.Count > 2) && (node == link.Nodes.First()) && BeamReachesNode;
    }

    void SelectSecondTime(NodeMover node){
        Vector3[] nodePositions = link.Nodes.Select(nd => nd.transform.position).ToArray();
        var point = ScoreManager.LinkToScore(nodePositions);

        ScoreManager.AddScore(point.TotalPoint());

        point.nodePoints.ForEach(pos_point => {
            PointEffectMover pointEffect = Instantiate(nodePointPrefab);
            pointEffect.transform.position = pos_point.Item1;
            pointEffect.Init(pos_point.Item2, link.Colors.First());
        });
        point.beamPoints.ForEach((i, pos_point) => {
            PointEffectMover pointEffect = Instantiate(beamPointPrefab);
            pointEffect.transform.position =
                pos_point.Item1 + new Vector3(i * 0.03f, i * 0.1f, 0);
            pointEffect.Init(pos_point.Item2, link.Colors.First());
        });

        foreach(NodeMover linkedNode in link.Nodes){
            //黒の存在を無視しているがまあ黒の方でlastは鳴るのでこれでいいかな……
            //これ100個ぐらい同時に消えても最後の一個しか最後に消えたことにならない？
            RemoveNode(linkedNode, link.Count >= nodes.Count);
        }
        link.OnClear();
        link = new Link();

        CheckEnd();
    }

    void CheckEnd(){
        if(nodes.All(nd => nd.Type == NodeType.Black)){
            var blacks = nodes.Where(nd => nd.Type == NodeType.Black).ToArray();
            foreach(NodeMover black in blacks){
                RemoveNode(black, true);
            }
        }
        if(nodes.Count == 0) AllNodeVanished?.Invoke(this, EventArgs.Empty);
    }

    void RemoveNode(NodeMover node, bool isLast){
        nodes.Remove(node);
        node.Vanish(isLast);
    }

    public void CancelSelect(){
        if(link.Count > 0) lineDeletedSound.Play(1);
        foreach(NodeMover node in link.Nodes){
            node.UnSelect();
        }
        link.OnClear();
        link = new Link();
    }

    void Update()
    {

        if(link.Count > 0){
            (Vector3 target, bool canReachNode) = CalcBeamTarget();
            if(target != posInvalid){
                link.CurrentBeam.LineRenderer.SetPosition(1, target);
                this.BeamReachesNode = canReachNode;
            }else{
                this.BeamReachesNode = false;
            }
        }
    }

    static readonly Vector3 posInvalid = new Vector3(99, 99, 99);

    (Vector3 target, bool canReachNode) CalcBeamTarget(){
        Beam currentBeam = link.CurrentBeam;
        Vector3 origin = currentBeam.LineRenderer.GetPosition(0);
        var target = ClickRayCaster.Instance.HitFirst;

        if(target != null){
            RayHitInfo hit;
            if(target.Hit is Shutter) hit = RayCastUtil.RayHit(origin, target.hitPos,                 LayerMask.GetMask("Default", "Shutter"));
            else                      hit = RayCastUtil.RayHit(origin, target.Hit.transform.position, LayerMask.GetMask("Default", "Shutter"));
            if(hit == null) return (posInvalid, false);
            if((nodeMouseOn != null) && (hit.Hit == nodeMouseOn.BeamTarget as TouchableByMouse)) return (nodeMouseOn.transform.position, true);
            return (hit.hitPos, false);
        }

        return (posInvalid, false);
    }

    public void KillAll(){
        link.OnClear();
        Destroy(gameObject);
    }
}

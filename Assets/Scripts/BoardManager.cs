using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    ITouchableByMouse shutter;
    [SerializeField] GameObject shutterObj;

    List<NodeMover> selectedNodes = new List<NodeMover>();
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
    List<Beam> beams = new List<Beam>();
    List<NodeMover> nodes;

    HashSet<NodeColor> colors = new HashSet<NodeColor>();

    public event EventHandler AllNodeVanished;

    public Vector2 NodeDistanceUnit{ get; set; }
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
        }

        shutter = shutterObj.GetComponent<ITouchableByMouse>();
    }

    void OnMouseOnNode(NodeMover node){
        nodeMouseOn = node;

        if(selectedNodes.Count > 0 && DebugParameters.Instance.LinkTrigger == LinkTriggerType.MouseOver){
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
            if(selectedNodes.Count == 0) return true;
            return DebugParameters.Instance.LinkTrigger == LinkTriggerType.Click;
        }

        if(WillSelectNode() && CanSelectNode(node)){
            Select(node);
        }
    }

    void Select(NodeMover node){
        CreateBeam(node);
        selectedNodes.Add(node);
        if(selectedNodes.Count==1){
            colors = new HashSet<NodeColor>(node.Colors);
        }else{
            int lastColors = colors.Count;
            colors.IntersectWith(node.Colors);
            if((lastColors > 1 || node.Colors.Count() > 1) && colors.Count == 1){
                foreach(Beam bm in beams){
                    bm.SetColor(colors.ElementAt(0).ToType());
                }
            }
        }
        node.OnSelected(selectedNodes.Count - 1);
    }

    bool CanSelectNode(NodeMover node){
        if(node.Type == NodeType.Black) return false;
        if(selectedNodes.Count == 0) return true;
        if(!BeamReachesNode){
            return false;
        }
        if(!node.CanBeSelected) return false;

        return colors.Intersect(node.Colors).Count() > 0;
    }

    void CreateBeam(NodeMover node){
        Beam beam = Instantiate(beamPrefab);
        beam.SetColor(node.Type);
        beam.LineRenderer.SetPositions(new Vector3[]{
            node.transform.position,
            node.transform.position
        });
        beams.Add(beam);
    }

    void OnNodeClickedSecondTime(NodeMover node){
        if(DebugParameters.Instance.LinkTrigger != LinkTriggerType.Click) return;

        if(CanSelectSecondTime(node)) SelectSecondTime(node);
    }

    bool CanSelectSecondTime(NodeMover node){
        return (selectedNodes.Count > 2) && (node == selectedNodes.First()) && BeamReachesNode;
    }

    void SelectSecondTime(NodeMover node){
        Vector3[] nodePositions = selectedNodes.Select(nd => nd.transform.position).ToArray();
        ScoreManager.LinkToScore(nodePositions, NodeDistanceUnit);

        foreach(NodeMover linkedNode in selectedNodes){
            //黒の存在を無視しているがまあ黒の方でlastは鳴るのでこれでいいかな……
            RemoveNode(linkedNode, selectedNodes.Count >= nodes.Count);
        }
        selectedNodes.Clear();
        colors.Clear();
        foreach(Beam beam in beams){
            beam.Vanish();
        }
        beams.Clear();

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

    void Update()
    {

        if(Input.GetMouseButtonDown(1)){
            if(selectedNodes.Count > 0) lineDeletedSound.Play(1);
            foreach(NodeMover node in selectedNodes){
                node.UnSelect();
                if(node is RainbowNodeMover rainbow){
                    rainbow.UnSelectColor();
                }else if(node is AllColorNodeMover allcol){
                    allcol.UnSelectColor();
                }
            }
            selectedNodes.Clear();
            colors.Clear();
            foreach(Beam beam in beams){
                beam.Vanish();
            }
            beams.Clear();
        }

        if(beams.Count > 0){
            (Vector3 target, bool canReachNode) = CalcBeamTarget();
            if(target != posInvalid){
                beams.Last().LineRenderer.SetPosition(1, target);
                this.BeamReachesNode = canReachNode;
            }else{
                this.BeamReachesNode = false;
            }
        }
    }

    static readonly Vector3 posInvalid = new Vector3(99, 99, 99);

    (Vector3 target, bool canReachNode) CalcBeamTarget(){
        Beam currentBeam = beams.Last();
        Vector3 origin = currentBeam.LineRenderer.GetPosition(0);
        var target = ClickRayCaster.Instance.HitFirst;

        if(target != null){
            RayHitInfo hit = RayCastUtil.RayHit(origin, target.hitPos);
            if(hit == null) return (posInvalid, false);
            if((nodeMouseOn != null) && (hit.Hit == nodeMouseOn.Sensor as ITouchableByMouse)) return (nodeMouseOn.transform.position, true);
            return (hit.hitPos, false);
        }

        return (posInvalid, false);
    }

    public void KillAll(){
        beams.ForEach(bm => bm.Vanish());
        Destroy(gameObject);
    }
}

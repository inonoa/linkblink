using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class StageWatcher : MonoBehaviour
{
    //なんかおかしくね？
    [SerializeField] ScoreManager scoreManager;

    Sequence currentSequence;
    public int CurrentStageIndex{ get; private set; }
    Stage _CurrentStage;
    Stage CurrentStage{
        get{
            if(PlayMode == PlayMode.Sequence) return currentSequence.Stages[CurrentStageIndex];
            else                              return _CurrentStage;
        }
    }

    [SerializeField] SequenceResultManager sequenceResultManager;
    [SerializeField] StageResultManager stageResultManager;
    [SerializeField] StageUIsHolder stageUIsHolder;
    [SerializeField] Button resetButton;
    [SerializeField] Button cancelButton;
    BoardManager currentBoard;
    [SerializeField] BoardManager boardPrefab;
    [SerializeField] GameObject rowPrefab;
    public PlayMode PlayMode{ get; private set; } = PlayMode.Sequence;

    bool _AcceptsInput = true;
    bool AcceptsInput{
        get => _AcceptsInput;
        set{
            (resetButton.interactable, cancelButton.interactable, _AcceptsInput) = (value, value, value);
        }
    }

    void Start(){
        resetButton.onClick.AddListener(() => {
            ResetStage();
        });
        cancelButton.onClick.AddListener(() => {
            currentBoard.CancelSelect();
        });
    }

    void Update(){
        if(AcceptsInput && Input.GetKeyDown(KeyCode.Z)){
            ResetStage();
        }
        //ここゲーム外でも発生して例外吐く
        if(AcceptsInput && Input.GetMouseButtonDown(1)){
            currentBoard.CancelSelect();
        }
    }

    BoardManager LoadBoard(StageData stage){

        BoardManager board = Instantiate(boardPrefab);
        if(stage.DistanceUnit == Vector2.zero){
            board.NodeDistanceUnit = new Vector2(
                6.0f / (stage.Rows[0].Nodes.Count - 1),
                6.0f / (stage.Rows.Count - 1)
            );
        }else{
            board.NodeDistanceUnit = stage.DistanceUnit;
        }
        board.ScoreManager = scoreManager;
        for(int i = 0; i < stage.Rows.Count; i++){
            Transform row = Instantiate(rowPrefab, board.transform).transform;

            
            row.position += new Vector3(0, (stage.Rows.Count / 2f - 0.5f - i) * board.NodeDistanceUnit.y, 0);

            for(int j = 0; j < stage.Rows[i].Nodes.Count; j++){
                
                NodeType type = stage.Rows[i].Nodes[j];
                if(type != NodeType.None){
                    NodeMover node = Instantiate(TypeDataHolder.Instance[type].Prefab, row);
                    node.transform.position += new Vector3((-stage.Rows[i].Nodes.Count / 2f + 0.5f + j) * board.NodeDistanceUnit.x, 0, 0);
                }
            }
        }

        CheckNewElements(stage);

        return board;
    }

    void CheckNewElements(StageData stage){

        List<NewElement> elements = new List<NewElement>();
        foreach(NewElement newelm in NewElement.Elements){
            if(newelm.ExistInFirstTime(stage)){
                elements.Add(newelm);
            }
        }
        if(elements.Count > 0) elements[0].Init();
        if(elements.Count > 1){
            for(int i = 0; i < elements.Count - 1; i++){
                NewElement tmp = elements[i + 1];
                elements[i].dialogClosed += (s, e) => tmp.Init();
            }
        }
    }

    public void Init(Sequence seq){
        PlayMode = PlayMode.Sequence;
        currentSequence = seq;
        CurrentStageIndex = 0;
        StartStage(seq.Stages[DebugParameters.Instance.StartStage]);
    }

    public void Init(Stage stage){
        PlayMode = PlayMode.SingleStage;
        _CurrentStage = stage;
        StartStage(stage);
    }

    void StartStage(Stage stage){
        AcceptsInput = true;

        currentBoard = LoadBoard(stage.data);
        currentBoard.AllNodeVanished += (s, e) => {
            AcceptsInput = false;
            LoadNextStage();
        };
        scoreManager.ResetCurrentScore();
    }

    void ResetStage(){
        currentBoard.KillAll();
        StartStage(CurrentStage);
    }

    void LoadNextStage(){
        StartCoroutine(LoadNextStageDelayed());

        IEnumerator LoadNextStageDelayed(){
            yield return new WaitForSeconds(2);

            Destroy(currentBoard.gameObject);

            if(PlayMode == PlayMode.Sequence){
                currentSequence.Scores.RegisterScore(CurrentStageIndex, scoreManager.Score);
                if(CurrentStageIndex == currentSequence.Data.Stages.Count - 1){
                    FinishSequence();
                    yield break;
                }
                CurrentStageIndex ++;

            }else if(PlayMode == PlayMode.SingleStage){
                CurrentStage.scoreHolder.score = scoreManager.Score;
                FinishSingleStage();
                yield break;
            }

            StartStage(CurrentStage);
        }
    }

    void FinishSequence(){
        stageUIsHolder.ForEach(group => {
            group.DOFade(0, 1)
            .onComplete += () => group.gameObject.SetActive(false);
        });
        sequenceResultManager.Init(currentSequence);
    }

    void FinishSingleStage(){
        stageUIsHolder.ForEach(group => {
            group.DOFade(0, 1)
            .onComplete += () => group.gameObject.SetActive(false);
        });

        stageResultManager.Init(CurrentStage);
    }
}

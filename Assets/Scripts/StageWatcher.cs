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
    
    StageLoder stageLoder;

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
    public PlayMode PlayMode{ get; private set; } = PlayMode.Sequence;

    [SerializeField] SkyMover skyMover;

    bool _AcceptsInput = false;
    bool AcceptsInput{
        get => _AcceptsInput;
        set{
            (resetButton.interactable, cancelButton.interactable, _AcceptsInput) = (value, value, value);
        }
    }


    void Start(){
        stageLoder = GetComponent<StageLoder>();
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
        stageUIsHolder.ForEach(group => {
            group.gameObject.SetActive(true);
            group.DOFade(1, 0.5f);
        });
        StartStage(seq.Stages[DebugParameters.Instance.StartStage]);
    }

    public void Init(Stage stage){
        PlayMode = PlayMode.SingleStage;
        _CurrentStage = stage;
        stageUIsHolder.ForEach(group => {
            group.gameObject.SetActive(true);
            group.DOFade(1, 0.5f);
        });
        StartStage(stage);
    }

    void StartStage(Stage stage){
        AcceptsInput = true;

        currentBoard = stageLoder.LoadBoard(stage.data);
        currentBoard.ScoreManager = scoreManager;
        CheckNewElements(stage.data);
        currentBoard.AllNodeVanished += (s, e) => {
            skyMover.Light();
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

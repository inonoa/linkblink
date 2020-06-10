using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankDataInjector : MonoBehaviour
{
    [SerializeField] Text rankText;
    [SerializeField] Text nameText;
    [SerializeField] Text colonText;
    [SerializeField] Text scoreText;

    [SerializeField] Color firstColor = new Color(0.7f, 0.8f, 0, 1);
    [SerializeField] Color highRankerColor = new Color(0, 0.8f, 0.6f, 1);
    [SerializeField] int highRankRange = 5;

    public void Init(int rank_1_index, RankData data){

        rankText.text = rank_1_index.ToString();
        nameText.text = data.name;
        scoreText.text =  data.score.ToString();

        if(rank_1_index == 1){
            rankText.color = firstColor;
            nameText.color = firstColor;
            colonText.color = firstColor;
            scoreText.color = firstColor;

        }else if(rank_1_index <= highRankRange){
            rankText.color = highRankerColor;
            nameText.color = highRankerColor;
            colonText.color =  highRankerColor;
            scoreText.color = highRankerColor;
        }
    }
}

public class RankData : IComparable{
    public string name;
    public int score;
    public RankData(string name, int score){
        (this.name, this.score) = (name, score);
    }

    public int CompareTo(object data2){
        
        if(data2 is RankData dt2){
            if(this.score == dt2.score){
                if(this.name.CompareTo(dt2.name) == 0) return this.GetHashCode().CompareTo(data2.GetHashCode()); 
                return this.name.CompareTo(dt2.name);
            }
            return this.score.CompareTo(dt2.score);
        }else{
            Debug.LogError("data2 is not RankData");
            throw new Exception();
        }
    }
}

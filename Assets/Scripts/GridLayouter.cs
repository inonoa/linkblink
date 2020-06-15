using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

public class GridLayouter : MonoBehaviour
{
    [SerializeField] Vector2 space = new Vector2(1, 1);
    [SerializeField] int widthConstraint = 3;

    public ObservableCollection<Transform> Children{ get; private set; }
        = new ObservableCollection<Transform>();
    bool dirty = true;

    void Start()
    {
        Children.CollectionChanged += (_, __) => dirty = true;
    }

    void LateUpdate(){
        if(dirty){
            SetChildrenPositions();
        }
        dirty = false;
    }

    void SetChildrenPositions(){
        int cnt = 0;
        
        int height = (Children.Count - 1) / widthConstraint + 1;
        for(int i = 0; i < height; i++){
            int width = Mathf.Min(widthConstraint, Children.Count - widthConstraint * i);

            for(int j = 0; j < width; j++){
                Children[cnt].SetParent(transform);
                Children[cnt].localPosition = space * new Vector2(j, -i);

                cnt ++;
            }
        }
    }
}

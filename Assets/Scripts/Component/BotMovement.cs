using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAI.PathFinding;
using EZ_Pooling;

public class BotMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private readonly string MOVE_UP_ANIMATION = "MoveUp";
    private readonly string MOVE_DOWN_ANIMATION = "MoveDown";
    private readonly string MOVE_LEFT_ANIMATION = "MoveLeft";
    private readonly string MOVE_RIGHT_ANIMATION = "MoveRight";

    public void Move(List<Transform> path)
    {
        var speed = BotController.Instance.Speed;
        
        StartCoroutine(MoveIEnum());
        
        IEnumerator MoveIEnum()
        {
            transform.position = path[0].position;
            
            int index = 0;
            Vector3 target = path[index].position;

            while (index < path.Count)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (transform.position == target)
                {
                    index++;

                    if (index == path.Count)
                    {
                        break;
                    }
                    target = path[index].position;

                    Vector3 dir = (target - transform.position).normalized;
                    ChangAnimation(dir);
                }
                yield return null;
            }
            
            // Finish path
            OnFinishPath();
        }
    }

    private void ChangAnimation(Vector3 dir)
    {
        string animationName = "";
        
        if (dir.x > 0 && dir.y <= 0)
        {
            animationName = MOVE_RIGHT_ANIMATION;
        }
        else if(dir.x < 0 && dir.y <= 0)
        {
            animationName = MOVE_LEFT_ANIMATION;
        }
        else if(dir.x <= 0 && dir.y > 0)
        {
            animationName = MOVE_UP_ANIMATION;
        }
        else if(dir.x >= 0 && dir.y < 0)
        {
            animationName = MOVE_DOWN_ANIMATION;
        }
        
        animator.CrossFade(animationName, 0.05f);
    }

    private void OnFinishPath()
    {
        EZ_PoolManager.Despawn(transform);
    }
    
    void OnDespawned()
    {
        //this method will be called when an object is despawned by the pool manager
        // print("Done");
    }
}
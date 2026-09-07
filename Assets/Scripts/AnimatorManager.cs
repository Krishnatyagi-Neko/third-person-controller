using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;
    int vertical;
    int horizontal;


    public void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation, 0.2f);
    }   

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {

        float snappedHorizontal;
        float snappedVertical;


        #region  snapped Horizontal
        if(horizontalMovement>0 && horizontalMovement < 0.55f)
        {
            snappedHorizontal = 0.5f;
        }
        else if(horizontalMovement > 0.55f)
        {
            snappedHorizontal = 1;
        }
        else if(horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if(horizontalMovement < -0.55f)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }
        #endregion



        #region  snapped Vertical
        if(verticalMovement>0 && verticalMovement < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if(verticalMovement > 0.55f)
        {
            snappedVertical = 1;
        }
        else if(verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if(verticalMovement < -0.55f)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        if(isSprinting)
        {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2;
        }

       animator.SetFloat(horizontal, snappedHorizontal, .1f , Time.deltaTime);
       animator.SetFloat(vertical, snappedVertical , .1f , Time.deltaTime);
    }


}
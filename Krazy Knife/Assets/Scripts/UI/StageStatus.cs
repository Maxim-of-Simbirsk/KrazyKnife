using UnityEngine;
using UnityEngine.UI;

public class StageStatus : MonoBehaviour
{
    public Image[] stage = new Image[5];
    public Text text;
    private bool boss = false;
    private int localStage = 0;  
    public void SetStage(int Stage)
    {
        
        if (Stage == 1)
        {
            localStage = 0;
            foreach (var item in stage)
                item.color = Color.white;
        }

        if (boss)
        {
            boss = false;           
            text.color = Color.white;
            foreach (var item in stage)
                item.color = Color.white;
        }  
        if (Stage % 5 == 0)
        {
            boss = true;
            localStage = 0;
            text.text = "BOSS";
            text.color = Color.red;
            foreach (var item in stage)
                item.color = Color.red;
        }
        else
        {
            stage[localStage].color = Color.yellow;
            text.text = "STAGE " + Stage;
            localStage += 1;
        }
    }
}

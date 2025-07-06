using UnityEngine;

public class CheckCapturedMemories : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerMemoryController.capturedMemory.Count <= 2)
            {
                InGameInformationSystem.Show(
                    "",
                    "Rodzice jeszcze nie zasneli - napewno coś ciekawego można jeszcze zrobić"
                );
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}

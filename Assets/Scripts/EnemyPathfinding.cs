using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    public float moveSpeed = 3f;
    
    // Düşmanın sırasıyla takip edeceği dünya koordinatları
    private List<Vector3> waypointList;
    private int currentWaypointIndex;

    public void Setup(List<Vector3> waypointList)
    {
        this.waypointList = waypointList;
        currentWaypointIndex = 0;
        
        // Düşmanı ilk noktanın (spawn noktasının) üzerine koy
        transform.position = waypointList[currentWaypointIndex];
    }

    private void Update()
    {
        // Eğer gidecek bir yolumuz varsa
        if (waypointList != null && currentWaypointIndex < waypointList.Count)
        {
            Vector3 targetPosition = waypointList[currentWaypointIndex];

            // Düşmanı hedefe doğru sabit hızla ilerlet
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Hedefe çok yaklaştıysak (vardıysak), bir sonraki noktaya geç
            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            // Son noktaya ulaşıldı (Oyuncu can kaybeder, düşman yok olur vs.)
            Debug.Log("Düşman üsse ulaştı!");
            Destroy(gameObject);
        }
    }
}
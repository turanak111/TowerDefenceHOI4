using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    public float moveSpeed = 3f;
    
    // Düşmanın sırasıyla takip edeceği dünya koordinatları
    private List<Vector3> waypointList;
    private int currentWaypointIndex;

    // YENİ: Düşmanın yoldaki kendi özel şeridi
    private Vector3 pathOffset;

    // Setup fonksiyonunu moveSpeed alacak şekilde güncelliyoruz
    public void Setup(List<Vector3> waypointList, float speed)
    {
        this.waypointList = waypointList;
        this.moveSpeed = speed;
        this.currentWaypointIndex = 0;

        // YENİ: Düşman doğduğunda ona özel rastgele bir sapma (offset) hesaplıyoruz.
        // Random.insideUnitCircle bize 1 birimlik daire içinde rastgele x,y verir.
        // Bunu 2.5f ile çarparak hücre (10 birim) içinde güvenli bir yayılma sağlıyoruz.
        float offsetRadius = 2f; 
        Vector2 randomCircle = Random.insideUnitCircle * offsetRadius;
        pathOffset = new Vector3(randomCircle.x, randomCircle.y, 0f);

        // Başlangıç pozisyonuna bu sapmayı ekleyerek doğuruyoruz
        transform.position = waypointList[currentWaypointIndex] + pathOffset;
    }

    private void Update()
    {
        // Eğer gidecek bir yolumuz varsa
        if (waypointList != null && currentWaypointIndex < waypointList.Count)
        {
            // YENİ: Hedef noktaya direkt gitmek yerine, hedef noktanın kendi offset'imiz kadar yanına gidiyoruz
            Vector3 targetPosition = waypointList[currentWaypointIndex] + pathOffset;

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
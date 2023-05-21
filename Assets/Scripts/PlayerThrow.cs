using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    public GameObject rock;
    public float throwSpeed = 0.01f;
    public float throwDistance = 100f;
    public GameObject player;
    const int throwLimit = 100;
    int currentRockCount;

    void Start()
    {
        currentRockCount = throwLimit;    
    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && currentRockCount > 0)
        {
            rock.SetActive(true);
            Vector3 throwPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
            throwPos.z = -9f; // Bunu yapmazsan gorunmez oluyo

            
            float distance = Vector2.Distance(player.transform.position, throwPos);
            if (distance > throwDistance)
            {
                // Burada distance, max distance limitini gecerse calisacask yani throwPos u burada limitlemen gerekiyor.
            }

            StartCoroutine(throwRock(throwPos));
        }
    }

    IEnumerator throwRock(Vector3 throwPos) 
    {
        currentRockCount--;
        GameObject newObject = Instantiate(rock, transform.parent);
        
        float elapsedTime = 0;
        float waitTime = 3f;
        newObject.transform.position = player.transform.position;
        while (elapsedTime < waitTime)
        {
            newObject.transform.position = Vector3.Lerp(newObject.transform.position, throwPos, throwSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
 
        //Emrecan bu kodtan sonra tas yere dusuyor burada ne yapmak istedigini yaparsin.
        yield return new WaitForSeconds(5);
        Destroy(newObject);
        yield return null;
    }
}

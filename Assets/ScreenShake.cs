using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public GameObject objectToShake;
    [SerializeField] float shakeDuration;
    [SerializeField] Vector3 maxShake;
    Vector3 startPos;
    float dieTime = 0.0f;
    Coroutine shake;

    private void Start()
    {
        startPos = transform.position;
    }

    public void Shake()
    {
        transform.position = startPos;
        dieTime = Time.time + shakeDuration;
    }

    IEnumerator ShakeObject()
    {
        do
        {
            objectToShake.transform.position += new Vector3(Random.Range(-maxShake.x, maxShake.x), Random.Range(-maxShake.y, maxShake.y), Random.Range(-maxShake.z, maxShake.z));
        } while (Time.time < dieTime);
        transform.position = startPos;

        yield return null;
    }
}

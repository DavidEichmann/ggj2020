using System.Collections;
using UnityEngine;

public class HingeBump : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine("Bump");
    }

    private IEnumerator Bump()
    {
        yield return new WaitForSeconds(Random.value);
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.value, 0, Random.value) * 2, ForceMode.Impulse);
    }
}

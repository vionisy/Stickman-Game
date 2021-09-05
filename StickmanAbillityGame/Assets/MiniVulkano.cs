using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniVulkano : MonoBehaviour
{
    public GameObject bounce;
    public ParticleSystem Eruption;
    public ParticleSystem BeforeEruption;
    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        bounce.SetActive(false);
        StartCoroutine("Vulcano");
    }

    private IEnumerator Vulcano()
    {
        yield return new WaitForSeconds(Random.Range(3, 6));
        photonView.RPC("VulcanoAction", PhotonTargets.All, 1f);
        yield return new WaitForSeconds(Random.Range(0.5f, 2));
        photonView.RPC("VulcanoAction", PhotonTargets.All, 2f);
        yield return new WaitForSeconds(Random.Range(3, 5));
        photonView.RPC("VulcanoAction", PhotonTargets.All, 3f);
        StartCoroutine("Vulcano");
    }
    [PunRPC]
    public void VulcanoAction(float action)
    {
        if (action == 1)
        {
            BeforeEruption.Play();
        }
        else if (action == 2)
        {
            BeforeEruption.Stop();
            Eruption.Play();
            bounce.SetActive(true);
        }
        else if (action == 3)
        {
            Eruption.Stop();
            bounce.SetActive(false);
        }
    }
}
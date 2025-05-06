using System.Collections;
using UnityEngine;

public class Bin : MonoBehaviour
{
    [SerializeField] private ParticleSystem dirtBlast;
    private AudioSource _addTrash;

    private void Awake()
    {
        _addTrash = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            var pos = other.transform.position;
            Destroy(other.gameObject);
            var blast = Instantiate(dirtBlast, pos, Quaternion.identity);
            PlayAddTrash();
        }
    }
    
    private void PlayAddTrash()
    {
        _addTrash.pitch = Random.Range(0.7f, 1.1f);
        _addTrash.PlayOneShot(_addTrash.clip);
    }
    
}

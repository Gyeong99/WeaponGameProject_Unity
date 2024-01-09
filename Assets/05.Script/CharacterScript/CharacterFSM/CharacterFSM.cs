using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterFSM : MonoBehaviour
{
    [SerializeField]
    private GameObject characterPrefab;
    [SerializeField]
    private GameObject characterCameraPrefab;
    [SerializeField]
    private Transform spawnPoint;

    private Character characterEntity;

    private void Awake()
    {
        GameObject clone = Instantiate(characterPrefab);
        characterEntity = clone.GetComponent<Character>();
        characterEntity.SetUp("Player");
        characterEntity.transform.position = spawnPoint.position;
        spawnPoint.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (characterEntity != null)
            characterEntity.Updated();
    }

    public void FixedUpdate()
    {
        characterEntity.FixedUpdated();
    }
}

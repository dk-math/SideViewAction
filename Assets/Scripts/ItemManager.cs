using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    GameManager gameManager;

    private void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

   public void GetItem() {
       gameManager.AddScore(1);
       Destroy(this.gameObject);
    }
}

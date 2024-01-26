using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemCollection : MonoBehaviour
{
   private int apple;
   [SerializeField] private Text AppleText;
   [SerializeField] private AudioSource collectionSoundEffect;
   private void OnTriggerEnter2D(Collider2D other)
   {
      //throw new NotImplementedException();

      if (other.gameObject.CompareTag("Apple"))
      {
         collectionSoundEffect.Play();
         Destroy(other.gameObject);
         apple++;
         AppleText.text= "Apple: " + apple; 
      }
   }
   
}

using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class PlayerSelect : NetworkBehaviour
{
   [SerializeField] private GameObject characterSelectDisplay = default;
   [SerializeField] private Transform characterPreviewParent = default;
   [SerializeField] private TMP_Text characterNameText = default;
   [SerializeField] private float turnSpeed;
   [SerializeField] private PlayerBase[] characters = default;

   private int currentCharacterIndex = 0;
   private List<GameObject> characterInstances = new List<GameObject>();

   public override void OnStartClient()
   {
      base.OnStartClient();
      if (characterPreviewParent.childCount == 0)
      {
         foreach (var character in characters)
         {
            GameObject characterInstance =
               Instantiate(character.PreviewPrefab, characterPreviewParent);

            characterInstance.SetActive(false);

            characterInstances.Add(characterInstance); 
         }
      }
      characterInstances[currentCharacterIndex].SetActive(true);
      characterNameText.text = characters[currentCharacterIndex].name;
      
      characterSelectDisplay.SetActive(true);
   }

   private void Update()
   {
      characterPreviewParent.RotateAround(characterPreviewParent.up, turnSpeed * Time.deltaTime);
   }

   public void ConfirmButton()
   {
      CmdConfirmButton(currentCharacterIndex);
      characterSelectDisplay.SetActive(false);
   }

   [Command(requiresAuthority = false)]
   public void CmdConfirmButton(int characterIndex, NetworkConnectionToClient sender = null)
   {
      GameObject characterInstance = Instantiate(characters[characterIndex].GameplayPrefab);
      NetworkServer.Spawn(characterInstance, sender);
   }
   
   public void RightButton()
   {
      characterInstances[currentCharacterIndex].SetActive(false);

      currentCharacterIndex = (currentCharacterIndex + 1) % characterInstances.Count;
      
      characterInstances[currentCharacterIndex].SetActive(true);
      characterNameText.text = characters[currentCharacterIndex].name;
   }

   public void LeftButton()
   {
      characterInstances[currentCharacterIndex].SetActive(false);

      currentCharacterIndex--;
      if (currentCharacterIndex < 0)
      {
         currentCharacterIndex += characterInstances.Count;
      }
      characterInstances[currentCharacterIndex].SetActive(true);
      characterNameText.text = characters[currentCharacterIndex].name;
   }
}

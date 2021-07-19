using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Player", menuName = "Player Selection/Player")]
public class PlayerBase : ScriptableObject
{
   [SerializeField] private string characterName = default;
   [SerializeField] private GameObject previewPrefab = default;
   [SerializeField] private GameObject gameplayPrefab = default;

   public string CharacterName => characterName;
   public GameObject PreviewPrefab => previewPrefab;
   public GameObject GameplayPrefab => gameplayPrefab;

}

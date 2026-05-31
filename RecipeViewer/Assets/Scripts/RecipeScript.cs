using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeScript", menuName = "Recipes functionality")]

public class RecipeScript : ScriptableObject
{



    [SerializeField] private string recipeName = "";
    [SerializeField] private string description = "";
    [SerializeField] private string[] ingredients = new string[4];

    public string RecipeName => recipeName;
    public string Description => description;
    public string[] Ingredients => ingredients;

}


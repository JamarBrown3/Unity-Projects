using UnityEngine;
using TMPro;
public class RecipeManager : MonoBehaviour
{
    public RecipeScript recipeScript;
    [SerializeField] private TMP_Text recipeNameText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private TMP_InputField indexInputField;

    public TMP_Text ingredient1Text;
    public TMP_Text ingredient2Text;
    public TMP_Text ingredient3Text;
    public TMP_Text ingredient4Text;



    public RecipeScript[] recipes; // list for recipes
    void Start()
    {
        ShowRecipe(0);
    }


    public void ShowRecipe(int index)
    {
        // Safety check
        if (index < 0 || index >= recipes.Length)
        {
            Debug.LogError("Index out of range");
            return;
        }

        // Update TMP_Text for the selected recipe
        RecipeScript myRecipe = recipes[index];

        recipeNameText.text = myRecipe.RecipeName;
        descriptionText.text = myRecipe.Description;
        ingredient1Text.text = myRecipe.Ingredients[0];
        ingredient2Text.text = myRecipe.Ingredients[1];
        ingredient3Text.text = myRecipe.Ingredients[2];
        ingredient4Text.text = myRecipe.Ingredients[3];

    }


    public void onIndexChanged()
    {
        int index;
        if (int.TryParse(indexInputField.text, out index))
        {
            if (index >= 0 && index < recipes.Length)
            {
                ShowRecipe(index);
            }
            else
            {
                Debug.LogError("Index out of range");
            }
        }
        else
        {
            Debug.LogError("Invalid input for index");
        }
    }
}

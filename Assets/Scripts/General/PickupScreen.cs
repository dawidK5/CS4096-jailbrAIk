using TMPro;
using UnityEngine;

public class PickupScreen : MonoBehaviour
{
  [SerializeField]
  public TextMeshProUGUI pickupText;
  public Color colour;
  bool startFading;
  void Start()
  {
    colour = pickupText.color;
    colour.a = 0.0f;
    pickupText.color = colour;
  }
  public void Pickup(string itemName)
  {
    pickupText.SetText($"* {itemName} picked up *");
    colour.a = 1.0f;
    pickupText.color = colour;
    startFading = true;
  }
  void Update()
  {
    if (startFading)
    {
      if (colour.a > 0.0f)
      {
        colour.a += Time.deltaTime;
        pickupText.color = colour;

      }
      else 
      {
        colour.a = 0.0f;
        startFading = false;
      }
    
    }
  }
}

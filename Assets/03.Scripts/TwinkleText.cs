using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TwinkleText : MonoBehaviour
{
    private TextMeshProUGUI myTextMeshPro;
    private bool isFading = false;
    // Start is called before the first frame update
    void Awake()
    {
        myTextMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myTextMeshPro)
        {
            if (myTextMeshPro.alpha >= 0.7f)
            {
                isFading = true;
            }else if (myTextMeshPro.alpha <= 0.3f)
            {
                isFading = false;
            }

            myTextMeshPro.alpha = Mathf.Lerp(myTextMeshPro.alpha, isFading ? 0f : 1f, Time.deltaTime);
        }
    }
}

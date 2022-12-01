using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Assertions;

public class HelpMarkerController : Interactable
{
    [TextArea]
    [SerializeField]
    private string defaultText;

    [TextArea]
    [SerializeField]
    private string gamepadText;

    [SerializeField]
    private SpriteRenderer signSprite;

    [SerializeField]
    private SpriteRenderer spriteLight;

    [SerializeField]
    private InteractMarkerManager interactMarker;

    [SerializeField]
    private Light2D pointLight;

    [SerializeField]
    private Vector2 signDimension = Vector2.zero;

    [SerializeField]
    private float colorTimeScale = 0.25f;

    private bool messageShown = false;

    private Color currColor;
    private float currHue;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(signSprite);
        Assert.IsNotNull(spriteLight);
        Assert.IsNotNull(pointLight);
        Assert.IsTrue(signDimension != Vector2.zero);

        currColor = Color.red;
        currHue = 0f;

        signSprite.size = new(signDimension.x, 0f);
    }

    public override void Interact()
    {
        if (!messageShown)
        {
            messageShown = true;
            interactMarker.Hide();

            if (GameSystem.Inst.Control == ControlMode.Keyboard || gamepadText.Length == 0)
            {
                UISystem.Inst.ShowMessage(defaultText);
            }
            else
            {
                UISystem.Inst.ShowMessage(gamepadText);
            }
        }
        else
        {
            messageShown = false;
            interactMarker.Show();
            UISystem.Inst.RemoveMessage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        currHue = (currHue + Time.deltaTime * colorTimeScale) % 1f;
        currColor = Color.HSVToRGB(currHue, 0.25f, 1f);
        spriteLight.color = currColor;
        pointLight.color = currColor;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.HasTag("Player"))
        {
            interactMarker.Show();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.HasTag("Player"))
        {
            interactMarker.Hide();

            if (messageShown)
            {
                UISystem.Inst.RemoveMessage();
                messageShown = false;
            }
        }
    }
}

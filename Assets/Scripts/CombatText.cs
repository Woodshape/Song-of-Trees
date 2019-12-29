using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    public string Text;
    public int Damage;

    public bool IsPlayerDamaged;

    public Transform MainTarget;

    public float FadeTime;
    public float YPos;

    public int YSpeed;

    public GUIStyle TextStyle = new GUIStyle();

    public float NewAlpha;

    public float FadeOutTimer;

    private Camera mainCamera;
    private Vector2 targetPos;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Start is called before the first frame update
    void Start()
    {
        Text = "";
        mainCamera = Camera.main;
        targetPos = mainCamera.WorldToScreenPoint(MainTarget.position);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void OnGUI()
    {
        if (MainTarget == null)
            return;

        if (IsPlayerDamaged)
        {
            TextStyle.normal.textColor = new Color(0.75f, 0, 0);
        }

        GUI.Label(new Rect(targetPos.x, Screen.height - targetPos.y - YPos, 120, 30), "" + FormatK(Damage), TextStyle);

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Update is called once per frame
    void Update()
    {
        if (MainTarget == null)
            return;

        Vector2 target = mainCamera.WorldToScreenPoint(MainTarget.position);
        targetPos = target;

        FadeOut();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    int FormatDamageNumbers(int dmg)
    {
        int holder = dmg;
        int kilo = (dmg % 1000);
        int mega = (dmg / 1000) % 1000;

        return holder;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void FadeOut()
    {
        if (FadeTime > 0)
        {
            if (FadeOutTimer > 0)
            {
                FadeOutTimer -= Time.deltaTime;
            }
            else
            {
                NewAlpha -= 0.05f;

                TextStyle.normal.textColor = new Color(0, 0, 0, NewAlpha);
            }

            FadeTime -= Time.deltaTime;

            for (int i = 0; i < YSpeed; i++)
            {
                YPos += 1.0f;
            }

            if (TextStyle.fontSize < 20)
            {
                TextStyle.fontSize += 1;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private static readonly SortedDictionary<long, string> abbrevations = new SortedDictionary<long, string>
     {
         {1000,"K"},
         {1000000, "M" },
         {1000000000, "B" },
         {1000000000000,"T"}
     };

    public static string FormatK(float number)
    {
        for (int i = abbrevations.Count - 1; i >= 0; i--)
        {
            KeyValuePair<long, string> pair = abbrevations.ElementAt(i);
            if (Mathf.Abs(number) >= pair.Key)
            {

                float rest = number % pair.Key;
                float k = (number - rest) / pair.Key;
                float f = Mathf.Round(rest / (pair.Key / 10));
                string roundedNumber;
                if (f == 0)
                {
                    roundedNumber = k.ToString();
                }
                else
                {
                    if (f == 10)
                    {
                        f = 9;
                    }
                    roundedNumber = k.ToString() + "." + f.ToString();
                }

                return roundedNumber + pair.Value;
            }
        }
        return number.ToString();
    }
}

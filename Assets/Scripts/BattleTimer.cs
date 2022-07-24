using System.Globalization;
using TMPro;
using UnityEngine;

public class BattleTimer : MonoBehaviour
{
    private float timerValue;
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timerValue += Time.deltaTime;

        text.SetText(timerValue.ToString("00.00", CultureInfo.InvariantCulture));
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniStorm;

public class WeatherDirector : MonoBehaviour
{
    [Header("Базовая цикличность")]
    public int reevaluateEveryGameMinutes = 90;
    [Range(0f,1f)] public float stormChance = 0.12f;
    public int stormDurationGameMinutes = 60;

    [Header("Распределение атмосфер")]
    public int weightOvercast = 50;
    public int weightFog = 20;
    public int weightDrizzle = 25;
    public int weightRain = 20;
    public int weightClear = 8;

    [Header("Соответствие типов UniStorm")]
    public WeatherType overcastType;
    public WeatherType fogType;
    public WeatherType drizzleType;
    public WeatherType rainType;
    public WeatherType stormType;
    public WeatherType clearType;   

    private System.Random rng = new();
    private int _lastEvaluatedMinute = -999;
    private UniStormSystem _system;
    private UniStormManager _manager;

    void Awake()
    {
        _system  = UniStormSystem.Instance ?? FindObjectOfType<UniStormSystem>();
        _manager = UniStormManager.Instance ?? FindObjectOfType<UniStormManager>();
    }

    IEnumerator Start()
    {
        // Дожидаемся, пока UniStorm полностью инициализируется
        yield return StartCoroutine(WaitForUniStormReady());

        // Подписка на событие часа (если доступно)
        if (_system != null && _system.OnHourChangeEvent != null)
            _system.OnHourChangeEvent.AddListener(OnHourChanged);

        // Первичный прогон
        EvaluateCycle(force: true);
    }

    IEnumerator WaitForUniStormReady()
    {
        float timeout = 5f; // 5 секунд — обычно хватает
        float t = 0f;

        while ((_system == null || _manager == null) && t < timeout)
        {
            _system  = UniStormSystem.Instance ?? FindObjectOfType<UniStormSystem>();
            _manager = UniStormManager.Instance ?? FindObjectOfType<UniStormManager>();
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        if (_system == null || _manager == null)
        {
            Debug.LogError("[StalkerWeatherDirector] UniStorm не найден в сцене. Убедитесь, что префаб UniStorm присутствует и активен.");
            enabled = false;
        }
    }

    void OnHourChanged()
    {
        EvaluateCycle(force: false);
    }

    void EvaluateCycle(bool force)
    {
        if (_system == null || _manager == null) return;

        int curHour = _system.Hour;
        int curMin  = _system.Minute;
        int totalMin = curHour * 60 + curMin;

        if (!force && Mathf.Abs(totalMin - _lastEvaluatedMinute) < reevaluateEveryGameMinutes)
            return;

        _lastEvaluatedMinute = totalMin;

        if (UnityEngine.Random.value < stormChance)
        {
            StartCoroutine(StormBurst());
        }
        else
        {
            var target = PickWeighted();
            // защита от null менеджера
            if (_manager != null)
                _manager.ChangeWeatherWithTransition(target);
        }
    }

    IEnumerator StormBurst()
    {
        if (_manager == null)
            yield break;

        // Мгновенный «выброс»
        _manager.ChangeWeatherInstantly(stormType);

        // Ждём заданные игровые минуты (переведённые в реальные секунды)
        float realSeconds = GameMinutesToRealSeconds(stormDurationGameMinutes);
        yield return new WaitForSeconds(realSeconds);

        // Откат к облачно/туманно
        if (_manager != null)
            _manager.ChangeWeatherWithTransition(UnityEngine.Random.value < 0.5f ? overcastType : fogType);
    }

    WeatherType PickWeighted()
    {
        var bag = new List<WeatherType>(weightOvercast + weightFog + weightDrizzle + weightRain + weightClear);
        for (int i = 0; i < weightOvercast; i++) bag.Add(overcastType);
        for (int i = 0; i < weightFog; i++)      bag.Add(fogType);
        for (int i = 0; i < weightDrizzle; i++)  bag.Add(drizzleType);
        for (int i = 0; i < weightRain; i++)     bag.Add(rainType);
        for (int i = 0; i < weightClear; i++)    bag.Add(clearType);
        return bag.Count == 0 ? overcastType : bag[rng.Next(bag.Count)];
    }

    // Временный конвертер. Лучше хранить точный коэффициент в конфиге.
    float GameMinutesToRealSeconds(int gameMinutes)
    {
        // Если вы знаете точную скорость течения времени — подставьте свой коэффициент.
        // Бейзлайн: 1 игровой час ≈ 5 реальных минут => 1 игровая минута ≈ 5 секунд.
        const float realSecondsPerGameMinute = 5f;
        return gameMinutes * realSecondsPerGameMinute;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TheSTAR.Utility;
using Zenject;

namespace TheSTAR.Data
{
    [Serializable]
    public sealed class DataController
    {
        [SerializeField] private bool lockSaves = false; // когда true, перезапись сохранений заблокирована, файлы сохранений не могут быть изменены

        [Header("Test")]
        [SerializeField] private int testCompleteEvents;

        private readonly Dictionary<DataSectionType, string> DataKeys = new ()
        {
            { DataSectionType.Common, "common_data" },
            { DataSectionType.Settings, "settings_data" },
            { DataSectionType.Currency, "currency_data" },
            { DataSectionType.Level, "level_data" },
            { DataSectionType.InappsData, "inapps_data" },
            { DataSectionType.Notifications, "notifications_data" },
            { DataSectionType.Tutorial, "tutorials_data" },
            { DataSectionType.DailyBonus, "daily_bonus_data" },
        };

        [Inject]
        private void Construct()
        {
            if (clearData) LoadDefault();
            else LoadAll();
        }

        public void Init(bool lockSaves)
        {
            this.lockSaves = lockSaves;
        }

        [ContextMenu("Save")]
        private void ForceSave()
        {
            SaveAll(true);
        }

        public void SaveAll(bool force = false)
        {
            var allSections = EnumUtility.GetValues<DataSectionType>();
            foreach (var section in allSections) Save(section, force);
        }

        public void Save(DataSectionType secion, bool force = false)
        {
            if (!force && lockSaves) return;

            JsonSerializerSettings settings = new() { TypeNameHandling = TypeNameHandling.Objects };
            string jsonString = JsonConvert.SerializeObject(gameData.GetSection(secion), Formatting.Indented, settings);

            PlayerPrefs.SetString(DataKeys[secion], jsonString);

            //Debug.Log($"Save {secion}");
        }

        [ContextMenu("Load")]
        private void LoadAll()
        {
            if (PlayerPrefs.HasKey(DataKeys[DataSectionType.Common]))
            {
                var allSections = EnumUtility.GetValues<DataSectionType>();
                foreach (var section in allSections) LoadSection(section);
            }
            else LoadDefault();

            void LoadSection(DataSectionType section)
            {
                string jsonString = PlayerPrefs.GetString(DataKeys[section]);
                var loadedData = JsonConvert.DeserializeObject<DataSection>(jsonString, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                gameData.SetSection(loadedData);
            }
        }

        [ContextMenu("ClearData")]
        private void LoadDefault()
        {
            gameData = new();
            SaveAll();
        }

        [Header("Data")]
        public GameData gameData = new();

        [SerializeField] private bool clearData = false;

        [Serializable]
        public class GameData
        {
            public CommonData commonData;
            public SettingsData settingsData;
            public CurrencyData currencyData;
            public LevelData levelData;
            public InappsData inappsData;
            public NotificationData notificationData;
            public TutorialData tutorialData;
            public DailyBonusData dailyBonusData;

            public GameData()
            {
                commonData = new();
                settingsData = new();
                currencyData = new();
                levelData = new();
                inappsData = new();
                notificationData = new();
                tutorialData = new();
                dailyBonusData = new();
            }

            public DataSection GetSection(DataSectionType sectionType)
            {
                switch (sectionType)
                {
                    case DataSectionType.Common: return commonData;
                    case DataSectionType.Settings: return settingsData;
                    case DataSectionType.Currency: return currencyData;
                    case DataSectionType.Level: return levelData;
                    case DataSectionType.InappsData: return inappsData;
                    case DataSectionType.Notifications: return notificationData;
                    case DataSectionType.Tutorial: return tutorialData;
                    case DataSectionType.DailyBonus: return dailyBonusData;
                    default:
                        break;
                }

                return null;
            }
            public void SetSection(DataSection sectionData)
            {
                switch (sectionData.SectionType)
                {
                    case DataSectionType.Common:
                        commonData = (CommonData)sectionData;
                        break;

                    case DataSectionType.Settings:
                        settingsData = (SettingsData)sectionData;
                        break;

                    case DataSectionType.Currency:
                        currencyData = (CurrencyData)sectionData;
                        break;

                    case DataSectionType.Level:
                        levelData = (LevelData)sectionData;
                        break;

                    case DataSectionType.InappsData:
                        inappsData = (InappsData)sectionData;
                        break;

                    case DataSectionType.Notifications:
                        notificationData = (NotificationData)sectionData;
                        break;

                    case DataSectionType.Tutorial:
                        tutorialData = (TutorialData)sectionData;
                        break;

                    case DataSectionType.DailyBonus:
                        dailyBonusData = (DailyBonusData)sectionData;
                        break;
                }
            }
        }

        [Serializable]
        public abstract class DataSection
        {
            public abstract DataSectionType SectionType { get; }
        }

        [Serializable]
        public class CommonData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.Common;

            public bool gdprAccepted;

            // rate us
            public bool gameRated;
            public bool rateUsPlanned;
            public DateTime nextRateUsPlan;

            public bool gameStarted = false;
            public SerializedVector3 playerPosition;
        }

        [Serializable]
        public class SettingsData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.Settings;

            public bool isMusicOn = true;
            public bool isSoundsOn = true;
            public bool isVibrationOn = true;
            public bool isNotificationsOn = true;
        }

        [Serializable]
        public class CurrencyData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.Currency;

            public Dictionary<CurrencyType, int> currencyData;

            public CurrencyData() => currencyData = new();

            public void AddCurrency(CurrencyType currencyType, int count, out int result)
            {
                if (currencyData.ContainsKey(currencyType)) currencyData[currencyType] += count;
                else currencyData.Add(currencyType, count);

                result = currencyData[currencyType];
            }

            public int GetCurrencyCount(CurrencyType currencyType)
            {
                if (currencyData.ContainsKey(currencyType)) return currencyData[currencyType];
                else return 0;
            }
        }

        // Данные по прогрессу игрока в рамках уровня
        [Serializable]
        public class LevelData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.Level;

            public Dictionary<int, bool> collectedCurrencyItems = new();
        }

        [Serializable]
        public class InappsData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.InappsData;
        }

        [Serializable]
        public class NotificationData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.Notifications;

            /// <summary>
            /// Хранит id зарегестрированных нотификаций. Если id равен -1, значит нотификация неактивна (например, она была отменена)
            /// </summary>
            public Dictionary<NotificationType, int> registredNotifications;

            public void ClearNotification(NotificationType notificationType) => RegisterNotification(notificationType, -1);

            public void RegisterNotification(NotificationType notificationType, int id)
            {
                if (registredNotifications == null) registredNotifications = new Dictionary<NotificationType, int>();

                if (registredNotifications.ContainsKey(notificationType)) registredNotifications[notificationType] = id;
                else registredNotifications.Add(notificationType, id);
            }

            /// <summary>
            /// Возвращает id зарегестрированной нотификации. Если нотификация не зарегестрирована, возвращает -1
            /// </summary>
            public int GetRegistredNotificationID(NotificationType notificationType)
            {
                if (registredNotifications == null)
                {
                    registredNotifications = new Dictionary<NotificationType, int>();
                    return -1;
                }

                if (registredNotifications.ContainsKey(notificationType)) return registredNotifications[notificationType];
                else return -1;
            }
        }

        [Serializable]
        public class TutorialData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.Tutorial;

            public List<string> completedTutorials;

            public void CompleteTutorial(string id)
            {
                if (!completedTutorials.Contains(id)) completedTutorials.Add(id);
            }

            public void UncompleteTutorial(string id)
            {
                if (completedTutorials.Contains(id)) completedTutorials.Remove(id);
            }
        }

        [Serializable]
        public class DailyBonusData : DataSection
        {
            public override DataSectionType SectionType => DataSectionType.DailyBonus;

            public DateTime previousDailyBonusTime;
            public int currentDailyBonusIndex = -1;
            public bool bonusIndexWasUpdatedForThisDay;
        }
    }

    public enum DataSectionType
    {
        Common,
        Settings,
        Currency,
        Level,
        InappsData,
        Notifications,
        Tutorial,
        DailyBonus
    }
}

[Serializable]
public struct SerializedVector3
{
    public float x;
    public float y;
    public float z;

    public static implicit operator Vector3(SerializedVector3 value)
    {
        return new (value.x, value.y, value.z);
    }
    
    public static implicit operator SerializedVector3(Vector3 value)
    {
        return new SerializedVector3 
        {
            x = value.x,
            y = value.y, 
            z = value.z 
        };  
    }
}
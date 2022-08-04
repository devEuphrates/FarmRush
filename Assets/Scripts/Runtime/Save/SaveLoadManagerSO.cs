using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Euphrates
{
    [CreateAssetMenu(fileName = "New Save And Load Manager", menuName = "Save And Load/Manager")]
    public class SaveLoadManagerSO : ScriptableObject
    {
        [SerializeField] ItemHolderSO _items;
        [SerializeField] PlayerUnlocksSO _unlocks;
        [SerializeField] IntSO _cash;

        [SerializeField] SaveData _defaultValues;

        public event Action OnSave;

        public async void Save()
        {
            SaveData data = new SaveData
            {
                Cash = _cash.Value
            };

            int[] unlockedItemIds = new int[_unlocks.UnlockedItems.Count];

            for (int i = 0; i < _unlocks.UnlockedItems.Count; i++)
                unlockedItemIds[i] = _unlocks.UnlockedItems[i].ItemID;

            data.UnlockedItems = unlockedItemIds;

            List<ObjectSaveData> objDatas = new List<ObjectSaveData>();
            SaveState[] savers = Resources.FindObjectsOfTypeAll<SaveState>();

            if (savers != null || savers.Length != 0)
            {
                for (int i = 0; i < savers.Length; i++)
                {
                    if (i % 100 == 0)
                        await Task.Yield();

                    if (savers[i] == null)
                        continue;

                    ObjectSaveData odata = new ObjectSaveData() { UName = savers[i].gameObject.GetNamePath().ToLower() };

                    if (savers[i].SaveActiveState)
                        odata.IsActive = savers[i].gameObject.activeSelf;

                    if (savers[i].SaveTransform)
                    {
                        odata.Position = savers[i].transform.position;
                        odata.Rotation = savers[i].transform.rotation;
                        odata.Scale = savers[i].transform.localScale;
                    }

                    objDatas.Add(odata);
                }

                data.ObjectDatas = objDatas.ToArray();
            }
            else
            {
                data.ObjectDatas = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveData")).ObjectDatas;
            }

            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("SaveData", json);
            OnSave?.Invoke();
        }

        public void Load()
        {
            try
            {
                SaveData save;

                if (!PlayerPrefs.HasKey("SaveData"))
                    save = _defaultValues;
                else
                {
                    string json = PlayerPrefs.GetString("SaveData");
                    save = JsonUtility.FromJson<SaveData>(json);
                }

                _cash.Value = save.Cash;
                _unlocks.UnlockedItems.Clear();

                for (int i = 0; i < save.UnlockedItems.Length; i++)
                {
                    _unlocks.UnlockedItems.Add(_items.GetItem(save.UnlockedItems[i]));
                }

                SaveState[] savers = Resources.FindObjectsOfTypeAll<SaveState>();
                for (int i = 0; i < savers.Length; i++)
                {
                    SaveState ss = savers[i];
                    if (ss == null)
                        continue;

                    for (int j = 0; j < save.ObjectDatas.Length; j++)
                    {
                        if (save.ObjectDatas[j].UName != ss.gameObject.GetNamePath().ToLower())
                            continue;

                        if (ss.SaveActiveState)
                            ss.gameObject.SetActive(save.ObjectDatas[j].IsActive);

                        if (ss.SaveTransform)
                        {
                            ss.transform.SetPositionAndRotation(save.ObjectDatas[j].Position, save.ObjectDatas[j].Rotation);
                            ss.transform.localScale = save.ObjectDatas[j].Scale;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

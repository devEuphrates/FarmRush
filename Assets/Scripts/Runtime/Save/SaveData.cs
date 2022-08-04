namespace Euphrates
{
    [System.Serializable]
    public struct SaveData
    {
        public int Cash;
        public int[] UnlockedItems;
        public ObjectSaveData[] ObjectDatas;
    }
}

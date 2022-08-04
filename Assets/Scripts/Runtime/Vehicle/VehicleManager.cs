using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
    public class VehicleManager : Singleton<VehicleManager>
    {
        [SerializeField] Stacker _moneyStack;

        [Header("Points")]
        [SerializeField] Transform _spawnPoint;
        [SerializeField] Transform _despawnPoint;
        [SerializeField] List<ParkSpot> _parkSpots = new List<ParkSpot>();

        [Space]
        [SerializeField] List<Truck> _trucks = new List<Truck>();

        List<Truck> _unOccupiedTrucks;

        void Start()
        {
            foreach (var truck in _trucks)
                truck.SetMoneyStack(_moneyStack);

            _unOccupiedTrucks = new List<Truck>(_trucks);

            if (_unOccupiedTrucks.Count == 0 || !_parkSpots.Exists(p => !p.Occupied))
            {
                Debug.LogError("vehicle Manager can't be initialized! No available truck or parking spot.");
                return;
            }

            CheckSpots();
        }

        void CheckSpots()
        {
            foreach (var spot in _parkSpots)
            {
                if (spot.Occupied)
                    continue;

                if (!SpawnTruck(spot))
                    return;
            }
        }

        bool SpawnTruck(ParkSpot spot)
        {
            if (_unOccupiedTrucks.Count == 0)
                return false;

            spot.Occupied = true;

            Truck truck = _unOccupiedTrucks.GetRandomItem();
            _unOccupiedTrucks.RemoveAll(p => p == truck);

            void OnOrderComplete()
            {
                spot.Occupied = false;
                SpawnTruck(spot);
                truck.MoveTo(_despawnPoint.position, OnComplete);
            }

            void OnComplete()
            {
                truck.transform.position = _spawnPoint.position;
                _unOccupiedTrucks.Add(truck);
                truck.ClearTruck();
                CheckSpots();
            }

            truck.MoveAndOrder(spot.Point.position, OnOrderComplete);
            return true;
        }
    }

    [System.Serializable]
    class ParkSpot
    {
        public Transform Point;
        public bool Occupied;
    }
}

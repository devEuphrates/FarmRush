using System;
using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
    public class MultiCrafter : MonoBehaviour
    {
        [SerializeField] List<RecipeSO> _recipes;
        RecipeSO _currentRecipe;

        [SerializeField] Transform _productSpawnPoint;

        [Header("Stacks"), Space]
        [SerializeField] Stacker _productStack;
        [SerializeField] List<Stacker> _resourceStacks;

        [Header("Animation"), Space]
        [SerializeField] Animator _animator;
        [SerializeField] string _animTriggerName;

        StationState _currentState = StationState.Idle;

        public event Action onStateChange;
        public StationState CurrentState { get { return _currentState; } }

        void OnEnable()
        {
            foreach (var stck in _resourceStacks)
            {
                stck.onItemAdded += CheckResources;
                stck.onItemRemoved += CheckResources;
            }

            _productStack.onItemRemoved += OnProductChange;
        }

        void OnDisable()
        {
            foreach (var stck in _resourceStacks)
            {
                stck.onItemAdded -= CheckResources;
                stck.onItemRemoved -= CheckResources;
            }

            _productStack.onItemRemoved -= OnProductChange;
        }

        float _timePassed;
        void Update()
        {
            if (_currentState != StationState.Producing)
                return;

            if ((_timePassed += Time.deltaTime) < _currentRecipe.Duration)
                return;

            _timePassed = 0f;
            CheckProducts();
        }

        void OnProductChange()
        {
            if (_currentState != StationState.Halted)
                return;

            bool canPopItems = _productStack.CanAddItem();

            if (canPopItems)
                CreateProduct();
        }

        void CheckResources()
        {
            if (_currentState != StationState.Idle)
                return;

            foreach (var rec in _recipes)
            {
                List<RecipeNeed> unsatisfiedNeeds = new List<RecipeNeed>(rec.Needs);
                foreach (var stck in _resourceStacks)
                {
                    for (int i = unsatisfiedNeeds.Count - 1; i >= 0; i--)
                    {
                        int cntInStack = stck.GetItemCount(unsatisfiedNeeds[i].NeededItem);

                        if (cntInStack >= unsatisfiedNeeds[i].NeededAmount)
                            unsatisfiedNeeds.RemoveAt(i);
                    }
                }

                if (unsatisfiedNeeds.Count > 0)
                    continue;


                _currentRecipe = rec;
                StartProduction();
            }
        }

        void CheckProducts()
        {
            bool canPopItems = _productStack.CanAddItem();

            switch (_currentState)
            {
                case StationState.Producing:
                    if (!canPopItems)
                    {
                        ChangeState(StationState.Halted);
                        return;
                    }

                    CreateProduct();
                    return;

                case StationState.Halted:
                    if (!canPopItems)
                        return;

                    CreateProduct();
                    return;

                default:
                    return;
            }

        }

        void StartProduction()
        {
            ChangeState(StationState.Producing);
            AnimState();

            List<RecipeNeed> unsatisfiedNeeds = new List<RecipeNeed>(_currentRecipe.Needs);
            foreach (var stck in _resourceStacks)
            {
                if (unsatisfiedNeeds.Count == 0)
                    break;

                for (int i = unsatisfiedNeeds.Count - 1; i >= 0; i--)
                {
                    int cntInStack = stck.GetItemCount(unsatisfiedNeeds[i].NeededItem);

                    if (cntInStack < unsatisfiedNeeds[i].NeededAmount)
                        continue;

                    for (int j = 0; j < unsatisfiedNeeds[i].NeededAmount; j++)
                    {
                        Item itm = stck.RemoveItem(unsatisfiedNeeds[i].NeededItem);
                        itm.FloatToPos(.5f, Vector3.zero, Quaternion.identity, 0f, () => SpawnManager.ReleaseItem(itm), transform);
                    }

                    unsatisfiedNeeds.RemoveAt(i);
                }
            }
        }

        void CreateProduct()
        {
            if (_currentState != StationState.Producing)
                return;

            ChangeState(StationState.Idle);

            Item itm = SpawnManager.SpawnItem(_currentRecipe.Product.ItemID);
            itm.transform.SetPositionAndRotation(_productSpawnPoint.position, _productSpawnPoint.rotation);
            _productStack.AddItem(itm);

            CheckResources();
        }

        void ChangeState(StationState state)
        {
            _currentState = state;
            onStateChange?.Invoke();
        }

        void AnimState(bool play = true)
        {
            if (_animator == null || string.IsNullOrWhiteSpace(_animTriggerName))
                return;

            _animator.SetTrigger(_animTriggerName);
        }
    }
}
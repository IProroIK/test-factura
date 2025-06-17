using System;
using System.Collections.Generic;
using Core.Items;

namespace Core.Service
{
    public class UIService : IUIService
    {
        private readonly Dictionary<Type, IUIElement> _elements = new();

        public void Register<T>(T element) where T : IUIElement
        {
            var type = typeof(T);
            if (_elements.ContainsKey(type))
                throw new Exception($"UI Element {type} already registered");

            element.Init();
            _elements[type] = element;
        }

        public void Show<T>(object data = null) where T : IUIElement
        {
            if (_elements.TryGetValue(typeof(T), out var element))
            {
                element.Show(data);
            }
        }

        public void Hide<T>() where T : IUIElement
        {
            if (_elements.TryGetValue(typeof(T), out var element))
            {
                element.Hide();
            }
        }

        public void HideAll()
        {
            foreach (var element in _elements.Values)
            {
                element.Hide();
            }
        }
    }
}
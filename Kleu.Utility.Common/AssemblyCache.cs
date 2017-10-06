using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
// ReSharper disable CollectionNeverQueried.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Kleu.Utility.Common
{
    public sealed class AssemblyCache : IList<Assembly>
    {
        private readonly object _lock = new object();
        private readonly List<Assembly> _assemblies = new List<Assembly>();
        public List<Type> Types { get; } = new List<Type>();

        public AssemblyCache(params string[] assemblyFiles)
        {
            Parallel.ForEach(assemblyFiles, file =>
            {
                var assemblyName = AssemblyName.GetAssemblyName(file);
                var loadedAssembly = Assembly.Load(assemblyName);
                var types = loadedAssembly.ExportedTypes;
                lock (_lock)
                {
                    _assemblies.Add(loadedAssembly);
                    Types.AddRange(types);
                }
            });
        }

        public IEnumerator<Assembly> GetEnumerator()
        {
            return _assemblies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_assemblies).GetEnumerator();
        }

        public void Add(Assembly item)
        {
            _assemblies.Add(item);
        }

        public void Clear()
        {
            _assemblies.Clear();
        }

        public bool Contains(Assembly item)
        {
            return _assemblies.Contains(item);
        }

        public void CopyTo(Assembly[] array, int arrayIndex)
        {
            _assemblies.CopyTo(array, arrayIndex);
        }

        public bool Remove(Assembly item)
        {
            return _assemblies.Remove(item);
        }

        public int Count => _assemblies.Count;

        public bool IsReadOnly => ((IList<Assembly>)_assemblies).IsReadOnly;

        public int IndexOf(Assembly item)
        {
            return _assemblies.IndexOf(item);
        }

        public void Insert(int index, Assembly item)
        {
            _assemblies.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _assemblies.RemoveAt(index);
        }

        public Assembly this[int index]
        {
            get => _assemblies[index];
            set => _assemblies[index] = value;
        }
    }
}

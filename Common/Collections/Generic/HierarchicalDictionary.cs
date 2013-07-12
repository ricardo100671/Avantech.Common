
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Avantech.Common.Collections.Generic
{
    /// <summary>
    /// A class for fast access and modification to hierachical items.
    /// </summary>
    /// <typeparam name="T">Any object implementing IHierarchicalDictionaryItem</typeparam>
    public class HierarchicalDictionary<T> : IDictionary<string, T> where T : class, IHierarchicalDictionaryItem
    {
        private readonly IDictionary<string, HashSet<string>> _childDictionary;
        private readonly IDictionary<string, T> _dictionary;


        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalDictionary&lt;T&gt;"/> class.
        /// </summary>
        public HierarchicalDictionary() {
            _dictionary = new Dictionary<string, T>();
            _childDictionary = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// Adds the specified hierarchical dictionary item.
        /// </summary>
        /// <param name="hierarchicalDictionaryItem">The hierarchical dictionary item.</param>
        public void Add(T hierarchicalDictionaryItem)
        {
            Add(hierarchicalDictionaryItem.Key, hierarchicalDictionaryItem);
        }

        /// <summary>
        /// Adds a range of hierarchical dictionary items.
        /// </summary>
        /// <param name="hierarchicalDictionaryItems">The hierarchical dictionary items.</param>
        public void AddRange(IEnumerable<T> hierarchicalDictionaryItems)
        {
            foreach (var hierarchicalDictionaryItem in hierarchicalDictionaryItems)
            {
                Add(hierarchicalDictionaryItem);
            }
        }

        /// <summary>
        /// Gets the root level item key of the initial start item key provided.
        /// </summary>
        /// <param name="startKey">The start key.</param>
        /// <returns></returns>
        public string GetRootLevelKey(string startKey)
        {
            return GetRootLevelItem(this[startKey]).Key;
        }

        /// <summary>
        /// Gets the root level item of the initial start item key provided.
        /// </summary>
        /// <param name="startKey">The start key.</param>
        /// <returns></returns>
        public T GetRootLevelItem(string startKey)
        {
            return GetRootLevelItem(this[startKey]);
        }

        /// <summary>
        /// Gets the root level item of the initial start item provided.
        /// </summary>
        /// <param name="hierarchicalDictionaryItem">The hierarchical dictionary item.</param>
        /// <returns></returns>
        public T GetRootLevelItem(T hierarchicalDictionaryItem)
        {
            var parentItem = GetParent(hierarchicalDictionaryItem);
            if (parentItem != null)
                GetRootLevelItem(parentItem);                

            return hierarchicalDictionaryItem;
        }

        /// <summary>
        /// Gets the parent associated with this element.
        /// </summary>
        /// <param name="key">The key of the element to find the parent of.</param>
        /// <returns></returns>
        public T GetParent(string key)
        {
            if(ContainsKey(key))
            {
                if(!string.IsNullOrEmpty(this[key].ParentKey) && ContainsKey(this[key].ParentKey))
                    return this[this[key].ParentKey];
            }
            else
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// Gets the parent associated with this element.
        /// </summary>
        /// <param name="hierarchicalDictionaryItem">The hierarchical dictionary item to find the parent of.</param>
        /// <returns></returns>
        public T GetParent(T hierarchicalDictionaryItem)
        {
            return GetParent(hierarchicalDictionaryItem.Key);
        }
        
        /// <summary>
        /// Gets the child elements associated with this key.
        /// </summary>
        /// <param name="key">The key of the parent record.</param>
        /// <returns></returns>
        public IEnumerable<T> GetChildren(string key)
        {
            return _childDictionary.ContainsKey(key) ? this.Select(x => x.Value).Where(x => _childDictionary[key].Contains(x.Key)) : new List<T>();
        }

        /// <summary>
        /// Gets a nonhierarchic list containing the root item and all it's recursively related children
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <returns></returns>
        public IEnumerable<T> GetItemListRecursive(string rootKey)
        {
            //Create a new list and add the root item
            var items = new List<T> {this[rootKey]};

            //Now find it's children and add them to the list
            var childItems = GetChildListRecursive(rootKey);
            if(childItems != null)
                items.AddRange(childItems);

            return items;

        }

        /// <summary>
        /// Gets a nonhierarchic list containing the root item key and all it's recursively related children key's
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <returns></returns>
        public IEnumerable<string> GetKeyListRecursive(string rootKey)
        {
            return GetItemListRecursive(rootKey).Select(x => x.Key);
        }

        /// <summary>
        /// Gets a nonhierarchic list containing recursively related children of the root key supplied.
        /// </summary>
        /// <param name="rootKey">The key of the required record.</param>
        /// <returns></returns>
        public IEnumerable<T> GetChildListRecursive(string rootKey)
        {
            if (_childDictionary.ContainsKey(rootKey))
            {
                var childList = this.Select(x => x.Value).Where(x => _childDictionary[rootKey].Contains(x.Key)).ToList();
                var grandchildList = new List<T>();
                foreach (var child in childList)
                {
                    var newChildren = GetChildListRecursive(child.Key);
                    if (newChildren != null)
                        grandchildList.AddRange(newChildren);
                }
                childList.AddRange(grandchildList);
                return childList;
            }

            return new List<T>();
        }

        /// <summary>
        /// Gets a nonhierarchic list containing recursively related children key's of the root key supplied.
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <returns></returns>
        public IEnumerable<string> GetChildKeysRecursive(string rootKey)
        {
            return GetChildListRecursive(rootKey).Select(x => x.Key);
        }

        /// <summary>
        /// Gets the child elements associated with this element.
        /// </summary>
        /// <param name="hierarchicalDictionaryItem">The hierarchical dictionary item.</param>
        /// <returns></returns>
        public IEnumerable<T> GetChildren(T hierarchicalDictionaryItem)
        {
            return GetChildren(hierarchicalDictionaryItem.Key);
        }

        /// <summary>
        /// Gets the top level items i.e. items where ParentKey is null.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetTopLevelItems()
        {
            return _dictionary.Values.Where(t => string.IsNullOrEmpty(t.ParentKey));
        }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<KeyValuePair<int,T>>

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(KeyValuePair<string, T> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            _dictionary.Clear();
            _childDictionary.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(KeyValuePair<string, T> item)
        {
            return _dictionary.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(KeyValuePair<string, T> item)
        {
            bool result = _dictionary.Remove(item);
            if(result)
                _childDictionary.Remove(item.Key);

            return result;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return _dictionary.Count(); }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return _dictionary.IsReadOnly; }
        }

        #endregion

        #region Implementation of IDictionary<int,T>

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public void Add(string key, T value)
        {
            _dictionary.Add(key, value);
            //Add a child entry if necessary
            if(!string.IsNullOrEmpty(value.ParentKey))
            {
                if(_childDictionary.ContainsKey(value.ParentKey)) // update the current list of children
                    _childDictionary[value.ParentKey].Add(key);
                else
                    _childDictionary.Add(value.ParentKey, new HashSet<string> { key }); //add a new child list entry
            }
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        /// <param name="key">The key of the element to remove.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public bool Remove(string key)
        {
            bool result = _dictionary.Remove(key);
            if(result)
                _childDictionary.Remove(key);

            return result;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key whose value to get.</param><param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool TryGetValue(string key, out T value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        /// The element with the specified key.
        /// </returns>
        /// <param name="key">The key of the element to get or set.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public T this[string key]
        {
            get { return _dictionary[key]; }
            set
            {
                if (_dictionary[key].ParentKey != value.ParentKey) //Some change required in children
                {
                    //find original parent and remove from list of children
                    string originalParentKey = _dictionary[key].ParentKey;
                    if (!string.IsNullOrEmpty(originalParentKey))
                    {
                        _childDictionary[originalParentKey].Remove(value.Key);
                    }
                    //add to children in new parent if necessary
                    if (!string.IsNullOrEmpty(value.ParentKey))
                    {
                        if (_childDictionary.ContainsKey(value.ParentKey)) // update the current list of children
                            _childDictionary[value.ParentKey].Add(key);
                        else
                            _childDictionary.Add(value.ParentKey, new HashSet<string> {key}); //add a new child list entry
                    }
                }
                else
                {
                     _dictionary[key] = value;          
                }

            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public ICollection<string> Keys
        {
            get { return _dictionary.Keys; }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public ICollection<T> Values
        {
            get { return _dictionary.Values; }
        }

        #endregion
    }
}

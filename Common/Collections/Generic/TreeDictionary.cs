namespace MyLibrary.Collections.Generic
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
    /// A class for fast access and modification to hierachical items.
    /// </summary>
    /// <typeparam name="T">Any object implementing ITreeDictionaryItem</typeparam>
    public class TreeDictionary<T> : IDictionary<int, T> where T : class, ITreeDictionaryItem
    {
        private readonly IDictionary<int, HashSet<int>> _childDictionary;
        private readonly IDictionary<int, T> _dictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeDictionary&lt;T&gt;"/> class.
        /// </summary>
        public TreeDictionary()
        {
            _dictionary = new Dictionary<int, T>();
            _childDictionary = new Dictionary<int, HashSet<int>>();
        }

        /// <summary>
        /// Adds the specified tree dictionary item.
        /// </summary>
        /// <param name="treeDictionaryItem">The tree dictionary item.</param>
        public void Add(T treeDictionaryItem)
        {
            Add(treeDictionaryItem.Id, treeDictionaryItem);
        }

        /// <summary>
        /// Adds a range of tree dictionary items.
        /// </summary>
        /// <param name="treeDictionaryItems">The tree dictionary items.</param>
        public void AddRange(IEnumerable<T> treeDictionaryItems)
        {
            foreach (var treeDictionaryItem in treeDictionaryItems)
            {
                Add(treeDictionaryItem);
            }
        }

        /// <summary>
        /// Gets the root level item id of the initial start item id provided.
        /// </summary>
        /// <param name="startId">The start id.</param>
        /// <returns></returns>
        public int GetRootLevelId(int startId)
        {
            return GetRootLevelItem(this[startId]).Id;
        }

        /// <summary>
        /// Gets the root level item of the initial start item id provided.
        /// </summary>
        /// <param name="startId">The start id.</param>
        /// <returns></returns>
        public T GetRootLevelItem(int startId)
        {
            return GetRootLevelItem(this[startId]);
        }

        /// <summary>
        /// Gets the root level item of the initial start item provided.
        /// </summary>
        /// <param name="treeDictionaryItem">The tree dictionary item.</param>
        /// <returns></returns>
        public T GetRootLevelItem(T treeDictionaryItem)
        {
            var parentItem = GetParent(treeDictionaryItem);
            if (parentItem != null)
                GetRootLevelItem(parentItem);                

            return treeDictionaryItem;
        }

        /// <summary>
        /// Gets the parent associated with this element.
        /// </summary>
        /// <param name="id">The id of the element to find the parent of.</param>
        /// <returns></returns>
        public T GetParent(int id)
        {
            if(ContainsKey(id))
            {
                if(this[id].ParentId.HasValue && ContainsKey(this[id].ParentId.Value))
                    return this[this[id].ParentId.Value];
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
        /// <param name="treeDictionaryItem">The tree dictionary item to find the parent of.</param>
        /// <returns></returns>
        public T GetParent(T treeDictionaryItem)
        {
            return GetParent(treeDictionaryItem.Id);
        }
        
        /// <summary>
        /// Gets the child elements associated with this id.
        /// </summary>
        /// <param name="id">The id of the parent record.</param>
        /// <returns></returns>
        public IEnumerable<T> GetChildren(int id)
        {
            if (_childDictionary.ContainsKey(id))
                return this.Select(x => x.Value).Where(x => _childDictionary[id].Contains(x.Id));
            else
                return new List<T>();
        }

        /// <summary>
        /// Gets a nonhierarchic list containing the root item and all it's recursively related children
        /// </summary>
        /// <param name="rootId">The root id.</param>
        /// <returns></returns>
        public IEnumerable<T> GetItemListRecursive(int rootId)
        {
            //Create a new list and add the root item
            var items = new List<T> {this[rootId]};

            //Now find it's children and add them to the list
            var childItems = GetChildListRecursive(rootId);
            if(childItems != null)
                items.AddRange(childItems);

            return items;

        }

        /// <summary>
        /// Gets a nonhierarchic list containing the root item id and all it's recursively related children id's
        /// </summary>
        /// <param name="rootId">The root id.</param>
        /// <returns></returns>
        public IEnumerable<int> GetIdListRecursive(int rootId)
        {
            return GetItemListRecursive(rootId).Select(x => x.Id);
        }

        /// <summary>
        /// Gets a nonhierarchic list containing recursively related children of the root id supplied.
        /// </summary>
        /// <param name="rootId">The id of the required record.</param>
        /// <returns></returns>
        public IEnumerable<T> GetChildListRecursive(int rootId)
        {
            if (_childDictionary.ContainsKey(rootId))
            {
                var childList = this.Select(x => x.Value).Where(x => _childDictionary[rootId].Contains(x.Id)).ToList();
                var grandchildList = new List<T>();
                foreach (var child in childList)
                {
                    var newChildren = GetChildListRecursive(child.Id);
                    if (newChildren != null)
                        grandchildList.AddRange(newChildren);
                }
                childList.AddRange(grandchildList);
                return childList;
            }
            else
            {
                return new List<T>();
            }
        }

        /// <summary>
        /// Gets a nonhierarchic list containing recursively related children id's of the root id supplied.
        /// </summary>
        /// <param name="rootId">The root id.</param>
        /// <returns></returns>
        public IEnumerable<int> GetChildIdsRecursive(int rootId)
        {
            return GetChildListRecursive(rootId).Select(x => x.Id);
        }

        /// <summary>
        /// Gets the child elements associated with this element.
        /// </summary>
        /// <param name="treeDictionaryItem">The tree dictionary item.</param>
        /// <returns></returns>
        public IEnumerable<T> GetChildren(T treeDictionaryItem)
        {
            return GetChildren(treeDictionaryItem.Id);
        }

        /// <summary>
        /// Gets the top level items i.e. items where ParentId is null.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetTopLevelItems()
        {
            return _dictionary.Values.Where(t => t.ParentId.HasValue == false);
        }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
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
        public void Add(KeyValuePair<int, T> item)
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
        public bool Contains(KeyValuePair<int, T> item)
        {
            return _dictionary.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. 
        /// The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
         public void CopyTo(KeyValuePair<int, T>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Remove(KeyValuePair<int, T> item)
        {
            var result = _dictionary.Remove(item);
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
        public bool ContainsKey(int key)
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
        public void Add(int key, T value)
        {
            _dictionary.Add(key, value);
            //Add a child entry if necessary
            if (!value.ParentId.HasValue) return;
            if(_childDictionary.ContainsKey(value.ParentId.Value)) // update the current list of children
                _childDictionary[value.ParentId.Value].Add(key);
            else
                _childDictionary.Add(value.ParentId.Value, new HashSet<int> { key }); //add a new child list entry
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        /// <param name="key">The key of the element to remove.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public bool Remove(int key)
        {
            var result = _dictionary.Remove(key);
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
        public bool TryGetValue(int key, out T value)
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
        public T this[int key]
        {
            get { return _dictionary[key]; }
            set
            {
                if (_dictionary[key].ParentId != value.ParentId) //Some change required in children
                {
                    //find original parent and remove from list of children
                    var originalParentId = _dictionary[key].ParentId;
                    if (originalParentId.HasValue)
                    {
                        _childDictionary[originalParentId.Value].Remove(value.Id);
                    }
                    //add to children in new parent if necessary
                    if (value.ParentId.HasValue)
                    {
                        if (_childDictionary.ContainsKey(value.ParentId.Value)) // update the current list of children
                            _childDictionary[value.ParentId.Value].Add(key);
                        else
                            _childDictionary.Add(value.ParentId.Value, new HashSet<int> {key}); //add a new child list entry
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
        public ICollection<int> Keys
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

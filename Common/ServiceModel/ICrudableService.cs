using System.Collections.Generic;

namespace Avantech.Common.ServiceModel
{
    [ServiceContract]
    public interface ICrudableService<TDto, in TId>
    {
        /// <summary>
        /// Returns a new instance of an item that the service responsible for managing.
        /// </summary>
        /// <returns>
        /// A new instance for type parameter 'TId'.
        /// </returns>
        [OperationContract]
        TDto Create();

        /// <summary>
        /// Retrieves an instance of an item that the service responsible for managing.
        /// </summary>
        /// <param name="id">The id of the tem to be retrieved.</param>
        /// <param name="forUpdate">Indicates that an entity retrieved will be updated. 
        /// This is usefull to assertian whether to include additional data 
        /// such indexes of related entities that may be updated.
        /// </param>
        /// <returns>
        /// If the value of parameter 'id' is the default value for type parameter 'TId', e.g. if 'TId' is 'int' and value of 'id' parameter is '0', then a new instance is returned,
        /// otherwise an existing instance is retrieved buy the specified 'id'.
        /// </returns>
        [OperationContract]
        TDto Retrieve(TId id, bool forUpdate = false);

        /// <summary>
        /// Deletes an item.
        /// </summary>
        /// <param name="id">The id of the item to be deleted.</param>
        [OperationContract]
        void Delete(TId id);

        /// <summary>
        /// Adds an a new item.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        [OperationContract]
        TDto Add(TDto dto);

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        /// <param name="dto">The item to be updated.</param>
        [OperationContract]
        TDto Update(TDto dto);

        /// <summary>
        /// Retrieves an enumeration of objects containing basic information of all existing item.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<TDto> SummariseAll();
    }
}

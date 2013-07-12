using System.Web.Mvc;

namespace Avantech.Common.Web.Mvc
{
    public interface ICrudableController<in TModel, in TId>
    {
        /// <summary>
        /// Returns a view containing a summary list, allowing a user to nivigate to individual items.
        /// </summary>
        /// <returns></returns>
        ViewResult Index();

        /// <summary>
        /// Returns a view that displays full details of individual items.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        ViewResult Details(TId id);

        /// <summary>
        /// Returns a view that allows a user to enter details for a new item.
        /// </summary>
        /// <returns></returns>
        ViewResult Add();

        /// <summary>
        /// Processes the details of a newly added item.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        ActionResult Add(TModel model);

        /// <summary>
        /// Returns a view that allows a user to edit details for an existing item.
        /// </summary>
        /// <returns></returns>
        ViewResult Edit(TId id);

        /// <summary>
        /// Processes the details of an edited item.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        ActionResult Edit(TModel model);

        /// <summary>
        /// Returns a view diplaying a confirmation page for deleting an individual item.
        /// </summary>
        /// <param name="id">The id of the item to be deleted.</param>
        /// <param name="name">A name for the item to be deleted.
        /// Used by the returned view to construct the message describing the item that will be deleted.</param>
        /// <returns></returns>
        ViewResult Delete(TId id, string name);

        /// <summary>
        /// Processes the delete request.
        /// </summary>
        /// <param name="id">The id for the item to be deleted.</param>
        /// <param name="delete">Result of the delete form action 
        ///		i.e. Whether the user confirmed the delete operation or not.
        /// </param>
        /// <returns></returns>
        RedirectToRouteResult Delete(TId id, bool delete);
    }
}

using Avantech.Common.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Avantech.Common.Web.Mvc.Html
{
    public static class HubTreeHtmlHelperExtensions
    {
        public static IHtmlString HubTree(this HtmlHelper htmlHelper, TreeDictionary<TreeDictionaryItem> treeDictionary, string controlName, int rootId, bool enableCheckbox = false, bool enableIcons = false, bool showRoot = true)
        {
            var rootItem = treeDictionary[rootId];
            var initialChildren = treeDictionary.GetChildren(rootId).OrderBy(t => t.DisplaySequence).ThenBy(t => t.Name);

            var htmlOutput = string.Empty;
            htmlOutput += "<div id=\"" + controlName + "\">";

            if (showRoot)
            {
                htmlOutput += "<ul><li id=\"TreeLi" + rootItem.Id + "\" rel=\"root" + "\" data-item-id=\"" + rootItem.Id +
                              "\">";
                htmlOutput += "<a id=\"TreeAnchor" + rootItem.Id + "\" data-item-id=\"" + rootItem.Id + "\" title=\"" + rootItem.Title +
                              "\" data-entity-id=\"" + rootItem.EntityId + "\" data-entity-type=\"" + rootItem.EntityType + "\" href=\"#\" >" + rootItem.Name + "</a>";
            }

            foreach (var treeDictionaryItem in initialChildren)
            {
                htmlOutput += "<ul><li id=\"TreeLi" + treeDictionaryItem.Id + "\" data-item-id=\"" +
                              treeDictionaryItem.Id + "\""
                              + "data-entity-id=\"" + treeDictionaryItem.EntityId + "\" data-entity-type=\"" +
                              treeDictionaryItem.EntityType;
                if (enableIcons)
                {
                    htmlOutput += "\" data-entity-icontype=\"" + treeDictionaryItem.IconType + "\">";
                    htmlOutput += "<span id=\"TreeIcon" + treeDictionaryItem.Id + "\" class=\"" + treeDictionaryItem.IconType + "\"></span>";
                }
                else
                    htmlOutput += "\">";

                if (enableCheckbox)
                {
                    htmlOutput += "<input type=\"checkbox\" id=\"TreeCheckBox" + treeDictionaryItem.Id + "\" value=\"" + treeDictionaryItem.Id + "\">";
                }

                htmlOutput += "<a id=\"TreeAnchor" + treeDictionaryItem.Id + "\" data-item-id=\"" + treeDictionaryItem.Id + "\" title=\"" + treeDictionaryItem.Title + "\" href=\"#\" >" + treeDictionaryItem.Name + "</a>";
                htmlOutput += HubTreeAddChildren(treeDictionary, treeDictionaryItem.Id, enableCheckbox, enableIcons);
                htmlOutput += "</li></ul>";
            }

            if (showRoot)
            {
                htmlOutput += "</li></ul>";
                htmlOutput += "</div>";
            }
            return MvcHtmlString.Create(htmlOutput);
        }

        private static string HubTreeAddChildren(TreeDictionary<TreeDictionaryItem> treeDictionary, int rootId, bool enableCheckbox, bool enableIcons)
        {
            var htmlOutput = string.Empty;

            var children = treeDictionary.GetChildren(rootId).OrderBy(t => t.DisplaySequence).ThenBy(t => t.Name);
            if (children.Any())
            {
                htmlOutput += "<ul>";

                foreach (var treeDictionaryItem in children)
                {
                    htmlOutput += "<li id=\"TreeLi" + treeDictionaryItem.Id + "\" data-item-id=\"" + treeDictionaryItem.Id + "\""
                        + " data-entity-id=\"" + treeDictionaryItem.EntityId + "\" data-entity-type=\"" + treeDictionaryItem.EntityType;
                    if (enableIcons)
                    {
                        htmlOutput += "\" data-entity-icontype=\"" + treeDictionaryItem.IconType + "\">";
                        htmlOutput += "<span id=\"TreeIcon" + treeDictionaryItem.Id + "\" class=\"" + treeDictionaryItem.IconType + "\"></span>";
                    }
                    else
                        htmlOutput += "\">";

                    if (enableCheckbox)
                    {
                        htmlOutput += "<input type=\"checkbox\" id=\"TreeCheckBox" + treeDictionaryItem.Id + "\" value=\"" + treeDictionaryItem.Id + "\">";
                    }

                    htmlOutput += "<a id=\"TreeAnchor" + treeDictionaryItem.Id + "\" data-item-id=\"" + treeDictionaryItem.Id + "\" title=\"" + treeDictionaryItem.Title + "\" href=\"#\" >" + treeDictionaryItem.Name + "</a>";
                    htmlOutput += HubTreeAddChildren(treeDictionary, treeDictionaryItem.Id, enableCheckbox, enableIcons);
                    htmlOutput += "</li>";
                }
                htmlOutput += "</ul>";
            }

            return htmlOutput;
        }
    }
}
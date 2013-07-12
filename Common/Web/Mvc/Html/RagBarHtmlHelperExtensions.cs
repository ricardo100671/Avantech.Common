
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Avantech.Common.Web.Mvc.Html
{
    public static class HubRagBarHtmlHelperExtensions
    {
        public static IHtmlString HubRagBar(this HtmlHelper htmlHelper, IRagBarItem ragBarItem)
        {
            var ragBarData = CalculatePercentages(ragBarItem);

			var sb = new StringBuilder();
            sb.Append("<div class='ragbar'>");
            if(ragBarData.RedPercent > 0)
				sb.AppendFormat("<span class=\"ragbar-item ragbar-item-red ragbar-item-{0}\">{1}</span>", ragBarData.RedPercent, ragBarData.RedCount);

            if (ragBarData.AmberPercent > 0)
				sb.AppendFormat("<span class=\"ragbar-item ragbar-item-amber ragbar-item-{0}\">{1}</span>", ragBarData.AmberPercent, ragBarData.AmberCount);

            if (ragBarData.GreenPercent > 0)
				sb.AppendFormat("<span class=\"ragbar-item ragbar-item-green ragbar-item-{0}\">{1}</span>", ragBarData.GreenPercent, ragBarData.GreenCount);

            sb.Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        private static RagBarData CalculatePercentages(IRagBarItem ragBarItem)
        {
            var ragBarData = new RagBarData();
            ragBarData.RedCount = ragBarItem.RedCount;
            ragBarData.AmberCount = ragBarItem.AmberCount;
            ragBarData.GreenCount = ragBarItem.GreenCount;

            var sumOfCounts = ragBarItem.RedCount + ragBarItem.AmberCount + ragBarItem.GreenCount;

            if (sumOfCounts == 0)
            {
                //We have no values so show all green
                ragBarData.GreenPercent = 100;
            }
            else if (ragBarItem.RedCount == ragBarItem.AmberCount && ragBarItem.AmberCount == ragBarItem.GreenCount)
            {
                //We have all equal values so evenly distribute
                ragBarData.GreenPercent = 33;
                ragBarData.AmberPercent = 33;
				ragBarData.RedPercent = 33;
            }
            else
            {
                // we need to build up percentages in 10% increments starting from lowest first
                // we will then leave the last value to make up the remaining 100%
                var itemList = new List<RagOrderItem>();
                itemList.Add(new RagOrderItem { ColorMarker = 'R', ItemCount = ragBarItem.RedCount });
                itemList.Add(new RagOrderItem { ColorMarker = 'A', ItemCount = ragBarItem.AmberCount });
                itemList.Add(new RagOrderItem { ColorMarker = 'G', ItemCount = ragBarItem.GreenCount });

                var sortedList = itemList.OrderBy(x => x.ItemCount).ToList();

                sortedList[0].ItemPercent = CalculatePercent(sortedList[0].ItemCount, sumOfCounts, 10);
                sortedList[1].ItemPercent = CalculatePercent(sortedList[1].ItemCount, sumOfCounts, 10);
                sortedList[2].ItemPercent = 100 - sortedList[0].ItemPercent - sortedList[1].ItemPercent;

                ragBarData.GreenPercent = sortedList.Where(x => x.ColorMarker == 'G').Select(x => x.ItemPercent).First();
                ragBarData.AmberPercent = sortedList.Where(x => x.ColorMarker == 'A').Select(x => x.ItemPercent).First();
                ragBarData.RedPercent = sortedList.Where(x => x.ColorMarker == 'R').Select(x => x.ItemPercent).First();
            }

            return ragBarData;
        }

        private static int CalculatePercent(int value, int sumValue, int rounding)
        {
            if (value == 0)
                return 0;

            var doubleValue = (decimal) value;
            var doubleSum = (decimal) sumValue;
            var percentage = doubleValue / doubleSum * 100;

            decimal factor = percentage/rounding;
            return (int) (Math.Ceiling(factor)*rounding);
        }

        private class RagOrderItem
        {
            public char ColorMarker { get; set; }
            public int ItemCount { get; set; }
            public int ItemPercent { get; set; }
        }

        private class RagBarData
        {
            public int RedCount { get; set; }
            public int RedPercent { get; set; }
            public int AmberCount { get; set; }
            public int AmberPercent { get; set; }
            public int GreenCount { get; set; }
            public int GreenPercent { get; set; }
        }
    }
}
using System;

namespace IconCMO
{
	public class IconSort
	{
		
		private IconSortSpec[] _sortSpecs;
		
		public IconSort (IconSortSpec[] sortSpecs)
		{
			_sortSpecs = sortSpecs;
		}
		
		public string GetSortXML()
		{
			string sort = string.Empty;

			sort = "<Sort>";
			foreach (IconSortSpec ss in _sortSpecs)
				sort += "<" + ss.Field + ">" + ss.Order + "</" + ss.Field + ">";
			sort += "</Sort>";
			
			return sort;	
		}
			
	}
	
	public struct IconSortSpec
	{
		public string Field, Order;
		public IconSortSpec(string field, string order)
		{
			Field = field;
			Order = order;
		}
	}
}

